<%@ Page Title="找回密码" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Register.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="http://static-files.socialcrmyun.com/customize/StockPlayer/Src/Register/ForgotPassword.css?v=2016102001" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
    <div class="register">
        <div class="register-head">
            <button type="button"  value="user" class="btn btn-default btn-user">用户找回密码</button>
            <button type="button"  value="company" class="btn btn-default btn-company">公司找回密码</button>
        </div>
    </div>
    <div class="row register-body" id="user">
        <form name="userRegister">
                 <div class="col-xs-4 RightR"><label class="control-label">新密码：</label></div>
                <div class="col-xs-4 LeftL"><input type="password" name="user_pwd" id="code_pwd" class="form-control" placeholder="请输入新密码"/></div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label">确认密码：</label></div>
                <div class="col-xs-4 LeftL"><input type="password" name="user_configpwd" id="code_configpwd" class="form-control" placeholder="请输入确认密码"/></div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label">手机：</label></div>
                <div class="col-xs-4 LeftL"><input type="number" name="user_phone" id="code_phone" class="form-control" maxlength="11" placeholder="请输入手机号码"/></div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label">验证码：</label></div>
                <div class="col-xs-4 LeftL"><input type="number" id="user_code" class="form-control inline-block text-code" placeholder="请输入验证码"/>
                    <button type="button" class="btn btn-default  BackGroupColor-Y" id="getcode">获取验证码</button>
                </div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label"></label></div>
                <div class="col-xs-4"><button type="button" id="RegisterUser" class="btn btn-default register-border">找回密码</button></div>
                <div class="col-xs-4">&nbsp;</div>
        </form>
    </div>
    <div class="row register-body" id="company" style="display:none;">
            <form name="companyRegister">
                 <div class="col-xs-12 company-message">
                     请联系平台客服修改密码！！！
                 </div>
            </form>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
<script src="/customize/StockPlayer/Src/Register/ForgotPassword.js?v=20161123"></script>
</asp:Content>
