using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Ask
{
    public partial class AskList : System.Web.UI.Page
    {
        protected System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        protected void Page_Load(object sender, EventArgs e)
        {
            int Tcount = 0;
            List<ArticleCategory> cates = bllArticleCategory.GetCateList(out Tcount, null, 80, bllJuActivity.WebsiteOwner);
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(cates, "AutoID", "PreID", "CategoryName", 80, "ddlcategory", "width:200px", ""));
        }
    }
}