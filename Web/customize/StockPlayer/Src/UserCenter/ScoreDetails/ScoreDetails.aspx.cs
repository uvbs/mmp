using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.ScoreDetails
{
    public partial class ScoreDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
            BLLJIMP.Model.UserInfo curUser = bllKeyValueData.GetCurrentUserInfo();
            if (curUser == null)
            {
                this.Response.Redirect("/customize/StockPlayer/SrcUserCenter/UserCenter.aspx", true);
                return;
            }
        }
    }
}