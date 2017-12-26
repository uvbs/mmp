using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// Details 的摘要说明
    /// </summary>
    public class Details : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string id = context.Request["id"];
            string sum = context.Request["sum"];
            string websiteOwner = bll.WebsiteOwner;

            TeamPerformance performance = bll.GetColByKey<TeamPerformance>("AutoId", id, "AutoId,YearMonth,DetailIds");
            string ids = string.IsNullOrWhiteSpace(performance.DetailIds) ? "-1" : performance.DetailIds;
            int yearMonth = performance.YearMonth;
            int total = bll.GetPerformanceDetailCount("", "", websiteOwner, yearMonth, "", ids);
            List<TeamPerformanceDetails> performanceList = new List<TeamPerformanceDetails>();
            if (total > 0) performanceList = bll.GetPerformanceDetailList(rows, page, "", "", websiteOwner, yearMonth, "", ids);

            decimal sumPerformance = 0;
            if (sum == "1") sumPerformance = bll.GetPerformanceDetailSum("", "", websiteOwner, yearMonth,"",ids);
            List<dynamic> resultList = new List<dynamic>();
            if (performanceList.Count > 0)
            {
                string parentIds = ZentCloud.Common.MyStringHelper.ListToStr(performanceList.Select(p=>p.DistributionOwner).ToList(), "'", ",");
                List<UserInfo> uuList = bll.GetColMultListByKey<UserInfo>(int.MaxValue, 1, "UserID", parentIds, "AutoID,UserID,TrueName,Phone", 
                    websiteOwner: websiteOwner);
                foreach (TeamPerformanceDetails item in performanceList)
                {
                    UserInfo pu = uuList.FirstOrDefault(p => p.UserID == item.DistributionOwner);
                    resultList.Add(new
                    {
                        id = item.AutoId,
                        name = item.UserName,
                        phone = item.UserPhone,
                        pid = pu.AutoID,
                        pname = pu.TrueName,
                        pphone = pu.Phone,
                        performance = item.Performance,
                        addtype = item.AddType,
                        addnote = item.AddNote,
                        addtime = item.AddTime.ToString("yyyy/MM/dd HH:mm:ss")
                    });

                }
            }

            apiResp.result = new
            {
                totalcount = total,
                list = resultList,
                sum = sumPerformance
            };
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "获取业绩列表";
            bll.ContextResponse(context, apiResp);

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