var basePath = '/customize/ubi/m/';
var baseViewPath = basePath + 'modules/';
var ubimodule = angular.module("ubimodule", ['ngRoute', 'ngSanitize', 'ngStorage', 'ngDialog', 'angularFileUpload','angular-carousel']);

var baseData = {
    slogan: '权威的劳动法信息服务平台',
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
    }
};

ubimodule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/index', {
            templateUrl: baseViewPath + 'public/index/tpls/index.html',
            controller:'indexCtrl'
        })
        .when('/reg', {
            templateUrl: baseViewPath + 'public/reg/tpls/index.html',
            controller: "regCtrl"        
        })
        .when('/reg/:id', {
            templateUrl: baseViewPath + 'public/reg/tpls/index.html',
            controller: "regCtrl"
        })
        .when('/login', {
            templateUrl: baseViewPath + 'public/login/tpls/index.html',
            controller:'loginCtrl'
        })
        .when('/login/:ref', {
            templateUrl: baseViewPath + 'public/login/tpls/index.html',
            controller:'loginCtrl'
        })
        .when('/msg', {
            templateUrl: baseViewPath + 'public/msg/tpls/index.html',
            controller:'msgCtrl'
        })
        
        .when('/open', {
            templateUrl: baseViewPath + 'public/open/tpls/list.html',
            controller:'openCtrl'
        })

        .when('/open/:id', {
            templateUrl: baseViewPath + 'public/open/tpls/detail.html',
            controller:'openDetailCtrl'
        })

        .when('/news', {
            templateUrl: baseViewPath + 'public/news/list/tpls/index.html',
            controller: 'newsCtrl'
        })
        .when('/newlist/:tag', {
            templateUrl: baseViewPath + 'public/news/list/tpls/index.html',
            controller:'newsCtrl'
        })
        .when('/news/:id',{
            templateUrl:baseViewPath + 'public/news/detail/tpls/index.html',
            controller:'newsDetailCtrl'
        })
        //1_0_3_83_1_1   6个参数分别表示ng-attr-hassearch（搜索）  ng-attr-hasimg（图片） ng-attr-page ng-attr-otherCateId（cateId）
        // ng-attr-.hasTag(是否有标签)  ng-attr-hassummary （是否显示summary）
         .when('/newsList/:id', {
             templateUrl: baseViewPath + 'public/news/list/tpls/index.html',
             controller: 'newsCtrl'
         })

        .when('/case', {
            templateUrl: baseViewPath + 'public/case/list/tpls/index.html'
        })
        .when('/caselist/:tag', {
            templateUrl: baseViewPath + 'public/case/list/tpls/index.html',
            controller: 'caseCtrl'
        })
        .when('/case/:id', {
            templateUrl: baseViewPath + 'public/case/detail/tpls/index.html',
            controller: 'caseDetailCtrl'
        })

        .when('/regulations', {
            templateUrl: baseViewPath + 'public/regulations/list/tpls/index.html'
        })
        .when('/regulationslist/:tag', {
            templateUrl: baseViewPath + 'public/regulations/list/tpls/index.html',
            controller:'regulationsCtrl'
        })
        .when('/regulations/:id', {
            templateUrl: baseViewPath + 'public/regulations/detail/tpls/index.html',
            controller: 'regulationsDetailCtrl'
        })

        .when('/ask', {
            templateUrl: baseViewPath + 'public/ask/list/tpls/index.html',
            controller: 'askListCtrl'
        })
        .when('/ask/:id', {
            templateUrl: baseViewPath + 'public/ask/detail/tpls/index.html',
            controller: 'askDetailCtrl'
        })
        .when('/askqu', {
            templateUrl: baseViewPath + 'public/ask/add/tpls/addQuestion.html',
            controller: 'askAddCtrl'
        })
        //.when('/askArticleDetail/:id', {
        //    templateUrl: baseViewPath + 'tpls/ask/articleDetail.html',
        //    controller: 'askArticleDetailCtrl'
        //})
        .when('/askArticle', {
            templateUrl: baseViewPath + 'public/ask/add/tpls/addArticle.html',
            controller: 'askAddCtrl'
        })
        //.when('/askti', {
        //    templateUrl: baseViewPath + 'tpls/ask/title.html'
        //})     


        .when('/update', {
            templateUrl: baseViewPath + 'public/update/list/tpls/index.html',
            controller:'updateListCtrl'
        })
        .when('/update/:id', {
            templateUrl: baseViewPath + 'public/update/detail/tpls/index.html',
            controller: 'updateDetailCtrl'
        })
        //外面项目调用时的路由，outParams为传入参数，包括多个参数，以下划线分隔，当一个参数时为家长社区，
        // 当为两个参数时为校区,第一个参数为cateId，第二个参数可为任意值
        .when('/update/:outParams', {
            templateUrl: baseViewPath + 'public/update/list/tpls/index.html',
            controller:'updateListCtrl'
        })

        .when('/activity', {
            templateUrl: baseViewPath + 'public/activity/list/tpls/index.html'
        })

        .when('/master', {
            templateUrl: baseViewPath + 'public/master/tpls/index.html',
            controller:'masterCtrl'
        })
        .when('/otherMaster/:id', {
            templateUrl: baseViewPath + 'public/master/tpls/index.html',
            controller:'masterCtrl'
        })
        .when('/master/:id', {
            templateUrl: baseViewPath + 'public/master/tpls/detail.html',
            controller:'masterDetailCtrl'
        })
        .when('/userCaseList/:id', {
            templateUrl: baseViewPath + 'public/master/tpls/userCaseList.html',
            controller:'userCaseListCtrl'
        })
        .when('/userArticleList/:id', {
            templateUrl: baseViewPath + 'public/master/tpls/userArticleList.html',
            controller:'userArticleListCtrl'
        })
        .when('/educationFirst', {
            templateUrl: baseViewPath + 'public/EducationFirst/tpls/index.html',
            controller: 'educationFirstCtrl'
        })
        
        //-----------------------个人中心模块------------------------
        .when('/my', {
            templateUrl: baseViewPath + 'public/user/center/tpls/index.html',
            controller:'userCtrl'
        })
        .when('/edituser', {
            templateUrl: baseViewPath + 'public/user/edit/tpls/index.html',
            controller:'userEditCtrl'
        })
        .when('/userPoints', {
            templateUrl: baseViewPath + 'public/user/points/tpls/index.html',
            controller:'userPointsCtrl'
        })
        .when('/myFollow', {
            templateUrl: baseViewPath + 'public/user/myFollow/tpls/myFollow.html',
            controller:'myFollowCtrl'
        })
        .when('/myFans', {
            templateUrl: baseViewPath + 'public/user/myFans/tpls/index.html',
            controller:'myFansCtrl'
        })
        .when('/myFollow/:id', {
            templateUrl: baseViewPath + 'public/user/myFollow/tpls/myFollow.html',
            controller:'myFollowCtrl'
        })
        .when('/myFans/:id', {
            templateUrl: baseViewPath + 'public/user/myFans/tpls/index.html',
            controller:'myFansCtrl'
        })

        .when('/myAsk/:id/:type', {
            templateUrl: baseViewPath + 'public/user/myAsk/tpls/index.html',
            controller: 'myAskCtrl'
        })
        .when('/AskList/:tag', {
            templateUrl: baseViewPath + 'public/user/myAsk/tpls/index.html',
            controller: 'myAskCtrl'
        })

        .when('/myUpdate', {
            templateUrl: baseViewPath + 'public/user/myUpdate/tpls/index.html',
            controller:'myUpdateCtrl'
        })

        .when('/myArticle', {
            templateUrl: baseViewPath + 'public/user/myArticle/tpls/index.html'
        })
        .when('/favoriteArticle/:id', {
            templateUrl: baseViewPath + 'public/user/favoriteArticle/tpls/index.html',
            controller: 'favoriteArticleCtrl'
        })
        .when('/myAnswer/:id', {
            templateUrl: baseViewPath + 'public/user/myAnswer/tpls/index.html',
            controller: 'myAnswerCtrl'
        })
        .when('/about/:id', {
            templateUrl: baseViewPath + 'public/about/tpls/index.html',
            controller: 'aboutCtrl'
        })
        .when('/followAsk', {
            templateUrl: baseViewPath + 'public/user/followAsk/tpls/index.html'
        })
        .when('/shareRegLink/:id', {
            templateUrl: baseViewPath + 'public/shareRegLink/tpls/index.html',
            controller: 'shareRegLinkCtrl'
        })
        //------------------------------------------------------------

        .otherwise({
            redirectTo: '/index'
        });
}]);

ubimodule.run(['commService',function (commService) {
    commService.checkLogin(function(data){
        console.log('run');
        console.log(data);
    });

}]);
