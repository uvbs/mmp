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