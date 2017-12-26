Vue.component('check-pay-password', function (resolve, reject) {
    $.ajax({
        type: 'get',
        url: '/App/Wap/component/CheckPayPwd/CheckPayPwd.html?v=2017022301',
        dataType: 'html',
        success: function (resp) {
            resolve({
                template: resp,
                props: ['check'],
                data: function () {
                    return { pay_pwd: '' }
                },
                methods: {
                    checkPayPwd: function () {
                        if (!this.pay_pwd) return;
                        var _this = this;
                        $('#loadingToast').show();
                        $.ajax({
                            type: 'post',
                            url: '/Serv/API/User/CheckPayPwd.ashx',
                            data: {
                                pay_pwd: _this.pay_pwd
                            },
                            dataType: 'json',
                            success: function (resp) {
                                $('#loadingToast').hide();
                                if (resp.status) {
                                    zcAlert(resp.msg, '', 2, function () {
                                        sessionStorage.setItem('checkPayPwd', 1);
                                        sessionStorage.setItem('checkPayPwdDate', (new Date().getTime() + 3600000));
                                        _this.$emit('checkpayok');
                                    });
                                }
                                else {
                                    zcAlert(resp.msg);
                                }
                            }
                        });
                    },
                    goForgetPassword: function () {
                        window.location.href = '/app/wap/ForgetPayPassword.aspx';
                    }
                }
            })
        }
    });
})