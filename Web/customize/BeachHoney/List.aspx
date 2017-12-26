<%@ Page Title="" Language="C#" MasterPageFile="~/customize/BeachHoney/Master.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.BeachHoney.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    沙滩宝贝-为她投票
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            background-color: White;
        }
        .form_a
        {
            width: auto;
            text-align: center;
            margin-left: 30%;
        }
        
        .list
        {
            margin-top: 0px;
            position: relative;
        }
        .left_maskbar
        {
            margin-left: 0px;
        }
        .bottom_maskbar
        {
            margin-right: 0px;
        }
        .top-search input
        {
            width: 100%;
        }
        
        #btnSearch
        {
            width: 55px;
        }
        .headimg
        {
            min-width: 142px;
        }
        #btnNext
        {
            margin-left: 20px;
        }
        .nodata
        {
            text-align: center;
            font-weight: bold;
            font-size: 18px;
        }

        .mask{width:45%;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="image_single">
        <img src="images/header_01.png" alt="" title="" border="0" />
    </div>
    <div class="list form">
        <table width="100%">
            <tr>
                <td>
                    <div class="top-search radius6 ">
                        <input type="text" id="txtKeyWord" placeholder="选手编号或姓名" />
                    </div>
                </td>
                <td>
                    &nbsp;<img src="images/btn_01.png" id="btnSearch" />
                </td>
            </tr>
        </table>
        <div class="page_padding2">
            <div class="menu1">
                <ul>
                    <li id="li1" class="active"><a href="javascript:void(0)">
                        <img src="images/btn_02.png" alt="" title="" id="btnSort_Time" /></a></li>
                    <li id="li2"><a href="javascript:void(0)">
                        <img src="images/btn_03.png" alt="" title="" id="btnSort_Rank" /></a></li>
                </ul>
            </div>
        </div>
        <div class="menu2">
            <ul id="ulobjlist">
            </ul>
        </div>
        <div class="form_div3">
            <a class="form_a red radius6" id="btnPre">上一页</a> <a class="form_a red radius6" id="btnNext">
                下一页</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
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

                sort = "time";
                sessionStorage.setItem("sort", sort);
                Search();


            });
            //按票数排序
            $("#btnSort_Rank").click(function () {
                sort = "";
                sessionStorage.setItem("sort", sort);
                Search();


            });


        });

        //加载数据
        function LoadData() {
            $.ajax({
                type: 'post',
                url: handlerurl,
                data: { Action: 'GetVoteObjectVoteList', pageindex: pageindex, pagesize: pagesize, keyword: $(txtKeyWord).val(), sort: sort },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.list.length; i++) {
                        var tag = "";
                        if (resp.list[i].rank == 1) {
                            tag = "<tt class=\"right_maskbar\"><b>1</b></tt>";
                        }
                        else if (resp.list[i].rank == 2 || resp.list[i].rank == 3) {

                            tag = "<tt class=\"right_maskbar1\"><b>" + resp.list[i].rank + "</b></tt>";
                        }
                        else {
                            tag = "<tt class=\"right_maskbar2\"><b>" + resp.list[i].rank + "</b></tt>";
                        }
                        //
                        var link = "window.location.href='Detail.aspx?id=" + resp.list[i].id + "'";
                        str.AppendFormat('<li onclick="{0}">', link);
                        str.AppendFormat('<div class="mask">');
                        str.AppendFormat('<tt class="left_maskbar">{0}号</tt>', resp.list[i].number);
                        str.AppendFormat('{0}', tag);
                        str.AppendFormat('</div>');
                        str.AppendFormat('<img src="{0}" alt="" title="" border="0" class="headimg" />', resp.list[i].headimg);
                        str.AppendFormat('<tt class="bottom_maskbar" id="span{1}">{0}票</tt>', resp.list[i].votecount, resp.list[i].id);
                        str.AppendFormat('<span>{0}&nbsp;{1}</span>', resp.list[i].name, resp.list[i].area);
                        str.AppendFormat('<a href="javascript:void(0)">');
                        str.AppendFormat('<img src="images/btn_04.png" />');
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                        //

                    };

                    var listHtml = str.ToString();
                    if (listHtml == "") {
                        $("#ulobjlist").html("<div class=\"nodata\">没有数据</div>");
                        //pageindex--;
                        //sessionStorage.setItem("pageIndex", pageindex);
                        $(window).scrollTop(0);
                    }
                    else {
                        $("#ulobjlist").html(str.ToString());
                        $(window).scrollTop(200);
                    }
                    $("#txtKeyWord").attr("placeholder", "输入选手编号或姓名 已有" + resp.totalcount + "人报名参加");

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
            $("#ulobjlist").html("");
            LoadData();
        }

        //分享
        var shareTitle = "1000人疯抢400000元奖品，看美女拿奖品两不误！快来参加沙滩宝贝微信评选！";
        var shareDesc = "duang~~ 上海热带风暴水上乐园暑期盛大开园，BeachHoney沙滩宝贝隆重登场了，只需一分钟报名，玩乐两不误，海量奖品任你拿！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/beachhoney/images/match_01.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
