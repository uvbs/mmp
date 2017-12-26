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