<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Iluxday/Master.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Iluxday.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    为TA点赞
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/userList.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .nodata
        {
            text-align: center;
            font-size: 24px;
        }
        .row + .row
        {
            margin-top: -10px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .summary .row .col .wordbar .dianzanImg
        {
            width: 122px;
            height: 43px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .avatar .name .aixin
        {
            position: absolute;
            top: 3px;
            left: 10px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .summary .row .col .wordbar .wordStyle
        {
            position: absolute;
            top: 237px;
            left: 47px;
            font-size: 3px;
            width: 84px;
            height: 24px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }
        .col
        {
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .summary .row .col .wordbar
        {
            height: 48px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .avatar img.user
        {
            width: 100%;
            height: 212px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .avatar .name .tubiaoImg
        {
            width: 32px;
            height: 45px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .avatar .name .spanStyle
        {
            position: absolute;
            top: 20px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .summary .row .col .wordbar .wordStyle
        {
            font-size: 10px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .wrapBtn a
        {
            padding: 5px 27px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .summary .row .col .wordbar .radius
        {
            font-size: 12px;
        }
        .wrapPlayer
        {
            background-size: 100% 100%;
        }
        .wrapPlayer .contact
        {
            margin-top: 30px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .avatar .name .spanStyle
        {
            font-size: 12px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .avatar .name .aixin
        {
            left: 7px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .avatar .name .spanStyle
        {
            top: 18px;
        }
        .wrapPlayer .wrapUserList .row .userItem .colContent .avatar .name .spanStyle
        {
            left: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapPlayer">
        <div class="header">
            <img src="images/top.png" alt="">
        </div>
        <div class="sologen">
            点赞您心中最好的微信！
        </div>
        <div class="search">
            <input type="text" placeholder="输入参赛者名字或编号" id="txtKeyWord">
            <span id="btnSearch">搜索</span>
        </div>
        <div class="wrapBtn mTop15 mBottom20">
            <a href="javascript:;" class="pageBtn" id="btnSort_Time">最新上传</a> <a href="javascript:;"
                class="pageBtn mLeft6" id="btnSort_Rank">点赞排名</a>
        </div>
        <div class="wrapUserList" id="objlist">
        </div>
        <div class="wrapBtn mTop15 mBottom20">
            <a href="javascript:;" class="pageBtn" id="btnPre">上一页</a> <a href="javascript:;"
                class="pageBtn mLeft6" id="btnNext">下一页</a>
        </div>
        <div class="contact">
            扫描二维码<br />
            获取活动最新进程<br />
            了解奖品领取方法<br />
            咨询活动相关事项
        </div>
        <div class="erweima">
            <img src="images/erweima.png" class="erweimaImg0" />
        </div>
        <div class="intro">
            “爱奢汇”-专业跨境进口电商平台，精致生活，<br />
            我们只做最赞的！<br />
            <span class="">长按二维码，关注爱奢汇马上明白</span><br />
            <span class="zhubanfang">活动主办方：跨境电商爱奢汇</span>
        </div>
        <div class="bottom">
            <div class="row">
                <div class="col borderLine pLeft0" onclick="window.location.href='Index.aspx'">
                    <i class="iconfont icon-shouye shouye"></i>
                </div>
                <div class="col col-80 pLeft0 pRight0">
                    <div class="row pLeft0 pRight0">
                        <div class="col borderLine" onclick="window.location.href='Rule.aspx'">
                            活动规则
                        </div>
                        <div class="col borderLine" onclick="window.location.href='SignUp.aspx'">
                            <%=signUpText %>
                        </div>
                        <div class="col pRight0" onclick="window.location.href='List.aspx'">
                            为TA点赞
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1; //第几页
        var pagesize = 12; //每页显示几条数据
        var sort = ""; //排序

        //页面加载
        $(document).ready(function () {
            if (sessionStorage.getItem("pageIndex") != null) {
                pageindex = parseInt(sessionStorage.getItem("pageIndex"));
            }
            if (sessionStorage.getItem("sort") != null) {
                sort = sessionStorage.getItem("sort");
            }
            //加载数据
            LoadData();

            //事件绑定
            //下一页
            $("#btnNext").click(function () {
                $("#btnNext").html("加载中...");
                pageindex++;
                sessionStorage.setItem("pageIndex", pageindex);
                LoadData();

            });

            //上一页
            $("#btnPre").click(function () {
                $("#btnPre").html("加载中...");
                pageindex--;
                if (pageindex < 1) {
                    pageindex = 1;
                }
                sessionStorage.setItem("pageIndex", pageindex);
                LoadData();

            });
            //搜索按钮
            $("#btnSearch").click(function () {

                Search();


            });
            //按时间排序
            $("#btnSort_Time").click(function () {

                sort = "time_desc";
                sessionStorage.setItem("sort", sort);
                Search();


            });
            //按票数排序
            $("#btnSort_Rank").click(function () {
                sort = "rank_asc";
                sessionStorage.setItem("sort", sort);
                Search();


            });


        });




        //
        //加载数据
        function LoadData() {
            $.ajax({
                type: 'post',
                url: 'Handler.ashx',
                data: { Action: 'GetVoteObjectVoteList', pageindex: pageindex, pagesize: pagesize, keyword: $("#txtKeyWord").val(), sort: sort },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.list.length; i++) {
                        var link = "";
                        var rank = 4;
                        if (i % 2 == 0) {
                            if (resp.list[i].rank == 1) {
                                rank = 1;
                            }
                            else if (resp.list[i].rank == 2) {

                                rank = 2;
                            }
                            else if (resp.list[i].rank == 3) {
                                rank = 3;
                            }
                            link = "window.location.href='Detail.aspx?id=" + resp.list[i].id + "'";
                            //行开始标签
                            str.AppendFormat('<div class="row">');

                            //左
                            str.AppendFormat('<div class="col-50 userItem" onclick="{0}">', link);
                            str.AppendFormat('<div class="colContent">');
                            str.AppendFormat('<div class="avatar">');
                            str.AppendFormat('<img src="{0}" class="user" alt="">', resp.list[i].headimg);

                            str.AppendFormat('<div class="name">');

                            str.AppendFormat('<img src="images/tubiao_03.png" class="tubiaoImg"/>');
                            str.AppendFormat('<i class="iconfont icon-aixin aixin"></i>');

                            str.AppendFormat('<span class="spanStyle">{0}</span>', resp.list[i].votecount);
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');

                            str.AppendFormat('<div class="summary">');
                            str.AppendFormat('<div class="row">');
                            str.AppendFormat('<div class="col">');
                            str.AppendFormat('<div class="wordbar">');
                            str.AppendFormat('<div class="radius">最赞<br/>理由</div>');
                            str.AppendFormat('<img src="images/dianzan_07.png" class="dianzanImg"/>');
                            str.AppendFormat('<div class="wordStyle">{0}</div>', resp.list[i].intro);

                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');
                            str.AppendFormat(' </div>');

                            str.AppendFormat('<div class="row">');
                            str.AppendFormat('<div class="col">');
                            str.AppendFormat('{0}', resp.list[i].name);
                            str.AppendFormat('</div>');
                            str.AppendFormat('<div class="col">');
                            str.AppendFormat('第{0}号', resp.list[i].number);
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');
                            str.AppendFormat('<div class="wrapBtn">');
                            str.AppendFormat('<a href="javascript:;" class="colorFFF">为TA点赞<i class="icon iconfont icon-aixin"></i> </a>');
                            str.AppendFormat('</div>');
                            str.AppendFormat(' </div>');
                            str.AppendFormat('</div>');
                            //



                            //右
                            if (resp.list[i + 1] != undefined) {

                                if (resp.list[i + 1].rank == 1) {
                                    rank = 1;
                                }
                                else if (resp.list[i + 1].rank == 2) {

                                    rank = 2;
                                }
                                else if (resp.list[i + 1].rank == 3) {
                                    rank = 3;
                                }
                                else if (resp.list[i + 1].rank == 4) {
                                    rank = 4;
                                }
                                link = "window.location.href='Detail.aspx?id=" + resp.list[i + 1].id + "'";


                                //
                                str.AppendFormat('<div class="col-50 userItem" onclick="{0}">', link);
                                str.AppendFormat('<div class="colContent">');
                                str.AppendFormat('<div class="avatar">');
                                str.AppendFormat('<img src="{0}" class="user" alt="">', resp.list[i + 1].headimg);

                                str.AppendFormat('<div class="name">');

                                str.AppendFormat('<img src="images/tubiao_03.png" class="tubiaoImg"/>');
                                str.AppendFormat('<i class="iconfont icon-aixin aixin"></i>');
                                str.AppendFormat('<span class="spanStyle">{0}</span>', resp.list[i + 1].votecount);
                                str.AppendFormat('</div>');
                                str.AppendFormat('</div>');

                                str.AppendFormat('<div class="summary">');
                                str.AppendFormat('<div class="row pAll0">');
                                str.AppendFormat('<div class="col pAll0">');
                                str.AppendFormat('<div class="wordbar">');
                                str.AppendFormat('<div class="radius">最赞<br/>理由</div>');
                                str.AppendFormat('<img src="images/dianzan_07.png" class="dianzanImg"/>');
                                str.AppendFormat('<div class="wordStyle">{0}</div>', resp.list[i + 1].intro);

                                str.AppendFormat('</div>');
                                str.AppendFormat('</div>');
                                str.AppendFormat(' </div>');

                                str.AppendFormat('<div class="row">');
                                str.AppendFormat('<div class="col">');
                                str.AppendFormat('{0}', resp.list[i + 1].name);
                                str.AppendFormat('</div>');
                                str.AppendFormat('<div class="col">');
                                str.AppendFormat('第{0}号', resp.list[i + 1].number);
                                str.AppendFormat('</div>');
                                str.AppendFormat('</div>');
                                str.AppendFormat('</div>');
                                str.AppendFormat('<div class="wrapBtn">');
                                str.AppendFormat('<a href="javascript:;" class="colorFFF">为TA点赞<i class="icon iconfont icon-aixin"></i> </a>');
                                str.AppendFormat('</div>');
                                str.AppendFormat(' </div>');
                                str.AppendFormat('</div>');
                                //

                            }
                            //右


                            //
                            //

                            //行结束标签
                            str.AppendFormat(' </div>');



                        }


                    };

                    var listHtml = str.ToString();
                    if (listHtml == "") {
                        $("#objlist").html("<div class=\"nodata\">没有数据</div>");

                        $(window).scrollTop(0);
                    }
                    else {
                        $("#objlist").html(str.ToString());
                        $(window).scrollTop(0);
                    }





                },
                complete: function () {
                    $("#btnPre").html("上一页");
                    $("#btnNext").html("下一页");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        layermsg("加载超时，请刷新页面");

                    }
                }
            });




        }

        //


        //搜索
        function Search() {
            pageindex = 1;
            sessionStorage.setItem("pageIndex", pageindex);
            $("#ulobjlist").html("");
            LoadData();
        }


    </script>
    <script type="text/javascript">
        //分享
        var shareTitle = "【爱奢汇】秀最赞微信，赢最赞时尚大奖";
        var shareDesc = "秀最赞微信，赢最赞时尚大奖。选取朋友圈获赞微信内容上传，即表示报名成功，小伙伴们可以火热开启拉票啦！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Iluxday/images/logo.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
