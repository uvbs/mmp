using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Order
{
    /// <summary>
    /// Add 的摘要说明   下单 
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BllOrder bll = new BLLJIMP.BllOrder();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {
                apiResp.code =(int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "json格式错误,请检查";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.total_fee <= 0)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "total_fee 为必填项,请检查";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.type))
            {
                apiResp.msg = "type 为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            OrderPay model = new OrderPay();
            model.OrderId = bll.GetGUID(BLLJIMP.TransacType.CommAdd);
            model.Total_Fee = requestModel.total_fee;
            model.InsertDate = DateTime.Now;
            model.Type = requestModel.type;
            model.WebsiteOwner = bll.WebsiteOwner;
            model.UserId = bll.GetCurrUserID();
            model.Status = 0;
            if (model.Type == "1")
            {
                model.Subject = "账户充值";
            }
            else if (model.Type == "2")
            {
                model.Subject = "购买VIP";
            }
            else if (model.Type == "3")
            {
                model.Subject = "信用金充值";
            }

            if (bll.Add(model))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
                apiResp.result = model.OrderId;
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "操作出错";
            }
            bll.ContextResponse(context, apiResp);
        }


        public class RequestModel
        {
            /// <summary>
            /// 总金额
            /// </summary>
            public decimal total_fee { get; set; }

            /// <summary>
            /// 订单类型 0账户充值  1信用金充值
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 交易流水号 由第三方支付 提供
            /// </summary>
            public string trade_no { get; set; }
        }
    }
}