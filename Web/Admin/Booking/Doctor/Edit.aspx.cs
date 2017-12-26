using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Admin.Booking.Doctor
{
    public partial class Edit : System.Web.UI.Page
    {
        /// <summary>
        /// 医生编号
        /// </summary>
        public int id = 0;
        /// <summary>
        /// 
        /// </summary>
        public string webAction = "add";
        /// <summary>
        /// 商城
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 医生信息
        /// </summary>
        public WXMallProductInfo productModel = new WXMallProductInfo();
        /// <summary>
        /// 
        /// </summary>
        public string headTitle = "添加";
        /// <summary>
        /// 分类
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                webAction = "edit"; 
            }
            if (webAction == "edit")
            {
                id = Convert.ToInt32(Request["id"]);
                productModel = bllMall.GetProduct(id.ToString());
                if (productModel == null)
                {
                    Response.End();
                }
                else
                {
                    headTitle = string.Format("{0}", productModel.PName);

                }
            }



            string type = Request["type"];
            if (!string.IsNullOrEmpty(type))
            {

                //BookingDoctorFuYou //医生预约妇幼
                sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(bllMall.GetCategoryList().Where(p => p.Type.Equals(type)).ToList(), "AutoID", "PreID", "CategoryName", 0, "ddlCategory", "width:200px", "", "", "全部"));

            }
            else
            {
                //BookingDoctor 医生预约
                sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(bllMall.GetCategoryList().Where(p => p.Type == "BookingDoctor").ToList(), "AutoID", "PreID", "CategoryName", 0, "ddlCategory", "width:200px", "", "", "全部"));

            }




        }
    }
}