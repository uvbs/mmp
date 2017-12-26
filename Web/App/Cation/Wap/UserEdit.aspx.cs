using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class UserEdit : System.Web.UI.Page
    {
        /// <summary>
        ///站点BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        protected WebsiteInfo webSite = new WebsiteInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            webSite = bllWebsite.GetWebsiteInfo(bllWebsite.WebsiteOwner);
        }
    }
}