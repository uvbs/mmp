<%@ Page Title="股权交易" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="StockList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Stock.StockList" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="http://static-files.socialcrmyun.com/customize/StockPlayer/Src/Stock/Stock.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="Width1000 mTop20">
         <div class="row body-head">
            <div class="col-xs-12">
                <div class="key1" id="buy">买入</div>
                <div class="key2">&nbsp;</div>
                <div class="key3" id="sell">卖出</div>
                <div class="key2">&nbsp;</div>
            </div>
        </div>
        <div class="row body-content">
            <div class="col-xs-4 stock-item" style="display:none;">
                <div class="body-content-border">
                    <div>
                        <img  src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/Stock.png" class="body-content-img"/>
                    </div>
                    <div class="stock-title">
                        <span class="Font18 title">奇境GinSPA</span>
                        <button type="button" class="btn btn-default content-bottom">类型</button>
                    </div>
                    <div class="stock-user">
                        <span class="colorY nick username">牛仔很忙</span>
                        <span class="mLeft10">发布于</span><span class="time">2012-12-12</span>
                    </div>
                    <div class="stock-content"><a class="Pointer" onclick="ToDetail(0)">
                        实打实大苏打都是收到收到收到收到
                        实打实大苏打都是收到收到收到收到
                        实打实大苏打都是收到收到收到收到
                        </a>
                    </div>
                    <div class="stock-buttom">
                        <span>股权数：</span>
                        <span class="stocknum">5000</span>
                        <br />
                        <button type="button" class="btn btn-default btn-send-notice">通知买家</button>
                    </div>
                 </div>
            </div>
           
        </div>

         <div class="row index-bottom">
            <div class="pagination pager1"></div>
            <div class="pagination pager2"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="buttom" runat="server">
      <script type="text/javascript">
          var notice_price = <%= sendNoticePrice%>;
      </script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/Stock/Stock.js"></script>
</asp:Content>
