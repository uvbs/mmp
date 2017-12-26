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