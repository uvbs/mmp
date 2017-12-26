<%@ Page Title="淘股币明细" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="ScoreDetails.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.ScoreDetails.ScoreDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/UserCenter/ScoreDetails/ScoreDetails.css?v=2016102401" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
     <div class="notice">
        <div class="notice-head">
            淘股币明细
        </div>
        <div class="list-notice">
            <div class="notice-item" style="display:none;">
                <div class="head"><div class="time"></div></div>
                <div class="content">
                </div>
                <div class="action">
                </div>
            </div>
        </div>
        <div class="bottom-pager">
            <div class="pagination pager1"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
     <script type="text/javascript" src="/customize/StockPlayer/Src/UserCenter/ScoreDetails/ScoreDetails.js?v=2016102401"></script>
</asp:Content>
