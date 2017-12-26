using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics
{
    /// <summary>
    ///  店铺统计  
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 50;
            string sDate = context.Request["start_date"];
            string eDate = context.Request["stop_date"];
            string sort=context.Request["sort"];
            int totalCount=0;
            List<WXMallStatistics> StatisticsList = bllMall.GetWXMallStatisticsList(pageSize, pageIndex, sDate, eDate, sort, out totalCount);

            List<dynamic> returnList = new List<dynamic>();

            foreach (WXMallStatistics item in StatisticsList)
            {
                returnList.Add(new {
                    date=item.Date,
                    order_count=item.OrderCount,
                    order_prouduct_count=item.OrderProuductTotalCount,
                    order_totalamount = item.OrderTotalAmount,
                    refund_product_totalcount = item.RefundProductTotalCount,
                    refund_totalamount=item.RefundTotalAmount,
                    pv=item.PV,
                    uv=item.UV,
                    product_totalcount=item.ProductTotalCount,
                    convertrate=item.ConvertRate,
                    per_customer_transaction=item.PerCustomerTransaction,
                    procuct_average_price=item.ProcuctAveragePrice,
                    order_totalamount_month=item.OrderTotalAmountMonth,
                    total_sales=item.TotalSales,
                    invoice_amount=item.InvoiceAmount,
                    merchant_settlemen_total_amount=item.MerchantSettlemenTotalAmount
                });
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new {
                total = totalCount,
                rows = returnList
            }));
           
        }

       
    }
}