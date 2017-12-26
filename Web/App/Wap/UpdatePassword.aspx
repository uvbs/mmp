<%@ Page Title="修改密码" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="UpdatePassword.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.UpdatePassword" %>
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
                            <label class="weui-label">旧密码：</label>
                        </div>
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="password" placeholder="请输入旧密码"  maxlength="20" id="txtPwd"/>
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">新密码：</label>
                        </div>
                        <div class="weui-cell__bd">
                             <input class="weui-input" type="password" placeholder="请输入新密码"  maxlength="20" id="txtPwdNew"/>
                        </div>
                    </div>
                </div>
                                 <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">密码：</label>
                        </div>
                        <div class="weui-cell__bd">
                             <input class="weui-input" type="password" placeholder="确认新密码"  maxlength="20" id="txtPwdNewConfirm"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="login">
                <a href="javascript:void(0);" class="weui-btn btn-login" onclick="updatePassword()">修改密码</a>
            </div>
            <div class="order-table">
                <div class="order-cell-left">
                    <span class="to-link" v-on:click="goForgetPayPassword()">忘记支付密码?</span>
                </div>
                <div class="order-cell-right">
                    <span class="to-link" v-on:click="goForgetPassword()">忘记密码?</span>
                </div>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/LoginBinding.js?v=2017030101"></script>
    <script type="text/javascript" src="/App/Wap/js/UpdatePassword.js?v=2017030101"></script>

</asp:Content>
