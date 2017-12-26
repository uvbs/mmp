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
    public partial class Edit : System.Web.UI.Page
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        protected JuActivityInfo nInfo;
        public StringBuilder sbCategory = new StringBuilder();
        protected string actName = "编辑";
        private int cateRootId =0;
        protected void Page_Load(object sender, EventArgs e)
        {

             
            string supplierId=Request["supplier_id"];
            string id = this.Request["id"];
            if (id == "0") { 
                actName = "新增";
                nInfo = new JuActivityInfo();
            }
            else
            {
                nInfo = bllJuActivity.GetColByKey<JuActivityInfo>("JuActivityID", id,
                    "JuActivityID,ActivityName,ActivityAddress,ThumbnailsPath,ServerTimeMsg,ServicesMsg,Sort,Tags,UserLongitude,UserLatitude,CategoryId,Province,ProvinceCode,City,CityCode,District,DistrictCode,K1,K4");
                if (nInfo == null) this.Response.Redirect("List.aspx");
            }

            if (!string.IsNullOrEmpty(supplierId))
            {
                actName = "编辑";
                nInfo = bllJuActivity.Get<JuActivityInfo>(string.Format(" K5='{0}' And WebsiteOwner='{1}' And ArticleType='Outlets'", supplierId,bllArticleCategory.WebsiteOwner));
                if (nInfo==null)
                {
                    actName = "新增";
                    nInfo = new JuActivityInfo();
                }

 

            }

            List<ArticleCategory> list = bllArticleCategory.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", bllArticleCategory.WebsiteOwner, "Outlets"));
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(list, "AutoID", "PreID", "CategoryName", cateRootId, "ddlCate", "width:200px", ""));
        }
    }
}