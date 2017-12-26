<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="ShopCar.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.ShopCar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    购物车
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <style>
        .radius6
        {
            margin-top: 20px;
        }
        .order_b_btn
        {
            margin-bottom: 100px;
        }
        
        .green
        {
            background-color: #2897AB;
        }
        .text_box
        {
            height: 28px;
            width: 45px;
        }
        .text_box_1
        {
            width: 40px;
        }
        .text_box_2
        {
            width: 40px;
        }
        .form_a
        {
            padding-left: 45px;
            margin-left: 25%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div id="header">
        <div class="homelogo">
            <a href="">购物车</a></div>
        <div class="gomenu3">
        </div>
    </div>
    <div class="menu_order1">
        <ul id="orderlist">
        </ul>
        <div class="page_padding9">
            <div class="order_s_price">
                共<span id="lbltotalnum">0</span>件商品 <span class="order_s_price_r">商品总金额：<span class="color1"
                    id="lbltotalprice">0</span></span>
            </div>
            <br />
            <hr />
            <div class="order_b_btn">
                <a href="javascript:void(0)" class="form_a green radius6" id="btnSettlement">去结算</a>
            </div>
            <div class="clearfix">
            </div>
        </div>
    </div>
    <!--Page 1 content-->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/tuao.js" type="text/javascript"></script>
        <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "土澳网，精心甄选源自澳洲商品的电商平台",
                desc: "土澳网，精心甄选源自澳洲商品的电商平台",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>/customize/tuao/images/logo.png"
            })
        })
    </script>
</asp:Content>
