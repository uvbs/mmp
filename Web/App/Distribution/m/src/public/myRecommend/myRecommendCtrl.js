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