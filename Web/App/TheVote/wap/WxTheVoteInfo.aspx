<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxTheVoteInfo.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.TheVote.wap.WxTheVoteInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=tvInfo.VoteName%></title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link rel="stylesheet" href="styles/css/style.css?v=0.0.1">
    <link href="//static-files.socialcrmyun.com/lib/layer.mobile/need/layer.css" rel="stylesheet" />

    <style type="text/css">
        .mainlist li div {
            display: block;
            width: 100%;
            box-sizing: border-box;
            padding: 10px;
            background-color: #fff;
            border-radius: 4px;
            box-shadow: 0 0 6px rgba(0,0,0,0.2);
        }

        img {
            width: 100%;
        }

        button {
            width: 100%;
        }

        .toupiaobox {
            margin-top: 0 !important;
            padding-top: 0 !important;
        }

        .title {
            margin-top: 10px;
        }
        .box {
            width:100% !important;
        }
    </style>
</head>
<body>
    <section class="box">
    <ul class="mainlist articlelist currentlist" id="needList" runat="server">
    </ul>
</section>
</body>
    
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>

    <script src="//static-files.socialcrmyun.com/lib/layer.mobile/layer.m.js" type="text/javascript" ></script>
    <script src="/Scripts/Common.js" type="text/javascript"></script>
    <script src="/Weixin/ArticleTemplate/JS/TheVote.js" type="text/javascript"></script>
    <script src="/Scripts/gzptcommon.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js" type="text/javascript"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "<%=tvInfo.VoteName%>",
            desc: "<%=tvInfo.Summary%>",
            //link: "",
            imgUrl: "<%=tvInfo.ThumbnailsPath%>"
        })
    });
</script>
</html>


