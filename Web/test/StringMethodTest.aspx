<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StringMethodTest.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Test.StringMethodTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css"></style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" runat="server" Width="442px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="移除最后的#RD" OnClick="Button1_Click" />
        <br />
        <asp:Label ID="Label1" runat="server" Text="结果："></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
