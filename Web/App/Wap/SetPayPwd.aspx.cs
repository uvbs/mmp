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
    public partial class SetPayPwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLUser bllUser = new BLLUser();
            UserInfo curUser = bllUser.GetCurrentUserInfo();
            if (curUser == null)
            {
                this.Response.Redirect("/app/wap/LoginBinding.aspx", true);
                return;
            }
            if (curUser.MemberLevel <= 0)
            {
                this.Response.Redirect("/app/wap/ApplyMember.aspx", true);
                return;
            }
        }
    }
}