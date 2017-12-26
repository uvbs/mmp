using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using CommonPlatform.Helper;

namespace ZentCloud.BLLJIMP
{
    public class BLLUserStatistics:BLL
    {
        BLLDashboard bllDashboard = new BLLDashboard();
        /// <summary>
        /// 往用户统计表跑数据
        /// </summary>
        public void AddUserStatistics()
        {
            var websiteList = GetList<WebsiteInfo>();

            LogHelper.WriteLog("访客统计","总" + websiteList.Count + "条",true);

            var index = 0;

            foreach (var item in websiteList)
            {
                index++;
                LogHelper.WriteLog("访客统计", "站点：" + item.WebsiteOwner + "，" + index, true);

                string nowDay = DateTime.Now.ToString("yyyy/MM/dd");
                int total=0;
                List<string> timeArray =new List<string>{ "day", "week", "month" };
                try
                {
                    for (int j = 0; j < timeArray.Count; j++)
                    {
                         List<DashboardMonitorInfo> dashList = bllDashboard.GetDashboardMonitorInfoList(int.MaxValue, 1,timeArray[j], out total,item.WebsiteOwner);
                        //LogHelper.WriteLog("访客统计", "dashList：" + dashList.Count, true);


                        if (dashList.Count <= 0) continue;
                         int number = 0;
                         if (timeArray[j] == "day")
                             number = -1;
                         else if (timeArray[j] == "week")
                             number = -7;
                         else
                             number = -30;
                         for (int i = 0; i < dashList.Count; i++)
                         {
                            
                             //访问次数
                            int visitCount = bllDashboard.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}'  AND EventDate>=DateAdd(dd,{3},'{1}') AND EventDate<'{1}' AND EventUserID='{2}' ", item.WebsiteOwner, nowDay, dashList[i].EventUserID,number));
                            //LogHelper.WriteLog("访客统计", "访问次数" + visitCount, true);
                            //文章访问次数
                            int articleBrowseCount = bllDashboard.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND EventDate>=DateAdd(dd,{4},'{1}') AND EventDate<'{1}' AND EventUserID='{2}' AND ModuleType='{3}' ", item.WebsiteOwner, nowDay, dashList[i].EventUserID, "article",number));
                            //LogHelper.WriteLog("访客统计", "文章访问次数" + articleBrowseCount, true);
                            //活动访问次数
                            int activityBrowseCount = bllDashboard.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND EventDate>=DateAdd(dd,{4},'{1}') AND EventDate<'{1}' AND EventUserID='{2}' AND ModuleType='{3}' ", item.WebsiteOwner, nowDay, dashList[i].EventUserID, "activity",number));
                            //LogHelper.WriteLog("访客统计", "活动访问次数" + activityBrowseCount, true);
                            //活动报名次数
                            int activitySignUpCount = bllDashboard.GetCount<ActivityDataInfo>(string.Format(" WebsiteOwner='{0}' AND InsertDate>=DateAdd(dd,{3},'{1}') AND InsertDate<'{1}' AND UserId='{2}' ", item.WebsiteOwner, nowDay, dashList[i].EventUserID,number));
                            //LogHelper.WriteLog("访客统计", "活动报名次数" + activitySignUpCount, true);
                            //下单次数
                            int orderCount = bllDashboard.GetCount<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND InsertDate>=DateAdd(dd,{3},'{1}') AND InsertDate<'{1}' AND OrderUserID='{2}' AND PaymentStatus=1 ", item.WebsiteOwner, nowDay, dashList[i].EventUserID,number));
                            //LogHelper.WriteLog("访客统计", "下单次数" + orderCount, true);
                            //积分记录
                            int score = Convert.ToInt32(ZentCloud.ZCBLLEngine.BLLBase.GetSingle(string.Format(" SELECT SUM(SCORE) FROM [ZCJ_UserScoreDetailsInfo] where WebsiteOwner='{0}' AND UserID='{1}' and AddTime>=DATEADD(dd,{3},'{2}') and AddTime<'{2}' AND Score>0", item.WebsiteOwner, dashList[i].EventUserID, nowDay,number)));
                            //LogHelper.WriteLog("访客统计", "积分记录" + score, true);
                            //其它访问次数
                            int otherCount = visitCount - articleBrowseCount - activityBrowseCount;
                            //LogHelper.WriteLog("访客统计", "其它访问次数" + otherCount, true);

                            UserStatistics userStatistics = bllDashboard.Get<UserStatistics>(string.Format(" WebsiteOwner='{0}' AND DateType='{1}' AND UserId='{2}' ", item.WebsiteOwner, timeArray[j], dashList[i].EventUserID));
                            
                            if (userStatistics != null)
                             {
                                //LogHelper.WriteLog("访客统计", "已存在", true);
                                if (!string.IsNullOrEmpty(dashList[i].EventUserWXNikeName)) userStatistics.WXNickName = dashList[i].EventUserWXNikeName;
                                 if (!string.IsNullOrEmpty(dashList[i].EventUserWXImg)) userStatistics.WXHeadimgurl = dashList[i].EventUserWXImg;
                                 if (!string.IsNullOrEmpty(dashList[i].EventUserTrueName)) userStatistics.TrueName = dashList[i].EventUserTrueName;
                                 userStatistics.UserId = dashList[i].EventUserID;
                                 userStatistics.WebsiteOwner = item.WebsiteOwner;
                                 userStatistics.UpdateDate = DateTime.Now;
                                 userStatistics.VisitCount = visitCount;
                                 userStatistics.ArticleBrowseCount = articleBrowseCount;
                                 userStatistics.ActivityBrowseCount = activityBrowseCount;
                                 userStatistics.ActivitySignUpCount = activitySignUpCount;
                                 userStatistics.OrderCount = orderCount;
                                 userStatistics.Score = score;
                                 userStatistics.OtherBrowseCount = otherCount;
                                //LogHelper.WriteLog("访客统计", "Update", true);
                                bool isEdit = bllDashboard.Update(userStatistics);
                                //LogHelper.WriteLog("访客统计", "isEdit:" + isEdit, true);
                            }
                             else
                             {
                                //LogHelper.WriteLog("访客统计", "新的", true);
                                userStatistics = new UserStatistics();
                                 if (!string.IsNullOrEmpty(dashList[i].EventUserWXNikeName)) userStatistics.WXNickName = dashList[i].EventUserWXNikeName;
                                 if (!string.IsNullOrEmpty(dashList[i].EventUserWXImg)) userStatistics.WXHeadimgurl = dashList[i].EventUserWXImg;
                                 if (!string.IsNullOrEmpty(dashList[i].EventUserTrueName)) userStatistics.TrueName = dashList[i].EventUserTrueName;
                                 userStatistics.UserId = dashList[i].EventUserID;
                                 userStatistics.WebsiteOwner = item.WebsiteOwner;
                                 userStatistics.DateType = timeArray[j];
                                 userStatistics.UpdateDate = DateTime.Now;
                                 userStatistics.VisitCount = visitCount;
                                 userStatistics.ArticleBrowseCount = articleBrowseCount;
                                 userStatistics.ActivityBrowseCount = activityBrowseCount;
                                 userStatistics.ActivitySignUpCount = activitySignUpCount;
                                 userStatistics.OrderCount = orderCount;
                                 userStatistics.Score = score;
                                 userStatistics.OtherBrowseCount = otherCount;
                                 bool isAdd = bllDashboard.Add(userStatistics);
                             }
                         }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("访客统计", "异常：" + ex.Message, true);
                    //throw new Exception("异常", ex);
                    continue;
                }

                LogHelper.WriteLog("访客统计", "完毕：" + item.WebsiteOwner, true);
            }
        }
        /// <summary>
        /// 获取访客列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyWord"></param>
        /// <param name="times">昨天、近7天、近30天</param>
        /// <param name="sort"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<UserStatistics> GetUserStatisticList(int pageSize,int pageIndex,string keyWord,string times,string sort,string order,out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ",WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND (WXNickName like '%{0}%' or TrueName like '%{0}%') ",keyWord);
            }
            if (!string.IsNullOrEmpty(times))
            {
                sbWhere.AppendFormat(" AND DateType='{0}' ", times);
            }
            string orderBy = " UpdateDate DESC ";
            if (!string.IsNullOrEmpty(sort)&&!string.IsNullOrEmpty(order))
            {
                orderBy = sort +" "+ order;
            }
            totalCount = GetCount<UserStatistics>(sbWhere.ToString());
            return GetLit<UserStatistics>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
        }
    }
}
