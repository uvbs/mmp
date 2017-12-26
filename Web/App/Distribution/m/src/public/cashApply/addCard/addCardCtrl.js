/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('addCardCtrl',['$routeParams','commService',function($routeParams,commService){
    /* jshint validthis: true */
    var vm=this;
    vm.title='添加银行卡';
    vm.bankCardId=$routeParams.bankCardId;
    vm.cardData=[];  //银行卡详情信息
    vm.dataInfo={
        account_name:'', //是	开户名姓名
        bank_account:'', //是	银行账号
        bank_name:''  //是	开户银行名称
    };
    vm.init=init;//初始化
    vm.addCard=addCard;  //新增银行卡
    vm.updateBankCard=updateBankCard; //修改银行卡
    vm.getBankCardDetail=getBankCardDetail;  //获取接口详情
    vm.checkIsNull=checkIsNull; //检查必填元素

    init();
    function checkIsNull(){
        if(vm.dataInfo.account_name==''){
            alert('请填写开户名');
            return false;
        }
        if(vm.dataInfo.bank_account==''){
            alert('请填写银行账号');
            return false;
        }
        if(vm.dataInfo.bank_name==''){
            alert('请填写开户银行名称');
            return false;
        }
        return true;
    }
    function addCard(){
        if(checkIsNull()){
            commService.addBankCard({
                data:angular.toJson(vm.dataInfo)
            },function(data){
                if(data.status){
                    alert('添加银行卡成功');
                }
                else{
                    alert(data.msg);
                }
            },function(data){
                alert(data.msg);
            });
        }
    }
    function updateBankCard(){
        if(checkIsNull()){
            commService.updateBankCard({
                data:angular.toJson(vm.dataInfo)
            },function(data){
                if(data.status){
                    alert('修改银行卡成功');
                }
                else{
                    alert(data.msg);
                }
            },function(data){
                alert(data.msg);
            });
        }
    }
    function getBankCardDetail(){
        commService.getBankCardDetail({
            id:vm.bankCardId
        },function(data){
            if(data.status){
                vm.dataInfo=data.result;
            }else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
    function init(){
        if(vm.bankCardId!='add'){
            getBankCardDetail();
        }
    }

}]);