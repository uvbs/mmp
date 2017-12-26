<%@ Page Title="消息中心" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="Notice.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.Notice.Notice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="http://static-files.socialcrmyun.com/customize/StockPlayer/Src/UserCenter/Notice/Notice.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="notice">
        <div class="notice-head">
            消息中心
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
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/customize/StockPlayer/Src/UserCenter/Notice/Notice.js"></script>
</asp:Content>
