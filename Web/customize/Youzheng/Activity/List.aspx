<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Youzheng.Activity.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/css/exam/all.min.css" rel="stylesheet" />

    <style>
        body {
            background-color: #F0F0F0;
            margin: 0;
            overflow: hidden;
            font-family: 'STHeiti','Microsoft YaHei',Helvetica,Arial,sans-serif;
        }

        html, body, div, span, applet, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, big, cite, code, del, dfn, em, img, ins, kbd, q, s, samp, small, strike, strong, sub, sup, tt, var, b, i, u, center, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, table, caption, tbody, tfoot, thead, tr, th, td, article, aside, canvas, details, embed, fieldset, figure, figcaption, footer, header, hgroup, menu, nav, output, ruby, section, summary, time, mark, audio, video {
            vertical-align: middle;
        }

        .top {
            padding-left: 5px;
            padding-top: 5px;
            height: 40px;
            position: fixed;
            top: 0px;
            background-color: white;
            width: 100%;
            z-index: 1000;
            opacity: 0.98;
        }

        #txtKeyWord {
            height: 30px;
            width: 100%;
            border-radius: 25px;
            border: 1px solid #E3E1DE;
            padding-left: 10px;
        }

        .top-left {
            float: left;
            width: 20%;
        }

            .top-left img {
                width: 90%;
                max-height: 30px;
                max-width: 80px;
            }

        .top-midd {
            float: left;
            padding-left: 10px;
            width: 75%;
            position: relative;
        }

        .top-right {
            float: left;
            padding-left: 10px;
            position: absolute;
            right: 25px;
        }

            .top-right img {
                height: 28px;
                padding-top: 5px;
            }






        .toolbar {
            height: 35px;
            color: #96928D;
            position: relative;
            background: #fff;
            position: fixed;
            top: 35px;
            width: 100%;
            background-color: white;
            z-index: 1000;
            opacity: 0.98;
            padding-top: 6px;
        }

        .toolbar-left {
            color: #7A7A7A;
            font-size: 16px;
        }

            .toolbar-left label {
                font-size: 14px;
            }

        .toolbar-right {
            text-align: right;
            padding-right: 10px;
            color: #333;
        }

            .toolbar-right img {
                height: 15px;
                vertical-align: middle;
            }


        .row-left {
            width: 110px;
            float: left;
            overflow: auto;
            padding-left: 20px;
            padding-bottom: 10px;
        }

            .row-left img {
                max-width: 100%;
                max-height: 100%;
                border-radius: 9px !important;
                -webkit-border-radius: 9px !important;
                -moz-border-radius: 9px !important;
            }

        .row-right {
            float: left;
            padding-left: 15px;
        }

        .price .info {
            padding-top: 5px;
        }

        .name {
            font-size: 15px;
            font-weight: bold;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .sale-price {
            font-size: 20px;
            font-weight: bold;
            color: #FF7E00;
        }

        .money-flag {
            font-size: 10px;
            font-weight: bold;
            color: #FF7E00;
        }

        .pre-price {
            text-decoration: line-through;
            padding-left: 25px;
        }

        .sale-count {
            color: #FF7E00;
            margin-left: 20px;
        }

        #courseList {
            margin-bottom: 150px;
            -webkit-overflow-scrolling: touch;
        }


        .content .foot .col {
            padding-top: 9px !important;
            padding-bottom: 8px !important;
        }

        .foot {
            margin: 0;
            filter: alpha(Opacity=96);
            -moz-opacity: 0.96;
            opacity: 0.96;
            width: 100%;
        }

            .foot img {
                width: 25px;
            }

            .foot .col {
                margin-top: 2px;
                padding: 0px;
            }

        .sort-area {
            display: none;
        }

        .sort {
            position: absolute;
            top: 35px;
            right: 10px;
            width: 150px;
            background-color: #343535;
            color: white;
            font-size: 15px;
            border-radius: 5px;
            padding-top: 10px;
            filter: alpha(Opacity=90);
            -moz-opacity: 0.90;
            opacity: 0.90;
            font-size: 18px;
            vertical-align: middle !important;
        }

            .sort div {
                padding-top: 10px;
                padding-bottom: 10px;
                vertical-align: middle !important;
                border-bottom: 1px solid #424342;
                font-size: 14px;
                font-weight: normal;
            }

                .sort div:last-child {
                    border-bottom: none;
                }

                .sort div img {
                    margin-left: 20px;
                    height: 15px;
                }

        .area {
            width: 20px;
            height: 10px;
            position: absolute;
            top: 15px;
            right: 50px;
            filter: alpha(Opacity=90);
            -moz-opacity: 0.90;
            opacity: 0.90;
        }



        .dot-top {
            font-size: 0;
            line-height: 0;
            border-width: 10px;
            border-color: #343535;
            border-top-width: 0;
            border-style: dashed;
            border-bottom-style: solid;
            border-left-color: transparent;
            border-right-color: transparent;
        }

        .category {
            position: absolute;
            top: 25px;
            width: 100%;
            background-color: #F3F2F1;
            color: black;
            font-size: 15px;
            filter: alpha(Opacity=98);
            -moz-opacity: 0.98;
            opacity: 0.98;
            font-size: 18px;
            vertical-align: middle !important;
            min-height: 150px;
            text-align: center;
            display: none;
            z-index: 1000;
        }

        .category-body {
            max-height: 300px;
            overflow-y: auto;
            margin-bottom: 30px;
        }

        .catecory-item {
            float: left;
            width: 65px;
            height: 90px;
            margin-left: 10px;
            margin-top: 10px;
            font-size: 12px;
        }

        .category img {
            width: 64px;
            height: 64px;
            border-radius: 60px !important;
        }

        .category-bottom {
            margin-top: 0px;
            margin-bottom: 10px;
            width: 100%;
            float: left;
        }

        .btn-all-category {
            background-color: #E8E7E4;
            width: 90%;
            border-radius: 5px;
            height: 40px;
            font-size: 18px;
            padding-top: 8px;
        }

        #btnMore {
            width: 95%;
            text-align: center;
            margin-top: 10px;
            margin-left: 2.5%;
            color: #96928D;
        }

        .nodata {
            text-align: center;
            font-size: 18px;
            margin-top: 100px;
            display: none;
        }

        .lblcategoryname {
            padding-top: 5px;
        }

        #lblCategory {
            font-size: 15px;
        }

        #lblSort {
            font-size: 14px;
        }

        .img-top {
            margin-top: 20px;
        }

            .img-top img {
                width: 100%;
                height: 180px;
            }



        .list-title {
            float: left;
            padding-left: 10px;
            color: #333;
        }

        .activity-item {
            background-color: white;
            margin-top: 15px;
        }



        .activity-info {
            background: #fff;
            padding: 10px;
        }

        .info-left {
            float: left;
            width: 75%;
        }

            .info-left img {
                width: 15px;
                height: 15px;
            }

        .info-right {
            text-align: right;
            color: #F29943;
            line-height: 39px;
        }

        .info-price {
            font-size: 20px;
            max-width: 100%;
        }


        .activity-title {
            font-size: 16px;
            color: #333;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        .toolbar-list {
            overflow-y: auto;
            display: block;
            height: 100%;
            position: fixed;
            top: 0px;
            padding-top: 50px;
            padding-bottom: 55px;
            overflow-x: hidden;
            width: 100%;
            -webkit-overflow-scrolling: touch;
        }

        .orange {
            color: #FF7E00 !important;
        }

        .Yellow {
            color: #EB9C53 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="top">
        <div class="top-left">
            <img src="../Images/logo.png" />
        </div>
        <div class="top-midd">
            <input type="text" placeholder="&nbsp;&nbsp;输入活动名称" id="txtKeyWord" />
        </div>
        <div class="top-right">
            <img src="../Images/search1.png" id="btnSearch" />
        </div>
    </div>

    <div class="toolbar">
        <div class="toolbar-left">
            <div class="list-title">
                <label><%=string.IsNullOrEmpty(Request["isgame"])?"活动列表":"竞赛列表"%></label>
            </div>

            <div class="toolbar-right" id="toolbarSort">
                <label id="lblSort">综合排序</label>
                <img src="../Images/sort.png" id="imgSort" />
            </div>
        </div>





        <div class="sort-area">

            <div class="area">
                <span class="dot-top"></span>

            </div>
            <div class="sort">

                <div data-sort-field="activity_sort" data-sort-name="综合排序" data-sort-img="../images/sortselect.png">
                    <img src="../images/sortwhite.png" />
                    综合排序
                </div>
                <div data-sort-field="activity_signcount" data-sort-name="火爆参与" data-sort-img="../images/hotselect.png">
                    <img src="../images/hot.png" />
                    火爆参与
                </div>
                <div data-sort-field="activity_createDate" data-sort-name="最新发布" data-sort-img="../images/timeselect.png">
                    <img src="../images/time.png" />
                    最新发布
                </div>
                <div data-sort-field="activity_start_time" data-sort-name="近期活动" data-sort-img="../images/shandianselect.png">
                    <img src="../images/shandian.png" />
                    近期活动
                </div>
            </div>


        </div>

    </div>


    <div class="toolbar-list" id="toolbar-list">
        <%--<div class="activity-item">
                <div class="img-top">
                    <img src="http://open-files.comeoncloud.net/www/hf/jubit/image/20161209/170A11AAF5C24DC1BD0621E01A4A524E.png" />
                </div>
                <div class="activity-info">
                    <div class="info-left">
                        <p class="activity-title">秋季精品商务主持大赛</p>
                        <img src="/img/address-icon.png" class="img-icon" />
                        <span>地址:</span>
                        <span>上海市闸北区江场西路江阳西路100号</span>
                    </div>
                    <div class="info-right">
                        <span>￥</span>
                        <span class="info-price">198.0</span>
                    </div>
                </div>
            </div>--%>

        <input id="btnMore" type="button" value="显示更多" class="btn btn-default" onclick="LoadMore()" />
    </div>

    <div class="nodata">
        搜索不到结果

    </div>

    <div class="row foot text-center">
        <a class="col" href="/customize/comeoncloud/Index.aspx?key=MallHome">
            <img src="../images/home.png" />
            <br />
            首页</a>

        <a class="col" href="/Customize/YouZheng/Course/List.aspx">
            <img src="../images/book.png" />
            <br />
            课程</a>


        <a class="col <%=string.IsNullOrEmpty(Request["isgame"])?"orange":""%>" href="/Customize/YouZheng/Activity/List.aspx">


            <img src="<%=string.IsNullOrEmpty(Request["isgame"])?"../images/huodongselect.png":"../images/huodong.png"%>" />
            <br />
            活动</a>
        <a class="col <%=string.IsNullOrEmpty(Request["isgame"])?"":"orange"%>" href="/Customize/YouZheng/Activity/List.aspx?isgame=1">
            <img src="<%=string.IsNullOrEmpty(Request["isgame"])?"../images/jiangbei.png":"../images/jiangbeiselect.png"%>" />
            <br />
            竞赛</a>

        <a class="col" href="/customize/comeoncloud/Index.aspx?key=PersonalCenter">
            <img src="../images/my.png" />
            <br />
            我的</a>

    </div>





</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/StringBuilder.Min.js"></script>
    <script>
        var handlerUrl = "/serv/api/activity/list.ashx";
        var sortField = "";
        var pageIndex = 1;
        var pageSize = 10;
        var isFee = 1;//只搜索收费活动
        var categoryId = "";
        var totalheight;



        $(function () {
            SetTouchScoll();
            if ("<%=Request["isgame"]%>" != "") {
                categoryId = "1451";
            }

            $("#lblSort").click(function () {
                if ($(".sort-area").css('display') == "none") {
                    $(".sort-area").show();
                    $('#lblSort').addClass('Yellow');
                    $('#imgSort').attr('src', '../Images/sortselect.png');
                }
                else {
                    $(".sort-area").hide();
                    $('#lblSort').removeClass('Yellow');
                    $('#imgSort').attr('src', '../Images/sort.png');
                }

            });

            $("[data-sort-field]").click(function () {

                sortField = $(this).attr("data-sort-field");
                $(lblSort).text($(this).attr("data-sort-name"));
                $(imgSort).attr("src", $(this).attr("data-sort-img"));
                $(".sort-area").hide();
                pageIndex = 1;
                Clear();
                Search();


            });




            $(window).scroll(function () {
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                if ($(document).height() <= totalheight) {
                    $("#btnMore").click();
                }
            })


            $("#btnSearch").click(function () {

                pageIndex = 1;
                Clear();
                Search();

            })


            Search();

        });
        function SetTouchScoll() {
            var startY, moveY, startX, moveX;
            $(document).on('touchstart', 'body', function (event) {
                startY = event.originalEvent.changedTouches[0].pageY;
                startX = event.originalEvent.changedTouches[0].pageX;
            })
            $(document).on('touchmove', 'body', function (event) {
                moveX = event.originalEvent.changedTouches[0].pageX;
                moveY = event.originalEvent.changedTouches[0].pageY;
                if (Math.abs(moveY - startY) > Math.abs(moveX - startX)) {
                    if (moveY - startY > 0) {
                        if ($('.toolbar-list').get(0).scrollTop == 0) {
                            event.preventDefault();
                        }
                    }
                    else if (moveY - startY < 0) {
                        if (Math.round($('.toolbar-list').get(0).scrollHeight) == Math.round($('.toolbar-list').get(0).scrollTop + $('.toolbar-list').height())) {
                            event.preventDefault();
                        }
                    }
                }
                //event.preventDefault();
            });
        };
        function Search() {

            var par = {
                pageindex: pageIndex,
                pagesize: pageSize,
                keyword: $(txtKeyWord).val(),
                activity_sort: sortField,
                isFee: isFee,
                category_id: categoryId

            };
            $("#btnMore").text("加载中...");
            layer.open({ type: 2 });
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: par,
                dataType: "json",
                success: function (resp) {
                    layer.closeAll();
                    $("#btnMore").text("显示更多");
                    $(".nodata").hide();
                    $("#btnMore").show();
                    if (resp.list.length > 0) {
                        var str = new StringBuilder();



                        for (var i = 0; i < resp.list.length; i++) {

                            str.AppendFormat("<div class=\"activity-item\"  onclick=\"goDetail({0})\">", resp.list[i].activity_id);

                            str.AppendFormat("<div class=\"img-top\" >");
                            str.AppendFormat("<img src=\"{0}\"/>", resp.list[i].activity_img_url);
                            str.AppendFormat("</div>");
                            str.AppendFormat(" <div class=\"activity-info\">");

                            str.AppendFormat(" <div class=\"info-left\">");

                            str.AppendFormat("<p class=\"activity-title\">{0}</p>", resp.list[i].activity_name);

                            str.AppendFormat("<img src=\"/img/address-icon.png\"  class=\"img-icon\"/>", resp.list[i].activity_img_url);

                            str.AppendFormat(" <span>地址:</span>");

                            str.AppendFormat("<span>{0}</span>", resp.list[i].activity_address);

                            str.AppendFormat(" </div>");
                            str.AppendFormat("  <div class=\"info-right\">");

                            str.AppendFormat(" <span>￥</span>");

                            str.AppendFormat("  <span class=\"info-price\">{0}</span>", resp.list[i].activity_price.toFixed(1));

                            str.AppendFormat("</div>");
                            str.AppendFormat("</div>");
                            str.AppendFormat("<div style=\"clear: both;\"></div>");
                            str.AppendFormat(" </div>");

                        }
                        $("#btnMore").before(str.ToString());
                        if (pageIndex == 1 && resp.list.length < parseInt(pageSize)) {
                            $("#btnMore").val("没有更多了");
                        }

                    }
                    else {
                        if (pageIndex == 1 && resp.list.length == 0) {
                            $(".nodata").show();
                            $("#courseList").html("<input  id=\"btnMore\" type=\"button\"  value=\"显示更多\" class=\"btn btn-default\" onclick=\"LoadMore()\"/>");
                            $("#btnMore").hide();

                        }
                        if (pageIndex > 1 && resp.list.length == 0) {
                            $(".nodata").hide();
                            $("#btnMore").show();
                            $("#btnMore").val("没有更多了");
                            // $("#btnMore").hide();
                        }


                    }



                }
            });



        }

        function LoadMore() {
            pageIndex++;
            Search();

        }

        function Clear() {

            $("#toolbar-list").html("<input  id=\"btnMore\" type=\"button\"  value=\"显示更多\" class=\"btn btn-default\" onclick=\"LoadMore()\"/>");


        }


        function goDetail(jid) {
            var linkurl = "http://" + window.location.host + "/" + parseInt(jid).toString(16) + "/" + "details.chtml";
            window.location.href = linkurl;
        }

    </script>
</asp:Content>
