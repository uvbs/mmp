using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.TypeConfig
{
    public partial class Set : System.Web.UI.Page
    {
        public ArticleCategoryTypeConfig nCategoryTypeConfig;
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        protected void Page_Load(object sender, EventArgs e)
        {
            nCategoryTypeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, Request["type"]);
        }
    }
}