<%@ Page Title="票据提交" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="ApplyPerformanceReward.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.ApplyPerformanceReward" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
    </svg>
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="comm-form" v-if="performance">
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">管理业绩：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="performance.performance">
                        </div>
                    </div>
                </div>
                <div class="performance" v-if="performance.rate >0">
                    <div class="weui-cells weui-cells_form">
                        <div class="weui-cell">
                            <div class="weui-cell__hd">
                                <label class="weui-label">姓名：</label>
                            </div>
                            <div class="weui_cell__bd" v-text="performance.member">
                            </div>
                        </div>
                    </div>
                    <div class="weui-cells weui-cells_form">
                        <div class="weui-cell">
                            <div class="weui-cell__hd">
                                <label class="weui-label">奖金比例：</label>
                            </div>
                            <div class="weui_cell__bd" v-text="performance.rate+'%'">
                            </div>
                        </div>
                    </div>
                    <div class="weui-cells weui-cells_form">
                        <div class="weui-cell">
                            <div class="weui-cell__hd">
                                <label class="weui-label">管理奖金：</label>
                            </div>
                            <div class="weui_cell__bd" v-text="performance.total_reward">
                            </div>
                        </div>
                    </div>
                    <div class="weui-cells weui-cells_form">
                        <div class="weui-cell">
                            <div class="weui-cell__hd">
                                <label class="weui-label">其他扣款：</label>
                            </div>
                            <div class="weui_cell__bd" v-text="performance.child_reward">
                            </div>
                        </div>
                    </div>
                    <div class="weui-cells weui-cells_form">
                        <div class="weui-cell">
                            <div class="weui-cell__hd">
                                <label class="weui-label">公积金：</label>
                            </div>
                            <div class="weui_cell__bd" v-text="performance.fund">
                            </div>
                        </div>
                    </div>
                    <div class="weui-cells weui-cells_form">
                        <div class="weui-cell">
                            <div class="weui-cell__hd">
                                <label class="weui-label">开票金额：</label>
                            </div>
                            <div class="weui_cell__bd" v-text="performance.amount">
                            </div>
                        </div>
                    </div>
                </div>
                <%--<div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">银行卡：</label>
                        </div>
                        <div class="weui_cell__bd"
                            v-bind:class="[!check.card.ok?'tip':'']"
                            v-html="check.card.text"
                            v-on:click="showCardDialog()">
                        </div>
                        <div class="weui_cell__ft cell-level"
                            v-on:click="showCardDialog()">
                            <div class="cell-tb">
                                <div class="cell-td">
                                    <svg class="icon arrow" aria-hidden="true">
                                        <use xlink:href="#icon-jiantouarrowdown"></use>
                                    </svg>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <div class="weui-cells__title">票据<span class="color-red">*</span>：</div>
                <div class="weui-cells weui-cells_form mTop10">
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <div class="weui-uploader">
                                <div class="weui-uploader__bd">
                                    <ul class="weui-uploader__files" v-bind:class="[images.length>0?'divBlock':'']">
                                        <li class="weui-uploader__file"
                                            v-bind:class="[!item.ok ?'weui-uploader__file_status':'']"
                                            v-bind:style="{'background-image': item.url? 'url('+item.url+')':''}"
                                            v-for="(item,index) in images">
                                            <div class="weui-uploader__file-content" v-if="!item.ok">
                                                <i class="weui-icon-warn" v-if="item.error"></i>
                                                <span v-if="!item.error" v-text="item.progress"></span>
                                            </div>
                                            <input class="weui-uploader__input" type="file" accept="image/jpeg;image/png;image/gif;" multiple="multiple" v-if="!check.inup" v-on:change="updateFile($event, index)" />
                                        </li>
                                    </ul>
                                    <div class="weui-uploader__input-box">
                                        <input id="uploaderInput" class="weui-uploader__input" type="file" accept="image/jpeg;image/png;image/gif;" multiple="multiple" v-if="!check.inup" v-on:change="addFile($event)" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="comm divNone"  v-bind:class="[ performance.reward >0 ?'divBlock':'']" v-if="performance.reward >0">
                <a href="javascript:void(0);" class="weui-btn btn-comm" v-on:click="applyPerformanceReward()">提交</a>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
        <%--<div class="weui-skin_android comm-dialog card-dialog" v-if="cards.length>0">
            <div class="weui-mask" v-on:click="clearDialog()"></div>
            <div class="weui-actionsheet">
                <div class="actionsheet-title">
                    银行卡
                </div>
                <div class="actionsheet-list">
                    <div class="weui-actionsheet__menu">
                        <div class="weui-actionsheet__cell"
                            v-for="item in cards"
                            v-html="item.Text"
                            v-on:click="selectCard(item)">
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/ApplyPerformanceReward.js?v=20170628"></script>
</asp:Content>
