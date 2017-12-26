var commVm = new Vue({
    el: '.wrapComm',
    data: {
        card: {
            list:[]
        },
        check: {
            payPwd: {
                ok: true
            }
        },
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            //var checkPayPwd = sessionStorage.getItem('checkPayPwd');
            //if (checkPayPwd == 1) this.check.payPwd.ok = true;
            this.loadData();
        },
        checkPayPwdOk: function () {
            this.check.payPwd.ok = true;
        },
        goEdit: function (item) {
            window.location.href = '/app/wap/BankCardAddEdit.aspx?id=' + item.AutoID;
        },
        loadData: function () {
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Handler/App/DistributionHandler.ashx',
                data: {
                    Action: 'GetMyBankCard'
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    if (resp.ExObj) {
                        _this.card.list = resp.ExObj;
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
    commVm.init();
});