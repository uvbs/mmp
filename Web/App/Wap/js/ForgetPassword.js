
var loginVm = new Vue({
    el: '.wrapLogin',
    data: {
        has_reg: false,
        has_forget: true,
        forget_paypwd: forget_paypwd,
        login_user: false,
        form: {
            phone: '',
            vcode: '',
            pay_pwd:''
        },
        num:0,
        itv: 0,
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            if (this.forget_paypwd) this.loadUser();
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
                        _this.form.phone = resp.result.phone;
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        getvcode: function () {
            if (this.num > 0) return;
            var _this = this;
            _this.setItv();
            $.ajax({
                type: 'post',
                url: '/Serv/API/Common/SmsVerCode.ashx',
                data: {
                    phone: _this.form.phone,
                    smscontent: '您的验证码为:{{SMSVERCODE}}',
                    is_reset: 1,
                    is_member:1
                },
                dataType: 'json',
                success: function (resp) {
                    if(resp.errcode != 0){
                        _this.num = 0;
                        clearInterval(_this.itv);
                        zcAlert(resp.errmsg);
                    }
                },
                error: function () {
                    _this.num = 0;
                    clearInterval(_this.itv);
                }
            });
        },
        setItv:function(){
            var _this = this;
            _this.num = 90;
            _this.itv = setInterval(function(){
                _this.num --;
                if(_this.num<=0) clearInterval(_this.itv);
            },1000);
        },
        showgetbtn: function () {
            $(this.$el).find('.btn-vcode').css('display','block');
        },
        loginbinding: function () {
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Serv/API/Common/ResetPassword.ashx',
                data: {
                    phone: _this.form.phone,
                    vcode: _this.form.vcode
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    zcAlert(resp.msg);
                }
            });
        },
        loginbinding: function () {
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Serv/API/Common/ResetPassword.ashx',
                data: {
                    phone: _this.form.phone,
                    vcode: _this.form.vcode
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    if (resp.status) {
                        zcAlert(resp.msg, '', 3, function () {
                            window.history.go(-1);
                        });
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        setPayPassword:function(){
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Serv/API/Common/ForgetPayPassword.ashx',
                data: {
                    vcode: _this.form.vcode,
                    pay_pwd: _this.form.pay_pwd
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    if (resp.status) {
                        sessionStorage.setItem('checkPayPwd', 1);
                        sessionStorage.setItem('checkPayPwdDate', (new Date().getTime() + 3600000));
                        zcAlert(resp.msg, '', 3, function () {
                            window.history.go(-1);
                        });
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        goRegister: function () {
        }
    }
});

$(function () {
    loginVm.init();
    loginVm.showgetbtn();
});