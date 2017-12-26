using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Components.Coupon.Mobile.BeachHoney
{
    public partial class Index : System.Web.UI.Page
    {
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!bllCardCoupon.IsLogin)
            {
                Response.Write("请用微信打开");
                Response.End();
            }
        }
    }
}