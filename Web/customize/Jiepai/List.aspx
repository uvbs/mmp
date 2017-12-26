<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Jiepai/Master.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Jiepai.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    全球街拍选手列表
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/list.css" rel="stylesheet" />
    <style>
        .nodata {
                color:black;
             }
    </style>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <header class="content">
    <header class="row">
        <div class="col">
            <img src="images/header/log.png" class="full-image">
        </div>
    </header>
    <header class="row padding-add-center">
        <div class="col col-33 col-center">
            <img src="images/header/switzerland.png" class="full-image">
        </div>
        <div class="col col-33 col-center">
            <img src="images/header/newzealand.png" class="full-image">
        </div>
        <div class="col col-33 col-center">
            <img src="images/header/victoria.png" class="full-image">
        </div>
    </header>
    <header class="row padding-add-center">
        <div class="col col-33 col-center">
            <img src="images/header/news.png" class="full-image">
        </div>
        <div class="col col-33 col-center">
            <img src="images/header/lillydale.png" class="full-image">
        </div>
        <div class="col col-33 col-center">
            <img src="images/header/qee.png" class="full-image">
        </div>
    </header>
</header>
<section class="content">
    <div class="row">
        <div class="col">
            <img src="images/list/log.png" class="full-image padding-add-center">
        </div>
    </div>
    <div class="row">
        <div class="col col-70 col-offset-10">
            <input type="text" placeholder="输入编号" id="txtKeyWord" class="full-image">
        </div>
        <div class="col col-25">
            <button class="btn-diy font" id="btnSearch">搜 索</button>
        </div>
    </div>
    <div class="text-center">
        <div class="div-btn">
            <button class="btn-diy-2 font" id="btnSort_Time">最新上传</button>
        </div>
        <div class="div-btn">
            <button class="btn-diy-2 font" id="btnSort_Rank">投票排名</button>
        </div>
    </div>
</section>
<section class="content text-center" id="player-pic">
</section>
<section class="content">
    <div class="text-center">
        <div class="div-btn">
            <button class="btn-diy-3 font" id="btnPre1">
                <img src="images/list/up.png" class="list-icon">
                上一页</button>
        </div>
        <div class="div-btn">
               <button class="btn-diy-3 font" id="btnNext1">
                <img src="images/list/down.png" class="list-icon">
                下一页</button>
        </div>
    </div>
</section>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
    <script src="//cdn.bootcss.com/jquery/2.1.4/jquery.min.js"></script>
    <script type="text/javascript">
        var pageindex = 1; //第几页
        var pagesize = 12 //每页显示几条数据
        var sort = "rank_asc"; //排序
        var count;
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
            $("#btnNext1").click(function () {
                $("#btnNext").html("加载中...");
                var pageNum = (count + pagesize - 1) / pagesize;
                if(pageindex<pageNum){
                    pageindex++;
                }
                sessionStorage.setItem("pageIndex", pageindex);
                LoadData();
            });

            //上一页
            $("#btnPre1").click(function () {
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

        //加载数据
        function LoadData() {
            var firstload = true;
            $.ajax({
                type: 'post',
                url: "Handler.ashx",
                data: { Action: 'GetVoteObjectVoteList', pageindex: pageindex, pagesize: pagesize, keyword: $(txtKeyWord).val(), sort: sort },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    count = resp.totalcount;
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.list.length; i++) {

                        var link = "window.location.href='Detail.aspx?id=" + resp.list[i].id + "'";
                        str.AppendFormat('<div class="bg-divbox">');
                        str.AppendFormat('<div class="bg-div" onclick="{0}">', link);
                        str.AppendFormat('<div class="player-id font"><span>{0}</span>号</div>', resp.list[i].number);
                        //if (resp.list[i].rank <= 3) {
                        //    str.AppendFormat(' <img src="images/list/{0}.png" class="ranking">', resp.list[i].rank);
                        //}

                        str.AppendFormat('<div class="player-view" style="background-image: url({0});">',resp.list[i].headimg);
                        str.AppendFormat('<div class="player-vote font">{0}票</div>', resp.list[i].votecount);
                        str.AppendFormat('</div>');
                        str.AppendFormat(' <span class="margin-top-1">{0}</span>', resp.list[i].address);
                        str.AppendFormat(' <br>');
                        str.AppendFormat('<button class="btn-diy-3 margin-top-1 font">为TA投票</button>');
                        str.AppendFormat('</div>');
                        str.AppendFormat('</div>');


                    };

                    var listHtml = str.ToString();
                    if (listHtml == "") {
                        $("#player-pic").html("<div class=\"nodata\">没有数据</div>");
                        $(window).scrollTop(0);
                    }
                    else {
                        $("#player-pic").html(str.ToString());
                        $(window).scrollTop(200);
                    }
                    if( firstload){
                        $("body").append("<style>.player-view{height:" + $(".player-view").width() * 1.5 + "px}</style>");
                        firstload = false;
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
            $("#ulobjlist").html("");
            LoadData();
        }

    </script>
   
<script src="//cdn.bootcss.com/jquery/2.1.4/jquery.min.js"></script>
<script>
    $("body").append("<style>.player-view{height:" + $(".player-view").width() * 1.5 + "px}</style>")
</script>
    <script>
        //分享
        var shareTitle = "Qee全球街拍探秘项目平台";
        var shareDesc = "参加Qee全球街拍探秘，有大奖在等您！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Jiepai/Images/logo.jpg";
        var shareLink = "http://<%=Request.Url.Host %>/customize/Jiepai/index.aspx";
        //分享
    </script>

</asp:Content>
