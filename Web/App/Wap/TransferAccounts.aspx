<%@ Page Title="转账" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="TransferAccounts.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.TransferAccounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="weui-tab divNone" v-bind:class="[check.payPwd.ok?'divBlock':'']" v-if="check.payPwd.ok">
                <div class="weui-navbar comm-navbar">
                    <div class="weui-navbar__item" v-bind:class="[tab===0?'weui-bar__item_on':'']" v-on:click="tab=0">
                        转账
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===1?'weui-bar__item_on':'']" v-on:click="selectTab(1)">
                        转账记录
                    </div>
                </div>
                <div class="weui-tab__panel" v-show="tab===0">
                    <div class="comm-form comm-textarea-form">
                        <div class="weui-cells weui-cells_form">
                            <div class="weui-cell">
                                <div class="weui-cell__hd">
                                    <label class="weui-label">会员编号：</label>
                                </div>
                                <div class="weui_cell__bd">
                                    <input class="weui-input"
                                        type="number"
                                        placeholder="会员编号/手机"
                                        v-model="check.spread.spreadid"
                                        maxlength="11"
                                        v-on:blur="checkSpread()" />
                                </div>
                            </div>
                        </div>
                        <div class="weui-cells weui-cells_form">
                            <div class="weui-cell">
                                <div class="weui-cell__hd">
                                    <label class="weui-label">会员姓名：</label>
                                </div>
                                <div class="weui_cell__bd">
                                    <div class="cell-spread-check" 
                                        v-bind:class="[check.spread.ok ? 'check-ok':'check-error']"
                                        v-text="check.spread.text">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="weui-cells weui-cells_form">
                            <div class="weui-cell">
                                <div class="weui-cell__hd">
                                    <label class="weui-label">转账金额：</label>
                                </div>
                                <div class="weui_cell__bd">
                                    <input class="weui-input"
                                        type="number"
                                        placeholder="请输入充值金额"
                                        v-model="form.amount"
                                        maxlength="7"
                                        v-on:keyup.enter="transfer()" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="comm divNone" v-bind:class="[tab==0?'divBlock':'']">
                        <a href="javascript:void(0);" class="weui-btn btn-disabled" v-if="!(form.amount>0 && form.spreadid)">转账</a>
                        <a href="javascript:void(0);" class="weui-btn btn-comm" v-if="form.amount>0 && form.spreadid" v-on:click="transfer()">转账</a>
                    </div>
                </div>
                <div class="weui-tab__panel" v-show="tab===1">
                    <div class="wrapCommList">
                        <div class="pLR10" v-if="groupLogs.length>0">
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
                        <div class="wrapCommGroup" v-for="group in groupLogs">
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
                                        <td class="txtCenter tbd-b">
                                            <div class="txt15" v-text="log.start_d"></div>
                                            <div class="txt13" v-text="log.start_m"></div>
                                        </td>
                                        <td class="txtCenter tbd-b">
                                            <div class="txt13" v-text="log.addnote"></div>
                                        </td>
                                        <td class="txtCenter tbd-b">
                                            <div class="txt15" v-bind:class="[log.score<0?'color-red':'']" v-text="log.score"></div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="wrapCommMore" v-bind:class="[tab===1 && log.list.length < log.total?'divBlock':'']" v-if="log.list.length < log.total"> -- 正在加载中 -- </div>
                    <div class="wrapCommMore" v-bind:class="[tab===1 && log.list.length >= log.total?'divBlock':'']" v-if="log.list.length >= log.total"> -- 没有更多了 -- </div>
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
    <script type="text/javascript" src="/App/Wap/js/TransferAccounts.js?v=2017030101"></script>
</asp:Content>
