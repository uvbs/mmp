<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.mobile.ClassDetail" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/green.css" />
    <script type="text/javascript" src="js/jquery-1.10.1.min.js"></script>
    <style>
    .menu2{margin-top:15px;margin-bottom:100px;}
    .image_single img{height:auto;}
    .menu5 ul li{width:33.33%;}
    .form_input12 img{margin-left:5px;}
    .form_input13{width:100px;}
    </style>
</head>
<body>
<%
    ZentCloud.BLLJIMP.BllVote bllVote = new ZentCloud.BLLJIMP.BllVote();
    ZentCloud.BLLJIMP.Model.VoteObjectInfo model = bllVote.GetVoteObjectInfo(int.Parse(Request["id"]));
 %>
    <!--Page 1 content-->
    <div class="swiper-slide sliderbg">
        <div class="swiper-container swiper-nested">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="slide-inner">
                        <div class="pages_container">
                            <h2 class="page_title" style="text-align:center">
                              <%=model.VoteObjectName %>
                            </h2>
                            <div class="index_container radius4">
                                <div class="image_single">
                                
                                    <img src="<%=model.VoteObjectHeadImage %>" alt="" title="" border="0" /></div>
                            </div>
                            <div class="index_container1">
                                <h3>
                                    我们班的参赛口号：</h3>
                                <p style="font-size:14px;font-weight:bold;">
                                    <%=model.Introduction %></p>
                            </div>
                             <table style="width:100%;  margin-left:1%;margin-top:15px;">
                                <tr>
                                    <td>
                                        <div class="form_input12 blue1 radius4">
                                            <img src="images/v1.png" />
                                            <div class="form_input13 white radius4">
                                             <label id="lblvotecount"><%=model.VoteCount %></label>票
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form_input12 red1 radius4">
                                            <img src="images/v2.png" /><div class="form_input13 white radius4">
                                                第<%=model.Rank %>名</div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <!--Menu page-->
                            <div class="menu2">
                                <ul>
                                    <li><a href="javascript:void(0)">
                                        <img src="images/vote01.png" alt="" title="" id="imgvote" /><span>投一票</span></a></li>
                                    <li><a href="javascript:void(0)">
                                        <img src="images/vote02.png" class="sentofriend" /><span>邀请朋友投票</span></a></li>
                                    <li><a href="javascript:void(0)">
                                        <img src="images/vote03.png" class="sharetofriend"  /><span>分享至朋友圈</span></a></li>
                                </ul>
                                <div class="clearfix">
                                </div>
                            </div>
                        </div>
                        <!--End of page container-->
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="footer">
        <div class="menu5">
            <ul>
              <li><a href="eventdetails.aspx"><span>
                    <img src="images/en1.png" alt="" title="" />活动详情</span></a></li>
                    <li><a href="index.aspx"><span>
                <img src="http://jkbp.comeoncloud.net/open/jkbp/images/home1.png" />
                    </span></a></li>
              <li><a href="<%=new ZentCloud.BLLJIMP.BllVote().GetTotemaApplyLingOrMyClassLink()%>"><span>
                    <img src="images/en3.png" alt="" title="" />我要报名</span></a></li>
            </ul>
        </div>
    </div>
          <div style="width: 100%; height: 100%; display: none; background: #000; opacity: 0.7;
            position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
            &nbsp;
        </div>
        <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; height: 100%;text-align: right;
            display: none;" id="sharebox">
            <img src="images/sharetip.png" width="100%" />
        </div>

</body>
<%--    <script type="text/javascript" src="js/jquery.swipebox.js"></script>
    <script type="text/javascript" src="js/idangerous.swiper-2.1.min.js"></script>
    <script type="text/javascript" src="js/idangerous.swiper.scrollbar-2.1.js"></script>
    <script type="text/javascript" src="js/jquery.tabify.js"></script>
    <script type="text/javascript" src="js/jquery.fitvids.js"></script>
    <script type="text/javascript" src="js/code.js"></script>
    <script type="text/javascript" src="js/load.js"></script>
    <script src="js/jquery.flexslider.js"></script>
--%>
<script type="text/javascript">
    $(document).ready(function () {
        $(".sentofriend,.sharetofriend").click(function () {
            $("#sharebg,#sharebox").show();
            $("#sharebox").css({ "top": $(window).scrollTop() })
        });

        $("#sharebg,#sharebox").click(function () {
            $("#sharebg,#sharebox").hide();
        });

        $("#imgvote").click(function () {
            Vote(<%=model.AutoID%>);
        })

    });
    </script><script type="text/javascript">
    var handlerurl = "/Handler/customize/totema/Handler.ashx";
    //投票
    function Vote(classid) {
        $.ajax({
            type: 'post',
            url: handlerurl,
            data: { Action: "UpdateVoteObjectVoteCount", classid: classid },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                if (resp.errcode == 0) {
                    //投票成功
                    var oldcount = $("#lblvotecount").text();
                    var newcount = parseInt(oldcount) + 1;
                    $("#lblvotecount").html(newcount);
                    alert("投票成功");
                }
                else {
                    alert(resp.errmsg);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("投票超时，请重新投票");

                }
            }
        });


    }
</script><script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "TOTEMA新国际主义校服为我们班投一票",
            desc: "TOTEMA新国际主义校服为我们班投一票",
            imgUrl: "http://<%=Request.Url.Host%>/customize/totema/mobile/images/logo.jpg"
        })
    })
</script>
</html>