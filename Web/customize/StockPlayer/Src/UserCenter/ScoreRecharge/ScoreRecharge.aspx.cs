using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.ScoreRecharge
{
    public partial class ScoreRecharge : System.Web.UI.Page
    {
        protected string Recharge;
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLJIMP.Model.UserInfo curUser = bllKeyValueData.GetCurrentUserInfo();
            if (curUser == null)
            {
                this.Response.Redirect("/customize/StockPlayer/SrcUserCenter/UserCenter.aspx", true);
                return;
            }
            Recharge = bllKeyValueData.GetDataVaule("Recharge", "100", bllKeyValueData.WebsiteOwner);
            if (string.IsNullOrWhiteSpace(Recharge)) Recharge = "100";
        }
    }
}