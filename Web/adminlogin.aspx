<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="AdminLogin.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.adminlogin" %>
<!DOCTYPE html>
<html>
<head>
    <title>移动营销管理平台</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" >
    <link href="/css/login/loginv1.css?v=1.0.0.1" rel="stylesheet" type="text/css" />
  
</head>
<body>
<%if (Request.Url.Host=="m.mixblu.com"||Request.Url.Host=="www.mixblu.com")
  {
      Response.Redirect("http://m.mixblu.com/customize/mixblu/?v=0");
  } %>
  <%if (Request.Url.Host=="www.aussieorigin.cn"||Request.Url.Host=="www.aussieorigin.com.cn")
  {
      Response.Redirect("http://www.aussieorigin.cn/customize/tuao/");
  } %>
<div id="nav">
    <a href="javascript:;" id="logo"></a>
    <ul>
       <%-- <li><a href="http://wx.comeoncloud.net/">微平台</a></li>--%>
        <li><a href="http://cloudnewthinking.comeoncloud.net/">移动营销平台</a></li>
        <li><a href="javascript:;">聚比特俱乐部</a></li>
        <li><a href="javascript:;">帮助中心</a></li>
    </ul>
    <a class="talktoqq" target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=2262561728&site=qq&menu=yes"></a>
</div>
<div id="QRcodebox">
    <div id="QRcode">
        <div class="loginbox">
              <form  action="" class="loginform " >
                <span class="logo"></span>
                <h2></h2>
                <span class="tip">密码或账号错误</span>
                <input class="acconttext" id="account" type="text" autofocus="autofocus" placeholder="请输入用户名" />
                <label for="" class="account"></label>
                <input id="password" type="password" placeholder="请输入密码" />
                <label for="" class="password"></label>
                <a href="#" class="forget">忘记密码？</a>
                <div class="btnbox"><a target="_blank" href="http://comeoncloud.comeoncloud.net/7c383/details.chtml" class="siginbtn"></a>
                <button class="loginbtn" type="button" id="btnLogin"></button></div>
                <a href="tel:021-61729583" class="ads">联系我们&nbsp;&nbsp;021-60960775</a>
            </form>
            <div class="zhiyuncode"></div>
        </div>
    </div>
</div>
<div class="leftbg"></div>
<div class="rightbg"></div>
</body>
<script src="http://static-files.socialcrmyun.com/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    localStorage.setItem('tabpanelItems','');
    var $document = $(document);
    function codeboxsize() {
        if ($(window).height() < 700) {
            $("#QRcodebox").css({ "height": $(window).height() - 60, "top": "-50px" });
        } else {
            $("#QRcodebox").css({ "height": $(window).height() - 60, "top": "-90px" });
        }
    }
    $(function () {
        codeboxsize();
        $(window).resize(function () {
            codeboxsize()
        })
        $document.on('click', '#btnLogin', function () {
            login();
        });
        $document.keyup(function (e) {
            if (e.keyCode == 13) {
                login();
            }
        });
    })

    function shock() {
        var right = 170;
        for (i = 1; i < 7; i++) {
            $('.talktoqq').animate({ 'right': '-=2' + 'px' }, 3, function () {
                $(this).animate({ 'right': '+=4' + 'px' }, 3, function () {
                    $(this).animate({ 'right': '-=4' + 'px' }, 3, function () {
                        $(this).animate({ 'right': '+=4' + 'px' }, 3, function () {
                            $(this).animate({ 'right': '-=2' + 'px' }, 3, function () {
                                $(this).animate({ 'right': right + 'px' }, 3, function () {
                                    $(this).stop();
                                });
                            });
                        });
                    });
                });
            });
        }
        qqtalkmove = setTimeout(function () {
            shock();
        }, 10000)
    }

    function login() {
        $.ajax({
            type: 'post',
            url: "/Handler/OpenGuestHandler.ashx",
            dataType: "json",
            data: { Action: 'AdminLogin', username: $("#account").val(), pwd: $("#password").val() },
            success: function (resp) {
                if (resp.Status == 1) {
                    window.location = "/index";
                }
                else {
                    alert(resp.Msg);
                }
            }
        });
    }

    var qqtalkmove = setTimeout(function () {
        shock();
    }, 3000)
    $(".zhiyuncode").mouseenter(function () {
        $(this).stop().animate({ "right": -135 }, 300);
    })
    $(".zhiyuncode").mouseout(function () {
        $(this).stop().animate({ "right": -40 }, 300);
    })
    $(".ads").mouseenter(function () {
        $(".zhiyuncode").stop().animate({ "right": -135 }, 300);
    })
    $(".ads").mouseout(function () {
        $(".zhiyuncode").stop().animate({ "right": -40 }, 300);
    })

    
</script>
</html>




