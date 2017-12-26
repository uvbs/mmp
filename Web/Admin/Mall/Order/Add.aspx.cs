using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Mall.Order
{
    public partial class Add : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCard = new BLLJIMP.BLLCardCoupon();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 优惠券列表
        /// </summary>
        protected List<CardCoupons> CardCouponList = new List<CardCoupons>();
        /// <summary>
        /// 供应商列表
        /// </summary>
        protected List<UserInfo> SupplierList = new List<UserInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            int totalCount = 0;
           CardCouponList= bllCard.GetCardCouponList("", 1, 10000, out totalCount, "");
           SupplierList = bllUser.GetSupplierList(bllUser.WebsiteOwner, 1, 10000, "","", out totalCount);

           
        }
    }
}