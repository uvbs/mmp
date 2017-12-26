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