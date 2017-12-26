<%@ Page Title="淘股币充值" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="ScoreRecharge.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.ScoreRecharge.ScoreRecharge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/UserCenter/ScoreRecharge/ScoreRecharge.css?v=2016102401" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="recharge">
        <div class="recharge-head">
            淘股币充值
        </div>
        <div class="recharge-content">
            <div class="recharge-warp">
                <div class="row-warp warp-num">
                    <label>充值数量：</label>

                    <input type="number" class="form-control textCheck recharge-num" placeholder="请输入充值数量" required="required"  value="100"
                        onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />

                    <span class="ng-binding">淘股币（100淘股币=<%= Recharge %>元）</span>
                </div>
                <div class="row-warp warp-tj">
                    <label>快捷选择：</label>
                    <span class="tj-num selected">100</span>
                    <span class="tj-num">200</span>
                    <span class="tj-num">500</span>
                    <span class="tj-num">1000</span>
                </div>
                <div class="row-warp warp-rmk"><label>应付金额：</label><span class="warp-money"><%= Recharge %></span><span>元</span></div>
                <div class="row-warp warp-pay"><label>支付方式：</label><img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/weixin.png" /></div>
                <div class="row-warp warp-btn"><button class="btn btn-warning btn-pay">充值</button></div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript">
        var rc_num= <%= Recharge %>;
    </script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/UserCenter/ScoreRecharge/ScoreRecharge.js?v=2016102401"></script>
</asp:Content>
