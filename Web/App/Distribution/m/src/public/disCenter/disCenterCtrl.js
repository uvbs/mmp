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