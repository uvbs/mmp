using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Wifi.Order
{
    /// <summary>
    /// 申诉
    /// </summary>
    public class Appeal : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 商城BLL
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 模块BLL
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];//订单号
            string appealContent=context.Request["appeal_content"];//申诉内容
            if (string.IsNullOrEmpty(orderId))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "order_id 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(appealContent))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "appeal_content 参数必传";
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
            orderInfo.Ex8 = "1";
            orderInfo.Ex9 = appealContent;
            orderInfo.LastUpdateTime = DateTime.Now;
            if (bllMall.Update(orderInfo))
            {
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "提交申诉", orderId);
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