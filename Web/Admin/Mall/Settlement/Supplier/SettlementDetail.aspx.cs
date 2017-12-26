using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Mall.Settlement.Supplier
{
    public partial class SettlementDetail : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        public BLLJIMP.Model.SupplierSettlement model;
        /// <summary>
        /// 
        /// </summary>
        public int SettlementOrderCount;
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllMall.Get<SupplierSettlement>(string.Format("SettlementId='{0}'", Request["settlement_id"]));
            SettlementOrderCount = bllMall.GetCount<SupplierSettlementDetail>(string.Format(" SettlementId='{0}'",model.SettlementId));


        }
    }
}