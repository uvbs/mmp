<%@ Page Title="" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="PupilDebateList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.PupilDebate.PupilDebateList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/customize/StockPlayer/Src/PupilDebate/PupilDebate.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="warpPupildebate row">
        <div class="col-xs-4 warp-left border1"><a href="javascript:;" onclick="GoWeekForecast('stock')">股市周线预测(上证、深证)</a></div>
        <div class="col-xs-4 warp-left border2"><a href="javascript:;" onclick="GoWeekForecast('Metal')">贵金属周线预测(黄金、白银)</a></div>
        <div class="col-xs-4 warp-left border3"><a href="javascript:;" onclick="GoWeekForecast('Crude')">原油周线预测</a></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript" src="/customize/StockPlayer/Src/PupilDebate/PupilDebate.js"></script>
</asp:Content>
