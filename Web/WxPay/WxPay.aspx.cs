using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using Payment.WeiXin;
using System.Xml.Linq;

namespace ZentCloud.JubitIMP.Web.WxPay
{
    public partial class WxPay : System.Web.UI.Page
    {
        BllPay bllPay = new BllPay();
        BllOrder bllOrder = new BllOrder();
        protected string PayString;
        protected void Page_Load(object sender, EventArgs e)
        {
            string orderId = this.Request["OrderId"];
            OrderPay orderPay = bllOrder.GetOrderPay(orderId);
            if (orderPay != null && orderPay.Status == 0)
            {
                PayConfig payConfig = bllPay.GetPayConfig();
                PayString = bllPay.GetBrandWcPayRequest(orderId, orderPay.Total_Fee, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, bllOrder.GetCurrentUserInfo().WXOpenId, this.Request.UserHostAddress, string.Format("http://{0}/Admin/DoPay/DoPayWxNotify.aspx", this.Request.Url.Host), "易劳积分充值" + orderPay.Total_Fee + "元");
            }



        }


    }
}