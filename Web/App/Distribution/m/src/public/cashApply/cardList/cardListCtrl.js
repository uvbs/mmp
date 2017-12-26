/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('cardListCtrl',['$sessionStorage','commService',function($sessionStorage,commService){
    /* jshint validthis: true */
    var vm=this;
    vm.title='银行卡列表';
    vm.pageindex=1;  //否	页码
    vm.pagesize=10;  //否	页数
    vm.keyword='';   //否	关键字查询  开户人、银行账号、银行名称
    vm.totalCount=0;
    vm.dataList=[];//存放获取的值
    vm.getBankCardList=getBankCardList; //获取银行卡列表
    vm.goToDetail=goToDetail;  //进入银行卡详情
    vm.deleteBankCard=deleteBankCard;  //删除银行卡
    vm.backToUseCard=backToUseCard;//返回使用卡的地方

    getBankCardList();
    function getBankCardList(){
        commService.getBankCardList({
            pageindex:vm.pageindex,
            pagesize:vm.pagesize,
            keyword:vm.keyword
        },function(data){
            if(data.status){
                vm.totalCount=data.result.totalcount;
                vm.dataList=data.result.list;
                console.log('银行卡列表',data.result.list);
            }
            else{
                alert(data.msg);
            }
        },function(data){
            alert(data.msg);
        });
    }
    function goToDetail(item){
        if(item=='add'){
            window.location.href='#/addcard/'+item;
        }
        else{
            window.location.href='#/addcard/'+item.id;
        }
    }
    function deleteBankCard(item){
        layer.open({
            content:'确定要删除这张银行卡？',
            btn:['确定','取消'],
            shadeClose: false,
            yes:function(){
                commService.deleteBankCard({
                    id:item.id
                },function(data){
                    if(data.status){
                        getBankCardList();
                    }
                    else{
                        alert(data.msg);
                    }
                },function(data){
                    alert(data.msg);
                });
            },
            no:function(){
                layer.closeAll();
            }
        });
    }
    function backToUseCard(item){
        if($sessionStorage.cardInfo==undefined){
            $sessionStorage.cardInfo=[];
        }
        $sessionStorage.cardInfo=item;
        window.location.href='#/cashrecord';
    }
}]);