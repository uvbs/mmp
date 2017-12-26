<%@ Page Title="银行卡" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="BankCardList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.BankCardList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="wrapCommList divNone" v-bind:class="[check.payPwd.ok?'divBlock':'']" v-if="check.payPwd.ok">
                <div class="weui-title">
                    <a href="BankCardAddEdit.aspx" class="weui-btn weui-btn_blue">添加银行卡</a>
                </div>
                <div class="weui-cells">
                    <div class="weui-cell weui-cell_access" v-on:click="goEdit(item)" v-for="item in card.list">
                        <div class="weui-cell__bd">
                            <p v-text="'发卡银行：'+item.BankName"></p>
                            <p v-text="'卡号：'+item.BankAccount"></p>
                            <p v-text="'开户名：'+item.AccountName"></p>
                        </div>
                        <div class="weui-cell__ft">
                            修改
                        </div>
                    </div>
                </div>
            </div>
            <check-pay-password v-on:checkpayok="checkPayPwdOk()" v-if="!check.payPwd.ok"></check-pay-password>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/component/CheckPayPwd/CheckPayPwd.js?v=2017030101"></script>
    <script type="text/javascript" src="/App/Wap/js/BankCardList.js?v=2017030101"></script>
</asp:Content>
