using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Booking.MeetingRoom.Order
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
        protected void Page_Load(object sender, EventArgs e)
        {
            categoryType = Request["type"];
            isAdded = categoryType.Contains("Added");
            nCategoryTypeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, categoryType);
            if (nCategoryTypeConfig != null) currShowName = nCategoryTypeConfig.CategoryTypeDispalyName;
        }
    }
}