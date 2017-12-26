var curDate = new Date();
var commVm = new Vue({
    el: '.wrapComm',
    data: {
        user: false,
        performance: false,
        form: {
            id: GetParm('id'),
            card: '',
            content: ''
        },
        images: [],
        cards: [],
        check: {
            card: {
                ok: false,
                text: '请选择银行卡<br />********'
            }
        },
        bottom_text: baseData.wrapBottomHtml,
        applyPerformanceRewardLock: false
    },
    methods: {
        init: function () {
            this.loadData();
        },
        clearDialog: function () {
            $(this.$el).find('.comm-dialog').css('display', 'none');
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
                            return;
                        }
                        for (var i = 0; i < resp.ExObj.length; i++) {
                            resp.ExObj[i].Text = resp.ExObj[i].BankName + '<br />' + resp.ExObj[i].BankAccount;
                        }
                        _this.cards = resp.ExObj;
                        _this.form.card = resp.ExObj[0].AutoID;
                        _this.check.card.text = resp.ExObj[0].Text;
                    }
                    else {
                        zcAlert('获取银行卡失败');
                    }
                }
            });
        },
        showCardDialog: function () {
            var _this = this;
            if (_this.cards.length > 0) {
                $(_this.$el).find('.comm-dialog.card-dialog').css('display', 'block');
            } else {
                zcAlert('请添加银行卡', '', 3, function () {
                    window.location.href = '/app/wap/BankCardList.aspx';
                });
            }
        },
        selectCard: function (item) {
            var _this = this;
            _this.form.card = item.AutoID;
            _this.check.card.text = item.Text;
            $(_this.$el).find('.comm-dialog.card-dialog').css('display', 'none');
        },
        loadData: function () {
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Serv/API/Performance/GetReward.ashx',
                data: {
                    id: _this.form.id
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    if (resp.status) {
                        _this.performance = resp.result;
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
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
        applyPerformanceReward: function () {

            var _this = this;

            if (_this.images.length <= 0) {
                zcAlert("请上传票据0");
                return;
            }
            var files = [];
            for (var i = 0; i < _this.images.length; i++) {
                files.push(_this.images[i].url);
            }
            zcConfirm('确认提交？', '确定', '关闭', function () {
                if (_this.applyPerformanceRewardLock) {
                    return;
                }
                _this.applyPerformanceRewardLock = true;

                $('#loadingToast').show();
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/Flow/Start.ashx',
                    data: {
                        flow_key: 'PerformanceReward',
                        rel_id: _this.form.id,
                        files: files.join(',')
                    },
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert(resp.msg, '', 2, function () {
                                window.location.href = '/app/wap/TeamPerformance.aspx';
                            });
                        }
                        else {
                            zcAlert(resp.msg);
                            _this.applyPerformanceRewardLock = false;
                        }
                    }
                });
            });
        }
    }
});

$(function () {
    commVm.init();
});