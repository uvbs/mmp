<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="RegisterOffLine.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Member.RegisterOffLine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2016121201" rel="stylesheet" />
    <link href="/App/Wap/css/ApplyMember.css?v=2016121201" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
    </svg>
    <div class="wrapApplyMember wrapComm" style="padding: 5px;">
        <div class="wrapContent">
            <div class="title"><%=Request["empty_bill"] =="1"?"添加空单会员":"会员申请" %></div>
            <div class="apply-form">
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2">
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">会员级别：</label>
                                    </div>
                                    <div class="weui_cell__bd"
                                        v-bind:class="[!check.level.ok?'tip':'',check.level.error?'check-error':'']"
                                        v-text="check.level.text"
                                        v-on:click="showLevelDialog()">
                                    </div>
                                    <div class="weui_cell__ft cell-level"
                                        v-if="check.level.has_arrow"
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
                        <td style="width: 50%;">
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">推荐人：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <input class="weui-input"
                                            type="tel"
                                            placeholder="推荐人编号/手机"
                                            v-model="check.spread.spreadid"
                                            maxlength="11"
                                            v-on:blur="checkSpread()" />
                                    </div>
                                    <div class="weui_cell_ft" v-if="check.spread.text">
                                        <div class="cell-tb">
                                            <div class="cell-td cell-spread-check" v-bind:class="[check.spread.ok ? 'check-ok':'check-error']"
                                                v-text="check.spread.text">
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
                                        <label class="weui-label">手机号：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <input class="weui-input"
                                            v-bind:class="[check.phone.ok?'':'check-error']"
                                            type="tel"
                                            placeholder="请输入手机号"
                                            v-model="form.phone"
                                            maxlength="11"
                                            v-on:blur="checkPhone()"
                                            v-on:focus="check.phone.ok=true" />
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
                                        <label class="weui-label">姓名：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <input class="weui-input" type="text" placeholder="请输入姓名" v-model="form.truename" maxlength="20" />
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">身份证号：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <input class="weui-input"
                                            v-bind:class="[check.idcard.ok?'':'check-error']"
                                            type="text"
                                            placeholder="请输入身份证号"
                                            v-model="form.idcard"
                                            maxlength="18"
                                            v-on:blur="checkIdCard()"
                                            v-on:focus="check.idcard.ok=true" />
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <%if(Request["empty_bill"] =="1"){ %>
                    <tr>
                        <td colspan="2">
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">所在地：</label>
                                    </div>
                                    <div class="weui_cell__bd" v-on:click="showAreaDialog(1)">
                                        <div class="cell-tb">
                                            <div class="cell-td area" v-bind:class="[!check.area.ok?'tip':'']"
                                                v-text="check.area.text">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="weui_cell_ft" v-on:click="showAreaDialog(1)">
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
                    <%} else{ %>
                    <tr>
                        <td>
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">充值渠道：</label>
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
                                        <label class="weui-label">所在地：</label>
                                    </div>
                                    <div class="weui_cell__bd" v-on:click="showAreaDialog(1)">
                                        <div class="cell-tb">
                                            <div class="cell-td area" v-bind:class="[!check.area.ok?'tip':'']"
                                                v-text="check.area.text">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="weui_cell_ft" v-on:click="showAreaDialog(1)">
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
                    <%} %>
                    <tr>
                        <td colspan="2">
                            <div class="weui-cells weui-cells_form">
                                <div class="weui-cell">
                                    <div class="weui-cell__hd">
                                        <label class="weui-label">备注：</label>
                                    </div>
                                    <div class="weui_cell__bd">
                                        <div class="txtContent" contenteditable="true" style="width: 100%;line-height: 21px;min-height: 73px;padding: 5px;border: solid #d3d3d3 1px;"></div>
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
                                        <label class="weui-label">资质上传：</label>
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
                                                    <input class="weui-uploader__input" type="file" accept="image/jpeg;image/png;image/gif;" multiple="multiple" v-if="!check.inup"  v-on:change="updateFile($event, index)" />
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
            <div class="apply">
                <a href="javascript:void(0);" class="weui-btn btn-apply" v-on:click="applyOffLine()">确认提交</a>
            </div>

            <div class="weui-skin_android comm-dialog member-level-dialog" v-if="check.level.has_arrow">
                <div class="weui-mask" v-on:click="clearDialog()"></div>
                <div class="weui-actionsheet">
                    <div class="actionsheet-title">
                        会员级别
                    </div>
                    <div class="actionsheet-list">
                        <div class="weui-actionsheet__menu">
                            <div class="weui-actionsheet__cell"
                                v-for="item in level_data"
                                v-text="item.name"
                                v-on:click="selectLevel(item)">
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="weui-skin_android comm-dialog member-area-dialog-1" v-if="province_data.length>0">
                <div class="weui-mask" v-on:click="clearDialog()"></div>
                <div class="weui-actionsheet">
                    <div class="actionsheet-title">
                        选择省份
                    </div>
                    <div class="actionsheet-list">
                        <div class="weui-actionsheet__menu">
                            <div class="weui-actionsheet__cell"
                                v-for="(item,index) in province_data"
                                v-text="item.name"
                                v-on:click="selectArea(1,item, index)">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="weui-skin_android comm-dialog member-area-dialog-2"
                v-if="city_show_data.length >0">
                <div class="weui-mask" v-on:click="clearDialog()"></div>
                <div class="weui-actionsheet">
                    <div class="actionsheet-title">
                        选择城市
                    </div>
                    <div class="actionsheet-list">
                        <div class="weui-actionsheet__menu">
                            <div class="weui-actionsheet__cell"
                                v-for="(item,index) in city_show_data"
                                v-text="item.name"
                                v-on:click="selectArea(2,item, index)">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="weui-skin_android comm-dialog member-area-dialog-3"
                v-if="district_show_data.length >0">
                <div class="weui-mask" v-on:click="clearDialog()"></div>
                <div class="weui-actionsheet">
                    <div class="actionsheet-title">
                        选择区/县
                    </div>
                    <div class="actionsheet-list">
                        <div class="weui-actionsheet__menu">
                            <div class="weui-actionsheet__cell"
                                v-for="(item,index) in district_show_data"
                                v-text="item.name"
                                v-on:click="selectArea(3,item, index)">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="weui-skin_android comm-dialog member-area-dialog-4"
                v-if="town_show_data.length >0">
                <div class="weui-mask" v-on:click="clearDialog()"></div>
                <div class="weui-actionsheet">
                    <div class="actionsheet-title">
                        选择镇/街
                    </div>
                    <div class="actionsheet-list">
                        <div class="weui-actionsheet__menu">
                            <div class="weui-actionsheet__cell"
                                v-for="(item,index) in town_show_data"
                                v-text="item.name"
                                v-on:click="selectArea(4,item, index)">
                            </div>
                        </div>
                    </div>
                </div>
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
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2016121201"></script>
    <script type="text/javascript" src="/App/Wap/js/ApplyMember.js?v=2017032802"></script>
    <%--<script type="text/javascript" src="/Scripts/ajaxImgUpload.js"></script>--%>
</asp:Content>
