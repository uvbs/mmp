var centerVm = new Vue({
    el: '.wrapMemberCenter',
    data: {
        have_openid:have_openid,
        login_user: login_user,
        nav: false,
        nav_key: 'MallHome',
        nav_property: 'navs_2',
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            var checkPayPwdDate = sessionStorage.getItem('checkPayPwdDate');
            if (checkPayPwdDate && parseInt(checkPayPwdDate) < new Date().getTime()) {
                sessionStorage.removeItem('checkPayPwd');
            }
            sessionStorage.removeItem('withrawData');
            this.loadUser();
            //this.loadNav();
            this.checkLevel();
        },
        loadUser:function(){
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
                        _this.checkLevel();
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        checkLevel: function () {
            if (this.login_user && this.login_user.level > 10) {
                $(this.$el).find('.team').css('display', 'flex');
            }
        },
        goRegister: comGo.goRegister,
        goUpgradeMember: comGo.goUpgradeMember,
        goRecharge: comGo.goRecharge,
        goOfflineRecharge: comGo.goOfflineRecharge,
        goTransferAccounts: comGo.goTransferAccounts,
        goWithdraw: comGo.goWithdraw,
        goFinancialDetails: comGo.goFinancialDetails,
        goAccountAmount: comGo.goAccountAmount,
        goMyTeam: comGo.goMyTeam,
        goMyTeamPerformance: comGo.goMyTeamPerformance,
        goHelpApplyMember: comGo.goHelpApplyMember,
        goEstimateAmount: comGo.goEstimateAmount,
        goEstimateFund: comGo.goEstimateFund,
        goUploadCredentials:comGo.goUploadCredentials,
        goBankCardList:comGo.goBankCardList,
        goUpdatePassword:comGo.goUpdatePassword,
        loginout: comGo.loginout,
        goFeedback: comGo.goFeedback,
        loadNavConfig: function () {
            var _this = this;
            $.ajax({
                type: 'get',
                url: "/serv/api/Component/GetKeyConfig.ashx",
                data: { key: _this.nav_key, property: _this.nav_property },
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    if (data.status && data.result && data.result.length > 0) {
                        _this.nav = data.result[0];
                    }
                }
            });
        },
        loadNavList: function () {

        },
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