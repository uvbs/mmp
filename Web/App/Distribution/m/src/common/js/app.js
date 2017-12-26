var baseDomain = '/';
var basePath = '/App/Distribution/m/';
//var baseDomain = 'http://localhost:28241/';
//var basePath = '/customize/distribution/m/';
var baseViewPath = basePath + 'src/';
var distributionmodule = angular.module("distributionmodule", ['ngRoute','ngSanitize','ngStorage', 'angular-carousel','infinite-scroll']);
var baseData = {
    slogan: '',
    handlerUrl: baseDomain + 'serv/api/DistributionOffLine/',
    localStorageKeys: {
        //hotActivity: 'hotActivity'
    }
};

window.alert = function (msg, theme, time, fn) {
    if (!time) {
        time = 5;
    }
    layer.open({
        content: msg,
        time: time
    });
    setTimeout(function () {
        if(typeof(fn) == 'function') {
            fn();
        }
    }, time * 1000);
};

distributionmodule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider

        .when('/apply', {
            templateUrl: baseViewPath + 'public/apply/tpls/index.html',
            controller: 'applyCtrl'
            //controllerAs:'apply'
        })
        //未成为分销会员
        .when('/apply/:recommendId', {
            templateUrl: baseViewPath + 'public/apply/tpls/index.html',
            controller: 'applyCtrl',
            controllerAs:'apply'
        })
        //提现记录
        .when('/cashrecord', {
            templateUrl: baseViewPath + 'public/cashApply/cashRecord/tpls/index.html',
            controller: 'cashRecordCtrl',
            controllerAs:'cashrecord'
        })
        //银行卡列表
        .when('/cardlist', {
            templateUrl: baseViewPath + 'public/cashApply/cardList/tpls/index.html',
            controller: 'cardListCtrl',
            controllerAs:'cardlist'
        })
        //添加银行卡
        .when('/addcard/:bankCardId', {
            templateUrl: baseViewPath + 'public/cashApply/addCard/tpls/index.html',
            controller: 'addCardCtrl',
            controllerAs:'addcard'
        })
        //分销中心
        .when('/discenter', {
            templateUrl: baseViewPath + 'public/disCenter/tpls/index.html',
            controller: 'disCenterCtrl',
            controllerAs:'discenter'
        })
        //分销统计
        .when('/discount', {
            templateUrl: baseViewPath + 'public/disCount/tpls/index.html',
            controller: 'disCountCtrl',
            controllerAs:'discount'
        })
        //我的项目(我的直销数据)
        .when('/mystatic', {
            templateUrl: baseViewPath + 'public/myPro/myStatic/tpls/index.html',
            controller: 'myStaticCtrl',
            controllerAs:'mystatic'
        })
        //我的项目(我的直销数据详情)
        .when('/staticdetail/:projectId', {
            templateUrl: baseViewPath + 'public/myPro/staticDetail/tpls/index.html',
            controller: 'staticDetailCtrl',
            controllerAs:'staticdetail'
        })
        //我的项目(项目详情里面项目状态)
        .when('/prostatus/:projectId', {
            templateUrl: baseViewPath + 'public/myPro/proStatus/tpls/index.html',
            controller: 'proStatusCtrl',
            controllerAs:'prostatus'
        })
        //我的项目(财富商机)
        .when('/submitinfo', {
            templateUrl: baseViewPath + 'public/myPro/submitInfo/tpls/index.html',
            controller: 'submitInfoCtrl',
            controllerAs:'submitinfo'
        })
        .when('/sbinfo', {
            templateUrl: baseViewPath + 'public/myPro/submitInfo/tpls/index.html',
            controller: 'submitInfoCtrl',
            controllerAs:'submitinfo'
        })

        //我的推荐
        .when('/myrecommend', {
            templateUrl: baseViewPath + 'public/myRecommend/tpls/index.html',
            controller: 'myRecommendCtrl',
            controllerAs:'myrecommend'
        })
        //我的积分
        .when('/myscore', {
            templateUrl: baseViewPath + 'public/score/tpls/index.html',
            controller: 'scoreCtrl',
            controllerAs:'score'
        })

        .otherwise({
            redirectTo: '/discenter'
        })

}]);

distributionmodule.run(['$sessionStorage','commService', function ($sessionStorage,commService) {

        if(currWebsiteConfig != '$$fx-websiteConfig$$')
        {
            $sessionStorage.currWebsiteConfig = currWebsiteConfig;
        }else{
            $sessionStorage.currWebsiteConfig = 0;
        }

        if(currentUserInfo != '$$CURRENTUSERINFO$$')
        {
            $sessionStorage.currUseInfo = currentUserInfo.result;
            console.log('用户信息',$sessionStorage.currUseInfo);
            //判断是否是分销员，是的话直接跳到分销中心，不是跳到申请页面
           //alert($sessionStorage.currUseInfo.is_distribution_member);
            if($sessionStorage.currUseInfo.is_distribution_member){

                setTimeout(function(){
                    window.location.href = '#/discenter';
                },100)
            }
            else{
                if(window.location.href.indexOf('#/apply/')<0){
                    window.location.href = '#/apply';
                }
            }
        }
        else{
            $sessionStorage.currUseInfo=[];
        }

        setInterval(function(){
            commService.getCurrUserInfo({
                isnoloading:true
            },function(data){
                if(data.status){
                    $sessionStorage.currUseInfo=data.result;
                }
                else{
                    alert(data.msg);
                }
            },function(data){
                alert(data.msg);
            });
        },10000);

    if(document.referrer==''){
        //alert('上一页：'+document.referrer);

        if(!$sessionStorage.isShowWellcome) {
            $sessionStorage.isShowWellcome = true;
            //$('.wrapWellcome').css('display', 'block');
            setTimeout(function () {
                if($('.wrapWellcome'))
                $('.wrapWellcome').fadeOut();
            }, 2200);
        }else{
            $('.wrapWellcome').css('display', 'none');
        }

    }
}]);