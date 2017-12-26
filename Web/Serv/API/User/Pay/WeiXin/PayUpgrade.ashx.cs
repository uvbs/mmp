using Newtonsoft.Json;
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
    /// PayUpgrade 的摘要说明
    /// </summary>
    public class PayUpgrade : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        BllPay bllPay = new BllPay();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.API.User.PayUpgrade payUpgrade = bll.ConvertRequestToModel<BLLJIMP.Model.API.User.PayUpgrade>(new BLLJIMP.Model.API.User.PayUpgrade());
            payUpgrade.level = CurrentUserInfo.MemberLevel;
            string websiteOwner = bll.WebsiteOwner;

            UserLevelConfig levelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", payUpgrade.level.ToString());
            if (levelConfig == null)
            {
                payUpgrade.userTotalAmount = 0;
            }
            else
            {
                payUpgrade.userTotalAmount = Convert.ToDecimal(levelConfig.FromHistoryScore);
            }
            UserLevelConfig toLevelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", payUpgrade.toLevel.ToString());
            if (toLevelConfig == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员等级未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (toLevelConfig.IsDisable == 1)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员级别禁止升级";
                bll.ContextResponse(context, apiResp);
                return;
            }
            payUpgrade.needAmount = Convert.ToDecimal(toLevelConfig.FromHistoryScore);
            payUpgrade.amount = payUpgrade.needAmount - payUpgrade.userTotalAmount;
            if (payUpgrade.amount < 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "暂时不支持降级";
                bll.ContextResponse(context, apiResp);
                return;
            }

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
            orderPay.Subject = "支付升级";
            orderPay.Total_Fee = payUpgrade.amount;
            orderPay.Type = "6";
            orderPay.WebsiteOwner = websiteOwner;
            orderPay.UserId = CurrentUserInfo.UserID;
            orderPay.Ex1 = JsonConvert.SerializeObject(payUpgrade);
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
            string notifyUrl = string.Format("http://{0}/WxPayNotify/PayUpgradeNotify.aspx", context.Request.Url.Authority);//支付升级通知地址
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