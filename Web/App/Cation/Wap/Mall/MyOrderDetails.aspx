<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="MyOrderDetails.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.MyOrderDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">订单详情</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section class="box">
    <div class="orderinfobox">
        <div class="order">
            <div class="orderdata">订单编号:<%=OrderInfo.OrderID %>&nbsp;<%=OrderInfo.InsertDate%>&nbsp;<%=OrderInfo.Status%></div>
            <%
                List<ZentCloud.BLLJIMP.Model.WXMallOrderDetailsInfo> orderDetailList = new ZentCloud.BLLJIMP.BLLMall().GetOrderDetailsList(OrderInfo.OrderID);
                foreach (var orderdetail in orderDetailList)
                {
                    ZentCloud.BLLJIMP.Model.WXMallProductInfo productInfo = new ZentCloud.BLLJIMP.BLLMall().Get<ZentCloud.BLLJIMP.Model.WXMallProductInfo>(string.Format("PID='{0}'", orderdetail.PID));
                    if (productInfo != null)
                    {
                        Response.Write("<div class=\"product\">");
                        Response.Write(string.Format("<img src=\"{0}\" >", productInfo.RecommendImg));
                        Response.Write("<div class=\"info\">");
                        Response.Write(string.Format("<span class=\"text\">{0}</span>", productInfo.PName));
                        Response.Write(string.Format("<span class=\"price\">￥{0}<span class=\"num\">x{1}</span></span>", orderdetail.OrderPrice, orderdetail.TotalCount));
                        Response.Write("</div>");
                        Response.Write("</div>");
                    }

                }
             %>
            <div class="total">共计<span class="totalnum"><%=OrderInfo.ProductCount %></span>件商品,商品金额<span class="totalprice"><%=OrderInfo.Product_Fee %></span>元.物流费用<span class="orangetext"><%=OrderInfo.Transport_Fee %></span>元.总金额<span class="orangetext"><%=OrderInfo.TotalAmount %></span>元
            </div>
        </div>
    </div>
       <div class="deliverytype">
   <%
       Response.Write("<div class=\"checktag\">");
       switch (OrderInfo.DeliveryType)
     {
         case 0://快递

            
             Response.Write("<span class=\"mydeliverytype\">快递</span>");
           
             break;
             
         case 1://上门自取
            
             Response.Write("<span class=\"mydeliverytype\">上门自取</span>");
            
            
             break;

         case 2:
          
             Response.Write("<span class=\"mydeliverytype\">商家承担配送费用</span>");
           
             break;

         default:
           
             Response.Write("<span class=\"mydeliverytype\">无需物流</span>");
            
             break;

     }
     Response.Write("</div>");
     Response.Write("<div class=\"express\" >");
     Response.Write("<div href=\"#\" class=\"addressbox\">");
     Response.Write(string.Format("<span class=\"name\" >收货人:{0}</span>", OrderInfo.Consignee));
     Response.Write(string.Format("<span class=\"phone\">{0}</span>", OrderInfo.Phone));
     Response.Write(string.Format("<span class=\"address\">地址 :{0}</span>", OrderInfo.Address));

     Response.Write("</div>");
     Response.Write("</div>");
     //if (!string.IsNullOrEmpty(OrderInfo.DeliveryStaff))
     //{
     //    var DeliverStaffInfo = new ZentCloud.BLLJIMP.BLLMall().GetSingleWXMallDeliveryStaff(OrderInfo.DeliveryStaff);
     //    if (DeliverStaffInfo != null)
     //    {
     //        Response.Write("<div class=\"shopaddress\">");
     //        Response.Write(string.Format("<div class=\"address current\">配送员：{0}<br/>电话:{1}<span class=\"icon\"></span></div>", DeliverStaffInfo.StaffName, DeliverStaffInfo.StaffPhone));
     //        Response.Write("</div>");

     //    }

     //}
    %>

    </div>
    <div class="backbar">
        <a href="javascript:history.go(-1)" class="back"><span class="icon"></span></a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

</asp:Content>
