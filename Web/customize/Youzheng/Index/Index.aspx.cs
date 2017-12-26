using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.Youzheng.Index
{
    public partial class Index : System.Web.UI.Page
    {
        public List<CompanyWebsite_ToolBar> barList = new List<CompanyWebsite_ToolBar>();
        public List<Slide> slideList = new List<Slide>();
        BLLJIMP.BLLCompanyWebSite bllCompanyWebsite = new BLLJIMP.BLLCompanyWebSite();
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyWebsite_Config config = bllCompanyWebsite.Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'",bllCompanyWebsite.WebsiteOwner));

            StringBuilder sbWhere = new StringBuilder();
       
            sbWhere.AppendFormat(" WebsiteOwner = '{0}'", bllCompanyWebsite.WebsiteOwner);
            sbWhere.AppendFormat(" AND UseType='nav'");
            if (config != null && !string.IsNullOrEmpty(config.ShopNavGroupName))
            {
                sbWhere.AppendFormat(" AND KeyType='{0}'", config.ShopNavGroupName);
            }
            barList = bllCompanyWebsite.GetList<CompanyWebsite_ToolBar>(sbWhere.ToString());

            StringBuilder sbWhere1 = new StringBuilder();

            if (config != null && !string.IsNullOrEmpty(config.ShopAdType))
            {
                slideList = bllCompanyWebsite.GetList<Slide>(string.Format(" WebsiteOwner='{0}' AND Type='{1}'", bllCompanyWebsite.WebsiteOwner, config.ShopAdType));
            }

            
          
        }
    }
}