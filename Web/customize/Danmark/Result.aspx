<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Danmark.Result" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <title>丹麦2015摄记你的旅程</title>
    <link href="css/ionic.css" rel="stylesheet" type="text/css" />
    <link href="css/m.css" rel="stylesheet" type="text/css" />
    <link href="css/index.css" rel="stylesheet" type="text/css" />
    <link href="css/result.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            font-family: 'Microsoft YaHei';
        }
        .wrapDanMarkResult .middleDiv .typeDiv
        {
            z-index: 5;
        }
    </style>
</head>
<body>
    <div class="wrapDanMarkIndex wrapDanMarkResult">
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
            <div class="typeDiv" id="divresult">
            </div>
            <div class="resultDiv">
                <div class="row">
                    <div class="col">
                        <img class="girlImg" src="" id="imgresult" />
                    </div>
                    <div class="col" id="divresultdesc">
                    </div>
                </div>
            </div>
            <div class="shareDiv">
                <img class="beautyImg" src="images/result_03.png" />
            </div>
            <div class="registDiv">
                <button id="resultSubmit" class="button button-block button-positive" onclick="window.location.href='SignUp.aspx'">
                    立即报名
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
<script type="text/javascript">

    $(function () {

        var type = getQueryString("answer");
        switch (type) {
            case "A":
                $(divresult).html("卖火柴的小女孩");
                $(imgresult).attr("src", "images/answera.png");
                $(divresultdesc).html("也许你是一个不起眼的人，但你对未来充满希望，幻想用自己内心小小的光给世界带来温暖，通过自己感染身边的亲友达到更好。");
                break;
            case "B":
                $(divresult).html("丑小鸭");
                $(imgresult).attr("src", "images/answerb.png");
                $(divresultdesc).html("理想是你人生路上一直追求的目标，你会为之努力奋斗，相信自己，是金子总会发光的。");
                break;
            case "C":
                $(divresult).html("小锡兵");
                $(imgresult).attr("src", "images/answerc.png");
                $(divresultdesc).html("也许你只是一个简单的凡人，却意志坚定，勇于面对困难，迎接挑战。相信不久的未来，你会成为理想中的自己。");
                break;
            case "D":
                $(divresult).html("皇帝的新衣");
                $(imgresult).attr("src", "images/answerd.png");
                $(divresultdesc).html("你有较强的控制欲，时常会陷入对生活过分的热衷追求，即使受到他人质疑，也依然坚持做好自己，让他们说去吧。");
                break;
            default:

        }



    })

    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    } 

</script>
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
