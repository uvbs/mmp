using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Distribution
{
    /// <summary>
    /// GetPerformanceList 的摘要说明
    /// </summary>
    public class GetPerformanceList : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        BLLFlow bllFlow = new BLLFlow();
        public void ProcessRequest(HttpContext context)
        {
            int yearMonth = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
            if (!string.IsNullOrWhiteSpace(context.Request["yearmonth"])) yearMonth = Convert.ToInt32(context.Request["yearmonth"]);
            bool hideName = true;
            bool hidePhone = true;

            string websiteOwner = bll.WebsiteOwner;
            string userId = CurrentUserInfo.UserID;
            TeamPerformance myPerformance = bll.GetMyPerformance(userId, websiteOwner, yearMonth);
            List<TeamPerformance> childPerformanceList = bll.GetChildPerformanceList(int.MaxValue, 1, userId, websiteOwner, yearMonth);

            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "获取团队业绩";
            apiResp.result = new
            {
                my = myPerformance == null ? new ResponsePerformce() : new ResponsePerformce
                {
                    id = myPerformance.AutoID,
                    performance = Convert.ToDouble(myPerformance.Performance),
                    reward = Convert.ToDouble(myPerformance.Reward),
                    status = myPerformance.Status,
                    act_id = myPerformance.FlowActionId,
                    act_status = myPerformance.FlowActionStatus
                },
                list = from p in childPerformanceList
                       select new
                       {
                           id = p.AutoID,
                           name = bllUser.GetUserShowDispalyName(p.UserName, hideName),
                           phone = bllUser.GetUserDispalyPhone(p.UserPhone, hidePhone),
                           performance = p.Performance
                       }
            };
            bll.ContextResponse(context, apiResp);
        }
        public class ResponsePerformce
        {
            public int id { get; set; }
            public double performance { get; set; }
            public double reward { get; set; }
            public int act_id { get; set; }
            public int act_status { get; set; }
            public int status { get; set; }
        }

        public class ResponseFlowAction
        {
            public int id { get; set; }
            public int status { get; set; }
            public string create_time { get; set; }
            public string end_time { get; set; }
        }
    }
}