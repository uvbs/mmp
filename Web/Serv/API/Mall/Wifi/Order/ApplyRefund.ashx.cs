using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Wifi.Order
{
    /// <summary>
    /// 申请退款
    /// </summary>
    public class ApplyRefund : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 模块BLL
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            if (string.IsNullOrEmpty(orderId))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "order_id 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "order_id 不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (orderInfo.OrderUserID != CurrentUserInfo.UserID)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "无权访问";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (orderInfo.PaymentStatus == 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "订单未付款";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (orderInfo.Status == "交易成功")
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "当前状态不能申请退款";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (orderInfo.Ex11=="1")
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "已经申请过退款";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            orderInfo.Ex11 = "1";
            orderInfo.Ex18 = "";
            orderInfo.Ex19 = "";
            orderInfo.IsRefund = 1;
            orderInfo.LastUpdateTime = DateTime.Now;
            if (bllMall.Update(orderInfo))
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "申请退款", orderId);
                apiResp.msg = "ok";
                apiResp.status = true;
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "fail";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));





        }


    }
}