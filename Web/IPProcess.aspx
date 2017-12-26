<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IPProcess.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.IPProcess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        <asp:Button ID="Button2" runat="server" Text="第一名" OnClick="Button2_Click" />
        <asp:Button ID="Button3" runat="server" Text="第二名" OnClick="Button3_Click" />
        <asp:Button ID="Button4" runat="server" Text="第三名" OnClick="Button4_Click" />
        <asp:Button ID="Button5" runat="server" Text="第四名" OnClick="Button5_Click" />
    </div>
    </form>
</body>
</html>
