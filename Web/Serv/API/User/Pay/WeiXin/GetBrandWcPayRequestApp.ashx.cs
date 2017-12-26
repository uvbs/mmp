using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Pay.WeiXin
{
    /// <summary>
    /// GetBrandWcPayRequestApp 的摘要说明
    /// </summary>
    public class GetBrandWcPayRequestApp : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            if (string.IsNullOrEmpty(orderId))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "order_id 为必填项,请检查";
                bllPay.ContextResponse(context, apiResp);
                return;
            }
            OrderPay orderInfo = bllOrder.GetOrderPay(orderId);
            if (orderInfo == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "订单号不存在";
                bllPay.ContextResponse(context, apiResp);
                return;
            }
            if (orderInfo.UserId != CurrentUserInfo.UserID)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "订单号无效";
                bllPay.ContextResponse(context, apiResp);
                return;
            }
            if (orderInfo.Status == 1)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                apiResp.msg = "订单已经支付,不需重复支付";
                bllPay.ContextResponse(context, apiResp);
                return;
            }
            RequestWeiXin weixin = new RequestWeiXin();
            weixin.notifyUrl= "http://" + context.Request.Url.Host + "/WxPayNotify/NotifyWeixinPay.aspx";//支付通知地址
            PayConfig payConfig = bllPay.GetPayConfig();
            weixin.appId = payConfig.WXAppId;
            weixin.mch_Id = payConfig.WXMCH_ID;
            weixin.key = payConfig.WXPartnerKey;
            weixin.openId = CurrentUserInfo.WXOpenId;
            weixin.ip = context.Request.UserHostAddress;
            string payReqStr = bllPay.GetBrandWcPayRequest(orderInfo.OrderId, orderInfo.Total_Fee, weixin.appId,weixin.mch_Id, weixin.key, weixin.openId, weixin.ip, weixin.notifyUrl, weixin.body,"APP");
            BllPay.WXPayReq payReqModel = ZentCloud.Common.JSONHelper.JsonToModel<BllPay.WXPayReq>(payReqStr);
            if (!string.IsNullOrEmpty(payReqModel.paySign))
            {
                  
                bllPay.ContextResponse(context, new 
                {
                    status=true,
                    pay_req = payReqModel
                });

            }
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            apiResp.msg = "出错";
            bllPay.ContextResponse(context, apiResp);
        }

        #region 请求参数
        public class RequestWeiXin 
        {
            /// <summary>
            /// 微信分配的公众账号ID
            /// </summary>
            public string appId { get; set; }

            /// <summary>
            /// 微信支付分配的商户号
            /// </summary>
            public string mch_Id { get; set; }

            /// <summary>
            /// key
            /// </summary>
            public string key { get; set; }

            /// <summary>
            /// trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。
            /// </summary>
            public string openId { get; set; }

            /// <summary>
            /// APP和网页支付提交用户端ip
            /// </summary>
            public string ip { get; set; }

            /// <summary>
            /// 支付通知地址
            /// </summary>
            public string notifyUrl { get; set; }

            /// <summary>
            /// 	商品或支付单简要描述
            /// </summary>
            public string body { get; set; }
        }
        #endregion
    }
}