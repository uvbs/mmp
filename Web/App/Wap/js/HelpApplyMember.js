
var applyVm = new Vue({
    el: '.wrapApplyMember',
    data: {
        level: GetParm('level'),
        spreadid: GetParm('spreadid'),
        login_user: login_user,
        has_forget: true,
        province_data: [],
        city_data: [],
        city_show_data: [],
        district_data: [],
        district_show_data: [],
        town_data: [],
        town_show_data: [],
        level_data: [],
        cardCoupon:[],
        select: {
            amount: 0,
            diffamount: 0
        },
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
            payMethod:''
        },
        check: {
            level: {
                ok: false,
                has_arrow: false,
                error:false,
                text: '请选择会员级别',
                diffamount: 0
            },
            phone: {
                ok: true
            },
            idcard: {
                ok: true
            },
            spread: {
                edit:false,
                ok: false,
                text: '',
                spreadid: '',
                ospreadid: ''
            },
            area: {
                ok: false,
                text: '请选择地区'
            },
            payWay: false
        },
        bottom_text: baseData.wrapBottomHtml,
        applySubmitLock:false
    },
    methods: {
        init: function () {
            this.loadUser();
            this.loadSpread();
            this.loadLevelData();
            this.loadAreaData(1, false);
            this.loadCardCouponList();
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

                    if (_this.cardCoupon[0]) {
                        _this.form.payMethod = _this.cardCoupon[0].ex1;
                    }
                }
            });
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
                    }
                    else {
                        //zcAlert(resp.msg);
                    }
                }
            });
        },
        loadSpread:function(){
            if(!this.spreadid){
                this.check.spread.edit = true;
                return;
            }
            this.check.spread.spreadid = this.spreadid;
            this.checkSpread();
        },
        loadLevelData:function(){
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
        showLevelDialog:function(){
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
            _this.select.amount = item.from_score;
            _this.computeAmount();
            $(_this.$el).find('.member-level-dialog').css('display', 'none');
        },
        computeAmount: function () {
            var _this = this;
            _this.select.diffamount = _this.login_user.totalamount - this.select.amount;
            if (_this.select.diffamount >= 0||_this.check.payWay) {
                _this.check.level.error = false;
            } else {
                _this.check.level.error = true;
            }
        },
        loadAreaData: function (num,fn) {
            var _this = this;
            var pdata = {
                action:'Provinces'
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
            $(_this.$el).find('.member-area-dialog-'+num).css('display', 'block');
        },
        selectPayWay:function(isPay){
            this.check.payWay = isPay;
            this.computeAmount();
        },
        selectPayMethod: function (isPay) {
            this.form.payMethod = isPay;
            this.computeAmount();
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
                _this.check.area.text = _this.form.province +' '+ _this.form.city;
            }
            else if (num == 3) {
                _this.form.district = item.name;
                _this.form.districtCode = item.code;
                _this.check.area.text = _this.form.province + ' '+ _this.form.city + ' '+ _this.form.district;
            }
            else if (num == 4) {
                _this.form.town = item.name;
                _this.form.townCode = item.code;
                _this.check.area.text = _this.form.province + ' '+ _this.form.city + ' '+ _this.form.district+ ' '+ _this.form.town;
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
                            },100);
                        }
                    })
                }
            }
        },
        clearDialog: function () {
            $(this.$el).find('.comm-dialog').css('display', 'none');
        },
        checkData: function () {
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
                zcAlert('推荐人有误');
                return false;
            }
            if (this.form.level == 12 || this.form.level == 13) {
                if (!this.check.payWay ) {
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
                this.form.spreadid = '';
                this.check.spread.ospreadid = false;
                this.check.spread.ok = false;
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
                        _this.check.spread.text = '未找到';
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
        hasAreaList: function (num,pcode) {
            var _this = this;
            if (num == 1) {
                return _this.province_data.length>0 ? true : false;
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

        applyCoupon: function () {
            var _this=this;
            if (!_this.check.payWay) return;
            if (!_this.checkData()) return;
            if (!_this.form.payMethod) {
                zcAlert('暂无优惠券');
                return;
            }
            zcConfirm('确认开始注册？', '确定', '关闭', function () {
                $('#loadingToast').show();
                if (_this.applySubmitLock) {
                    return;
                }
                _this.applySubmitLock = true;
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/User/CouponRegister.ashx',
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
                            zcAlert('注册完成，密码会发送到所填手机，请稍候', '', 3, function () {
                                _this.resetForm();
                            });
                        }
                        else {
                            zcAlert(resp.msg);
                            _this.applySubmitLock = false;
                        }
                    }
                });
            });
        },
        apply: function () {
            var _this = this;
            if (!_this.checkData()) {
                return;
            }
            zcConfirm('确认开始注册？', '确定', '关闭', function () {
                $('#loadingToast').show();
                if (_this.applySubmitLock) {
                    return;
                }
                _this.applySubmitLock = true;
                $.ajax({
                    type: 'post',
                    url: '/Serv/API/User/TotalAmountRegister.ashx',
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
                            zcAlert('注册完成，密码会发送到所填手机，请稍候', '', 3, function () {
                                _this.resetForm();
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
        goRegister: comGo.goRegister,
        goRecharge: comGo.goRecharge,
        goForgetPassword: comGo.goForgetPassword,
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
                townCode: ''
            };
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
                area: {
                    ok: false,
                    text: '请选择地区'
                }
            }
        }
    }
});

$(function () {
    applyVm.init();
});