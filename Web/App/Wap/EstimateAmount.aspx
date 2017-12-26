<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="EstimateAmount.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.EstimateAmount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="wrapCommList">
                <div class="pLR10">
                    <table class="wrapCommTitleTable h40">
                        <tr>
                            <td class="txtRight">
                                <div class="txt16" style="line-height: 30px;">
                                    <span v-text="login_user.lockamount + '（账面<%= website.TotalAmountShowName %>）'"></span>
                                </div>
                                <div class="txt16" style="line-height: 30px;">
                                    <span v-text="'= '+ log.sum + '（冻结<%= website.TotalAmountShowName %>）+ '"></span><span v-text="login_user.totalamount+'（可用<%= website.TotalAmountShowName %>）'"></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="wrapCommGroup">
                    <div class="list-title">
                        <table class="wrapCommTitleTable">
                            <tr>
                                <td>
                                    <span class="txt15">冻结<%= website.TotalAmountShowName %></span>
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
                                    <div class="txt15" v-text="log.amount"></div>
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
    <script type="text/javascript">
        var login_user = JSON.parse('<%= new ZentCloud.BLLJIMP.BLLUser().GetLoginUserJsonString() %>');
    </script>
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/EstimateAmount.js?v=2017030101"></script>
</asp:Content>
