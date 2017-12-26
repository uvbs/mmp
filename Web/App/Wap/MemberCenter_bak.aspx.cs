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
    public partial class MemberCenter_bak : System.Web.UI.Page
    {
        protected WebsiteInfo website = new WebsiteInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!MemberCenter.checkUser(this.Context))
            {
                return;
            }
            BLLUser bllUser = new BLLUser();
            website = bllUser.GetColByKey<WebsiteInfo>("WebsiteOwner", bllUser.WebsiteOwner, "WebsiteOwner,TotalAmountShowName");
        }

        public static bool checkUser(HttpContext context, bool checkPayPwd = false, UserInfo curUser = null)
        {
            BLLUser bllUser = new BLLUser();
            if(curUser==null) curUser = bllUser.GetCurrentUserInfo();
            if (curUser == null)
            {
                context.Response.Redirect("/app/wap/LoginBinding.aspx", true);
                return false;
            }
            if (curUser.MemberLevel <= 0)
            {
                context.Response.Redirect("/app/wap/ApplyMember.aspx", true);
                return false;
            }
            if (string.IsNullOrWhiteSpace(curUser.PayPassword))
            {
                context.Response.Redirect("/app/wap/SetPayPwd.aspx", true);
                return false;
            }
            return true;
        }
    }
}