<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.BookingDoctor.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>膏方专家预约平台</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/customize/BookingDoctor/Style/comm.css?v=1.0.0.2" rel="stylesheet" />

    <style>
        body {
            /*padding-left: 10px
            padding-right: 10px;
            font-family: "Microsoft YaHei" !important;*/
        }

        .navbar-brand {
            padding: 8px 15px;
        }

        .navbar-default {
            background-color: #00D5C9;
            border-color: #e7e7e7;
            height: 30px;
        }

        .navbar-fixed-top, .navbar-fixed-bottom {
            position: relative;
        }



        .imglogo {
            width: 75px;
            height: 75px;
        }

        /*.logo {
            text-align: center;
            position: absolute;
            top: 10px;
            z-index: 10000;
            width: 100%;
        }*/

        .split {
            width: 100%;
        }



        .head {
            width: 80px;
            height: 80px;
            border-radius: 55px;
        }

        .name {
            font-size: 20px;
            margin-left: 15px;
        }

        .position {
            font-size: 16px;
            color: #908C88;
            margin-bottom: 5px;
        }

        /*.address {
            margin-left: 15px;
        }*/

        .hospital {
            margin-left: 15px;
            word-break: keep-all; /* 不换行 */
            white-space: nowrap; /* 不换行 */
            overflow: hidden; /* 内容超出宽度时隐藏超出部分的内容 */
            text-overflow: ellipsis; /* 当对象内文本溢出时显示省略标记(...) ；需与overflow:hidden;一起使用。*/
        }

        .yuyue {
            color: #00D5C9;
            text-align: center;
        }






        .nodata {
            font-size: 18px;
            /*font-weight:bold;*/
            text-align: center;
        }

        .divmore {
            text-align: center;
        }

        .btn-default {
            width: 100%;
            /*color:white;
            background-color: #00D5C9;*/
        }

        .main {
            margin-top: 2%;
        }

        .tab {
        }

        .tableft, .tabright {
            width: 50%;
            float: left;
            text-align: center;
            font-size: 18px;
            color: #908C88;
        }

        .tabright {
            position: relative;
        }

            .tabright span {
                margin-left: -15%;
                margin-right: 15px;
            }


        .tabright {
            border-left: solid;
            border-left-color: #00D5C9;
            color: #00D5C9;
        }

        #articlelist {
            display: none;
        }

        .moredoctor {
            font-size: 12px;
            /*float: right;*/
            position: absolute;
            right: 0;
            top: -4px;
        }

        .articleimg {
            width: 80px;
        }

        .articletitle, .articlesummary {
            margin-left: 5px;
        }

        .articletitle {
            font-size: 16px;
        }

        .articlesummary {
            color: #969696;
            font-size: 12px;
        }

        #btnMoreArticle, #btnMoreDoctor {
            margin-bottom: 50px;
        }

        .tdarticle {
            vertical-align: top;
            padding-left: 4px;
        }

        .wrapBanner {
            height: 275px;
        }

        .navbar-fixed-top, .navbar-fixed-bottom {
            position: inherit;
        }
    </style>

    <style>
        body {
            /*-webkit-font-smoothing: antialiased;
      font: normal 15px/1.5 "Helvetica Neue", Helvetica, Arial, sans-serif;
      color: #232525;*/
        }

        #slides {
            display: none;
        }

            #slides .slidesjs-navigation {
                margin-top: 5px;
            }

        a.slidesjs-next,
        a.slidesjs-previous,
        a.slidesjs-play,
        a.slidesjs-stop {
            background-image: url(img/btns-next-prev.png);
            background-repeat: no-repeat;
            display: block;
            width: 12px;
            height: 18px;
            overflow: hidden;
            text-indent: -9999px;
            float: left;
            margin-right: 5px;
        }

        a.slidesjs-next {
            margin-right: 10px;
            background-position: -12px 0;
        }

        a:hover.slidesjs-next {
            background-position: -12px -18px;
        }

        a.slidesjs-previous {
            background-position: 0 0;
        }

        a:hover.slidesjs-previous {
            background-position: 0 -18px;
        }

        a.slidesjs-play {
            width: 15px;
            background-position: -25px 0;
        }

        a:hover.slidesjs-play {
            background-position: -25px -18px;
        }

        a.slidesjs-stop {
            width: 18px;
            background-position: -41px 0;
        }

        a:hover.slidesjs-stop {
            background-position: -41px -18px;
        }

        .slidesjs-pagination {
            margin: 7px 0 0;
            float: right;
            list-style: none;
        }

            .slidesjs-pagination li {
                float: left;
                margin: 0 1px;
            }

                .slidesjs-pagination li a {
                    display: block;
                    width: 13px;
                    height: 0;
                    padding-top: 13px;
                    background-image: url(img/pagination.png);
                    background-position: 0 0;
                    float: left;
                    overflow: hidden;
                }

                    .slidesjs-pagination li a.active,
                    .slidesjs-pagination li a:hover.active {
                        background-position: 0 -13px;
                    }

                    .slidesjs-pagination li a:hover {
                        background-position: 0 -26px;
                    }

        #slides a:link,
        #slides a:visited {
            color: #333;
        }

        #slides a:hover,
        #slides a:active {
            color: #9e2020;
        }

        .navbar {
            overflow: hidden;
        }
    </style>
    <!-- End SlidesJS Optional-->

    <!-- SlidesJS Required: These styles are required if you'd like a responsive slideshow -->
    <style>
        #slides {
            display: none;
        }

        /*.container {
            margin: 0 auto;
            width: auto;
        }

        /* For tablets & smart phones */
        @media (max-width: 767px) {
            body {
                /*padding-left: 20px;
        padding-right: 20px;*/
            }
        }

        /* For smartphones */
        @media (max-width: 480px) {
            .container {
                width: auto;
            }
        }

        /* For smaller displays like laptops */
        @media (min-width: 768px) and (max-width: 979px) {
            .container {
                width: 724px;
            }
        }

        /* For larger displays */
        @media (min-width: 1200px) {
            .container {
                width: 1170px;
            }
        }

        */ .container {
            padding-left: 0px;
        }

        slidesjs-container {
        }

        .slides_container1 {
            overflow: visible;
        }
    </style>
</head>
<body>

    <div class="">
        <%--        <nav class="navbar navbar-default navbar-fixed-top mBottom0 border0" role="navigation">
            <div class="col-sm-1 col-xs-1">
                <div class="navbar-header">
                    <a class="navbar-brand">
                        <div class="return_ico">
                        </div>
                    </a>
                </div>
            </div>
            <div class="col-sm-10 col-xs-10 page_title">
                
            </div>
        </nav>--%>
        <%--<div class="logo">

            <img src="images/logo.png" class="imglogo" />
        </div>--%>
    </div>
    <%
        ZentCloud.BLLJIMP.BLLSlide bllSlide = new ZentCloud.BLLJIMP.BLLSlide();
        var list = bllSlide.GetList<ZentCloud.BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}'", bllSlide.WebsiteOwner));
        if (list.Count > 0)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" <div class=\"containerzzz\">");
            sb.AppendFormat("<div id=\"slides\" style=\"height:auto;\">");
            foreach (var item in list)
            {

                if (item.Link == "")
                {
                    sb.AppendFormat("<img src=\"{0}\" class=\"imgPreview\"  data-rel=\"\" />", item.ImageUrl);
                }
                else
                {
                    sb.AppendFormat("<img src=\"{0}\"  data-rel=\"{1}\" onclick=\"window.location.href='{1}'\" />", item.ImageUrl, item.Link);
                }

            }
            sb.AppendFormat("</div>");
            sb.AppendFormat("</div>");
            Response.Write(sb.ToString());
        } 
    %>

    <div class="container-fluid">


        <div class="main pLeft10 pRight10">

            <div class="tab">

                <div class="tableft" id="tableft">健康资讯</div>
                <div class="tabright" id="tabright">
                    <span>专家预约</span>

                    <div class="moredoctor" onclick="window.location.href='List.aspx';">
                        <img class="floatL" src="images/select.png" id="imgmore" />
                        <div class="floatL">
                            更多<br />
                            专家
                        </div>

                        <div class="clear"></div>
                    </div>

                </div>
            </div>

            <div>
                <img src="images/split.png" class="split" />
            </div>
            <div id="articlelist">

                <input id="btnMoreArticle" type="button" value="显示更多..." class="btn btn-default" />
            </div>
            <div id="doctorlist">

                <input id="btnMoreDoctor" type="button" value="显示更多..." class="btn btn-default" />
            </div>




        </div>

    </div>



</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="/Ju-Modules/bootstrap/js/bootstrap.min.js"></script>
<script src="/Scripts/Common.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js" type="text/javascript"></script>
<script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
<script src="/Scripts/StringBuilder.Min.js"></script>
<script src="/Scripts/jquery.slides.min.js"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>

<script>
    var pageIndexDoctor = 1;
    var pageSizeDoctor = 10;
    var pageIndexArticle = 1;
    var pageSizeArticle = 10;
    var currentTab = "doctor";
    $(function () {


        LoadDataArticle();
        LoadDataDoctor();

        $("#tableft").click(function () {

            $(this).css("color", "#00D5C9");
            $("#tabright").css("color", "#908C88");
            $("#articlelist").show();
            $("#doctorlist").hide();
            $("#imgmore").attr("src", "images/unselect.png");
            currentTab = "article";
            sessionStorage.setItem("lastTab", "article");
        });

        $("#tabright").click(function () {

            $(this).css("color", "#00D5C9");
            $("#tableft").css("color", "#908C88");
            $("#articlelist").hide();
            $("#doctorlist").show();
            $("#imgmore").attr("src", "images/select.png");
            currentTab = "doctor";
            sessionStorage.setItem("lastTab", "doctor");

        });

        if (sessionStorage.getItem("lastTab") != null) {

            if (sessionStorage.getItem("lastTab").toString() == "doctor") {
                $("#tabright").click();

            }
            else {
                $("#tableft").click();
            }
        }

        $("#btnMoreArticle").click(function () {

            pageIndexArticle++;
            LoadDataArticle();

        });
        $("#btnMoreDoctor").click(function () {

            pageIndexDoctor++;
            LoadDataDoctor();

        });


        $(window).scroll(function () {
            totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
            if ($(document).height() <= totalheight) {
                if (currentTab == "article") {
                    $(btnMoreArticle).click();
                }
                else {
                    $(btnMoreDoctor).click();

                }

            }
        });



    });

    function LoadDataDoctor() {

        $.ajax({
            type: 'post',
            url: "Handler.ashx",
            data: { Action: "DoctorList", keyWord: "", pageIndex: pageIndexDoctor, pageSize: pageSizeDoctor },
            dataType: "json",
            success: function (resp) {
                if (resp.result != null && resp.result.length > 0) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.result.length; i++) {
                        str.AppendFormat("<div onclick=\"window.location.href='Detail.aspx?id={0}'\">", resp.result[i].PID);
                        str.AppendFormat("<table class=\" mBottom6 mTop10\">");
                        str.AppendFormat("<tr>");
                        str.AppendFormat("<td>");
                        str.AppendFormat("<img class=\"head\" src=\"{0}\" />", resp.result[i].RecommendImg);

                        str.AppendFormat("</td>");
                        str.AppendFormat("<td >");
                        str.AppendFormat("<span class=\"name\">{0}</span>", resp.result[i].PName);
                        str.AppendFormat(" <span class=\"position\">{0}</span><br />", resp.result[i].ExArticleTitle_1);
                        //str.AppendFormat("<img src=\"images/address.png\" class=\"address\" />");
                        str.AppendFormat("<span class=\"hospital\">{0}</span><br/>", resp.result[i].ExArticleTitle_2);
                        str.AppendFormat("<span class=\"hospital\">擅长:{0}</span>", resp.result[i].ExArticleTitle_3);

                        str.AppendFormat("</td>");
                        str.AppendFormat("</tr>");
                        str.AppendFormat("</table>");
                        //str.AppendFormat(" <img src=\"images/split.png\" class=\"split\" />");
                        str.AppendFormat(" <hr class=\"split\" />");
                        str.AppendFormat(" </div>");

                    }
                    $("#btnMoreDoctor").before(str.ToString());

                }
                else {




                }
                if (pageIndexDoctor > 1 && resp.result.length == 0) {

                    $("#btnMoreDoctor").val("没有更多了");
                }



            }
        });

    }




    function LoadDataArticle() {


        $.ajax({
            type: 'post',
            url: "Handler.ashx",
            data: { Action: "ArticleList", pageIndex: pageIndexArticle, pageSize: pageSizeArticle },
            dataType: "json",
            success: function (resp) {
                if (resp.result != null && resp.result.length > 0) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.result.length; i++) {
                        str.AppendFormat("<div onclick=\"window.location.href='/{0}/detail.chtml'\">", resp.result[i].JuActivityIDHex);
                        str.AppendFormat("<table  class=\" mBottom6 mTop10\">");
                        str.AppendFormat("<tr>");
                        str.AppendFormat("<td>");
                        str.AppendFormat("<img class=\"articleimg\" src=\"{0}\" />", resp.result[i].ThumbnailsPath);

                        str.AppendFormat("</td>");
                        str.AppendFormat("<td class=\"tdarticle\">");
                        str.AppendFormat("<span class=\"articletitle\">{0}</span><br/>", resp.result[i].ActivityName);
                        str.AppendFormat("<span class=\"articlesummary\">{0}</span>", resp.result[i].Summary);

                        str.AppendFormat("</td>");
                        str.AppendFormat("</tr>");
                        str.AppendFormat("</table>");
                        //str.AppendFormat(" <img src=\"images/split.png\" class=\"split\" />");
                        str.AppendFormat(" <hr class=\"split\" />");
                        str.AppendFormat(" </div>");


                    }
                    $("#btnMoreArticle").before(str.ToString());


                }
                else {


                    if (pageIndexArticle > 1 && resp.result.length == 0) {

                        $("#btnMoreArticle").val("没有更多了");
                    }


                }



            }
        });

    }





</script>
<script>
    $(function () {
        $('#slides').slidesjs({
            width: 600,
            height: 275,
            generateNextPrev: false,
            generatePagination: false,
            autoHeight: true,
            container: 'slides_container1',
            play: {
                active: false,
                auto: true,
                interval: 4000,
                swap: true
            },
            navigation: {
                active: false,
                // [boolean] Generates next and previous buttons.
                // You can set to false and use your own buttons.
                // User defined buttons must have the following:
                // previous button: class="slidesjs-previous slidesjs-navigation"
                // next button: class="slidesjs-next slidesjs-navigation"
                effect: "slide"
                // [string] Can be either "slide" or "fade".
            },
            pagination: {
                active: false,
                // [boolean] Create pagination items.
                // You cannot use your own pagination. Sorry.
                effect: "slide"
                // [string] Can be either "slide" or "fade".
            }

        });


        var __mls = [];
        $.each($('img'), function (i, item) {

            var _this = $(this);


            if (item.src && _this.hasClass('imgPreview')) {
                __mls.push(item.src);
                $(item).click(function (e) {
                    wx.previewImage({
                        current: this.src,
                        urls: __mls
                    });
                });
            }
        });

    });
</script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "<%=config.WebsiteTitle%>",
            desc: "<%=config.WebsiteDescription%>",
            //link: '', 
            imgUrl: "<%=config.WebsiteImage%>"
        })
    })
</script>
</html>
