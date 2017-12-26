<%@ Page Title="支付宝付款" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="MallOrderAliPay.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.Pay.MallOrderAliPay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .head{
            background-color:#1aad19;
            color:#fff;
            padding:10px 6px;
        }
        .bottom{
            padding:0 2%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="txtCenter font14">        
        <%if (orderInfo == null || !string.IsNullOrWhiteSpace(errorMsg))
            { %>
        <div class="font20">
            <br />
            <br />
            <%=errorMsg %>
        </div>

        <%}
            else
            { %>

        
        <div class="head">
            请点击右上角，选择在浏览器中打开完成支付
        </div>
        <br />
        <br />
        <div class="">
            <div>
                订单号 <%=orderInfo.OrderID %>
            </div>
            <br />
            <div class=" font20">
                需付金额 ￥<%=orderInfo.TotalAmount %>
            </div>
            <br />
            <div>
                下单时间 <%=orderInfo.InsertDate.ToString("yyyy-MM-dd hh:mm:ss") %>
            </div>
        </div>
<br />
        <div>成功付款后，请点击“已完成付款”</div>
        <br />
        <div class="bottom">
            <a href="/customize/shop/?v=1.0&ngroute=/orderDetail/<%=orderInfo.OrderID %>#/orderDetail/<%=orderInfo.OrderID %>" class="weui-btn weui-btn_primary">已完成付款</a>
        </div>
        

        <%} %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
</asp:Content>
