var basePath = '/customize/10000care/m/src/';
var publicPath = basePath + 'public/';
var comonModule = angular.module("comonModule", ['ngRoute', 'ngSanitize', 'ngStorage', 'ngDialog','angular-carousel']);

var baseData = {
    slogan: '阳光基地',
    handlerUrl: '/Serv/pubapi.ashx'
};

comonModule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider

        //首页
        .when('/index', {
            templateUrl: publicPath + 'index/tpls/index.html',
            controller: 'indexCtrl'
        })

        .otherwise({
            redirectTo: '/index'
        });
}]);
