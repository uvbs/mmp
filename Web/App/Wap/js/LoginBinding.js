
var loginVm = new Vue({
    el: '.wrapLogin',
    data: {
        has_reg: true,
        has_forget: true,
        form: {
            phone: '',
            password:''
        },
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
        },
        loginbinding: function () {
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/LoginBindingOpendId.ashx',
                data: {
                    phone: _this.form.phone,
                    password: _this.form.password,
                    limitmember:1
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    if (resp.status) {
                        zcAlert('登录成功', 0, 2, function () {
                            comGo.goMemberCenter();
                        });
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        goRegister: comGo.goRegister,
        goForgetPayPassword: comGo.goForgetPayPassword,
        goForgetPassword: comGo.goForgetPassword
    }
});

$(function () {
    loginVm.init();
});