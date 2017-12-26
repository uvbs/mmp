<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Vote/Comm/Master.Master"
    AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Vote.Comm.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
<%=currVote.VoteName %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        body
        {
            background-color: White;
        }

    </style>
     <%=styleCustomize.ToString()%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">

     <%if (!string.IsNullOrWhiteSpace(currVote.BgMusic))
        { %>
     <audio id="audioBg" src="<%=currVote.BgMusic %>" ></audio>    <div id="musicbutton" class="musicplay" style="left:0%;" onclick="changeMusicCtrl()"></div>
    <%} %>

    <%--<div class="image_single">
        <img src="images/index_01.jpg" alt="" title="" border="0" />
        <img src="images/index_02.jpg" alt="" title="" border="0" />
        <img src="images/index_03.jpg" alt="" title="" border="0" /></div>
    <div class="page_padding0">
        <div class="index-txt radius8">
            <p>
                一、价值8000元的沙巴+美人鱼5天4日游（3个名额）</p>
            <p>
                二、Victoria Gossip品牌包包100支（总价值10万元）</p>
            <p>
                三、M-Studio 兑换券（总价值40,000元）</p>
            <p>
                四、热带风暴电子门票1140张总价值228,000元）</p>
        </div>
    </div>--%>
    


        <div class="wrapIndex">
            <%--<img src="<%=currVote.IndexBg %>" class="bgImg" />--%>
            <%
                if (!string.IsNullOrEmpty(currVote.IndexPageHtml))
                {
                    %>
                            <%=currVote.IndexPageHtml %>
                    <%
                }
             %>

    </div>
    <%
        if (currVote.IsHideIndexFooterMenu==0)
        {
            Response.Write(footerHtml.ToString());
        }    
     %>
     
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    //分享
    var shareTitle = "<%=currVote.ShareTitle%>";
    var shareDesc = "<%=currVote.Summary%>";
    var shareImgUrl = "<%=currVote.VoteImage.StartsWith("http")? currVote.VoteImage:"http://" + Request.Url.Host + currVote.VoteImage%>";  //"http://<%=Request.Url.Host %>/customize/beachhoney/images/match_01.jpg";
    var shareLink = window.location.href;
    var currVid = <%=currVote.AutoID%>;
    //分享
</script>
</asp:Content>
