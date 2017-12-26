using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Wap
{
    public partial class LoginBinding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //UserInfo curUser = new BLLUser().GetCurrentUserInfo();
            //if (curUser !=null && curUser.MemberLevel > 0)
            //{
            //this.Response.Redirect("/app/wap/UserCenter.aspx", true);
            //}
        }
    }
}