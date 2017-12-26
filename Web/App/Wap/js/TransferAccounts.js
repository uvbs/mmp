
var commVm = new Vue({
    el: '.wrapComm',
    data: {
        spreadid: GetParm('spreadid'),
        tab: 0,
        log: {
            page: 1,
            rows: 15,
            list: [],
            sum: 0,
            total: 1,
            loading: false
        },
        rep_year: '',
        rep_month: '',
        groupLogs: [

        ],
        form: {
            amount: '',
            spreadid: ''
        },
        check: {
            spread: {
                edit: false,
                ok: false,
                text: '',
                spreadid: '',
                ospreadid: ''
            },
            payPwd: {
                ok:false
            }
        },
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            var checkPayPwd = sessionStorage.getItem('checkPayPwd');
            if (checkPayPwd == 1) this.check.payPwd.ok = true;

            this.rep_year = new Date().format("yyyy年");
            this.rep_month = new Date().format("yyyy年M月");
            $(this.$el).find('.comm.defhide').css('display', 'block');
            $(this.$el).find('.weui-tab__panel').css('display', 'block');
            this.loadSpread();
            this.bindingScroll();
        },
        loadSpread: function () {
            if (!this.spreadid) {
                this.check.spread.edit = true;
                return;
            }
            this.check.spread.spreadid = this.spreadid;
            this.checkSpread();
        },
        checkSpread: function () {
            var _this = this;
            if (this.check.spread.spreadid == this.check.spread.ospreadid) return;
            if (!this.check.spread.spreadid) {
                this.form.spreadid = '';
                this.check.spread.ospreadid = false;
                this.check.spread.ok = false;
                this.check.spread.text = '';
                return;
            }
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/GetSpreadUser.ashx',
                data: { spreadid: _this.check.spread.spreadid },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.check.spread.ok = true;
                        _this.form.spreadid = resp.result.id;
                        _this.check.spread.text = resp.result.name;
                        _this.check.spread.ospreadid = _this.check.spread.spreadid;
                    } else {
                        _this.check.spread.ok = false;
                        _this.form.spreadid = '';
                        _this.check.spread.text = '未找到';
                        _this.check.spread.ospreadid = _this.check.spread.spreadid;
                    }
                }
            });
        },
        selectTab: function (num) {
            if (num == 1 && this.log.list.length == 0 && this.log.list.length < this.log.total) {
                this.loadData();
            }
            this.tab = num;
        },
        bindingScroll: function () {
            var _this = this;
            $(window).bind('scroll', function (e) {
                if (_this.tab === 1 && !_this.log.loading) {
                    var _wh = $(window).height();
                    var _st = $('body').get(0).scrollTop;
                    var _sh = $('body').get(0).scrollHeight;
                    if ((_sh - _st - _wh < 10) && (_this.log.list.length < _this.log.total)) {
                        _this.loadMore();
                    }
                }
            });
        },
        checkPayPwdOk: function () {
            this.check.payPwd.ok = true;
        },
        loadData: function () {
            var _this = this;
            $('#loadingToast').show();
            _this.log.loading = true;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Score/List.ashx',
                data: {
                    score_type: 'TotalAmount',
                    score_events: '转账',
                    all_score: 1,
                    sum_score: 1,
                    month_score: 1,
                    is_first_truename: 1,
                    rows: _this.log.rows,
                    page: _this.log.page
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    _this.log.loading = false;
                    if (resp.status) {
                        _this.log.total = resp.result.total;
                        _this.log.sum = resp.result.sum;
                        _this.log.list = _this.log.list.concat(resp.result.list);
                        for (var i = 0; i < resp.result.list.length; i++) {
                            var ri = resp.result.list[i];
                            var rdt = new Date(ri.time_str);
                            ri.start_d = rdt.format('MM-dd');
                            ri.start_m = rdt.format('hh:mm');
                            var group = rdt.format("yyyy年M月");
                            if (group == _this.rep_month) group = "本月";
                            group = group.replace(_this.rep_year, '');
                            var groupRows = _.where(_this.groupLogs, { group: group });
                            if (groupRows.length == 0) {
                                _this.groupLogs.push({ group: group, logs: [ri], groupAmount: ri.month_score });
                            } else {
                                groupRows[0].logs.push(ri);
                            }
                        }
                        //console.log(_this.groupLogs);
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
        },
        transfer: function () {
            var _this = this;
            if (_this.form.spreadid == '') {
                zcAlert("会员未找到");
                return;
            }
            if (_this.form.amount <= 0) {
                zcAlert("转账数额必须大于0");
                return;
            }
            zcConfirm('确认转账给' + _this.check.spread.text + '？', '确定', '关闭', function () {
                $('#loadingToast').show();
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/User/TotalAmountTransferAccounts.ashx',
                    data: {
                        amount: _this.form.amount,
                        spreadid: _this.form.spreadid
                    },
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert(resp.msg, '', 3, function () {
                                _this.resetForm();
                                //comGo.goMemberCenter();
                            });
                        }
                        else {
                            zcAlert(resp.msg);
                        }
                    }
                });
            });
        },
        resetForm: function () {
            this.form = {
                amount: '',
                spreadid: ''
            };
            this.check = {
                spread: {
                    edit: false,
                    ok: false,
                    text: '',
                    spreadid: '',
                    ospreadid: ''
                }
            };
        }
    }
});

$(function () {
    commVm.init();
});