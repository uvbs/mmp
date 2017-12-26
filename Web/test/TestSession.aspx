<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestSession.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Test.TestSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%
        foreach (var item in Session.Keys)
        {
            Response.Write(item.ToString()+"<br />");
        }
         %>
    </div>
        <div>
            Session名:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
            Session值:<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><br />
            <asp:Button ID="Button1" runat="server" Text="设置Session" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
