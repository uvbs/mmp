using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Train.SignUp
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            var data = bll.GetActivityDataByOrderId(orderId);
            apiResp.result = data;
            bll.ContextResponse(context, apiResp);


        }

        
    }
}