using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.TransfersAudit
{
    public partial class List : System.Web.UI.Page
    {
        BLLJIMP.BLLTransfersAudit bll = new BLLJIMP.BLLTransfersAudit();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!bll.IsLogin)
            {
                Response.Redirect("/error/commonmsg.aspx?msg=请在微信客户端中打开");
            }
            if (!bll.IsTransfersAuditPer(bll.GetCurrentUserInfo()))
            {
                Response.Redirect("/error/commonmsg.aspx?msg=您没有审核权限");
            }
        }
    }
}