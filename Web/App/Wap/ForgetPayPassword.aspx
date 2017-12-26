<%@ Page Title="忘记支付密码" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="ForgetPayPassword.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.ForgetPayPassword" %>
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
                        <div class="weui-cell__bd" v-text="form.phone">
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
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">支付密码：</label>
                        </div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="tel" placeholder="请设置6-20位支付密码" v-model="form.pay_pwd" maxlength="20" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="login divNone" v-bind:class="[form?'divBlock':'']">
                <a href="javascript:void(0);" class="weui-btn btn-login" v-if="form.pay_pwd && form.pay_pwd.length>=6" v-on:click="setPayPassword()">设置支付密码</a>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript"> var forget_paypwd = true;</script>
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/ForgetPassword.js?v=2017030101"></script>
</asp:Content>
