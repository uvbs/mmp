using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class WebsiteCompile : System.Web.UI.Page
    {
        BLLJIMP.BLLWebsiteDomainInfo bllDomain = new BLLJIMP.BLLWebsiteDomainInfo();
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        protected string action = string.Empty;
        protected string webAction = string.Empty;
        protected WebsiteInfo model = new WebsiteInfo();
        protected List<UserLevelConfig> levelList = new List<UserLevelConfig>();
        protected void Page_Load(object sender, EventArgs e)
        {
            action = !string.IsNullOrEmpty(Request["websiteowner"]) ? "编辑" : "新建";
            webAction = !string.IsNullOrEmpty(Request["websiteowner"]) ? "EditWebsite" : "AddWebsite";
            if (!string.IsNullOrEmpty(Request["websiteowner"]))
            {
                model = bllWebsite.GetWebsiteInfo(Request["websiteowner"]);
            }
            levelList = bllDis.QueryUserLevelList(model.WebsiteOwner, "DistributionOnLine");

        }
    }
}