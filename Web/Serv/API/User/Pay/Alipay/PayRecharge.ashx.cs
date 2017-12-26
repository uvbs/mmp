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
    /// PayRecharge 的摘要说明
    /// </summary>
    public class PayRecharge : BaseHandlerNeedLoginNoAction
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
            decimal amount = Convert.ToDecimal(context.Request["amount"]);
            if (amount <= 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "金额必须大于0";
                bll.ContextResponse(context, apiResp);
                return;
            }
            string websiteOwner = bll.WebsiteOwner;
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
            orderPay.Subject = "支付充值";
            orderPay.Total_Fee = amount;
            orderPay.Type = "4";
            orderPay.WebsiteOwner = websiteOwner;
            orderPay.UserId = user.UserID;
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
                pay_order_id = orderPay.OrderId
            };
            bllUser.ContextResponse(context, apiResp);
        }
    }
}