using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class index : System.Web.UI.Page
    {

        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 商品一级分类列表
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        public ZentCloud.BLLJIMP.Model.WebsiteInfo currWebSiteInfo;
        /// <summary>
        /// 商品二级分类
        /// </summary>
        public System.Text.StringBuilder sbSecondCategory = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            var CategoryList = bll.GetLit<WXMallCategory>(100, 1, string.Format("WebsiteOwner='{0}' And (PreID=0 Or PreID is  null) ", bll.WebsiteOwner));
            //sbCategory.AppendFormat("<li class=\"current\" data-categoryid=\"\"><a href=\"javascript:\">全部<span class=\"icon\"></span></a></li>");

            if (CategoryList.Count > 0)
            {
                sbCategory.Append("<div class=\"kindbox\"><ul class=\"kind\">");
                sbCategory.AppendFormat("<li data-categoryid=\"\" data-rootcategroy=1 class=\"current\"><a class=\"categorylist\" href=\"javascript:\">全部</a></li>");
                foreach (var item in CategoryList)
                {

                    var SecondCategoryList = bll.GetList<WXMallCategory>(string.Format("PreId={0}",item.AutoID));
                    if (SecondCategoryList.Count>0)
                    {
                        sbSecondCategory.AppendFormat("<ul class=\"kind\" id=\"fengleip{0}\">",item.AutoID);
                        foreach (var item1 in SecondCategoryList)
	                    {
		                 sbSecondCategory.AppendFormat("<li data-categoryid=\"{0}\"><a class=\"categorylist\" >{1}</a></li>",item1.AutoID,item1.CategoryName);
                             
	                    }
                        sbSecondCategory.AppendFormat("</ul> ");




                        sbCategory.AppendFormat("<li>");
                        sbCategory.AppendFormat("<div class=\"categorylist fenleip\" sid=\"fengleip{0}\">{1}",item.AutoID,item.CategoryName);
                        sbCategory.AppendFormat("<em class=\"icon\"></em>");
                        sbCategory.AppendFormat("</div>");
                        sbCategory.AppendFormat("</li>");

                    }
                    else
                    {
                        sbCategory.AppendFormat("<li data-categoryid=\"{0}\" data-rootcategroy=1><a class=\"categorylist\" href=\"javascript:\">{1}</a></li>", item.AutoID, item.CategoryName);
                        
                    }

                   




                }
                



                sbCategory.Append("</ul>");
                sbCategory.Append("</div>");

            }
            currWebSiteInfo = bll.GetWebsiteInfoModel();
            if (string.IsNullOrEmpty(currWebSiteInfo.MallType))
            {
                currWebSiteInfo.MallType = "0";
            }
        }
    }
}