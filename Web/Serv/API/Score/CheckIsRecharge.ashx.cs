using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// CheckIsRecharge 的摘要说明
    /// </summary>
    public class CheckIsRecharge : BaseHandlerNoAction
    {
        /// <summary>
        /// Api响应模型
        /// </summary>
        protected BaseResponse apiResp = new BaseResponse();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["id"];
            BllOrder bllOrder = new BllOrder();
            OrderPay orderPay = bllOrder.GetOrderPay(orderId, "1", bllOrder.WebsiteOwner);
            if (orderPay != null && orderPay.Status == 1)
            {
                apiResp.status = true;
            }
            bllOrder.ContextResponse(context, apiResp);
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