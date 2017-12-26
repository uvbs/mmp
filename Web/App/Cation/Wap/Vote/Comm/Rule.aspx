<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Vote/Comm/Master.Master"
    AutoEventWireup="true" CodeBehind="Rule.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm.Rule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
<%=currVote.VoteName %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="css/idangerous.swiper.css" />
    <link href="css/slider1.css" rel="stylesheet" type="text/css" />--%>
    <style>
        body
        {
            background-color: White;
        }
        

        .match-div2{margin-top:0px;}
    </style>
     <%=styleCustomize.ToString()%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">

     <%if (!string.IsNullOrWhiteSpace(currVote.BgMusic))
        { %>
     <audio id="audioBg" src="<%=currVote.BgMusic %>" ></audio>    <div id="musicbutton" class="musicplay" style="left:0%;" onclick="changeMusicCtrl()"></div>
    <%} %>

    <div class="wrapRule">
        <%=currVote.RulePageHtml%>"
        
    </div>

    <%=footerHtml.ToString()%>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
   <%-- <script src="js/jquery.flexslider.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        //$(function () {

        //    $('.panels_slider').flexslider({
        //        animation: "slide",
        //        directionNav: false,
        //        controlNav: true,
        //        animationLoop: true,
        //        slideToStart: 0,
        //        slideshowSpeed: 3000,
        //        animationDuration: 300,
        //        slideshow: true,
        //        slideDirection: "horizontal"
        //    });
        //})
        //分享
        var shareTitle = "<%=currVote.ShareTitle%>";
        var shareDesc = "<%=currVote.Summary%>";
        var shareImgUrl = "<%=currVote.VoteImage.StartsWith("http")? currVote.VoteImage:"http://" + Request.Url.Host + currVote.VoteImage%>";  //"http://<%=Request.Url.Host %>/customize/beachhoney/images/match_01.jpg";        
        var shareLink = window.location.href;
        //分享

    </script>
</asp:Content>
