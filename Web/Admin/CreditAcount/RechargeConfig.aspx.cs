using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Admin.CreditAcount
{
    public partial class RechargeConfig : System.Web.UI.Page
    {
        protected string Recharge;
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        protected void Page_Load(object sender, EventArgs e)
        {
            Recharge = bllKeyValueData.GetDataVaule("CreditAcountRecharge", "100", bllKeyValueData.WebsiteOwner);
        }
    }
}