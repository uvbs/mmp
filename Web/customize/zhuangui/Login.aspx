<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.zhuangui.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/login.css" rel="stylesheet" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="weui-tab">
        <div class="weui-tab__panel">
            <div class="login-text">
                <img src="images/bgtext.png" />
            </div>
            <div class="login-tip">
                <img src="images/logintip.png" />
            </div>
            <div class="weui-cells weui-cells_form">
                <div class="weui-cell weui-cell_vcode">
                    <div class="weui-cell__bd">
                        <input class="weui-input" type="text" placeholder="请输入学校专属代码" v-model="componyCode" />
                    </div>
                    <div class="weui-cell__ft">
                        <button class="weui-vcode-btn" v-on:click="login()">登录</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="weui-tabbar">
            <a href="/customize/comeoncloud/Index.aspx?key=MallHome" class="weui-tabbar__item">
                <span style="display: inline-block; position: relative;">
                    <img src="images/home.png" alt="" class="weui-tabbar__icon" />
                </span>
                <p class="weui-tabbar__label">首页</p>
            </a>
            <a href="ProductList.aspx" class="weui-tabbar__item">
                <img src="images/search.png" alt="" class="weui-tabbar__icon" />
                <p class="weui-tabbar__label">全部商品</p>
            </a>
            <a href="/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/926025#/shopBasket" class="weui-tabbar__item">
                <span style="display: inline-block; position: relative;">
                    <img src="images/buycar.png" alt="" class="weui-tabbar__icon" />
                </span>
                <p class="weui-tabbar__label">购物车</p>
            </a>
            <a href="/customize/comeoncloud/Index.aspx?key=PersonalCenter" class="weui-tabbar__item">
                <img src="images/my.png" alt="" class="weui-tabbar__icon" />
                <p class="weui-tabbar__label">个人中心</p>
            </a>
        </div>



    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/login.js"></script>
</asp:Content>
