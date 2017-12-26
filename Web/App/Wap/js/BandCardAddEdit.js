
var handlerUrl = "/Handler/App/DistributionHandler.ashx";

$(function () {
    if (getQueryString("id") != "" && getQueryString("id") != undefined) {
        $("#ddlBankName").val(loadBankName);
        $("#btnDel").show();
    }
});
function addEdit() {

    var accountName = $("#txtAccountName").val();
    var bankAccount = $("#txtBankAccount").val();
    var bankName = $("#ddlBankName").val();
    if (bankName== "") {
        zcAlert("请选择银行");
        return false;
    }
    if (bankAccount == "") {
        zcAlert("请输入银行卡号");
        return false;
    }
    if (bankAccount.length < 16 || bankAccount.length >19) {
        zcAlert("银行卡号有误");
        return false;
    }
    if (accountName == "") {
        zcAlert("请输入持卡人姓名");
        return false;
    }

    var data = {

        Action: getAction(),
        AutoID: getQueryString("id") == null ? 0 : getQueryString("id"),
        AccountName: accountName,
        BankAccount: bankAccount,
        BankName: bankName

    }
    $.ajax({
        type: 'post',
        url: handlerUrl,
        data: data,
        dataType: 'json',
        success: function (resp) {
            if (resp.Status == 1) {
                history.back(-1);
                //window.location.href = "BankCardList.aspx";
            }
            else {
                zcAlert(resp.Msg);
            }
        }
    });



}
function delEdit() {
    var id = getQueryString("id");
    if (id) {
        var data = {
            Action: 'DelBankCard',
            AutoID: id
        }
        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: data,
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 1) {
                    history.back(-1);
                    //window.location.href = "BankCardList.aspx";
                }
                else {
                    zcAlert(resp.Msg);
                }
            }
        });

    }
}
function getAction() {
    var action = "AddBankCard";
    if (getQueryString("id") != "" && getQueryString("id")!=undefined) {
        action = "EditBankCard";
    }
    return action;
}
$(function () {
    $("#bottom_text").html(baseData.wrapBottomHtml);
})

