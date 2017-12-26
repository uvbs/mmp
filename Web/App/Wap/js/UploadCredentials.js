var commVm = new Vue({
    el: '.wrapComm',
    data: {
        images: [],
        form: {},
        check:{inup:false},
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            $(this.$el).find('.comm.defhide').css('display', 'block');
            if ($.trim(login_user.ex1)) this.images.push({ url: $.trim(login_user.ex1), error: false, ok: true, progress: '100%' });
            if ($.trim(login_user.ex2)) this.images.push({ url: $.trim(login_user.ex2), error: false, ok: true, progress: '100%' });
            if ($.trim(login_user.ex3)) this.images.push({ url: $.trim(login_user.ex3), error: false, ok: true, progress: '100%' });
            if ($.trim(login_user.ex4)) this.images.push({ url: $.trim(login_user.ex4), error: false, ok: true, progress: '100%' });
            if ($.trim(login_user.ex5)) this.images.push({ url: $.trim(login_user.ex5), error: false, ok: true, progress: '100%' });
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
        change: function () {
            var _this = this;
            var postData = {};
            for (var i = 0; i < 5; i++) {
                if (_this.images[i] && _this.images[i].url) {
                    postData['ex' + (i + 1)] = _this.images[i].url;
                } else {
                    postData['ex' + (i + 1)] = '';
                }
            }
            zcConfirm('确认修改执照、身份证信息？', '确定', '关闭', function () {
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/UpdateExInfo.ashx',
                data: postData,
                dataType: 'json',
                success: function (resp) {
                    if (resp.status == true) {
                        zcAlert("资质已上传");
                    }
                    else {
                        zcAlert(resp.msg);
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