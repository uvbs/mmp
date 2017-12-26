using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Wifi.Log
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 日志模块
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string orderID = context.Request["order_id"];
            if (string.IsNullOrEmpty(orderID))
            {
                apiResp.msg = "订单号为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderID);
            if (orderInfo == null)
            {
                apiResp.msg = "订单号不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            int totalCount = 0;
            var sourceData = bllLog.List(pageIndex, pageSize, null, null, null, null, out totalCount, orderID);
            var list = from p in sourceData
                       select new
                       {
                           remark = p.Remark,
                           insert_time = bllLog.GetTimeStamp(p.InsertDate)
                       };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list,//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }
    }
}