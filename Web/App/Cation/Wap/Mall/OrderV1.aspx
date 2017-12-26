<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" CodeBehind="OrderV1.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.OrderV1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">购物车</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section class="box">
    <div class="productlist marginborttom" id="orderlist">
    </div>
    <div class="total">
        共计<span class="totalnum" id="lbltotalnum">0</span>件商品,<span class="totalprice" id="lbltotalprice">0.00</span><span class="orangetext">元</span>
    </div>
    <div class="backbar">
        <a href="index.aspx" class="back"><span class="icon"></span></a>
        <a href="index.aspx" class="btn orange" >返回</a>
        <a href="javascript:" class="btn orange" id="btnSettlement">确认订单</a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script src="/Scripts/wxmall/initv1.js" type="text/javascript"></script>
</asp:Content>
