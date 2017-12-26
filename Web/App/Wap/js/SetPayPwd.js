
var commVm = new Vue({
    el: '.wrapComm',
    data: {
        form: {
            pay_pwd: ''
        },
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
        },
        setPwd: function () {
            var _this = this;
            if (!_this.form.pay_pwd || _this.form.pay_pwd.length < 6) {
                zcAlert('请输入6-20位以上支付密码');
                return;
            }
            zcConfirm('确认设置支付密码为：' + _this.form.pay_pwd + '？', '确定', '关闭', function () {
                $('#loadingToast').show();
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/User/SetPayPwd.ashx',
                    data: {
                        pay_pwd: _this.form.pay_pwd
                    },
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert(resp.msg, '', 2, function () {
                                comGo.goBack();
                            });
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