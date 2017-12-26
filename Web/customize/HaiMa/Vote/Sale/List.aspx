<%@ Page Title="" Language="C#" MasterPageFile="~/customize/HaiMa/Vote/Sale/Master.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.Vote.Sale.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    海马真英雄-选手列表
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .voteobjname
        {
            font-weight: bold;
        }
        .nodata
        {
            color: White;
            text-align: center;
            font-size: 20px;
            margin-top: 20px;
            margin-bottom: 20px;
        }
        .wrapManagers .wrapUserList .row .userItem .avatar img.user
        {
            height: auto;
            min-width: 100%;
        }
        .wrapManagers .wrapUserList .row .userItem .avatar img.user
        {
            max-width: 100%;
        }
        .wrapManagers .wrapUserList .row .col-50 .colContent .ranking
        {
            right: 5px;
        }
        #ddlPage
        {
            min-width: 50px;
            height: 34px;
            border-radius: 5px 5px 5px 5px;
        }
        .mLeft6
        {
            margin-left: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapManagers mBottom40">
        <div class="header">
            <img src="images/header.jpg" alt="">
        </div>
        <div class="search">
            <input type="text" placeholder="输入名字" id="txtKeyWord" placeholder="">
            <span id="btnSearch">搜索</span>
        </div>
        <div class="sologen">
            <img src="images/wenzi.png" alt="">
        </div>
        <div class="wrapUserList" id="objlist">
        </div>
        <div class="wrapBtn mTop10 mBottom10">
            <a href="javascript:;" class="pageBtn" id="btnPre">上一页</a>
            <select id="ddlPage">
            </select>
            <a href="javascript:;" class="pageBtn" id="btnNext">下一页</a>
        </div>
        <div class="wrapBottomMsg">
            前<span>200</span>名进入评委评分阶段
        </div>
        <div class="footer">
            <img src="images/footer.png" alt="">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1; //第几页
        var pagesize = 12; //每页显示几条数据
        //页面加载
        $(document).ready(function () {
            if (sessionStorage.getItem("pageIndex") != null) {
                pageindex = parseInt(sessionStorage.getItem("pageIndex"));
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

            //页码改变
            $("#ddlPage").change(function () {

                $("#txtKeyWord").val("");
                pageindex = parseInt($(this).val());
                sessionStorage.setItem("pageIndex", pageindex);
                LoadData();


            })




        });

        //加载数据
        function LoadData() {
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: 'GetVoteObjectVoteList', pageindex: pageindex, pagesize: pagesize, keyword: $("#txtKeyWord").val() },
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
                            str.AppendFormat('<div class="colContent"> ');
                            str.AppendFormat('<div class="avatar"> ');
                            str.AppendFormat('<img src="{0}" class="user" alt=""> ', resp.list[i].headimg);
                            str.AppendFormat('<div class="dz">');
                            str.AppendFormat('<div> ');
                            str.AppendFormat('<i class="iconfont icon-dianzan"></i>');
                            str.AppendFormat('<span>{0}</span> ', resp.list[i].votecount);
                            str.AppendFormat('</div> ');
                            str.AppendFormat('</div>');
                            str.AppendFormat('<div class="name"> ');
                            str.AppendFormat('{0}号', resp.list[i].number);
                            str.AppendFormat('</div> ');
                            str.AppendFormat('</div> ');
                            str.AppendFormat('<div class="summary"><span class="voteobjname">{0}</span></br>{1}</div> ', resp.list[i].name, resp.list[i].intro);
                            str.AppendFormat(' <div class="wrapBtn">');
                            str.AppendFormat(' <a href="javascript:;">为TA加油</a> ');
                            str.AppendFormat('</div> ');
                            str.AppendFormat(' <img src="images/h{0}.png" class="crown" alt="">', rank);
                            str.AppendFormat('<div class="ranking"> ');
                            str.AppendFormat('{0}', resp.list[i].rank);
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');

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
                                str.AppendFormat('<div class="col-50 userItem" onclick="{0}">', link);
                                str.AppendFormat('<div class="colContent"> ');
                                str.AppendFormat('<div class="avatar"> ');
                                str.AppendFormat('<img src="{0}" class="user" alt=""> ', resp.list[i + 1].headimg);
                                str.AppendFormat('<div class="dz">');
                                str.AppendFormat('<div> ');
                                str.AppendFormat('<i class="iconfont icon-dianzan"></i>');
                                str.AppendFormat('<span>{0}</span> ', resp.list[i + 1].votecount);
                                str.AppendFormat('</div> ');
                                str.AppendFormat('</div>');
                                str.AppendFormat('<div class="name"> ');
                                str.AppendFormat('{0}号', resp.list[i + 1].number);
                                str.AppendFormat('</div> ');
                                str.AppendFormat('</div> ');
                                str.AppendFormat('<div class="summary"><span class="voteobjname">{0}</span></br>{1}</div> ', resp.list[i + 1].name, resp.list[i + 1].intro);
                                str.AppendFormat(' <div class="wrapBtn">');
                                str.AppendFormat(' <a href="javascript:;">为TA加油</a> ');
                                str.AppendFormat('</div> ');
                                str.AppendFormat(' <img src="images/h{0}.png" class="crown" alt="">', rank);
                                str.AppendFormat('<div class="ranking"> ');
                                str.AppendFormat('{0}', resp.list[i + 1].rank);
                                str.AppendFormat('</div>');
                                str.AppendFormat('</div>');
                                str.AppendFormat('</div>');

                            }
                            //右

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
                    $("#txtKeyWord").attr("placeholder", "输入编号或姓名");
                    if (resp.list.length > 0) {
                        LoadPageList(resp.totalcount);
                    }
                    else {

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

        //搜索
        function Search() {
            pageindex = 1;
            sessionStorage.setItem("pageIndex", pageindex);
            $("#objlist").html("");
            LoadData();
        }


        ///加载页数
        function LoadPageList(totalCount) {
            var strPageList = new StringBuilder();
            for (var i = 1; i <= Math.ceil(totalCount / pagesize); i++) {
                strPageList.AppendFormat('<option value="{0}">第{0}页</option>', i);
            }
            $("#ddlPage").html(strPageList.ToString());
            $("#ddlPage").val(pageindex);

        }

    </script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "海马精英营销大赛,大奖等你来!",
                desc: "海马精英营销大赛,大奖等你来!",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>/customize/HaiMa/images/logo.jpg"
            })
        })
    </script>
</asp:Content>
