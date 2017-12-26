<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.ubi.m.app" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>易劳，一站式的解决你的劳动法问题</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width" />
    
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/lib/angularjs/ngDialog/css/ngDialog.min.css" rel="stylesheet" />
    <link href="/lib/angularjs/ngDialog/css/ngDialog-theme-default.min.css" rel="stylesheet" />
    <link href="/lib/angularjs/ngDialog/css/ngDialog-custom-width.css" rel="stylesheet" />
    <link href="/lib/angularjs/carousel/angular-carousel.min.css" rel="stylesheet" />
    <link href="/lib/layer.mobile/need/layer.css" rel="stylesheet" />
   <%-- <link type="text/css" href="/lib/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="/customize/ubi/m/css/all.css" rel="stylesheet" />

    <script src="/lib/zepto/zepto.min.js"></script>
    <script src="/lib/classie/classie.js"></script>
    <script src="/lib/angularjs/1.3.15/angular.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-route.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-sanitize.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-touch.min.js"></script>
    <script src="/lib/angularjs/ngStorage/ngStorage.min.js"></script>
    <script src="/lib/angularjs/ngDialog/js/ngDialog.min.js"></script>
    <script src="/lib/angularjs/angularjsUpload/angular-file-upload.min.js"></script>
    <script src="/lib/angularjs/carousel/angular-carousel.min.js"></script>
    <%--<script src="/lib/angular-bootstrap/ui-bootstrap-tpls-0.12.1.min.js"></script>--%>
    <script src="/lib/layer.mobile/layer.m.js"></script>
    <script src="/Scripts/Common.js"></script>
     <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <%--<script src="/customize/ubi/m/modules/public/reg/regCtrl.js"></script>--%>
   <%-- <script src="/customize/ubi/m/js/services.js"></script>--%>

    <script src="/customize/ubi/m/js/app.bundle.js"></script>
    
</head>
<body>
    <div ng-app="ubimodule" ng-controller="pageBaseCtrl" class="pageBase" ng-class="{wrapQQBrowser:isQQBrowser}" >
        <div ng-view></div>
    </div>
</body>
</html>
