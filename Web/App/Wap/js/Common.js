var baseData = {
    wrapBottomHtml: 'Copyright © 2017 天下华商月供宝 All Rights Resered',

};
var memberCenterUrl = '/customize/comeoncloud/Index.aspx?key=PersonalCenter';
var comGo = {
    goNull: function () {
        alert('即将推出，敬请期待');
    },
    goMemberCenter: function () {
        window.location.href = memberCenterUrl;
    },
    goRegister: function () {
        window.location.href = '/app/wap/ApplyMember.aspx';
    },
    goForgetPayPassword: function () {
        window.location.href = '/app/wap/ForgetPayPassword.aspx';
    },
    goForgetPassword: function () {
        window.location.href = '/app/wap/ForgetPassword.aspx';
    },
    goUpdatePassword: function () {
        window.location.href = '/app/wap/UpdatePassword.aspx';
    },
    goSignIn: function () {
        window.location.href = '/app/cation/wap/usersignin/UserSignIn.aspx';
    },
    goScore: function () {
        window.location.href = '/customize/shop/?v=1.0&ngroute=/myscores/0#/myscores/0';
    },
    goShopBasket: function () {
        window.location.href = '/customize/shop/?v=1.0&ngroute=/shopBasket#/shopBasket'
    },
    goCollect: function () {
        window.location.href = '/customize/shop/?v=1.0&ngroute=/collectList#/collectList'
    },
    goSVCard: function () {
        window.location.href = '/App/SVCard/Wap/Index.aspx'
    },
    goSale: function () {
        window.location.href = '/customize/shop/?v=1.0&ngroute=/saleList#/saleList'
    },
    goUpgradeMember: function () {
        window.location.href = '/app/wap/UpgradeMember.aspx';
    },
    goRecharge: function () {
        window.location.href = '/app/wap/Recharge.aspx';
    },
    goOfflineRecharge: function () {
        window.location.href = '/customize/comeoncloud/Index.aspx?cgid=32';
        //window.location.href = '/app/wap/OfflineRecharge.aspx';
    },
    goTransferAccounts: function () {
        window.location.href = '/app/wap/TransferAccounts.aspx';
    },
    goWithdraw: function () {
        window.location.href = '/app/wap/Withdraw.aspx';
    },
    goFinancialDetails: function () {
        window.location.href = '/app/wap/FinancialDetails.aspx';
    },
    goOfflineRechargeDetail: function (id, flowname) {
        window.location.href = '/app/wap/OfflineRechargeDetail.aspx?id=' + id + '&flowname=' + flowname;
    },
    goAccountAmount: function () {
        window.location.href = '/app/wap/FinancialDetails.aspx?hide_tab=1';
    },
    goFinancialDetail1: function () {
        window.location.href = '/app/wap/FinancialDetails.aspx?hide_tab=1&tab=1';
    },
    goMyTeam: function () {
        window.location.href = '/app/wap/MyTeam.aspx';
    },
    goMyTeamPerformance: function () {
        window.location.href = '/app/wap/TeamPerformance.aspx';
    },
    goHelpApplyMember: function () {
        window.location.href = '/app/wap/HelpApplyMember.aspx';
    },
    goEstimateAmount: function () {
        window.location.href = '/app/wap/EstimateAmount.aspx';
    },
    goEstimateFund: function () {
        window.location.href = '/app/wap/EstimateFund.aspx';
    },
    goUploadCredentials: function () {
        window.location.href = '/app/wap/UploadCredentials.aspx';
    },
    goBankCardList: function () {
        window.location.href = '/app/wap/BankCardList.aspx';
    },
    goTgCode: function () {
        window.location.href = '/App/Cation/Wap/Mall/Distribution/Index.aspx';
    },
    goFeedback: function () {
        window.location.href = 'http://webchat.7moor.com/wapchat.html?accessId=b4a96270-5c44-11e6-92b0-ff36df5fae0c&fromUrl=%E5%BE%AE%E5%95%86%E5%9F%8E';
    },
    loginout: function () {
        delAllCookie();
        window.location.href = '/app/wap/LoginBinding.aspx';
    },
    goBack: function (back_url) {
        if (back_url) {
            window.location.href = back_url;
        } else {
            history.go(-1);
        }
    },
    goOrder: function (status) {
        if (status) {
            window.location.href = '/customize/shop/?v=1.0&ngroute=/orderList#/orderList?status=' + status;
        } else {
            window.location.href = '/customize/shop/?v=1.0&ngroute=/orderList#/orderList';
        }
    }
}
function checkPhone(phone) {
    var reg = /^(((13[0-9]{1})|(14[5|7]{1})|(15[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
    return reg.test(phone);
}
function checkIdCard(idcard) {
    var reg = /^((\d{15})|(\d{18})|(\d{17})(\d|X|x))$/;
    return reg.test(idcard);
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function delAllCookie() {
    var myDate = new Date();
    myDate.setTime(-1000); //设置时间    
    var data = document.cookie;
    var dataArray = data.split("; ");
    for (var i = 0; i < dataArray.length; i++) {
        var varName = dataArray[i].split("=");
        document.cookie = varName[0] + "=''; expires=" + myDate.toGMTString();
    }

}
