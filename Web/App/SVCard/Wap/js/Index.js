
var indexVm = new Vue({
    el: '.wrapCardCoupon',
    data: {
        card_count: 0,
        coupon_count: 0
    },
    methods: {
        init: function () {
            this.loadCardCount();
            this.loadCouponCount();
        },
        loadCardCount: function () {
            var _this = this;
            $.ajax({
                type: 'post',
                url: '/Serv/API/SVCard/GetCount.ashx',
                data: {
                    status: 0
                },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.card_count = resp.result;
                    }
                    else {
                        alert(resp.msg);
                    }
                }
            });
        },
        loadCouponCount: function () {
            var _this = this;
            $.ajax({
                type: 'post',
                url: '/Serv/API/Mall/Coupon/GetCount.ashx',
                data: {
                    is_can_use: 1
                },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.coupon_count = resp.result;
                    }
                    else {
                        alert(resp.msg);
                    }
                }
            });
        },
        goBack: function () {
            window.history.back();
        },
        goMySVCard: function () {
            window.location.href = '/App/SVCard/Wap/List.aspx';
        },
        goMyCoupon: function () {
            window.location.href = '/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons';
        }

    }
});

indexVm.init();
