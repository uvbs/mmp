using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.Supplier
{
    /// <summary>
    /// 未结算订单
    /// </summary>
    public class UnSettlementList : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string orderStatus = context.Request["order_status"];//订单状态
            string orderFromDate = context.Request["order_from_date"];//日期
            string orderToDate = context.Request["order_to_date"];//日期
            if (!string.IsNullOrEmpty(orderToDate))
            {
                orderToDate = Convert.ToDateTime(orderToDate).AddDays(1).AddSeconds(-1).ToString();
            }
            string orderId = context.Request["order_id"];//订单号
            string supplierUserId = context.Request["supplier_user_id"];//供应商账号

            int totalCount = 0;
            var sourceData = bllMall.UnSettlementList(bllMall.WebsiteOwner, pageIndex, pageSize, supplierUserId, orderId, orderStatus, orderFromDate, orderToDate, out totalCount);
            var data = from p in sourceData
                       select new
                       {
                           id = p.AutoId,
                           supplier_name = p.SupplierName,
                           supplier_user_id = p.SupplierUserId,
                           order_id=p.OrderId,
                           order_date = p.OrderDate.ToString(),
                           order_status=p.OrderStatus,
                           settlement_baseamount=p.BaseAmount,
                           settlement_sale_amount = p.SaleAmount,
                           settlement_server_amount = p.ServerAmount,
                           transportfee = p.TransportFee,
                           settlement_amount = p.SettlementAmount
                          
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