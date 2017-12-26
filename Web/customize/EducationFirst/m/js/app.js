var basePath = "/customize/EducationFirst/m/";
var baseViewPath = basePath + "modules/";
var yingfumodule = angular.module("yingfumodule", ['ngRoute', 'ngSanitize', 'ngStorage', 'angular-carousel']);

var baseData = {
    slogan: '英孚',
    handlerUrl: '/Serv/pubapi.ashx',
    moduleCateIds: {

    }
};

yingfumodule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/index', {
            templateUrl: baseViewPath + 'public/index/tpls/index.html',
            controller: 'indexCtrl'
        })
        .otherwise({
            redirectTo: '/index'
        });
}]);