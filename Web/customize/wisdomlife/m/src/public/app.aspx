<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.wisdomlife.m.app" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/common/menu.css" rel="stylesheet" />
    <link href="/lib/angularjs/carousel/angular-carousel.min.css" rel="stylesheet" />
    <link href="/lib/layer.mobile/need/layer.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/public/index/style.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/public/takeAd/style.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/public/putAd/style.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/public/selfMedia/style.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/public/advertiser/style.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/public/rank/style.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/public/selfMediaDetail/style.css" rel="stylesheet" />
    <link href="/customize/wisdomlife/m/src/public/advertiserDetail/style.css" rel="stylesheet" />

    <script src="/lib/classie/classie.js"></script>
    <script src="/lib/angularjs/1.3.15/angular.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-route.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-sanitize.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-touch.min.js"></script>
    <script src="/lib/angularjs/carousel/angular-carousel.min.js"></script>
    <script src="/lib/angular-bootstrap/ui-bootstrap-tpls-0.12.1.min.js"></script>
    <script src="/lib/layer.mobile/layer.m.js"></script>
    <!--<script src="/customize/ubi/m/js/global.js"></script>-->
    <script src="/customize/wisdomlife/m/js/app.js"></script>
    <script src="/customize/wisdomlife/m/js/services.js"></script>
    <script src="/customize/wisdomlife/m/src/common/pageBaseCtrl.js"></script>
    <script src="/customize/wisdomlife/m/src/public/index/indexCtrl.js"></script>
    <script src="/customize/wisdomlife/m/src/public/takeAd/takeAdCtrl.js"></script>
    <script src="/customize/wisdomlife/m/src/public/putAd/putAdCtrl.js"></script>
    <script src="/customize/wisdomlife/m/src/public/selfMedia/selfMediaCtrl.js"></script>
    <script src="/customize/wisdomlife/m/src/public/advertiser/advertiserCtrl.js"></script>
    <script src="/customize/wisdomlife/m/src/public/rank/rankCtrl.js"></script>
    <script src="/customize/wisdomlife/m/src/public/selfMediaDetail/selfMediaDetailCtrl.js"></script>
    <script src="/customize/wisdomlife/m/src/public/advertiserDetail/advertiserDetailCtrl.js"></script>

</head>
<body>
    <div ng-app="wisdomlifemodule" ng-controller="pageBaseCtrl" class="pageBase" ng-class="{wrapQQBrowser:isQQBrowser}">
        <div ng-view></div>
    </div>
</body>
</html>
