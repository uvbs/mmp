var commVm = new Vue({
    el: '.wrapComm',
    data: {
        tab: 0,
        log: {
            page: 1,
            rows: 15,
            list: [],
            total: 1,
            sum:0,
            loading: false
        },
        rep_year: '',
        rep_month: '',
        groupLogs: [
        ],
        pay_method:{
            is_wx_pay: false,
            is_ali_pay: false,
            is_jd_pay: false
        },
        form: {
            amount: '',
            payMethod: 0
        },
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            this.rep_year = new Date().format("yyyy年");
            this.rep_month = new Date().format("yyyy年M月");
            $(this.$el).find('.comm.defhide').css('display', 'block');
            $(this.$el).find('.weui-tab__panel').css('display', 'block');
            this.bindingScroll();
            this.initPay();
        },
        initPay: function () {
            var _this = this;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Common/PayMethod.ashx',
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.pay_method.is_wx_pay = resp.result.is_wx_pay;
                        _this.pay_method.is_ali_pay = resp.result.is_ali_pay;
                        _this.pay_method.is_jd_pay = resp.result.is_jd_pay;
                        if (_this.pay_method.is_wx_pay) {
                            _this.form.payMethod = 1;
                        }
                        else if (_this.pay_method.is_ali_pay) {
                            _this.form.payMethod = 2;
                        }
                        else if (_this.pay_method.is_jd_pay) {
                            _this.form.payMethod = 3;
                        }
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
        selectPayMethod: function (num) {
            this.form.payMethod = num;
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
        loadData: function () {
            var _this = this;
            $('#loadingToast').show();
            _this.log.loading = true;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Score/List.ashx',
                data: {
                    score_type: 'TotalAmount',
                    score_events: '线上充值',
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
        recharge: function () {
            var _this = this;
            if (_this.form.amount <= 0) {
                zcAlert("充值金额必须大于0");
                return;
            }
            zcConfirm('确认充值？', '确定', '关闭', function () {
                if (_this.form.payMethod == 1) {
                    var data = {
                        handerUrl: '/Serv/API/User/Pay/WeiXin/PayRecharge.ashx',
                        successUrl: memberCenterUrl,
                        successMsg: '充值成功',
                        amount: _this.form.amount
                    }
                    sessionStorage.setItem("payData", JSON.stringify(data));
                    window.location.href = "/customize/shop/wxpay.aspx";
                } else if (_this.form.payMethod == 2){
                   $('#loadingToast').show();
                   $.ajax({
                        type: 'post',
                        url: '/Serv/API/User/Pay/Alipay/PayRecharge.ashx',
                        data: {
                            amount: _this.form.amount
                        },
                        dataType: 'json',
                        success: function (resp) {
                            $('#loadingToast').hide();
                            if (resp.status) {
                                window.location.href = "/app/wap/pay/alipay.aspx?order_id=" + resp.result.pay_order_id;
                            }
                            else {
                                zcAlert(resp.msg);
                            }
                        }
                    });
                } else if (_this.form.payMethod == 3) {
                    $('#loadingToast').show();
                    $.ajax({
                        type: 'post',
                        url: '/Serv/API/User/Pay/JDPay/PayRecharge.ashx',
                        data: {
                            amount: _this.form.amount
                        },
                        dataType: 'json',
                        success: function (resp) {
                            $('#loadingToast').hide();
                            if (resp.status) {
                                window.location.href = "/app/wap/pay/jdpay.aspx?order_id=" + resp.result.pay_order_id;
                            }
                            else {
                                zcAlert(resp.msg);
                            }
                        }
                    });
                }

            //$('#loadingToast').show();
            //$.ajax({
            //    type: 'post',
            //    url: '/Serv/API/User/Pay/WeiXin/PayRecharge.ashx',
            //    data: {
            //        amount: _this.form.amount
            //    },
            //    dataType: 'json',
            //    success: function (resp) {
            //        $('#loadingToast').hide();
            //        if (resp.status) {
            //            var paydata = {
            //                timestamp: resp.result.timeStamp,
            //                nonceStr: resp.result.nonceStr,
            //                package: resp.result.package,
            //                signType: resp.result.signType,
            //                paySign: resp.result.paySign,
            //                success: function (res) {
            //                    zcAlert('充值成功', '', 3, function () {
            //                        //_this.resetForm();
                //                          comGo.goMemberCenter();
            //                    });
            //                },
            //                cancel: function (res) {
            //                }
            //            };
            //            wx.chooseWXPay(paydata);
            //        }
            //        else {
            //            zcAlert(resp.msg);
            //        }
            //    }
            //});
            });
        },
        resetForm: function () {
            this.amount = '';
        }
    }
});

$(function () {
    commVm.init();
});