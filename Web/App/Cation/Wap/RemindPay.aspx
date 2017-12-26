<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemindPay.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.RemindPay" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <title>
        <%=JuactivityInfo.ActivityName %></title>
    <link href="/lib/animate/animate.css" rel="stylesheet" />
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.1" />
    <link href="/Weixin/ArticleTemplate/css/comm.css?v=20160509" rel="stylesheet" />
    <!-- 活动收费模板 -->
    <style type="text/css">
        .col-xs-8
        {
            width: 100%;
        }
        
        .mustinput
        {
            position: relative;
            top: 4px;
            left: 2px;
            color: red;
        }
        
        select
        {
            height: 30px;
            width: 100%;
        }
        
        input[type="checkbox"]
        {
            width: 20px;
            height: 20px;
            padding: 0 0px 0 0;
        }
        
        .w100P
        {
            width: 100%;
        }
        
        #lblAmount
        {
            color: Red;
            font-weight: bold;
        }
        .wrapUseAmount
        {
          display:none;  
            
          }
    </style>
</head>
<body class="whitebg">
    <div class="wcontainer activetitle">
        <h1>
            <%=JuactivityInfo.ActivityName %>
        </h1>
        <%--        <div class="tagbox">
            <span class="wbtn_tag wbtn_red">
                <span class="iconfont icon-eye"></span>12
            </span><span class="wbtn_tag wbtn_orange">
                <span class="iconfont icon-36"></span>
                <label id="lblsignuptotalcounttop">
                    0
                </label>
            </span>
        </div>--%>
    </div>
    <div class="mainlist activelist activeinfomainlist">
        <div class="listbox">
            <div class="mainimg">
                <img src="<%=JuactivityInfo.ThumbnailsPath %>" />
            </div>
            <!--            <span class="wbtn_fly wbtn_flytr wbtn_greenyellow">费用：
            1积分 </span>-->
            <span class="baomingstatus"><span class="text">进行中 </span>
                <svg class="sanjiao" version="1.1" viewbox="0 0 100 100"><polygon points="100,100 0.2,100 100,0.2" /></svg>
            </span>
            <div class="activeconcent">
                <div class="textbox">
                    <p>
                        <span class="iconfont icon-clock"></span><span class="text">开始时间:<%=((DateTime)JuactivityInfo.ActivityStartDate).ToString("yyyy年MM月dd日 HH时mm分")%>
                        </span>
                    </p>
                    <p>
                        <span class="iconfont icon-adress"></span><span class="text">地址:<%=JuactivityInfo.ActivityAddress %></span>
                    </p>
                </div>
            </div>
            <div class="wcontainer applyactive">
                <div class="applyactiveinbox">
                    <form action="" id="formsignin">
                    </form>
                    <div class="input-group activeinfosubmitbtn">
                        <span class="wbtn wbtn_main"><span class="text" onclick="UpdateOrder()">支付</span>
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <!-- listbox -->
    </div>
    <!-- mainlist -->
    <div class="footerbar" style="display: none;">
        <div class="col-xs-8">
            <span class="wbtn wbtn_line_main" id="applyactivebtn"><span class="iconfont icon-b55 smallicon">
            </span>支付</span>
        </div>
    </div>
    <!-- footerbar -->
    <div id="wrapSelectCoupon">
        <div class="header">
            <i class="iconfont icon-fanhui btnClose" onclick="hideSelectCoupon()"></i><span onclick="hideSelectCoupon()">
                选择优惠券</span>
        </div>
        <div class="noCoupon">
            ——&nbsp;没有可用优惠券&nbsp;——
        </div>
        <div class="couponList">
            <div class="wrapCouponTotalCount">
                可用优惠券 (<span class="couponTotalCount">1</span>张)</div>
            <div class="wrapCouponDataList new-coupons">
                <div class="new-coupon " v-for="item in currCouponList" v-on:click="selectCoupon(item)"
                    v-bind:class="{ 'bglinghtRed': item.checked}">
                    <div class="coupon-type ">
                        {{item.cardcoupon_type_name}}
                    </div>
                    <div class="coupon-content">
                        <div class="coupon-left">
                            {{{item.value_txt}}}
                            <!-- ￥<span class="big-word ">500</span> -->
                        </div>
                        <div class="coupon-center">
                            <div class="title ">
                                {{item.cardcoupon_name}}</div>
                            <div class="desc  ">
                                {{item.limit_txt}}</div>
                            <!-- <div class="time ">2016.2.20-2016.5.19</div> -->
                        </div>
                        <div class="coupon-right">
                            <span class="iconfont icon-xuanze" v-show="item.checked"></span>
                        </div>
                        <div class="clear-both">
                        </div>
                    </div>
                    <div class="coupon-desc ">
                        <span class="left">还有{{item.end_time_diff}}天过期</span> <span class="right">有效期至：{{item.end_time_str}}</span>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="big-white-circle-left">
                    </div>
                    <div class="big-white-circle-right">
                    </div>
                    <div class="small-white-circle-wrap">
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                        <div style="width: 2px; height: 2px; border-radius: 2px; background: #fff; float: left;
                            margin: 0 0 0 8px;">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="couponBtn">
            <a href="javascript:;" id="btnSelectCoupon" class="btn btn-warning" onclick="hideSelectCoupon()">
                确定选择</a>
        </div>
    </div>
</body>
</html>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="/WuBuHui/js/jquery.js"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="/WuBuHui/js/bootstrap.js"></script>
<script src="/wubuhui/js/weixinsharebtn.js" type="text/javascript"></script>
<script src="/WuBuHui/js/partyinfo.js"></script>
<script src="/WuBuHui/js/gotopageanywhere.js"></script>
<script src="/Scripts/jquery.form.js" type="text/javascript"></script>
<script src="/lib/vue/vue.min.js" type="text/javascript"></script>
<script src="/lib/la-datePicker/datePicker.js" type="text/javascript"></script>
<script src="/Scripts/Common.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js" type="text/javascript"></script>
<script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script type="text/javascript">
    var signUpId = '<%=JuactivityInfo.SignUpActivityID %>'; //真实报名活动ID
    var juactivityId = '<%=JuactivityInfo.JuActivityID %>'; //主表ID
    var myTototalScore = 0; //我的剩余积分
    var myAmount=0;//我的余额
    var maxUseAmount=0;//最多使用余额
    var maxUseScore = 0; //最多使用积分
    var exchangeAmount = 0; //积分兑换金额
    var exchangeScore = 0; //需要积分
    var currBuyItemId = 0;
    var currBuyItemAmount = 0;
    var currCouponList = [];
    var canUserCouponCount = 0;
    var amountShowName="";
    var vm = new Vue({
        el: 'body',
        data: {
            currCouponList: [],
            currSelectCouponId: ''
        },
        methods: {
            selectCoupon: selectCoupon//选择优惠券
        }
    });

    $(function () {

        LoadConfig();

        //LoadScore(); //加载我的积分

        LoadScoreExchangRate(); //加载积分兑换比例

        LoadSignUpField(); //加载报名字段

        setTimeout("$(applyactivebtn).click()", 2000);

        window.alert = window.Alert = function (msg) {
            layer.open({
                content: msg,
                time: 2
            });
        };

        setTimeout(function () {
            $.get('/serv/pubapi.ashx?action=VistWxpay', {}, function (data) { }, 'json');
        }, 5000);

        

    });





    //获取要使用的积分
    function getUseScore() {
      
        if ($('#chkUseScore').prop('checked')) {
            return maxUseScore;
        }

        return 0;
    }

     //获取要使用的余额
    function getUseAmount() {
       

        if ($("#cbUseAmount").prop('checked')) {
            return maxUseAmount;
        }

        return 0;
    }


    //重新下单
    function UpdateOrder() {

        var dataModel = {
            Action: "UpdateActivityOrder",
            OrderId:"<%=Request["orderId"] %>",
            ActivityId: signUpId,
            ItemId: currBuyItemId,
            UseScore: getUseScore(),
            UseAmount:getUseAmount(),
            MyCouponId: vm.currSelectCouponId
            

        }


        if (dataModel.ItemId == "") {
            alert("请选择选项");
            return false;
        }
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXMallHandler.ashx",
            data: dataModel,
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 1) {
                    if (parseFloat(resp.ExObj.toString()) > 0) {
                      
                        window.location.href = '/customize/shop/wxpay.aspx?order_id=' + resp.ExStr + '&return_url_success=/App/Cation/Wap/MyActivityLlists.aspx&return_url_fail=/App/Cation/Wap/MyActivityLlists.aspx';
                    }
                    else {
                        window.location.href = '/App/Cation/Wap/MyActivityLlists.aspx'; //不用调微信支付，直接转到我报名的活动
                    }

                }
                else {
                    alert(resp.Msg);
                }

            }
        });



    }


    //加载报名字段
    function LoadSignUpField() {

        var html = '';
        //购买选项
        html += '<div class="wrapBuy input-group">';


        html += '<div class="wrapBuyItem"></div>';

        //html += '<div class="wrapScore"></div>';

        //Coupon
        html += '<div class="wrapCoupon">';




        html += '<a id="btnShowSelectCoupon" href="javascript:;" onclick="showSelectCoupon(this)">您有<span class="colorRed">' + currCouponList.length + '</span>张可用优惠券，请选择优惠券</a>';
        html += '<sapn id="lbNoSelectCoupon" class="color999">没有可用优惠券</sapn>';



        html += '<div class="wrapScore"></div>';

        html += '<div><hr/></div>';

        html += '<div class="wrapUseAmount"></div>';
        html += '</div>';

        html += '<div class="wrapAmount">';
        html += '<span class="input-group-addon">支付金额:&nbsp;';
        html += '<label id=\"lblAmount\">0</label>元';
        html += '</span>';
        html += '</div>';



        html += '</div>';
        //购买选项

        $("#formsignin").append(html); //填充报名字段


        LoadItem(); //加载活动选项

    }



        function showSelectCoupon(el) {
            $('#wrapSelectCoupon').show();
        }

        function hideSelectCoupon(el) {
            CouponChange();
            $('#wrapSelectCoupon').hide();
        }

        //选择卡券
        function selectCoupon(item) {
            for (var i = 0; i < vm.currCouponList.length; i++) {
                if (vm.currCouponList[i].cardcoupon_id != item.cardcoupon_id)
                    vm.currCouponList[i].checked = false;
            };
            item.checked = !item.checked;
            if (item.checked) {
                vm.currSelectCouponId = item.cardcoupon_id;
                $('#btnShowSelectCoupon').html('已选优惠券：<span class="colorRed">' + item.cardcoupon_name + '</span>');
            } else {
                vm.currSelectCouponId = '';
                $('#btnShowSelectCoupon').html('您有<span class="colorRed">' + currCouponList.length + '</span>张可用优惠券，请选择优惠券');
            }
            //console.log('vm.currSelectCouponId', vm.currSelectCouponId);



        }

        function confirmSelectCoupon(el) {
            if (currCouponList.length == 0) {
                $('#wrapSelectCoupon').hide();
                return;
            };
        }


        function checkItem(el) {
            var _this = $(el);
            $('.wrapBuyItem .buyItem').removeClass("isChecked");
            _this.addClass("isChecked");
            currBuyItemId = _this.data('id');
            currBuyItemAmount = _this.data('amount');

            ItemChange();
        }

        //加载活动选项
        function LoadItem() {
            $.ajax({
                type: 'post',
                url: "/Handler/Activity/ActivityHandler.ashx",
                data: { Action: "GetActivityItemList", JuActivityId: juactivityId },
                dataType: "json",
                success: function (resp) {
                    //console.log('buyitem', resp);
                    var html = '';
                    $.each(resp.ExObj, function (index, item) {

                        if (index == 0) {
                            html += '<div class="buyItem isChecked" onclick="checkItem(this)" data-amount=\"' + item.Amount + '\"  data-id=\"' + item.ItemId + '\">';
                            currBuyItemId = item.ItemId;
                            currBuyItemAmount = item.Amount;
                        } else {
                            html += '<div class="buyItem" onclick="checkItem(this)" data-amount=\"' + item.Amount + '\" data-id=\"' + item.ItemId + '\">';
                        }

                        html += '<div class="amount">￥' + item.Amount + '</div>';

                        if (item.ProductName) {
                            html += '<div class="name">' + item.ProductName + '</div>';
                        }
                        if (item.Description) {
                            html += '<div class="description">' + item.Description + '</div>';
                        }

                        html += '<div class="checkedbg"></div>';
                        html += '<i class="iconfont checkIcon icon-xuanze"></i>';
                        html += '</div>';

                    });

                    //$(ddlItem).append(html);

                    $('.wrapBuyItem').html(html);

                    setTimeout(function () {
                        ItemChange();
                        LoadMyCoupon(); //加载我的优惠券
                    }, 1000)

                }
            });
        }


        //加载积分
        function LoadScore() {
            $.ajax({
                type: 'post',
                url: "/serv/api/user/info.ashx",
                data: { Action: "currentuserinfo" },
                dataType: "json",
                success: function (resp) {
                    myTototalScore = resp.totalscore;
                    myAmount=resp.account_amount;
                    maxUseAmount=myAmount;
                    if (myTototalScore >=0) {

                        var html = '<div>使用积分(共有' + myTototalScore + '积分)</div>';
                        html += '<div><label><input type="checkbox" id="chkUseScore" onclick="CalcAmount()" />可用<span id="lbMaxUseScore" class="colorRed">0</span>积分，抵<span id="lbCanPayMoney" class="colorRed">0</span>元</label></div>';

                        $('.wrapScore').html(html);

                        

                    } else {
                        //$("#chkUseScore").attr("disabled", "disabled");
                    }

                    if (myAmount>=0) {
                        var html = '<div>使用'+amountShowName+'(共有' + myAmount +amountShowName+')</div>';
                        html += '<div><label><input type="checkbox" id="cbUseAmount" onclick="CalcAmount()" />可用<span id="lbMaxUseAmount" class="colorRed">'+maxUseAmount+'</span>'+amountShowName+'，抵<span id="lbAmountCanPayMoney" class="colorRed">'+maxUseAmount+'</span>元</label></div>';
                        $('.wrapUseAmount').html(html);


                    }else {
                     //$("#cbUseAmount").attr("disabled", "disabled");

                    }






                }
            });
        }

        //加载配置
        function LoadConfig() {
            $.ajax({
                type: 'post',
                url: "/serv/api/config/get.ashx",
                dataType: "json",
                success: function (resp) {
               console.log(resp.result.malll.is_enable_account_amount_pay);
                if (resp.result.malll.is_enable_account_amount_pay==true) {
                amountShowName=resp.result.malll.account_amount_pay_showname;
                $('.wrapUseAmount').show();
                }
                 LoadScore();






                }
            });
        }



        //加载积分兑换比例
        function LoadScoreExchangRate() {
            $.ajax({
                type: 'post',
                url: "/serv/api/mall/score.ashx",
                data: { Action: "ExchangeRate" },
                dataType: "json",
                success: function (resp) {

                    exchangeAmount = resp.amount;
                    exchangeScore = resp.score;
                    if (resp.amount == 0 || resp.score == 0) {
                        $("#divscore").hide();
                        $('#wrapScore').hide();
                    }

                }
            });
        }

        //加载我的优惠券
        function LoadMyCoupon() {
            $.ajax({
                type: 'post',
                url: "/serv/api/mall/CardCoupon.ashx",
                data: { Action: "List", is_can_use: 1, cardcoupon_status: 0, amount: currBuyItemAmount },
                dataType: "json",
                success: function (resp) {

                    console.log('LoadMyCoupon', resp.list);

                    if (resp.list) {
                        $('#wrapSelectCoupon .couponList').show();
                        $('#wrapSelectCoupon .noCoupon').hide();
                        $('#btnShowSelectCoupon').show();
                        $('#lbNoSelectCoupon').hide();

                        for (var i = 0; i < resp.list.length; i++) {
                            var cardcoupon = resp.list[i];
                            cardcoupon.checked = false;
                            cardcoupon.end_time_str = new Date(cardcoupon.valid_to_timestamp).format('yyyy-MM-dd');
                            cardcoupon.end_time_diff = LADatePicker.GetDayDiff(cardcoupon.valid_to_timestamp, new Date());

                            switch (cardcoupon.cardcoupon_type) {
                                case 0:
                                    cardcoupon.cardcoupon_type_name = '折扣券';
                                    cardcoupon.value_txt = '<span class="big-word ">' + cardcoupon.discount + '</span>折';
                                    cardcoupon.limit_txt = '使用无金额限制';

                                    break;
                                case 1:
                                    cardcoupon.cardcoupon_type_name = '现金券';
                                    cardcoupon.value_txt = '￥<span class="big-word ">' + cardcoupon.deductible_amount + '</span>';
                                    cardcoupon.limit_txt = '使用无金额限制';

                                    break;
                                case 2:
                                    cardcoupon.cardcoupon_type_name = '免邮券';
                                    cardcoupon.value_txt = '<span class="big-word ">包邮</span>';
                                    cardcoupon.limit_txt = '满' + freefreight_amount + '元可用';

                                    break;
                                case 3:
                                    cardcoupon.cardcoupon_type_name = '满减券';
                                    cardcoupon.value_txt = '￥<span class="big-word ">' + cardcoupon.buckle_sub_amount + '</span>';
                                    cardcoupon.limit_txt = '满' + cardcoupon.buckle_amount + '元可用';
                                    break;
                            }


                        };

                        currCouponList = resp.list;
                        $('.couponTotalCount').html(currCouponList.length);

                        $('#btnShowSelectCoupon').html('您有<span class="colorRed">' + currCouponList.length + '</span>张可用优惠券，请选择优惠券');

                        vm.currCouponList = resp.list;
                    } else {
                        $('#wrapSelectCoupon .noCoupon').show();
                        $('#wrapSelectCoupon .couponList').hide();
                        $('#btnShowSelectCoupon').hide();
                        $('#lbNoSelectCoupon').show();
                        vm.currCouponList = [];
                    }



                }
            });
        }

        //计算应付金额
        function CalcAmount(type) {

            if (currBuyItemId == "") {
                alert("请选择选项");
                $('#chkUseScore').prop('checked', false);
                return false;
            }

//            if ((!chkUseScore.checked)&&(cbUseAmount.checked)&&myTototalScore>0) {
//            alert("请先选择积分");
//            cbUseAmount.checked=false;
//            CalcAmount();
//            return false;
//}

            $.ajax({
                type: 'post', 
                url: "/Handler/App/WXMallHandler.ashx",
                data: { Action: "CalcActivityPayAmount", ItemId: currBuyItemId, UseScore: getUseScore(), MyCouponId: vm.currSelectCouponId,UseAmount:getUseAmount()},
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        $(lblAmount).html(resp.ExStr);//计算应付金额

                        //积分选 余额不选
                        if (chkUseScore.checked&&(!cbUseAmount.checked)) {
                         if (resp.ExStr=="0.00") {
                         maxUseAmount=0;
                         $('#lbMaxUseAmount').html(maxUseAmount);
                         $('#lbAmountCanPayMoney').html(maxUseAmount);      
                        }
                        else {
                            CalcMaxUseAmount(parseFloat(resp.ExStr));
                           
                            }

                         }
                         //积分选 余额不选 

                         //积分不选 余额选
                         if (cbUseAmount.checked&&(!chkUseScore.checked)) {
                        if (resp.ExStr=="0.00") {
                          maxUseScore=0;
                         $('#lbMaxUseScore').html(maxUseScore);
                         $('#lbCanPayMoney').html(maxUseScore);
                         }
                         else {
                            CalcMaxUseAmount(parseFloat(resp.ExStr));
                            
                         }

                         }
                         //积分不选 余额选

//                         //积分选 余额选
//                       if (cbUseAmount.checked&&(chkUseScore.checked)) {
//                        if (resp.ExStr=="0.00") {
//                          maxUseScore=0;
//                         $('#lbMaxUseScore').html(maxUseScore);
//                         $('#lbCanPayMoney').html(maxUseScore);
//                         }
//                         else {
//                            
//                         }

//                         }
                         //积分选 余额选


//                         //积分不选 余额不选
//                        if (!cbUseAmount.checked&&(!chkUseScore.checked)) {
//                        if (resp.ExStr=="0.00") {
//                          maxUseScore=0;
//                         $('#lbMaxUseScore').html(maxUseScore);
//                         $('#lbCanPayMoney').html(maxUseScore);
//                         }
//                         else {
//                            
//                         }

//                         }
//                         //积分不选 余额不选

                        if (!chkUseScore.checked) {
                            CalcMaxUseScore(parseFloat(resp.ExStr));
                        
                        }
                        if (!cbUseAmount.checked) {
                            CalcMaxUseAmount(parseFloat(resp.ExStr));
                        
                        }


//                        if (!(chkUseScore.checked&&cbUseAmount.checked)) {
//                        CalcMaxUseScore(parseFloat(resp.ExStr));
//                        CalcMaxUseAmount(parseFloat(resp.ExStr));
//}

                    }
                    else {

                        alert(resp.Msg);
                        switch (type) {
                            case 0:
                                ClearSelectCoupon();
                                // $(ddlCoupon).val("");
                                break;

                            default:

                        }

                    }

                }
            });


        }

        //计算最多使用的积分
        function CalcMaxUseScore(totalAmount) {

            if (exchangeAmount == 0 || exchangeScore == 0) {
                return;
            }
            maxUseScore = Math.ceil(totalAmount / (exchangeAmount / exchangeScore));

            if (myTototalScore < maxUseScore) {
                maxUseScore = myTototalScore;
            }

            //$(ddlUseScore).html("<option value=\"0\">不使用</option><option checked=\"checked\" value=\"" + maxUseScore + "\">" + maxUseScore + "积分</option>");


            $('#lbMaxUseScore').html(maxUseScore);
            $('#lbCanPayMoney').html((maxUseScore * (exchangeAmount / exchangeScore)).toFixed(2));

        }

       //计算最多使用的余额
        function CalcMaxUseAmount(totalAmount) {
        
            if (myAmount== 0) {
                return;
            }
            if (myAmount>=totalAmount) {
             maxUseAmount=totalAmount;

            }
            if (myAmount < totalAmount) {
                maxUseAmount = myAmount;
            }

            $('#lbMaxUseAmount').html(maxUseAmount);
            $('#lbAmountCanPayMoney').html(maxUseAmount);

        }

//        //积分变动
//        function ScoreChange() {

//            $.ajax({
//                type: 'post',
//                url: "/Handler/App/WXMallHandler.ashx",
//                data: { Action: "CalcActivityPayAmount", ItemId: currBuyItemId, UseScore: getUseScore(), MyCouponId: vm.currSelectCouponId,UseAmount:getUseAmount() },
//                dataType: "json",
//                success: function (resp) {
//                    if (resp.Status == 1) {
//                        $(lblAmount).html(resp.ExStr);
//                        if (chkUseScore.checked&&(!cbUseAmount.checked)) {//积分选 余额不选 
//                         if (resp.ExStr=="0.00") {
//                         maxUseAmount=0;
//                         $('#lbMaxUseAmount').html(maxUseAmount);
//                         $('#lbAmountCanPayMoney').html(maxUseAmount);      
//                        }
//                        else {
//                          CalcAmount();
//                            }

//                         }
//                      else if(!chkUseScore.checked&&(cbUseAmount.checked)) {//积分不选 余额选
//                        
//                        }
//                        else if(chkUseScore.checked&&(cbUseAmount.checked)) {//积分 余额都选
//                        
//                        }
//                       else if(!chkUseScore.checked&&(!cbUseAmount.checked)) {//积分 余额都不选
//                        
//                        }
//                    else {
//                        
//                        CalcAmount();

//                     }


//                    }
//                    else {



//                    }

//                }
//            });

//        }

//        //余额
//        function AmountChange() {

//            $.ajax({
//                type: 'post',
//                url: "/Handler/App/WXMallHandler.ashx",
//                data: { Action: "CalcActivityPayAmount", ItemId: currBuyItemId, UseScore: getUseScore(), MyCouponId: vm.currSelectCouponId,UseAmount:getUseAmount() },
//                dataType: "json",
//                success: function (resp) {
//                    if (resp.Status == 1) {
//                        $(lblAmount).html(resp.ExStr);
//                        if (cbUseAmount.checked&&(!chkUseScore.checked)) {
//                        if (resp.ExStr=="0.00") {
//                         maxUseScore=0;
//                         $('#lbMaxUseScore').html(maxUseScore);
//                         $('#lbCanPayMoney').html(maxUseScore);
//                         }
//                         else {
//                            CalcAmount();
//}

//                         }
//                    else {
//                        
//                        CalcAmount();

//                     }


//                    }
//                    else {



//                    }

//                }
//            });

//        }

        //优惠券变动
        function CouponChange() {
            
            chkUseScore.checked=false;
            cbUseAmount.checked=false;
            CalcAmount(0);
        }

        //选择状态清除
        function ClearSelectCoupon() {

            if (vm.currCouponList) {
                for (var i = 0; i < vm.currCouponList.length; i++) {
                    vm.currCouponList[i].checked = false;
                };
            };

            vm.currSelectCouponId = '';

            $('#btnShowSelectCoupon').html('您有<span class="colorRed">' + currCouponList.length + '</span>张可用优惠券，请选择优惠券');
        }

        //选项变动
        function ItemChange() {

            //$(ddlUseScore).html("<option value=\"0\">不使用</option><option checked=\"checked\" value=\"" + maxUseScore + "\">" + maxUseScore + "积分</option>");

            $('#chkUseScore').prop('checked', false);

            if (typeof cbUseAmount != 'undefined') {
                cbUseAmount.checked = false;
            }
            
            //优惠券清除
            ClearSelectCoupon();

            CalcAmount();
       }

        //    //获取购买选项id
        //    function getBuyItemId() {

        //    }
    //}
</script>
<script type="text/javascript">
    //touchstart
    $("#applyactivebtn").unbind().bind("click", function () {

        gotopageanywhere('.activeinfomainlist', function () {
            if (!$(".applyactive").attr("style")) {
                var applyheight = $(".applyactiveinbox").height() + 10
                $(".applyactive").css({ "height": applyheight })
            }
        })
    })

    var pageData = {
        currUserOpenId: '', //当前用户的wxopenId
        currUserId: 'jubit', //当前用户的userId
        title: '收费活动', //标题
        summary: '', //描述
        shareImgUrl: 'http://comeoncloud.comeoncloud.net/img/hb/hb5.jpg', //分享缩略图
        shareUrl: window.location.href, //分享链接
        tempShareId: CreateGUID(),
        preShareId: GetParm('comeonshareid'),
        callback: callback
    };

    var shareCallBackFunc = {
        timeline_s: function () {
            submitShare('timeline_s');
            shareComeplete();
        },
        timeline_c: function () {
            //朋友圈分享取消
        },
        message_s: function () {
            //分享给朋友
            submitShare('message_s');
            shareComeplete();
        },
        message_c: function () {
            //朋友分享取消
        }
    }

    var processUrl = function (url) {
        url = DelUrlParam(url, 'comeonshareid');
        url = DelUrlParam(url, 'from');
        url = DelUrlParam(url, 'isappinstalled');
        return url;
    }

    var callback = function (data) { }

    var submitShare = function (WxMsgType) {
        var reqData = {
            Action: 'ShareSubmit',
            url: processUrl(pageData.shareUrl),
            shareId: pageData.tempShareId,
            preId: pageData.preShareId,
            userId: pageData.currUserId,
            userWxOpenId: pageData.currUserOpenId,
            wxMsgType: WxMsgType
        }

        //分享到朋友圈
        $.ajax({
            type: 'post',
            url: '/serv/pubapi.ashx',
            data: reqData,
            dataType: 'jsonp',
            success: function (data) {
                pageData.tempShareId = CreateGUID();
            }
        });
    }

    //TODO:Url处理
    //移除原有参数 comeonshareid from isappinstalled
    pageData.shareUrl = processUrl(pageData.shareUrl);
    pageData.shareUrl = pageData.shareUrl + '?comeonshareid=' + pageData.tempShareId;


    wx.ready(function () {
        wxapi.wxshare({
            title: pageData.title,
            desc: pageData.summary,
            link: pageData.shareUrl,
            imgUrl: pageData.shareImgUrl
        }, shareCallBackFunc)
    });
</script>
