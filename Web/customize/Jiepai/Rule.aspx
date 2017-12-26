<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Jiepai/Master.Master" AutoEventWireup="true" CodeBehind="Rule.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Jiepai.Rule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
全球街拍规则
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section id="article-content">
</section>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script src="//cdn.bootcss.com/jquery/2.1.4/jquery.min.js"></script>
<script type="text/javascript">
    $(function () {
        var id = "508937";
        $.get("http://comeoncloud.comeoncloud.net/serv/pubapi.ashx?action=getnewsdetail&newsid=" + id, function (data, status) {
            $("#article-content").append($(data.newscontent).addClass("full-image"));
        });
    });
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
