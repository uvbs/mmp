var basePath = '/customize/ubi/';
var baseViewPath = basePath + 'view/';
var ubimodule = angular.module("ubimodule", ['ngRoute', 'ui.bootstrap', 'ngSanitize', 'textAngular', 'ng.ueditor', 'angularFileUpload', 'angular-carousel', 'ngClipboard']);

var baseData = {
    slogan: '易劳，一站式的解决你的劳动法问题',
    handlerUrl: '/Serv/pubapi.ashx',
    loginHandlerUrl: '/Serv/loginapi.ashx',
    localStorageKeys: {
        hotActivity: 'hotActivity',
        currUserInfo: 'currUserInfo'
    }, //localStorage存放键值管理
    moduleCateIds:{

        // regulations:490,//政策法规
        // news:491,//新闻资讯
        // case:492,//案例分析

        regulations:84,//政策法规
        news:83,//新闻资讯
        case:79,//案例分析

        askquestion:81,//问答-提问
        askarticle:82,//问答-文章
        openclass: 86,//公开课
        asknotice: 89,//问答-公告

        //pureCar
        serverCateRoot: 113

    }
};
ubimodule.config(['ngClipProvider', function (ngClipProvider) {
    ngClipProvider.setPath("/lib/zeroclipboard-2.1.6/ZeroClipboard.swf");
}]);

ubimodule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/index', {
            templateUrl: baseViewPath + 'tpls/index/index.html',
            controller: 'indexCtrl'
        })
         .when('/indexlo', {
            templateUrl: baseViewPath + 'tpls/index/login.html'
        })
          .when('/indexba', {
            templateUrl: baseViewPath + 'tpls/index/back.html'
        })

    /* activity module start */
    .when('/activity', {
            templateUrl: baseViewPath + 'tpls/activity/index.html',
            controller: 'activityCtrl'
        })
        .when('/activity/:aid', {
            templateUrl: baseViewPath + 'tpls/activity/detail.html',
            controller: 'activityDetailCtrl'
        })
        .when('/activityList', {
            templateUrl: baseViewPath + 'tpls/activity/list.html',
            controller: 'activityListCtrl'
        }).when('/activityList/:searchkey', {
            templateUrl: baseViewPath + 'tpls/activity/list.html',
            controller: 'activityListCtrl'
        })
        /* activity module end */

    .when('/ask', {
            templateUrl: baseViewPath + 'tpls/ask/index.html',
            controller:'askCtrl'
    })
        .when('/askTag/:tag', {
            templateUrl: baseViewPath + 'tpls/ask/index.html',
            controller: 'askCtrl'
        })
    .when('/askqu', {
            templateUrl: baseViewPath + 'tpls/ask/question.html'
        })
    .when('/askArticleDetail/:id', {
            templateUrl: baseViewPath + 'tpls/ask/articleDetail.html',
            controller:'askArticleDetailCtrl'
    })
        .when('/askArticleDetail/:id/:isnotice', {
            templateUrl: baseViewPath + 'tpls/ask/articleDetail.html',
            controller: 'askArticleDetailCtrl'
        })
    .when('/ask/:id', {
            templateUrl: baseViewPath + 'tpls/ask/askDetail.html',
            controller:'askArticleDetailCtrl'
        })
    .when('/askAdd/:type', {
            templateUrl: baseViewPath + 'tpls/ask/add.html',
            controller:'askAddCtrl'
        })
    .when('/askti', {
            templateUrl: baseViewPath + 'tpls/ask/title.html'
        })




        .when('/case', {
            templateUrl: baseViewPath + 'tpls/case/index.html',
            controller:'caseCtrl'
        })
        .when('/caseTag/:tag', {
            templateUrl: baseViewPath + 'tpls/case/index.html',
            controller: 'caseCtrl'
        })

        .when('/case/:id', {
            templateUrl: baseViewPath + 'tpls/case/details.html',
            controller:'caseDetailCtrl'
        })
        .when('/addCase', {
            templateUrl: baseViewPath + 'tpls/case/announce.html',
            controller:'addCaseCtrl'
        })



        .when('/master', {
            templateUrl: baseViewPath + 'tpls/master/index.html',
            controller:'masterCtrl'
        })
         .when('/masterpe', {
            templateUrl: baseViewPath + 'tpls/master/man.html'
        })
          .when('/masterfa', {
            templateUrl: baseViewPath + 'tpls/master/fans.html'
        })


           .when('/user', {
            templateUrl: baseViewPath + 'tpls/user/index.html'
        })
            .when('/userps', {
            templateUrl: baseViewPath + 'tpls/user/password.html'
        })
            .when('/userhi', {
            templateUrl: baseViewPath + 'tpls/user/history.html'
        })
            .when('/userin', {
            templateUrl: baseViewPath + 'tpls/user/information.html'
        })
             .when('/userap', {
            templateUrl: baseViewPath + 'tpls/user/applyman.html'
        })
            .when('/userme', {
            templateUrl: baseViewPath + 'tpls/user/member.html'
        })
            .when('/userre', {
            templateUrl: baseViewPath + 'tpls/user/recharge.html'
        })
        .when('/news', {
            templateUrl: baseViewPath + 'tpls/news/index.html',
            controller:'newsCtrl'
        })
        .when('/newsTag/:tag', {
            templateUrl: baseViewPath + 'tpls/news/index.html',
            controller: 'newsCtrl'
        })
        .when('/news/:id', {
            templateUrl: baseViewPath + 'tpls/news/details.html',
            controller:'newsDetailCtrl'
        })
        //公开课
        .when('/open', {
            templateUrl: baseViewPath + 'tpls/open/index.html',
            controller: 'openListCtrl'
        })
        .when('/open/:id', {
            templateUrl: baseViewPath + 'tpls/open/detail.html',
            controller: 'openDetailCtrl'
        }).when('/open/:searchkey', {
            templateUrl: baseViewPath + 'tpls/open/index.html',
            controller: 'activityListCtrl'
        })
        .when('/regulations', {
            templateUrl: baseViewPath + 'tpls/regulations/index.html',
            controller:'regulationsCtrl'
        })
        .when('/regulationsTag/:tag', {
            templateUrl: baseViewPath + 'tpls/regulations/index.html',
            controller: 'regulationsCtrl'
        })
        .when('/regulations/:id', {
            templateUrl: baseViewPath + 'tpls/regulations/details.html',
            controller:'regulationsDetailCtrl'
        })
        .when('/update', {
            templateUrl: baseViewPath + 'tpls/update/index.html',
            controller:'updateCtrl'
        })
        .when('/update/:cateId', {
            templateUrl: baseViewPath + 'tpls/update/index.html',
            controller: 'updateCtrl'
        })
        .when('/userspace/:id', {
            templateUrl: baseViewPath + 'tpls/master/person.html',
            controller: 'userspaceCtrl'
        }) 
        .when('/passwordback', {
            templateUrl: baseViewPath + 'tpls/index/back.html',
            controller: 'pwdbackCtrl'
        })
        .when('/register', {
            templateUrl: baseViewPath + 'tpls/index/login.html',
            controller: 'registerCtrl'
        })
        .when('/register/:pid', {
            templateUrl: baseViewPath + 'tpls/index/login.html',
            controller: 'registerCtrl'
        })
        .when('/information/:id', {
            templateUrl: baseViewPath + 'tpls/user/information.html',
            controller: 'informationCtrl'
        })
        .when('/applyman/:id', {
            templateUrl: baseViewPath + 'tpls/user/applyman.html',
            controller: 'applymanCtrl'
        })
        .when('/applylawyer/:id', {
            templateUrl: baseViewPath + 'tpls/user/applyLawyer.html',
            controller: 'applylawyerCtrl'
        })
        .when('/password/:id', {
            templateUrl: baseViewPath + 'tpls/user/password.html',
            controller: 'passwordCtrl'
        })
        .when('/edituser/:id', {
            templateUrl: baseViewPath + 'tpls/user/index.html',
            controller: 'edituserCtrl'
        })
        .when('/history/:id', {
            templateUrl: baseViewPath + 'tpls/user/history.html',
            controller: 'historyCtrl'
        })
        .when('/contact/:id', {
            templateUrl: baseViewPath + 'tpls/user/member.html',
            controller: 'contactCtrl'
        })
        .when('/about/:id', {
            templateUrl: baseViewPath + 'tpls/about/index.html',
            controller: 'aboutCtrl'
        })
        .when('/recharge/:id', {
            templateUrl: baseViewPath + 'tpls/user/rechargeBuy.html',
            controller: 'rechargeCtrl'
        })
        .otherwise({
            redirectTo: '/index'
        });
}]);
