<%@ Page Title="会员申请" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="ApplyMember.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.ApplyMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
    <link href="/App/Wap/css/ApplyMember.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
    </svg>
    <div class="wrapComm wrapApplyMember">
        <div class="wrapContent">
            <div class="title">会员申请</div>
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
                                type="tel"
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
                                type="number"
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
                            <label class="weui-label">归属地：</label>
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
                            <label class="weui-label">付款方式：</label>
                        </div>
                        <div class="weui_cell__bd child-cell">
                            <div class="weui-cells weui-cells_radio">
                                <label class="weui-cell weui-check__label divNone" for="rdm1" v-bind:class="[pay_method.is_wx_pay?'divFlex':'']" v-if="pay_method.is_wx_pay">
                                    <div class="weui-cell__bd" style="flex:1;position:relative;">
                                        <img class="weui-vcode-img" style="height:30px;" src="/img/pay/wx_logo.png">
                                        <span style="position:absolute;left: 50px;line-height: 30px;">微信支付</span>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" class="weui-check" name="rdPayMethod" id="rdm1" v-bind:checked="form.payMethod==1" v-on:click="selectPayMethod(1)" />
                                        <i class="weui-icon-checked"></i>
                                    </div>
                                </label>
                                <label class="weui-cell weui-check__label divNone" for="rdm2" v-bind:class="[pay_method.is_ali_pay?'divFlex':'']" v-if="pay_method.is_ali_pay">
                                    <div class="weui-cell__bd" style="flex:1;position:relative;">
                                        <img class="weui-vcode-img" style="height:30px;" src="/img/pay/zfb_logo.png">
                                        <span style="position:absolute;left:50px;line-height: 30px;">支付宝</span>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" class="weui-check" name="rdPayMethod" id="rdm2" v-bind:checked="form.payMethod==2" v-on:click="selectPayMethod(2)" />
                                        <i class="weui-icon-checked"></i>
                                    </div>
                                </label>
                                <label class="weui-cell weui-check__label divNone" for="rdm3" v-bind:class="[pay_method.is_jd_pay?'divFlex':'']" v-if="pay_method.is_jd_pay">
                                    <div class="weui-cell__bd" style="flex:1;position:relative;">
                                        <img class="weui-vcode-img" style="height:30px;" src="http://file-cdn.songhebao.com/img/pay/jdpay_logo.png">
                                        <span style="position:absolute;left:50px;line-height: 30px;">京东支付(网银在线)</span>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" class="weui-check" name="rdPayMethod" id="rdm3" v-bind:checked="form.payMethod==3" v-on:click="selectPayMethod(3)" />
                                        <i class="weui-icon-checked"></i>
                                    </div>
                                </label>
                                
                                <label class="weui-cell weui-check__label divNone" v-for="(item,index) in cardCoupon" v-bind:for="'rdm'+(index+4)"   v-bind:class="[item.cardcoupon_name?'divFlex':'']">
                                    <div class="weui-cell__bd" style="flex:1;position:relative;">
                                        <img class="weui-vcode-img" style="height:30px;" v-bind:src="item.img_url">
                                        <span style="position:absolute;left:50px;line-height: 30px;" v-text="item.cardcoupon_name"></span>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" class="weui-check" name="rdPayMethod" v-bind:id="'rdm'+(index+4)" v-bind:checked="form.payMethod==item.ex1" v-on:click="selectPayMethod(item.ex1,item.main_cardcoupon_id)" />
                                        <i class="weui-icon-checked"></i>
                                    </div>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="apply">
                <a href="javascript:void(0);" class="weui-btn btn-apply" v-on:click="apply()">确认提交</a>
            </div>
            <div class="order-table">
                <div class="order-cell-right">
                    <span class="to-link" v-if="has_login" v-on:click="goLogin()">去登录</span>
                </div>
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
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/ApplyMember.js?v=2017062301"></script>
    <script src="//static-files.socialcrmyun.com/Scripts/wxshare/wxshare1.1.0/wxshare.js"></script>
</asp:Content>
