<%@ Page Title="注册中心" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/customize/StockPlayer/StockPlayerSite.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.StockPlayer.Src.Register.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/customize/StockPlayer/Src/Register/Register.css?v=2016102004" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentBodyCenter" runat="server">

    <div class="register">
        <div class="register-head">
            <button type="button"  value="user" class="btn btn-default btn-user">用户注册</button>
            <button type="button"  value="company" class="btn btn-default btn-company">公司注册</button>
        </div>
    </div>
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
                 <div class="col-xs-4 RightR"><label class="control-label">验证码：</label></div>
                <div class="col-xs-4 LeftL"><input type="number" name="user_code" class="form-control inline-block Width140 text-code" placeholder="请输入验证码"/>
                    <button type="button" class="btn btn-default btn-code" id="getcode">获取验证码</button>
                </div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label"></label></div>
                <div class="col-xs-4 LeftL"><input type="checkbox" name="ck" class="position-top3" id="ck"/><label for="ck" class="mLeft10">阅读并接受</label><a class="article" href="javascript:;">《金融玩家协议》</a></div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label"></label></div>
                <div class="col-xs-4"><button type="button" id="RegisterUser" class="btn btn-default register-border">注册</button></div>
                <div class="col-xs-4">&nbsp;</div>
        </form>
    </div>
    <div class="row register-body" id="company" style="display:none;">
            <form name="companyRegister">
                 <div class="col-xs-4 RightR"><label class="control-label">公司名称：</label></div>
                <div class="col-xs-4 LeftL"><input type="text" id="company_name" class="form-control" placeholder="请输入公司名称"/></div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label">密码：</label></div>
                <div class="col-xs-4 LeftL"><input type="password" id="company_pwd" class="form-control" placeholder="请输入密码"/></div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label">确认密码：</label></div>
                <div class="col-xs-4 LeftL"><input type="password" id="company_confirmpwd" class="form-control" placeholder="请输入确认密码"/></div>
                <div class="col-xs-4">&nbsp;</div>
        
                <div class="col-xs-4 RightR lincences"><label class="control-label label-lince">营业执照：</label></div>
                <div class="col-xs-4 LeftL img-width">
                    <img src="http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zhizhao.png" id="img-lince"  class="img-rounded img-license">
                    <%--<a href="javascript:;" class="a-upload">上传图片<input type="file" name="file" accept="image/png,image/gif,image/jpeg,image/jpg" id="exampleInputFile"></a>--%>
                    <input type="file" id="licensePath" accept="image/*" class="licensePath" name="file1"/>
                </div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label"></label></div>
                <div class="col-xs-4 LeftL  text-width"><input type="checkbox" class="position-top3 cb-box"  id="cb"/><label for="cb" class="mLeft10">阅读并接受</label><a class="article" href="javascript:;">《金融玩家协议》</a></div>
                <div class="col-xs-4">&nbsp;</div>
                 <div class="col-xs-4 RightR"><label class="control-label"></label></div>
                <div class="col-xs-4 mLeft310"><button type="button" id="RegisterComapny" class="btn btn-default register-border1">注册</button></div>
                <div class="col-xs-4">&nbsp;</div>
            </form>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="buttom" runat="server">
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js"></script>
    <script src="/customize/StockPlayer/Src/Register/Register.js?v=20161123"></script>
</asp:Content>
