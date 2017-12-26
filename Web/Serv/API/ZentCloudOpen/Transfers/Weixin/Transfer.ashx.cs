using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;
using System.IO;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Transfers.Weixin
{
    /// <summary>
    /// 微信打款
    /// </summary>
    public class Transfer : BaseHanderOpen
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 微信
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {


            string userId = context.Request["user_id"];
            string amountStr = context.Request["amount"];
            string orderSn = context.Request["order_sn"];

            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(orderSn))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "user_id,order_sn 不能同时为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if ((!string.IsNullOrEmpty(userId)) && (!string.IsNullOrEmpty(orderSn)))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "user_id,order_sn 不能同时传入";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(amountStr))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "amount 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            decimal amount = 0;
            if (!decimal.TryParse(amountStr, out amount))
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "amount 参数错误";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (amount <= 0)
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "amount 需大于0";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            UserInfo userInfo = new UserInfo();
            WXMallOrderInfo orderInfo = null;
            if (string.IsNullOrEmpty(userId) && (!string.IsNullOrEmpty(orderSn)))//给指定订单号打款
            {
                orderInfo = bllMall.GetOrderInfoByOutOrderId(orderSn);
                if (orderInfo == null)
                {
                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "订单号不存在";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (amount > orderInfo.TotalAmount)
                {
                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "amount 不能大于订单总金额" + orderInfo.TotalAmount + "元";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                userInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);


            }
            else if ((!string.IsNullOrEmpty(userId)) && (string.IsNullOrEmpty(orderSn)))//给指定用户打款
            {
                userInfo = bllUser.GetUserInfo(userId);
            }
            if (userInfo == null)//指定用户不存在
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "指定的用户不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(userInfo.WXOpenId))
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "用户微信 OpenId不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ZentCloud.BLLJIMP.Model.PayConfig payConfig = bllPay.GetPayConfig();
            if (payConfig == null)
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "未配置支付信息";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(payConfig.WXAppId) || string.IsNullOrEmpty(payConfig.WXMCH_ID) || string.IsNullOrEmpty(payConfig.WXPartnerKey))
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "支付配置信息不完整";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }

            Log(string.Format("准备打款:订单号{0}金额{1}",orderInfo.OutOrderId,amountStr));
            string msg = "";
            string weiXinRefundId = "";//微信退款单号
            string refundNumber = orderInfo.OutOrderId + System.Math.Round(bllMall.GetTimeStamp(DateTime.Now), 0).ToString();
            bool isSuccess = bllPay.WeixinRefund(orderInfo.OrderID, refundNumber, orderInfo.TotalAmount, amount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weiXinRefundId);
            if ((!string.IsNullOrEmpty(msg)) && (msg.Contains("请使用可用余额退款")))
            {
                isSuccess = bllPay.WeixinRefundYuEr(orderInfo.OrderID, refundNumber, orderInfo.TotalAmount, amount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weiXinRefundId);
            }
            if (isSuccess)
            {
                resp.status = true;
                resp.msg = "ok";
                if (orderInfo != null)
                
                {
                    if (orderInfo.Status == "待退押金中")
                    {
                        orderInfo.Ex10 = "";
                        orderInfo.Status = "交易成功";
                    }
                    else
                    {
                        orderInfo.Status = "已取消";
                    }
                    if (orderInfo.IsRefund==1)
                    {
                        orderInfo.Status = "已取消";
                        orderInfo.IsRefund = 0;
                        orderInfo.Ex11 = "";
                    }
                    orderInfo.Ex8 = "";
                    orderInfo.Ex9 = "";
                    orderInfo.LastUpdateTime = DateTime.Now;
                    orderInfo.Ex17 = weiXinRefundId;
                    if (!bllMall.Update(orderInfo))
                    {
                         
                        var orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                        if (orderUserInfo!=null)
                        {
                            string title="您收到打款";
                            string content = string.Format("金额:{0}\\n备注:{1}",amount,orderInfo.Ex14);
                            bllWeixin.SendTemplateMessageNotifyComm(orderUserInfo,title,content);
                        }
                        resp.msg = "更新订单状态失败";
                        Log(string.Format("更新订单状态失败:订单号{0}金额{1}", orderInfo.OutOrderId, amountStr));
                    }
                    Log(string.Format("打款成功:订单号{0}金额{1}", orderInfo.OutOrderId, amountStr));
                }

            }
            else
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = msg;
                Log(string.Format("打款失败:订单号{0}金额{1}信息{2}", orderInfo.OutOrderId, amountStr,msg));
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }

        private void Log(string log) {

            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\wifitransferlog.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                }
            }
            catch { }
        
        
        }
    }
}