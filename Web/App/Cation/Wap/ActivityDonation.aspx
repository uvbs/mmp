<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityDonation.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.ActivityDonation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>转赠</title>
    <link href="/Weixin/ArticleTemplate/css/comm.css" rel="stylesheet" />
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.1"/>
    <link href="/css/buttons2.css" rel="stylesheet" type="text/css" />
    <link href="/Plugins/LayerM/need/layer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
         a:link, a:visited
        {
            color: White;
        }
    </style>
</head>
<body class="whitebg">
    <div class="wcontainer activetitle">
        <h1>
            <%=JuactivityInfo.ActivityName %>
        </h1>
    </div>
    <div class="mainlist activelist activeinfomainlist" onclick="window.location.href='/<%=JuactivityInfo.JuActivityIDHex %>/detail.chtml'">
        <div class="listbox">
            <div class="mainimg">
                <img src="<%=JuactivityInfo.ThumbnailsPath %>">
            </div>
            <%--            <span class="baomingstatus"><span class="text">进行中 </span>
                <svg class="sanjiao" version="1.1" viewbox="0 0 100 100"><polygon points="100,100 0.2,100 100,0.2" /></svg>
            </span>--%>
            <div class="activeconcent">
                <div class="textbox">
                    <p>
                        <span class="iconfont icon-clock"></span><span class="text">开始时间:<%=((DateTime)JuactivityInfo.ActivityStartDate).ToString("yyyy年MM月dd日 HH:mm") %>
                        </span>
                    </p>
                    <p>
                        <span class="iconfont icon-adress"></span><span class="text"><%=JuactivityInfo.ActivityAddress %></span>
                    </p>
                </div>
            </div>
        </div>
        <!-- listbox -->
    </div>
    <!-- mainlist -->
    <div class="wcontainer articlebox">
        <a id="btnReceive" href="#" class="button button-block button-rounded button-action button-large">
            接收转赠</a>
    </div>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {

        $("#btnReceive").click(function () {
            $.ajax({
                type: 'post',
                url: '/Handler/Activity/ActivityHandler.ashx',
                data: { Action: "ReceiveActivity",ActivityId:<%=Request["ActivityId"] %>, FromUserAutoId:<%=Request["FromUserAutoId"] %>},
                dataType:'json',
                success: function (resp) {
                if (resp.Status==1) {
                    alert("接收成功");
                    window.location.href='/App/Cation/Wap/MyActivityLlists.aspx';
                }
                else {
                    alert(resp.Msg);
                    }
                }
            });

        
        
                
        
        
        })



    })
            window.alert = function (msg) {
            layer.open({
                content: msg,
                btn: ['OK']
            });
        
        }
</script>
</html>
