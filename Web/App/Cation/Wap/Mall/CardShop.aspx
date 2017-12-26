<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="CardShop.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.CardShop" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">分店</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<section class="box">

   <%foreach (var item in new ZentCloud.BLLJIMP.BLLMall().GetWXMallStoreListByWebSite())
     {
       Response.Write("<div class=\"m_addressbox\">");
       Response.Write(string.Format("<span class=\"name\"><span class=\"nameinfo\">{0}</span></span>",item.StoreName));
       Response.Write(string.Format("<span class=\"address\">地址 : <span class=\"addressinfo\">{0}</span></span>",item.StoreAddress));
       Response.Write("</div>");
         
     } %>
    <div class="m_cardshopback">
        <a href="Card.aspx" class="btn orange">返回</a>
    </div>

    <div class="backbar">
        <a href="Card.aspx" class="back"><span class="icon"></span></a>
        
    </div>
</section>
</asp:Content>

