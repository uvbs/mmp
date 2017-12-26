<%@ Page Title="替他人注册" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="HelpApplyMember.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.HelpApplyMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
    <link href="/App/Wap/css/ApplyMember.css?v=20170630" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
    </svg>
    <div class="wrapApplyMember wrapHelpApplyMember">
        <div class="wrapContent">
            <div class="title">替他人注册</div>
            <div class="apply-form">
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
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">手机号：</label>
                        </div>
                        <div class="weui_cell__bd">
                            <input class="weui-input"
                                v-bind:class="[check.phone.ok?'':'check-error']"
                                type="text"
                                placeholder="请输入手机号"
                                v-model="form.phone"
                                maxlength="11"
                                v-on:blur="checkPhone()"
                                v-on:focus="check.phone.ok=true" />
                        </div>
                    </div>
                </div>
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
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">推荐人编号：</label>
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
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell parent-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">支付方式：</label>
                        </div>
                        <div class="weui_cell__bd child-cell">
                             <div class="weui-cells weui-cells_radio">
                                <label class="weui-cell help1  weui-check__label" for="rd1">
                                    <div class="weui-cell__bd" style="flex:1;position:relative;">
                                        <p>余额注册</p>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" name="rdPay" class="weui-check" id="rd1" v-bind:checked="!check.payWay" v-on:click="selectPayWay(false)" />
                                        <span class="weui-icon-checked"></span>
                                    </div>
                                </label>
                                <label class="weui-cell help  weui-check__label" for="rd2">
                                    <div class="weui-cell__bd" style="flex:1;position:relative;">
                                        <p>优惠券注册</p>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" name="rdPay" class="weui-check" id="rd2" v-bind:checked="check.payWay" v-on:click="selectPayWay(true)" />
                                        <span class="weui-icon-checked"></span>
                                    </div>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form" v-if="check.payWay">
                    <div class="weui-cell parent-cell" style="min-height:50px;">
                        <div class="weui-cell__hd">
                            <label class="weui-label">付款方式：</label>
                        </div>
                        <div class="weui_cell__bd child-cell" v-if="cardCoupon.length>0">
                            <div class="weui-cells weui-cells_radio" v-for="(item,index) in cardCoupon">
                                <label class="weui-cell weui-check__label" v-bind:for="'rdm'+index">
                                    <div class="weui-cell__bd" style="flex: 1; position: relative;">
                                        <img class="weui-vcode-img" style="height: 30px;" v-bind:src="item.img_url">
                                        <span style="position: absolute; left: 50px; line-height: 30px;">{{item.cardcoupon_name}}</span>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" class="weui-check" name="rdPayMethodC" v-bind:id="'rdm'+index" v-bind:checked="form.payMethod==item.ex1" v-on:click="selectPayMethod(item.ex1)" />
                                        <i class="weui-icon-checked"></i>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <div class="weui_cell__bd child-cell" v-if="cardCoupon.length<=0">
                            <div class="weui-cells weui-cells_radio">
                                <label class="weui-cell weui-check__label" style="font-size:12px;">
                                    暂无优惠券
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="apply divNone" v-bind:class="[login_user?'divBlock':'']">
                <a href="javascript:void(0);" class="weui-btn btn-apply" v-if="select.diffamount>=0&!check.payWay" v-on:click="apply()">确认提交</a>
                <a href="javascript:void(0);" class="weui-btn weui-btn_warn" v-if="select.diffamount<0&&!check.payWay" v-on:click="goRecharge()">您的<%=website.TotalAmountShowName %>不足，去充值</a>
                <a href="javascript:void(0);" class="weui-btn btn-apply" v-if="check.payWay" v-on:click="applyCoupon()">确认提交</a>

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
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var login_user = JSON.parse('<%= new ZentCloud.BLLJIMP.BLLUser().GetLoginUserJsonString() %>');
        var totalAmountShowName = '<%=website.TotalAmountShowName %>';
    </script>
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/HelpApplyMember.js?v=2017063003"></script>
</asp:Content>
