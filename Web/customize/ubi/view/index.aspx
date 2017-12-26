<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.ubi.view.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>易劳-权威的劳动法信息服务平台</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width" />

    <!-- TODO:发布打包 -->
    <link type="text/css" href="/lib/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />
    <link type="text/css" href="/lib/fontawesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" />
<%--    <link href="/customize/ubi/css/global.css" rel="stylesheet" />
    <link href="/customize/ubi/css/style.css" rel="stylesheet" />--%>
    <link type="text/css" href="/lib/angularjs/textAngular/textAngular.css" rel="stylesheet" />
    <link href="/lib/angularjs/carousel/angular-carousel.min.css" rel="stylesheet" />
    <link type="text/css" href="/customize/ubi/css/all.css" rel="stylesheet" />
    <!-- 发布打包 -->
    
    <!-- TODO:发布打包 -->
    <script src="/lib/ueditor/ueditor.config.js"></script>
    <script src="/lib/ueditor/ueditor.all.js"></script>
    <script src="/lib/jquery/jquery-2.1.1.min.js"></script>
    <script src="/lib/zeroclipboard-2.1.6/ZeroClipboard.js"></script>
    <script src="/lib/jquery/jquery-2.1.1.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-route.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-touch.min.js"></script>
    <script src="/lib/angularjs/1.3.15/angular-sanitize.min.js"></script>
    <script src="/lib/angularjs/textAngular/textAngular-rangy.min.js"></script>
    <script src="/lib/angularjs/textAngular/textAngular.min.js"></script>
    <script src="/lib/angularjs/textAngular/textAngularSetup.js"></script>
    <script src="/lib/angularjs/carousel/angular-carousel.min.js"></script>
    <script src="/lib/angular-bootstrap/ui-bootstrap-tpls-0.12.1.min.js"></script>
    <script src="/lib/ueditor/angular-ueditor.min.js"></script>
    <script src="/lib/angularjs/angularjsUpload/angular-file-upload.min.js"></script>
    <script src="/lib/ng-clip-0.2.6/src/ngClip.js"></script>
    <script src="/lib/zeroclipboard-2.1.6/ZeroClipboard.js"></script>

    <script src="/customize/ubi/js/global.js"></script>
  
    <script src="/customize/ubi/js/app.js"></script>
    <script src="/customize/ubi/js/services.js"></script>
    <script src="/customize/ubi/js/directives.js"></script>
    <script src="/customize/ubi/js/controllers.js"></script>
    <script src="/customize/ubi/js/controllers/masterCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/indexCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/activityCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/activityListCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/activityDetailCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/updateCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/caseCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/newsCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/regulationsCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/userspaceCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/askCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/registerCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/informationCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/applymanCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/passwordCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/pwdbackCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/historyCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/edituserCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/contactCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/aboutCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/rechargeCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/openCtrl.js"></script>
    <script src="/customize/ubi/js/controllers/applylawyerCtrl.js"></script>
    <!-- 发布打包 -->
    

</head>
<body>
    
    <div ng-app="ubimodule" ng-controller="pageBaseCtrl">
        <ubiheader></ubiheader>
        <div ng-view></div>
        <ubifooter></ubifooter>
    </div>
</body>
</html>
