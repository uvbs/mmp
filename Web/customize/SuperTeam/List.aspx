<%@ Page Title="" Language="C#" MasterPageFile="~/customize/SuperTeam/Master.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SuperTeam.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    荣耀精英团队
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/voteForHim.css" rel="stylesheet" type="text/css" />
    <style>
        .nodata
        {
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            color: White;
        }
        .row-erweima .col-erweima
        {
            padding: 10px 60px;
        }
        .row-logo .col-logo
        {
            padding: 10px 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapVoteForHim">
        <div class="top">
            <img class="imgWidth" src="image/top.png">
        </div>
        <div class="content">
            <div class="row">
                <div class="col textC colorFFF font17">
                    为Team投票
                </div>
            </div>
            <div class="list list-inset list-reset">
                <label class="item item-input item-reset">
                    <input class="input-reset" type="text" placeholder="输入编号或团队名称" id="txtKeyWord">
                    <div class="search" id="btnSearch">
                        <i class="icon placeholder-icon iconfont icon-search mLeft10"></i>
                    </div>
                </label>
            </div>
            <div id="objlist">

            </div>
            <div class="row row-button-reset">
                <div class="col col-button-reset">
                    <button class="button button-block button-positive button-reset" id="btnPre">
                        上一页
                    </button>
                </div>
                <div class="col col-button-reset">
                    <button class="button button-block button-positive button-reset" id="btnNext">
                        下一页
                    </button>
                </div>
            </div>
        </div>
        <div class="row row-erweima">
            <div class="col col-erweima">
                <img class="imgWidth" src="image/qrcode.png" style="width:100px;height:auto;text-align:center;margin-left:20%;">
            </div>
        </div>
        <div class="row row-logo">
            <div class="col col-logo">
                <img class="imgWidth" src="image/xinwen.png">
            </div>
        </div>
        <div class="bottom">
            <div class="row">
                <div class="col borderLine" onclick="window.location.href='Index.aspx'">
                    <i class="iconfont icon-shixinshouye shouye"></i>
                </div>
                <div class="col col-80">
                    <div class="row">
                        <div class="col borderLine" onclick="window.location.href='Rule.aspx'">
                            参赛细则
                        </div>
                        <div class="col borderLine" onclick="window.location.href='SignUp.aspx'">
                            <%=signUpText %>
                        </div>
                        <div class="col" onclick="window.location.href='Area.aspx'">
                            为Team投票
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
        var area = "";
        //页面加载
        $(document).ready(function () {

            area = ConvertToAreaName(GetParm("areacode"));

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




        });




        //
        //加载数据
        function LoadData() {
            $.ajax({
                type: 'post',
                url: 'Handler.ashx',
                data: { Action: 'GetVoteObjectVoteList', pageindex: pageindex, pagesize: pagesize, keyword: $("#txtKeyWord").val(), sort: sort, area: area },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.list.length; i++) {

                        link = "window.location.href='Detail.aspx?id=" + resp.list[i].id + "'";
                        var className = "row padding20";
                        if (i == 0) {
                            className = "row padding20 mTopZero16";
                        }
                        str.AppendFormat('<div class="{0}" onclick="{1}">', className, link);
                        str.AppendFormat('<div class="col col-reset">');
                        str.AppendFormat('<img class="imgWidth" src="{0}">', resp.list[i].headimg);
                        str.AppendFormat('<div class="no"> NO.{0}</div>', resp.list[i].number);
                        str.AppendFormat('<div class="row row-bottom-reset">');
                        str.AppendFormat('<div class="col col-inside-reset">');
                        str.AppendFormat('{0}', resp.list[i].intro);
                        str.AppendFormat('</div>');
                        str.AppendFormat('<div class="col col-inside-reset">');
                        str.AppendFormat(' {0}', resp.list[i].name);
                        str.AppendFormat('</div>');
                        str.AppendFormat('<div class="col col-inside-reset">');
                        str.AppendFormat('<i class="icon iconfont icon-"></i>{0}赞', resp.list[i].votecount);
                        str.AppendFormat('</div>');
                        str.AppendFormat('</div>');
                        str.AppendFormat('</div>');
                        str.AppendFormat('</div>');



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

        //获取Get参数
        function GetParm(parm) {
            //获取当前URL
            var local_url = window.location.href;

            //获取要取得的get参数位置
            var get = local_url.indexOf(parm + "=");
            if (get == -1) {
                return "";
            }
            //截取字符串
            var get_par = local_url.slice(parm.length + get + 1);
            //判断截取后的字符串是否还有其他get参数
            var nextPar = get_par.indexOf("&");
            if (nextPar != -1) {
                get_par = get_par.slice(0, nextPar);
            }
            return get_par;
        }
        //获取参数

        function ConvertToAreaName(areacode) {
            var areaName = "";
            switch (areacode) {

                case "1":
                    areaName = "黄浦、卢湾区";
                    break;
                case "2":
                    areaName = "静安、闸北区";
                    break;
                case "3":
                    areaName = "徐汇区";
                    break;
                case "4":
                    areaName = "浦东新区(南)";
                    break;
                case "5":
                    areaName = "浦东新区(北)";
                    break;
                case "6":
                    areaName = "虹口、杨浦区";
                    break;
                case "7":
                    areaName = "宝山区";
                    break;
                case "8":
                    areaName = "普陀、嘉定区";
                    break;
                case "9":
                    areaName = "长宁、青浦区";
                    break;
                case "10":
                    areaName = "闵行区";
                    break;
                case "11":
                    areaName = "松江、金（山）奉（贤）区";
                    break;
               
                default:

            }
            return areaName;


        }

    </script>
    <script type="text/javascript">
        //分享
        var shareTitle = "SuperTeam";
        var shareDesc = "SuperTeam";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/SuperTeam/image/logo.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
