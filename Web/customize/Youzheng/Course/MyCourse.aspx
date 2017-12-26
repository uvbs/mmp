<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="MyCourse.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Youzheng.Course.MyCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/css/exam/all.min.css" rel="stylesheet" />

    <style>
        html, body, div, span, applet, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, big, cite, code, del, dfn, em, img, ins, kbd, q, s, samp, small, strike, strong, sub, sup, tt, var, b, i, u, center, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, table, caption, tbody, tfoot, thead, tr, th, td, article, aside, canvas, details, embed, fieldset, figure, figcaption, footer, header, hgroup, menu, nav, output, ruby, section, summary, time, mark, audio, video {
            vertical-align: middle;
        }

        body {
            background-color: #F3F2F1;
            font-size: 14px;
            margin: 0;
            overflow: auto;
            font-family: 'STHeiti','Microsoft YaHei',Helvetica,Arial,sans-serif;
            vertical-align: middle;
        }



        .row {
            margin-top: 10px;
            background-color: white;
            padding-top: 10px;
            padding-bottom: 10px;
            margin-left: 0;
        }

        .row-left {
            width: 120px;
            float: left;
            overflow: auto;
        }

            .row-left img {
                max-width: 100%;
                max-height: 100%;
                height: 85px;
                border-radius: 3px !important;
                -webkit-border-radius: 3px !important;
                -moz-border-radius: 3px !important;
            }

        .row-right {
            float: left;
            padding-left: 8px;
            width: 100%;
        }

        .price {
            padding-top: 6px;
            color: #333;
            font-size:13px;
        }

        .info {
            padding-top: 5px;
        }

        .name {
            font-size: 13px;
            color: #333;
        }

        .time {
            font-size: 14px;
            color: #FF7E00 !important;
        }



        #divList {
            margin-bottom: 60px;
        }

        .row + .row {
            padding-top: 10px;
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
        }

            .foot img {
                width: 25px;
            }

            .foot .col {
                margin-top: 2px;
                padding: 0px;
            }

        .btn-exam {
            background-color: #fff;
            border: 1px solid #F8C38C;
            color: #FB981C;
            height: 35px;
            line-height: 35px;
            border-radius: 3px;
            width: 100%;
        }
        .btn-exam1{
             background-color: #F08619;
            color: #fff;
            height: 35px;
            line-height: 35px;
            border-radius: 3px;
            width: 100%;
        }
        .btn-exam-over {
            background-color: #D6D3CE;
            height: 35px;
            line-height: 35px;
            border-radius: 3px;
            width: 100%;
        }
        .btn-exam-over1 {
            background-color: #E6E6E6;
            color:#BEBEBE;
            height: 35px;
            line-height: 35px;
            border-radius: 3px;
            width: 100%;
        }
        .info {
            vertical-align: middle;
        }

        .info-left {
            width: 55%;
            float: left;
            padding-top:5px;
        }
        .info-left label{
            font-size:13px;
            color:#333;

        }

        .info-right {
            width: 45%;
            float: left;
            text-align: center;
        }

        .notice {
            background-color: #FFFAE9;
            text-align: left;
            padding-left: 5px;
            padding-right: 5px;
            padding-top: 10px;
            padding-bottom: 10px;
            vertical-align: middle;
        }

            .notice img {
                width: 20px;
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
        }

            .foot img {
                width: 25px;
            }

            .foot .col {
                margin-top: 2px;
                padding: 0px;
            }

        .nodata {
            text-align: center;
            margin-top: 100px;
            font-size: 16px;
        }

        .notice-left {
            float: left;
            width: 10%;
        }

        .notice-right {
            float: left;
            width: 90%;
            color: #333;
            font-size:13px;
        }

        .orange {
            color: #FF7E00 !important;
        }

        .list-warp {
            padding-bottom: 20px;
        }
        .Gray{
            color:#C4C4C4 !important;
            font-size:13px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

    <div class="notice">
        <div class="notice-left">
            <img src="../Images/laba.png" /></div>
        <div class="notice-right">
            报名起15天后可参与考试,报名起至90天内未参与考试则视为弃权。
        </div>


    </div>



    <div id="divList">
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


        <a class="col" href="/Customize/YouZheng/Activity/List.aspx">
            <img src="../images/huodong.png" />
            <br />
            活动</a>
        <a class="col" href="/Customize/YouZheng/Activity/List.aspx?isgame=1">
            <img src="../images/jiangbei.png" />
            <br />
            竞赛</a>

        <a class="col orange" href="/customize/comeoncloud/Index.aspx?key=PersonalCenter">
            <img src="../images/myselect.png" />
            <br />
            我的</a>

    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/StringBuilder.Min.js"></script>
    <script>
        var handlerUrl = "/serv/api/course/mylist.ashx";
        var pageIndex = 1;
        var pageSize = 100;


        $(function () {

            Search()

        })

        function Search() {
            layer.open({ type: 2 });
            var par = {

                pageIndex: pageIndex,
                pageSize: pageSize

            };


            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: par,
                dataType: "json",
                success: function (resp) {
                    layer.closeAll();
                    if (resp.result != null && resp.result.list.length > 0) {
                        var str = new StringBuilder();
                        for (var i = 0; i < resp.result.list.length; i++) {

                            switch (resp.result.list[i].status) {
                                case 0://未到考试时间
                                    str.AppendFormat("<div class=\"row list-warp\">");
                                    str.AppendFormat("<div class=\"row-left\">");
                                    str.AppendFormat("<img src=\"{0}@80h_100w_1e_1c\" />", resp.result.list[i].course_img_url);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat(" <div class=\"row-right\">");
                                    str.AppendFormat("<div class=\"name\">已报课程:{0}</div>", resp.result.list[i].course_name.replace('证书类别:', ''));
                                    str.AppendFormat("<div class=\"price\">");
                                    str.AppendFormat("距考试开始日期还有{0}天", resp.result.list[i].day);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("<div class=\"info\">");
                                    str.AppendFormat("<div class=\"info-left\">");
                                    str.AppendFormat("<label>考试时间:{0}分钟</label>", resp.result.list[i].exam_minute);
                                    str.AppendFormat(" </div>");
                                    str.AppendFormat("<div class=\"info-right\">");
                                    str.AppendFormat("<label class=\"btn-exam\" onclick=\"javascript:alert('还未到时间')\">去考试</label>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");


                                    break;
                                case 1://正常考试时间

                                    str.AppendFormat("<div class=\"row\">");
                                    str.AppendFormat("<div class=\"row-left\">");
                                    str.AppendFormat("<img src=\"{0}@80h_100w_1e_1c\" />", resp.result.list[i].course_img_url);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat(" <div class=\"row-right\">");
                                    str.AppendFormat("<div class=\"name\">已报课程:{0}</div>", resp.result.list[i].course_name.replace('证书类别:', ''));
                                    str.AppendFormat("<div class=\"price\">");
                                    str.AppendFormat("距离考试期限还有");
                                    str.AppendFormat("<span class=\"time\">{0}</span>天", resp.result.list[i].day);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("<div class=\"info\">");
                                    str.AppendFormat("<div class=\"info-left\">");
                                    str.AppendFormat("<label>考试时间：</label><label class=\"time\">{0}</label><label>分钟</label>",resp.result.list[i].exam_minute);
                                    str.AppendFormat(" </div>");
                                    str.AppendFormat("<div class=\"info-right\">");
                                    str.AppendFormat("<label class=\"btn-exam1\" onclick=\"window.location.href='/App/Exam/Exam.aspx?id={0}'\">去考试</label>", resp.result.list[i].exam_id);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    break;
                                case 2://已经考过了
                                    str.AppendFormat("<div class=\"row\">");
                                    str.AppendFormat("<div class=\"row-left\">");
                                    str.AppendFormat("<img src=\"{0}@80h_100w_1e_1c\" />", resp.result.list[i].course_img_url);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat(" <div class=\"row-right\">");
                                    str.AppendFormat("<div class=\"name\">已报课程:{0}</div>", resp.result.list[i].course_name.replace('证书类别:', ''));
                                    str.AppendFormat("<div class=\"price\">");
                                    str.AppendFormat("考试日期:");
                                    str.AppendFormat("{0}", resp.result.list[i].time);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("<div class=\"info\">");
                                    str.AppendFormat("<div class=\"info-left\">");
                                    str.AppendFormat("<label>考试时间：{0}分钟</label>", resp.result.list[i].exam_minute);
                                    str.AppendFormat(" </div>");
                                    str.AppendFormat("<div class=\"info-right\">");
                                    str.AppendFormat("<label class=\"btn-exam-over\" onclick=\"window.location.href='/App/Exam/Exam.aspx?id={0}'\">已考完</label>", resp.result.list[i].exam_id);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    break;
                                case 3://缺考
                                    str.AppendFormat("<div class=\"row\">");
                                    str.AppendFormat("<div class=\"row-left\">");
                                    str.AppendFormat("<img src=\"{0}@80h_100w_1e_1c\" />", resp.result.list[i].course_img_url);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat(" <div class=\"row-right\">");
                                    str.AppendFormat("<div class=\"Gray\">已报课程:{0}</div>", resp.result.list[i].course_name.replace('证书类别:', ''));
                                    str.AppendFormat("<div class=\"Gray\">");
                                    str.AppendFormat("考试结束日期::");
                                    str.AppendFormat("{0}", resp.result.list[i].time);
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("<div class=\"info\">");
                                    str.AppendFormat("<div class=\"info-left\">");
                                    str.AppendFormat("<label class=\"Gray\">考试时间:{0}分钟</label>", resp.result.list[i].exam_minute);
                                    str.AppendFormat(" </div>");
                                    str.AppendFormat("<div class=\"info-right\">");
                                    str.AppendFormat("<label class=\"btn-exam-over1\">已缺考</label>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    str.AppendFormat("</div>");
                                    break;
                                default:

                            }


                        }

                        $("#divList").html(str.ToString());


                    }
                    else {

                        //无记录

                        $("#divList").html("<div class=\"nodata\">暂无课程</div>");


                    }



                }
            });



        }

        function LoadMore() {
            pageIndex++;
            Search();

        }

        // 将分钟数量转换为小时和分钟字符串
        function toHourMinute(minutes) {
            if (minutes < 60) {
                return minutes + "分钟";
            }
            var h = (minutes / 60) + "小时";
            return h;
        }



    </script>
</asp:Content>
