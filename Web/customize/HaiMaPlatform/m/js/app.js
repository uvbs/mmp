var basePath="/customize/HaiMaPlatform/m/";
var baseViewPath=basePath+"modules/";
var haimamodule=angular.module("haimamodule",['ngRoute','ngSanitize', 'ngStorage','angular-carousel']);

var baseData={
    slogan:'海马精英成长平台',
    handlerUrl:'/Serv/pubapi.ashx',
    moduleCateIds:{
        news:'83',
        hotclass:'114',
        knowledge:'113'
    }
};

haimamodule.config(['$routeProvider',function($routeProvider){
    $routeProvider
        .when('/index',{
            templateUrl:baseViewPath+'public/index/tpls/index.html',
            controller:'indexCtrl'
        })
        .when('/onlineClass',{
            templateUrl:baseViewPath+'public/onlineClass/tpls/index.html',
            controller:'onlineClassCtrl'
        })
        .when('/eliteStyle',{
            templateUrl:baseViewPath+'public/eliteStyle/tpls/index.html',
            controller:'eliteStyleCtrl'
        })
        .when('/onlineClassDetail',{
            templateUrl:baseViewPath+'public/onlineClassDetail/tpls/index.html',
            controller:'onlineClassDetailCtrl'
        })
        .when('/onlineClassDetail/:id',{
            templateUrl:baseViewPath+'public/onlineClassDetail/tpls/index.html',
            controller:'onlineClassDetailCtrl'
        })
        .when('/center',{
            templateUrl:baseViewPath+'public/user/center/tpls/index.html',
            controller:'userCtrl'
        })
        .when('/editInfo',{
            templateUrl:baseViewPath+'public/user/editInfo/tpls/index.html',
            controller:'editInfoCtrl'
        })
        .otherwise({
            redirectTo:'/index'
        });
}]);