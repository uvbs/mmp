<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Youzheng.Course.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/css/exam/all.min.css" rel="stylesheet" />

    <style>
        html, body, div, span, applet, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, big, cite, code, del, dfn, em, img, ins, kbd, q, s, samp, small, strike, strong, sub, sup, tt, var, b, i, u, center, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, table, caption, tbody, tfoot, thead, tr, th, td, article, aside, canvas, details, embed, fieldset, figure, figcaption, footer, header, hgroup, menu, nav, output, ruby, section, summary, time, mark, audio, video {
            vertical-align: middle;
        }

        body {
            background-color: white;
            margin: 0;
            overflow: hidden;
            font-family: 'STHeiti','Microsoft YaHei',Helvetica,Arial,sans-serif;
        }



        .top {
            padding-top: 5px;
            padding-left: 5px;
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
            margin-left: 0px;
            padding-top: 10px;
            padding-bottom: 5px;
            height: 30px;
            font-weight: bold;
            color: #96928D;
            position: fixed;
            top: 35px;
            width: 100%;
            background-color: white;
            z-index: 1000;
            opacity: 0.98;
            height: 35px;
        }

        .toolbar-left {
            width: 70%;
            float: left;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

            .toolbar-left img {
                height: 15px;
                vertical-align: middle;
                margin-left: 10px;
            }

        .toolbar label {
            color: #333;
        }

        .toolbar-right {
            width: 30%;
            float: left;
            text-align: right;
            padding-right: 10px;
            color: #FF7E00;
        }

            .toolbar-right img {
                height: 14px;
                vertical-align: middle;
            }

        .row {
            margin:0;
		 border-bottom: 1px solid #ccc;

        }

        /*.row {
            margin-left: 0px;
            margin-right: 0px;
        }*/

        .row-left {
            width: 30%;
            float: left;
            overflow: auto;
            padding-left: 5px;
            padding-bottom: 10px;
        }

            .row-left img {
                max-width: 100%;
                max-height: 100%;
                -webkit-border-radius: 3px !important;
                -moz-border-radius: 3px !important;
            }

        .row-right {
            width: 70%;
            float: left;
            padding: 0px 8px;
        }

        .price {
            padding-top: 5px;
            vertical-align: auto !important;
        }

        .info {
            padding-top: 5px;
        }

        .name {
            font-size: 16px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .sale-price {
            font-size: 20px;
            color: #FF7E00;
        }

        .money-flag {
            font-size: 10px;
            color: #FF7E00;
        }

        .info {
            color: #96928D;
        }

        .pre-price {
            text-decoration: line-through;
            padding-left: 25px;
            font-size: 14px;
            color: #96928D;
        }

        .sale-count {
            color: #FF7E00;
            margin-left: 20px;
        }

        #courseList {
            overflow-y: auto;
            display: block;
            height: 100%;
            position: fixed;
            top: 0px;
            padding-top: 75px;
            padding-bottom: 55px;
            overflow-x: hidden;
            -webkit-overflow-scrolling: touch;
            width:100%;
        }


        .row + .row {
            margin-top: 10px;
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
            top: 40px;
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
            top: 20px;
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
            top: 35px;
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
            max-height: 250px;
            overflow-y: auto;
            padding: 20px 15px;
        }

        .catecory-item {
            float: left;
            width: 25%;
            height: 85px;
            font-size: 12px;
            overflow: hidden;
            color: #96928D;
        }

            .catecory-item label {
                font-size: 100%;
            }

        .category img {
            width: 50px;
            height: 50px;
            border-radius: 50px !important;
        }

        .category-bottom {
            margin-bottom: 20px;
            width: 100%;
            float: left;
        }

        .btn-all-category {
            background-color: #E8E7E4;
            width: 92%;
            border-radius: 4px;
            height: 36px;
            line-height: 36px;
            font-size: 14px;
            color: #96928D;
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
            color: #96928D;
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
            <input type="text" placeholder="&nbsp;&nbsp;输入课程名称" id="txtKeyWord" />
        </div>
        <div class="top-right">
            <img src="../Images/search1.png" id="btnSearch" />
        </div>
    </div>

    <div class="toolbar">
        <div class="toolbar-left">
            <img src="../Images/category.png" />
            <label id="lblCategory">课程分类</label>



        </div>

        <div class="toolbar-right" id="toolbarSort">
            <label id="lblSort">综合排序</label>
            <img src="../Images/sort.png" id="imgSort" />

        </div>

        <div class="sort-area">

            <div class="area">
                <span class="dot-top"></span>

            </div>
            <div class="sort">

                <div data-sort-field="" data-sort-name="综合排序" data-sort-img="../images/sortselect.png">
                    <img src="../images/sortwhite.png" />
                    综合排序

                </div>
                <div data-sort-field="sales_volume" data-sort-name="爆考" data-sort-img="../images/hotselect.png">
                    <img src="../images/hot.png" />
                    爆考
                </div>
                <div data-sort-field="time_desc" data-sort-name="最新" data-sort-img="../images/timeselect.png">
                    <img src="../images/time.png" />
                    最新
                </div>
                <div data-sort-field="sales_volume" data-sort-name="人气" data-sort-img="../images/shandianselect.png">
                    <img src="../images/shandian.png" />
                    人气
                </div>
            </div>


        </div>


        <div class="category">

            <div class="category-body">

                <%foreach (var item in categoryList)
                  {%>
                <div class="catecory-item" data-category-id="<%=item.AutoID %>" data-category-name="<%=item.CategoryName %>">
                    <img src="<%=item.CategoryImg %>" />
                    <br />
                    <label class="lblcategoryname"><%=item.CategoryName %></label>


                </div>

                <% } %>
            </div>


            <div class="category-bottom">

                <label class="btn-all-category">全部课程</label>

            </div>

        </div>

    </div>

    <div id="courseList">





        <input id="btnMore" type="button" value="显示更多..." class="btn btn-default" onclick="LoadMore()" />

    </div>

    <div class="nodata">
        搜索不到结果

    </div>

    <div class="row foot text-center">
        <a class="col" href="/customize/comeoncloud/Index.aspx?key=MallHome">
            <img src="../images/home.png" />
            <br />
            首页</a>

        <a class="col orange" href="/Customize/YouZheng/Course/List.aspx">
            <img src="../images/bookselect.png" />
            <br />
            课程</a>


        <a class="col" href="/Customize/YouZheng/Activity/List.aspx">
            <img src="../images/huodong.png" />
            <br />
            活动</a>
        <a class="col" href="/Customize/YouZheng/Activity/List.aspx?isgame=1">
            <img src="../images/jiangbei.png" />
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
        var handlerUrl = "/serv/api/course/list.ashx";
        var sortField = "";
        var categoryId = "";
        //var categoryId = "1452";
        var pageIndex = 1;
        var pageSize = 20;
        var type = "Course";

        var keyword = '<%=keyword%>';


        $(function () {
            SetTouchScoll();
            if (keyword != '') {
                $('#txtKeyWord').val(keyword);
            }


            if ("<%=Request["categoryid"]%>" != "") {
                categoryId = "<%=Request["categoryid"]%>";

                var categoryName="";



                $.each($("[data-category-id]"), function (index, item) {
                
                  
                    if ($(item).attr("data-category-id")==categoryId) {
                        categoryName = $(item).attr("data-category-name");
                    }

                })

                
                $(lblCategory).html("课程分类/"+categoryName);

            }

            $("#toolbarSort").click(function () {

                $(".category").hide();
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
            })

            $(".toolbar-left").click(function () {


                $(".sort-area").hide();

                if ($(".category").css('display') == "none") {
                    $(".category").show();
                }
                else {
                    $(".category").hide();
                }
            });

            $(".btn-all-category").click(function () {

                categoryId = "";
                $(".category").hide();
                $("#lblCategory").text("课程分类");
                pageIndex = 1;
                Clear();
                Search();
            })

            $(".catecory-item").click(function () {

                categoryId = $(this).attr("data-category-id");
                $("#lblCategory").text("课程分类/" + $(this).attr("data-category-name"));
                $(".category").hide();
                pageIndex = 1;
                Clear();
                Search();

            })


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

            });

            Search();

        })
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
                        if ($('#courseList').get(0).scrollTop == 0) {
                            event.preventDefault();
                        }
                    }
                    else if (moveY - startY < 0) {
                        if (Math.round($('#courseList').get(0).scrollHeight) == Math.round($('#courseList').get(0).scrollTop + $('#courseList').height())) {
                            event.preventDefault();
                        }
                    }
                }
                //event.preventDefault();
            });
        };
        function Search() {
            var par = {
                type: type,
                pageIndex: pageIndex,
                pageSize: pageSize,
                keyWord: $('#txtKeyWord').val(),
                category_id: categoryId,
                sort: sortField
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
                    if (resp.result != null && resp.result.list.length > 0) {
                        var str = new StringBuilder();
                        for (var i = 0; i < resp.result.list.length; i++) {
                            str.AppendFormat("<div class=\"row\" onclick=\"window.location.href='Detail.aspx?id={0}'\">", resp.result.list[i].product_id);
                            str.AppendFormat(" <div class=\"row-left\">");
                            str.AppendFormat("<img src=\"{0}?x-oss-process=image/resize,m_fill,h_160,w_200/format,png\" />", resp.result.list[i].img_url);
                            str.AppendFormat("</div>");
                            str.AppendFormat(" <div class=\"row-right\">");
                            str.AppendFormat("<div class=\"name\">{0}</div>", resp.result.list[i].title);
                            str.AppendFormat("<div class=\"price\">");
                            str.AppendFormat(" <span class=\"money-flag\">￥</span><span class=\"sale-price\">{0}</span>", resp.result.list[i].price.toFixed(1));
                            str.AppendFormat("<span class=\"pre-price\">￥{0}</span>", resp.result.list[i].quote_price.toFixed(1));
                            str.AppendFormat("</div>");
                            str.AppendFormat("<div class=\"info\">");
                            str.AppendFormat("<span class=\"category-name\">{0}</span>", resp.result.list[i].category_name);
                            if (resp.result.list[i].category_name != '') {
                                str.AppendFormat("<span class=\"sale-count\">{0}</span>", resp.result.list[i].sale_count);
                            } else {
                                str.AppendFormat("<span class=\"Yellow\">{0}</span>", resp.result.list[i].sale_count);
                            }
                            str.AppendFormat("<span class=\"tagg\">人气</span>");
                            str.AppendFormat("</div>")

                            str.AppendFormat("</div>");
                            str.AppendFormat(" </div>");
                        }

                        $("#btnMore").before(str.ToString());

                        if (pageIndex == 1 && resp.result.list.length < parseInt(pageSize)) {
                            $("#btnMore").val("没有更多了");
                        }

                    }
                    else {

                        if (pageIndex == 1 && resp.result.list.length == 0) {
                            $(".nodata").show();
                            $("#courseList").html("<input  id=\"btnMore\" type=\"button\"  value=\"显示更多...\" class=\"btn btn-default\" onclick=\"LoadMore()\"/>");
                            $("#btnMore").hide();

                        }

                        if (pageIndex > 1 && resp.result.list.length == 0) {
                            $(".nodata").hide();
                            $("#btnMore").show();
                            $("#btnMore").val("没有更多了");
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
            $("#courseList").html("<input  id=\"btnMore\" type=\"button\"  value=\"显示更多...\" class=\"btn btn-default\" onclick=\"LoadMore()\"/>");
        }

    </script>
</asp:Content>
