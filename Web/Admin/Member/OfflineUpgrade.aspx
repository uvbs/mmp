<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="OfflineUpgrade.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Member.OfflineUpgrade" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017032101" rel="stylesheet" />
    <link href="/App/Wap/css/UpgradeMember.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
    </svg>
    <div class="wrapComm wrapUpgradeMember" style="padding: 5px;">
        <div class="wrapContent">
            <div class="title">线下升级</div>
            <div class="title-2">(<span class="color-red">线下升级仅对全额线下支付升级</span>)</div>
            <div class="comm-form upgrade-form comm-textarea-form">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%;">
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">会员手机 <span class="color-red">*</span>：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <input class="weui-input"
                                            type="tel"
                                            placeholder="会员手机"
                                            v-model="check.user.spreadid"
                                            maxlength="11"
                                            v-on:blur="checkUser()" />
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">会员：</label>
                                    </div>
                                    <div class="weui_cell__bd" v-if="check.user.text" v-bind:class="[check.user.ok ? 'check-ok':'check-error']"
                                        v-text="check.user.text">
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                        </td>
                        <td>
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">升级到：</label>
                                    </div>
                                    <div class="weui_cell__bd"
                                        v-bind:class="[!check.tolevel.ok?'tip':'',check.tolevel.error?'check-error':'']"
                                        v-text="check.tolevel.text"
                                        v-on:click="showLevelDialog()">
                                    </div>
                                    <div class="weui_cell__ft cell-level"
                                        v-bind:class="[tolevels.length > 1?'divBlock':'divNone']"
                                        v-if="tolevels.length > 1"
                                        v-on:click="showLevelDialog()">
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
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">备注 <span class="color-red">*</span>：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <div class="txtContent" contenteditable="true" style="width: 100%; line-height: 21px; min-height: 73px; padding: 5px; border: solid #d3d3d3 1px;"></div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">充值凭证<br />（照片） <span class="color-red">*</span>：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <div class="weui-uploader">
                                            <div class="weui-uploader__bd">
                                                <ul class="weui-uploader__files" v-bind:class="[images.length>0?'divBlock':'']">
                                                    <li class="weui-uploader__file"
                                                        v-bind:class="[!item.ok ?'weui-uploader__file_status':'']"
                                                        v-bind:style="{'background-image': item.url? 'url('+item.url+')':''}"
                                                        v-for="(item,index) in images">
                                                        <div class="weui-uploader__file-content" v-if="!item.ok">
                                                            <i class="weui-icon-warn" v-if="item.error"></i>
                                                            <span v-if="!item.error && !item.ok" v-text="item.num"></span><span v-if="!item.error && !item.ok">%</span>
                                                        </div>
                                                        <input class="weui-uploader__input" type="file" accept="image/jpeg;image/png;image/gif;" multiple="multiple" v-if="!check.inup" v-on:change="updateFile($event, index)" />
                                                    </li>
                                                </ul>
                                                <div class="weui-uploader__input-box" v-if="images.length<5">
                                                    <input id="uploaderInput" class="weui-uploader__input" type="file" accept="image/jpeg;image/png;image/gif;" multiple="multiple" v-if="!check.inup" v-on:change="addFile($event)" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="comm divNone" v-bind:class="[ form?'divBlock':'']">
                <a href="javascript:void(0);" class="weui-btn btn-disabled" v-if="!(form.tolevel && form.userid)">提交</a>
                <a href="javascript:void(0);" class="weui-btn btn-comm" v-if="form.tolevel && form.userid" v-on:click="offLineUpgrade()">提交</a>
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
            <div class="weui-skin_android member-level-dialog" v-if="tolevels.length > 1">
                <div class="weui-mask"></div>
                <div class="weui-actionsheet">
                    <div class="actionsheet-title">
                        会员级别
                    </div>
                    <div class="actionsheet-list">
                        <div class="weui-actionsheet__menu">
                            <div class="weui-actionsheet__cell"
                                v-for="item in tolevels"
                                v-text="item.name"
                                v-if=" item.level > check.user.level"
                                v-on:click="selectLevel(item)">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var login_user = {level:10,totalamount:9999999,levelamount:10000};
        var totalAmountShowName = '余额';
        var noPay = true;
    </script>
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032101"></script>
    <script type="text/javascript" src="/App/Wap/js/UpgradeMember.js?v=2017032802"></script>
</asp:Content>
