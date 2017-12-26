<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Danmark.Index" %>

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <title>丹麦2015摄记你的旅程</title>
    <link href="css/ionic.css" rel="stylesheet" type="text/css" />
    <link href="css/m.css" rel="stylesheet" type="text/css" />
    <link href="css/index.css" rel="stylesheet" type="text/css" />
    <style>
    .wrapDanMarkIndex .middleDiv .testPic {

  z-index: 40;
}
    
    </style>
</head>
<body>
    <div class="wrapDanMarkIndex">
        <div class="top">
            <div class="topImgDiv">
                <img class="topImg" src="images/top.jpg" />
                <div class="phote">
                    <img class="photeImg" src="images/photo_03.png">
                </div>
                <div class="balloon">
                    <img class="balloonImg" src="images/photo_05.png">
                </div>
            </div>
        </div>
        <div class="middleDiv">
            <div class="testPic">
                <img class="testImg" src="images/testPic.png" />
            </div>
            <div class="wordPic">
                <img class="wordImg" src="images/wordPic.png" />
            </div>
            <div class="beginBtn">
                <button id="beginMyTrip" class="button button-block button-positive" onclick="window.location.href='Question.aspx'">
                    开始我的旅程
                </button>
            </div>
        </div>
        <div class="car">
        </div>
        <div class="people">
        </div>
        <div class="bottom">
            <div class="bottomImgDiv">
                <img class="bottomImg" src="images/bottom.jpg" />
                <div class="car">
                    <img class="carImg" src="images/car.png">
                </div>
                <div class="people">
                    <img class="peopleImg" src="images/people.png">
                </div>
            </div>
        </div>
    </div>
</body>
   <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "测一测，您是丹麦童话里的哪个角色",
                desc: "测一测，您是丹麦童话里的哪个角色",
                link: 'http://huiji.comeoncloud.net/customize/Danmark/Index.aspx',
                imgUrl: "http://<%=Request.Url.Host%>/customize/Danmark/images/logo.jpg" 
            })
        })
    </script>
</html>
