using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.mmpadmin
{
    public partial class index : System.Web.UI.Page
    {
        BLLWebSite bllWebsite = new BLLWebSite();
        protected void Page_Load(object sender, EventArgs e)
        {
            WebsiteInfo webSiteModel = bllWebsite.GetWebsiteInfo();
            UserInfo curUser = bllWebsite.GetCurrentUserInfo();
            if (webSiteModel.WebsiteExpirationDate.HasValue &&
                webSiteModel.WebsiteExpirationDate.Value.AddDays(1).AddSeconds(-1) < DateTime.Now &&
                curUser.UserType != 1)
            {
                this.Response.Redirect("/Error/expire.htm");
            }
        }
    }
}