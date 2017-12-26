var baseData = {
    tab: 0,
    login_user: false,
    rep_year: '',
    rep_month: '',
    groupLogs: [

    ],
    log: {
        page: 1,
        rows: 15,
        list: [],
        total: 1,
        loading: false
    },
    cards:[],
    form: {
        amount: '',
        ex3: '',
        content: ''
    },
    scroll: {
        scrollTop:0
    },
    check:{
        ex3: {
            ok: false,
            text: '请选择银行卡<br />********'
        },
        payPwd: {
            ok: false
        },
        canApply:0 
    },
    bottom_text: baseData.wrapBottomHtml,
    back_url: GetParm("back_url")
};
var withrawData = sessionStorage.getItem('withrawData');
if (withrawData) baseData = JSON.parse(withrawData);

var commVm = new Vue({
    el: '.wrapComm',
    data: baseData,
    methods: {
        init: function () {
            this.checkSystemTime();
            this.loadStorage();
            this.loadUser();
            this.bindingScroll();
        },
        loadStorage: function () {
            if (!withrawData) {
                var checkPayPwd = sessionStorage.getItem('checkPayPwd');
                if (checkPayPwd == 1) this.check.payPwd.ok = true;
                this.rep_year = new Date().format("yyyy年");
                this.rep_month = new Date().format("yyyy年M月");
                $(this.$el).find('.comm.defhide').css('display', 'block');
                $(this.$el).find('.weui-tab__panel').css('display', 'block');
                this.loadCard();
            } else {
                var _this = this;
                setTimeout(function () {
                    $('body').get(0).scrollTop = _this.scroll.scrollTop;
                }, 200);
            }
        },
        loadUser: function () {
            var _this = this;
            //$('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/GetUserJson.ashx',
                dataType: 'json',
                success: function (resp) {
                    //$('#loadingToast').hide();
                    if (resp.status) {
                        _this.login_user = resp.result;
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        checkSystemTime: function () {
            var _this = this;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Common/GetSystemTime.ashx',
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        var curDay = new Date(resp.result).getDate();
                        if (curDay == 5 || curDay == 15 || curDay == 25) {
                            _this.check.canApply = 1;
                        } else {
                            _this.check.canApply = 2;
                        }
                    }
                    else {
                        var curDay = new Date().getDate();
                        if (curDay == 5 || curDay == 15 || curDay == 25) {
                            _this.check.canApply = 1;
                        } else {
                            _this.check.canApply = 2;
                        }
                    }
                    //_this.check.canApply = 1;
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
                var _st = $('body').get(0).scrollTop;
                _this.scroll.scrollTop = _st;
                if (_this.tab === 1 && !_this.log.loading) {
                    var _wh = $(window).height();
                    var _sh = $('body').get(0).scrollHeight;
                    if ((_sh - _st - _wh < 10) && (_this.log.list.length < _this.log.total)) {
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
                url: '/Serv/API/Flow/List.ashx',
                data: {
                    flow_key: 'Withdraw',
                    rows: _this.log.rows,
                    page: _this.log.page
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    _this.log.loading = false;
                    if (resp.status) {
                        _this.log.total = resp.result.totalcount;
                        _this.log.list = _this.log.list.concat(resp.result.list);
                        for (var i = 0; i < resp.result.list.length; i++) {
                            var ri = resp.result.list[i];
                            var rdt = new Date(ri.start);
                            if (ri.status == 9) {
                                ri.status_s = '提现成功';
                            }
                            else if (ri.status == 8) {
                                ri.status_s = '审核未过';
                            }
                            else if (ri.status == 11) {
                                ri.status_s = '申请取消';
                            }
                            else if (ri.status == 10) {
                                ri.status_s = '已取消';
                            }
                            else if (ri.status == 12) {
                                ri.status_s = '拒绝取消';
                            } else if (ri.status == 0) {
                                ri.status_s = '处理中';
                            }
                            ri.start_d = rdt.format('MM-dd');
                            ri.start_m = rdt.format('hh:mm');
                            var group = rdt.format("yyyy年M月");
                            if (group == _this.rep_month) group = "本月";
                            group = group.replace(_this.rep_year, '');
                            var groupRows = _.where(_this.groupLogs, { group: group });
                            if (groupRows.length == 0) {
                                _this.groupLogs.push({ group: group, logs: [ri], groupAmount: ri.amount });
                            } else {
                                groupRows[0].groupAmount += ri.amount;
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
        loadCard: function () {
            var _this = this;
            //$('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Handler/App/DistributionHandler.ashx',
                data: {
                    Action: 'GetMyBankCard'
                },
                dataType: 'json',
                success: function (resp) {
                    //$('#loadingToast').hide();
                    if (resp.ExObj) {
                        if (resp.ExObj.length == 0) {
                            zcAlert('请添加银行卡', '', 3, function () {
                                window.location.href = '/app/wap/BankCardList.aspx';
                            });
                            return;
                        }
                        for (var i = 0; i < resp.ExObj.length; i++) {
                            resp.ExObj[i].Text = resp.ExObj[i].BankName + '<br />' + resp.ExObj[i].BankAccount;
                        }
                        _this.cards = resp.ExObj;
                        _this.form.ex3 = resp.ExObj[0].AutoID;
                        _this.check.ex3.text = resp.ExObj[0].Text;
                    }
                    else {
                        zcAlert('获取银行卡失败');
                    }
                }
            });
        },
        showCardDialog: function () {
            var _this = this;
            if (_this.cards.length>0) {
                $(_this.$el).find('.comm-dialog').css('display', 'block');
            }
        },
        selectCard: function (item) {
            var _this = this;
            _this.form.ex3 = item.AutoID;
            _this.check.ex3.text = item.Text;
            $(_this.$el).find('.comm-dialog').css('display', 'none');
        },
        clearDialog: function () {
            $(this.$el).find('.comm-dialog').css('display', 'none');
        },
        checkPayPwdOk: function () {
            this.check.payPwd.ok = true;
        },
        goDetail: function (log) {
            sessionStorage.setItem('withrawData', JSON.stringify(this._data));
            var detailUrl = '/app/wap/FlowDetail.aspx?id=' + log.id + '&flowname=' + log.flowname;
            
            if (this.back_url) {
                detailUrl += "&withdraw_back_url=" + this.back_url;
            }

            window.location.href = detailUrl;
        },
        withdraw: function () {
            var _this = this;
            if (!_this.form.ex3) {
                zcAlert("请选择银行卡");
                return;
            }
            if (_this.form.amount <= 0) {
                zcAlert("提现金额必须大于0");
                return;
            }
            zcConfirm('确认提现？', '确定', '关闭', function () {
                $('#loadingToast').show();
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/Flow/Start.ashx',
                    data: {
                        flow_key: 'Withdraw',
                        amount: _this.form.amount,
                        ex3: _this.form.ex3,
                        content: _this.form.content
                    },
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert(resp.msg, '', 3, function () {
                                sessionStorage.removeItem('withrawData');
                                if (!_this.back_url) {
                                    comGo.goMemberCenter();
                                } else {
                                    comGo.goBack(_this.back_url);
                                }
                                
                            });
                        }
                        else {
                            zcAlert(resp.msg);
                        }
                    }
                });
            });
        },
        clearWithdraw: function () {
            this.form.amount = '';
            this.form.content = '';
;        }
    }
});

$(function () {
    commVm.init();
});