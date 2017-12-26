using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Performance
{
    /// <summary>
    /// GetReward 的摘要说明
    /// </summary>
    public class GetReward : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string websiteOwner = bll.WebsiteOwner;
            TeamPerformance myPerformance = bll.GetByKey<TeamPerformance>("AutoID", id, websiteOwner: websiteOwner);
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "获取管理奖信息";

            apiResp.result = myPerformance == null ? new ResponsePerformce() : new ResponsePerformce
                {
                    id = myPerformance.AutoID,
                    member = CurrentUserInfo.TrueName,
                    performance = Convert.ToDouble(myPerformance.Performance),
                    rate = Convert.ToDouble(myPerformance.Rate),
                    total_reward = Convert.ToDouble(myPerformance.TotalReward),
                    child_reward = Convert.ToDouble(myPerformance.ChildReward),
                    reward = Convert.ToDouble(myPerformance.Reward),
                    amount = Convert.ToDouble((myPerformance.Reward * 80 / 100)),
                    fund = Convert.ToDouble((myPerformance.Reward * 20 / 100))
                };
            bll.ContextResponse(context, apiResp);
        }

        public class ResponsePerformce
        {
            public int id { get; set; }
            public string member { get; set; }
            public double performance { get; set; }
            public double rate { get; set; }
            public double total_reward { get; set; }
            public double child_reward { get; set; }
            public double reward { get; set; }
            public double amount { get; set; }
            public double fund { get; set; }
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