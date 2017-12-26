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
    /// PayRegister 的摘要说明
    /// </summary>
    public class PayRegister : BaseHandlerNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        BllPay bllPay = new BllPay();
        public void ProcessRequest(HttpContext context)
        {
            BuildOrder(context, 1);
        }
        public void BuildOrder(HttpContext context, int payType)
        {
            BLLJIMP.Model.API.User.PayRegisterUser requestUser = bll.ConvertRequestToModel<BLLJIMP.Model.API.User.PayRegisterUser>(new BLLJIMP.Model.API.User.PayRegisterUser());
            string websiteOwner = bll.WebsiteOwner;
            if (string.IsNullOrWhiteSpace(requestUser.level.ToString()))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请选择会员级别";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestUser.spreadid))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入推荐人编号";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestUser.phone))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入手机号码";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(requestUser.phone))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "手机号码格式不正确";
                bll.ContextResponse(context, apiResp);
                return;
            }

            if (!ZentCloud.Common.MyRegex.IsIDCard(requestUser.idcard))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "身份证号码必须如实填写";
                bll.ContextResponse(context, apiResp);
                return;
            }
            UserLevelConfig levelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", requestUser.level.ToString());
            if (levelConfig == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员级别未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (levelConfig.IsDisable == 1)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员级别禁止注册";
                bll.ContextResponse(context, apiResp);
                return;
            }
            requestUser.levelname = levelConfig.LevelString;
            UserInfo spreadUser = bllUser.GetSpreadUser(requestUser.spreadid, websiteOwner);
            if (spreadUser == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "推荐人未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            requestUser.spreadid = spreadUser.UserID; //推荐人
            UserInfo oldUserInfo = bllUser.GetUserInfoByPhone(requestUser.phone, websiteOwner);
            if (oldUserInfo != null && oldUserInfo.MemberLevel > 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该手机已注册会员";
                bll.ContextResponse(context, apiResp);
                return;
            }

            PayConfig payConfig = bllPay.GetPayConfig();
            if (payType ==1 && !bllPay.IsAliPay(payConfig))
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
            if (oldUserInfo != null)
            {
                requestUser.userid = oldUserInfo.UserID;
            }
            else
            {
                requestUser.userid = string.Format("ZYUser{0}{1}", DateTime.Now.ToString("yyyyMMdd"), Guid.NewGuid().ToString("N").ToUpper());
            }
            requestUser.regIP = context.Request.UserHostAddress;//ip
            requestUser.password = ZentCloud.Common.Rand.Number(6);

            OrderPay orderPay = new OrderPay();
            orderPay.OrderId = bll.GetGUID(TransacType.PayRegisterOrder);
            orderPay.InsertDate = DateTime.Now;
            orderPay.Subject = "支付注册会员";
            orderPay.Total_Fee = Convert.ToDecimal(levelConfig.FromHistoryScore);
            orderPay.Type = "5";
            orderPay.WebsiteOwner = websiteOwner;
            orderPay.UserId = requestUser.userid;
            orderPay.Ex1 = JsonConvert.SerializeObject(requestUser);
            orderPay.Ex2 = requestUser.phone;
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