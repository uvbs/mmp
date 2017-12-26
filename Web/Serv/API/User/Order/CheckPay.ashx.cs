using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Order
{
    /// <summary>
    /// CheckPay 的摘要说明
    /// </summary>
    public class CheckPay : BaseHandlerNoAction
    {
        BllOrder bllOrder = new BllOrder();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string websiteOwner = bllOrder.WebsiteOwner;
            OrderPay orderPay = bllOrder.GetColByKey<OrderPay>("OrderId", orderId, "AutoID,Status", websiteOwner: websiteOwner);
            if (orderPay == null) { 
                apiResp.msg = "订单未找到";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            else if (orderPay.Status == 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "订单还未支付";
            }
            else if (orderPay.Status == 1)
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "订单支付完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "订单状态未知";
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