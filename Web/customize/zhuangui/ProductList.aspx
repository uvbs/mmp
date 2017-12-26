<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.zhuangui.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/productlist.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

    <div class="vue-el">

        <div class="top">
            <div class="top-left">
                <a href="Login.aspx"><img src="images/return.png" class="top-left-img" /></a>
                
            </div>
            <div class="logo">
                <img src="images/logo.png" />
            </div>
            <div class="top-right">
                 <a href="/customize/comeoncloud/Index.aspx?key=MallHome"><img class="top-right-img" src="images/homewhite.png" /></a>
            </div>
        </div>
        <div class="banner">
            <div class="weui-btn weui-btn_mini weui-btn_default" id="btnLoinout" v-on:click="loginOut()">退出</div>
            <img v-bind:src="supplierInfo.image" />

        </div>
        <div class="info">
            <div class="info-left">
                <img class="info-left-logo" v-bind:src="supplierInfo.head_img_url" />
            </div>
            <div class="info-right">
                <div class="info-right-name">{{supplierInfo.company}}</div>
                <div class="info-right-desc"><span v-html="supplierInfo.desc"></span><a v-if="supplierInfo.ex1" class="info-right-link" v-bind:href="supplierInfo.ex1">查看></a></div>
                <div class="info-right-link">
                </div>
            </div>
        </div>

        <div class="weui-tab">
            <div class="weui-navbar">
                <div v-bind:class="['weui-navbar__item', { 'weui-bar__item_on': tag==item.value }]"
                    v-for="item in tagList"
                    v-text="item.text"
                    v-on:click="selectTag(item.value)">
                </div>
            </div>
        </div>


        <div class="product-list">
            <a class="product-item" v-for="item in productData.list" v-on:click="toDetail(item)">

                <div class="product-img">
                    <img v-bind:src="item.img_url" />
                </div>
                <div class="product-title" v-text="item.title"></div>
                <div class="product-price">￥{{item.price}}</div>

            </a>
            <div style="clear:both"></div>
        </div>


        <div class="weui-loadmore" v-show="isLoading">
            <i class="weui-loading"></i>
            <span class="weui-loadmore__tips">正在加载</span>
        </div>


        
        <div class="notice" v-html="supplierInfo.ex3">
        </div>
        <div style="clear:both"></div>
        <div class="weui-tabbar">
            <a href="/customize/comeoncloud/Index.aspx?key=MallHome" class="weui-tabbar__item">
                <span style="display: inline-block; position: relative;">
                    <img src="images/home.png" alt="" class="weui-tabbar__icon">
                </span>
                <p class="weui-tabbar__label">首页</p>
            </a>
            <a href="ProductList.aspx" class="weui-tabbar__item">
                <img src="images/search.png" alt="" class="weui-tabbar__icon">
                <p class="weui-tabbar__label">全部商品</p>
            </a>
            <a href="/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/926025#/shopBasket" class="weui-tabbar__item">
                <span style="display: inline-block; position: relative;">
                    <img src="images/buycar.png" alt="" class="weui-tabbar__icon">
                </span>
                <p class="weui-tabbar__label">购物车</p>
            </a>
            <a href="/customize/comeoncloud/Index.aspx?key=PersonalCenter" class="weui-tabbar__item">
                <img src="images/my.png" alt="" class="weui-tabbar__icon">
                <p class="weui-tabbar__label">个人中心</p>
            </a>
        </div>
    </div>

    <%--        <div class="weui-loadmore weui-loadmore_line" v-if="isLoadAll">
            <span class="weui-loadmore__tips">没有更多了</span>
        </div>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/productlist.js"></script>
</asp:Content>
