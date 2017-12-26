using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.customize.Shop
{
    public partial class JDPay : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BllPay bllPay = new BllPay();
        /// <summary>
        /// 
        /// </summary>
        BLLMall bllMall = new BLLMall();
        protected void Page_Load(object sender, EventArgs e)
        {


            string
                orderId = Request["order_id"],
                callBackUrl = "",
                notifyUrl = "",
                baseUrl = string.Format("http://{0}", Request.Url.Host);

            if (string.IsNullOrWhiteSpace(orderId))
            {
                Response.Redirect("/error/commonmsg.aspx?msg=订单号未找到");
                Response.End();
                return;
            }
            notifyUrl = baseUrl + "/JDPayNotify/MallNotify.ashx";
            callBackUrl = baseUrl + "/customize/shop/index.aspx?v=1.0&ngroute=/orderList#/orderList";//返回订单列表页面
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {

                Response.Redirect("/error/commonmsg.aspx?msg=订单未找到");
                return;

            }
            if (orderInfo.TotalAmount <= 0)
            {

                Response.Redirect("/error/commonmsg.aspx?msg=支付金额小于等于0，无法创建支付");
                return;
            }
            if (orderInfo.PaymentStatus.Equals(1))
            {
                Response.Redirect("/error/commonmsg.aspx?msg=订单已支付");
                return;
            }
            if (bllMall.Update(orderInfo, " PaymentType=3 ", string.Format(" (OrderID = '{0}' Or ParentOrderId='{0}') ", orderInfo.OrderID)) > 0)
            {
                bool isSuccess = false;
                var payForm = bllPay.CreateJDPayRequestMobile(out isSuccess, orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.OrderUserID, orderInfo.IsNoExpress.ToString(), callBackUrl, notifyUrl);
                if (isSuccess)
                {
                    Response.Write(payForm);
                }
                else
                {
                    Response.Redirect("/error/commonmsg.aspx?msg=创建京东订单失败");
                }
            }



        }
    }
}
