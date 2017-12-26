
var centerVm = new Vue({
    el: '.wrapComm',
    data: {
        login_user: {},
        bw:0,
        nav: false,
        nav_key: 'MallHome',
        nav_property: 'navs_2',
        orderWaitPayCount: 0,
        orderWaitSendCount: 0,
        orderWaitGetCount: 0,
        orderWaitReviewCount: 0,
        orderWaitBackCount: 0
    },
    methods: {
        init: function () {
            var checkPayPwdDate = sessionStorage.getItem('checkPayPwdDate');
            if (checkPayPwdDate && parseInt(checkPayPwdDate) < new Date().getTime()) {
                sessionStorage.removeItem('checkPayPwd');
            }
            sessionStorage.removeItem('withrawData');
            this.bw = $(window).width();
            this.loadUser();
            this.loadNav();
            this.loadOrderCount();
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
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        loadNav: function () {
            var _this = this;
            $.ajax({
                type: 'get',
                url: "/serv/api/Component/GetKeyConfig.ashx",
                data: { key: _this.nav_key, property: _this.nav_property },
                dataType: "json",
                success: function (data) {
                    if (data.status && data.result && data.result.length > 0) {
                        _this.nav = data.result[0];
                        _this.nav.col_width = _this.bw / _this.nav.col_count;
                        console.log(_this.nav);
                    }
                }
            });
        },
        loadOrderCount: function () {
            this.loadOrderList('待付款', 'orderWaitPayCount');
            this.loadOrderList('待发货', 'orderWaitSendCount');
            this.loadOrderList('待收货', 'orderWaitGetCount');
            this.loadOrderList('交易成功', 'orderWaitReviewCount');
            this.loadOrderList('退款退货', 'orderWaitBackCount');
        },
        loadOrderList: function (status, key) {
            var _this = this;
            var postModel = {
                action: 'list',
                pageindex: 1,
                pagesize: 0,
                order_status: status,
                order_type: '0,2',
                is_appeal: 0,
                is_refund: 0
            };
            if (key == 'orderWaitReviewCount') postModel.has_review = 0;
            $.ajax({
                type: 'post',
                url: '/serv/api/mall/order.ashx',
                data: postModel,
                dataType: 'json',
                success: function (resp) {
                    if (resp.totalcount) _this[key] = resp.totalcount;
                }
            });
        },
        goNull: comGo.goNull,
        goSignIn:comGo.goSignIn,
        goScore:comGo.goScore,
        goUpgradeMember:comGo.goUpgradeMember,
        goRecharge:comGo.goRecharge,
        goOfflineRecharge:comGo.goOfflineRecharge,
        goTransferAccounts:comGo.goTransferAccounts,
        goWithdraw:comGo.goWithdraw,
        goFinancialDetails:comGo.goFinancialDetails,
        goFinancialDetail1:comGo.goFinancialDetail1,
        goAccountAmount:comGo.goAccountAmount,
        goMyTeam:comGo.goMyTeam,
        goMyTeamPerformance:comGo.goMyTeamPerformance,
        goHelpApplyMember:comGo.goHelpApplyMember,
        goEstimateAmount:comGo.goEstimateAmount,
        goEstimateFund:comGo.goEstimateFund,
        goUploadCredentials:comGo.goUploadCredentials,
        goBankCardList:comGo.goBankCardList,
        goUpdatePassword:comGo.goUpdatePassword,
        goFeedback: comGo.goFeedback,
        goOrder: comGo.goOrder,
        goShopBasket: comGo.goShopBasket,
        goCollect: comGo.goCollect,
        goSVCard: comGo.goSVCard,
        goSale: comGo.goSale,
        goTgCode: comGo.goTgCode,
        loginout: comGo.loginout,
        bindOpenId: function () {
            var _this = this;
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/bindWXOpenId.ashx',
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        _this.login_user.wxbind = true;
                        zcAlert('绑定成功');
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        }
    }
});

$(function () {
    centerVm.init();
});