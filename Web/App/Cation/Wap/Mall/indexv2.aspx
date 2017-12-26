<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="indexv2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.indexv2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">首页</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section class="box">
     <%=sbCategory.ToString()%>
    <ul class="mainlist food_mall" id="productList">

    </ul>
    <div class="total2">
        共计<span class="totalnum" id="lbltotalnum">0</span>件商品,<span class="totalprice" id="lbltotalprice">0.0</span><span class="orangetext">元</span>
    </div>
    <div class="toolbar">
        <a href="javascript:void(0)" class="left current"><span class="homeicon"></span><br/>首页</a>
        <a id="btnSettlement" class="btn orange gary2">立刻下单</a>
        <a href="MyCenter.aspx" class="right"><span class="mycenter"></span><br/>个人中心</a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
     <script type="text/javascript">
     var categoryid = "<%=Request["cid"]%>";    
    </script>
<script src="/Scripts/wxmall/initv2.js" type="text/javascript"></script>
    <script src="/Scripts/wxmall/quo.js" type="text/javascript"></script>
    <script src="/Scripts/wxmall/touchkind.js" type="text/javascript"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "<%=currWebSiteInfo.WXMallName%>",
                desc: "<%=currWebSiteInfo.WXMallName%>",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%><%=currWebSiteInfo.WXMallBannerImage%>"
            })
        })
    </script>
</asp:Content>

