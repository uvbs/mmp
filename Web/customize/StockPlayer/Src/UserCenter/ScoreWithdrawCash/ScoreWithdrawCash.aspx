<%@ Page Title="淘股币提现" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="ScoreWithdrawCash.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.UserCenter.ScoreWithdrawCash.ScoreWithdrawCash" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/UserCenter/ScoreWithdrawCash/ScoreWithdrawCash.css?v=2016102401" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="notice">
        <div class="notice-apply">
            <div class="apply-warp">
                <div class="row-warp warp-u-score">
                    <label>当前淘股币：</label><span class="warp-score"><%= curUser.TotalScore %></span><span>淘股币</span>
                </div>
                <div class="row-warp warp-num">
                    <label>提现淘股币：</label>
                    <input type="number" class="form-control textCheck recharge-num" placeholder="请输入提现数额" required="required" value="100"
                        onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                    <span class="ng-binding">淘股币（100淘股币=<%= Recharge %>元）</span>
                    <span class="lbTip" data-tip-msg="<b>提现说明</b><br>1.当淘股币大于200以上才可以提现；<br>2.提现金额不小于100淘股币；<br>3.12小时内到账，详情可咨询客服；<br/>谢谢您的理解和支持！">?</span>
                </div>
                <div class="row-warp warp-tj">
                    <label>快捷选择：</label>
                    <span class="tj-num selected">100</span>
                    <span class="tj-num">200</span>
                    <span class="tj-num">500</span>
                    <span class="tj-num">1000</span>
                </div>
                <div class="row-warp warp-rmk">
                    <label>实际金额：</label><span class="warp-money"><%= Recharge %></span><span>元</span></div>
                <div class="row-warp warp-u-sscore">
                    <label>剩余淘股币：</label><span class="warp-sscore"><%= (curUser.TotalScore-100) %></span><span>淘股币</span></div>
                <div class="row-warp warp-pay">
                    <label>提现方式：</label><img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/weixin.png" /></div>
        <%if(curUser.TotalScore > Convert.ToDouble(MinScore) + Convert.ToDouble(MinWithdrawCashScore)){ %>
                <div class="row-warp warp-btn">
                    <button class="btn btn-warning btn-apply">申请提现</button></div>
        <%}else{ %>
                <div class="row-warp warp-btn">
                    <button disabled="disabled" class="btn btn-warning btn-dis">余额不足</button></div>
        <%} %>
            </div>
        </div>
        <div class="notice-head">
            提现记录
        </div>
        <div class="list-notice">
            <div class="notice-item" style="display: none;">
                <div class="head">
                    <div class="code">编号：<span class="autoid"></span></div>
                    <div class="time"></div>
                </div>
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
    <script type="text/javascript">
        var rc_num= <%= Recharge %>;
        var min_score= <%= MinScore %>;
        var min_cashscore = <%= MinWithdrawCashScore %>;
    </script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/UserCenter/ScoreWithdrawCash/ScoreWithdrawCash.js?v=2016102401"></script>
</asp:Content>
