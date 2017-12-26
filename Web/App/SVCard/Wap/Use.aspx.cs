using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.SVCard.Wap
{
    public partial class Use : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!bllUser.IsLogin)
            {
                Response.Redirect("/error/commonmsg.aspx?msg=请在微信客户端中打开");
                Response.End();
            }
            if (!bllUser.IsWeixinKefu(bllUser.GetCurrentUserInfo()))
            {
                Response.Redirect("/error/commonmsg.aspx?msg=无权操作");
                Response.End();
            }
        }
    }
}