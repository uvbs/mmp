using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Appointment
{
    /// <summary>
    /// GetTodayRank 的摘要说明
    /// </summary>
    public class GetTodayCreditAcountRank : BaseHandlerNoAction
    {
        BLLJuActivity bll = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            decimal credit_acount = Convert.ToDecimal(context.Request["credit_acount"]);
            int rank = bll.GetTodayCreditAcountRank("Appointment", null, bll.WebsiteOwner, credit_acount);
            apiResp.result = rank;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "查询完成";
            bll.ContextResponse(context, apiResp);
        }

    }
}