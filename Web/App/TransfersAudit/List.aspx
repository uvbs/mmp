<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.TransfersAudit.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/list.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

    <div class="vue-el">
        <div class="weui-tab">
            <div class="weui-navbar">
                <div v-bind:class="['weui-navbar__item', { 'weui-bar__item_on': status==item.value }]"
                    v-for="item in statusList"
                    v-text="item.text"
                    v-on:click="selectTab(item.value)">
                </div>
            </div>
        </div>

        <div class="list">
            <div class="weui-form-preview" v-for="item in listData.list">
            <div class="weui-form-preview__hd">
                <div class="weui-form-preview__item">
                    <label class="weui-form-preview__label">金额</label>
                    <em class="weui-form-preview__value">¥{{item.amount}}</em>
                </div>
            </div>
            <div class="weui-form-preview__bd">
               <div class="weui-form-preview__item">
                    <label class="weui-form-preview__label">申请时间</label>
                    <span class="weui-form-preview__value">{{item.insert_date}}</span>
                </div>
                 <div class="weui-form-preview__item" v-if="item.status==1">
                    <label class="weui-form-preview__label"><span class="green">已打款</span></label>
                    
                </div>
                <div class="weui-form-preview__item">
                    <label class="weui-form-preview__label">类型</label>
                    <span class="weui-form-preview__value">{{item.type|typeFormart}}</span>
                </div>

                <div class="weui-form-preview__item">
                    <label class="weui-form-preview__label">信息</label>
                    <span class="weui-form-preview__value" v-html="item.tran_info"></span>
                </div>
            </div>
            <div class="weui-form-preview__ft" v-if="item.status==0">
                <a class="weui-form-preview__btn weui-form-preview__btn_primary" href="javascript:" v-on:click="pass(item)">同意打款</a>
            </div>
        </div>

        <div class="weui-loadmore" v-show="isLoading">
            <i class="weui-loading"></i>
            <span class="weui-loadmore__tips">正在加载</span>
        </div>

    </div>

            <div class="weui-loadmore weui-loadmore_line" v-if="isLoadAll">
            <span class="weui-loadmore__tips">没有更多了</span>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/list.js"></script>
</asp:Content>
