<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Member.Wap.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <title>登录</title>
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
            <input type="password" class="password" placeholder="密码" value="" />
        </label>
        <label class="item item-input">
            <i class="icon icon-lock iconfont"></i>
            <input type="text" class="code" placeholder="验证码" />
            <img class="checkCode" onclick="getVerCode()" />
        </label>
    </div>
    <div class="itemDiv">
        <button type="button" class="button button-full button-positive allRadius20" onclick="postCode()">提交</button>
    </div>
    <div class="itemDiv">
        <a style="color:#0c63ee; float:left;"  href="ForgetPassword.aspx?redirect=<%= HttpUtility.UrlEncode(Request["redirect"]) %>">忘记密码？</a>
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
        getVerCode();
    })
    var handerUrl = "/Serv/API/User/";
    var nlayerprogress;
    var redirect = '<%= Request["redirect"] %>';
    function getVerCode() {
        $(".checkCode").attr("src", "/ValidateCode.aspx?rd=" + Math.random());
    }
    function postCode() {
        var phone = $.trim($(".phone").val());
        if (phone == "") {
            alert("请输入手机号码");
            return;
        }
        var password = $.trim($(".password").val());
        if (password == "") {
            alert("请输入密码");
            return;
        }
        var code = $.trim($(".code").val());
        if (code == "") {
            alert("请输入手机验证码");
            return;
        }
        nlayerprogress = layer.open({ type: 2, time: 90 });
        $.ajax({
            type: 'POST',
            url: handerUrl + "LoginCode.ashx",
            data: { username: phone, password: password, checkcode: code },
            success: function (result) {
                layer.close(nlayerprogress);
                alert(result.msg);
                if (result.status) {
                    setTimeout(function () {
                        GotoNewPage();
                    }, 2000);
                }
                else {
                    alert(result.msg);
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
