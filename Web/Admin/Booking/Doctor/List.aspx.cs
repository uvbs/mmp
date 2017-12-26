using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.Booking.Doctor
{
    public partial class List : System.Web.UI.Page
    {
        /// <summary>
        /// 商城
        /// </summary>
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        /// <summary>
        /// 分类
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {

            string type = Request["type"];
            if (!string.IsNullOrEmpty(type))
            {
                
                //BookingDoctorFuYou //医生预约妇幼
                sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(bll.GetCategoryList().Where(p => p.Type.Equals(type)).ToList(), "AutoID", "PreID", "CategoryName", 0, "ddlCategory", "width:200px", "", "", "全部"));

            }
            else
            {
                //BookingDoctor 医生预约
                sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(bll.GetCategoryList().Where(p => p.Type == "BookingDoctor").ToList(), "AutoID", "PreID", "CategoryName", 0, "ddlCategory", "width:200px", "", "", "全部"));

            }
        }
    }
}