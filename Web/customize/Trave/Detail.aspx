<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Trave/Master.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Trave.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    为TA投票
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/slider2.css" rel="stylesheet" type="text/css" />
    <style>
    #sharebox img{width:100%;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="pages_container">
        <div class="list1">
            <div class="page_padding5" style="margin-top: 20%">
                <div class="panels_slider">
                    <ul class="slides">
                        <%
                            if (!string.IsNullOrEmpty(model.ShowImage1))
                            {
                                Response.Write(string.Format(" <li><img src=\"{0}\" border=\"0\" /></li>", model.ShowImage1));
                            }
                            if (!string.IsNullOrEmpty(model.ShowImage2))
                            {
                                Response.Write(string.Format(" <li><img src=\"{0}\" border=\"0\" /></li>", model.ShowImage2));
                            }
                                  
                        %>
                    </ul>
                </div>
            </div>
            <div class="page_padding6">
                <div class="title_u">
                    <h4>
                        <%=model.VoteObjectName %>&nbsp;&nbsp;

                        <%if (model.Age.Contains("岁"))
                          {
                              Response.Write(model.Age);
                          }
                          else
                          {
                              Response.Write(string.Format("{0}岁",model.Age));
                          } %>
                        &nbsp;&nbsp;<%=model.Number %>号</h4>
                </div>
                <div class="slogan radius6">
                    <p>
                        <span>我去过</span>：<%=model.Address %>
                        <br />
                        <span>我的小小旅游宣言</span> :
                        <%=model.Introduction %></p>
                </div>
                <div class="menu5">
                    <ul>
                        <li>
                            <table>
                                <tr>
                                    <td>
                                        <a href="#tab1">
                                            <img src="images/detail_03.png" alt="" title="" /></a>
                                    </td>
                                    <td>
                                        <span class="count" id="lblvotecount">
                                            <%=model.VoteCount %>票</span>
                                    </td>
                                </tr>
                            </table>
                        </li>
                        <li>
                            <table>
                                <tr>
                                    <td>
                                        <a href="#tab2">
                                            <img src="images/detail_04.png" alt="" title="" /></a>
                                    </td>
                                    <td>
                                        <span class="level">第<%=model.Rank %>名</span>
                                    </td>
                                </tr>
                            </table>
                        </li>
                    </ul>
                </div>
                <div class="image_single">
                    <a href="javascript:void(0)" class="detail_h">
                        <img src="images/detail_07.png" id="imgvote" title="" border="0" /></a> 
                        

                       <%-- <a href="javascript:void(0)" class="detail_h">
                            <img src="images/detail_08.png" alt="" title="" border="0" /></a>--%>
                </div>
                <div class="menu4">
                    <ul>
                        <li id="li1" class="active">
                        <a href="javascript:void()" onclick="javascript:void(0)">
                            <img id="btnShare" src="images/detail_05.png" alt="" title="" /></a></li>
                        <li id="li2"><a href="List.aspx">
                            <img src="images/detail_06.png" alt="" title="" /></a></li>
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

            <div style="width: 100%; height: 1500px; display: none; background: #000; opacity: 0.7;
            position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
            &nbsp;
        </div>
        <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; height: 1500px;
            text-align: right; display: none;" id="sharebox">
            <img src="images/sharetip.png" />
        </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <script src="js/jquery.flexslider.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('.panels_slider').flexslider({
                animation: "slide",
                directionNav: false,
                controlNav: true,
                animationLoop: true,
                slideToStart: 0,
                slideshowSpeed: 3000,
                animationDuration: 300,
                slideshow: true,
                slideDirection: "horizontal"
            });

            $("#imgvote").click(function () {

                Vote();

            })

            $("#btnShare").click(function () {
                $("#sharebg,#sharebox").show();
                $("#sharebox").css({ "top": $(window).scrollTop() })
            });

            $("#sharebg,#sharebox").click(function () {
                $("#sharebg,#sharebox").hide();
            });


        })
        //投票
        function Vote() {
            $.ajax({
                type: 'post',
                url: handlerPath,
                data: { Action: "UpdateVoteObjectVoteCount", id: "<%=model.AutoID%>" },
                timeout: 30000,
                dataType: "json",
                success: function (resp) {

                    if (resp.errcode == 0) {
                        //投票成功
                        var oldcount = $("#lblvotecount").text();
                        var newcount = parseInt(oldcount) + 1;
                        $("#lblvotecount").html(newcount + "票");
                        layermsg("投票成功!");


                    }
                    else {

                        layermsg(resp.errmsg);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        layermsg("投票超时，请重新投票");

                    }
                }
            });


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
