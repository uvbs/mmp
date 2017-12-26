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