using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Outlets.Comm
{
    public partial class Edit : System.Web.UI.Page
    {
        protected ArticleCategoryTypeConfig typeConfig = new ArticleCategoryTypeConfig();
        protected List<TableFieldMapping> formField = new List<TableFieldMapping>();
        protected JuActivityInfo nInfo = new JuActivityInfo();
        protected JToken nInfoJtoken = new JObject();
        protected string id;
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        BLLJuActivity bll = new BLLJuActivity();
        public StringBuilder sbCategory = new StringBuilder();
        private int cateRootId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request["type"];
            id = this.Request["id"];

            formField = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_JuActivityInfo", Request["type"], null, "0", null);
            typeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, type);

            string colName = "JuActivityID,UserLongitude,UserLatitude";
            if (formField.Count > 0) colName = colName + "," + ZentCloud.Common.MyStringHelper.ListToStr(formField.Select(p => p.Field).Distinct().ToList(),"",",");

            if (id != "0")
            {
                nInfo = bll.GetColByKey<JuActivityInfo>("JuActivityID", id, colName);
                if (nInfo == null) this.Response.Redirect("List.aspx?type=" + type, true);
            }
            nInfoJtoken = JToken.FromObject(nInfo);
            List<ArticleCategory> list = bllArticleCategory.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", bllArticleCategory.WebsiteOwner, type));
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(list, "AutoID", "PreID", "CategoryName", cateRootId, "ddlCate", "width:200px", ""));
        }
    }
}