using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Outlets.Comm
{
    public partial class Info : System.Web.UI.Page
    {
        protected ArticleCategoryTypeConfig typeConfig = new ArticleCategoryTypeConfig();
        protected List<TableFieldMapping> formField = new List<TableFieldMapping>();
        protected JToken nInfoJtoken = new JObject();

        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        BLLJuActivity bll = new BLLJuActivity();
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request["type"];
            string id = this.Request["id"];

            List<TableFieldMapping> fields = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_JuActivityInfo", type, null, "0", null);
            typeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, type);


            if (!string.IsNullOrWhiteSpace(typeConfig.Ex5))
            {
                formField = fields.Where(p => typeConfig.Ex5.Split(',').Contains(p.Field)).ToList();
            }
            if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2) {
                formField.Add(new TableFieldMapping() { Field = "UserLongitude" });
                formField.Add(new TableFieldMapping() { Field = "UserLatitude" });
            }
            if (id != "0")
            {
                string colName = ZentCloud.Common.MyStringHelper.ListToStr(formField.Select(p => p.Field).Distinct().ToList(), "", ",");
                if (!colName.Contains("JuActivityID")) colName = "JuActivityID," + colName;
                JuActivityInfo nInfo = bll.GetColByKey<JuActivityInfo>("JuActivityID", id, colName);
                if (nInfo == null) this.Response.Redirect("List.aspx?type=" + type, true);
                JToken ttoken = JToken.FromObject(nInfo);
                foreach (var item in formField)
                {
                    nInfoJtoken[item.Field] = ttoken[item.Field];
                }
            }
        }
    }
}