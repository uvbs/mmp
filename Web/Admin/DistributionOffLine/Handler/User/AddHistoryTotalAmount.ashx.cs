using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.User
{
    /// <summary>
    /// 增加累计佣金
    /// </summary>
    public class AddHistoryTotalAmount : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string autoIds = context.Request["autoIds"];
            decimal amount = decimal.Parse(context.Request["amount"]);
            apiResp.status = bll.UpdateHistoryTotalAmount(autoIds, amount);
            context.Response.Write(Common.JSONHelper.ObjectToJson(apiResp));
        }
    }
}