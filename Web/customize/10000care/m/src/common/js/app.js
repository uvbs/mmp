var basePath = '/customize/10000care/m/src/';
var publicPath = basePath + 'public/';
var comeonModule = angular.module("comeonModule", ['ngRoute', 'ngSanitize', 'ngStorage', 'ngDialog', 'angular-carousel', 'ngTouch']);

var baseData = {
    slogan: '阳光基地',
    handlerUrl: '/Handler/WanBang/Wap.ashx'
};

comeonModule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider

        //首页
        .when('/index', {
            templateUrl: publicPath + 'index/tpls/index.html',
            controller: 'indexCtrl',
            title:'首页',
            requiredLogin:false
        })
        //登录
        .when('/login', {
            templateUrl: publicPath + 'login/tpls/index.html',
            controller: 'loginCtrl',
            title:'登陆',
            requiredLogin:false
        })
        //发布项目
        .when('/pubProject', {
            templateUrl: publicPath + 'pubProject/tpls/index.html',
            controller: 'pubProjectCtrl',
            title:'发布项目',
            requiredLogin:true
        })
        .when('/about', {
            templateUrl: publicPath + 'about/tpls/index.html',
            title:'介绍',
            requiredLogin:false
        })
        .when('/memorabilia', {
            templateUrl: publicPath + 'memorabilia/tpls/index.html',
            title:'大事记',
            requiredLogin:false
        })
        .when('/internet', {
            templateUrl: publicPath + 'internet/tpls/index.html',
            title:'互联网+ 阳光基地新型劳动项目',
            requiredLogin:false
        })
        .when('/internetDetail', {
            templateUrl: publicPath + 'internetDetail/tpls/index.html',
            title:'互联网+ 阳光基地新型劳动项目',
            requiredLogin:false
        })
        .when('/internetDetail/:id', {
            templateUrl: publicPath + 'internetDetail/tpls/index.html',
            title:'互联网+ 阳光基地新型劳动项目',
            controller:'internetDetailCtrl',
            requiredLogin:false
        })
        .when('/projectList', {
            templateUrl: publicPath + 'projectList/tpls/index.html',
            controller:'projectListCtrl',
            title:'传统项目列表',
            requiredLogin:false
        })
        .when('/projectList/:userid', {
            templateUrl: publicPath + 'projectList/tpls/index.html',
            controller:'projectListCtrl',
            title:'我的项目列表',
            requiredLogin:true
        })
        .when('/my', {
            templateUrl: publicPath + 'my/tpls/index.html',
            controller:'myCtrl',
            title:'基地中心',
            requiredLogin:true
        })
        .when('/baseEdit', {
            templateUrl: publicPath + 'baseEdit/tpls/index.html',
            controller:'baseEditCtrl',
            title:'基地信息修改',
            requiredLogin:true
        })
        .when('/companyEdit', {
            templateUrl: publicPath + 'companyEdit/tpls/index.html',
            controller:'companyEditCtrl',
            title:'企业信息修改',
            requiredLogin:true
        })
        .when('/signup', {
            templateUrl: publicPath + 'signup/tpls/index.html',
            controller: 'signupCtrl',
            title: '劳动项目对接洽谈会邀请函',
            requiredLogin: false
        })
        //Invitation1
        .when('/Invitation1', {
            templateUrl: publicPath + 'Invitation1/tpls/index.html',
            controller: 'Invitation1Ctrl',
            title: '邀请函',
            requiredLogin: false
        })
        .when('/Invitation2', {
            templateUrl: publicPath + 'Invitation2/tpls/index.html',
            controller: 'Invitation2Ctrl',
            title: '邀请函',
            requiredLogin: false
        })

        .otherwise({
            redirectTo: '/index'
        });

}]);

comeonModule.run(function ($rootScope, $location, $window) {

    // document.title = baseData.slogan;

    

    $rootScope.$on("$routeChangeStart", function (event, nextRoute, currentRoute) {
        document.title = nextRoute.title + ' - ' + baseData.slogan;
        var islogin = sessionStorage.getItem('islogin');
        if (nextRoute.requiredLogin && !islogin) {
            //alert('请先登录');
            window.location.hash = '#/login';
        }

    });

});