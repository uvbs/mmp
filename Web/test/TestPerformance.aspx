<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPerformance.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Test.TestPerformance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        站点：<asp:TextBox ID="TextBox3" runat="server" Text="study"></asp:TextBox>
        月份(201612)：<asp:TextBox ID="TextBox4" runat="server" Text="201612"></asp:TextBox>
        <asp:Button ID="Button10" runat="server" Text="计算业绩" OnClick="Button10_Click"/>
    </div>
    </form>
</body>
</html>
