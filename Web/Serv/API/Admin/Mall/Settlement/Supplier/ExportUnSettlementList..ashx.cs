using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.Supplier
{
    /// <summary>
    /// 导出未结算单列表
    /// </summary>
    public class ExportUnSettlementList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            string orderStatus = context.Request["order_status"];
            string orderFromDate = context.Request["order_from_date"];
            string orderToDate = context.Request["order_to_date"];
            string orderId = context.Request["order_id"];
            string supplierUserId = context.Request["supplier_user_id"];


            System.Text.StringBuilder sbSql = new System.Text.StringBuilder();
            sbSql.AppendFormat("Select ");


            sbSql.AppendFormat(" SupplierName as [商户名称],");
            sbSql.AppendFormat(" OrderId as [订单编号],");
            sbSql.AppendFormat(" OrderDate as [下单时间],");
            sbSql.AppendFormat(" BaseAmount as [产品结算金额],");
            sbSql.AppendFormat(" SaleAmount as [销售额],");
            sbSql.AppendFormat(" ServerAmount as [服务费=销售额-产品结算额],");
            sbSql.AppendFormat(" TransportFee as [代收运费],");
            sbSql.AppendFormat(" SettlementAmount as [结算金额=产品结算金额+代收运费],");
            sbSql.AppendFormat(" OrderStatus as [订单状态],");
            sbSql.AppendFormat(" [退款]=(case when IsRefund=0 then '无' when IsRefund=1 then '有' end) ");
            sbSql.AppendFormat(" from ZCJ_SupplierUnSettlement ");
            sbSql.AppendFormat(" Where Websiteowner='{0}'", bllMall.WebsiteOwner);

            if (!string.IsNullOrEmpty(supplierUserId))
            {
                sbSql.AppendFormat(" And SupplierUserId='{0}'", supplierUserId);
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                sbSql.AppendFormat(" And OrderId='{0}'", orderId);
            }
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (orderStatus != "退款退货")
                {
                    sbSql.AppendFormat(" And OrderStatus='{0}'", orderStatus);
                }
                else
                {
                    sbSql.AppendFormat(" And IsRefund=1 ");
                }

            }
            if (!string.IsNullOrEmpty(orderFromDate))
            {
                sbSql.AppendFormat(" And OrderDate>='{0}'", orderFromDate);
            }
            if (!string.IsNullOrEmpty(orderToDate))
            {
                sbSql.AppendFormat(" And OrderDate<='{0}'", orderToDate);
            }



            //

            System.Data.DataTable dt = dt = ZentCloud.ZCBLLEngine.BLLBase.Query(sbSql.ToString()).Tables[0];

            DataLoadTool.ExportDataTable(dt, string.Format("{0}_{1}_data.xls", "未结算单", DateTime.Now.ToString()));





        }


    }
}