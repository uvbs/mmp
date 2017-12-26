var basePath = '/Components/Coupon/Mobile/BeachHoney/';
var baseViewPath = basePath + 'src/';
var couponmodule = angular.module("couponmodule", ['ngRoute']);

couponmodule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
    .when('/couponList', {
        templateUrl: baseViewPath + 'public/couponList/tpls/template.html',
        controller: 'couponListCtrl'
    })
    .when('/couponDetail', {
        templateUrl: baseViewPath + 'public/couponDetail/tpls/template.html',
    })
        .when('/couponUse', {
            templateUrl: baseViewPath + 'public/couponUse/tpls/template.html',
            controller: 'couponUseCtrl'
        })
        .when('/couponUsed', {
            templateUrl: baseViewPath + 'public/couponUsed/tpls/template.html',
            controller: 'couponUsedCtrl'
        })
        .when('/couponOverdate', {
            templateUrl: baseViewPath + 'public/couponOverdate/tpls/template.html',
            controller: 'couponOverdateCtrl'
        })

    .otherwise({
        redirectTo: '/couponList'
    });
}]);
