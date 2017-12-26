<%@ Page Title="水军注册页面" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="Register2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Register.Register2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/Register/Register.css?v=2016102004" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">
   
    <div class="row register-body" id="user">
        <form name="userRegister">
                <div class="col-xs-4 RightR"><label class="control-label">昵称：</label></div>
                <div class="col-xs-4 LeftL"><input type="text" name="user_nick" id="code_nick"  class="form-control" placeholder="请输入用户昵称"/></div>
                <div class="col-xs-4">&nbsp;</div>
                <div class="col-xs-4 RightR"><label class="control-label">密码：</label></div>
                <div class="col-xs-4 LeftL"><input type="password" name="user_pwd" id="code_pwd" class="form-control" placeholder="请输入密码"/></div>
                <div class="col-xs-4">&nbsp;</div>
                <div class="col-xs-4 RightR"><label class="control-label">确认密码：</label></div>
                <div class="col-xs-4 LeftL"><input type="password" name="user_configpwd" id="code_configpwd" class="form-control" placeholder="请输入确认密码"/></div>
                <div class="col-xs-4">&nbsp;</div>
                <div class="col-xs-4 RightR"><label class="control-label">手机：</label></div>
                <div class="col-xs-4 LeftL"><input type="number" name="user_phone" id="code_phone" class="form-control" maxlength="11" placeholder="请输入手机号码"/></div>
                <div class="col-xs-4">&nbsp;</div>
                <div class="col-xs-4 RightR"><label class="control-label"></label></div>
                <div class="col-xs-4 LeftL"><input type="checkbox" name="ck" class="position-top3" id="ck"/><label for="ck" class="mLeft10">阅读并接受</label><a class="article" href="javascript:;">《金融玩家协议》</a></div>
                <div class="col-xs-4">&nbsp;</div>
                <div class="col-xs-4 RightR"><label class="control-label"></label></div>
                <div class="col-xs-4"><button type="button" id="RegisterUser" class="btn btn-default register-border">注册</button></div>
                <div class="col-xs-4">&nbsp;</div>
        </form>
    </div>
    <div class="row register-body" id="login">
        <form name="userLogin">
                <div class="col-xs-4 RightR"><label class="control-label">账号：</label></div>
                <div class="col-xs-4 LeftL"><input type="text" name="user_nick" id="login_account1"  class="form-control" placeholder="请输入账号"/></div>
                <div class="col-xs-4">&nbsp;</div>
                <div class="col-xs-4 RightR"><label class="control-label">密码：</label></div>
                <div class="col-xs-4 LeftL"><input type="password" name="user_pwd" id="login_pwd1" class="form-control" placeholder="请输入密码"/></div>
                <div class="col-xs-4">&nbsp;</div>
                <div class="col-xs-4 RightR"><label class="control-label"></label></div>
                <div class="col-xs-4"><button type="button" id="LoginUser" class="btn btn-default register-border">登录</button></div>
                <div class="col-xs-4">&nbsp;</div>
        </form>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
     <script src="/customize/StockPlayer/Src/Register/Register2.js?v=20161123"></script>
</asp:Content>
