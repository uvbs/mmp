<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.Login" %>



<!DOCTYPE html>
<html lang="zh-CN">
	<head>
		<meta charset="UTF-8">
		<meta http-equiv="Content-Type" content="text/html;charset=UTF-8">
		<meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0;">
		<title>登录</title>
		<link href="../Css/wanbang.css" rel="stylesheet" type="text/css" />
        <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
                <style type="text/css">
        .logingroup input {width:70%;}
        .login-from {
            padding: 10px 15px 0;
            }
        </style>
	</head>
	<body>
       <!-- Top -->
        <header class="head">
        	<a id="logo" href="Index.aspx"><img src="../Images/logo.png"></a>
            
        </header>
		<!--/ Top -->
        <!-- Login -->
        <div class="login-from">
            <form method="post" action="#">
                <p class="option">
                <input class="radiobox" type="radio" id="prisegroup" name="group1" checked="checked" value="1">
                <label for="prisegroup" style="margin-right:16px;">
                <span class="wbtn"><span class="iconfont"></span></span>爱心企业</label>
                <input class="radiobox" type="radio" id="basegroup" name="group1" value="0"><label for="basegroup"><span class="wbtn"><span class="iconfont"></span></span>阳光基地</label></p>
                <p class="logingroup username">
                    <label for="txtUserName"><span class="iconfont icon-yonghu"></span></label>
                    <input id="txtUserName" type="text" value=""/>
                </p>
                <p class="logingroup password">
                    <label for="txtPwd"><span class="iconfont icon-suo"></span></label>
                   <input id="txtPwd" type="password" value=""/>
                </p>
               
                <button class="loging" type="button" id="btnLogin">立即进入</button>
            </form>
        <div class="apply"><a class="quick" href="/325dd/details.chtml">快速申请</a><a class="member" href="/325df/details.chtml">忘记密码？</a>  
        </div>
        <!--<p class="copyright">技术支持：<br>上海至云信息可以有限公司，点击到<a href="#">公司微网</a></p>-->
        <!--/ Login -->
        </div>
	</body>
     <script type="text/javascript">
         $(function () {

             $("#btnLogin").click(function () {
                 var userName = $.trim($("#txtUserName").val());
                 var pwd = $.trim($("#txtPwd").val());
                 if (userName == "") {
                     $("#txtUserName").focus();
                     return false;
                 }
                 if (pwd == "") {
                     $("#txtPwd").focus();
                     return false;

                 }

                 login(userName, pwd);

             });

             $(".logingroup.username").click(function () {
                 $("#txtUserName").focus();
             });
             $(".logingroup.password").click(function () {
                 $("#txtPwd").focus();
             });


         });
         function login(userName, pwd) {
             $.ajax({
                 type: 'post',
                 url: "/Handler/WanBang/Wap.ashx",
                 data: { Action: 'Login', UserName: userName, PassWord: pwd, UserType: $("input[name='group1']:checked").val() },
                 dataType: "json",
                 success: function (resp) {
                     if (resp.Status == 1) {

                         var redirecturl = "<%=redirecturl%>";

                         if (redirecturl != "") {
                             window.location = redirecturl;
                         } else {

                             if ($("input[name='group1']:checked").val() == "0") {
                                 window.location.href = "BaseCenter.aspx";

                             }
                             else {
                                 window.location.href = "CompanyCenter.aspx";
                             }
                         }


                     }
                     else {
                         alert(resp.Msg);
                     }


                 }
             });


         }

    </script>
</html>