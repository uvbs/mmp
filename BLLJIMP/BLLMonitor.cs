using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 监测BLL
    /// </summary>
    public class BLLMonitor : BLL
    {
        /// <summary>
        /// 分查询分享排名
        /// </summary>
        /// <param name="monitorPlanId">监测任务ID</param>
        /// <param name="orderBy">排序：IP、PV</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataTable QueryMonitorPlanSpreadRank(int monitorPlanId, string orderBy, int pageIndex, int pageSize, out int totalCount)
        {
            DataTable dt = new DataTable();
            totalCount = 0;
            try
            {
                string tmpTableName = "#" + Guid.NewGuid().ToString().Replace("-", "");

                StringBuilder strSql = new StringBuilder();

                strSql.AppendFormat("select b.LinkName,b.RealLink,c.Name,c.WeixinOpenID,count(SourceIP) as PV,count(distinct(SourceIP)) as IP,a.LinkID into {0}  ", tmpTableName);
                strSql.AppendFormat("from  ");
                strSql.AppendFormat("ZCJ_MonitorEventDetailsInfo a left join ZCJ_MonitorLinkInfo b on a.LinkID = b.LinkID  ");
                strSql.AppendFormat("left join dbo.ZCJ_WXMemberInfo c on c.MemberID = b.WXMemberID  ");
                strSql.AppendFormat("where b.MonitorPlanID = {0}  ", monitorPlanId);
                strSql.AppendFormat("group by a.LinkID,b.LinkName,c.Name,c.WeixinOpenID,b.RealLink  ");
                strSql.AppendFormat("order by {0} desc;  ", orderBy);

                strSql.AppendFormat("select top {0} *,(select count(*) from {1}) as totalCount from   ", pageSize, tmpTableName);
                strSql.AppendFormat("(select top {0} row_number() over(order by {1} desc) as COL_ROWNUMBER, * from {2} )   ", pageSize * pageIndex, orderBy, tmpTableName);
                strSql.AppendFormat("TABLE_ORDERDATA  ");
                strSql.AppendFormat("where COL_ROWNUMBER > {0};", (pageIndex - 1) * pageSize);

                strSql.AppendFormat("drop table {0}", tmpTableName);

                dt = ZCDALEngine.DbHelperSQL.Query(strSql.ToString()).Tables[0];

                try
                {
                    if (dt != null)
                    {
                        totalCount = (int)dt.Rows[0]["totalCount"];
                    }
                }
                catch
                {

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// 查询全部转发排名
        /// </summary>
        /// <param name="monitorPlanId"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public DataTable QueryMonitorPlanSpreadRank(int monitorPlanId, string orderBy)
        {
            DataTable dt = new DataTable();

            try
            {
                string tmpTableName = "#" + Guid.NewGuid().ToString().Replace("-", "");

                StringBuilder strSql = new StringBuilder();

                strSql.AppendFormat("select b.LinkName,c.Name,c.WeixinOpenID,count(SourceIP) as PV,count(distinct(SourceIP)) as IP,a.LinkID ");
                strSql.AppendFormat("from  ");
                strSql.AppendFormat("ZCJ_MonitorEventDetailsInfo a left join ZCJ_MonitorLinkInfo b on a.LinkID = b.LinkID  ");
                strSql.AppendFormat("left join dbo.ZCJ_WXMemberInfo c on c.MemberID = b.WXMemberID  ");
                strSql.AppendFormat("where b.MonitorPlanID = {0}  ", monitorPlanId);
                strSql.AppendFormat("group by a.LinkID,b.LinkName,c.Name,c.WeixinOpenID  ");
                strSql.AppendFormat("order by {0} desc;  ", orderBy);

                dt = ZCDALEngine.DbHelperSQL.Query(strSql.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// 更新转发人带来的UV
        /// </summary>
        /// <param name="monitorId"></param>
        /// <param name="spreadUserId"></param>
        /// <returns></returns>
        public bool UpdateUV(int monitorId,string spreadUserId)
        {
            MonitorLinkInfo linkInfo = Get<MonitorLinkInfo>(string.Format("[LinkName]='{0}' And [MonitorPlanID]={1}", spreadUserId, monitorId));
            //int count = GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' AND ArticleId='{1}' AND DistributionOwner='{2}'", WebsiteOwner, linkInfo.ActivityId, linkInfo.LinkName));
            int uv = GetCount<MonitorEventDetailsInfo>("EventUserID",string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND SpreadUserID='{2}'", WebsiteOwner, linkInfo.MonitorPlanID, linkInfo.LinkName));
            linkInfo.UV = uv;
            //linkInfo.PowderCount = count;
            return Update(linkInfo); 
        }

        //public bool UpdateSignUpCount(int monitorsId, string spreadUserId)
        //{
        //    MonitorLinkInfo linkModel = Get<MonitorLinkInfo>(string.Format(" MonitorPlanID={0} AND LinkName='{1}' ", monitorsId, spreadUserId));
        //    int signupCount = GetCount<ActivityDataInfo>(string.Format("MonitorPlanID={0} And SpreadUserID='{1}' And IsDelete=0", monitorsId, spreadUserId));
        //    bool result = false;
        //    if (linkModel != null)
        //    {
        //        linkModel.ActivitySignUpCount = signupCount;
        //        result=Update(linkModel);
        //    }
        //    return result;
        //}
        /// <summary>
        /// 查询昨天、近7天、近30天访问数据
        /// time=day    昨天
        /// time=week   近7天
        /// time=month  近30天
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<MonitorEventDetailsInfo> GetMonitorEventDetailsByTime(int pageSize, int pageIndex, string time, out int totalCount, bool? isUV = false, string userAutoId = "", string userId = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            string nowDay = DateTime.Now.ToString("yyyy-MM-dd");
            switch (time)
            {
                case "day":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-1,'{0}') AND EventDate<'{0}'", nowDay);
                    break;
                case "week":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-7,'{0}') AND EventDate<'{0}' ", nowDay);
                    break;
                case "month":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-30,'{0}') AND EventDate<'{0}' ", nowDay);
                    break;
                default:
                    break;
            }
            if (isUV.HasValue && isUV.Value)
            {
                sbWhere.AppendFormat(" AND EventUserID > '' ");
            }
            if (!string.IsNullOrEmpty(userAutoId))
            {
                sbWhere.AppendFormat(" And EventUserID in(Select UserId from ZCJ_UserInfo where AutoId={0})", userAutoId);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" AND EventUserID='{0}' ", userId);
            }
            totalCount = GetCount<MonitorEventDetailsInfo>(sbWhere.ToString());
            List<MonitorEventDetailsInfo> returnList = GetLit<MonitorEventDetailsInfo>(pageSize, pageIndex, sbWhere.ToString(), " DetailID DESC ");
            return returnList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="time"></param>
        /// <param name="moduleType"></param>
        /// <returns> LinkID 暂时代替排序</returns>
        public List<MonitorEventDetailsInfo> GetMonitorStatisticsList(int Top, string time, string moduleType)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            if (!string.IsNullOrEmpty(moduleType))
            {
                sbWhere.AppendFormat(" AND ModuleType in ('{0}') ", moduleType);
            }
            string nowDay = DateTime.Now.ToString("yyyy-MM-dd");
            switch (time)
            {
                case "day":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-1,'{0}') AND EventDate<'{0}'", nowDay);
                    break;
                case "week":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-7,'{0}') AND EventDate<'{0}' ", nowDay);
                    break;
                case "month":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-30,'{0}') AND EventDate<'{0}' ", nowDay);
                    break;
                default:
                    break;
            }
            string strSql = "SELECT top " + Top + " [MonitorPlanID],COUNT(1) tCount FROM [ZCJ_MonitorEventDetailsInfo] Where " + sbWhere.ToString() + " group by [MonitorPlanID] order by tCount desc";
            return Query<MonitorEventDetailsInfo>(strSql);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <param name="moduleType"></param>
        /// <param name="monitorPlanIDs"></param>
        /// <param name="location"></param>
        /// <returns> LinkID 暂时代替排序</returns>
        public int GetMonitorStatisticsLocationCount(string time, string moduleType, string monitorPlanIDs, string location)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            if (!string.IsNullOrEmpty(moduleType))
            {
                sbWhere.AppendFormat(" AND ModuleType in ('{0}') ", moduleType);
            }
            string nowDay = DateTime.Now.ToString("yyyy-MM-dd");
            switch (time)
            {
                case "day":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-1,'{0}') AND EventDate<'{0}'", nowDay);
                    break;
                case "week":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-7,'{0}') AND EventDate<'{0}' ", nowDay);
                    break;
                case "month":
                    sbWhere.AppendFormat(" AND EventDate>=DateAdd(dd,-30,'{0}') AND EventDate<'{0}' ", nowDay);
                    break;
                default:
                    break;
            }
            sbWhere.AppendFormat(" AND [MonitorPlanID] In ({0}) ",monitorPlanIDs);
            sbWhere.AppendFormat(" AND [IPLocation] Like '%{0}%'  ", location);
            return GetCount<MonitorEventDetailsInfo>(sbWhere.ToString());
        }
    }
}
