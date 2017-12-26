
var commVm = new Vue({
    el: '.wrapComm',
    data: {
        hideTab: GetParm('hide_tab'),
        tab: -1,
        tabEvents: [
            '',
            '返利,返购房补助,撤单扣返利,撤单扣购房补助,变更扣返利,变更扣购房补助',
            '申请提现,提现退款',
            '转账,获得转账',
            '线上充值,线下充值,线上注册充值,线下注册充值,注册充值,升级充值',
            '注册会员,替他人注册,他人注册转入,他人代替注册,补账,撤单,下级撤单,冲正,变更推荐人,管理奖'
        ],
        log: {},
        rep_year: '',
        rep_month: '',
        data: {},
        check:{
            payPwd: {
                ok: true
            }
        },
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            //var checkPayPwd = sessionStorage.getItem('checkPayPwd');
            //if (checkPayPwd == 1) this.check.payPwd.ok = true;

            this.rep_year = new Date().format("yyyy年");
            this.rep_month = new Date().format("yyyy年M月");
            $(this.$el).find('.comm.defhide').css('display', 'block');
            $(this.$el).find('.weui-tab__panel').css('display', 'block');
            var nTab = GetParm('tab');
            nTab = !nTab ? 0 : Number(nTab);
            this.selectTab(nTab);
            this.bindingScroll();
        },
        selectTab: function (num) {
            if (!this.log.loading) {
                var n = this.tab;
                if (this.log.list && this.log.list.length>0) {
                    this.data['l' + n] = JSON.parse(JSON.stringify(this.log));
                }
                if (this.data['l' + num]) {
                    this.log = this.data['l' + num];
                } else {
                    this.log = {
                        page: 1,
                        score_events: this.tabEvents[num],
                        rows: 15,
                        list: [],
                        total: 1,
                        sum:0,
                        groupLogs: [],
                        loading: false
                    }
                    this.loadData();
                }
                this.tab = num;
            }
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
                url: '/Serv/API/Score/List.ashx',
                data: {
                    score_type: 'TotalAmount',
                    score_events: _this.log.score_events,
                    all_score:1,
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
                            var groupRows = _.where(_this.log.groupLogs, { group: group });
                            if (groupRows.length == 0) {
                                _this.log.groupLogs.push({ group: group, logs: [ri], groupAmount: ri.month_score });
                            } else {
                                groupRows[0].logs.push(ri);
                            }
                        }
                        //console.log(_this.log.groupLogs);
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
        checkPayPwdOk: function () {
            this.check.payPwd.ok = true;
        },
        transfer: function () {
            var _this = this;
            if (_this.form.spreadid == '') {
                zcAlert("会员未找到");
                return;
            }
            if (_this.form.amount <= 0) {
                zcAlert("充值金额必须大于0");
                return;
            }
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
                            comGo.goMemberCenter();
                        });
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        }
    }
});

$(function () {
    commVm.init();
});