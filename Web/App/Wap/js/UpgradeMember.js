var upgradeVm = new Vue({
    el: '.wrapUpgradeMember',
    data: {
        id: GetParm('id'),
        tolevels: [],
        cardCoupon:[],
        login_user: login_user,
        noPay: noPay,
        form: {
            userid: '',
            ex1: '',
            content: '',
            level: false,
            tolevel: false,
            phone: '',
            truename: '',
            idcard: '',
            spreadid: '',
            province: '',
            provinceCode: '',
            city: '',
            cityCode: '',
            district: '',
            districtCode: '',
            town: '',
            townCode: '',
            payWay: 1,
            payMethod: 0,
            payMethodC:0,
            paypwd:''
        },
        pay_method: {
            is_wx_pay: true,
            is_ali_pay: true,
            is_jd_pay: false
        },
        select: {
            tolevel: false,
            amount: 0,
            diffamount: 0
        },
        ex1_list: [
            '现金',
            'POS机',
            '银行转账机',
            '微信转账',
            '其他'
        ],
        images: [],
        check: {
            user:{
                edit: false,
                ok: false,
                level:0,
                text: '',
                spreadid: '',
                ospreadid: ''
            },
            passwordwarp:false,
            ex1: {
                ok: false,
                text: '请选择充值渠道'
            },
            level: {
                ok: false,
                has_arrow: false,
                error: false,
                text: '请选择会员级别'
            },
            tolevel: {
                ok: false,
                has_arrow: false,
                error: false,
                text: '请选择会员级别'
            },
            phone: {
                ok: true
            },
            idcard: {
                ok: true
            },
            area: {
                ok: false,
                text: '请选择地区'
            },
            payPwd: {
                ok: false
            }
        },
        bottom_text: baseData.wrapBottomHtml,
        upgradeSubmitLock:false
    },
    methods: {
        init: function () {
            this.IsPassword();
            if (!noPay) {
                var checkPayPwd = sessionStorage.getItem('checkPayPwd');
                if (checkPayPwd == 1) this.check.payPwd.ok = true;
                if (this.login_user.level) {
                    this.loadToLevels(this.login_user.level + 1);
                    $(this.$el).find('.upgrade').css('display', 'block');
                }
                this.initPay();
                this.loadCardCouponList();
            } else {
                this.check.payPwd.ok = true;
                this.loadToLevels(this.login_user.level + 1);
                $(this.$el).find('.upgrade').css('display', 'block');
            }
        },
        IsPassword:function(){
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/IsPayPassword.ashx',
                dataType: 'json',
                success: function (resp) {
                    if (!resp.status) {
                        window.location.href = "/App/Wap/SetPayPwd.aspx";
                    }
                }
            });
        },
        loadCardCouponList: function () {
            var _this = this;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Mall/CardCoupon.ashx',
                data: { action: 'GetMyCardList' },
                dataType: 'json',
                success: function (resp) {
                    _this.cardCoupon = resp.list;
                    console.log(['_this.cardCoupon', _this.cardCoupon]);
                   
                    if (_this.cardCoupon[0]) {
                        _this.form.payMethodC = _this.cardCoupon[0].ex1;
                    }
                }
            });
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
        selectPayMethod: function (num) {
            this.form.payMethod = num;
        },
        loadToLevels: function (level) {
            var _this = this;
            var pdata = {
                type: 'DistributionOnLine',
                from: level
            }
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/Level/List.ashx',
                data: pdata,
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        if (resp.result.length == 0) {
                            _this.check.tolevel.text = '已是最高级会员';
                            _this.check.tolevel.error = true;
                            _this.check.tolevel.ok = true;
                        } else if (resp.result.length == 1) {
                            _this.check.tolevel.error = false;
                            _this.check.tolevel.ok = true;
                            _this.check.tolevel.text = resp.result[0].name;
                            _this.select.tolevel = resp.result[0].level;
                            _this.select.amount = resp.result[0].from_score;
                            _this.computeAmount();
                        }
                        else {
                            _this.tolevels = resp.result;
                            setTimeout(function () {
                                $(_this.$el).find('.cell-level').css('display', 'block');
                            }, 100);
                        }
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
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
        showLevelDialog: function () {
            var _this = this;
            if (_this.tolevels.length > 0) {
                $(_this.$el).find('.member-level-dialog').css('display', 'block');
            }
        },
        selectLevel: function (item) {
            var _this = this;
            _this.check.tolevel.text = item.name;
            _this.check.tolevel.ok = true;
            _this.select.tolevel = item.level;
            _this.form.tolevel = _this.select.tolevel;
            _this.select.amount = item.from_score;
            _this.computeAmount();
            $(_this.$el).find('.member-level-dialog').css('display', 'none');
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
                        _this.check.user.level = resp.result.level;
                        _this.check.user.ospreadid = _this.check.user.spreadid;
                    } else {
                        _this.check.user.ok = false;
                        _this.form.userid = '';
                        _this.check.user.text = '未找到';
                        _this.check.user.level = 0;
                        _this.check.user.ospreadid = _this.check.user.spreadid;
                    }
                }
            });
        },
        computeAmount: function () {
            var _this = this;
            _this.select.diffamount = _this.select.amount - _this.login_user.levelamount;
            if (_this.login_user.totalamount >= _this.select.diffamount || _this.form.payWay==2||_this.form.payWay==3) {
                _this.form.tolevel = _this.select.tolevel;
            } else {
                _this.form.tolevel = 0;
            }
        },
        checkData: function () {
            if (this.form.tolevel ==0) {
                zcAlert('请选择升级级别');
                return false;
            }
            if (this.form.tolevel == 12 || this.form.tolevel == 13) {
                if (this.form.payWay == 1 || this.form.payWay == 2) {
                    zcAlert('请选择优惠券支付');
                    return false;
                }
            }
            if (this.form.payWay==1 && this.login_user.totalamount < this.select.amount - this.login_user.levelamount) {
                zcAlert('您的' + totalAmountShowName+'不足');
                return false;
            }
            return true;
        },
        selectPayWay: function (pw) {
            this.form.payWay = pw;
            this.computeAmount();
        },
        selectPayMethodC:function(pw){
            this.form.payMethodC = pw;
            this.computeAmount();
        },
        checkPayPwdOk: function () {
            this.check.payPwd.ok = true;
        },
        upgrade: function () {
            var _this = this;
            if (!_this.checkData()) {
                return;
            }
            zcConfirm('确认升级为' + _this.check.tolevel.text + '？', '确定', '关闭', function () {
                $('#loadingToast').show();
                if (_this.upgradeSubmitLock) {
                    return;
                }
                _this.upgradeSubmitLock = true;
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/User/TotalAmountUpgrade.ashx',
                    data: {
                        toLevel: _this.form.tolevel
                    },
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert('升级成功', '', 3, function () {
                                comGo.goMemberCenter();
                            });
                        }
                        else {
                            zcAlert(resp.msg);
                            _this.upgradeSubmitLock = false;
                        }
                    }
                });
            });
        },
        payUpgrade: function () {
            var _this = this;
            if (!_this.checkData()) {
                return;
            }
            zcConfirm('确认升级为' + _this.check.tolevel.text + '？', '确定', '关闭', function () {
                if (_this.form.payMethod == 1) {
                    var data = {
                        handerUrl: '/Serv/API/User/Pay/WeiXin/PayUpgrade.ashx',
                        successUrl: memberCenterUrl,
                        successMsg: '升级成功',
                        toLevel: _this.form.tolevel
                    }
                    sessionStorage.setItem("payData", JSON.stringify(data));
                    window.location.href = "/customize/shop/wxpay.aspx";
                } else if (_this.form.payMethod == 2) {
                    $('#loadingToast').show();
                    $.ajax({
                        type: 'post',
                        url: '/Serv/API/User/Pay/Alipay/PayUpgrade.ashx',
                        data: {
                            toLevel: _this.form.tolevel
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
                        url: '/Serv/API/User/Pay/JDPay/PayUpgrade.ashx',
                        data: {
                            toLevel: _this.form.tolevel
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
            });
        },
        payConponUpgrade: function () {
            var _this = this;
            if (!_this.checkData()) {
                return;
            }
            if (_this.cardCoupon.length <= 0) {
                zcAlert("暂无优惠券");
                return;
            }

            _this.form.paypwd = "";
            $('.wrapPwd').show();
        },
        cencelUpgrade:function(){
            $('.wrapPwd').hide();
        },
        configUpgrade:function(){
            var _this = this;
            if (!_this.form.paypwd) return;
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/CheckPayPwd.ashx',
                data: {
                    pay_pwd: _this.form.paypwd
                },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        $('.wrapPwd').hide();
                        _this.form.paypwd = '';
                        if (!_this.form.payMethodC) {
                            zcAlert("请选择优惠券");
                            return;
                        }
                        if (_this.form.payMethodC == "V1" || _this.form.payMethodC == "V2") {
                            $('#loadingToast').show();
                            if (_this.upgradeSubmitLock) {
                                return;
                            }
                            _this.upgradeSubmitLock = true;
                            $.ajax({
                                type: 'post',
                                url: '/Serv/API/User/Pay/Coupon/PayUpgrade.ashx',
                                data: {
                                    toLevel: _this.form.tolevel,
                                    vType: _this.form.payMethodC
                                },
                                dataType: 'json',
                                success: function (resp) {
                                    $('#loadingToast').hide();
                                    if (resp.status) {
                                        sessionStorage.setItem('checkPayPwd', 1);
                                        sessionStorage.setItem('checkPayPwdDate', (new Date().getTime() + 3600000));
                                        _this.$emit('checkpayok');
                                        zcAlert('升级成功', '', 3, function () {
                                            comGo.goMemberCenter();
                                        });
                                    }
                                    else {
                                        zcAlert(resp.msg);
                                        _this.upgradeSubmitLock = false;
                                    }
                                }
                            });
                        }
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        goForgetPassword:function(){
            window.location.href = '/app/wap/ForgetPayPassword.aspx';
        },
        offLineUpgrade: function () {
            var _this = this;
            if (!_this.form.userid) {
                zcAlert("会员未找到");
                return;
            }
            if (!_this.form.ex1) {
                zcAlert("请选择充值渠道");
                return;
            }
            if (!_this.form.tolevel) {
                zcAlert("请选择级别");
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
            zcConfirm('确认升级为' + _this.check.tolevel.text + '？', '确定', '关闭', function () {
                $('#loadingToast').show();
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/Admin/Flow/Start.ashx',
                    data: {
                        flow_key: 'OfflineUpgrade',
                        member_userid: _this.form.userid,
                        ex1: _this.form.ex1,
                        ex2: _this.form.tolevel,
                        content: content,
                        files: files.join(',')
                    },
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert(resp.msg, '', 3, function () {
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
                ex1: '',
                content: '',
                level: false,
                tolevel: false,
                phone: '',
                truename: '',
                idcard: '',
                spreadid: '',
                province: '',
                provinceCode: '',
                city: '',
                cityCode: '',
                district: '',
                districtCode: '',
                town: '',
                townCode: '',
                payWay: 1,
                payMethod: 0
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
                tolevel: {
                    ok: false,
                    has_arrow: false,
                    error: false,
                    text: '请选择会员级别'
                },
                inup: false
            };
            $(this.$el).find(".txtContent").html('');
        },
        goRecharge: function () {
            window.location.href = '/app/wap/Recharge.aspx';
        }
    }
});

$(function () {
    upgradeVm.init();
});