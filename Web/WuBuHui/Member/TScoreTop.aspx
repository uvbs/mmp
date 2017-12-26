﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TScoreTop.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Member.TScoreTop" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>五步会排行榜</title>
    <!-- Bootstrap -->
    <link href="../css/wubu.css" rel="stylesheet" type="text/css" />
          <script>              var IsHaveUnReadMessage = "<%=IsHaveUnReadMessage%>"; </script>
</head>
<body class="whitebg">
    <div class="mainlist">
        <div class="tagbar">
            <a href="ScoreTop.aspx" class="wbtn" id="newslistbtn">
                <!-- <span class="iconfont icon-78"></span> -->
                <span class="title">用户排行(现有积分)</span> </a><a href="" class="wbtn wbtn_greenyellow" id="discusslistbtn">
                    <!-- <span class="iconfont icon-78"></span> -->
                    <span class="title">导师排行(现有积分)</span> </a>
        </div>
    </div>
    <div class="scoretoplist">
        <%
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "No-Cache");
            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < UserList.Count; i++)
            {
                ZentCloud.BLLJIMP.Model.TutorInfo tuInfo = new ZentCloud.BLLJIMP.BLL("").Get<ZentCloud.BLLJIMP.Model.TutorInfo>(string.Format("UserId='{0}'", UserList[i].UserID));
                sb.AppendLine(string.Format("<div class=\"listbox\" onclick=\"window.location.href='../Tutor/WxTutorInfo.aspx?UserId={0}'\">", tuInfo.AutoId));
                sb.AppendLine(string.Format("<pan class=\"listnum\">{0}</pan>", (i + 1).ToString()));
                sb.AppendLine("<span class=\"wbtn_round touxiang\">");
                sb.AppendLine(string.Format("<img src=\"{0}\" >", tuInfo.TutorImg));
                sb.AppendLine("</span>");
                sb.AppendLine(string.Format("<span class=\"name\">{0}</span>", tuInfo.TutorName));
                sb.AppendLine(string.Format("<span class=\"score\">{0}</span>", UserList[i].TotalScore));
                sb.AppendLine("</div>");

            }
            Response.Write(sb.ToString());
        
        %>

    <!-- mainlist -->
       <script type="text/javascript" src="../js/footer.js"></script>
    <!-- footerbar -->
</body>
<script type="text/javascript">
    //var appId = "wx082a38235bae238f";
    var desc = "五步会";
    var titleshare = "五步会排行榜";
    var imgUrl = "http://" + window.location.host + "/WuBuHui/img/logo.png";
    var shareUrl = window.location.href;
    // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。
    document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
        // 发送给好友
        WeixinJSBridge.on('menu:share:appmessage', function (argv) {
            WeixinJSBridge.invoke('sendAppMessage', {
                //"appid": appId,
                "img_url": imgUrl,
                "img_width": "100",
                "img_height": "100",
                "link": shareUrl,
                "desc": desc,
                "title": titleshare
            }, function (res) {

            })
        });
        WeixinJSBridge.on('menu:share:timeline', function (argv) {
            WeixinJSBridge.invoke('shareTimeline', {
                //"appid": appId,
                "img_url": imgUrl,
                "img_width": "100",
                "img_height": "100",
                "link": shareUrl,
                "desc": desc,
                "title": titleshare
            }, function (res) {

            })
        });
    })
</script>
</html>
