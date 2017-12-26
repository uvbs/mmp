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


}]);
distributionmodule.controller("pageBaseCtrl", ['$scope', '$sessionStorage', 'commService', function ($scope, $sessionStorage, commService) {
    $scope.go = function (url) {
        window.location.href = url;
    };

    $scope.goback = function () {
        history.go(-1);
    };

    var ua = navigator.userAgent;

    if (ua.indexOf('MQQBrowser') > 0) {
        $scope.isQQBrowser = true;
    }

    //commService.getCurrUserInfo({
    //},function(data){
    //    if(data.status){
    //        $scope.shareFunc(data.result.recommend_id);
    //    }else{
    //        alert(data.msg);
    //    }
    //},function(data){
    //    alert(data.msg);
    //});

    $scope.shareFunc=function(id){
        //commService.getDistributeConfig({}, function (data) {
        //    if (data.status) {
        //        //if ($sessionStorage.disConfig == undefined) {
        //        //    $sessionStorage.disConfig = [];
        //        //}
        //        //$sessionStorage.disConfig = data.result;
        //        $scope.code_url = 'http://' + window.location.host + '/App/Distribution/m/index.aspx?ngroute=/apply/' + id + '#/apply/' + id;
        //        console.log('分销级数1', data.result);
        //        wxapi.wxshare({
        //            title: data.result.share_title,
        //            desc: data.result.share_desc,
        //            imgUrl: data.result.share_img_url,
        //            link: $scope.code_url
        //        }, '')
        //        //wx.ready(function () {
        //        //
        //        //});
        //    }
        //    else {
        //        alert(data.msg);
        //    }
        //}, function (data) {
        //    alert(data.msg);
        //});
    };

}]);
distributionmodule.factory('commService', ['$http', '$rootScope', '$location', '$anchorScroll', '$sce',
        function ($http, $rootScope, $modal, $location, $anchorScroll, $sce) {
            var commService = {};
            //新接口处理方法
            /**
             * [timeShow 时间展示处理]
             * @param  {[type]} time [description]
             * @return {[type]}      [description]
             */
            commService.timeShow = function (timeValue) {

                //几分钟前  几小时前  几天前  几月前 超过一年的则展现原数据
                var time = new Date(timeValue);
                var now = new Date();
                var diffValue = now.getTime() - time.getTime();

                var minute = 1000 * 60;
                var hour = minute * 60;
                var day = hour * 24;
                var halfamonth = day * 15;
                var month = day * 30;
                var year = 365 * day;

                var yearC = diffValue / year;
                var monthC = diffValue / month;
                var weekC = diffValue / (7 * day);
                var dayC = diffValue / day;
                var hourC = diffValue / hour;
                var minC = diffValue / minute;

                if (yearC >= 1) {
                    // result = time.format("yyyy-MM-dd hh:mm");
                }
                if (monthC >= 1) {
                    result = "" + parseInt(monthC) + "个月前";
                } else if (weekC >= 1) {
                    result = "" + parseInt(weekC) + "周前";
                } else if (dayC >= 1) {
                    result = "" + parseInt(dayC) + "天前";
                } else if (hourC >= 1) {
                    result = "" + parseInt(hourC) + "小时前";
                } else if (minC >= 1) {
                    result = "" + parseInt(minC) + "分钟前";
                } else {
                    result = "刚刚";
                }
                return result;
                //return result;
            };
            /**
             * [serializeData 序列化对象数据]
             * @param  {[type]} data [description]
             * @return {[type]}      [description]
             */
            commService.serializeData = function (data) {
                // If this is not an object, defer to native stringification.
                if (!angular.isObject(data)) {
                    return ((data == null) ? "" : data.toString());
                }

                var buffer = [];

                // Serialize each key in the object.
                for (var name in data) {
                    if (!data.hasOwnProperty(name)) {
                        continue;
                    }

                    var value = data[name];

                    buffer.push(
                        encodeURIComponent(name) + "=" + encodeURIComponent((value == null) ? "" : value)
                    );
                }

                // Serialize the buffer and clean it up for transportation.
                var source = buffer.join("&").replace(/%20/g, "+");
                return (source);
            };
            /**
             * [urlParams get参数处理]
             * @param  {[type]} action      [description]
             * @param  {[type]} jsonData [description]
             * @return {[type]}          [description]
             */
            commService.urlParams = function (action, jsonData) {
                var url = baseData.handlerUrl + action;
                if (typeof jsonData === 'object') {
                    url += '?';
                    for (var key in jsonData) {
                        url += key + '=' + jsonData[key] + '&';
                    }
                    url = url.slice(0, url.length - 1);
                }
                return url;

                //var result = newurl + '?';
                ////var result = url + '?';
                //var i = 0;
                //var keys = Object.keys(jsonData);
                //for (var i = 0; i < keys.length; i++) {
                //    if (i != 0) {
                //        result += '&';
                //    }
                //    result += keys[i] + '=' + jsonData[keys[i]];
                //}
                //return result;
            };
            commService.loadRemoteData = function (action, reqData, callBack, failCallBack) {
                if(reqData.isnoloading){
                    
                }else{
                    layer.open({type: 2});
                }
                $http.get(commService.urlParams(action, reqData)).success(function (data) {
                    layer.closeAll();
                    callBack(data);
                }).error(function (data) {
                    layer.closeAll();
                    failCallBack(data);
                });
            };

            commService.get =commService.loadRemoteData;

            commService.postData = function (url, reqData, callBack, failCallBack) {
                layer.open({type: 2});
                $http({
                    method: 'POST',
                    url: url,
                    data: commService.serializeData(reqData),
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).success(function (data) {
                    layer.closeAll();
                    callBack(data)
                });
            };

            //新接口处理方法 end

            //commService.extend = function (reqData, option) {
            //    var keys = Object.keys(option);
            //    for (var i = 0; i < keys.length; i++) {
            //        if (typeof(option[keys[i]]) != 'undefined')
            //            reqData[keys[i]] = option[keys[i]];
            //    }
            //};

            //添加银行卡接口(post)
            commService.addBankCard = function(option, callBack, failCallBack) {
                var action='bankcard/add.ashx';
                commService.postData(baseData.handlerUrl+action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取银行卡列表接口(get)
            commService.getBankCardList = function(option, callBack, failCallBack) {
                var action='bankcard/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取银行卡详情接口(get)
            commService.getBankCardDetail = function(option, callBack, failCallBack) {
                var action='bankcard/get.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //修改银行卡接口(post)
            commService.updateBankCard = function(option, callBack, failCallBack) {
                var action='bankcard/update.ashx';
                commService.postData(baseData.handlerUrl+action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //删除银行卡接口(get)
            commService.deleteBankCard = function(option, callBack, failCallBack) {
                var action='bankcard/delete.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };

            //添加项目接口(get)
            commService.addProject = function(option, callBack, failCallBack) {
                var action='project/add.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取项目接口(get)
            commService.getProjectList = function(option, callBack, failCallBack) {
                var action='project/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //申请提现接口(get)
            commService.applyForCash = function(option, callBack, failCallBack) {
                var action='WithdrawCash/add.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //申请提现记录接口(get)
            commService.applyForCashRecord = function(option, callBack, failCallBack) {
                var action='withdrawcash/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取当前用户信息接口(get)
            commService.getCurrUserInfo = function(option, callBack, failCallBack) {
                var action='user/currentuserinfo.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取用户下级分销接口(get)
            commService.getUserNextLevel = function(option, callBack, failCallBack) {
                var action='user/down/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //分销配置接口(get)
            commService.getDistributeConfig = function(option, callBack, failCallBack) {
                var action='config/get.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //生成二维码接口(get)  http://dev1.comeoncloud.net/serv/api/common/qrcode.ashx?code=http://www.qq.com
            //commService.getQrCode = function(option, callBack, failCallBack) {
            //    var url=baseDomain+'serv/api/common/qrcode.ashx?code='+option;
            //    $http.get(url).success(function (data) {
            //        layer.closeAll();
            //        callBack(data);
            //    }).error(function (data) {
            //        layer.closeAll();
            //        failCallBack(data);
            //    });
            //};
            //活动报名接口(post)
            commService.getQrCode = function(option, callBack, failCallBack) {
                commService.postData(baseDomain+'serv/api/common/qrcode.ashx', option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取线下分销申请活动ID接口(get)
            commService.getDisActivity = function(option, callBack, failCallBack) {
                var action='User/ApplyActivity.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取项目日志列表接口(get)
            commService.getProjectLog = function(option, callBack, failCallBack) {
                var action='projectlog/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //活动报名接口(post)
            commService.activityRegist = function(option, callBack, failCallBack) {
                commService.postData(baseDomain+'serv/ActivityApiJson.ashx', option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取用户当前信息详细接口(get)  http://fenxiao.comeoncloud.net/serv/api/user/info.ashx?action=currentuserinfo
            commService.getUserDetail = function(callBack, failCallBack) {
                var url=baseDomain+'serv/api/user/info.ashx?action=currentuserinfo';
                $http.get(url).success(function (data) {
                    layer.closeAll();
                    callBack(data);
                }).error(function (data) {
                    layer.closeAll();
                    failCallBack(data);
                });
            };
            //获取项目分佣记录(积分记录)接口(get)
            commService.getScore = function(option, callBack, failCallBack) {
                var action='projectcommission/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取项目提交字段接口(get)
            commService.getProjectFields = function(option, callBack, failCallBack) {
                var action='config/get.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };

            return commService;
        }]);

/**
 * Created by add on 2016/1/25.
 */
distributionmodule.directive('header',function(){
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/header/tpls/index.html',
        replace: true,
        scope: {
            title:'@', //标题
            btnshow:'@',  //是否显示标题 0不显示 1显示
            isshow:'@'
        },
        controller:function($scope){
            var pageFunc=$scope.pageFunc={};
            var pageData=$scope.pageData={
                title:'财富中心',
                showBtn:$scope.btnshow,  //0不显示 1显示
                showBack:false,
                isshow:$scope.isshow
            };
            document.title = pageData.title;
            pageFunc.init=function(){
                //pageData.showBack = document.referrer != '';
                if($scope.title){
                    pageData.title = $scope.title;
                    document.title = pageData.title;
                }
            };
            pageFunc.init();
        }
    }
});

/**
 * Created by add on 2016/1/25.
 */

//(function () {
//    'use strict';
//
//    angular
//        .module('distribution')
//        .controller('applyCtrl', applyCtrl);
//    function applyCtrl() {
//        /* jshint validthis: true */
//        var vm = this;
//        vm.title = '分销系统';
//    }
//});
distributionmodule.controller('applyCtrl', ['$scope','$sessionStorage','$routeParams', 'commService', function ($scope,$sessionStorage,$routeParams, commService) {
    /* jshint validthis: true */
    var vm = $scope.vm = {};
    vm.title = '财富中心';
    vm.showMsg='';  //审核显示信息
    vm.signData = []; //报名字段
    vm.currUseInfo=$sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员
    vm.currUseDetail=[]; //当前用户信息
    vm.recommendId = $routeParams.recommendId;
    //vm.getCurrUserInfo=getCurrUserInfo;  //获取当前用户接口
    vm.getDisActivity = getDisActivity; //获取活动接口
    vm.submitSignup = submitSignup;//提交报名
    vm.getUserDetail=getUserDetail; //获取当前用户详细信息

    init();

    function init() {
        var t = setInterval(function(){
            $('.headerContent').html($('#wrapApplyHeader').html());

        },200);

        if(!$sessionStorage.currUseInfo && currentUserInfo != '$$CURRENTUSERINFO$$')
        {
            $sessionStorage.currUseInfo = currentUserInfo.result;
        }

        //alert(vm.currUseInfo.is_distribution_member);
        if(vm.currUseInfo.is_distribution_member){
            window.location.href = '#/discenter';
        }
        else{
            //alert('getUserDetail');
            getUserDetail();
        }
    }

    function getDisActivity() {
        commService.getDisActivity({}, function (data) {
            if (data.status) {
                console.log('活动报名', data);
                vm.signData = data.result;
                //apply_status  0未申请  1 待审核  2已通过
                if(data.result.apply_status == 1){
                    //vm.showMsg='您的资料正在审核中请耐心等候...';
                }else if (data.result.apply_status == 2) {
                    window.location.href = '#/discenter';
                }else if(data.result.apply_status==4001){
                    vm.showMsg=data.result.remarks;
                }
                if (data.result.signfield) {
                    //console.log('vm.currUseDetail:' + JSON.stringify(vm.currUseDetail));
                    for (var i = 0; i < data.result.signfield.length; i++) {
                        vm.signData.signfield[i].input = '';
                        if (vm.signData.signfield[i].value == 'Name') {
                            vm.signData.signfield[i].input =  vm.currUseDetail.truename ? vm.currUseDetail.truename : '';
                        }
                        if (vm.signData.signfield[i].value == 'Phone') {
                            vm.signData.signfield[i].input = vm.currUseDetail.phone ? vm.currUseDetail.phone : '';
                        }
                        if (vm.signData.signfield[i].value == 'DistributionOffLineRecommendCode') {
                            vm.signData.signfield[i].input = vm.recommendId ? vm.recommendId : '';
                        }
                    }
                }
            } else {
                alert(data.msg);
            }
        }, function (data) {
            alert(data.msg);
        });
    }

    function submitSignup() {
        var reqData = {
            ActivityID: vm.signData.activity_id,
        };
        var signData = vm.signData.signfield;
        for (var i = 0; i < signData.length; i++) {
            if (signData[i].input && signData[i].input != '') {
                reqData[signData[i].value] = $.trim(signData[i].input);
            }
        }

        if(!reqData.DistributionOffLineRecommendCode){
            alert('请输入邀请码');
            return;
        }

        commService.activityRegist(reqData, function (data) {
            if (data.Status == 0) {
                vm.signData.is_enroll = true; //true为已经报名 false为没有报名
                vm.signData.apply_status = 1;
                vm.signData.apply_status=1;
                //vm.showMsg='您的资料正在审核中请耐心等候...';
                //getDisActivity();
            } else {
                alert(data.Msg);
            }
        }, function (data) {
            alert(data.Msg);
        });
    }
    //
    //function getCurrUserInfo(){
    //    commService.getCurrUserInfo({
    //    },function(data){
    //        if(data.status){
    //            //vm.currUseInfo=data.result;
    //            if(data.result.is_distribution_member){
    //                window.location.href = '#/discenter';
    //            }
    //            else{
    //                getUserDetail();
    //            }
    //            console.log('用户信息',vm.currUseInfo);
    //        }
    //        else{
    //            alert(data.msg);
    //        }
    //    },function(data){
    //        alert(data.msg);
    //    });
    //}

    function getUserDetail(){
        commService.getUserDetail(function(data){
            if(data){
                vm.currUseDetail=data;
                getDisActivity();
            }else{
                alert(data.errmsg);
            }
        },function(data){
            alert(data.errmsg);
        });
    }

}]);


/**
 * Created by add on 2016/1/25.
 */
distributionmodule.controller('disCenterCtrl',['$sessionStorage','commService',function($sessionStorage,commService){
    /* jshint validthis: true */
    var vm=this;
    vm.title='财富中心';
    vm.currUseInfo=$sessionStorage.currUseInfo;
    vm.levels=$sessionStorage.currWebsiteConfig;//分销级数
    vm.loadDone = false;
    //vm.userLevel=[];  //用户等级
    //vm.getCurrUserInfo=getCurrUserInfo; //获取当前用户信息
    vm.getDistributeConfig=getDistributeConfig;//获取后台配置分销级数

    init();
    function init() {
        if (vm.currUseInfo.is_distribution_member) {
            setInterval(function(){
                getDistributeConfig();
            },10000);
        }
        else {
            window.location.href = '#/apply';
        }
    }
    function getDistributeConfig(){
        commService.getDistributeConfig({
            isnoloading:true
        },function(data){
            vm.loadDone = true;
            if(data.status){
                vm.levels=data.result;
                console.log('分销级数',vm.levels);
                if($sessionStorage.commName==undefined){
                    $sessionStorage.commName=[];
                }
                $sessionStorage.commName=vm.levels.commission_show_name;
            }
            else{
                alert(data.msg);
            }
        },function(data){
            vm.loadDone = true;
            alert(data.msg);
        });
    }
}]);
/**
 * Created by add on 2016/1/25.
 */
distributionmodule.controller('disCountCtrl',['$sessionStorage','commService',function($sessionStorage,commService){
    /* jshint validthis: true */
    var vm=this;
    vm.title='我的分销统计';
    vm.currUseInfo=$sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员

    vm.pageIndex=1;
    vm.pageSize=5;
    vm.totalCount=0;
    vm.dataList=[];

    vm.currPageIndex=0;
    vm.levels=[]; //分销级数
    vm.classLevel=$sessionStorage.currUseInfo;
    vm.commission='';
    vm.commissions=[{
        id:'0',
        name:'贡献佣金最多'
    },{
        id:'1',
        name:'贡献佣金最少'
    },{
        id:'2',
        name:'合计贡献最多'
    },{
        id:'3',
        name:'合计贡献最少'
    }];
    vm.commission=vm.commissions[0];
    vm.getUserNextLevel=getUserNextLevel; //获取用户下级分销
    vm.getDistributeConfig=getDistributeConfig;//获取后台配置分销级数

    console.log('用户信息',vm.classLevel);

    init();
    function init(){
        if(vm.currUseInfo.is_distribution_member){
            getUserNextLevel(true);
            getDistributeConfig();

            $(function () {
                $(window).scroll(function () {
                    //当内容滚动到底部时加载新的内容
                    if ($(this).scrollTop() + $(window).height() >= $(document).height()) {
                        //判断当没有数据的时候不加载
                        if (vm.totalCount > vm.dataList.length) {
                            getUserNextLevel(false);
                        }
                    }
                });
            });

        }
        else{
            window.location.href = '#/apply';
        }
    }
    function getUserNextLevel(isNew){
        if (isNew) {
            vm.dataList = [];
            vm.pageIndex = 1;
        }
        else {
            vm.pageIndex++;
        }
        commService.getUserNextLevel({
            pageindex:vm.pageIndex,
            pagesize:vm.pageSize
        },function(data){
            if(data.status){
                vm.totalCount = data.result.totalcount;
                vm.dataList = vm.dataList.concat(data.result.list);
            }else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }


    //function getUserNextLevel(isNew){
    //    if (isNew) {
    //        vm.dataList = [];
    //        vm.pageIndex = 1;
    //    }
    //    else {
    //        vm.pageIndex++;
    //    }
    //
    //    commService.getUserNextLevel({
    //        pageindex:vm.pageIndex,
    //        pagesize:vm.pageSize
    //    },function(data){
    //        if(data.status){
    //            vm.totalCount=data.result.totalcount;
    //            //vm.dataList=data.result.list;
    //            //console.log('用户下级分销',vm.dataList);
    //
    //            for (var i = 0; i < data.result.list.length; i++) {
    //                vm.dataList.push(data.result.list[i]);
    //            }
    //
    //        }else{
    //            alert(data.msg);
    //        }
    //    },function(data){
    //        alert(data.msg);
    //    });
    //}

    function getDistributeConfig(){
        commService.getDistributeConfig({
        },function(data){
            if(data.status){
                vm.levels=data.result;
                console.log('分销级数',vm.levels);
            }
            else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }

}]);
/**
 * Created by add on 2016/1/25.
 */
distributionmodule.controller('myRecommendCtrl',['$sessionStorage','commService',function($sessionStorage,commService){
    /* jshint validthis: true */
    var vm=this;
    vm.title='我的推荐';
    vm.currUseInfo='';
    vm.code_url=''; //分销申请链接
    vm.erweima='';//二维码链接
    vm.currUseInfo=$sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员
    //vm.getCurrUserInfo=getCurrUserInfo; //获取当前用户信息
    vm.getQrCode=getQrCode; //生成二维码
    init();
    function init(){
        if(vm.currUseInfo.is_distribution_member){
            vm.code_url= 'http://' + window.location.host+'/App/Distribution/m/index.aspx?ngroute=/apply/'+vm.currUseInfo.recommend_id+'#/apply/'+vm.currUseInfo.recommend_id;
            getQrCode(vm.code_url);
            //wx.ready(function () {
            //    wxapi.wxshare({
            //        title: share_title,
            //        desc: share_desc,
            //        imgUrl:share_img_url,
            //        link: vm.code_url
            //    }, '')
            //});
        }
        else{
            window.location.href = '#/apply';
        }
    }
    function getQrCode(url){
        commService.getQrCode({code:url},function(data){
            if(data.status){
                vm.erweima=data.result.qrcode_url;
                console.log('二维码',data);
            }else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
}]);
/**
 * Created by add on 2016/2/1.
 */
distributionmodule.controller('scoreCtrl',['$sessionStorage','$localStorage','commService','$routeParams',function($sessionStorage,$localStorage,commService,$routeParams){
    /* jshint validthis : true*/
    var vm=this;
    vm.title='积分历史';
    vm.projectId=$routeParams.projectId;

    vm.pageIndex=1;
    vm.pageSize=5;
    vm.totalCount=0;
    vm.scoreList=[]; //日志列表

    vm.minHeight=0;  //最低高度，解决一半灰一半白

    vm.currUseInfo=$sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员
    vm.commName=$sessionStorage.commName; //显示是什么历史（积分/金豆/佣金...）

    vm.getScore=getScore;//获取日志列表

    init();

    function init(){
        getScore(true);

        vm.minHeight=$(window).height();
        $(function () {
            $(window).scroll(function () {
                //当内容滚动到底部时加载新的内容
                if ($(this).scrollTop() + $(window).height() >= $(document).height()) {
                    //判断当没有数据的时候不加载
                    if (vm.totalCount > vm.scoreList.length) {
                        getScore(false);
                    }
                }
            });
        });
    }
    function getScore(isNew){
        if (isNew) {
            vm.scoreList = [];
            vm.pageIndex = 1;
        }
        else {
            vm.pageIndex++;
        }
        commService.getScore({
            project_id:vm.projectId, //是	项目id
            pageindex:vm.pageIndex,
            pagesize:vm.pageSize,
            isnoloading:true  //不加载数据
        },function(data){
            if(data.status){
                vm.totalCount = data.result.totalcount;
                vm.scoreList = vm.scoreList.concat(data.result.list);
                console.log('积分历史',vm.scoreList);
            }
           else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
}]);
/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('addCardCtrl',['$routeParams','commService',function($routeParams,commService){
    /* jshint validthis: true */
    var vm=this;
    vm.title='添加银行卡';
    vm.bankCardId=$routeParams.bankCardId;
    vm.cardData=[];  //银行卡详情信息
    vm.dataInfo={
        account_name:'', //是	开户名姓名
        bank_account:'', //是	银行账号
        bank_name:''  //是	开户银行名称
    };
    vm.init=init;//初始化
    vm.addCard=addCard;  //新增银行卡
    vm.updateBankCard=updateBankCard; //修改银行卡
    vm.getBankCardDetail=getBankCardDetail;  //获取接口详情
    vm.checkIsNull=checkIsNull; //检查必填元素

    init();
    function checkIsNull(){
        if(vm.dataInfo.account_name==''){
            alert('请填写开户名');
            return false;
        }
        if(vm.dataInfo.bank_account==''){
            alert('请填写银行账号');
            return false;
        }
        if(vm.dataInfo.bank_name==''){
            alert('请填写开户银行名称');
            return false;
        }
        return true;
    }
    function addCard(){
        if(checkIsNull()){
            commService.addBankCard({
                data:angular.toJson(vm.dataInfo)
            },function(data){
                if(data.status){
                    alert('添加银行卡成功');
                }
                else{
                    alert(data.msg);
                }
            },function(data){
                alert(data.msg);
            });
        }
    }
    function updateBankCard(){
        if(checkIsNull()){
            commService.updateBankCard({
                data:angular.toJson(vm.dataInfo)
            },function(data){
                if(data.status){
                    alert('修改银行卡成功');
                }
                else{
                    alert(data.msg);
                }
            },function(data){
                alert(data.msg);
            });
        }
    }
    function getBankCardDetail(){
        commService.getBankCardDetail({
            id:vm.bankCardId
        },function(data){
            if(data.status){
                vm.dataInfo=data.result;
            }else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
    function init(){
        if(vm.bankCardId!='add'){
            getBankCardDetail();
        }
    }

}]);
/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('cardListCtrl',['$sessionStorage','commService',function($sessionStorage,commService){
    /* jshint validthis: true */
    var vm=this;
    vm.title='银行卡列表';
    vm.pageindex=1;  //否	页码
    vm.pagesize=10;  //否	页数
    vm.keyword='';   //否	关键字查询  开户人、银行账号、银行名称
    vm.totalCount=0;
    vm.dataList=[];//存放获取的值
    vm.getBankCardList=getBankCardList; //获取银行卡列表
    vm.goToDetail=goToDetail;  //进入银行卡详情
    vm.deleteBankCard=deleteBankCard;  //删除银行卡
    vm.backToUseCard=backToUseCard;//返回使用卡的地方

    getBankCardList();
    function getBankCardList(){
        commService.getBankCardList({
            pageindex:vm.pageindex,
            pagesize:vm.pagesize,
            keyword:vm.keyword
        },function(data){
            if(data.status){
                vm.totalCount=data.result.totalcount;
                vm.dataList=data.result.list;
                console.log('银行卡列表',data.result.list);
            }
            else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
    function goToDetail(item){
        if(item=='add'){
            window.location.href='#/addcard/'+item;
        }
        else{
            window.location.href='#/addcard/'+item.id;
        }
    }
    function deleteBankCard(item){
        layer.open({
            content:'确定要删除这张银行卡？',
            btn:['确定','取消'],
            shadeClose: false,
            yes:function(){
                commService.deleteBankCard({
                    id:item.id
                },function(data){
                    if(data.status){
                        getBankCardList();
                    }
                    else{
                        alert(data.msg);
                    }
                },function(data){
                    alert(data.msg);
                });
            },
            no:function(){
                layer.closeAll();
            }
        });
    }
    function backToUseCard(item){
        if($sessionStorage.cardInfo==undefined){
            $sessionStorage.cardInfo=[];
        }
        $sessionStorage.cardInfo=item;
        window.location.href='#/cashrecord';
    }
}]);
/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('cashRecordCtrl',['$sessionStorage','$filter','commService',function($sessionStorage,$filter,commService){
    /* jshint validthis: true */
    var vm=this;
    vm.title='提现记录';
    //提现参数
    vm.amount=0;  //是	提现金额
    vm.bank_card_id='';  //是	银行卡id
    vm.bankInfo='';
    //提现记录参数
    vm.dataList=[];
    vm.currUseInfo=$sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员
    //vm.timeObj=[];

    vm.applyForCash=applyForCash; //申请提现接口
    vm.applyForCashRecord=applyForCashRecord; //申请提现记录
    //vm.getCurrUserInfo=getCurrUserInfo; //获取当前用户信息
    //vm.getDaySort=getDaySort;//获取按月排序

    init();
    function init(){
        if(vm.currUseInfo.is_distribution_member){
            if($sessionStorage.cardInfo==undefined){
                $sessionStorage.cardInfo=[];
            }
            else{
                vm.bank_card_id=$sessionStorage.cardInfo.id;
                vm.bankInfo=$sessionStorage.cardInfo;
            }
            console.log('银行卡',$sessionStorage.cardInfo);
            applyForCashRecord();
        }
        else{
            window.location.href = '#/apply';
        }
        //getCurrUserInfo();
    }
    function applyForCash(){
        if(vm.amount==0){
            alert('请输入提现金额');
            return;
        }
        if(vm.amount<50){
            alert('提现金额要大于50');
            return;
        }
        if(vm.bank_card_id==''){
            alert('请选择银行卡');
            return;
        }
        commService.applyForCash({
            amount:vm.amount,
            bank_card_id:vm.bank_card_id
        },function(data){
            if(data.status){
                vm.amount=0;
                vm.bank_card_id='';
                $sessionStorage.cardInfo=[];
                vm.bankInfo='';
                alert('提现成功');
            }
            else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
    function applyForCashRecord(){
        commService.applyForCashRecord({
            pageindex:1,
            pagesize:10
        },function(data){
            if(data.status){
                vm.totalCount=data.result.totalcount;
                vm.dataList=data.result.list;
                //debugger;
                //getDaySort(vm.dataList);
                console.log('提现记录',vm.dataList);
            }
            else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
    //function getCurrUserInfo(){
    //    commService.getCurrUserInfo({
    //    },function(data){
    //        if(data.status){
    //            vm.currUseInfo=data.result;
    //            applyForCashRecord();
    //            console.log('用户信息',vm.currUseInfo);
    //        }
    //        else{
    //            alert(data.msg);
    //        }
    //    },function(data){
    //        alert(data.msg);
    //    });
    //}

}]);
/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('myStaticCtrl',['$scope','$sessionStorage','$timeout','commService',function($scope,$sessionStorage,$timeout,commService){
    /* jshint validthis : true*/
    var vm=this;
    vm.title='我的直销数据';
    vm.currUseInfo=$sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员

    vm.pageIndex=1;
    vm.pageSize=4;
    vm.totalCount=0;
    vm.dataList=[];//存放获取数据

    vm.latestDay='';
    vm.latestDays=[{
        id:'',
        from:'',
        to:'',
        name:'全部'
    },{
        id:1,
        from:'',
        to:'',
        name:'昨天'
    },{
        id:7,
        from:'',
        to:'',
        name:'最近7天'
    },{
        id:30,
        from:'',
        to:'',
        name:'最近30天'
    }];
    vm.from='';//开始时间
    vm.to='';//结束时间

    vm.getProjectList=getProjectList;//获取项目列表
    vm.goToDetail=goToDetail; //进入项目详情
    vm.changeDate=changeDate; //获取最近时间

    init();//初始化
    function init(){
        vm.latestDay= vm.latestDays[0];
        if(vm.currUseInfo.is_distribution_member){
            getProjectList(true);

            $(function () {
                $(window).scroll(function () {
                    //当内容滚动到底部时加载新的内容
                    if ($(this).scrollTop() + $(window).height() >= $(document).height()) {
                        //判断当没有数据的时候不加载
                        if (vm.totalCount > vm.dataList.length) {
                            getProjectList(false);
                        }
                    }
                });
            });

        }
        else{
            window.location.href = '#/apply';
        }
    }
    function getProjectList(isNew){
        if (isNew) {
            vm.dataList = [];
            vm.pageIndex = 1;
        }
        else {
            vm.pageIndex++;
        }
        commService.getProjectList({
            pageindex:vm.pageIndex,
            pagesize:vm.pageSize,
            keyword:'',
            status:'',
            from:vm.latestDay.from,
            to:vm.latestDay.to
        },function(data){
            if(data.status){
                vm.totalCount = data.result.totalcount;
                vm.dataList = vm.dataList.concat(data.result.list);
                console.log('项目列表',vm.dataList);
            }else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
    function goToDetail(item){
        window.location.href='#/staticdetail/'+item.project_id;
    }

    function changeDate(days){
        var today=new Date(); // 获取今天时间
        if(days == 1){
            vm.latestDay.from=today.setTime(today.getTime()-1*24*3600*1000);
            vm.latestDay.to=new Date().getTime();
        }else if(days == 7){
            vm.latestDay.from=today.setTime(today.getTime()-7*24*3600*1000);
            vm.latestDay.to=new Date().getTime();
        }else if(days == 30){
            vm.latestDay.from=today.setTime(today.getTime()-30*24*3600*1000);
            vm.latestDay.to=new Date().getTime();
        }
        getProjectList(true);
    }
}]);
/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('proStatusCtrl',['$sessionStorage','$localStorage','commService','$routeParams',function($sessionStorage,$localStorage,commService,$routeParams){
    /* jshint validthis : true*/
    var vm=this;
    vm.title='项目状态日志';
    vm.projectId=$routeParams.projectId;
    vm.logList=[]; //日志列表
    vm.currUseInfo=$sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员

    vm.getProjectLog=getProjectLog;//获取日志列表

    init();

    function init(){
        getProjectLog();
    }
    function getProjectLog(){
        commService.getProjectLog({
            project_id:vm.projectId, //是	项目id
            pageindex:1,
            pagesize:10000
        },function(data){
            vm.logList = data.result.list;
        },function(data){
            alert(data.msg);
        });
    }
}]);
/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('staticDetailCtrl',['$sessionStorage','$routeParams','$localStorage','commService',function($sessionStorage,$routeParams,$localStorage,commService){
    /* jshint validthis : true*/
    var vm=this;
    vm.title='我的直销数据';
    vm.currUseInfo=$sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员
    vm.projectId=$routeParams.projectId; //项目id
    vm.project={};
    vm.logList=[]; //日志列表
    vm.fields=[];// 项目字段

    vm.fn = {
        getProjectDetail:getProjectDetail,//获取项目详情
        getProjectStatusLog:getProjectStatusLog,//获取项目状态列表
        getProjectLog:getProjectLog,//获取日志列表
        getProjectFields:getProjectFields  //获取项目字段
    };

    init();

    function init(){
        getProjectDetail();
        getProjectLog();
    };

    function getProjectDetail(){
        commService.get('project/get.ashx',{
            project_id:vm.projectId
        },function(data){
            vm.project = data.result;
            if(data.result.prop_list){
                vm.fields=data.result.prop_list;
            }
            console.log('项目详情',vm.project);
        },function(data){
        });
    }

    function getProjectStatusLog(){
        if(!vm.project.statusLogloadDone) {
            commService.get('projectlog/list.ashx', {
                project_id: vm.projectId,
                pageindex: 1,
                pagesize: 10000
            }, function (data) {
                console.log('log:',data);
                vm.project.statusLog = data.result.list;
                vm.project.statusLogloadDone = true;
            }, function (data) {
            });
        }
    };

    function getProjectLog(){
        commService.getProjectLog({
            project_id:vm.projectId, //是	项目id
            pageindex:1,
            pagesize:10000
        },function(data){
            vm.logList = data.result.list;
        },function(data){
            alert(data.msg);
        });
    }

    function getProjectFields() {
        commService.getProjectFields({}, function (data) {
            if (data.status) {
                vm.fields = data.result.project_field_list;
                if (data.result.project_field_list) {
                    for (var i = 0; i < data.result.project_field_list.length; i++) {
                        vm.fields[i].input = '';
                    }
                }
                console.log('项目提交信息', vm.fields);
            } else {
                alert(data.msg);
            }
        }, function (data) {
            alert(data.msg);
        });
    }

}]);
/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('submitInfoCtrl', ['$sessionStorage','$timeout', 'commService', function ($sessionStorage,$timeout, commService) {
    /* jshint validthis : true*/
    var vm = this;
    vm.title = '财富商机';
    vm.currUseInfo = $sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员
    vm.fields = []; //后台传回来的字段
    vm.dataInfo = {
        contact: '', //是	联系人
        phone: '',   //是	联系方式
        weixin: '',  //否	微信
        company: '',  //否	单位
        project_intro: '备注'  //否	项目推荐  推荐理由
    };//提交信息
    vm.submitInfo = submitInfo;
    //vm.checkIsNull = checkIsNull;
    //vm.clearAll = clearAll;
    vm.backToList = backToList;
    vm.getProjectFields = getProjectFields; //获取项目提交字段

    init();
    function init() {
        getProjectFields();
    }

    //function checkIsNull() {
    //    if (vm.dataInfo.porject_name == '') {
    //        alert('项目名称不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.contact == '') {
    //        alert('姓名不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.phone == '') {
    //        alert('手机号码不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.weixin == '') {
    //        alert('微信不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.company == '') {
    //        alert('单位不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.project_intro == '') {
    //        alert('推荐理由不能为空');
    //        return false;
    //    }
    //    return true;
    //}
    //
    //function clearAll() {
    //    vm.dataInfo.porject_name = '';
    //    vm.dataInfo.contact = '';
    //    vm.dataInfo.phone = '';
    //    vm.dataInfo.weixin = '';
    //    vm.dataInfo.company = '';
    //    vm.dataInfo.project_intro = '推荐理由';
    //}

    function submitInfo() {
        var reqData = {
            //ActivityID: vm.signData.activity_id,
        };
        var signData = vm.fields;
        for (var i = 0; i < signData.length; i++) {
            if (signData[i].input && signData[i].input != '') {
                reqData[signData[i].field] = $.trim(signData[i].input);
            }
        }
        commService.addProject(reqData, function (data) {
            if (data.status) {
                alert('添加成功');
                $timeout(function () {
                    getProjectFields();
                }, 2000);
            } else {
                alert(data.msg);
            }
        }, function (data) {
            alert(data.msg);
        });
    }

    function backToList() {
        window.location.href = '#/mystatic';
    }

    function getProjectFields() {
        commService.getProjectFields({}, function (data) {
            if (data.status) {
                vm.fields = data.result.project_field_list;
                if (data.result.project_field_list) {
                    for (var i = 0; i < data.result.project_field_list.length; i++) {
                        vm.fields[i].input = '';
                    }
                }
                console.log('项目提交信息', vm.fields);
            } else {
                alert(data.msg);
            }
        }, function (data) {
            alert(data.msg);
        });
    }
}]);