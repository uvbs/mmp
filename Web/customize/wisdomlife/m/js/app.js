var basePath = '/customize/wisdomlife/m/';
var baseViewPath = basePath + 'src/';
var wisdomlifemodule = angular.module("wisdomlifemodule", ['ngRoute', 'ngSanitize', 'angular-carousel']);

var baseData = {
    slogan: '智慧生活',
    handlerUrl: '/Serv/pubapi.ashx',
    localStorageKeys: {
        hotActivity: 'hotActivity'
    },
    moduleCateIds: {
        //advertiser: 108,//广告主
        //selfmedia: 109,//自媒体
        //news: 110,//资讯
        advertiser: 523,//广告主
        selfmedia: 524,//自媒体
        news: 525,//资讯
        rank: '',//英雄榜
        openclass: 86,//公开课
        case: 545
    }
}


wisdomlifemodule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
    .when('/index', {
        templateUrl: baseViewPath + 'public/index/tpls/index.html',
        controller: 'indexCtrl'
    })
        .when('/index/:id', {
            templateUrl: baseViewPath + 'public/index/tpls/index.html',
            controller: 'indexCtrl'
        })

        .when('/takeAd', {
            templateUrl: baseViewPath + 'public/takeAd/tpls/index.html',
            controller: 'takeAdCtrl'
        })
        .when('/putAd', {
            templateUrl: baseViewPath + 'public/putAd/tpls/index.html',
            controller: 'putAdCtrl'
        })
        .when('/selfMedia', {
            templateUrl: baseViewPath + 'public/selfMedia/tpls/index.html',
            controller: 'selfMediaCtrl'
        })
         .when('/advertiser', {
             templateUrl: baseViewPath + 'public/advertiser/tpls/index.html',
             controller: 'advertiserCtrl'
         })
         .when('/list/:cateId', {
             templateUrl: baseViewPath + 'public/advertiser/tpls/index.html',
             controller: 'advertiserCtrl'
         })
        .when('/list/:cateId/:moduleName', {
            templateUrl: baseViewPath + 'public/advertiser/tpls/index.html',
            controller: 'advertiserCtrl'
        })
        .when('/rank', {
            templateUrl: baseViewPath + 'public/rank/tpls/index.html',
            controller: 'rankCtrl'
        })
        .when('/rank/:id', {
            templateUrl: baseViewPath + 'public/rank/tpls/index.html',
            controller: 'rankCtrl'
        })

        .when('/selfMediaDetail/:id', {
            templateUrl: baseViewPath + 'public/selfMediaDetail/tpls/index.html',
            controller: 'selfMediaDetailCtrl'
        })
         .when('/advertiserDetail/:id', {
             templateUrl: baseViewPath + 'public/advertiserDetail/tpls/index.html',
             controller: 'advertiserDetailCtrl'
         })
        .when('/newsDetail/:id', {
            templateUrl: baseViewPath + 'public/newsDetail/tpls/index.html',
            controller: 'newsDetailCtrl'
        })

        .when('/case', {
            templateUrl: baseViewPath + 'public/case/tpls/index.html',
            controller: 'caseCtrl'
        })
        .when('/case/:id', {
            templateUrl: baseViewPath + 'public/caseDetail/tpls/index.html',
            controller: 'caseDetailCtrl'
        })
        .when('/case/comm/:cateId', {
            templateUrl: baseViewPath + 'public/case/tpls/index.html',
            controller: 'caseCtrl'
        })


    .otherwise({
        redirectTo: '/index'
    });
}]);