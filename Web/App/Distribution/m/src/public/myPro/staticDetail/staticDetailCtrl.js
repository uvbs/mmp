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