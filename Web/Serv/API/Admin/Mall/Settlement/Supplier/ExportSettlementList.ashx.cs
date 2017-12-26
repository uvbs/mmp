using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.Supplier
{
    /// <summary>
    /// 导出结算单列表
    /// </summary>
    public class ExportSettlementList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            System.Text.StringBuilder sbSql = new System.Text.StringBuilder();
            sbSql.AppendFormat("Select ");
            //sbSql.AppendFormat(" FromDate as [开始日期],");
            //sbSql.AppendFormat(" ToDate as [结束日期],");
            sbSql.AppendFormat(" (CONVERT(varchar(100), FromDate, 121) +'至'+CONVERT(varchar(100), ToDate, 121)) as 起止日期,");
            sbSql.AppendFormat(" SettlementId as [结算单号],");
            sbSql.AppendFormat(" SupplierName as [商户名称],");
            sbSql.AppendFormat(" SettlementTotalAmount as [产品结算金额],");
            sbSql.AppendFormat(" SaleTotalAmount as [销售额],");
            sbSql.AppendFormat(" ServerTotalAmount as [服务费=销售额-产品结算额],");
            sbSql.AppendFormat(" TotalTransportFee as [代收运费],");
            sbSql.AppendFormat(" SettlementTotalAmount as [结算金额=产品结算金额+代收运费],");
            sbSql.AppendFormat(" Status as [结算状态],");
            sbSql.AppendFormat(" [开票]=(case when IsInvoice=0 then '未开票' when IsInvoice=1 then '已开票' end) ");
            sbSql.AppendFormat(" from ZCJ_SupplierSettlement ");
            sbSql.AppendFormat(" Where Websiteowner='{0}'", bllMall.WebsiteOwner);
            string settlementId = context.Request["settlement_id"];
            string status = context.Request["status"];
            string supplierUserId= context.Request["supplier_user_id"];
            string date = context.Request["date"];
            if (!string.IsNullOrEmpty(supplierUserId))
            {
                sbSql.AppendFormat(" And SupplierUserId='{0}'", supplierUserId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbSql.AppendFormat(" And Status='{0}'", status);
            }
            if (!string.IsNullOrEmpty(date))
            {
                sbSql.AppendFormat(" And FromDate='{0}'", date);
            }
            if (!string.IsNullOrEmpty(settlementId))
            {
                sbSql.AppendFormat(" And SettlementId='{0}'", settlementId);
            }



            //

            System.Data.DataTable dt = dt = ZentCloud.ZCBLLEngine.BLLBase.Query(sbSql.ToString()).Tables[0];

            DataLoadTool.ExportDataTable(dt, string.Format("{0}_{1}_data.xls", "结算单", DateTime.Now.ToString()));





        }


    }
}