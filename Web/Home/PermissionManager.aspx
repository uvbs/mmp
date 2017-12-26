<%@ Page Title="" Language="C#" MasterPageFile="~/NoCntStyle.Master" AutoEventWireup="true"
    CodeBehind="PermissionManager.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.PermissionManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="box">
        <h3>
        </h3>
    </div>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        
        <tr>
            <td height="25" width="30%" align="right">
                权限链接 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtUrl" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
                菜单：
            </td>
            <td height="25" width="*" align="left">
                <hzh:MenuSelect ID="wucMenu" runat="server" />
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
                权限名称 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtPermissionName" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
                权限说明 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtPermissionDescription" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
            </td>
            <td height="25" width="*" align="left">
                <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="grvData" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="PermissionID" HeaderText="权限ID" SortExpression="PermissionID"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="Url" HeaderText="权限链接" SortExpression="Url" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="MenuID" HeaderText="菜单ID" SortExpression="MenuID" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="PermissionName" HeaderText="权限名称" SortExpression="PermissionName"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="PermissionDescription" HeaderText="权限说明" SortExpression="PermissionDescription"
                ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>
</asp:Content>
