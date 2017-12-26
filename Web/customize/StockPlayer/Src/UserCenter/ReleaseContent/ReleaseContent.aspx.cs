using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.ReleaseContent
{
    public partial class ReleaseContent : System.Web.UI.Page
    {
        BLLUser bllUser = new BLLUser();
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJuActivity();
        protected UserInfo curUser = null;
        protected string jid = string.Empty;
        protected JuActivityInfo model = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            curUser = bllUser.GetCurrentUserInfo();
            if (curUser == null)
            {
                this.Response.Redirect("/customize/StockPlayer/Src/UserCenter/UserCenter.aspx", true);
                return;
            }
            //公司账号未通过审核
            if (curUser.UserType == 6 && curUser.MemberApplyStatus != 9)
            {
                this.Response.Redirect("/customize/StockPlayer/Src/UserCenter/UserCenter.aspx", true);
                return;
            }
            if (!string.IsNullOrEmpty(Request["jid"]))
            {
                jid = Request["jid"];
                model = bllJuActivity.GetJuActivity(int.Parse(jid),true,bllJuActivity.WebsiteOwner);
                if (model == null || curUser.UserID != model.UserID)
                {
                    this.Response.Redirect("/customize/StockPlayer/Src/UserCenter/UserCenter.aspx", true);
                    return;
                }
            }
        }
    }
}