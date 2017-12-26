using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Wap.Pay
{
    public partial class Alipay : System.Web.UI.Page
    {
        BllPay bllPay = new BllPay();
        BllOrder bllOrder = new BllOrder();
        protected OrderPay orderPay;
        protected string errorMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string cur_order_id = this.Request["order_id"];
            if (!bllPay.IsMobile)
            {
                errorMsg = "请用手机浏览器访问";
                return;
            }
            if (string.IsNullOrWhiteSpace(cur_order_id))
            {
                errorMsg = "订单号未找到";
                return;
            }
            orderPay = bllOrder.GetOrderPay(cur_order_id, websiteOwner: bllOrder.WebsiteOwner, payType: 1);
            if (orderPay == null)
            {
                errorMsg = "订单未找到";
                return;
            }
            if (orderPay.Status == 1)
            {
                errorMsg = "订单已经付款";
                //formString = "<div style=\"height:100%;color:red;font-size:24px;text-algin:center;\">订单已经付款</div>";
                return;
            }
            if (bllPay.IsWeiXinBrowser)
            {
                return;
            }

            try
            {
                PayConfig payConfig = bllPay.GetPayConfig();
                string baseUrl = string.Format("http://{0}", this.Request.Url.Authority);
                string notifyUrl = baseUrl + "/Alipay/ShMemberNotifyUrl.aspx";
                string formString = bllPay.GetAliPayRequestMobile(orderPay.OrderId, (double)orderPay.Total_Fee,
                    payConfig.Seller_Account_Name, payConfig.Partner, payConfig.PartnerKey, notifyUrl);
                Response.Write(formString);
            }
            catch (Exception ex)
            {
                errorMsg = "支付页生成失败";
            }
        }
    }
}