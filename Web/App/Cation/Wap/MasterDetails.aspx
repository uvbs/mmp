<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMain.Master" AutoEventWireup="true" CodeBehind="MasterDetails.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.MasterDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {

        });
    
    </script>
    <style type="text/css">
        .clear
        {
            clear: both;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div data-role="header" data-theme="b" data-position="fixed" style="" id="divTop">
        <a href="#" data-role="button" data-rel="back" data-icon="arrow-l">返回</a>
        <h1>
            <%=masterInfo.MasterName %></h1>
    </div>
    <div class="clear">
    </div>
     <%=masterInfo.IntroductionContent %>

    <div title="返回顶部" style="position: fixed; _position: absolute; z-index: 1000; bottom: 10px;
        right: 0; _right: 17px; height: 60px; width: 22px; height: 20px;" onclick="$.scrollTo('#divTop', 200)">
        <img src="/JuActivity/Wap/image/top.gif" width="20px" height="20px" /></div>
</asp:Content>
