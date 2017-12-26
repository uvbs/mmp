using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Outlets
{
    public partial class List : System.Web.UI.Page
    {
        public StringBuilder sbCategory = new StringBuilder();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        public int cateRootId = 0;
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ArticleCategory> list = bllArticleCategory.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", bllArticleCategory.WebsiteOwner, "Outlets"));
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(list, "AutoID", "PreID", "CategoryName", cateRootId, "ddlCate", "width:200px", ""));
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
        }
    }
}