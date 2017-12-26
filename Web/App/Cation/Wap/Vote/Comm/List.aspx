<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Vote/Comm/Master.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <%=currVote.VoteName %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            <%
                if(!string.IsNullOrWhiteSpace(currVote.VotePageBgColor))
                {
                    %>
                         background-color:<%=currVote.VotePageBgColor%>;
                    <%
                }else{

                     %>
                         background-color:White;
                     <%
                 }
            %>
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
        /*#ulobjlist  li{
           overflow: hidden;
        }
        #ulobjlist li .headimg{
                height: 170px;
        }*/
    </style>
     <%=styleCustomize.ToString()%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    
         <%if (!string.IsNullOrWhiteSpace(currVote.BgMusic))
        { %>
            <audio id="audioBg" src="<%=currVote.BgMusic %>" ></audio>
        <div id="musicbutton" class="musicplay" style="left:0%;" onclick="changeMusicCtrl()"></div>
        <%} %>

        <% if (currVote.VoteStatus == 0 && !string.IsNullOrWhiteSpace(currVote.NotStartPoster))
            { %>
        <div class="wrapNotStartPoster">
            <img class="width100P" alt="" src="<%=currVote.NotStartPoster %>" />
        </div>
        <style type="text/css">
            .wrapList{
                display:none;
            }
        </style>

     <%} %>
     <div class="wrapList">

        <div class="image_single">
            <img src="<%=currVote.VoteObjListBannerImg %>" alt="" title="" border="0" />
        </div>
        <div class="list form pBottom30 wrapList">

             <%if (currVote.VoteStatus != 3) { %>
            <table width="100%" style="margin-left: 14px;">
                <tr>
                    <td class="pTop4">
                        <div class="top-search radius6 ">
                            <input type="text" id="txtKeyWord" placeholder="选手编号" />
                        </div>
                    </td>
                    <td>
                        &nbsp;<%--<img src="images/btn_01.png" id="btnSearch" />--%>
                        &nbsp;<span class="btnSearch" id="btnSearch">搜索</span>
                    </td>
                </tr>
            </table>
            <div class="wrapSortBtn">
                <div class="menu1">
                    <ul>
                        <li id="li1" class="active"><a href="javascript:void(0)"  class="btnSort">
                            <%--<img src="images/btn_02.png" alt="" title="" id="btnSort_Time" />--%>
                            <span id="btnSort_Time">最新上传</span>    
                            </a>

                        </li>

                        <li id="li2"><a href="javascript:void(0)"  class="btnSort">
                            <%--<img src="images/btn_03.png" alt="" title="" id="btnSort_Rank" />--%>

                            <span id="btnSort_Rank">投票排名</span>   
                                     </a></li>
                    </ul>
                </div>
            </div>
              <%} %>

            <div class="menu2">
                <div class="ulobjlist">
                </div>
            </div>
            
            <%if (currVote.VoteStatus != 3) { %>
            <div class="form_div3" style="margin: 26% 0 50px;    padding-left: 8%;">
                <a class="form_a red radius6" id="btnPre">上一页</a> <a class="form_a red radius6" id="btnNext">
                    下一页</a>
            </div>
            <%} %>

        </div>

    
    </div>
    <%=footerHtml.ToString()%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageindex = 1; //第几页
        var pagesize = 20; //每页显示几条数据
        var sort = ""; //排序
        var voteStatus = "<%=currVote.VoteStatus%>";
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
                $("#btnNext").css({ "visibility": "visible" });
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
                sort = "";
                sessionStorage.setItem("sort", sort);
                Search();


            });


        });

        //加载数据
        function LoadData() {

            if (voteStatus == 3) {

                $.ajax({
                    type: 'get',
                    url: handlerurl,
                    data: { Action: 'GetVoteGroupList', vid: '<%=currVote.AutoID%>' },
                    success: function (resp) {

                        console.log(resp);

                        var html = new StringBuilder();


                        for (var i = 0; i < resp.result.length; i++) {
                            var group = resp.result[i];

                            var str = new StringBuilder();
                            str.AppendFormat('<div class="group"><div class="groupName">{0}</div><ul>', group.VoteGroupName);
                            if (group.ObjectList) {
                                for (var j = 0; j < group.ObjectList.length; j++) {
                                    var member = group.ObjectList[j];

                                    var tag = "";
                                    if (member.Rank == 1) {
                                        tag = "<tt class=\"right_maskbar\"><b>1</b></tt>";
                                    }
                                    else if (member.Rank == 2 || member.Rank == 3) {

                                        tag = "<tt class=\"right_maskbar1\"><b>" + member.Rank + "</b></tt>";
                                    }
                                    else {
                                        tag = "<tt class=\"right_maskbar2\"><b>" + member.Rank + "</b></tt>";
                                    }
                                    //
                                    var link = "window.location.href='Detail.aspx?vid=<%=currVote.AutoID%>&id=" + member.AutoID + "'";
                                    str.AppendFormat('<li onclick="{0}">', link);
                                    str.AppendFormat('<div class="mask">');
                                    str.AppendFormat('<tt class="left_maskbar">{0}号</tt>', member.Number);
                                    str.AppendFormat('{0}', tag);
                                    str.AppendFormat('</div>');
                                    str.AppendFormat('<div style="height:250px;display:table;width:100%;"><div style="vertical-align:middle;display:table-cell;"><img src="{0}" alt="" title="" border="0" class="headimg" /></div></div>', member.VoteObjectHeadImage);
                                    str.AppendFormat('<tt class="bottom_maskbar" id="span{1}">{0}票</tt>', member.VoteCount, member.AutoID);
                                    str.AppendFormat('<span>{0}&nbsp;{1}</span>', member.VoteObjectName, member.Area);
                                    str.AppendFormat('<a href="javascript:void(0)">');
                                    //str.AppendFormat('<img src="images/btn_04.png" />');//投票按钮不用图片
                                    str.AppendFormat('<span class="btnToVote">为TA投票</span>');
                                    str.AppendFormat('</a>');
                                    str.AppendFormat('</li>');
                                    //

                                }
                            }
                            str.AppendFormat('</ul></div>');

                            html.Append(str.ToString());
                        }


                        $(".ulobjlist").html(html.ToString());

                    }
                });


            } else {
                $.ajax({
                    type: 'post',
                    url: handlerurl,
                    data: { Action: 'GetVoteObjectVoteList', pageindex: pageindex, pagesize: pagesize, keyword: $(txtKeyWord).val(), sort: sort,vid:'<%=currVote.AutoID%>' },
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
                            var link = "window.location.href='Detail.aspx?vid=<%=currVote.AutoID%>&id=" + resp.list[i].id + "'";

                            if (voteStatus == 2) {
                                str.AppendFormat('<li onclick="{0}" style="height: auto;">', link);
                            } else {
                                str.AppendFormat('<li onclick="{0}">', link);
                            }
                            

                            str.AppendFormat('<div class="mask">');
                            str.AppendFormat('<tt class="left_maskbar">{0}号</tt>', resp.list[i].number);
                            str.AppendFormat('{0}', tag);
                            str.AppendFormat('</div>');
                            str.AppendFormat('<div style="height:250px;display:table;width:100%;"><div style="vertical-align:middle;display:table-cell;"><img src="{0}" alt="" title="" border="0" class="headimg" /></div></div>', resp.list[i].headimg);
                            str.AppendFormat('<tt class="bottom_maskbar" id="span{1}">{0}票</tt>', resp.list[i].votecount, resp.list[i].id);
                            str.AppendFormat('<span>{0}&nbsp;{1}</span>', resp.list[i].name, resp.list[i].area);

                            if (resp.list[i].conatact) {
                                str.AppendFormat('<span>{0}</span>', resp.list[i].conatact);
                            }

                            if (voteStatus != 2) {
                                str.AppendFormat('<a href="javascript:void(0)">');
                                //str.AppendFormat('<img src="images/btn_04.png?v={0}" />', Math.random());
                                str.AppendFormat('<span class="btnToVote">为TA投票</span>');
                                str.AppendFormat('</a>');
                            }
                            
                            str.AppendFormat('</li>');
                            //

                        };

                        var listHtml = str.ToString();
                        if (listHtml == "") {
                            $(".ulobjlist").html("<div class=\"nodata\">没有更多了</div>");
                            $("#btnNext").css({ "visibility": "hidden" });
                            //pageindex--;
                            //sessionStorage.setItem("pageIndex", pageindex);
                            $(window).scrollTop(0);
                        }
                        else {
                            $(".ulobjlist").html('<ul>' + str.ToString() + '</ul>');
                            //$(window).scrollTop(200);
                        }
                        $("#txtKeyWord").attr("placeholder", "输入选手编号 ");

                    },
                    complete: function () {
                        $("#btnPre").html("上一页");
                        $("#btnNext").html("下一页");
                        if (pageindex==1) {
                            $("#btnPre").css({ "visibility": "hidden" });
                            $("#btnNext").css({ "visibility": "visible" });
                        }
                        else {
                            $("#btnPre").css({ "visibility": "visible" });

                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        if (textStatus == "timeout") {
                            layermsg("加载超时，请刷新页面");

                        }
                    }
                });

            }


        }

        //搜索
        function Search() {
            pageindex = 1;
            sessionStorage.setItem("pageIndex", pageindex);
            $(".ulobjlist").html("");
            LoadData();
        }

        //分享
        var shareTitle = "<%=currVote.ShareTitle%>";
        var shareDesc = "<%=currVote.Summary%>";
        var shareImgUrl = "<%=currVote.VoteImage.StartsWith("http")? currVote.VoteImage:"http://" + Request.Url.Host + currVote.VoteImage%>";  //"http://<%=Request.Url.Host %>/customize/beachhoney/images/match_01.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
