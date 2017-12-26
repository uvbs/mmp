﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.pureCar.m.app" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PureCar</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width" />
    
   <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/lib/angularjs/ngDialog/css/ngDialog.min.css" rel="stylesheet" />
    <link href="/lib/angularjs/ngDialog/css/ngDialog-theme-default.min.css" rel="stylesheet" />
    <link href="/lib/angularjs/ngDialog/css/ngDialog-custom-width.css" rel="stylesheet" />
    <link href="/lib/angularjs/carousel/angular-carousel.min.css" rel="stylesheet" />
    <link href="/lib/layer.mobile/need/layer.css" rel="stylesheet" />

    <link href="/customize/pureCar/m/dist/all.css" rel="stylesheet" />

    <script src="/lib/classie/classie.js"></script>
    <script src="/lib/jquery/jquery-2.1.1.min.js"></script>
    <script src="/lib/moment/moment.js"></script>
    <script src="/lib/angularjs/1.3.15/angular.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-route.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-sanitize.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-touch.min.js"></script>
    <script src="/lib/angularjs/ngInfiniteScroll/ng-infinite-scroll.js"></script>
    <script src="/lib/angularjs/ngStorage/ngStorage.min.js"></script>
    <script src="/lib/angularjs/ngDialog/js/ngDialog.min.js"></script>
    <script src="/lib/angularjs/carousel/angular-carousel.min.js"></script>
    <script src="/lib/layer.mobile/layer.m.js"></script> 

    <script src="/customize/pureCar/m/dist/app.bundle.js"></script>

</head>
<body>
     <div ng-app="pureCarModule" ng-controller="pageBaseCtrl" class="pageBase" ng-class="{wrapQQBrowser:isQQBrowser}" >
        <div ng-view></div>
         <back-button></back-button>
    </div>
</body>
</html>
