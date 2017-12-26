<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.TransfersAudit.Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/detail.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="vue-el">
            <div class="weui-form-preview">
            <div class="weui-form-preview__hd">
                <div class="weui-form-preview__item">
                    <label class="weui-form-preview__label">金额</label>
                    <em class="weui-form-preview__value">¥{{data.amount}}</em>
                </div>
            </div>
            <div class="weui-form-preview__bd">
                 <div class="weui-form-preview__item" v-if="data.status==1">
                    <label class="weui-form-preview__label"><span class="green">已打款</span></label>
                    
                </div>
                <div class="weui-form-preview__item">
                    <label class="weui-form-preview__label">类型</label>
                    <span class="weui-form-preview__value">{{data.type|typeFormart}}</span>
                </div>

                <div class="weui-form-preview__item">
                    <label class="weui-form-preview__label">信息</label>
                    <span class="weui-form-preview__value" v-html="data.tran_info"></span>
                </div>
            </div>
            <div class="weui-form-preview__ft" v-if="data.status==0">
                <a class="weui-form-preview__btn weui-form-preview__btn_primary" href="javascript:" v-on:click="pass(data)">同意打款</a>
            </div>
        </div>

       
        </div>
   

   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/detail.js"></script>
</asp:Content>
