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
        setTimeout(function(){
            $('.headerContent').html($('#wrapApplyHeader').html());
        },100);

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

