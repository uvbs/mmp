<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>登录-</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var redirecturl = "<%=redirecturl%>";
        $(function () {

            $(".checktag").each(function (index) {
                $(this).click(function () {
                    if (index == 0) {
                        $(".checked").removeClass("checked");
                        $(".logintag").addClass("checked");
                        $(".showbox").removeClass("showbox");
                        $(".login").addClass("showbox");
                    } else {
                        $(".checked").removeClass("checked");
                        $(".signintag").addClass("checked");
                        $(".showbox").removeClass("showbox");
                        $(".signin").addClass("showbox");
                    }
                })
            })

            //登录
            $("#btnLogin").click(function () {
                var userID = $("#txtUserId").val();
                var pwd = $("#txtPwd").val();
                if (userID == "") {
                    $("#txtUserId").focus();
                    return false;
                }
                if (pwd == "") {
                    $("#txtPwd").focus();
                    return false;

                }
                $("#btnLogin").html("正在登录...");
                $.ajax({
                    type: 'post',
                    url: "/Handler/OpenGuestHandler.ashx",
                    data: { Action: 'Login', userID: userID, pwd: pwd },
                    dataType: "json",
                    success: function (resp) {
                        $("#btnLogin").html("登录");
                        if (resp.Status == 1) {

                            if (redirecturl != "") {
                                window.location = redirecturl;
                            }
                            else {
                                alert("登录成功");
                            }

                        }
                        else {
                            alert(resp.Msg);
                        }


                    }
                });

            });
            //注册
            $("#btnReg").click(function () {
                var userID = $.trim($("#txtRegUserId").val());
                var pwd = $.trim($("#txtRegPwd").val());
                var pwdconfirm = $.trim($("#txtRegPwdConfirm").val());
                var vercode = $.trim($("#txtRegVerCode").val());
                if (userID == "") {
                    $("#txtRegUserId").focus();
                    return false;
                }
                if (pwd == "") {
                    $("#txtRegPwd").focus();
                    return false;

                }
                if (pwdconfirm == "") {
                    $("#txtRegPwdConfirm").focus();
                    return false;

                }
                if (pwd != pwdconfirm) {
                    alert("密码不一致");
                    $("#txtRegPwdConfirm").focus();
                    return false;
                }
                if (vercode == "") {
                    $("#txtRegVerCode").focus();
                    return false;
                }
                $(this).html("正在注册...");
                $.ajax({
                    type: 'post',
                    url: "/Handler/OpenGuestHandler.ashx",
                    data: { Action: 'RegUser', userID: userID, pwd: pwd, VerCode: vercode },
                    dataType: "json",
                    success: function (resp) {
                        $("#btnReg").html("注册");
                       
                        if (resp.Status == 1) {

                            if (redirecturl != "") {

                                window.location.href = redirecturl;
                            }
                            else {
                                alert("注册成功");
                            }

                        }
                        else {
                            alert(resp.Msg);
                        }


                    }
                });

            });

            document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
                WeixinJSBridge.call('hideToolbar');
            });


        });

        //获取get参数
        function GetParm(parm) {
            //获取当前URL
            var local_url = window.location.href;

            //获取要取得的get参数位置
            var get = local_url.indexOf(parm + "=");
            if (get == -1) {
                return "";
            }
            //截取字符串
            var get_par = local_url.slice(parm.length + get + 1);
            //判断截取后的字符串是否还有其他get参数
            var nextPar = get_par.indexOf("&");
            if (nextPar != -1) {
                get_par = get_par.slice(0, nextPar);
            }
            return get_par;
        }

    </script>
</head>


<body>
<section class="box">
    <div class="logintagbox">
        <span class="logintag checktag checked">登录</span>
        <span class="signintag checktag">注册</span>
    </div>
    <div class="loginbox">
        <div class="login showbox">
            <input type="text" id="txtUserId" placeholder="账号">
            <input type="password" id="txtPwd" placeholder="请输入密码">
            <button type="button" id="btnLogin">登录</button>           
        </div>
        <div class="signin">
            <input type="text" id="txtRegUserId" placeholder="输入您要注册的账号">
            <input type="password" id="txtRegPwd" placeholder="请输入密码">
            <input type="password" id="txtRegPwdConfirm" placeholder="请确认密码">
            
            <img id="imgVerify" src="/ValidateCode.aspx?"  onclick="this.src=this.src+'?'"
                                            style="width: 65px; height: 30px;" />
                                            看不清?点击图片换一张
            <input type="text" id="txtRegVerCode" placeholder="输入图片验证码">
            <button type="button" id="btnReg">注册</button>           
        </div>
    </div>
    <div class="loginbg"></div>
    <div class="backbar">
        <a href="javascript:history.go(-1);" class="back"><span class="icon"></span></a>

    </div>
</section>
</body>

</html>



