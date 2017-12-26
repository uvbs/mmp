<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Youzheng.Index.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/lib/bootstrap/3.3.4/css/bootstrap.css" rel="stylesheet" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/css/exam/all.min.css" rel="stylesheet" />
    <link href="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/lib/swiper/css/swiper.min.css" rel="stylesheet" />
    <style type="text/css">
        body {
            background-color: #F0F0F0;
            margin: 0;
            overflow: auto;
            font-family: 'STHeiti','Microsoft YaHei',Helvetica,Arial,sans-serif;
        }

        html, body, div, span, applet, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, big, cite, code, del, dfn, em, img, ins, kbd, q, s, samp, small, strike, strong, sub, sup, tt, var, b, i, u, center, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, table, caption, tbody, tfoot, thead, tr, th, td, article, aside, canvas, details, embed, fieldset, figure, figcaption, footer, header, hgroup, menu, nav, output, ruby, section, summary, time, mark, audio, video {
            vertical-align: middle;
        }

        .top {
            padding-top: 5px;
            padding-left: 5px;
            height: 40px;
            top: 0px;
            width: 100%;
            z-index: 1000;
            opacity: 0.98;
        }

        #txtKeyWord {
            height: 30px;
            width: 100%;
            border-radius: 25px;
            border: 1px solid #E3E1DE;
            position: relative;
        }

        .top-left {
            float: left;
            width: 20%;
        }

            .top-left img {
                width: 90%;
                max-height: 30px;
            }

        .top-midd {
            float: left;
            padding-left: 10px;
            width: 75%;
        }

        .top-right {
            float: left;
            padding-left: 10px;
            position: relative;
            right: 25px;
        }

            .top-right img {
                height: 28px;
                padding-top: 5px;
                position: absolute;
                left: 0px;
            }

        #txtKeyWord {
            padding-left: 10px;
        }





        .barbox {
            text-align: center;
            margin-left: 0px;
            margin-right: 0px;
            background-color: #fff;
            display: inline-block;
            padding: 5px;
        }

        .col-xs-3 {
            background: #fff;
            padding-bottom: 6px;
            padding-top: 5px;
        }

            .col-xs-3 p {
                overflow: hidden;
                white-space: nowrap;
            }

            .col-xs-3 img {
                height: 50px;
                width: 50px;
                border-radius: 50px !important;
            }

        .midd-img {
        }

            .midd-img img {
                width: 100%;
                margin-top: 10px;
            }

        .courseList {
            margin-top: 10px;
            background: #fff;
        }

        .listtitle {
            padding: 5px;
        }

            .listtitle .shu {
                font-weight: bold;
                font-size: 18px;
                color: #ff9e00;
            }

            .listtitle .title {
                font-size: 16px;
                color: #333;
            }

        .clists {
            padding: 0px 5px 0px 5px;
            min-height: 85px;
            margin-bottom: 15px;
            border-bottom: 1px solid #F0F0F0;
        }

        .col-xs-4 {
            padding-left: 0px;
            padding-right: 0px;
        }

            .col-xs-4 img {
                width: 100%;
                height: 75px;
            }

        .col-xs-8 .ctitle {
            font-size: 16px;
            color: #333;
            overflow: hidden;
            text-overflow: ellipsis;
            -o-text-overflow: ellipsis;
            -webkit-text-overflow: ellipsis;
            -moz-text-overflow: ellipsis;
            white-space: nowrap;
        }

        .col-xs-8 {
            padding-right: 0px;
            padding-left: 15px;
        }

            .col-xs-8 .midtext {
                margin-top: 6px;
                margin-bottom: 6px;
            }

        .cPrice {
            color: #FF8e00;
            font-size: 18px;
        }

        .cicon {
            color: #FF8e00;
        }

        .oPrice {
            color: #969595;
            text-decoration: line-through;
            margin-left: 20px;
        }

        .scount {
            margin-left: 30px;
            color: #FF8e00;
        }

        .buttonimg {
            margin-top: 10px;
            margin-bottom: 80px;
            background: #fff;
            text-align: center;
        }

            .buttonimg img {
                width: 100%;
            }


        .foot {
            margin: 0;
            filter: alpha(Opacity=96);
            -moz-opacity: 0.96;
            opacity: 0.96;
            width: 100%;
            text-align: center;
        }

            .foot img {
                width: 25px;
            }

            .foot .col {
                margin-top: 2px;
                padding: 0px;
            }

        .study {
            color: #A09999;
        }

        .icon {
            width: 1em;
            height: 1em;
            vertical-align: -0.15em;
            fill: currentColor;
            overflow: hidden;
        }

        .swiper-slide {
            text-align: center;
            font-size: 18px;
            background: #F0F0F0;
            display: -webkit-box;
            display: -ms-flexbox;
            display: -webkit-flex;
            display: flex;
            -webkit-box-pack: center;
            -ms-flex-pack: center;
            -webkit-justify-content: center;
            justify-content: center;
            -webkit-box-align: center;
            -ms-flex-align: center;
            -webkit-align-items: center;
            align-items: center;
        }

        .swiper-container {
            max-width: 100%;
            height: 140px;
            max-height: 100%;
        }

        .htmleaf-container {
        }

        @media screen and (min-width: 320px) {
            .swiper-container {
                width: 320px;
            }
        }

        @media screen and (min-width: 360px) {
            .swiper-container {
                width: 360px;
            }
        }

        @media screen and (min-width: 375px) {
            .swiper-container {
                width: 375px;
            }
        }

        @media screen and (min-width: 414px) {
            .swiper-container {
                width: 414px;
            }
        }

        .rank1 {
            width: 25px;
            height: 26px;
            background: #FF4141;
            color: #fff;
            font-size: 20px;
            text-align: center;
            line-height: 30px;
            position: absolute;
            top: 0px;
        }

            .rank1:after {
                border: 12px solid transparent;
                border-top: 5px solid #FF4141;
                margin-top: 26px;
                width: 25px;
                position: absolute;
                content: ' ';
                left: 0px;
            }

        .rank2 {
            width: 25px;
            height: 26px;
            background: #EF841A;
            color: #fff;
            font-size: 20px;
            text-align: center;
            line-height: 30px;
            position: absolute;
            top: 0px;
        }

            .rank2:after {
                border: 12px solid transparent;
                border-top: 5px solid #EF841A;
                margin-top: 26px;
                width: 25px;
                position: absolute;
                content: ' ';
                left: 0px;
            }

        .rank3 {
            width: 25px;
            height: 26px;
            background: #00AFFF;
            color: #fff;
            font-size: 20px;
            text-align: center;
            line-height: 30px;
            position: absolute;
            top: 0px;
        }

            .rank3:after {
                border: 12px solid transparent;
                border-top: 5px solid #00AFFF;
                margin-top: 26px;
                width: 25px;
                position: absolute;
                content: ' ';
                left: 0px;
            }

        .orange {
            color: #FF7E00 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="top">
        <div class="top-left">
            <img src="../Images/logo.png" />
        </div>
        <div class="top-midd">
            <input type="text" placeholder="&nbsp;&nbsp;趣谈古诗研究解析论" id="txtKeyWord" />
        </div>
        <div class="top-right">
            <img src="../Images/search1.png" id="btnSearch" />
        </div>
        <div class="clear-both">
        </div>
    </div>

    <div class="slides">
        <div class="slidebox">
            <div class="htmleaf-container">
                <div class="swiper-container">
                    <div class="swiper-wrapper">

                        <%
                        
                            for (int i = 0; i < slideList.Count; i++)
                            {
                                if (string.IsNullOrEmpty(slideList[i].ImageUrl)) continue;
                                Response.Write(string.Format(" <div data-link=\"{1}\" class=\"swiper-slide\" style=\"background-image:url({0});background-size:100% auto;background-repeat: no-repeat;\"></div>", slideList[i].ImageUrl, slideList[i].Link));
                            }
                        %>
                    </div>
                    <!-- Add Pagination -->
                    <div class="swiper-pagination"></div>
                </div>
            </div>
        </div>
    </div>

    <div class="barbox">
        <%
            foreach (var item in barList)
            {
        %>
        <div class="col-xs-3">
            <%
                if (!string.IsNullOrEmpty(item.ToolBarImage))
                {

                    if (!string.IsNullOrEmpty(item.IcoColor))
                    {
                      
            %>
            <div style="height: 50px; font-size: 50px;">
                <a href="javascript:;" onclick="goNavUrl('<%=item.ToolBarTypeValue %>')">
                    <svg class="icon" style="color: <%=item.IcoColor%>;" aria-hidden="true">
                        <use xlink:href="#<%=item.ToolBarImage.Replace("iconfont ","") %>"></use>
                    </svg>
                </a>
            </div>
            <%
                    }
                    else
                    {
            %>
            <div style="height: 50px; font-size: 50px;">
                <a href="javascript:;" onclick="goNavUrl('<%=item.ToolBarTypeValue %>')">
                    <svg class="icon" aria-hidden="true">
                        <use xlink:href="#<%=item.ToolBarImage.Replace("iconfont ","") %>"></use>
                    </svg>
                </a>
            </div>
            <%
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.ImageUrl))
                    {
            %>
            <a href="javascript:;" onclick="goNavUrl('<%=item.ToolBarTypeValue %>')">
                <img src="<%=item.ImageUrl %>" />
            </a>
            <%
                    }

                }
            %>

            <p><%=item.ToolBarName %></p>
        </div>
        <%
            }     
            
        %>

        <%--<div class="col-xs-3">
            <img src="../images/bars.png" class="img-circle" />
            <p>互联网</p>
        </div>
        <div class="col-xs-3">
            <img src="../images/bars.png" class="img-circle" />
            <p>经营</p>
        </div>
        <div class="col-xs-3">
            <img src="../images/bars.png" class="img-circle" />
            <p>互联网</p>
        </div>
        <div class="col-xs-3">
            <img src="../images/bars1.png" class="img-circle" />
            <p>互联网</p>
        </div>
        <div class="col-xs-3">
            <img src="../images/bars1.png" class="img-circle" />
            <p>经营</p>
        </div>
        <div class="col-xs-3">
            <img src="../images/bars1.png" class="img-circle" />
            <p>互联网</p>
        </div>
        <div class="col-xs-3">
            <img src="../images/bars1.png" class="img-circle" />
            <p>经营</p>
        </div>--%>
    </div>

    <div class="midd-img">
        <img src="../images/oneyuan.png" />
    </div>


    <div class="courseList">
        <div class="listtitle">
            <span class="shu">|</span>
            <span class="title">热门排行榜</span>
        </div>
        <div id="lists">
            <%--<div class="clists">
                <div class="col-xs-4">
                    <img src="../images/test.png" />
                </div>
                <div class="col-xs-8">
                    <div class="ctitle">趣谈古诗研究解析论</div>
                    <div class="midtext">
                        <span class="cicon">￥</span>
                        <span class="cPrice">360.0</span>
                        <span class="oPrice">￥420.0</span>
                    </div>
                    <div>
                        <span style="color: #A09999">线上学习</span>
                        <span class="scount">43</span>
                        <span>人学习</span>
                    </div>
                </div>
            </div>--%>
        </div>



    </div>






    <div class="buttonimg">
        <img src="../images/tese1.png" />
    </div>

    <div class="row foot">
        <a class="col orange" href="/Customize/YouZheng/Index/Index.aspx">
            <img src="../images/homeselect.png" />
            <br />
            首页</a>

        <a class="col" href="/Customize/YouZheng/Course/List.aspx">
            <img src="../images/book.png" />
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
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
    <script src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/lib/swiper/js/swiper.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var url = '/serv/api/mall/product.ashx';
        var type = 'Course';
        var keywords = '';
        $(function () {

            //加载课程列表
            loadCourseList(keywords);

            $('#btnSearch').click(function () {
                keywords = $('#txtKeyWord').val();
                loadCourseList(keywords);
            });

            $('.swiper-slide').click(function () {
                var link = $(this).attr('data-link');
                if (link) {
                    window.location.href = link;
                }
            });
            var swiper = new Swiper('.swiper-container', {
                pagination: '.swiper-pagination',
                paginationClickable: true,
                autoplay: 2500,                loop: true
            });

            $(document).on('click', '.clists', function () {
                var dataid = $(this).attr('data-id');
                if (dataid) {
                    window.location.href = '../course/detail.aspx?id=' + dataid;
                }
            });


        });

        function goNavUrl(value) {
            if (value) {
                window.location.href = value;
            }
        }



        function loadCourseList(keywords) {

            $.ajax({
                type: 'POST',
                url: url,
                data: { Action: 'List', pageindex: 1, pagesize: 10, type: type, has_cate_name: '1', sort: 'sales_volume', keyword: keywords },
                dataType: 'json',
                success: function (resp) {
                    $('#lists').html('');
                    var str = new StringBuilder();
                    if (resp.totalcount > 0) {
                        for (var i = 0; i < resp.list.length; i++) {
                            if (i > 2) continue;
                            str.AppendFormat('<div data-id={0} class="clists">', resp.list[i].product_id);
                            str.AppendFormat('<div class="col-xs-4">');
                            str.AppendFormat('<img src="{0}" />', resp.list[i].img_url);
                            if (i <= 2) {
                                str.AppendFormat('<div class="rank{0}">{0}</div>', i + 1);
                            }
                            str.AppendFormat('</div>');
                            str.AppendFormat(' <div class="col-xs-8">');
                            str.AppendFormat('<div class="ctitle">{0}</div>', resp.list[i].title);
                            str.AppendFormat('<div class="midtext">');
                            str.AppendFormat('<span class="cicon">￥</span>');
                            str.AppendFormat('<span class="cPrice">{0}</span>', resp.list[i].price);
                            str.AppendFormat('<span class="oPrice">￥{0}</span>', resp.list[i].quote_price);
                            str.AppendFormat('</div>');
                            str.AppendFormat('<div>');
                            str.AppendFormat(' <span class="study">{0}</span>', resp.list[i].category_name == '' ? '显示学习' : resp.list[i].category_name.toString().substr(0, 10));
                            str.AppendFormat('<span class="scount">{0}</span>', resp.list[i].sale_count);
                            str.AppendFormat('<span class="study">人在学</span>');
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');
                        }
                        $('#lists').append(str.ToString());
                    }
                }
            });

        }


    </script>
    <%= new ZentCloud.BLLJIMP.BLL().GetIcoScript() %>
</asp:Content>
