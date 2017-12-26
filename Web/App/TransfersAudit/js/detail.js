Vue.filter('typeFormart', function (value) {

    if (value == "MallRefund") return '商城退款';
    if (value == "DistributionWithdraw") return '分销提现';

    return '';
});

var listVm = new Vue({
    el: '.vue-el',
    data: {
        handler: '/Serv/API/Admin/TransfersAudit/',
        data: {}

    },
    methods: {
        init: function () {          
            this.loadData();
        },
        loadData: function () {
            var _this = this;
         
            $.ajax({
                type: 'post',
                url: _this.handler + "Get.ashx",
                data: {

                    id: GetParm('tranid')

                },
                dataType: 'json',
                success: function (resp) {

                    _this.data = resp.result;
                  

                }
            });
        },
        pass: function (item) {
            var _this = this;
            layer.open({
                content: '确定打款？'
                , btn: ['打款', '取消']
                , yes: function (index) {
                    layer.close(index);
                    $('#loadingToast').show();
                    $.ajax({
                        type: 'post',
                        url: _this.handler+"Update.ashx",
                        data: {

                            id:item.id

                        },
                        dataType: 'json',
                        success: function (resp) {
                            $('#loadingToast').hide();
                            if (resp.status) {
                                alert("打款成功");
                                _this.loadData();
                              

                            }
                            else {
                                alert(resp.msg);
                            }

                        }
                    });




                }
            });

        }







    }
});

listVm.init();

