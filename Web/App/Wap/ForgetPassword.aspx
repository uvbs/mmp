<%@ Page Title="忘记密码" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.ForgetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
    <link href="/App/Wap/css/LoginBinding.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapLogin">
        <div class="wrapContent">
            <div class="logo-table">
                <div class="logo-cell">
                    <img class="logo" src="http://file-cdn.songhebao.com/www/jubit/jubit/image/20170309/B2D620E4BFD249AB962DFFD55BF1F72B.png" />
                </div>
            </div>
            <div class="login-form">
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">手机号码：</label>
                        </div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="tel" placeholder="请输入手机号码" v-model="form.phone" maxlength="11" />
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell cell-vcode">
                        <div class="weui-cell__hd">
                            <label class="weui-label">验证码：</label>
                        </div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="tel" placeholder="短信验证码" v-model="form.vcode" maxlength="6" />
                        </div>
                        <div class="weui-cell__ft">
                            <a href="javascript:void(0);" class="weui-btn weui-btn_mini weui-btn_default btn-vcode" v-if="num<=0" v-on:click="getvcode()">
                                <span>获取</span>
                            </a>
                            <a href="javascript:void(0);" class="weui-btn weui-btn_mini weui-btn_disabled btn-vcode" v-if="num>0">（<span v-text="num">50</span>）
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="login">
                <a href="javascript:void(0);" class="weui-btn btn-login" v-on:click="loginbinding()">重置</a>
            </div>
            <div class="order-table">
                <div class="order-cell-left" v-if="has_reg">
                    <span class="to-link">我要注册</span>
                </div>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript"> var forget_paypwd = false;</script>
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/ForgetPassword.js?v=2017030101"></script>
</asp:Content>
