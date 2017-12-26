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