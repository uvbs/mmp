using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionMall
{
    public partial class Config : System.Web.UI.Page
    {
        BLLDistribution bllDisb = new BLLDistribution();
        BLLWebSite bllWebsite = new BLLWebSite();
        public string qrcondeUrl = string.Empty;
        public WebsiteInfo website = null;
        public CompanyWebsite_Config config = null;
        BLLWeixin bllWeixin = new BLLWeixin();
        protected void Page_Load(object sender, EventArgs e)
        {
            qrcondeUrl = bllDisb.GetDistributionWxQrcodeLimitUrl(bllDisb.WebsiteOwner);
            qrcondeUrl = bllWeixin.CompoundImageLogoToOss(qrcondeUrl,bllWebsite.WebsiteOwner);
            
            website = bllDisb.GetWebsiteInfoModelFromDataBase();
            config = bllWebsite.GetCompanyWebsiteConfig();
            if (string.IsNullOrWhiteSpace(website.DistributionShareQrcodeBgImg))
            {
                website.DistributionShareQrcodeBgImg = "http://files.comeoncloud.net/img/gxfc.png";
            }

        }
    }
}