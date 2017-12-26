//过滤器
Vue.filter('amountFommat', function (value) {
    if (!value) return '';
    return '¥' + value;
});
Vue.filter('validFommat', function (value) {
    if (!value) return '';
    return '有效期：' + new Date(value).format('yyyy-MM-dd hh:mm');
});
Vue.filter('statusFommat', function (value) {
    if (!value) return '';
    if (value == 1) return '已使用';
    if (value == 2) return '已过期';
    if (value == 11 || value == 12) return '已转赠';
    if (value == 9 || value == 99) return '已作废';
    return '';
});

var vm = new Vue({
    el: '.wrapDetail',
    data: {
        handler: '/Serv/API/SVCard/',//处理路径
        id: GetParm('id'),//储值卡Id
        isLoading: false,//是否处理中标识
        loadingText: "请稍候...",//处理中文本
        useAmount: "",//使用金额
        maxUseAmount: 0,//最多使用金额
        remark: "",//备注
        isExpire: true,//是否已经过期
        card: {}//储值卡信息

    },
    watch: {//监视输入
        useAmount: function (val) {
            if (parseFloat(val)>parseFloat(this.maxUseAmount)) {
                alert("该储值卡最多可使用" + this.maxUseAmount + "元");
                this.useAmount = this.maxUseAmount;
            }
        }},
    methods: {//方法集合
        loadData: function () {//加载储值卡信息
            var _this = this;
            _this.isLoading = true;
            $.ajax({
                type: 'post',
                url: _this.handler + 'Get.ashx',
                data: {
                    id: _this.id
                    
                },
                dataType: 'json',
                success: function (resp) {
                    _this.isLoading = false;
                    if (resp.status) {
                        _this.card = resp.result;
                        var dateTo = new Date(_this.card.valid_to);
                        if (dateTo <= new Date()) {
                            _this.isExpire = true;
                        } else {
                            _this.isExpire = false;
                        }
                        _this.maxUseAmount = _this.card.canuse_amount;
                        if (_this.maxUseAmount == 0) {
                            _this.isExpire = true;
                        }
                        _this.setPageTitle();
                    }
                    else {
                        alert(resp.msg);
                    }
                }
            });
        },
        useCard: function () {//使用储值卡
            var _this = this;
            var dateTo = new Date(_this.card.valid_to);
            if (dateTo<=new Date()) {
                alert("该储值卡已过期");
                return false;
            }

            if (_this.useAmount == 0) {
                alert("请输入消费金额");
                return false;
            }
            if (_this.maxUseAmount == 0) {
                alert("该储值卡已经使用全部金额");
                return false;
            }
            if (_this.useAmount > _this.maxUseAmount) {
                alert("该储值卡最多可以使用" + _this.maxUseAmount + "元");
                return false;
            }

            if (_this.remark == "") {
                alert("请输入备注");
                return false;
            }
            layer.open({
                content: '确认使用？',
                btn: ['确认', '取消'],
                yes: function (index) {
                    _this.isLoading = true;
                    _this.loadingText = "正在处理...";
                    layer.close(index);
                    $.ajax({
                        type: 'post',
                        url: _this.handler + "Use.ashx",
                        data: { id: _this.card.id, useAmount: _this.useAmount, remark: _this.remark },
                        dataType: 'json',
                        success: function (resp) {
                            _this.isLoading = false;
                            _this.loadingText = "请稍候...";
                            if (resp.status) {
                                _this.card.use_status = 1;
                                _this.card.canuse_amount -= parseFloat(_this.useAmount);
                                _this.maxUseAmount = _this.card.canuse_amount;
                            }
                            alert(resp.msg);
                        }
                    });
                }
            });
        },
        setPageTitle: function () {//设置标题
            SetPageTitle(this.card.name);
        }

    }
});
vm.loadData();//加载储值卡详情



