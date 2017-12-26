using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.CompanyWebsite
{
    public partial class CompanyWebsiteTemplateSet : System.Web.UI.Page
    {
        BLLJIMP.BLLWebSite bll = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 当前使用模板
        /// </summary>
        public string templatename = null;
        /// <summary>
        /// 所有模板列表
        /// </summary>
        public List<CompanyWebsiteTemplate> TemplateList;
        protected void Page_Load(object sender, EventArgs e)
        {
             templatename = bll.GetWebsiteInfoModelFromDataBase().CompanyWebSiteTemplateName;
             TemplateList = bll.GetCompanyWebsiteTemplateList();
        }
    }
}