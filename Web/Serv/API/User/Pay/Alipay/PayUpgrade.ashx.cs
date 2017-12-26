using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Pay.Alipay
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
            BuildOrder(context, 1, CurrentUserInfo);
        }
        public void BuildOrder(HttpContext context, int payType, UserInfo user)
        {
            BLLJIMP.Model.API.User.PayUpgrade payUpgrade = bll.ConvertRequestToModel<BLLJIMP.Model.API.User.PayUpgrade>(new BLLJIMP.Model.API.User.PayUpgrade());
            payUpgrade.level = user.MemberLevel;
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
            PayConfig payConfig = bllPay.GetPayConfig();
            if (payType == 1 && !bllPay.IsAliPay(payConfig))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该商户支付宝支付还没有配置";
                bll.ContextResponse(context, apiResp);
                return;
            }
            else if (payType == 2 && !bllPay.IsJDPay(payConfig))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该商户京东支付还没有配置";
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
            orderPay.UserId = user.UserID;
            orderPay.Ex1 = JsonConvert.SerializeObject(payUpgrade);
            orderPay.PayType = payType;
            if (!bll.Add(orderPay))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "生成支付订单失败";
                bll.ContextResponse(context, apiResp);
                return;
            }
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.result = new
            {
                user_id = user.AutoID,
                pay_order_id = orderPay.OrderId
            };
            bllUser.ContextResponse(context, apiResp);

        }

    }
}