<%@ Page Title="线上充值" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Recharge.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.Recharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="weui-tab">
                <div class="weui-navbar comm-navbar">
                    <div class="weui-navbar__item" v-bind:class="[tab===0?'weui-bar__item_on':'']" v-on:click="tab=0">
                        充值
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===1?'weui-bar__item_on':'']" v-on:click="selectTab(1)">
                       充值记录
                    </div>
                </div>
                <div class="weui-tab__panel pTop20" v-show="tab===0">
                    <div class="comm-form">
                        <div class="weui-cells weui-cells_form">
                            <div class="weui-cell">
                                <div class="weui-cell__hd">
                                    <label class="weui-label">充值金额：</label>
                                </div>
                                <div class="weui_cell__bd">
                                    <input class="weui-input"
                                        type="tel"
                                        placeholder="请输入充值金额"
                                        v-model="form.amount"
                                        maxlength="7"
                                        v-on:keyup.enter="recharge()" />
                                </div>
                            </div>
                        </div>
                        <div class="weui-cells__title">付款方式：</div>
                        <div class="weui-cells weui-cells_checkbox">
                            <label class="weui-cell weui-check__label divNone" for="rd1" v-bind:class="[pay_method.is_wx_pay?'divFlex':'']" v-if="pay_method.is_wx_pay">
                                <div class="weui-cell__bd" style="flex:1;padding: 0px 10px;position:relative;">
                                    <img class="weui-vcode-img" style="height:30px;" src="/img/pay/wx_logo.png">
                                    <span style="position:absolute;left: 60px;line-height: 30px;">微信支付</span>
                                </div>
                                <div class="weui-cell__ft">
                                    <input type="checkbox" class="weui-check" name="checkbox1" id="rd1" v-bind:checked="form.payMethod==1" v-on:click="selectPayMethod(1)" />
                                    <i class="weui-icon-checked"></i>
                                </div>
                            </label>
                            <label class="weui-cell weui-check__label divNone" for="rd2" v-bind:class="[pay_method.is_ali_pay?'divFlex':'']" v-if="pay_method.is_ali_pay">
                                <div class="weui-cell__bd" style="flex:1;padding: 0px 10px;position:relative;">
                                    <img class="weui-vcode-img" style="height:30px;" src="/img/pay/zfb_logo.png">
                                    <span style="position:absolute;left:60px;line-height: 30px;">支付宝</span>
                                </div>
                                <div class="weui-cell__ft">
                                    <input type="checkbox" class="weui-check" name="checkbox1" id="rd2" v-bind:checked="form.payMethod==2" v-on:click="selectPayMethod(2)" />
                                    <i class="weui-icon-checked"></i>
                                </div>
                            </label>
                            <label class="weui-cell weui-check__label divNone" for="rd3" v-bind:class="[pay_method.is_jd_pay?'divFlex':'']" v-if="pay_method.is_jd_pay">
                                <div class="weui-cell__bd" style="flex:1;padding: 0px 10px;position:relative;">
                                    <img class="weui-vcode-img" style="height:30px;" src="http://file-cdn.songhebao.com/img/pay/jdpay_logo.png">
                                    <span style="position:absolute;left:60px;line-height: 30px;">京东支付(网银在线)</span>
                                </div>
                                <div class="weui-cell__ft">
                                    <input type="checkbox" class="weui-check" name="checkbox1" id="rd3" v-bind:checked="form.payMethod==3" v-on:click="selectPayMethod(3)" />
                                    <i class="weui-icon-checked"></i>
                                </div>
                            </label>
                        </div>
                    </div>
                    <div class="comm defhide">
                        <a href="javascript:void(0);" class="weui-btn btn-disabled" v-if="form.amount<=0">充值</a>
                        <a href="javascript:void(0);" class="weui-btn btn-comm" v-if="form.amount>0" v-on:click="recharge()">充值</a>
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
                                        <td class="tdTime txtCenter tbd-b">
                                            <div class="txt15" v-text="log.start_d"></div>
                                            <div class="txt13" v-text="log.start_m"></div>
                                        </td>
                                        <td class="txtCenter tbd-b">
                                            <div class="txt15" v-bind:class="[log.score<0?'color-red':'']" v-text="log.score_event"></div>
                                        </td>
                                        <td class="tdAmount txtCenter tbd-b pRight10">
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
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/Recharge.js?v=2017032801"></script>
    <script src="//static-files.socialcrmyun.com/Scripts/wxshare/wxshare1.1.0/wxshare.js"></script>
</asp:Content>
