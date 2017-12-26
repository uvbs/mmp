using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Pay.Alipay
{
    /// <summary>
    /// GetAliPayRequestMobile 的摘要说明     支付宝请求接口
    /// </summary>
    public class GetAliPayRequestMobile : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string callbackUrl = context.Request["callback_url"];
            if (string.IsNullOrEmpty(orderId))
            {
                apiResp.msg = "请传入订单编号";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllPay.ContextResponse(context, apiResp);
                return;
            }
            OrderPay orderInfo = bllOrder.GetOrderPay(orderId);
            if (orderInfo == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "不存在订单";
                bllPay.ContextResponse(context, apiResp);
                return;
            }
            if (orderInfo.UserId != CurrentUserInfo.UserID)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "无效订单";
                bllPay.ContextResponse(context, apiResp);
                return;
            }
            if (orderInfo.Status == 1)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "订单已经支付,不需重复支付";
                bllPay.ContextResponse(context, apiResp);
                return;
            }



            RequestAlipay aliPay = new RequestAlipay();
            aliPay.notify_url = "http://" + context.Request.Url.Host + "/Alipay/NotifyAplipay.aspx";//支付通知地址
            PayConfig payConfig = bllPay.GetPayConfig();
            aliPay.order_id = orderInfo.OrderId;
            aliPay.total_fee=orderInfo.Total_Fee;
            aliPay.seller_account_name=payConfig.Seller_Account_Name;
            aliPay.partner=payConfig.Partner;
            aliPay.key=payConfig.PartnerKey;
            try
            {
                string payReqStr = bllPay.GetAliPayRequestMobile(aliPay.order_id, Convert.ToDouble(aliPay.total_fee), aliPay.seller_account_name, aliPay.partner, aliPay.key, aliPay.notify_url,"",callbackUrl,"");
                if (!string.IsNullOrEmpty(payReqStr))
                {
                    apiResp.result = payReqStr;
                    apiResp.status = true;
                }
            }
            catch (Exception ex)
            {
                apiResp.msg = ex.ToString();
            }
            bllPay.ContextResponse(context, apiResp);
        }


        #region 请求参数
        public class RequestAlipay 
        {
            /// <summary>
            /// 订单号
            /// </summary>
            public string order_id { get; set; }

            /// <summary>
            /// 金额
            /// </summary>
            public decimal total_fee { get; set; }

            /// <summary>
            /// 支付宝账号
            /// </summary>
            public string seller_account_name { get; set; }

            /// <summary>
            /// 商户号
            /// </summary>
            public string partner { get; set; }

            /// <summary>
            /// 签名
            /// </summary>
            public string key { get; set; }

            /// <summary>
            /// 通知地址
            /// </summary>
            public string notify_url { get; set; }
        }
        #endregion
    }
}