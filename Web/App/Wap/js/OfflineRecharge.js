var baseData = {
    tab: 0,
    log: {
        page: 1,
        rows: 15,
        list: [],
        total: 1,
        loading: false
    },
    rep_year: '',
    rep_month: '',
    groupLogs: [

    ],
    scroll: {
        scrollTop: 0
    },
    images: [],
    ex1_list: [
        '现金',
        'POS机',
        '银行转账机',
        '微信转账',
        '其他'
    ],
    form: {
        userid:'',
        amount: '',
        ex1: '',
        content: ''
    },
    check: {
        user:{
            edit:false,
            ok: false,
            text: '',
            spreadid: '',
            ospreadid: ''
        },
        ex1: {
            ok: false,
            text: '请选择充值渠道'
        },
        inup: false
    },
    bottom_text: baseData.wrapBottomHtml
};
var withrawData = sessionStorage.getItem('withrawData');
if (withrawData) baseData = JSON.parse(withrawData);

var commVm = new Vue({
    el: '.wrapComm',
    data: baseData,
    methods: {
        init: function () {
            this.rep_year = new Date().format("yyyy年");
            this.rep_month = new Date().format("yyyy年M月");
            $(this.$el).find('.comm.defhide').css('display', 'block');
            $(this.$el).find('.weui-tab__panel').css('display', 'block');
            this.bindingScroll();
        },
        selectTab: function (num) {
            if (num == 1 && this.log.list.length == 0 && this.log.list.length < this.log.total) {
                this.loadData();
            }
            this.tab = num;
        },
        checkUser: function () {
            var _this = this;
            if (this.check.user.spreadid == this.check.user.ospreadid) return;
            if (!this.check.user.spreadid) {
                this.form.userid = '';
                this.check.user.ospreadid = false;
                this.check.user.ok = false;
                this.check.user.text = '';
                return;
            }
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/GetSpreadUser.ashx',
                data: { spreadid: _this.check.user.spreadid },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.check.user.ok = true;
                        _this.form.userid = resp.result.uid;
                        _this.check.user.text = resp.result.name;
                        _this.check.user.ospreadid = _this.check.user.spreadid;
                    } else {
                        _this.check.user.ok = false;
                        _this.form.userid = '';
                        _this.check.user.text = '未找到';
                        _this.check.user.ospreadid = _this.check.user.spreadid;
                    }
                }
            });
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
                    flow_key: 'OfflineRecharge',
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
                                ri.status_s = '充值成功';
                            }
                            else if (ri.status == 8) {
                                ri.status_s = '审核未过';
                            }
                            else if (ri.status == 10) {
                                ri.status_s = '已取消';
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
        showEx1Dialog: function () {
            $(this.$el).find('.comm-dialog.ex1-dialog').css('display', 'block');
        },
        clearDialog: function () {
            $(this.$el).find('.comm-dialog').css('display', 'none');
        },
        selectEx1: function (item) {
            this.form.ex1 = item;
            this.check.ex1.text = item;
            this.check.ex1.ok = true;
            $(this.$el).find('.comm-dialog.ex1-dialog').css('display', 'none');
        },
        addFile: function (e) {
            if (this.check.inup) return;
            var ob = e.target;
            var files = e.target.files || e.dataTransfer.files
            if (!files.length) {
                zcAlert('请选择图片');
                return;
            }
            var _this = this;
            var index = _this.images.length;
            _this.check.inup = true;
            _this.images.push({ url: '', error: false, ok: false, progress: '0%' });
            zcUpload(
                files[0],
                800,
                0,
                function (progress) {
                    if (progress.lengthComputable) {
                        var percentComplete = Math.round(progress.loaded * 100 / progress.total);
                        _this.images[index].progress = percentComplete.toString() + '%';
                    }
                },
                function (complete) {
                    _this.check.inup = false;
                    var resp = JSON.parse(complete.target.responseText);
                    if (resp.errcode == 0 && resp.file_url_list && resp.file_url_list.length > 0) {
                        _this.images[index].progress = '100%';
                        _this.images[index].ok = true;
                        _this.images[index].url = resp.file_url_list[0];
                        $(ob).val('');
                    }
                    else {
                        _this.images[index].error = true;
                        zcAlert(resp.errmsg);
                        $(ob).val('');
                    }
                },
                function (error) {
                    _this.check.inup = false;
                    _this.images[index].error = true;
                    zcAlert('上传出错');
                    $(ob).val('');
                }
            );
        },
        updateFile: function (e, index) {
            if (this.check.inup) return;
            var ob = e.target;
            var files = e.target.files || e.dataTransfer.files
            if (!files.length) {
                zcAlert('请选择图片');
                return;
            }
            var _this = this;
            _this.check.inup = true;
            _this.images[index].error = false;
            _this.images[index].ok = false;
            _this.images[index].progress = '0%';
            zcUpload(
                files[0],
                800,
                0,
                function (progress) {
                    if (progress.lengthComputable) {
                        var percentComplete = Math.round(progress.loaded * 100 / progress.total);
                        _this.images[index].progress = percentComplete.toString() + '%';
                    }
                },
                function (complete) {
                    _this.check.inup = false;
                    var resp = JSON.parse(complete.target.responseText);
                    if (resp.errcode == 0 && resp.file_url_list && resp.file_url_list.length > 0) {
                        _this.images[index].progress = '100%';
                        _this.images[index].ok = true;
                        _this.images[index].url = resp.file_url_list[0];
                        $(ob).val('');
                    }
                    else {
                        _this.images[index].error = true;
                        zcAlert(resp.errmsg);
                        $(ob).val('');
                    }
                },
                function (error) {
                    _this.check.inup = false;
                    _this.images[index].error = true;
                    zcAlert('上传出错');
                    $(ob).val('');
                }
            );
        },
        recharge: function () {
            var _this = this;
            if (_this.form.amount <= 0) {
                zcAlert("充值金额必须大于0");
                return;
            }
            var files = [];
            for (var i = 0; i < _this.images.length; i++) {
                files.push(_this.images[i].url);
            }
            var content = $.trim($(this.$el).find(".txtContent").html());
            if (content.length > 500) {
                zcAlert("备注最多能输入300个字");
                return;
            }
            zcConfirm('确认提交？', '确定', '关闭', function () {
                $('#loadingToast').show();
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/Flow/Start.ashx',
                    data: {
                        flow_key: 'OfflineRecharge',
                        amount: _this.form.amount,
                        ex1: _this.form.ex1,
                        content: content,
                        files: files.join(',')
                    },
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert(resp.msg, '', 2, function () {
                                comGo.goMemberCenter();
                            });
                        }
                        else {
                            zcAlert(resp.msg);
                        }
                    }
                });
            });
        },
        offLineRecharge: function () {
            var _this = this;
            if(!_this.form.userid){
                zcAlert("会员未找到");
                return;
            }
            if (_this.form.amount <= 0) {
                zcAlert("充值金额必须大于0");
                return;
            }
            if (!_this.form.ex1) {
                zcAlert("请选择充值渠道");
                return;
            }
            var files = [];
            for (var i = 0; i < _this.images.length; i++) {
                files.push(_this.images[i].url);
            }
            var content = $.trim($(this.$el).find(".txtContent").html());
            if (content.length > 500) {
                zcAlert("备注最多能输入300个字");
                return;
            }
            zcConfirm('确认提交？', '确定', '关闭', function () {
                $('#loadingToast').show();
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/Admin/Flow/Start.ashx',
                    data: {
                        flow_key: 'OfflineRecharge',
                        member_userid: _this.form.userid,
                        amount: _this.form.amount,
                        ex1: _this.form.ex1,
                        content: content,
                        files: files.join(',')
                    },
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert(resp.msg, '', 2, function () {
                                _this.resetForm();
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
            this.images = [];
            this.form = {
                userid: '',
                amount: '',
                ex1: '',
                content: ''
            };
            this.check = {
                user: {
                    edit: false,
                    ok: false,
                    text: '',
                    spreadid: '',
                    ospreadid: ''
                },
                ex1: {
                    ok: false,
                    text: '请选择充值渠道'
                },
                inup: false
            };
            $(this.$el).find(".txtContent").html('');
        },
        goDetail: function (log) {
            sessionStorage.setItem('withrawData', JSON.stringify(this._data));
            comGo.goOfflineRechargeDetail(log.id, log.flowname);
        }
    }
});

$(function () {
    commVm.init();
});