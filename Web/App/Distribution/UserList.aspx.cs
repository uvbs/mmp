using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Distribution
{
    public partial class UserList : System.Web.UI.Page
    {
        public string websiteOwner = string.Empty;

        public BLLJIMP.Model.WebsiteInfo webSite = new BLLJIMP.Model.WebsiteInfo();

        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();

        protected void Page_Load(object sender, EventArgs e)
        {
            websiteOwner = BLLJIMP.BLLStatic.bll.WebsiteOwner;

            webSite = bllWebsite.GetWebsiteInfoModelFromDataBase();
        }
    }
}