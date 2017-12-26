<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CouponDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.CouponDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>优惠券 </title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/basic.css" />
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <link href="../../css/buttons2.css" rel="stylesheet" type="text/css" />
    <style>
        .divnext
        {
            text-align: center;
        }
        .orderinfobox, .orderbox
        {
            padding-bottom: 0px;
        }
        
        .h1, h2, h4, h5, h6
        {
            clear: none;
        }
        img{width:100%;}
        .orderinfobox .product img, .orderbox .product img {height:auto;}
    </style>
</head>
<body>
    <section class="box">
    <div id="objlist" class="orderbox">
    <a href="javascript:void(0)" class="order">'
    <div class="product">
    <img src="/customize/tuao/images/logo.png" >
    <div class="info">
    <span class="text">土澳商城-优惠券号码 <br/><%=Model.CouponNumber %></span>
    <span class="text">折扣:<%=Model.Discount %>折</span>
    <span class="price" onclick="{0}"></span>
    </div>
    </div>
    <div class="total">
    </div>
    </a>
    </div>
    <div style="text-align:center;">
    <a id="btnShare" href="javascript:void(0)" class="button button-3d button-action button-pill">分享</a>    
    </div>

            <div style="width: 100%; height: 1500px; display: none; background: #000; opacity: 0.7;
            position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
            &nbsp;
        </div>
        <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; height: 1500px;
            text-align: right; display: none;" id="sharebox">
            <img src="images/sharetip.png" />
        </div>

</section>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {


        $("#btnShare").click(function () {
            $("#sharebg,#sharebox").show();
            $("#sharebox").css({ "top": $(window).scrollTop() })
        });

        $("#sharebg,#sharebox").click(function () {
            $("#sharebg,#sharebox").hide();
        });


    })

</script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "土澳网-优惠券",
            desc: "土澳网，精心甄选源自澳洲商品的电商平台",
            //link: '', 
            imgUrl: "http://localhost/customize/tuao/images/logo.png"
        })
    })
</script>
</html>
