<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Jiepai/Master.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Jiepai.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Qee全球街拍探秘
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/index.css" rel="stylesheet" />
    <style>
        img {
            width: 100%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div id="divcontent"></div>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(function () {
            var id = "508939";
            $.get("http://comeoncloud.comeoncloud.net/serv/pubapi.ashx?action=getnewsdetail&newsid=" + id, function (data, status) {
                $("#divcontent").append($(data.newscontent).addClass("full-image"));
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
