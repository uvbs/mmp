using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Performance
{
    public partial class Set : System.Web.UI.Page
    {
        BLLDistribution bll = new BLLDistribution();
        protected List<TeamPerformanceSet> baseSets;
        protected void Page_Load(object sender, EventArgs e)
        {
            string websiteOwner = bll.WebsiteOwner;
            baseSets = bll.GetListByKey<TeamPerformanceSet>("UserId",websiteOwner,websiteOwner);
        }
    }
}