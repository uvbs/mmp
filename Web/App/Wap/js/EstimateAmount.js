var commVm = new Vue({
    el: '.wrapComm',
    data: {
        login_user: login_user,
        log: {
            page: 1,
            rows: 15,
            list: [],
            total: 1,
            sum:0,
            loading: false
        },
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            this.loadData();
            this.bindingScroll();
        },
        bindingScroll: function () {
            var _this = this;
            $(window).bind('scroll', function (e) {
                if (!_this.log.loading) {
                    var _wh = $(window).height();
                    var _st = $('body').get(0).scrollTop;
                    var _sh = $('body').get(0).scrollHeight;
                    if ((_sh - _st - _wh < 10) && (_this.log.list) && (_this.log.list.length < _this.log.total)) {
                        _this.loadMore();
                    }
                }
            });
        },
        loadData: function () {
            var _this = this;
            $('#loadingToast').show();
            _this.log.loading = true;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Distribution/GetEstimateList.ashx',
                data: {
                    score_events: '',
                    sum_col: 'Score',
                    rows: _this.log.rows,
                    page: _this.log.page
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    _this.log.loading = false;
                    if (resp.status) {
                        _this.log.total = resp.result.totalcount;
                        _this.log.sum = resp.result.sum;
                        for (var i = 0; i < resp.result.list.length; i++) {
                            var ri = resp.result.list[i];
                            var rdt = new Date(ri.time_str);
                            ri.start_d = rdt.format('MM-dd');
                            ri.start_m = rdt.format('hh:mm');
                        }
                        _this.log.list = _this.log.list.concat(resp.result.list);
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        loadMore: function () {
            if (this.log.list.length >= this.log.total) return;
            this.log.page++;
            this.loadData();
        }
    }
});

$(function () {
    commVm.init();
});