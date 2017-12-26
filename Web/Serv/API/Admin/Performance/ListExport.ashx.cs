using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.File;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// ListExport 的摘要说明
    /// </summary>
    public class ListExport : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int rows = int.MaxValue;
            int page = 1;
            int yearMonth = Convert.ToInt32(context.Request["yearmonth"]);
            string up_member = context.Request["up_member"];
            string member = context.Request["member"];
            string websiteOwner = bll.WebsiteOwner;
            string userIds = "";
            #region 查出userIds
            if (!string.IsNullOrWhiteSpace(member))
            {
                userIds = bllUser.GetSpreadUserIds(member, websiteOwner);
            }
            string parentIds = "";
            if (!string.IsNullOrWhiteSpace(up_member))
            {
                parentIds = bllUser.GetSpreadUserIds(up_member, websiteOwner);
            }
            #endregion
            int total = bll.GetChildPerformanceCount(parentIds, websiteOwner, yearMonth, userIds);
            List<TeamPerformance> performanceList = new List<TeamPerformance>();
            if (total > 0) performanceList = bll.GetChildPerformanceList(rows, page, parentIds,
                websiteOwner, yearMonth, userIds, "AutoID,UserName,UserPhone,DistributionOwner,YearMonth,Performance,Reward,Status");

            DataTable dt = new DataTable();
            dt.Columns.Add("会员手机", typeof(string));
            dt.Columns.Add("会员姓名", typeof(string));
            dt.Columns.Add("推荐人手机", typeof(string));
            dt.Columns.Add("推荐人姓名", typeof(string));
            dt.Columns.Add("月份", typeof(int));
            dt.Columns.Add("公司", typeof(string));
            dt.Columns.Add("业绩", typeof(decimal));
            dt.Columns.Add("管理奖", typeof(decimal));
            if (performanceList.Count > 0)
            {
                List<string> uIdList = performanceList.Select(p => p.UserId).Distinct().ToList();
                uIdList.AddRange(performanceList.Select(p => p.DistributionOwner).Distinct().ToList());
                string uIds = ZentCloud.Common.MyStringHelper.ListToStr(uIdList, "'", ",");
                List<UserInfo> uList = bllUser.GetColMultListByKey<UserInfo>(int.MaxValue, 1, "UserID", uIds, "AutoID,WXNickname,TrueName,Phone,UserID,IntelligenceCertificateBusiness", websiteOwner: websiteOwner);
                foreach (TeamPerformance item in performanceList)
                {
                    UserInfo u = uList.FirstOrDefault(p => p.UserID == item.UserId);
                    UserInfo upUser = uList.FirstOrDefault(p => p.UserID == item.DistributionOwner);

                    DataRow dr = dt.NewRow();
                    dr["会员手机"] = u == null ? item.UserPhone : u.Phone;
                    dr["会员姓名"] = u == null? item.UserName: bllUser.GetUserDispalyName(u);
                    dr["推荐人手机"] = upUser == null ? "" : upUser.Phone;
                    dr["推荐人姓名"] = upUser == null ? "" : upUser.TrueName;
                    dr["月份"] = item.YearMonth;
                    dr["公司"] = u == null ? "" : u.IntelligenceCertificateBusiness;
                    dr["业绩"] = item.Performance;
                    dr["管理奖"] = item.Reward;
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
            }
            MemoryStream ms = Web.DataLoadTool.NPOIHelper.Export(dt, "会员业绩");
            ExportCache exCache = new ExportCache()
            {
                FileName = string.Format("会员业绩{0}.xls", DateTime.Now.ToString("yyyyMMddHHmm")),
                Stream = ms
            };
            string cache = Guid.NewGuid().ToString("N").ToUpper();
            ZentCloud.Common.DataCache.SetCache(cache, exCache, slidingExpiration: TimeSpan.FromMinutes(5));

            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "生成完成";
            apiResp.result = new
            {
                cache = cache
            };
            bllUser.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}