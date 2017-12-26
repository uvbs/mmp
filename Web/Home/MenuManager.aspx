<%@ Page Title="" Language="C#" MasterPageFile="~/NoCntStyle.Master" AutoEventWireup="true"
    CodeBehind="MenuManager.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.MenuManager" %>

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
                节点名称 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtNodeName" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
                链接 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtUrl" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
                所属菜单 ：
            </td>
            <td height="25" width="*" align="left">
                <hzh:MenuSelect ID="wucPreMenu" runat="server" />
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
                图标样式 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtICOCSS" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
                菜单排序 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtMenuSort" runat="server" Width="200px"></asp:TextBox>
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
    <asp:GridView ID="grvData" runat="server"  DataKeyNames="MenuID"
        onselectedindexchanging="grvData_SelectedIndexChanging">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
    </asp:GridView>

</asp:Content>
