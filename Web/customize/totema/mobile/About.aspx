<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.mobile.About" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>关于我们</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/green.css" />
    <%--    <link type="text/css" rel="stylesheet" href="css/idangerous.swiper.css" />
    <link type="text/css" rel="stylesheet" href="css/swipebox.css" />--%>
    <style type="text/css">
        .menu5 ul li
        {
            width: 33.33%;
        }
        .about_container1{margin-bottom:100px;}
    </style>
</head>
<body>
    <!--Page 1 content-->
    <div class="swiper-slide sliderbg">
        <div class="swiper-container swiper-nested">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="slide-inner">
                        <div class="pages_container">
                            <div class="about_container">
                                <div class="image_single">
                                    <img src="images/about_01.png" alt="" title="" border="0" /></div>
                            </div>
                            <div class="about_container1">
                                <p>
                                    秉承Inspiration with Integirety (创新和诚信) 的信念，凭借匹敌任何品牌服饰的品质以及独特的 校服服务理念和方式，TOTEMA如今自豪地拥有百
                                    所顶尖国际学校忠实客户，百万优秀国际学生穿着 TOTEMA产品走向世界，数百家来自全球各地的学生 运动俱乐部穿着TOTEMA产品驰骋各级赛事。每年 圣诞，我们都接到来自全球各地的问候邮件或者明信
                                    片数以千计…
                                </p>
                                <p>
                                    2014年伊始，我们把视线投向中国本土学校， 我们相信TOTEMA的创新与诚信以及TOTEMA 新国际主义校服产品和服务将为中国本土学校和孩子们 带来喜悦和满足，也借此重塑国人对校服在设计、健康、
                                    安全上的信心，并为中国教育国际化的崭新形象提供有 力保障。TOTEMA新国际主义校服(上海图麦诗服饰公 司)营销和服务中心位于上海闵行，旗下拥有自主产品 开发中心，样版中心，现代化成衣加工厂包括丝网印
                                    花车间、电脑绣花车间、数码印花中心等。
                                </p>
                                <p>
                                    用教育的情怀，打造专属于学生的品牌, 这是情怀；
                                </p>
                                <p>
                                    用国际一流的品质，服务中国的孩子们，这是实力。
                                </p>
                            </div>
                        </div>
                        <!--End of page container-->
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="footer">
        <div class="menu5">
            <ul>
                <li><a href="eventdetails.aspx"><span>
                    <img src="images/en1.png" alt="" title="" />活动详情</span></a></li>
                <li><a href="index.aspx"><span>
                    <img src="http://jkbp.comeoncloud.net/open/jkbp/images/home1.png" />
                </span></a></li>
                <li><a href="<%=new ZentCloud.BLLJIMP.BllVote().GetTotemaApplyLingOrMyClassLink()%>">
                    <span>
                        <img src="images/en3.png" alt="" title="" />我要报名</span></a></li>
            </ul>
        </div>
    </div>
</body>
</html>
