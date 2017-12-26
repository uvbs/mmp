using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 外部支付
    /// </summary>
    public class OutPayment : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            string orderId = context.Request["order_id"];

            if (string.IsNullOrEmpty(orderId))
            {
                apiResp.code = 1;
                apiResp.msg = "order_id 必传";
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                apiResp.code = 1;
                apiResp.msg = "order_id 不存在";
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            if (orderInfo.PaymentStatus == 1)
            {
                apiResp.code = 1;
                apiResp.msg = "订单已经支付";
                bllMall.ContextResponse(context, apiResp);
                return;

            }
            Open.HongWareSDK.Client cl = new Open.HongWareSDK.Client(bllMall.WebsiteOwner);
            string callBackUrl = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderList#/orderDetail/{1}", context.Request.Url.Host, orderInfo.OrderID);
            callBackUrl = HttpUtility.UrlEncode(callBackUrl);
            string payUrl = "";
            string errMsg = "";
            bool payResult = cl.OrderPay(orderInfo.OrderID, (orderInfo.TotalAmount * 100).ToString("F0"), callBackUrl, out  payUrl,out errMsg);

            if (payResult && (!string.IsNullOrEmpty(payUrl)))
            {
                apiResp.status = true;
                apiResp.result = new
                {
                    pay_url = payUrl
                };
            }
            else
            {
                apiResp.msg = errMsg;

            }
            bllMall.ContextResponse(context, apiResp);


        }


    }
}