
var commVm = new Vue({
    el: '.wrapComm',
    data: {
        pid: GetParm('pid'),
        me: {},
        childrens:[],
        team: {
            limitlevel:1,
            page: 1,
            rows: 10000,
            list: [],
            total: 1,
            loading: false
        },
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            this.loadData();
            //this.bindingScroll();
        },
        bindingScroll: function () {
            var _this = this;
            $(window).bind('scroll', function (e) {
                if (!_this.team.loading) {
                    var _wh = $(window).height();
                    var _st = $('body').get(0).scrollTop;
                    var _sh = $('body').get(0).scrollHeight;
                    if ((_sh - _st - _wh < 10) && (_this.team.list) && (_this.team.list.length < _this.team.total)) {
                        _this.loadMore();
                    }
                }
            });
        },
        loadData: function () {
            var _this = this;
            $('#loadingToast').show();
            _this.team.loading = true;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Distribution/GetChildrens.ashx',
                data: {
                    rows: 10000,
                    page: 1,
                    hide_name: 1,
                    hide_phone: 1,
                    has_base: 1,
                    parent_id: _this.pid
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    _this.team.loading = false;
                    if (resp.status) {
                        //resp.result.base_user.empty_bill_txt = empty_bill == 1 ? '空单' : '实单';
                        _this.me = resp.result.base_user;
                        _this.childrens = resp.result.list;
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        loadMore: function () {
            if (this.team.list.length >= this.team.total) return;
            this.team.page++;
            this.loadData();
        },
        goChildren: function (li) {
            window.location.href = '/app/wap/MyTeam.aspx?pid='+li.id;
        }
    }
});

$(function () {
    commVm.init();
});