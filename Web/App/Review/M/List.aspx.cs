using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Review.M
{
    public partial class List : System.Web.UI.Page
    {

        /// <summary>
        /// 分类列表html
        /// </summary>
        protected string CategoryHtml;
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLReview bll = new BLLJIMP.BLLReview();
        /// <summary>
        /// 当前站点信息
        /// </summary>
        protected WebsiteInfo currentWebsiteInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ArticleCategory> categoryList = bll.GetList<ArticleCategory>(string.Format(" CategoryType='word'AND WebsiteOwner='{0}'", bll.WebsiteOwner));
            foreach (ArticleCategory item in categoryList)
            {
                CategoryHtml += "<li class=\"catli\" v=\"" + item.AutoID + "\"><a >" + item.CategoryName + "</a></li>";
            }
            currentWebsiteInfo = bll.GetWebsiteInfoModelFromDataBase();

        }

        
    }
}