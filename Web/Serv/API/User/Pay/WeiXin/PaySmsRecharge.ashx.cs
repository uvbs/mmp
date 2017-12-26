using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Pay.WeiXin
{
    /// <summary>
    /// 短信充值
    /// </summary>
    public class PaySmsRecharge : BaseHandlerNeedLoginNoAction
    {

        BLLJIMP.BLL bll = new BLL();
        BllPay bllPay = new BllPay();
        BllOrder bllOrder = new BllOrder();
        public void ProcessRequest(HttpContext context)
        {
           string orderId=context.Request["order_id"];
            if (string.IsNullOrEmpty(orderId))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "order_id必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            OrderPay orderPay = bllOrder.GetOrderPay(orderId);
            PayConfig payConfig = bllPay.GetPayConfig();
            if (payConfig == null || string.IsNullOrEmpty(payConfig.WXAppId) || string.IsNullOrEmpty(payConfig.WXMCH_ID) || string.IsNullOrEmpty(payConfig.WXPartnerKey))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该商户微信支付还没有配置";
                bll.ContextResponse(context, apiResp);
                return;
            }

            string appId = payConfig.WXAppId;//微信AppId
            string mchId = payConfig.WXMCH_ID;//商户号
            string key = payConfig.WXPartnerKey;//api密钥
            string openId = CurrentUserInfo.WXOpenId;//openid
            string ip = context.Request.UserHostAddress;//ip
            string notifyUrl = string.Format("http://{0}/WxPayNotify/SmsRechargeNotify.aspx", context.Request.Url.Authority);//支付充值通知地址
            string body = "";//订单内容

            string payReqStr = bllPay.GetBrandWcPayRequest(orderPay.OrderId, orderPay.Total_Fee, appId, mchId, key, openId, ip, notifyUrl, body);
            BllPay.WXPayReq payReqModel = ZentCloud.Common.JSONHelper.JsonToModel<BllPay.WXPayReq>(payReqStr);
            if (string.IsNullOrEmpty(payReqModel.paySign))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "构造支付信息失败";
                bll.ContextResponse(context, apiResp);
                return;
            }

            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.result = new
            {
                pay_req = payReqModel
            };
            bll.ContextResponse(context, apiResp);

        }
    }
}