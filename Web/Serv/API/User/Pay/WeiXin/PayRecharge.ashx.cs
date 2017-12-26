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
    /// PayRecharge 的摘要说明
    /// </summary>
    public class PayRecharge : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        BllPay bllPay = new BllPay();
        public void ProcessRequest(HttpContext context)
        {
            decimal amount = Convert.ToDecimal(context.Request["amount"]);
            if (amount <= 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "金额必须大于0";
                bll.ContextResponse(context, apiResp);
                return;
            } 
            string websiteOwner = bll.WebsiteOwner;
            if (string.IsNullOrWhiteSpace(CurrentUserInfo.WXOpenId))
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "请先绑定微信";
                bll.ContextResponse(context, apiResp);
                return;
            }
            PayConfig payConfig = bllPay.GetPayConfig();
            if (payConfig == null || string.IsNullOrEmpty(payConfig.WXAppId) || string.IsNullOrEmpty(payConfig.WXMCH_ID) || string.IsNullOrEmpty(payConfig.WXPartnerKey))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该商户微信支付还没有配置";
                bll.ContextResponse(context, apiResp);
                return;
            }
            OrderPay orderPay = new OrderPay();
            orderPay.OrderId = bll.GetGUID(TransacType.PayRegisterOrder);
            orderPay.InsertDate = DateTime.Now;
            orderPay.Subject = "支付充值";
            orderPay.Total_Fee = amount;
            orderPay.Type = "4";
            orderPay.WebsiteOwner = websiteOwner;
            orderPay.UserId = CurrentUserInfo.UserID;
            if (!bll.Add(orderPay))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "生成支付订单失败";
                bll.ContextResponse(context, apiResp);
                return;
            }

            string appId = payConfig.WXAppId;//微信AppId
            string mchId = payConfig.WXMCH_ID;//商户号
            string key = payConfig.WXPartnerKey;//api密钥
            string openId = CurrentUserInfo.WXOpenId;//openid
            string ip = context.Request.UserHostAddress;//ip
            string notifyUrl = string.Format("http://{0}/WxPayNotify/PayRechargeNotify.aspx", context.Request.Url.Authority);//支付充值通知地址
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
            bllUser.ContextResponse(context, apiResp);

        }
    }
}