//过滤器
Vue.filter('amountFommat', function (value) {
    if (!value) return '';
    return '¥' + value;
});
Vue.filter('validFommat', function (value) {
    if (!value) return '';
    return '有效期：' + new Date(value).format('yyyy-MM-dd hh:mm');
});
Vue.filter('statusFommat', function (value) {
    if (!value) return '';
    if (value == 1) return '已使用';
    if (value == 2) return '已过期';
    if (value == 11 || value == 12) return '已转赠';
    if (value == 9 || value == 99) return '已作废';
    return '';
});

var detailVm = new Vue({
    el: '.wrapDetail',
    data: {
        handler: '/Serv/API/SVCard/',
        id: GetParm('id'),
        give: GetParm('give'),
        card: {},
        share: {
            title: '储值卡',
            desc: '好友分享给你一张储值卡',
            imgUrl: 'http://static-files.socialcrmyun.com/img/SVCard/ico.png'
        },
        isShowQrCode:false//是否显示二维码弹框
    },
    methods: {
        init: function () {
            this.initShare();//初始加载
            this.loadData();
        },
        loadData: function () {
            var _this = this;
            _this.loading();
            $.ajax({
                type: 'post',
                url: _this.handler + 'Get.ashx',
                data: {
                    id: this.id,
                    give: this.give
                },
                dataType: 'json',
                success: function (resp) {
                    _this.closeLoading();
                    if (resp.status) {
                        _this.card = resp.result;
                        _this.share.title = _this.card.name;
                        var link = this.location.origin + '/App/SVCard/Wap/Detail.aspx?id=' + _this.card.id;
                        if (_this.card.use_status == 0 || _this.card.use_status == 10) {
                            link = link + '&give=1';
                        }
                        _this.share.link = link;
                        _this.initShare();//读取到数据后修改分享
                        _this.setPageTitle();
                    }
                    else {
                        alert(resp.msg);
                    }
                }
            });
        },
        loading: function () {
            $('#loadingToast').show();
        },
        closeLoading: function () {
            $('#loadingToast').hide();
        },
        giveCard: function () {
            var _this = this;
            layer.open({
                content: '确认领取好友的储值卡？',
                btn: ['确认', '取消'],
                yes: function (index) {
                    _this.loading();
                    layer.close(index);
                    $.ajax({
                        type: 'post',
                        url: _this.handler + 'Give.ashx',
                        data: { id: _this.card.id },
                        dataType: 'json',
                        success: function (resp) {
                            _this.closeLoading();
                            if (resp.status) {
                                _this.card.status = 1;
                                _this.card.use_status = 0;
                            }
                            alert(resp.msg);
                        }
                    });
                }
            });
        },//赠送
        useCard: function () {
            var _this = this;
            layer.open({
                content: '确认充值到余额？',
                btn: ['确认', '取消'],
                yes: function (index) {
                    _this.loading();
                    layer.close(index);

                    $.ajax({
                        type: 'post',
                        url: _this.handler + 'Use.ashx',
                        data: { id: _this.card.id },
                        dataType: 'json',
                        success: function (resp) {
                            _this.closeLoading();

                            if (resp.status) {
                                _this.card.use_status = 1;
                                _this.share.link = window.location.origin + '/App/SVCard/Wap/Detail.aspx?id=' + _this.card.id;
                                _this.initShare();//使用后修改分享
                            }
                            alert(resp.msg);
                        }
                    });
                }
            });
        },
        showShareTip: function () {
            if (!$('.shareTip').hasClass('shareTipHide')) {
                $('.shareTip').addClass('shareTipHide');
            } else {
                $('.shareTip').removeClass('shareTipHide');
            }
        },
        setPageTitle: function () {
            SetPageTitle(this.card.name);
        },
        initShare: function () {
            var share = JSON.parse(JSON.stringify(this.share));
            wx.ready(function () {
                wxapi.wxshare(share, {
                    timeline_s: function () {
                    },
                    timeline_c: function () {
                    },
                    message_s: function () {
                    },
                    message_c: function () {
                    }
                });
            });
        },
        goCenter: function () {
            window.location.href = '/customize/comeoncloud/Index.aspx?key=PersonalCenter';
        }
    }
});


 detailVm.init();

