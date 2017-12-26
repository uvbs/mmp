using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Wap.Pay
{
    public partial class JDPay : System.Web.UI.Page
    {
        BllPay bllPay = new BllPay();
        BllOrder bllOrder = new BllOrder();
        protected void Page_Load(object sender, EventArgs e)
        {
            string
                orderId = Request["order_id"],
                callBackUrl = "",
                notifyUrl = "",
                websiteOwner = bllOrder.WebsiteOwner,
                baseUrl = string.Format("http://{0}", Request.Url.Authority);
            if (string.IsNullOrWhiteSpace(orderId))
            {
                Response.Redirect("/error/commonmsg.aspx?msg=订单号未找到");
                Response.End();
                return;
            }
            notifyUrl = baseUrl + "/JDPayNotify/ShMemberNotifyUrl.ashx";
            callBackUrl = baseUrl + "/customize/comeoncloud/Index.aspx?key=PersonalCenter";//返回订单列表页面
            var orderInfo = bllOrder.GetOrderPay(orderId, websiteOwner: websiteOwner, payType: 2);
            if (orderInfo == null)
            {
                Response.Redirect("/error/commonmsg.aspx?msg=订单未找到");
                return;
            }
            if (orderInfo.Total_Fee <= 0)
            {
                Response.Redirect("/error/commonmsg.aspx?msg=支付金额小于等于0，无法创建支付");
                return;
            }
            if (orderInfo.Status.Equals(1))
            {
                Response.Redirect("/error/commonmsg.aspx?msg=订单已支付");
                return;
            }
            try
            {
                bool isSuccess = false;
                var payForm = bllPay.CreateJDPayRequestMobile(out isSuccess, orderInfo.OrderId, orderInfo.Total_Fee, orderInfo.UserId, "1", callBackUrl, notifyUrl,"会员交易订单");
                Response.Write(payForm);
            }
            catch (Exception)
            {
                Response.Redirect("/error/commonmsg.aspx?msg=创建京东表单失败");
            }
        }
    }
}