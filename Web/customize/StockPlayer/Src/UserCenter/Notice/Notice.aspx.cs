using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.Notice
{
    public partial class Notice : System.Web.UI.Page
    {
        BLLUser bllUser = new BLLUser();
        protected UserInfo curUser = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            curUser = bllUser.GetCurrentUserInfo();
            if (curUser == null)
            {
                this.Response.Redirect("/customize/StockPlayer/SrcUserCenter/UserCenter.aspx", true);
            }
        }
    }
}