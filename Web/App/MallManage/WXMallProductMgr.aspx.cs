using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallProductMgr : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        //public System.Text.StringBuilder sbStores = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            //foreach (var item in bll.GetWXMallCategoryListByWebsite())
            //{
            //    sbCategory.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.CategoryName);

            //}

            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(bll.GetCategoryList(), "AutoID", "PreID", "CategoryName", 0, "ddlcategory", "width:200px", ""));

            //foreach (var item in bll.GetWXMallStoreListByWebSite())
            //{
            //    sbStores.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.StoreName);

            //}
        }
    }
}