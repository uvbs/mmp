<%@ Page Title="线下充值" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="OfflineRecharge.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.OfflineRecharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
        <symbol id="icon-youjiantou" viewBox="0 0 1024 1024">
            <path d="M679.374008 511.753383 280.140305 112.531959c-11.102872-11.090593-11.102872-29.109991 0-40.177048 11.090593-11.109012 29.092595-11.109012 40.188304 0l414.455383 414.450267c2.229784 1.246387 4.973268 0.947582 6.874571 2.843768 6.076392 6.076392 8.508791 14.167674 7.936763 22.103414 0.572028 7.941879-1.860371 16.034185-7.936763 22.097274-1.902326 1.908466-4.650927 1.603521-6.886851 2.856048L320.329633 951.169251c-11.096732 11.084453-29.097712 11.084453-40.188304 0-11.102872-11.114129-11.102872-29.091572 0-40.200584L679.374008 511.753383z"  ></path>
        </symbol>
    </svg>
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="weui-tab">
                <div class="weui-navbar comm-navbar">
                    <div class="weui-navbar__item" v-bind:class="[tab===0?'weui-bar__item_on':'']" v-on:click="tab=0">
                        线下充值
                    </div>
                    <div class="weui-navbar__item" v-bind:class="[tab===1?'weui-bar__item_on':'']" v-on:click="selectTab(1)">
                        提交记录
                    </div>
                </div>
                <div class="weui-tab__panel" v-if="tab===0">
                    <div class="comm-form comm-textarea-form">
                        <div class="weui-cells weui-cells_form">
                            <div class="weui-cell">
                                <div class="weui-cell__hd">
                                    <label class="weui-label">充值金额 <span class="color-red">*</span>：</label>
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
                        <div class="weui-cells weui-cells_form">
                            <div class="weui-cell">
                                <div class="weui-cell__hd">
                                    <label class="weui-label">充值渠道 <span class="color-red">*</span>：</label>
                                </div>
                                <div class="weui_cell__bd"
                                    v-bind:class="[!check.ex1.ok?'tip':'']"
                                    v-text="check.ex1.text"
                                    v-on:click="showEx1Dialog()">
                                </div>
                                <div class="weui_cell__ft"
                                    v-on:click="showEx1Dialog()">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <svg class="icon arrow" aria-hidden="true">
                                                <use xlink:href="#icon-jiantouarrowdown"></use>
                                            </svg>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="weui-cells__title">备注 <span class="color-red">*</span>：</div>
                        <div class="weui-cells weui-cells_form">
                            <div class="weui-cell">
                                <div class="weui-cell__bd">
                                    <div class="txtContent" contenteditable="true" style="width: 96%; line-height: 21px; min-height: 73px; padding: 5px; border: solid #d3d3d3 1px;"></div>
                                </div>
                            </div>
                        </div>
                        <div class="weui-cells__title">充值凭证（照片） <span class="color-red">*</span>：</div>
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
                    <div class="comm divNone" v-bind:class="[tab==0?'divBlock':'']">
                        <a href="javascript:void(0);" class="weui-btn btn-disabled" v-if="!(form.amount>0 && form.ex1 && images.length>0)">充值</a>
                        <a href="javascript:void(0);" class="weui-btn btn-comm" v-if="form.amount>0 && form.ex1 && images.length>0" v-on:click="recharge()">充值</a>
                    </div>
                </div>
                <div class="weui-tab__panel" v-if="tab===1">
                    <div class="wrapCommList">
                        <div class="wrapCommGroup" v-for="group in groupLogs">
                            <div class="list-title">
                                <table class="wrapCommTitleTable">
                                    <tr>
                                        <td>
                                            <span v-text="group.group"></span>
                                        </td>
                                        <td class="txtRight">
                                            <span v-text="group.groupAmount"></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="weui-flex" style="background-color: #FDFDFD;border-top: 1px solid #f1f1f1;">
                                <div class="weui-flex__item txtCenter">
                                    <div class="txt13">时间</div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="txt13">充值渠道</div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="txt13">状态</div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="txt13">金额</div>
                                </div>
                                <div class="weui-flex__item to-detail-icon txtCenter">
                                    <div class="txt13"></div>
                                </div>
                            </div>
                            <div class="weui-flex" v-for="log in group.logs" v-on:click="goDetail(log)">
                                <div class="weui-flex__item txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <div class="txt16" v-text="log.start_d"></div>
                                            <div class="txt13" v-text="log.start_m"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <div class="txt16" v-text="log.ex1"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <div class="txt16"
                                                v-bind:class="[log.status==8 || log.status==10?'color-red':'' ]"
                                                v-text="log.status_s">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="weui-flex__item txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <div class="txt16" v-text="log.amount"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="weui-flex__item to-detail-icon txtCenter">
                                    <div class="cell-tb">
                                        <div class="cell-td">
                                            <svg class="icon" aria-hidden="true">
                                                <use xlink:href="#icon-youjiantou"></use>
                                            </svg>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="wrapCommMore" v-bind:class="[tab===1 && log.list.length < log.total?'divBlock':'']" v-if="log.list.length < log.total">-- 正在加载中 -- </div>
                    <div class="wrapCommMore" v-bind:class="[tab===1 && log.list.length >= log.total?'divBlock':'']" v-if="log.list.length >= log.total">-- 没有更多了 -- </div>
                </div>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
        <div class="weui-skin_android comm-dialog ex1-dialog">
            <div class="weui-mask" v-on:click="clearDialog()"></div>
            <div class="weui-actionsheet">
                <div class="actionsheet-title">
                    充值渠道
                </div>
                <div class="actionsheet-list">
                    <div class="weui-actionsheet__menu">
                        <div class="weui-actionsheet__cell"
                            v-for="item in ex1_list"
                            v-text="item"
                            v-on:click="selectEx1(item)">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/OfflineRecharge.js?v=2017032802"></script>
    <%--<script type="text/javascript" src="/Scripts/ajaxImgUpload.js"></script>--%>
</asp:Content>
