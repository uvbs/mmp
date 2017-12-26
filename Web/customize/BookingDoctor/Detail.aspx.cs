using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.BookingDoctor
{
    public partial class Detail : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        BLLJIMP.BLLWebSite bllWebSite = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 医生信息
        /// </summary>
        public WXMallProductInfo model = new WXMallProductInfo();
        /// <summary>
        /// 扩展字段
        /// </summary>
        public List<TableFieldMapping> fieldList = new List<TableFieldMapping>();
        /// <summary>
        /// 科系列表
        /// </summary>
        public List<WXMallCategory> categoryList = new List<WXMallCategory>();

        public CompanyWebsite_Config config = new CompanyWebsite_Config();

        public string categoryId = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            config = bllWebSite.GetCompanyWebsiteConfig();
            if (config == null)
            {
                config = new CompanyWebsite_Config();
                config.WebsiteTitle = "膏方专家预约平台";
                config.WebsiteDescription = "膏方专家预约平台";

            }
           



            if (string.IsNullOrEmpty(Request["id"]))
            {
                Response.Write("id 参数必传");
                Response.End();
            }
            model = bllMall.GetProduct(Request["id"]);
            if (model==null)
            {
                Response.Write("id 参数错误");
                Response.End();
            }
            config.WebsiteTitle = string.Format("中医专家-{0}-膏方预约平台已开通！",model.PName);
            if (model.IsOnSale=="0")
            {
                 config.WebsiteTitle = string.Format("中医专家-{0}-膏方预约平台即将开通！", model.PName);
            }

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WebsiteOwner='{0}' And TableName ='ZCJ_WXMallOrderInfo' Order by Sort DESC", bllMall.WebsiteOwner);
            fieldList = bllMall.GetList<TableFieldMapping>(sbWhere.ToString());

            categoryList = bllMall.GetCategoryList().Where(p=>p.PreID==0).Where(p=>p.Type=="BookingDoctor").ToList();

            if (!string.IsNullOrEmpty(model.CategoryId))
            {
                categoryId = model.CategoryId;
                WXMallCategory category = bllMall.Get<WXMallCategory>(string.Format(" AutoID={0}",model.CategoryId));
                if (category!=null)
                {
                    if (category.PreID>0)
                    {
                        categoryId=bllMall.Get<WXMallCategory>(string.Format(" AutoID={0}",category.PreID)).AutoID.ToString();
                    }

                }
            }


        }
    }
}