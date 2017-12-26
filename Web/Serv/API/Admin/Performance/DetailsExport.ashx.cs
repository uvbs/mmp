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
    /// DetailsExport 的摘要说明
    /// </summary>
    public class DetailsExport : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            int rows = int.MaxValue;
            int page = 1;
            string id = context.Request["id"];
            string websiteOwner = bll.WebsiteOwner;

            TeamPerformance performance = bll.GetColByKey<TeamPerformance>("AutoId", id, "AutoId,YearMonth,DetailIds");
            string ids = string.IsNullOrWhiteSpace(performance.DetailIds) ? "-1" : performance.DetailIds;
            int yearMonth = performance.YearMonth;
            int total = bll.GetPerformanceDetailCount("", "", websiteOwner, yearMonth, "", ids);
            List<TeamPerformanceDetails> performanceList = new List<TeamPerformanceDetails>();
            if (total > 0) performanceList = bll.GetPerformanceDetailList(rows, page, "", "", websiteOwner, yearMonth, "", ids);

            DataTable dt = new DataTable();
            dt.Columns.Add("会员手机", typeof(string));
            dt.Columns.Add("会员姓名", typeof(string));
            dt.Columns.Add("推荐人手机", typeof(string));
            dt.Columns.Add("推荐人姓名", typeof(string));
            dt.Columns.Add("月份", typeof(int));
            dt.Columns.Add("事件", typeof(string));
            dt.Columns.Add("金额", typeof(decimal));
            dt.Columns.Add("时间", typeof(string));
            dt.Columns.Add("说明", typeof(string));
            if (performanceList.Count > 0)
            {
                string parentIds = ZentCloud.Common.MyStringHelper.ListToStr(performanceList.Select(p=>p.DistributionOwner).ToList(), "'", ",");
                List<UserInfo> uuList = bll.GetColMultListByKey<UserInfo>(int.MaxValue, 1, "UserID", parentIds, "AutoID,UserID,TrueName,Phone", 
                    websiteOwner:websiteOwner);
                foreach (TeamPerformanceDetails item in performanceList)
                {
                    UserInfo pu = uuList.FirstOrDefault(p => p.UserID == item.DistributionOwner);

                    DataRow dr = dt.NewRow();
                    dr["会员手机"] = item.UserPhone;
                    dr["会员姓名"] = item.UserName;
                    dr["推荐人手机"] = pu == null ? "" : pu.Phone;
                    dr["推荐人姓名"] = pu == null ? "" : pu.TrueName;
                    dr["月份"] = item.YearMonth;
                    dr["事件"] = item.AddType;
                    dr["金额"] = item.Performance;
                    dr["时间"] = item.AddTime.ToString("yyyy-MM-dd HH:mm:ss");
                    dr["说明"] = item.AddNote;
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
            }
            MemoryStream ms = Web.DataLoadTool.NPOIHelper.Export(dt, "会员业绩明细");
            ExportCache exCache = new ExportCache()
            {
                FileName = string.Format("会员业绩明细{0}.xls", DateTime.Now.ToString("yyyyMMddHHmm")),
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