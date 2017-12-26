<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="AdminLogin.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.adminlogin" %>
<!DOCTYPE html>
<html>
<head>
    <title>天下华商月供宝平台</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" >
    <link href="/css/login/loginv1.css" rel="stylesheet" type="text/css" />
    <style>
        #nav
        {
           background: rgba(221, 221, 221, 0) !important;
           box-shadow: none !important;
        }
        #logo
        {
                background-image: url(http://songhe.comeoncloud.com.cn/FileUpload/image/songhe/songhe/20170104/211u8.png);
                height: 58px;
                width: 256px;
                background-position:0;
        }
        .loginform
        {
            position:relative;    
        }
        .loginbtn
        {
            margin-top: 35px !important;
        }
        img.songhelogo 
        {
            width: 232px !important;
            height: 64px !important;
            position: absolute !important;
            left: 42px !important;
        }
    </style>
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
    <!-- <a href="javascript:;" id="logo"></a> !-->
   
</div>
<div id="QRcodebox">
    <div id="QRcode">
        <div class="loginbox">
              <form  action="" class="loginform " >
                <img class="songhelogo" src="http://songhe.comeoncloud.com.cn/FileUpload/image/songhe/songhe/20170104/211u8.png" alt="天下华商月供宝平台" />
                <div style="height: 60px;"></div>
                <span class="tip">密码或账号错误</span>
                <input class="acconttext" id="account" type="text" autofocus="autofocus" placeholder="请输入用户名" />
                <label for="" class="account"></label>
                <input id="password" type="password" placeholder="请输入密码" />
                <label for="" class="password"></label>
                <div class="btnbox">
                <button class="loginbtn" type="button" id="btnLogin"></button></div>
            </form>
           
        </div>
    </div>
</div>
<div class="leftbg"></div>
<div class="rightbg"></div>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
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




