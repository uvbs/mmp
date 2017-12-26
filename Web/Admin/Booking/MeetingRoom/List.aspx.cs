using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Booking.MeetingRoom
{
    public partial class List : System.Web.UI.Page
    {
        public string categoryType;
        public string currShowName;
        public int cateRootId = 0;
        public bool isAdded = false;
        public ArticleCategoryTypeConfig nCategoryTypeConfig;
        public StringBuilder sbCategory = new StringBuilder();
        BLLJIMP.BLLArticleCategory bllArticleCategory = new BLLJIMP.BLLArticleCategory();
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            categoryType = Request["type"];
            isAdded = categoryType.Contains("Added");
            nCategoryTypeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, categoryType);
            if (nCategoryTypeConfig != null) currShowName = nCategoryTypeConfig.CategoryTypeDispalyName;

            List<BLLJIMP.Model.ArticleCategory> list = bllArticleCategory.GetList<BLLJIMP.Model.ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'",bllArticleCategory.WebsiteOwner, categoryType));
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(list, "AutoID", "PreID", "CategoryName", cateRootId, "ddlCate", "width:200px", ""));

            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
        }
    }
}