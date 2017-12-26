
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenForm.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.OpenForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtOrderId" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" Height="21px" 
            onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>
