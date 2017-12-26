using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Outlets.Comm
{
    public partial class List : System.Web.UI.Page
    {
        protected ArticleCategoryTypeConfig typeConfig = new ArticleCategoryTypeConfig();
        protected List<TableFieldMapping> formField = new List<TableFieldMapping>();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory(); 
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }

            formField = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_JuActivityInfo", Request["type"], null, "0", null);
            typeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, Request["type"]);
        }
    }
}