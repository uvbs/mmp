Vue.filter('typeFormart', function (value) {

    if (value == "MallRefund") return '商城退款';
    if (value == "DistributionWithdraw") return '分销提现';

    return '';
});

var listVm = new Vue({
    el: '.vue-el',
    data: {
        handler: '/Serv/API/Admin/TransfersAudit/',
        rows: 15,
        listData: { page: 1, total: 0, list: [] },
        isLoading: true,
        isLoadAll: false,
        statusList: [{ text: '待审核', value: "0" }, { text: '已打款', value: "1" }],
        status: "0"
    },
    methods: {
        init: function () {

            this.bindScroll();
            this.loadData();
        },
        loadData: function () {
            var _this = this;
            _this.isLoading = true;
            $.ajax({
                type: 'post',
                url: _this.handler + "List.ashx",
                data: {

                    status: _this.status,
                    page: this.listData.page,
                    rows: this.rows

                },
                dataType: 'json',
                success: function (resp) {
                    _this.isLoading = false;
                    if (resp.rows.length == 0) {
                        _this.isLoadAll = true;

                    }
                    _this.listData.total = resp.total;
                    _this.listData.list = _this.listData.list.concat(resp.rows);

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
                                _this.listData = { page: 1, total: 0, list: [] };
                                _this.loadData();

                            }
                            else {
                                alert(resp.msg);
                            }

                        }
                    });




                }
            });

        },
        selectTab: function (value) {

            if (value === this.status) return;
            this.status = value;
            this.listData = { page: 1, total: 0, list: [] };
            this.loadData();
        },
        bindScroll: function () {
            //console.log($(window).scrollTop());
            var _this = this;
            $(window).bind('scroll', function (e) {
                var _wh = $(window).height();
                var _st = $('body').get(0).scrollTop;
                var _sh = $('body').get(0).scrollHeight;
                if ((_sh - _st - _wh < 10) && (!_this.isLoadAll)) {
                    _this.loadMore();
                } else {



                }
            });
        },
        loadMore: function () {
            this.listData.page++;
            this.loadData();
        }




    }
});

listVm.init();

