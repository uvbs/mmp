<%@ Page Title="" Language="C#" MasterPageFile="~/customize/HaiMa/Vote/Sale/Master.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.Vote.Sale.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    海马真英雄-选手详情
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <%--    <link href="style/slider1.css" rel="stylesheet" type="text/css" />--%>
    <style>
        #sharebox img
        {
            width: 100%;
        }
        .wrapDetail .wrapUserInfo .row .col-50:nth-child(3) div:nth-child(5)
        {
            font-size: 20px;
            color: #ff8100;
        }
        .wrapDetail .wrapUserInfo .row .col-50:nth-child(3) div
        {
            font-size: 16px;
            margin: 6px 0 10px;
        }
        .wrapDetail .wrapUserInfo .row .col-50 .colContent
        {
            height: 195px;
        }
        .wrapDetail .colContent .panels_slider img
        {
            max-height: 195px;
        }
        .panels_slider ol.flex-control-nav
        {
            margin-top: 4px;
        }
        .wrapDetail .wrapUserInfo .row .col-50 .ranking
        {
            right: 5px;
        }
        .panels_slider .slides img
        {
            width: auto;
        }
        .col-50:first-child
        {
            text-align: center;
        }
        .col{padding:0px;}
         
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <%
        ZentCloud.BLLJIMP.BllVote bllVote = new ZentCloud.BLLJIMP.BllVote();
        ZentCloud.BLLJIMP.Model.VoteObjectInfo model = bllVote.GetVoteObjectInfo(int.Parse(Request["id"]));
        if (model == null)
        {
            Response.End();
        }
        if (model.Status != 1)
        {
            Response.Write("等待审核中");
            Response.End();
        }
    %>
    <div class="wrapDetail mBottom40">
        <div class="wrapUserInfo">
            <div class="row">
                <div class="col-50">
                    <div class="colContent">
                        <div class="panels_slider">
                            <ul class="slides">
                                <%
                                    if (!string.IsNullOrEmpty(model.VoteObjectHeadImage))
                                    {
                                        Response.Write(string.Format(" <li><img src=\"{0}\" border=\"0\" /></li>", model.VoteObjectHeadImage));
                                    }
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
                    <%int rank = model.Rank;
                      if (rank <= 3)
                      {
                          rank = model.Rank;
                      }
                      else
                      {
                          rank = 4;
                      }   
                        
                    %>
                    <img src="images/h<%=rank%>.png" class="crown" alt="" />
                    <div class="ranking">
                        <%=model.Rank %>
                    </div>
                </div>
                <div class="col-50" style="margin-left: 10px;">
                    <div>
                        <%=model.Number %>号</div>
                    <div>
                        <div>
                            <%=model.VoteObjectName %></div>
                        <div>
                            <%=model.Ex1%></div>
                        <div>
                            <%=model.Ex2%></div>
                        <div>
                            <%=model.Introduction %></div>
                        <div>
                            <i class="iconfont icon-dianzan"></i><span id="spvotecount">
                                <%=model.VoteCount %></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="wrapVideo mTop10">
                <div class="row title">
                    <div class="col">
                        课题1：<%=model.Ex3 %></div>
                </div>
                <div class="row videoPlay">
                    <div class="col">
                        <%if (!string.IsNullOrEmpty(model.Ex4))
                          {%>
                        <video id="video1" src="<%=model.Ex4%>" width="200px;" controls="controls">
                        
</video>
                        <%} %>
                    </div>
                </div>
                <div class="row title">
                    <div class="col">
                        课题2：<%=model.Ex5 %></div>
                </div>
                <div class="row videoPlay">
                    <div class="col">
                        <%if (!string.IsNullOrEmpty(model.Ex6))
                          {%>
                        <video id="video2" src="<%=model.Ex6%>" wdith="300px;" preload="preload" controls="controls">
Your browser does not support the video tag.
</video>
                        <%} %>
                    </div>
                </div>
            </div>
            <div class="wrapBottomBtn">
                <div class="row">
                    <div class="col">
                        <a href="javascript:;" class="font18" id="btnVote">为TA加油</a>
                    </div>
                </div>
                <div class="row">
                    <div class="col col-50">
                        <a href="javascript:;" id="btnShare">喊好友为TA加油</a>
                    </div>
                    <div class="col col-50">
                        <a href="List.aspx">为其他人加油</a>
                    </div>
                </div>
            </div>
            <div class="footer">
                <img src="images/footer.png" alt="">
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
        <script type="text/javascript">
            var shareTitle = "<%=model.VoteObjectName %>";
            var shareImg = "<%=model.VoteObjectHeadImage %>";

        </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/jquery.flexslider.js" type="text/javascript"></script>
    <script>

    try {
    var myVideo=document.getElementById("video1");
myVideo.height=166; 
var myVideo2=document.getElementById("video2");
myVideo2.height=166;
} catch (e) {
    
}
 
$(function(){

$("#btnVote").click(function(){
try {

document.getElementById("video1").pause();
document.getElementById("video2").pause();
} catch (e) {
    
}
Vote();

})

        $("#btnShare").click(function () {
            $("#sharebg,#sharebox").show();
            $("#sharebox").css({ "top": $(window).scrollTop() })
        });

        $("#sharebg,#sharebox").click(function () {
            $("#sharebg,#sharebox").hide();
        });

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


})
        //投票
        function Vote() {
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "UpdateVoteObjectVoteCount", id: "<%=Request["id"]%>" },
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    
                    if (resp.errcode == 0) {
                        //投票成功
                        var oldcount = $("#spvotecount").text();
                        var newcount = parseInt(oldcount) + 1;
                        $("#spvotecount").html(newcount+"票");
                       Alert(resp.errmsg);
                        //layermsg(resp.errmsg);

                    }
                    else {
                        //alert(resp.errmsg);
                       //layermsg(resp.errmsg);
                       Alert(resp.errmsg);
                    }
                   
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        layermsg("投票超时，请重新投票");

                    }
                }
            });


        }
        
  function Alert(msg){
  if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {
    //alert(navigator.userAgent); 
    layermsg(msg); 
   
} else if (/(Android)/i.test(navigator.userAgent)) {
    alert(msg); 
    
} 
else {
layermsg(msg); 

}   
        
        
        }

    </script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "【海马精英成长平台】精英锤炼,为TA加油!",
                desc: "【海马精英成长平台】精英锤炼,为TA加油!",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>" + shareImg
            })
        })
    </script>
</asp:Content>
