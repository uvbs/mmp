using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.Supplier
{
    /// <summary>
    /// SettlementDetail 的摘要说明
    /// </summary>
    public class SettlementDetail : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string settlementId = context.Request["settlement_id"];
            string orderId = context.Request["order_id"];
            int totalCount = 0;
            var sourceData = bllMall.SettlementDetail(settlementId, orderId, pageIndex, pageSize, out totalCount);
            var data = from p in sourceData
                       select new
                       {
                           id = p.AutoId,
                           settlement_id = p.SettlementId,
                           order_id = p.OrderId,
                           baseamount = p.BaseAmount,
                           transportfee = p.TransportFee,
                           refund_amount = p.RefundAmount,
                           settlement_amount = p.SettlementAmount,
                           insert_date = p.InsertDate
                       };

            apiResp.status = true;
            apiResp.result = new
            {
                totalcount = totalCount,
                list = data

            };
            bllMall.ContextResponse(context, apiResp);

        }



    }
}