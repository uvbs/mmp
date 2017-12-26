using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    

    public partial class WXMallProductCompile : System.Web.UI.Page
    {
        public int pid = 0;
        public string webAction = "add";
        BLLMall bllMall = new BLLMall();
        public WXMallProductInfo productModel = new WXMallProductInfo();
        public string headTitle = "添加商品";
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        //public System.Text.StringBuilder sbStores = new System.Text.StringBuilder();
        //属性列表
        protected void Page_Load(object sender, EventArgs e)
        {
            webAction = Request["Action"];
            if (webAction == "edit")
            {
                pid = Convert.ToInt32(Request["pid"]);
                productModel = bllMall.GetProduct(pid.ToString());
                if (productModel == null)
                {
                    Response.End();
                }
                else
                {
                    headTitle = string.Format("{0}", productModel.PName);

                }
            }

            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(bllMall.GetCategoryList(), "AutoID", "PreID", "CategoryName", 0, "ddlcategory", "width:200px", ""));

            //foreach (var item in bll.GetWXMallStoreListByWebSite())
            //{
            //    sbStores.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.StoreName);

            //}
           // propList = bllMall.GetProductPropertyList();



        }
    }

}