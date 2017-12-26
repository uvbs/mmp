using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Booking.MeetingRoom
{
    public partial class Edit : System.Web.UI.Page
    {
        public string categoryType;
        public string currShowName;
        public string currStockName="容量";
        public string currActionName;
        public string product_id;
        public int cateRootId = 0;
        public bool isAdded = false;
        public int[] weekIndex = new int[7] { 1, 2, 3, 4, 5, 6, 0 };
        public string[] weekIndexString = new string[7] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
        public ArticleCategoryTypeConfig nCategoryTypeConfig;
        public StringBuilder sbCategory = new StringBuilder();
        BLLJIMP.BLLArticleCategory bllArticleCategory = new BLLJIMP.BLLArticleCategory();
        protected void Page_Load(object sender, EventArgs e)
        {

            product_id = Request["product_id"];
            if (product_id == "0")
            {
                currActionName = "添加";
            }
            else
            {
                currActionName = "编辑";
            }

            categoryType = Request["type"];
            isAdded = categoryType.Contains("Added");
            nCategoryTypeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, categoryType);
            if (nCategoryTypeConfig != null) { 
                currShowName = nCategoryTypeConfig.CategoryTypeDispalyName;
                if (!string.IsNullOrWhiteSpace(nCategoryTypeConfig.CategoryTypeStockName)) currStockName = nCategoryTypeConfig.CategoryTypeStockName;
            }
            else { nCategoryTypeConfig = new ArticleCategoryTypeConfig(); }


            List<BLLJIMP.Model.ArticleCategory> list = bllArticleCategory.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", bllArticleCategory.WebsiteOwner, categoryType));
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(list, "AutoID", "PreID", "CategoryName", cateRootId, "ddlCate", "width:300px", ""));
        }
    }
}