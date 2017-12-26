<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestApp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Test.TestApp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width:100%;">
            <tr>
                <td style="width:100px;">
                    userId<span style="color:red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" style="width:500px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    title<span style="color:red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server" style="width:500px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    text<span style="color:red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server" style="width:500px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    link<span style="color:red;">*</span>
                </td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server" style="width:500px;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                        <asp:Button ID="Button1" runat="server" Text="发送" OnClick="Button1_Click" />
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
