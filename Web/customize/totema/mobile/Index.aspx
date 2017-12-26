<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.mobile.Index" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/green.css" />
<%--    <link type="text/css" rel="stylesheet" href="css/idangerous.swiper.css" />
    <link type="text/css" rel="stylesheet" href="css/swipebox.css" />--%>
    <script type="text/javascript" src="js/jquery-1.10.1.min.js"></script>
<%--    <script src="js/jquery.validate.min.js" type="text/javascript"></script>--%>
    <style>
    .image_single img{height:auto;}
    .menu{margin-bottom:80px;}
    </style>
</head>
<body>
    <!--Page 1 content-->
    <div class="swiper-slide sliderbg">
        <div class="swiper-container swiper-nested">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="slide-inner">
                        <div class="pages_container white">
                            <div id="main_panels">
                                <div class="panels_slider">
                                    <ul class="slides">
                                        <li>
                                            <img src="images/new1.png" alt="" title="" border="0" />
                                        </li>
                                        <li>
                                            <img src="images/new2.png" alt="" title="" border="0" />
                                        </li>
                                        <li>
                                            <img src="images/new3.png" alt="" title="" border="0" />
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="index_container radius4" onclick="window.location.href='eventdetails.aspx'">
                                <div class="image_single">
                                    <img src="images/ad.png" alt="" title="" border="0" /></div>
                                <div class="image_caption black ">
                                    <a href="javascript:">“我爱我班！我为我班赢班服！” </a>
                                </div>
                                <p class="p-text">
                                  TOTEMA图麦诗校服上海市教博会官方活动：找几个同班的小伙伴，拍一张最能体现你们班风采的照片，上传本活动，就有机会为你们班赢取全套定制班服等丰厚大奖。来为你们班争光添彩吧！ 
                                </p>
                            </div>
                            <!--Menu page-->
                            <div class="menu">
                                <ul>
                                    <li><a href="eventdetails.aspx"><span>活动详情</span>
                                        <img src="images/idnex01.png" alt="" title="" /></a></li>
                                    <li><a href="<%=new ZentCloud.BLLJIMP.BllVote().GetTotemaApplyLingOrMyClassLink()%>"><span>我要报名</span>
                                        <img src="images/idnex02.png" alt="" title="" /></a></li>
                                    <li><a href="classlist.aspx"><span>我要投票</span>
                                        <img src="images/idnex03.png" alt="" title="" /></a></li>
                                </ul>
                                <div class="clearfix">
                                </div>
                            </div>
                            <!--End of pages-->
                        </div>
                        <!--End of page container-->
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="footer">
        <div class="menu1">
            <ul>
                <li><a href="#"><span>首页</span></a></li>
                <li><a href="javascript:" onclick="openUl(1)"><span>
                    <img src="images/list.png" alt="" title="" />热门活动</span></a>
                    <div class="menu1-1" id="child1" style="display:none;">
                        <ul>
                            <li><a href="classlist.aspx"><span>我爱我班</span></a></li>
                            <li><a href="FreeWearApply.aspx"><span>免费穿报名</span></a></li>
                            <li><a href="#"><span>入选名单</span></a></li></ul>
                    </div>
                </li>
                <li><a href="javascript:void(0)" onclick="openUl(2)"><span>
                    <img src="images/list.png" alt="" title="" />关于我们</span></a>
                    <div class="menu1-1" id="child2" style="display:none;">
                        <ul>
                            <li><a href="About.aspx"><span>公司简介</span></a></li>
                           
                        </ul>
                    </div>
                </li>
                <li><a href="Contact.aspx"><span>联系我们</span></a></li>
            </ul>
        </div>
    </div>
<%--    <script type="text/javascript" src="js/jquery.swipebox.js"></script>
    <script type="text/javascript" src="js/idangerous.swiper-2.1.min.js"></script>
    <script type="text/javascript" src="js/idangerous.swiper.scrollbar-2.1.js"></script>
    <script type="text/javascript" src="js/jquery.tabify.js"></script>
    <script type="text/javascript" src="js/jquery.fitvids.js"></script>
    <script type="text/javascript" src="js/code.js"></script>
    <script type="text/javascript" src="js/load.js"></script>
--%>    
    <script type="text/javascript" src="js/jquery.flexslider.js"></script>
    <script type="text/javascript">

        var $ = jQuery.noConflict();
        $(window).load(function () {
            $('.panels_slider').flexslider({
                animation: "slide",
                directionNav: false,
                controlNav: true,
                animationLoop: true,
                slideToStart: 0,
                slideshowSpeed: 3000,
                animationDuration: 300,
                slideshow: true
            });
        });
        function openUl(obj) {
            if ($("#child" + obj).css('display') == "none") {
                $(".menu1-1").css("display", "none");
                $("#child" + obj).css('display', 'block');
            }
            else {
                $("#child" + obj).css('display', 'none');
            }
        }
    </script>

    <script type="text/javascript" src="/Scripts/Common.js"></script>
    <script type="text/javascript" src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        var pageData = {

            currUserOpenId: '<%=ZentCloud.BLLJIMP.BLLStatic.bll.GetCurrentUserInfo().WXOpenId%>',//当前用户的wxopenId
            currUserId: '<%=ZentCloud.BLLJIMP.BLLStatic.bll.GetCurrentUserInfo().UserID%>',//当前用户的userId
            title: '我爱我班！我为我班赢班服！',//标题
            summary: 'TOTEMA新国际主义校服上海教博会官方活动。丰厚大奖,火速报名啦!',//描述
            shareImgUrl: 'http://<%=Request.Url.Host%>/customize/totema/mobile/images/shareimg.png',//分享缩略图
            shareUrl: 'http://<%=Request.Url.Host%>/customize/totema/mobile/eventDetails.aspx',//分享链接

            tempShareId: CreateGUID(),
            preShareId: GetParm('comeonshareid'),
            callback: callback
        };

        var shareCallBackFunc = {
            timeline_s: function () {
                submitShare('timeline_s');
            },
            timeline_c: function () {
                //朋友圈分享取消
            },
            message_s: function () {
                //分享给朋友
                submitShare('message_s');
            },
            message_c: function () {
                //朋友分享取消
            }
        }

        var processUrl = function (url) {
            url = DelUrlParam(url, 'comeonshareid');
            url = DelUrlParam(url, 'from');
            url = DelUrlParam(url, 'isappinstalled');
            return url;
        }

        var callback = function (data) { }

        var submitShare = function (WxMsgType) {
            var reqData = {
                Action: 'ShareSubmit',
                url: processUrl(pageData.shareUrl),
                shareId: pageData.tempShareId,
                preId: pageData.preShareId,
                userId: pageData.currUserId,
                userWxOpenId: pageData.currUserOpenId,
                wxMsgType: WxMsgType
            }

            //分享到朋友圈
            $.ajax({
                type: 'post',
                url: '/serv/pubapi.ashx',
                data: reqData,
                dataType: 'jsonp',
                success: function (data) {
                    pageData.tempShareId = CreateGUID();
                }
            });
        }

        //移除原有参数 comeonshareid from isappinstalled
        pageData.shareUrl = processUrl(pageData.shareUrl);
        pageData.shareUrl = pageData.shareUrl + '?comeonshareid=' + pageData.tempShareId;

        wx.ready(function () {
            wxapi.wxshare({
                title: pageData.title,
                desc: pageData.summary,
                link: pageData.shareUrl,
                imgUrl: pageData.shareImgUrl
            }, shareCallBackFunc)
        });

    </script>




</body>
</html>