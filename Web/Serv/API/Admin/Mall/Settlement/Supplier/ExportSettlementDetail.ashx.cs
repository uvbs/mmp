using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.Supplier
{
    /// <summary>
    /// 导出结算单列表
    /// </summary>
    public class ExportSettlementDetail : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {



            string settlementId = context.Request["settlement_id"];
            System.Text.StringBuilder sbSql = new System.Text.StringBuilder();

            sbSql.AppendFormat("Select ");

            sbSql.AppendFormat(" OrderId as [订单号],");

            sbSql.AppendFormat(" BaseAmount as [产品结算金额],");

            sbSql.AppendFormat(" TransportFee as [代收运费],");

            sbSql.AppendFormat(" SettlementAmount as [结算金额=产品结算金额+代收运费]");

            sbSql.AppendFormat(" from ZCJ_SupplierSettlementDetail ");
            sbSql.AppendFormat(" Where SettlementId='{0}'", settlementId);

            var settlement= bllMall.Get<BLLJIMP.Model.SupplierSettlement>(string.Format(" SettlementId='{0}'",settlementId));
            System.Data.DataTable dt = dt = ZentCloud.ZCBLLEngine.BLLBase.Query(sbSql.ToString()).Tables[0];
            DataLoadTool.ExportDataTable(dt, string.Format("{0}_{1}至{2}_结算单号{3}.xls", settlement.SupplierName, settlement.FromDate.ToString("yyyyMMdd"), settlement.ToDate.ToString("yyyyMMdd"),settlementId));





        }


    }
}