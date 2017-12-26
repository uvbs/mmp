using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class indexv2 : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 商品分类列表
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        public ZentCloud.BLLJIMP.Model.WebsiteInfo currWebSiteInfo;
        public ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            var CategoryList = bll.GetLit<WXMallCategory>(100, 1, string.Format("WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (CategoryList.Count > 0)
            {
                sbCategory.Append("<div class=\"kindbox\"><ul class=\"kind\">");
                foreach (var item in CategoryList)
                {

                    sbCategory.AppendFormat("<li data-categoryid=\"{0}\"><a href=\"javascript:\">{1}<span class=\"icon\"></span></a></li>", item.AutoID, item.CategoryName);

                }
                sbCategory.Append("</ul>");
                sbCategory.Append("</div>");

            }
            currWebSiteInfo = bll.GetWebsiteInfoModel();
            currWebSiteUserInfo = new BLLJIMP.BLLUser("").GetUserInfo(DataLoadTool.GetWebsiteInfoModel().WebsiteOwner);




        }

    }
}