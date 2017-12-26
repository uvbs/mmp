<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Components.Coupon.Mobile.BeachHoney.Index" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/Components/Coupon/Mobile/BeachHoney/src/public/couponList/style.css"
        rel="stylesheet" />
    <link href="/Components/Coupon/Mobile/BeachHoney/src/public/couponDetail/style.css"
        rel="stylesheet" />
    <link href="/Components/Coupon/Mobile/BeachHoney/src/public/couponUse/style.css"
        rel="stylesheet" />
    <link href="/Components/Coupon/Mobile/BeachHoney/src/public/couponUsed/style.css"
        rel="stylesheet" />
    <link href="/Components/Coupon/Mobile/BeachHoney/src/public/couponOverdate/style.css"
        rel="stylesheet" />
            <script src="js/comm.js" type="text/javascript"></script>

    <script src="/lib/angularjs/1.3.15/angular.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-route.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-sanitize.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-touch.min.js"></script>
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <script src="/Components/Coupon/Mobile/BeachHoney/js/app.js"></script>
    <script src="/Components/Coupon/Mobile/BeachHoney/src/public/couponList/couponListCtrl.js"></script>
    <script src="/Components/Coupon/Mobile/BeachHoney/src/public/couponUse/couponUseCtrl.js"></script>
    <script src="/Components/Coupon/Mobile/BeachHoney/src/public/couponUsed/couponUsedCtrl.js"></script>
        <script src="/Components/Coupon/Mobile/BeachHoney/src/public/couponOverdate/couponOverdateCtrl.js"></script>
</head>
<body>
    <div ng-app="couponmodule" class="pageBase" ng-class="{wrapQQBrowser:isQQBrowser}">
        <div ng-view>
        </div>
    </div>
</body>
</html>
