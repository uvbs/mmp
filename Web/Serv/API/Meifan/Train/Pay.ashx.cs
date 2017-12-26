using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Train
{

    /// <summary>
    /// 活动 比赛 培训 支付
    /// </summary>
    public class Pay : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMeifan bllMeifan = new BLLJIMP.BLLMeifan();
        /// <summary>
        /// 
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        public void ProcessRequest(HttpContext context)
        {
            //if (!bllPay.IsWeiXinBrowser)
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "请在微信中打开";
            //    bllPay.ContextResponse(context,resp);
            //    return;

            //}
            string orderId = context.Request["order_id"];
            string appId = "";//微信AppId
            string mchId = "";//商户号
            string key = "";//api密钥
            string openId = "";//openid
            string ip = "";//ip
            string notifyUrl = string.Format("http://{0}/WxPayNotify/NotifyActivity.aspx", context.Request.Url.Host);//支付通知地址
            string body = "";//订单内容
            if (string.IsNullOrEmpty(orderId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_id 必传";
                bllPay.ContextResponse(context, resp);
                return;
            }
            var orderInfo = bllMeifan.GetActivityDataByOrderId(orderId);
            if (orderInfo != null)
            {

                if (orderInfo.PaymentStatus == 1)
                {
                    resp.errcode = 1;
                    resp.errmsg = "该订单已经支付,不需重复支付";
                    bllPay.ContextResponse(context, resp);
                    return;
                }
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "order_id 错误";
                bllPay.ContextResponse(context, resp);
                return;


            }

            PayConfig payConfig = bllPay.GetPayConfig();
            if (payConfig == null)
            {
                resp.errcode = 1;
                resp.errmsg = "该商户微信支付还没有配置";
                bllPay.ContextResponse(context, resp);
                return;
            }
            if ((string.IsNullOrEmpty(payConfig.WXAppId)) || (string.IsNullOrEmpty(payConfig.WXMCH_ID)) || (string.IsNullOrEmpty(payConfig.WXPartnerKey)))
            {
                resp.errcode = 1;
                resp.errmsg = "该商户微信支付还没有配置";
                bllPay.ContextResponse(context, resp);
                return;
            }
            appId = payConfig.WXAppId;
            mchId = payConfig.WXMCH_ID;
            key = payConfig.WXPartnerKey;
            openId = CurrentUserInfo.WXOpenId;
            ip = context.Request.UserHostAddress;
            try
            {
                string payReqStr = bllPay.GetBrandWcPayRequest(orderId, orderInfo.Amount, appId, mchId, key, openId, ip, notifyUrl, body);
                ZentCloud.BLLJIMP.BllPay.WXPayReq payReqModel = ZentCloud.Common.JSONHelper.JsonToModel<ZentCloud.BLLJIMP.BllPay.WXPayReq>(payReqStr);
                if (!string.IsNullOrEmpty(payReqModel.paySign))
                {
                    var data = new
                    {
                        errcode = 0,
                        errmsg = "ok",
                        order_id=orderId,
                        pay_req = payReqModel
                    };
                    bllPay.ContextResponse(context, data);

                }
            }
            catch (Exception)
            {
                resp.errcode = 1;
                resp.errmsg = "支付失败";
                bllPay.ContextResponse(context, resp);

            }

        }

        /// <summary>
        /// 默认响应模型
        /// </summary>
        public class DefaultResponse
        {

            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }


        }

    }

}