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
    return '';
    if (!value) return '';
    if (value == 1) return '已使用';
    if (value == 2) return '已过期';
    if (value == 11 || value == 12) return '已转赠';
    if (value == 9 || value == 99) return '已作废';
    return '';
});

var listVm = new Vue({
    el: '.wrapList',
    data: {
        handler: '/Serv/API/SVCard/List.ashx',
        status: GetParm('status'),
        rows: 5,
        statusList: [{ text: '未使用', value: 0 }, { text: '已使用', value: 1 }, { text: '已过期', value: 3 }, { text: '已转赠', value: 2 }],
        allData: {},
        curCards: {},
        share: {
            title: '我的储值卡',
            desc: '我的储值卡',
            imgUrl: 'http://static-files.socialcrmyun.com/img/SVCard/ico.png'
        }
    },
    methods: {
        init: function () {
            this.bindScroll();
            this.initAllData();
        },
        initAllData: function () {
            this.allData.s_0 = { status: 0, total: 1, page: 1, list: [] };
            this.allData.s_1 = { status: 1, total: 1, page: 1, list: [] };
            this.allData.s_2 = { status: 2, total: 1, page: 1, list: [] };
            this.allData.s_3 = { status: 3, total: 1, page: 1, list: [] };
            if (!this.status) {
                this.selectStatus(0);
            } else {
                this.curCards = this.allData['s_' + this.status];
                this.loadData();
            }
        },
        loadData: function () {
            var _this = this;

            $.ajax({
                type: 'post',
                url: _this.handler,
                data: {
                    page: this.curCards.page,
                    rows: this.rows,
                    status: this.curCards.status
                },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.curCards.total = resp.result.totalcount;
                        _this.curCards.list = _this.curCards.list.concat(resp.result.list);
                    }
                    else {
                        alert(resp.msg);
                    }
                }
            });
        },
        bindScroll: function () {
            //console.log($(window).scrollTop());
            var _this = this;
            $(window).bind('scroll', function (e) {
                var _wh = $(window).height();
                var _st = $('body').get(0).scrollTop;
                var _sh = $('body').get(0).scrollHeight;
                if ((_sh - _st - _wh < 10) && (_this.curCards.list.length < _this.curCards.total)) {
                    _this.loadMore();
                }
            });
        },
        loadMore: function () {
            this.curCards.page++;
            this.loadData();
        },
        selectStatus: function (status) {
            if (typeof (status) == 'string') status = Number(status);
            if (status === this.status) return;
            this.status = status;
            this.curCards = this.allData['s_' + this.status];
            if (this.curCards.list.length < this.curCards.total) this.loadData();
        },
        toDetail: function (card) {
            window.location.href = '/App/SVCard/Wap/Detail.aspx?id=' + card.id;
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
        goBack: function () {
            window.history.back();
        }
    }
});

listVm.init();

