using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.BookingDoctorFuYou
{
    public partial class Index : System.Web.UI.Page
    {
        public CompanyWebsite_Config config = new CompanyWebsite_Config();
        BLLJIMP.BLLWebSite bll = new BLLJIMP.BLLWebSite();
        protected void Page_Load(object sender, EventArgs e)
        {
            config = bll.GetCompanyWebsiteConfig();
            if (config==null)
            {
                config = new CompanyWebsite_Config();
                config.WebsiteTitle = "膏方专家预约平台";
                config.WebsiteDescription = "膏方专家预约平台";

            }

        }
    }
}