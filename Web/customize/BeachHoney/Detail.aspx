<%@ Page Title="" Language="C#" MasterPageFile="~/customize/BeachHoney/Master.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.BeachHoney.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    沙滩宝贝-选手
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="css/slider1.css" rel="stylesheet" type="text/css" />
    <style>
        body
        {
            background-color: White;
        }
        
        .panels_slider ol.flex-control-nav
        {
            margin-top: 5px;
        }
        .code2
        {
            right: 0;
            margin-top: -120px;
            margin-right: 20px;
            text-align: right;
            position: absolute;
        }
        #imgqrcode
        {
            width: 100px;
        }
        .filter-div
        {
            border: 1px #000 solid;
            position: fixed;
            top: 0;
            left: 0;
            z-index: 9999;
            width: 100%;
            height: 100%;
            background: #000;
            filter: alpha(opacity=60);
            opacity: 0.6;
            display: none;
        }
        .popup
        {
            width: 80%;
            left: 10%;
            top: 15%;
            height: auto;
            min-height: 48%;
            background: #53a3da;
            padding-top: 10%;
            padding-bottom: 10%;
            position: fixed;
            z-index: 99999;
            filter: alpha(opacity=80);
            opacity: 0.8;
            display: none;
        }
        .popup1
        {
            width: 80%;
            left: 10%;
            top: 15%;
            height: auto;
            padding-top: 10%;
            padding-bottom: 10%;
            position: fixed;
            z-index: 99999;
            display: none;
        }
        .radius8
        {
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
        }
        .op_td img
        {
            max-width: 60%;
        }
        .op_td .td1
        {
            padding-left: 5%;
        }
        .op_td td
        {
            width: 50%;
            text-align: center;
        }
        .op_td
        {
            width: 100%;
        }
        table
        {
            border-collapse: collapse;
            border-spacing: 0;
        }
        .op_td .td2
        {
            padding-right: 5%;
        }
        .op_td td
        {
            width: 50%;
            text-align: center;
        }
        .op_td .td3
        {
            padding-top: 5%;
        }
        .op_td td
        {
            width: 50%;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <%
        ZentCloud.BLLJIMP.BLLVote bllVote = new ZentCloud.BLLJIMP.BLLVote();
        ZentCloud.BLLJIMP.Model.VoteObjectInfo model = bllVote.GetVoteObjectInfo(int.Parse(Request["id"]));
        if (model == null)
        {
            Response.End();
        }
        if (model.Status!=1)
        {
            Response.Write("审核未通过");
            Response.End();
        }
    %>
    <div class="image_single">
        <img src="images/detail_01.png" alt="" title="" border="0" />
    </div>
    <div class="list1">
        <div class="page_padding5">
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
                        if (!string.IsNullOrEmpty(model.ShowImage3))
                        {
                            Response.Write(string.Format(" <li><img src=\"{0}\" border=\"0\" /></li>", model.ShowImage3));
                        }
                        if (!string.IsNullOrEmpty(model.ShowImage4))
                        {
                            Response.Write(string.Format(" <li><img src=\"{0}\" border=\"0\" /></li>", model.ShowImage4));
                        }
                        if (!string.IsNullOrEmpty(model.ShowImage5))
                        {
                            Response.Write(string.Format(" <li><img src=\"{0}\" border=\"0\" /></li>", model.ShowImage5));
                        } 
                                                
                    %>
                </ul>
            </div>
        </div>
        <div class="page_padding6 slogan">
            <h4>
                <%=model.VoteObjectName %>,<%=model.Area %>&nbsp;&nbsp;&nbsp;第<%=model.Number %>号</h4>
            <p class="slogan">
                <%=model.Introduction %>
            </p>
        </div>
        <div class="page_padding8 sliderbg1">
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
        </div>
        <div class="page_padding8 sliderbg1">
            <div class="menu4">
                <ul>
                    <li id="li1" class="active">
                    <a href="javascript:void(0)">
                        <img src="images/detail_05.png" alt="" title="" id="imgvote" /></a>
                    </li>
                    <%switch (Request.Url.Host)
                      {
                          case "beachhoney.comeoncloud.net"://当前域名是沙滩宝贝
                          //输出
                              Response.Write("<li id=\"li2\">");
                              Response.Write("<a href=\"http://mp.weixin.qq.com/s?__biz=MjM5MzQ1MDg0Mw==&mid=216676021&idx=1&sn=f3d538f5405655668135fb07779a894a#rd\">");//热带风暴
                              Response.Write("<img src=\"images/detail_06.png\"  />");
                              Response.Write("</a>");
                              Response.Write("</li>");
                              
                              Response.Write("<li id=\"li3\">");
                              Response.Write(" <a href=\"http://mp.weixin.qq.com/s?__biz=MzAwMTU5MzQwOQ==&mid=207465536&idx=1&sn=e2666d556034794f524baac7aeade2fc#rd\">");//mstudio
                              Response.Write("<img src=\"images/detail_07.png\" />");
                              Response.Write("</a>");
                              Response.Write("</li>");
                    
                          //输出
                              break;
                          case "beachhoney2.comeoncloud.net"://当前域名是热带风暴
                              //输出
                              Response.Write("<li id=\"li2\">");
                              Response.Write(string.Format("<a href=\"http://beachhoney.comeoncloud.net/customize/BeachHoney/Detail.aspx?id={0}\">",model.AutoID));
                              Response.Write("<img src=\"images/btnvoteshatan.png\"  />");//沙滩宝贝
                              Response.Write("</a>");
                              Response.Write("</li>");

                              Response.Write("<li id=\"li3\">");
                              Response.Write(" <a href=\"http://mp.weixin.qq.com/s?__biz=MzAwMTU5MzQwOQ==&mid=207465536&idx=1&sn=e2666d556034794f524baac7aeade2fc#rd\">");//mstudio
                              Response.Write("<img src=\"images/detail_07.png\" />");
                              Response.Write("</a>");
                              Response.Write("</li>");

                              //输出

                              break;
                          case "mstudio.comeoncloud.net"://当前域名是 mstudio
                              //输出
                              Response.Write("<li id=\"li2\">");
                              Response.Write(string.Format("<a href=\"http://beachhoney.comeoncloud.net/customize/BeachHoney/Detail.aspx?id={0}\">", model.AutoID));
                              Response.Write("<img src=\"images/btnvoteshatan.png\"  />");//沙滩宝贝
                              Response.Write("</a>");
                              Response.Write("</li>");

                              Response.Write("<li id=\"li3\">");
                              Response.Write(" <a href=\"http://mp.weixin.qq.com/s?__biz=MjM5MzQ1MDg0Mw==&mid=216676021&idx=1&sn=f3d538f5405655668135fb07779a894a#rd\">");//热带风暴
                              Response.Write("<img src=\"images/detail_06.png\" />");
                              Response.Write("</a>");
                              Response.Write("</li>");
                              //输出
                              break;
                          default:
                              break;
                      } %>


                </ul>
            </div>
        </div>
    </div>
    <div class="image_single">
        <img src="images/detail_09.png" alt="" title="" border="0" />
        <a class="code2">
            <img src="" alt="" title="" border="0" id="imgqrcode" />
        </a>
    </div>
    <div id="filter" class="filter-div">
    </div>
    <div id="success1" class="popup radius8">
    </div>
    <div id="success" class="popup1">
        <a href="javascript:void(0);" onclick="closePopup()" style="float: right; margin-top: -30px;
            margin-right: 10px;">
            <img src="images/delete-black.png" style="width: 30px" /></a>
        <div class="image_single">
            <img src="images/op_01.png" id="imgvoteresult" /></div>
        <table class="op_td">
            <tr>


                      <%switch (Request.Url.Host)
                      {
                          case "beachhoney.comeoncloud.net"://当前域名是沙滩宝贝
                          //输出
                          Response.Write("<td class=\"td1\">");
                          Response.Write("<a href=\"http://mp.weixin.qq.com/s?__biz=MjM5MzQ1MDg0Mw==&mid=216676021&idx=1&sn=f3d538f5405655668135fb07779a894a#rd\">");//热带风暴
                          Response.Write("<img src=\"images/op_02.png\" />");
                          Response.Write("</a>");
                          Response.Write("</td>");
                          Response.Write("<td class=\"td2\">");
                          Response.Write("<a href=\"http://mp.weixin.qq.com/s?__biz=MzAwMTU5MzQwOQ==&mid=207465536&idx=1&sn=e2666d556034794f524baac7aeade2fc#rd\">");//mstudio
                          Response.Write("<img src=\"images/op_03.png\" />");             
                          Response.Write("</a>");            
                          Response.Write("</td>");
                                          
                          //输出
                              break;
                          case "beachhoney2.comeoncloud.net"://当前域名是热带风暴
                              //输出
                          Response.Write("<td class=\"td1\">");
                          Response.Write(string.Format("<a href=\"http://beachhoney.comeoncloud.net/customize/BeachHoney/Detail.aspx?id={0}\">",model.AutoID));
                          Response.Write("<img src=\"images/btnvoteshatan.png\" />");//沙滩宝贝
                          Response.Write("</a>");
                          Response.Write("</td>");
                          Response.Write("<td class=\"td2\">");
                          Response.Write("<a href=\"http://mp.weixin.qq.com/s?__biz=MzAwMTU5MzQwOQ==&mid=207465536&idx=1&sn=e2666d556034794f524baac7aeade2fc#rd\">");//mstudio
                          Response.Write("<img src=\"images/op_03.png\" />");             
                          Response.Write("</a>");            
                          Response.Write("</td>");
                          //输出

                              break;
                          case "mstudio.comeoncloud.net"://当前域名是 mstudio
                          //输出
                          Response.Write("<td class=\"td1\">");
                          Response.Write(string.Format("<a href=\"http://beachhoney.comeoncloud.net/customize/BeachHoney/Detail.aspx?id={0}\">",model.AutoID));
                          Response.Write("<img src=\"images/btnvoteshatan.png\" />");//沙滩宝贝
                          Response.Write("</a>");
                          Response.Write("</td>");
                                
                          Response.Write("<td class=\"td2\">");
                          Response.Write("<a href=\"http://mp.weixin.qq.com/s?__biz=MjM5MzQ1MDg0Mw==&mid=216676021&idx=1&sn=f3d538f5405655668135fb07779a894a#rd\">");//热带风暴
                          Response.Write("<img src=\"images/op_02.png\" />");
                          Response.Write("</a>");      
                          Response.Write("</td>");           
                              //输出
                              break;
                          default:
                              break;
                      } %>








            </tr>
            <tr>
                <td align="center" colspan="2" class="td3">
                    <a href="http://beachhoney.comeoncloud.net/customize/BeachHoney/SignUp.aspx">
                        <img src="images/op_04.png" /></a>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
    var number=<%=model.Number%>;
    var intro="<%=model.Introduction %>";
    var headImg="<%=model.VoteObjectHeadImage %>";
    var autoId="<%=model.AutoID %>";
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/jquery.flexslider.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function(){
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

        $("#imgvote").click(function(){
        
        Vote();
        
        })

        //加载二维码
         $.ajax({
            type: 'post',
            url: '/handler/qcode.ashx',
            data: { code: "http://<%=Request.Url.Host %>/customize/beachhoney/detail.aspx?id="+autoId },
            success: function (path) {
                $("#imgqrcode").attr("src", path);
            }
        });

        })
        //投票
        function Vote() {
            $.ajax({
                type: 'post',
                url: handlerurl,
                data: { Action: "UpdateVoteObjectVoteCount", id: "<%=Request["id"]%>" },
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    
                    if (resp.errcode == 0) {
                        //投票成功
                        var oldcount = $("#lblvotecount").text();
                        var newcount = parseInt(oldcount) + 1;
                        $("#lblvotecount").html(newcount+"票");
                        openPopup();
                        

                    }
                    else {
                    $("#imgvoteresult").attr("src","images/voteed.png");
                    openPopup();
                        //layermsg(resp.errmsg);
                    }
                   
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        layermsg("投票超时，请重新投票");

                    }
                }
            });


        }
        //分享

         function openPopup() {
            $('#filter').css("display", "block");
            $('#success').css("display", "block");
             $('#success1').css("display", "block");
            $(".sliderbg1").hide();


        }
        function closePopup() {
            $('#filter').css("display", "none");
            $('#success').css("display", "none");
             $('#success1').css("display", "none");
            $(".sliderbg1").show();
            
        }

       var shareTitle="我是第"+number+"号沙滩宝贝选手，我要拿免费包包和旅游！你也来参加吧！";
       var shareDesc=intro;
       var shareImgUrl="http://<%=Request.Url.Host%>"+headImg;
       var shareLink=window.location.href;
         //分享
    </script>
</asp:Content>
