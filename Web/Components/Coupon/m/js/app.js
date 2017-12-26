var basePath = '/Components/Coupon/m/';
var baseViewPath = basePath + 'src/';
var couponmodule = angular.module("couponmodule", ['ngRoute']);

couponmodule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
    .when('/couponList', {
        templateUrl: baseViewPath + 'public/couponList/tpls/index.html',
        controller: 'couponListCtrl'
    })
    .when('/couponDetail', {
        templateUrl: baseViewPath + 'public/couponDetail/tpls/index.html',
    })
        .when('/couponUse', {
            templateUrl: baseViewPath + 'public/couponUse/tpls/index.html',
        })
        .when('/couponUsed', {
            templateUrl: baseViewPath + 'public/couponUsed/tpls/index.html',
        })
        .when('/couponOverdate', {
            templateUrl: baseViewPath + 'public/couponOverdate/tpls/index.html',
        })

    .otherwise({
        redirectTo: '/couponList'
    });
}]);

//couponmodule.run(['commService', function (commService) {
//    commService.checkLogin(function (data) {
//        console.log('run');
//        console.log(data);
//    });

//}]);