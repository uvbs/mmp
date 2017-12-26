using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.Supplier
{
    /// <summary>
    /// 已经结算
    /// </summary>
    public class SettlementList :BaseHandlerNeedLoginAdminNoAction
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
            string status=context.Request["status"];//状态
            string date=context.Request["date"];//日期
            string settlementId = context.Request["settlement_id"];//结算单号
            string supplierUserId=context.Request["supplier_user_id"];//供应商账号
            if (bllUser.IsSupplier(currentUserInfo))
            {
                supplierUserId = currentUserInfo.UserID;
            }
            int totalCount = 0;
            var sourceData = bllMall.SettlementList(bllMall.WebsiteOwner, supplierUserId, pageIndex, pageSize, status,settlementId, date, out totalCount);
            var data = from p in sourceData
                       select new
                       {
                           id = p.AutoId,
                           settlement_id=p.SettlementId,
                           supplier_user_id=p.SupplierUserId,
                           supplier_name=p.SupplierName,
                           from_date=p.FromDate.ToString(),
                           to_date=p.ToDate.ToString(),
                           status=p.Status,
                           total_baseamount=p.TotalBaseAmount,
                           total_transportfee=p.TotalTransportFee,
                           refund_total_amount=p.RefundTotalAmount,
                           settlement_total_amount=p.SettlementTotalAmount,
                           insert_date=p.InsertDate,
                           remark=p.Remark,
                           img_url=p.ImgUrl,
                           settlement_sale_amount=p.SaleTotalAmount,
                           settlement_server_amount = p.ServerTotalAmount,
                           is_invoice=p.IsInvoice
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