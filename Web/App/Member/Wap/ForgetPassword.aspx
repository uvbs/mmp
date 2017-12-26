<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Member.Wap.ForgetPassword" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <title>忘记密码</title>
    <link href="/lib/ionic/ionic.css" rel="stylesheet" type="text/css" />
    <link href="/App/Cation/Wap/Style/styles/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/Plugins/LayerM/need/layer.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <style>
        @charset "utf-8";
        @import url('<%= ico_css_file%>');
        body {
            font-size: 14px;
        }

        img {
            max-width: 100%;
        }

        .padding20 {
            padding: 20px;
        }

        .allRadius20 {
            border-radius: 20px;
        }

        .itemDiv {
            padding: 0px 20px 10px 20px;
        }

        .postDiv {
            padding: 0px 20px 20px 20px;
        }

        .item {
            border: 0px;
            border-bottom: 1px solid #44A5DF;
            padding: 5px;
            background-color: transparent !important;
        }

            .item input, textarea {
                padding-left:10px;
                padding-right:10px;
                height: 40px;
            }

            .item i {
                color: #2690d0;
                font-size: 22px;
            }

        input::-webkit-outer-spin-button,
        input::-webkit-inner-spin-button {
            -webkit-appearance: none;
        }

        input[type="number"] {
            -moz-appearance: textfield;
        }
        .code{
            flex: 1 100px !important;
        }
    </style>
</head>
<body>
    <div class="padding20">
    </div>
    <div class="itemDiv">
        <label class="item item-input">
            <i class="icon icon-shouji iconfont"></i>
            <input type="number" class="phone" placeholder="手机号码" value="" />
        </label>
        <label class="item item-input">
            <i class="icon icon-lock iconfont"></i>
            <input type="number" class="code" placeholder="验证码" />
            <button type="button" class="button button-small button-positive getSMSCode allRadius20" onclick="getSMSCode()">发送验证码</button>
        </label>
        <label class="lblPwd item item-input">
            <i class="icon icon-lock iconfont"></i>
            <input type="password" class="password" placeholder="新密码" value="" />
        </label>
        <label class="lblPwd item item-input">
            <i class="icon icon-lock iconfont"></i>
            <input type="password" class="confirmpassword" placeholder="确认密码" value="" />
        </label>
    </div>
    <div class="itemDiv">
        <button id="btnPost" type="button" class="button button-full button-positive allRadius20" onclick="postCode()">提交</button>
    </div>
    <div class="itemDiv">
        <a style="color:#0c63ee; float:left;"  href="Login.aspx?redirect=<%= HttpUtility.UrlEncode(Request["redirect"]) %>">返回登陆？</a>
        <a style="color:#0c63ee; float:right;"  href="Register.aspx?redirect=<%= HttpUtility.UrlEncode(Request["redirect"]) %>">去注册？</a>
    </div>
</body>
</html>
<script type="text/javascript">
    $(function () {
        window.alert = function (msg) {
            layer.open({
                content: msg,
                time: 2
            });
        };
    })
    var handerUrl = "/Serv/API/User/";
    var handerMemberUrl = "/Serv/API/Member/";
    var nlayerprogress;
    var redirect = '<%= Request["redirect"] %>';
    var timeNum = 0;
    var Inter;
    var auto_login = 1;
    function startInter() {
        Inter = setInterval(function () {
            timeNum--;
            if (!$(".getSMSCode").hasClass("button-stable")) {
                $(".getSMSCode").addClass("button-stable");
                $(".getSMSCode").removeClass("button-positive");
            }
            $(".getSMSCode").html("等待(" + timeNum + ")");
            if (timeNum <= 0) {
                $(".getSMSCode").addClass("button-positive");
                $(".getSMSCode").removeClass("button-stable");
                $(".getSMSCode").html("发送验证码");
                clearInterval(Inter);
            }
        }, 1000);
    }
    function getSMSCode() {
        if (timeNum > 0) return;
        var phone = $.trim($(".phone").val());
        if (phone == "") {
            alert("请输入手机号码");
            return;
        }
        timeNum = 90;
        startInter();
        nlayerprogress = layer.open({ type: 2, time: 90 });
        $.ajax({
            type: 'POST',
            url: handerMemberUrl + "GetSMSCode.ashx",
            data: { phone: phone ,check_user:1 ,limit_user: 1 },
            success: function (result) {
                layer.close(nlayerprogress);
                alert(result.msg);
                if (result.status === false) {
                    timeNum = 0;
                }
            },
            failure: function (result) {
                layer.close(nlayerprogress);
            }
        });
    }
    function postCode() {
        var phone = $.trim($(".phone").val());
        if (phone == "") {
            alert("请输入手机号码");
            return;
        }
        var code = $.trim($(".code").val());
        if (code == "") {
            alert("请输入手机验证码");
            return;
        }
        var password = $.trim($(".password").val());
        if (password == "" || password.length<8) {
            alert("请输入8位以上新密码");
            return;
        }
        var confirmpassword = $.trim($(".confirmpassword").val());
        if (confirmpassword == "" || confirmpassword.length < 8) {
            alert("请输入8位以上确认密码");
            return;
        }
        if (confirmpassword != password) {
            alert("两次密码不一致");
            return;
        }
        nlayerprogress = layer.open({ type: 2, time: 90 });
        $.ajax({
            type: 'POST',
            url: handerUrl + "SMSBackPwd.ashx",
            data: { phone: phone, code: code, new_pwd: password, confirm_pwd: confirmpassword, auto_login: auto_login },
            success: function (result) {
                layer.close(nlayerprogress);
                alert(result.errmsg);
                if (result.isSuccess && auto_login == 1) {

                    setTimeout(function () {
                        GotoNewPage();
                    }, 2000);
                }
            },
            failure: function (result) {
                layer.close(nlayerprogress);
            }
        });
    }
    function GotoNewPage() {
        if (redirect != "") {
            document.location.href = redirect;
        }
        else {
            document.location.href = "/customize/comeoncloud/Index.aspx?key=PersonalCenter";
        }
    }
</script>
