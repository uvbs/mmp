var skymemory = angular.module("skymemory", ['ngRoute']);
skymemory.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
    .when('/about', {
        templateUrl: 'tpls/about.html',
        controller: 'aboutCtrl'
    })
    .when('/film', {
        templateUrl: 'tpls/film.html',
        controller: 'filmCtrl'
    })
    .when('/update', {
        templateUrl: 'tpls/update.html',
        controller: 'updateCtrl'
    })
    .when('/articleList/:cateid/:title', {
        templateUrl: 'tpls/articleList.html',
        controller: 'articleListCtrl'
    })
    .when('/filmDetail', {
        templateUrl: 'tpls/filmDetail.html',
        controller: 'filmDetailCtrl'
    })
    .otherwise({
        redirectTo: '/about'
    });
}]);


function stringToHex(str){
　　　　var val="";
　　　　for(var i = 0; i < str.length; i++){
　　　　　　if(val == "")
　　　　　　　　val = str.charCodeAt(i).toString(16);
　　　　　　else
　　　　　　　　val += "," + str.charCodeAt(i).toString(16);
　　　　}
　　　　return val;
　　}