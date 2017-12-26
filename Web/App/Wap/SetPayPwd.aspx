<%@ Page Title="设置支付密码" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="SetPayPwd.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.SetPayPwd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="logo-table">
                <div class="logo-cell">
                    <img class="logo" src="http://file-cdn.songhebao.com/www/jubit/jubit/image/20170309/B2D620E4BFD249AB962DFFD55BF1F72B.png" />
                </div>
            </div>
            <div class="comm-form">
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
            <div class="comm divNone" v-bind:class="[form?'divBlock':'']">
                <a href="javascript:void(0);" class="weui-btn btn-disabled" v-if="!form.pay_pwd || form.pay_pwd.length<6">设置支付密码</a>
                <a href="javascript:void(0);" class="weui-btn btn-comm" v-if="form.pay_pwd && form.pay_pwd.length>=6" v-on:click="setPwd()">设置支付密码</a>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/SetPayPwd.js?v=2017030101"></script>
</asp:Content>
