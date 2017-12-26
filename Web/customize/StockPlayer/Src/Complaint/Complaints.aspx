<%@ Page Title="投诉与建议" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="Complaints.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Complaint.Complaints" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/Complaint/Complaints.css" rel="stylesheet"  />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="warp-Suggestion">
        <div class="row content">
                <div class="col-xs-12">
                    <h3>投诉建议</h3>
                </div>
                <div class="col-xs-3 textRight">
                    <label class="control-label title">标题：</label></div>
                <div class="col-xs-6">
                    <input type="text" id="title"  maxlength="50" class="form-control width400" placeholder="请输入标题" /></div>
                <div class="col-xs-3 dTip"><span class="lbTip" data-tip-msg="<b>投诉建议说明</b><br>1.后台审核通过将获取淘股币奖励；<br>2.12小时内到账，详情可咨询客服；<br/>谢谢您的理解和支持！">?</span></div>
           
                <div class="col-xs-3 textRight">
                    <label class="control-label title">内容：</label></div>
                <div class="col-xs-9 body-content">
                    <script id="suggestion"  type="text/plain">
                    </script>
                </div>
        </div>
         <div class="button">
            <button type="button" id="BtnAdd"  class="btn btn-default btn-add">确定</button>
            <button type="button" id="BtnReset" class="btn btn-default btn-reset">清空</button>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script type="text/javascript" src="/lib/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" src="/lib/ueditor/ueditor.all.min.js"> </script>
    <script type="text/javascript" src="/customize/StockPlayer/Src/Complaint/Complaints.js"></script>
</asp:Content>
