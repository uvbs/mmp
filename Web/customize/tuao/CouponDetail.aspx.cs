using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.customize.tuao
{
    public partial class CouponDetail : TuAoBase
    {
        ZentCloud.BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 优惠券
        /// </summary>
        public Coupon Model = new Coupon();
        protected void Page_Load(object sender, EventArgs e)
        {
            Model=bllCardCoupon.Get<Coupon>(string.Format(" AutoId={0} and CreateUserId='{1}'",Request["id"],currentUserInfo.UserID));
            if (Model==null)
            {
                Response.Write("卡券无效");
                Response.End();
            }

        }
    }
}