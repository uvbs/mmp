<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Withdraw.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.Withdraw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>申请提现</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="css/fenxiao.css" rel="stylesheet" type="text/css" />
    <link href="/Plugins/fancybox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    <style type="text/css">
        #dlgAddCard > input
        {
            margin-bottom: 15px;
            border-radius: 5px;
            height: 30px;
            width: 100%;
        }
        #dlgAddCard > select
        {
            margin-bottom: 15px;
            border-radius: 5px;
            height: 30px;
            width: 100%;
            min-width: 50px;
        }
        #txtAmount
        {
            color: Red;
        }
        #fancybox-wrap
        {
            padding: 0px;
        }
        #txtAccountName
        {
            font-size: 18px;
            font-weight: bold;
        }
        #txtBankAccount
        {
            font-size: 18px;
            font-weight: bold;
        }
        #ddlType
        {
            width: 98%;
            height: 30px;
            border-radius: 5px;
            margin-bottom: 5px;
        }
        #cardlist,#btnShowAddCard {
            display:none;
        }
    </style>
</head>
<body>
    <div>
    </div>
    <div class="bankcardlist">
        请选择到账方式:
        <select id="ddlType">
           <%-- <option value="0">银行卡</option>--%>
            <%if (isShowWeixin)
              {%>
            <option value="1">微信</option>
            <% } %>
           
            <option value="2">账户余额</option>
            
            
        </select>
        <div id="cardlist">
        </div>
        <a href="#dlgAddCard" class="addcardbtn" id="btnShowAddCard">添加银行卡</a>
    </div>
    <div class="widthdrawbox">
        <p class="note">
            您当前可提现的金额是<%=CurrentUserInfo.TotalAmount-CurrentUserInfo.FrozenAmount %>元</p>
        <input type="number" id="txtAmount" placeholder="请输入提现金额" pattern="\d*" class="cashnum"
            onkeyup="value=value.replace(/[^\d]/g,'')">
        <p class="inputnote">
            温馨提示：提现金额要大于<%=config.LowestAmount%>元</p>
        <div class="btn_main" id="btnWithdrawCash">
            申请提现
        </div>
        <div style="text-align: center; padding-top: 20px;">
            <a href="WithdrawRecord.aspx?ischannel=<%=Request["ischannel"] %>" style="color: #518dca; text-decoration: underline;">提现历史</a>
        </div>
    </div>
    <div style="display: none;">
        <div id="dlgAddCard" style="padding: 10px; margin-left: 20px;">
            <select id="ddlBankName">
                <option value="">请选择银行</option>
                <option value="招商银行">招商银行</option>
                <option value="工商银行">工商银行</option>
                <option value="中国农业银行">中国农业银行</option>
                <option value="北京银行">北京银行</option>
                <option value="中国银行">中国银行</option>
                <option value="交通银行">交通银行</option>
                <option value="上海银行">上海银行</option>
                <option value="中国建设银行">中国建设银行</option>
                <option value="中国光大银行">中国光大银行</option>
                <option value="兴业银行">兴业银行</option>
                <option value="中信银行">中信银行</option>
                <option value="中国民生银行">中国民生银行</option>
                <option value="广发银行">广发银行</option>
                <option value="华厦银行">华厦银行</option>
                <option value="南京银行">南京银行</option>
                <option value="平安银行">平安银行</option>
                <option value="中国邮政储蓄银行">中国邮政储蓄银行</option>
                <option value="浦发银行">浦发银行</option>
                <option value="天津银行">天津银行</option>
                <%--<option value="其它银行">其它银行</option>--%>
            </select>
            <br />
            <input type="text" id="txtAccountName" placeholder="开户名" />
            <br />
            <input id="txtBankAccount" type="text" placeholder="银行卡号" />
            <br />
            <label>
                开户行省份</label>
            <select id="ddlProvince">
            </select>
            <br />
            <label>
                开户行城市</label>
            <select id="ddlCity">
            </select><select id="ddlDistrict" style="display: none;"></select>
            <br />
            <input id="txtAccountBranchName" type="text" placeholder="开户网点" />
            <button type="button" onclick="$.fancybox.close();" style="float: left; width: 40%;" class="btn btn-default">
                取消</button>
            &nbsp;&nbsp;&nbsp;
            <button type="button" id="btnAddCard" style="float: right; width: 40%;" class="btn btn-default">
                确定</button>
        </div>
    </div>
    <div class="backbar">
        <a class="col-xs-2" href="javascript:history.go(-1);">
            <svg class="icon colorDDD" aria-hidden="true">
                <use xlink:href="#icon-fanhui"></use>
            </svg>

        </a>
        <div class="col-xs-8">
        </div>
    </div>
</body>
    <script src="js/quo.js" type="text/javascript"></script>
    <script src="js/comm.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Plugins/fancybox/jquery.fancybox-1.3.4.js" type="text/javascript"></script>
    <script src="js/jsAddress.js" type="text/javascript"></script>
<script type="text/javascript">
    var IsSubmit = false;
    $(function () {

        LoadMyBankCard();
        $("#btnWithdrawCash").click(function () {
            ApplyWithrawCash();

        })

        $("#btnShowAddCard").fancybox({
            'centerOnScroll': true

        });
        $("#btnAddCard").click(function () {
            AddBankCard();

        })
        addressInit('ddlProvince', 'ddlCity', 'ddlDistrict', "", "", "");

        $("#ddlType").change(function () {

            if ($(this).val() == "0") {
                $("#cardlist").show();
                $("#btnShowAddCard").show();
            }
            else if ($(this).val() == "1") {
                $("#cardlist").hide();
                $("#btnShowAddCard").hide();
            }

        })


    })

    //加载我的银行卡
    function LoadMyBankCard() {

        $.ajax({
            type: 'post',
            url: handerurl,
            data: { Action: 'GetMyBankCard',IsChannel:"<%=Request["ischannel"]%>" },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.length; i++) {

                    str.AppendFormat('<input type="radio"  class="chechcardradio" id="card{0}" name="chechcard" value="{0}">', resp.ExObj[i].AutoID);
                    str.AppendFormat('<label class="bankcard" for="card{0}">', resp.ExObj[i].AutoID);
                    str.AppendFormat('<i>{0}</i>', resp.ExObj[i].BankName);
                    str.AppendFormat('<span class="cardnum">{0}</span>', resp.ExObj[i].BankAccount);
                    str.AppendFormat('</label>');

                };
                if (str.ToString() == "") {
                    $("#cardlist").html("<div style=\"text-align:center;margin-bottom:10px;\">您还没有添加银行卡</div>");
                }
                else {
                    $("#cardlist").html(str.ToString());
                }

            },
            complete: function () { },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("加载超时，请刷新页面");

                }

            }
        });
    }

    //添加银行卡
    function AddBankCard() {

        var model = {
            Action: 'AddBankCard',
            AccountName: $.trim($("#txtAccountName").val()),
            BankAccount: $.trim($("#txtBankAccount").val()),
            BankName: $.trim($("#ddlBankName").val()),
            AccountBranchName: $.trim($("#txtAccountBranchName").val()),
            AccountBranchProvince: $.trim($("#ddlProvince").val()),
            AccountBranchCity: $.trim($("#ddlCity").val()),
            IsChannel: "<%=Request["ischannel"]%>"


        }
        if (model.BankName == "") {
            alert("请选择银行");
            return false;
        }
        if (model.AccountName == "") {
            $("#txtAccountName").focus();
            return false;
        }
        if (model.BankAccount == "") {
            $("#txtBankAccount").focus();
            return false;
        }

        if (model.AccountBranchProvince == "") {
            alert("请选择开户省份");
            return false;
        }
        if (model.AccountBranchCity == "") {
            alert("请选择开户城市");
            return false;
        }
        if (model.AccountBranchName == "") {
            $("#txtAccountBranchName").focus();
            return false;
        }
        if (IsSubmit) {
            return false;
        }
        if (confirm("确定要添加此银行卡，请在添加前仔细核对?")) {
            IsSubmit = true;
            $.ajax({
                type: 'post',
                url: handerurl,
                data: model,
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        alert("添加银行卡成功!");
                        $.fancybox.close();
                        LoadMyBankCard();

                    }
                    else {
                        alert(resp.Msg);
                    }

                },
                complete: function () { IsSubmit = false; },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("操作超时，请刷新页面");

                    }

                }
            });

        }


    }

    //申请提现
    function ApplyWithrawCash() {
        var model = {
            Action: 'ApplyWithrawCash',
            Amount: $.trim($("#txtAmount").val()),
            BankCardId: $("input[name='chechcard']:checked").val(),
            Type: $("#ddlType").val(),
            IsChannel:"<%=Request["ischannel"]%>"
        }

        if (model.Type == "0" && model.BankCardId == undefined) {
            alert("请选择银行卡");
            return false;
        }
        if (model.Amount == "") {
            $("#txtAmount").focus();
            return false;
        }
        if (model.Amount == "0") {
            alert("金额需大于0");
            return false;
        }
        if (parseFloat(model.Amount)><%=CurrentUserInfo.TotalAmount-CurrentUserInfo.FrozenAmount %>) {
            alert("余额不足");
            return false;
        }

        if (IsSubmit) {
            return false;
        }
        if (confirm("确认申请提现？")) {
            IsSubmit = true;
            $.ajax({
                type: 'post',
                url: handerurl,
                data: model,
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        alert("您的提现申请已成功提交");
                        window.location.href = "withdrawrecord.aspx?ischannel="+"<%=Request["ischannel"]%>";
                    }
                    else {
                        alert(resp.Msg);
                    }

                },
                complete: function () { IsSubmit = false; },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("操作超时");

                    }

                }
            });
        }
    }
</script>
<% = new ZentCloud.BLLJIMP.BLL().GetIcoScript() %>
</html>
