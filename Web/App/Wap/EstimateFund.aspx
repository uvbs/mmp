<%@ Page Title="账面公积金" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="EstimateFund.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.EstimateFund" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="wrapCommList">
                <div class="wrapCommGroup">
                    <div class="list-title">
                        <table class="wrapCommTitleTable">
                            <tr>
                                <td>
                                    <span class="txt15">账面公积金</span>
                                </td>
                                <td class="txtRight">
                                    <span class="txt15" v-text="log.sum"></span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table class="wrapCommListTable" cellspacing="0" cellpadding="0">
                        <tbody v-for="log in log.list">
                            <tr>
                                <td class="tdTime txtCenter" v-bind:class="['tbd-b']">
                                    <div class="txt15" v-text="log.start_d"></div>
                                    <div class="txt13" v-text="log.start_m"></div>
                                </td>
                                <td class="txtLeft" v-bind:class="['tbd-b']">
                                    <div class="txt13" v-text="log.desc"></div>
                                </td>
                                <td class="tdAmount txtRight pRight10" v-bind:class="['tbd-b']">
                                    <div class="txt15" v-text="log.deduct_amount"></div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="wrapCommMore" v-bind:class="[log.list && log.list.length < log.total?'divBlock':'']" v-if="log.list && log.list.length < log.total">-- 正在加载中 -- </div>
            <div class="wrapCommMore" v-bind:class="[log.list && log.list.length >= log.total?'divBlock':'']" v-if="log.list && log.list.length >= log.total">-- 没有更多了 -- </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/EstimateFund.js?v=20170605"></script>
</asp:Content>
