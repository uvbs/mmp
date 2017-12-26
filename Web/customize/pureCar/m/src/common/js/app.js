var basePath = '/customize/pureCar/m/src/';
var baseServer = 'http://purecar.comeoncloud.net';
var apiPath = baseServer + '/serv/api/';
var publicPath = basePath + 'public/';
var pureCarModule = angular.module("pureCarModule", ['ngRoute', 'ngSanitize', 'ngStorage', 'ngDialog','angular-carousel']);

//TODO test login
$.get(apiPath + 'user/login.ashx?username=purecar&password=abcd@efg');

var baseData = {
    slogan: 'PureCar',
    handlerUrl: '/Serv/pubapi.ashx',
};

window.alert = function(msg){
    layer.open({
        content:msg
    });
};

var getData = function getData (path, args, onError) {
    var loadingIndex = layer.open({type:2});
    return $.get(apiPath + path, args).then(function (result) {
        layer.close(loadingIndex);
        if(!result || !result.isSuccess) {
            return $.Deferred().reject(onError ? onError(result) : alert(result.errmsg || result.errcode));
        }

        return result;
    });
}
var getReturnObj = function getReturnObj (path, args, onError) {
    return getData(path, args, onError).then(function(result) {
        return result.returnObj;
    });
}

pureCarModule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider

        //---------养车模块 start----------
        //首页
        .when('/service/index', {
            templateUrl: publicPath + 'service/index/tpls/index.html',
            controller: 'serviceIndexCtrl'
        })
        
        //我的订单
        .when('/service/orders', {
            templateUrl: publicPath + 'service/orders/tpls/index.html',
            controller: 'serviceOrdersCtrl'
        })
        
        //评价订单
        .when('/service/orders/:id/rate', {
            templateUrl: publicPath + 'service/orders/tpls/rate.html',
            controller: 'serviceOrderRateCtrl'
        })
        
        //订单详情
        .when('/service/orders/:id', {
            templateUrl: publicPath + 'service/orders/tpls/detail.html',
            controller: 'serviceOrderDetailCtrl'
        })
        
        //养护预约
        .when('/service/appointment', {
            templateUrl: publicPath + 'service/appointment/tpls/index.html',
            controller: 'serviceAppointmentCtrl'
        })
        
        .when('/service/appointment/endpoints', {
            templateUrl: publicPath + 'service/appointment/tpls/select-endpoint.html',
            controller: 'serviceSelectEndpointCtrl'
        })
        
        //爱车日志
        .when('/service/record', {
            templateUrl: publicPath + 'service/record/tpls/index.html',
            controller: 'serviceRecordCtrl'
        })
        
        //优惠券
        .when('/service/coupon', {
            templateUrl: publicPath + 'service/coupon/tpls/coupon.html',
            controller: 'serviceCouponCtrl'
        })
        
        //我的消息
        .when('/service/message', {
            templateUrl: publicPath + 'service/message/tpls/index.html',
            controller: 'serviceMessageCtrl'
        })
        
        //我的活动
        .when('/service/activity', {
            templateUrl: publicPath + 'service/activity/tpls/index.html',
            controller: 'serviceActivityCtrl'
        })
        
        
        //个人中心
        .when('/service/userhome', {
            templateUrl: publicPath + 'service/userhome/tpls/index.html',
            controller: 'serviceUserhomeCtrl'
        })
        
        //我的评价
        .when('/service/ratings', {
            templateUrl: publicPath + 'service/ratings/tpls/index.html',
            controller: 'serviceRatingsCtrl'
        })
        
        //车务服务
        .when('/service/traffic', {
            templateUrl: publicPath + 'service/traffic/tpls/index.html',
            controller: 'serviceTrafficCtrl'
        })

        //---------养车模块 end----------

        //部落购车
        .when('/store/index', {
            templateUrl: publicPath + 'store/index/tpls/index.html',
            controller: 'storeIndexCtrl'
        })

        //购车需求
        .when('/store/demand', {
            templateUrl: publicPath + 'store/demand/tpls/index.html',
            controller: 'storeDemandCtrl'
        })

        //我的订单
        .when('/store/orders', {
            templateUrl: publicPath + 'store/orders/tpls/index.html',
            controller: 'storeOrdersCtrl'
        })
        
        //评价订单
        .when('/store/orders/:id/rate', {
            templateUrl: publicPath + 'store/orders/tpls/rate.html',
            controller: 'storeOrderRateCtrl'
        })
        
        //订单详情
        .when('/store/orders/:id', {
            templateUrl: publicPath + 'store/orders/tpls/detail.html',
            controller: 'storeOrderDetailCtrl'
        })

        //优惠凭证
        .when('/store/voucher/:id', {
            templateUrl: publicPath + 'store/voucher/tpls/index.html',
            controller: 'storeVoucherCtrl'
        })

        //个人中心
        .when('/store/userhome', {
            templateUrl: publicPath + 'store/userhome/tpls/index.html',
            controller: 'storeUserhomeCtrl'
        })

        .when('/store/stores', {
            templateUrl: publicPath + 'store/stores/tpls/index.html',
            controller: 'storeListCtrl'
        })

        .when('/store/stores/:id', {
            templateUrl: publicPath + 'store/stores/tpls/detail.html',
            controller: 'storeDetailCtrl'
        })

        .when('/store/stores/:id/comment', {
            templateUrl: publicPath + 'store/stores/tpls/comment.html',
            controller: 'storeCommentCtrl'
        })

        .when('/store/consultation', {
            templateUrl: publicPath + 'store/Consultation/tpls/index.html'
        })

        .otherwise({
            redirectTo: '/store/index'
        });
}]);
