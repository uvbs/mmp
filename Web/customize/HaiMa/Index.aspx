<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>首页</title>
    <link rel="stylesheet" href="css/normalize.css">
    <link rel="stylesheet" href="css/allPage.css">
    <link rel="stylesheet" href="css/index.css">
    <style>
        .page1 .index02 img
        {
            height: auto;
        }
        .rule
        {
            color: White;
            text-decoration:underline;
            font-size:20px;
            font-weight:bold;
        }
        .sign_btn
        {
            z-index: 10;
        }
        .index03_01 p{margin-right:5px;padding-right:10px;}
    </style>
</head>
<body>
    <div class="container">
        <div class="container-inner">
            <div class="page page0">
                <div class="index01">
                    <img src="images/index01.png" /></div>
                <a class="sign_btn" id="btnSignUp"><b></b></a><span class="start"><b></b></span>
            </div>
            <div class="page page1">
                <div class="index02">
                    <img src="images/index02_01.png" /></div>
                <div class="index03">
                </div>
                <div class="index03_03">
                    <img src="images/index02_02.png" /></div>
                <div class="index03_01">
                    <p>
                        1.竞赛时间：2015年7月-2015年12月；
                    </p>
                    <p>
                        2.参与对象：所有从事海马汽车郑州基地产品销售的销售服务店、销售经理/市场经理、销售顾问；</p>
                    <p>
                        3.竞赛由初赛、复赛、决赛三个阶段组成；</p>
                    <p>
                        4.参赛规则详见 <a class="rule" href="http://haima.comeoncloud.net/6353a/details.chtml">参赛手册</a>。</p>
                    <div class="index03_02">
                        <img src="images/code.png" />
                        <p>
                            长按二维码<br />
                            关注微信公众号
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
<script type="text/javascript">
    var canSignUp = "<%=CanSignUp%>";
</script>
<script src="LayerM/layer.m.js" type="text/javascript"></script>
<script src="js/comm.js" type="text/javascript"></script>
<script src="js/zepto.min.js"></script>
<script src="js/fullPage.js"></script>
<script src="js/index.js"></script>
<script src="js/jquery-1.10.1.min.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "首页",
            desc: "海马精英成长平台",
            //link: '', 
            imgUrl: "http://<%=Request.Url.Host%>/customize/HaiMa/images/logo.jpg"
        })
    })
</script>
</html>
