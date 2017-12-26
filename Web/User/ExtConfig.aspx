<%@ Page Title="外部配置" Language="C#" AutoEventWireup="true" CodeBehind="ExtConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.ExtConfig" %>


<html>
<head></head>
<body>
<%ZentCloud.BLLJIMP.Model.UserInfo userInfo = ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel(); %>
外部连接登录名：<h3 style="color:Black;font-weight:bold;"><%= ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(userInfo.UserID) %></h3>
<br />
外部连接密码：<h3  style="color:Black;font-weight:bold;"><%=ZentCloud.Common.DEncrypt.ZCEncrypt(userInfo.Password)%></h3>

</body>
</html>



