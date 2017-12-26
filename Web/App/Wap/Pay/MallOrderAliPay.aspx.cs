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
    public partial class MallOrderAliPay : System.Web.UI.Page
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();

        public bool isWeiXinBrowser = false;
        public bool IsMobile = false;
        public WXMallOrderInfo orderInfo = null;
        public string errorMsg = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string orderId = Request["order_id"];

            if (string.IsNullOrWhiteSpace(orderId))
            {
                errorMsg = "订单号未找到";
                return;
            }

            orderInfo = bllMall.GetOrderInfo(orderId);

            isWeiXinBrowser = bllPay.IsWeiXinBrowser;
            IsMobile = bllPay.IsMobile;

            if (orderInfo == null)
            {
                errorMsg = "订单未找到";
                return;
            }
            if (bllPay.IsWeiXinBrowser)
            {
                return;
            }
            if (!bllPay.IsMobile)
            {
                errorMsg = "请用手机浏览器访问";
                return;
            }
            if (orderInfo.PaymentStatus == 1)
            {
                errorMsg = "订单已经支付";
                return;
            }

            if (orderInfo.Status != "待付款")
            {
                errorMsg = "订单不在待付款状态";
                return;
            }

            //更改支付方式
            bllMall.Update(orderInfo, " PaymentType=1 ", string.Format(" (OrderID = '{0}' Or ParentOrderId='{0}')  ", orderInfo.OrderID));

            PayConfig payConfig = bllPay.GetPayConfig();
            if (payConfig == null)
            {
                errorMsg = "商户还未配置支付宝付款";
                return;
            }
            if ((string.IsNullOrEmpty(payConfig.Seller_Account_Name)) || (string.IsNullOrEmpty(payConfig.Partner)) || (string.IsNullOrEmpty(payConfig.PartnerKey)))
            {
                errorMsg = "商户还未配置支付宝付款";
                return;
            }

            string baseUrl = string.Format("http://{0}", Request.Url.Host);
            string notifyUrl = baseUrl + "/Alipay/MallNotifyUrlV2.aspx";
            var payForm = bllPay.GetAliPayRequestMobile(orderInfo.OrderID, (double)orderInfo.TotalAmount, payConfig.Seller_Account_Name, payConfig.Partner, payConfig.PartnerKey, notifyUrl);

            Response.Write(payForm);
        }
    }
}