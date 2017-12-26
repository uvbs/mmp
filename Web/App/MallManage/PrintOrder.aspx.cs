using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class PrintOrder : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public System.Text.StringBuilder sbPrint = new System.Text.StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(Request["oid"]);
                if (orderInfo==null)
                {
                    Response.End();
                }
                if (!orderInfo.WebsiteOwner.Equals(bllMall.WebsiteOwner))
                {
                  Response.End();

                }
                WebsiteInfo websiteInfo = bllMall.GetWebsiteInfoModel();
                sbPrint.Append("欢迎光临");
                sbPrint.Append("<br/>");
                sbPrint.AppendFormat("{0}", websiteInfo.WebsiteName);
                sbPrint.Append("<br/>");
                sbPrint.Append("<hr/>");
                int totalProductCount = 0;
                foreach (var item in bllMall.GetOrderDetailsList(Request["oid"]))
                {

                    WXMallProductInfo productInfo = bllMall.GetProduct(item.PID);
                    if (productInfo != null)
                    {

                        sbPrint.AppendFormat(" {0}", productInfo.PName);
                        sbPrint.Append("<br/>");
                        sbPrint.AppendFormat("{0} X {1}= {2}元", item.OrderPrice, item.TotalCount, (item.OrderPrice * item.TotalCount));
                        sbPrint.Append("<br/>");
                        sbPrint.Append("<hr/>");
                        totalProductCount += item.TotalCount;

                    }
                }
                sbPrint.AppendFormat("数量: {0}",totalProductCount);
                sbPrint.AppendFormat("<br/>");
                sbPrint.AppendFormat("金额: {0}", orderInfo.Product_Fee);
                sbPrint.AppendFormat("<br/>");
                sbPrint.AppendFormat("配送费用: {0} 元", orderInfo.Transport_Fee);
                sbPrint.AppendFormat("<br/>");
                sbPrint.AppendFormat("合计: {0} 元", orderInfo.TotalAmount);
                sbPrint.Append("<hr/>");
                sbPrint.AppendFormat("订单编号: {0}", orderInfo.OrderID);
                sbPrint.AppendFormat("<br/>");
                sbPrint.AppendFormat("姓名:{0}", orderInfo.Consignee);
                sbPrint.AppendFormat("<br/>");
                sbPrint.AppendFormat("电话:{0}", orderInfo.Phone);
                sbPrint.AppendFormat("<br/>");
                sbPrint.AppendFormat("地址:{0}", orderInfo.Address);
                sbPrint.AppendFormat("<br/>");

                string PayMentType = "";
                switch (orderInfo.PaymentType)
                {
                    case 0:
                        PayMentType = "现金";
                        break;
                    case 1:
                        PayMentType = "支付宝" + (orderInfo.PaymentStatus.Equals(0) ? "[未付]" : "[已付]");
                        break;
                    case 2:
                        PayMentType = "微信支付" + (orderInfo.PaymentStatus.Equals(0) ? "[未付]" : "[已付]");
                        break;
                    default:
                        break;
                }
                sbPrint.AppendFormat("支付方式:{0}", PayMentType);
                sbPrint.AppendFormat("<br/>");
                if (!string.IsNullOrEmpty(orderInfo.DeliveryStaff))
                {
                    sbPrint.AppendFormat("配送员姓名:{0}", orderInfo.DeliveryStaff);
                    sbPrint.AppendFormat("<br/>");
                    var deliverStaffInfo = bllMall.GetDeliveryStaff(orderInfo.DeliveryStaff);
                    if (deliverStaffInfo!=null)
                    {
                        sbPrint.AppendFormat("配送员电话:{0}", deliverStaffInfo.StaffPhone);
                        sbPrint.AppendFormat("<br/>");

                    }

                }
                if (orderInfo.DeliveryTime!=null)
                {
                    sbPrint.AppendFormat("用餐时间:{0}", orderInfo.DeliveryTime);
                    sbPrint.AppendFormat("<br/>");
                }
                sbPrint.AppendFormat("备注:{0}", orderInfo.OrderMemo);
                sbPrint.AppendFormat("<br/>");
                sbPrint.AppendFormat("谢谢惠顾!");
            }
            catch (Exception)
            {

                Response.End();
            }



        }
    }
}