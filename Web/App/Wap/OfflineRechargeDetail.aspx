<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="OfflineRechargeDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.OfflineRechargeDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="comm-form comm-textarea-form">
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">充值金额：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.amount">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">充值渠道：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.ex1">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">提交时间：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.start">
                        </div>
                    </div>
                </div>
                <div class="weui-cells__title">备注：</div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <div class="txtContent" v-html="form.content"></div>
                        </div>
                    </div>
                </div>
                <div class="weui-cells__title">充值凭证：</div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <img v-bind:src="file" style="width:100%; margin:5px 0px 0px;" v-if="form.files" v-for=" file in form.files" />
                        </div>
                    </div>
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
    <script type="text/javascript" src="/App/Wap/js/FlowDetail.js?v=2017030101"></script>
</asp:Content>
