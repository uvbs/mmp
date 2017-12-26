using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCDALEngine;

namespace ZentCloud.JubitIMP.Web.Admin.Outlets.Comm
{
    public partial class SetTypeConfig : System.Web.UI.Page
    {
        public ArticleCategoryTypeConfig nCategoryTypeConfig;
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        protected List<string> limitForeach = new List<string>() {};
        protected List<string> fieldList = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            nCategoryTypeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, Request["type"]);
            MetaTable metaTable = DALEngine.GetMetas().Tables["ZCJ_JuActivityInfo"];
            fieldList = metaTable.Columns.Keys.Where(p => !limitForeach.Contains(p)).ToList();
        }
    }
}