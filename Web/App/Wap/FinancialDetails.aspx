<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="FinancialDetails.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.FinancialDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="weui-tab divNone" v-bind:class="[hideTab?'pTop0':'',check.payPwd.ok?'divBlock':'']" v-if="check.payPwd.ok">
                <div class="divNone" v-bind:class="[hideTab?'weui-navbar comm-navbar':'weui-navbar comm-navbar divFlex']">
                    <div class="weui-navbar__item" v-bind:class="[tab===0?'weui-bar__item_on':'']" v-on:click="selectTab(0)">
                        全部
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===1?'weui-bar__item_on':'']" v-on:click="selectTab(1)">
                        分佣
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===2?'weui-bar__item_on':'']" v-on:click="selectTab(2)">
                        提现
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===3?'weui-bar__item_on':'']" v-on:click="selectTab(3)">
                        转账
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===4?'weui-bar__item_on':'']" v-on:click="selectTab(4)">
                        充值
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===5?'weui-bar__item_on':'']" v-on:click="selectTab(5)">
                        其他
                    </div>
                </div>
                <div class="weui-tab__panel">
                    <div class="wrapCommList">
                        <div class="pLR10" v-if="log.groupLogs && log.groupLogs.length>0">
                            <table class="wrapCommTitleTable h40">
                                <tr>
                                    <td>
                                        <span class="txt16" v-text="'总额'"></span>
                                    </td>
                                    <td class="txtRight">
                                        <span class="txt16" v-text="log.sum"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="wrapCommGroup" v-for="group in log.groupLogs">
                            <div class="list-title">
                                <table class="wrapCommTitleTable">
                                    <tr>
                                        <td>
                                            <span class="txt15" v-text="group.group"></span>
                                        </td>
                                        <td class="txtRight">
                                            <span class="txt15" v-text="group.groupAmount"></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="wrapCommListTable" cellspacing="0" cellpadding="0">
                                <tbody v-for="log in group.logs">
                                    <tr>
                                        <td class="txtCenter" v-bind:class="['tbd-b']" style="width: 60px;">
                                            <div class="txt15" v-text="log.start_d"></div>
                                            <div class="txt13" v-text="log.start_m"></div>
                                        </td>
                                        <td class="txtCenter" v-bind:class="['tbd-b']">
                                            <div class="txt13" v-text="log.addnote"></div>
                                        </td>
                                        <td class="txtRight" v-bind:class="['tbd-b']" style="width: 65px;">
                                            <div class="txt14" v-bind:class="[log.score<0?'color-red':'']" v-text="log.score"></div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="wrapCommMore" v-bind:class="[log.list && log.list.length < log.total?'divBlock':'']" v-if="log.list && log.list.length < log.total">-- 正在加载中 -- </div>
                    <div class="wrapCommMore" v-bind:class="[log.list && log.list.length >= log.total?'divBlock':'']" v-if="log.list && log.list.length >= log.total">-- 没有更多了 -- </div>
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
    <script type="text/javascript" src="/App/Wap/js/FinancialDetails.js?v=2017030101"></script>
</asp:Content>
