<%@ Page Title="会员升级" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="UpgradeMember.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.UpgradeMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
    <link href="/App/Wap/css/UpgradeMember.css?v=2017030101" rel="stylesheet" />
    <style type="text/css">
        .checkpwd {
            font-size: 15px;
            height: 40px;
            width: 70%;
            padding: 0 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-jiantouarrowdown" viewBox="0 0 1024 1024">
            <path d="M543.97576 742.991931 127.510604 281.008069 896.48735 281.008069Z"></path>
        </symbol>
    </svg>
    <div class="wrapComm wrapUpgradeMember">
        <div class="wrapContent">
            <div class="wrapMemberHead">
                <div class="wrapImg">
                    <img class="img-bg" src="/App/Wap/img/HeadImgBg.png" />
                    <img class="img-ico" v-if="login_user.levelico" v-bind:src="login_user.levelico" />
                    <img class="img-name" v-if="login_user.levelnameimg" v-bind:src="login_user.levelnameimg" />
                    <img class="img-avatar" v-if="login_user.avatar" v-bind:src="login_user.avatar" />
                    <span class="show-id" v-if="login_user.id"><span>账号：</span><span v-text="login_user.phone"></span></span>
                    <span class="show-name" v-if="!login_user.levelnameimg && login_user.levelname" v-text="login_user.levelname"></span>
                    <span class="true-name" v-if="login_user.name" v-text="login_user.name"></span>
                </div>
            </div>
            <div class="comm-form upgrade-form">
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">可用佣金：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="login_user.totalamount">
                        </div>
                    </div>
                </div>
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
                <div class="weui-cells weui-cells_form" v-if="form.payWay<3">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">需补金额：</label>
                        </div>
                        <div class="weui_cell__bd">
                            <span v-bind:class="[!form.payWay && select.diffamount && form.tolevel==0?'color-red':'']" v-if="select.diffamount" v-text="select.diffamount"></span>
                            <a v-if="!form.payWay && select.diffamount && form.tolevel==0" class="color-blue to-link mLeft10" v-on:click="goRecharge()">去充值</a>
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell parent-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">升级方式：</label>
                        </div>
                        <div class="weui_cell__bd child-cell">
                            <div class="weui-cells weui-cells_radio">
                                <label class="weui-cell  weui-check__label" for="rd1">
                                    <div class="weui-cell__bd">
                                        <p><%=website.TotalAmountShowName %>升级</p>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" name="rdPay" class="weui-check" id="rd1" v-bind:checked="form.payWay==1" v-on:click="selectPayWay(1)" />
                                        <span class="weui-icon-checked"></span>
                                    </div>
                                </label>
                                <label class="weui-cell  weui-check__label" for="rd2">
                                    <div class="weui-cell__bd">
                                        <p>支付升级</p>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" name="rdPay" class="weui-check" id="rd2" v-bind:checked="form.payWay==2" v-on:click="selectPayWay(2)" />
                                        <span class="weui-icon-checked"></span>
                                    </div>
                                </label>
                                <label class="weui-cell  weui-check__label" for="rd3">
                                    <div class="weui-cell__bd">
                                        <p>优惠券升级</p>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" name="rdPay" class="weui-check" id="rd3" v-bind:checked="form.payWay==3" v-on:click="selectPayWay(3)" />
                                        <span class="weui-icon-checked"></span>
                                    </div>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="weui-cells weui-cells_form" v-if="form.payWay==2">
                    <div class="weui-cell parent-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">付款方式：</label>
                        </div>
                        <div class="weui_cell__bd child-cell">
                            <div class="weui-cells weui-cells_radio">
                                <label class="weui-cell weui-check__label divNone" for="rdm1" v-bind:class="[pay_method.is_wx_pay?'divFlex':'']" v-if="pay_method.is_wx_pay">
                                    <div class="weui-cell__bd" style="flex: 1; position: relative;">
                                        <img class="weui-vcode-img" style="height: 30px;" src="/img/pay/wx_logo.png">
                                        <span style="position: absolute; left: 50px; line-height: 30px;">微信支付</span>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" class="weui-check" name="rdPayMethod" id="rdm1" v-bind:checked="form.payMethod==1" v-on:click="selectPayMethod(1)" />
                                        <i class="weui-icon-checked"></i>
                                    </div>
                                </label>
                                <label class="weui-cell weui-check__label divNone" for="rdm2" v-bind:class="[pay_method.is_ali_pay?'divFlex':'']" v-if="pay_method.is_ali_pay">
                                    <div class="weui-cell__bd" style="flex: 1; position: relative;">
                                        <img class="weui-vcode-img" style="height: 30px;" src="/img/pay/zfb_logo.png">
                                        <span style="position: absolute; left: 50px; line-height: 30px;">支付宝</span>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" class="weui-check" name="rdPayMethod" id="rdm2" v-bind:checked="form.payMethod==2" v-on:click="selectPayMethod(2)" />
                                        <i class="weui-icon-checked"></i>
                                    </div>
                                </label>
                                <label class="weui-cell weui-check__label divNone" for="rdm3" v-bind:class="[pay_method.is_jd_pay?'divFlex':'']" v-if="pay_method.is_jd_pay">
                                    <div class="weui-cell__bd" style="flex: 1; position: relative;">
                                        <img class="weui-vcode-img" style="height: 30px;" src="http://file-cdn.songhebao.com/img/pay/jdpay_logo.png">
                                        <span style="position: absolute; left: 50px; line-height: 30px;">京东支付(网银在线)</span>
                                    </div>
                                    <div class="weui-cell__ft">
                                        <input type="radio" class="weui-check" name="rdPayMethod" id="rdm3" v-bind:checked="form.payMethod==3" v-on:click="selectPayMethod(3)" />
                                        <i class="weui-icon-checked"></i>
                                    </div>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form" v-if="form.payWay==3">
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
                                        <input type="radio" class="weui-check" name="rdPayMethodC" v-bind:id="'rdm'+index" v-bind:checked="form.payMethodC==item.ex1" v-on:click="selectPayMethodC(item.ex1)" />
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
            <div class="upgrade">
                <a href="javascript:void(0);" class="weui-btn btn-upgrade" v-if="form.payWay==1 && form.tolevel > 0 && login_user.totalamount>= select.amount - login_user.levelamount" v-on:click="upgrade()">升级</a>
                <a href="javascript:void(0);" class="weui-btn btn-upgrade" v-if="form.payWay==2 && form.tolevel > 0" v-on:click="payUpgrade()">升级</a>
                <a href="javascript:void(0);" class="weui-btn btn-upgrade" v-if="form.payWay==3 && form.tolevel > 0" v-on:click="payConponUpgrade()">升级</a>
                <a href="javascript:void(0);" class="weui-btn weui-btn_disabled btn-disabled" v-if="!(form.payWay && form.tolevel > 0) && !(!form.payWay && form.tolevel > 0 && login_user.totalamount>= select.amount - login_user.levelamount)">升级</a>
            </div>
        </div>
        <%--    <div class="wrapContent divNone" v-bind:class="[!form.payWay && !check.payPwd.ok?'divBlock':'']" v-if="!form.payWay && !check.payPwd.ok">
            <check-pay-password v-on:checkpayok="checkPayPwdOk()" v-if="!form.payWay && !check.payPwd.ok"></check-pay-password>
        </div>--%>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
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
                            v-on:click="selectLevel(item)">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="display:none;" class="wrapPwd">
            <div class="weui-mask"></div>
            <div class="weui-dialog">
                <div class="weui-dialog__hd"><strong class="weui-dialog__title">系统提示</strong></div>
                <div class="weui-dialog__bd">
                     <div class="weui-cells weui-cells_form">
                        <div class="weui-cell">
                                <input class="weui-input" type="password" v-model="form.paypwd" placeholder="请输入支付密码"  maxlength="20" v-on:keyup.enter="checkPayPwd()" />
                        </div>
                    </div>
                    <div class="order-table">
                        <div class="order-cell-right">
                            <span class="to-link" v-on:click="goForgetPassword()">忘记支付密码?</span>
                        </div>
                    </div>
                </div>
                <div class="weui-dialog__ft">
                    <a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_default" v-on:click="cencelUpgrade()">取消</a>
                    <a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_primary" v-on:click="configUpgrade()">确定</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var login_user = JSON.parse('<%= new ZentCloud.BLLJIMP.BLLUser().GetLoginUserJsonString() %>');
        var totalAmountShowName = '<%=website.TotalAmountShowName %>';
        var noPay = false;
    </script>
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/component/CheckPayPwd/CheckPayPwd.js?v=2017030101"></script>
    <script type="text/javascript" src="/App/Wap/js/UpgradeMember.js?v=2017062601"></script>
</asp:Content>
