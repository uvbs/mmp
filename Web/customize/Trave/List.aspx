<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Trave/Master.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Trave.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    为TA投票
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .page_padding10
        {
            padding: 0% 20% 5% 20%;
            text-align: center;
        }
        .nodata
        {
            text-align: center;
            color: White;
            font-size: 25px;
        }
        .left_maskbar
        {
            margin-left: 0px;
        }
        .bottom_maskbar
        {
            margin-right: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="pages_container">
        <div class="image_single">
            <img src="images/header_01.png" alt="" title="" border="0" />
        </div>
        <div class="list2 form">
            <table width="100%">
                <tr>
                    <td>
                        <div class="top-search radius6 ">
                            <input type="text" placeholder="输入宝贝名字或编号" id="txtKeyWord" />
                        </div>
                    </td>
                    <td>
                        <img src="images/btn_01.png" style="width: 55px" id="btnSearch" />
                    </td>
                </tr>
            </table>
            <div class="page_padding2">
                <div class="menu1">
                    <ul>
                        <li id="li1" class="active"><a href="javascript:void(0)">
                            <img src="images/btn_02.png" id="btnSort_Time" />
                        </a></li>
                        <li id="li2"><a href="javascript:void(0)">
                            <img src="images/btn_03.png" id="btnSort_Rank" />
                        </a></li>
                    </ul>
                </div>
            </div>
            <div class="menu2">
                <ul id="ulobjlist">
                </ul>
            </div>
            <div class="page_padding2">
                <div class="menu1">
                    <ul>
                        <li id="li3" class="active"><a href="javascript:void(0)">
                            <img src="images/btn_05.png" id="btnPre" title="" /></a></li>
                        <li id="li7"><a href="javascript:void(0)">
                            <img src="images/btn_06.png" id="btnNext" title="" /></a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="list3">
            <div class="page_padding10">
                <div class="image_single">
                    <img src="images/code.png" alt="" title="" border="0" />
                    <img src="images/code_msg.png" alt="" title="" border="0" />
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
                url: handlerPath,
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
                        var age = resp.list[i].age;

                        if (age.indexOf("岁") > -1) {
                            
                        }
                        else {
                            age += "岁";
                        }
                        str.AppendFormat('<span>{0}&nbsp;{1}</span>', resp.list[i].name, age);
                        str.AppendFormat('<a href="javascript:void(0)">');
                        str.AppendFormat('<img src="images/btn_04.png" />');
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');

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
    <script type="text/javascript">
        //分享
        var shareTitle = "中青旅遨游网，寻找小小旅行家，境外亲子游大奖等你来赢!";
        var shareDesc = "晒宝贝旅行靓照，分享旅途趣闻，赢取境外亲子游等丰厚大奖！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Trave/images/logo.png";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
