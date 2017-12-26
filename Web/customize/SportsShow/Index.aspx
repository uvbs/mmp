<%@ Page Language="C#" AutoEventWireup="true" EnableSessionState="ReadOnly" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SportsShow.Index" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0">
    <title>首页</title>
    <link href="/lib/ionic/ionic.css" rel="stylesheet">
    <link href="css/base.css" rel="stylesheet">
    <link href="css/index.css" rel="stylesheet">
    <script src="/lib/jquery/jquery-2.1.1.min.js"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script src="js/SportsShow.js"></script>
</head>
<body class="bg">
    <header class="content">
        <div class="row">
            <div class="col col-60 col-offset-4">
                <img src="images/index/logo.png" class="full-image">
            </div>
        </div>
        <div class="row">
            <div class="col offset-headerbg">
                <img src="images/index/header-bg.png" class="full-image header-bg">
            </div>
        </div>
    </header>
    <section class="content">
        <div class="row">
            <div class="col-50 col-offset-50">
                <img src="images/index/font-1.png" class="full-image">
            </div>
        </div>
        <div class="row offset-font-2">
            <div class="col-60">
                <img src="images/index/font-2.png" class="full-image">
            </div>
        </div>
        <div class="row offset-btn">
            <div class="col-67 col-offset-35 content fix-width-67">
                <img src="images/index/btn-bg.png" class="full-image">
                <a href="javascript:void(0)" class="btn" onclick="postData()">
                    <% if (DateTime.Now < Convert.ToDateTime("2015-11-09"))
                       { %>
                    测一测
                    <%}
                       else
                       {%>
                    已结束
                    <%}%>
                </a>
            </div>
        </div>
        <div class="offset-bike">
            <img src="images/index/bike.png" class="full-image">
        </div>
        <div class="row">
            <div class="col-67 col-offset-25">
                <img src="images/index/football.png" class="full-image offset-football">
            </div>
        </div>
    </section>
    <section class="content offset-section">
        <div class="row">
            <div class="col-90 col-offset-5">
                <strong>主办单位：</strong>

                <p>上海市体育总会</p>

                <p>上海东浩兰生国际服务贸易（集团）有限公司</p>
                <strong>支持单位：</strong>

                <p>中国体育用品业联合会</p>

                <p>上海市体育局</p>
                <strong>承办单位：</strong>

                <p>上海市东浩会展活动策划有限公司</p>
            </div>
            <img src="images/index/glass.png" class="ico-glass">
        </div>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-50 fix-width-50 col-offset-25">
                <img src="images/index/2dcode.png" class="full-image">
                <p class="font-code">长按识别图中二维码</p>
            </div>
            <img src="images/index/seastar.png" class="ico-seastar">
        </div>
    </section>
    <section class="content text-center">
        <strong>http://www.shanghaisportshow.com/</strong>
        <br>
        <strong>2015.11.5-11.8 上海世博展览馆</strong>
    </section>
    <form id="form1" runat="server">
        <div style="display: none">
            <asp:Button ID="btnPost" runat="server" Text="提交" OnClick="btnPost_Click" />
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "你的体商有多少？测一测，赢取免费好礼！",
            desc: "这里有马拉松，也有lol等电竞大赛，2015上海体博会，更多精彩，等你来战！",
            link: "http://<%=Request.Url.Host%>/customize/SportsShow/Index.aspx",
            imgUrl: "http://<%=Request.Url.Host%>/customize/SportsShow/images/share.png"
        })
    })
    $(function () {
        window.name = "Index";
    })
    function postData() {
        $("#btnPost").click();
    }
</script>
