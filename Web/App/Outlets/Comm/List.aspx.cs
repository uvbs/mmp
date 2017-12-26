using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Outlets.Comm
{
    public partial class List : System.Web.UI.Page
    {
        protected ArticleCategoryTypeConfig typeConfig = new ArticleCategoryTypeConfig();
        protected List<TableFieldMapping> searchField = new List<TableFieldMapping>();
        protected List<TableFieldMapping> listField = new List<TableFieldMapping>();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        protected void Page_Load(object sender, EventArgs e)
        {
            typeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, Request["type"]);
            List<TableFieldMapping> formField = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_JuActivityInfo", Request["type"], null, "0", null);
            if (!string.IsNullOrWhiteSpace(typeConfig.Ex1))
            {
                searchField = formField.Where(p => !string.IsNullOrWhiteSpace(p.Options) && typeConfig.Ex1.Split(',').Contains(p.Field)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(typeConfig.Ex4))
            {
                listField = formField.Where(p => typeConfig.Ex4.Split(',').Contains(p.Field)).ToList();
            }
        }
    }
}