
var applyVm = new Vue({
    el: '.wrapApplyMember',
    data: {
        level: GetParm('level'),
        spreadid: GetParm('spreadid'),
        empty_bill: GetParm('empty_bill'),
        has_login: true,
        province_data: [],
        city_data: [],
        city_show_data: [],
        district_data: [],
        district_show_data: [],
        town_data: [],
        town_show_data: [],
        level_data: [],
        cardCoupon:[],
        ex1_list: [
            '现金',
            'POS机',
            '银行转账机',
            '微信',
            '其他'
        ],
        images: [],
        form: {
            level: false,
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
            ex1: '',
            payMethod: 0
        },
        pay_method: {
            is_wx_pay: false,
            is_ali_pay: false,
            is_jd_pay: false
        },
        check: {
            level: {
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
            spread: {
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
            area: {
                ok: false,
                text: '请选择地区'
            },
            inup: false
        },
        user_info:{
            parent_userid: '',
            is_exists:false
        },
        bottom_text: baseData.wrapBottomHtml,
        applySubmitLock: false
    },
    methods: {
        init: function () {
            this.loadSpread();
            this.initPay();
            this.loadLevelData();
            this.loadAreaData(1, false);
            this.loadUserInfo();
            this.loadCardCouponList();
        },
        loadCardCouponList:function(){
            var _this = this;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Mall/CardCoupon.ashx',
                data: { action: 'GetMyCardList' },
                dataType: 'json',
                success: function (resp) {
                    _this.cardCoupon = resp.list;
                    console.log(['_this.cardCoupon', _this.cardCoupon]);
                }
            });
        },
        loadUserInfo:function(){
            var _this = this;
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/Info.ashx',
                data: { action: 'currentuserinfo' },
                dataType: 'json',
                success: function (resp) {
                    if (resp.distribution_owner) {
                        _this.form.spreadid = resp.distribution_owner;
                        _this.check.spread.ok = true;

                        _this.user_info.parent_userid = resp.distribution_owner;
                        _this.user_info.is_exists = true;
                    } else {
                        _this.form.spreadid = '';
                        _this.check.spread.ok = false;

                        _this.user_info.parent_userid = '';
                        _this.user_info.is_exists = false;
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
        loadSpread: function () {
            if (!this.spreadid) {
                this.check.spread.edit = true;
                return;
            }
            this.check.spread.spreadid = this.spreadid;
            this.checkSpread();
        },
        loadLevelData: function () {
            var _this = this;
            var pdata = {
                type: 'DistributionOnLine',
                from: 10
            }
            if (_this.level) pdata.level = _this.level;
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/Level/List.ashx',
                data: pdata,
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.level_data = resp.result;
                        _this.formatLevel();
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        formatLevel: function () {
            var _this = this;
            if (_this.level_data) {
                if (_this.level_data.length > 1) {
                    _this.check.level.has_arrow = true;
                    setTimeout(function () {
                        $(_this.$el).find('.cell-level').css('display', 'block');
                    }, 100)
                    return;
                } else if (_this.level_data.length == 1) {
                    _this.form.level = _this.level_data[0].level;
                    _this.check.level.text = _this.level_data[0].name;
                    _this.check.level.ok = true;
                    return;
                }
            }
            _this.check.level.text = '会员级别未找到';
            _this.check.level.ok = true;
            _this.check.level.error = true;
        },
        showLevelDialog: function () {
            var _this = this;
            if (_this.check.level.has_arrow) {
                $(_this.$el).find('.member-level-dialog').css('display', 'block');
            }
        },
        selectLevel: function (item) {
            var _this = this;
            _this.form.level = item.level;
            _this.check.level.text = item.name;
            _this.check.level.ok = true;
            $(_this.$el).find('.member-level-dialog').css('display', 'none');
        },
        loadAreaData: function (num, fn) {
            var _this = this;
            var pdata = {
                action: 'Provinces'
            }
            if (num == 2) {
                pdata.action = 'Cities';
                pdata.province_code = _this.form.provinceCode;
            }
            else if (num == 3) {
                pdata.action = 'Districts';
                pdata.city_code = _this.form.cityCode;
            }
            else if (num == 4) {
                pdata.action = 'Areas';
                pdata.district_code = _this.form.districtCode;
            }
            $.ajax({
                type: 'post',
                url: '/Serv/API/Mall/Area.ashx',
                data: pdata,
                dataType: 'json',
                success: function (resp) {
                    if (resp.list && resp.list.length > 0) {
                        if (num == 1) {
                            _this.province_data = resp.list;
                        }
                        else if (num == 2) {
                            _this.city_show_data = resp.list;
                            _this.city_data = _this.city_data.concat(resp.list);
                        }
                        else if (num == 3) {
                            _this.district_show_data = resp.list;
                            _this.district_data = _this.district_data.concat(resp.list);
                        }
                        else if (num == 4) {
                            _this.town_show_data = resp.list;
                            _this.town_data = _this.town_data.concat(resp.list);
                        }
                    }
                    if (fn) fn(resp.list);
                }
            });
        },
        showAreaDialog: function (num) {
            var _this = this;
            $(_this.$el).find('.member-area-dialog-' + num).css('display', 'block');
        },
        selectArea: function (num, item, index) {
            var _this = this;
            var nnum = num + 1;
            _this.check.area.ok = true;
            if (num == 1) {
                _this.form.province = item.name;
                _this.form.provinceCode = item.code;
                _this.check.area.text = _this.form.province;
            }
            else if (num == 2) {
                _this.form.city = item.name;
                _this.form.cityCode = item.code;
                _this.check.area.text = _this.form.province + ' ' + _this.form.city;
            }
            else if (num == 3) {
                _this.form.district = item.name;
                _this.form.districtCode = item.code;
                _this.check.area.text = _this.form.province + ' ' + _this.form.city + ' ' + _this.form.district;
            }
            else if (num == 4) {
                _this.form.town = item.name;
                _this.form.townCode = item.code;
                _this.check.area.text = _this.form.province + ' ' + _this.form.city + ' ' + _this.form.district + ' ' + _this.form.town;
            }
            $(_this.$el).find('.member-area-dialog-' + num).css('display', 'none');
            if (nnum < 5) {
                if (_this.hasAreaList(nnum, item.code)) {
                    $(_this.$el).find('.member-area-dialog-' + nnum).css('display', 'block');
                } else {
                    $('#loadingToast').show();
                    _this.loadAreaData(nnum, function (list) {
                        $('#loadingToast').hide();
                        if (list && list.length > 0) {
                            setTimeout(function () {
                                $(_this.$el).find('.member-area-dialog-' + nnum).css('display', 'block');
                            }, 100);
                        }
                    })
                }
            }
        },
        showEx1Dialog: function () {
            $(this.$el).find('.comm-dialog.ex1-dialog').css('display', 'block');
        },
        selectEx1: function (item) {
            this.form.ex1 = item;
            this.check.ex1.text = item;
            this.check.ex1.ok = true;
            $(this.$el).find('.comm-dialog.ex1-dialog').css('display', 'none');
        },
        clearDialog: function () {
            $(this.$el).find('.comm-dialog').css('display', 'none');
        },
        checkData: function (offline) {
            checkIdCard();
            checkPhone();
            if (!this.check.level.ok) {
                zcAlert('请选择会员级别');
                return false;
            }
            if (!this.form.phone || !this.check.phone.ok) {
                zcAlert('请确认手机号码');
                return false;
            }
            if (!this.form.truename) {
                zcAlert('请输入姓名');
                return false;
            }
            if (!this.form.idcard || !this.check.idcard.ok) {
                zcAlert('请确认身份证号码');
                return false;
            }
            if (!this.check.spread.ok) {
                zcAlert('请输入推荐人编号/手机');
                return false;
            }

            if (offline && this.empty_bill != '1' && !this.check.ex1.ok) {
                zcAlert('请选择充值渠道');
                return false;
            }
            if (this.form.level == 12||this.form.level==13) {
                if(this.form.payMethod==1||this.form.payMethod==2||this.form.payMethod==3){
                    zcAlert('请选择优惠券支付');
                    return false;
                }
            }
            return true;
        },
        checkSpread: function () {
            var _this = this;
            if (this.check.spread.spreadid == this.check.spread.ospreadid) return;
            if (!this.check.spread.spreadid) {
                this.form.spreadid = this.user_info.parent_userid;
                this.check.spread.ospreadid = false;
                this.check.spread.ok = this.user_info.is_exists;
                this.check.spread.text = '';
                return;
            }
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/GetSpreadUser.ashx',
                data: { spreadid: _this.check.spread.spreadid },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.check.spread.ok = true;
                        _this.form.spreadid = resp.result.id;
                        _this.check.spread.text = resp.result.name;
                        _this.check.spread.ospreadid = _this.check.spread.spreadid;
                    } else {
                        _this.check.spread.ok = false;
                        _this.form.spreadid = '';
                        _this.check.spread.text = '未找到';
                        _this.check.spread.ospreadid = _this.check.spread.spreadid;
                    }
                }
            });
        },
        checkIdCard: function () {
            this.check.idcard.ok = checkIdCard(this.form.idcard);
        },
        checkPhone: function () {
            this.check.phone.ok = checkPhone(this.form.phone);
        },
        hasAreaList: function (num, pcode) {
            var _this = this;
            if (num == 1) {
                return _this.province_data.length > 0 ? true : false;
            }
            else if (num == 2) {
                _this.city_show_data = _.where(_this.city_data, { pcode: pcode });
                return _this.city_show_data.length > 0 ? true : false;
            }
            else if (num == 3) {
                _this.district_show_data = _.where(_this.district_data, { pcode: pcode });
                return _this.district_show_data.length > 0 ? true : false;
            }
            else if (num == 4) {
                _this.town_show_data = _.where(_this.town_data, { pcode: pcode });
                return _this.town_show_data.length > 0 ? true : false;
            }
            return false;
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
        applyOffLine: function () {
            var _this = this;
            if (!_this.checkData(true)) {
                return;
            }
            var content = $.trim($(this.$el).find(".txtContent").html());
            if (content.length > 500) {
                zcAlert("备注最多能输入500个字");
                return;
            }
            var files = [];
            for (var i = 0; i < _this.images.length; i++) {
                files.push(_this.images[i].url);
            }
            var postData = {
                flow_key: 'RegisterOffLine',
                level: _this.form.level,
                phone: _this.form.phone,
                truename: _this.form.truename,
                idcard: _this.form.idcard,
                spreadid: _this.form.spreadid,
                province: _this.form.province,
                provinceCode: _this.form.provinceCode,
                city: _this.form.city,
                cityCode: _this.form.cityCode,
                district: _this.form.district,
                districtCode: _this.form.districtCode,
                town: _this.form.town,
                townCode: _this.form.townCode,
                ex1: _this.form.ex1,
                content: content,
                files: files.join(',')
            };
            if (this.empty_bill == "1") {
                postData.flow_key = "RegisterEmptyBill";
            }
            zcConfirm('确认开始注册？', '确定', '关闭', function () {
                $('#loadingToast').show();
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/Admin/Flow/StartRegister.ashx',
                    data: postData,
                    dataType: 'json',
                    success: function (resp) {
                        $('#loadingToast').hide();
                        if (resp.status) {
                            zcAlert('注册完成', '', 3, function () {
                                _this.resetForm();
                            });
                        } else {
                            zcAlert(resp.msg);
                        }
                    }
                });
            });
        },
        selectPayMethod: function (num,mid) {
            this.form.payMethod = num;
        },
        apply: function () {
            var _this = this;
            if (!_this.checkData(false)) {
                return;
            }
            zcConfirm('确认注册？', '确定', '关闭', function () {
                if (_this.form.payMethod == 1) {
                    var data = {
                        handerUrl: '/Serv/API/User/Pay/WeiXin/PayRegister.ashx',
                        successUrl: '/app/wap/LoginBinding.aspx',
                        successMsg: '注册完成，密码会发送到所填手机，请稍候',
                        level: _this.form.level,
                        phone: _this.form.phone,
                        truename: _this.form.truename,
                        idcard: _this.form.idcard,
                        spreadid: _this.form.spreadid,
                        province: _this.form.province,
                        provinceCode: _this.form.provinceCode,
                        city: _this.form.city,
                        cityCode: _this.form.cityCode,
                        district: _this.form.district,
                        districtCode: _this.form.districtCode,
                        town: _this.form.town,
                        townCode: _this.form.townCode
                    }
                    sessionStorage.setItem("payData", JSON.stringify(data));
                    window.location.href = "/customize/shop/wxpay.aspx";
                } else if (_this.form.payMethod == 2) {
                    $('#loadingToast').show();
                    $.ajax({
                        type: 'post',
                        url: '/Serv/API/User/Pay/Alipay/PayRegister.ashx',
                        data: {
                            level: _this.form.level,
                            phone: _this.form.phone,
                            truename: _this.form.truename,
                            idcard: _this.form.idcard,
                            spreadid: _this.form.spreadid,
                            province: _this.form.province,
                            provinceCode: _this.form.provinceCode,
                            city: _this.form.city,
                            cityCode: _this.form.cityCode,
                            district: _this.form.district,
                            districtCode: _this.form.districtCode,
                            town: _this.form.town,
                            townCode: _this.form.townCode
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
                        url: '/Serv/API/User/Pay/JDPay/PayRegister.ashx',
                        data: {
                            level: _this.form.level,
                            phone: _this.form.phone,
                            truename: _this.form.truename,
                            idcard: _this.form.idcard,
                            spreadid: _this.form.spreadid,
                            province: _this.form.province,
                            provinceCode: _this.form.provinceCode,
                            city: _this.form.city,
                            cityCode: _this.form.cityCode,
                            district: _this.form.district,
                            districtCode: _this.form.districtCode,
                            town: _this.form.town,
                            townCode: _this.form.townCode
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
                } else if (_this.form.payMethod == "V1" || _this.form.payMethod == "V2") {
                    $('#loadingToast').show();
                    if (_this.applySubmitLock) {
                        return;
                    }
                    _this.applySubmitLock = true;
                    $.ajax({
                        type: 'post',
                        url: '/Serv/API/User/Pay/Coupon/PayRegister.ashx',
                        data: {
                            level: _this.form.level,
                            phone: _this.form.phone,
                            truename: _this.form.truename,
                            idcard: _this.form.idcard,
                            spreadid: _this.form.spreadid,
                            province: _this.form.province,
                            provinceCode: _this.form.provinceCode,
                            city: _this.form.city,
                            cityCode: _this.form.cityCode,
                            district: _this.form.district,
                            districtCode: _this.form.districtCode,
                            town: _this.form.town,
                            townCode: _this.form.townCode,
                            vType: _this.form.payMethod
                        },
                        dataType: 'json',
                        success: function (resp) {
                            $('#loadingToast').hide();
                            if (resp.status) {
                                window.location.href = "/customize/comeoncloud/Index.aspx?key=PersonalCenter";
                            }
                            else {
                                zcAlert(resp.msg);
                                _this.applySubmitLock = false;
                            }
                        }
                    });

                }
            });
        },
        goLogin: function () {
            window.location.href = '/app/wap/LoginBinding.aspx';
        },
        resetForm: function () {
            this.form = {
                level: false,
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
                ex1: ''
            };
            this.images = [];
            this.check = {
                level: {
                    ok: false,
                    has_arrow: true,
                    error: false,
                    text: '请选择会员级别'
                },
                phone: {
                    ok: true
                },
                idcard: {
                    ok: true
                },
                spread: {
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
                area: {
                    ok: false,
                    text: '请选择地区'
                },
                inup: false
            }
            $('.txtContent').html('');
        }
    }
});

$(function () {
    applyVm.init();
});