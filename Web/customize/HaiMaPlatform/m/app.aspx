<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMaPlatform.m.app" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>海马精英成长平台</title>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/common/menu.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/common/global.css" rel="stylesheet" />
    <link href="/lib/angularjs/carousel/angular-carousel.min.css" rel="stylesheet" />
    <link href="/lib/layer.mobile/need/layer.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/public/index/style.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/directives/footermenu/style.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/public/onlineClass/style.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/public/eliteStyle/style.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/public/onlineClassDetail/style.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/public/user/center/style.css" rel="stylesheet" />
    <link href="/customize/HaiMaPlatform/m/modules/public/user/editInfo/style.css" rel="stylesheet" />

    <script src="/lib/zepto/zepto.min.js"></script>
    <script src="/lib/classie/classie.js"></script>
    <script src="/lib/angularjs/1.3.15/angular.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-route.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-sanitize.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-touch.min.js"></script>
    <script src="/lib/angularjs/ngStorage/ngStorage.min.js"></script>
    <script src="/lib/angularjs/carousel/angular-carousel.min.js"></script>
    <script src="/lib/angular-bootstrap/ui-bootstrap-tpls-0.12.1.min.js"></script>
    <script src="/lib/layer.mobile/layer.m.js"></script>
    <script src="/customize/HaiMaPlatform/m/js/app.js"></script>
    <script src="/customize/HaiMaPlatform/m/js/services.js"></script>
    <script src="/customize/HaiMaPlatform/m/modules/common/pageBaseCtrl.js"></script>

    <script src="/customize/HaiMaPlatform/m/modules/public/index/indexCtrl.js"></script>
    <script src="/customize/HaiMaPlatform/m/modules/directives/footermenu/footermenu.js"></script>
    <script src="/customize/HaiMaPlatform/m/modules/public/onlineClass/onlineClassCtrl.js"></script>
    <script src="/customize/HaiMaPlatform/m/modules/public/eliteStyle/eliteStyleCtrl.js"></script>
    <script src="/customize/HaiMaPlatform/m/modules/public/onlineClassDetail/onlineClassDetailCtrl.js"></script>
    <script src="/customize/HaiMaPlatform/m/modules/public/user/center/userCtrl.js"></script>
    <script src="/customize/HaiMaPlatform/m/modules/public/user/editInfo/editInfoCtrl.js"></script>

    <script>
        window.alert = function (msg, theme, time, fn) {
            if (!time) {
                time = 5;
            }
            layer.open({
                content: msg,
                time: time
            });
            setTimeout(function () {
                fn();
            }, time * 1000);
        };
    </script>
</head>
<body>
    <div ng-app="haimamodule" ng-controller="pageBaseCtrl" class="pageBase" ng-class="{wrapQQBrowser:isQQBrowser}">
        <div ng-view></div>
    </div>
</body>
</html>
