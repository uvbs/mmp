<%@ Page Title="" Language="C#" MasterPageFile="~/NoCntStyle.Master" AutoEventWireup="true"
    CodeBehind="PmsGroupManager.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.PmsGroupManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td height="25" width="30%" align="right">
                组名称 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtGroupName" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25" width="30%" align="right">
                组说明 ：
            </td>
            <td height="25" width="*" align="left">
                <asp:TextBox ID="txtGroupDescription" runat="server" Width="200px"></asp:TextBox>
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
    <asp:GridView ID="grvData" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="GroupID" onselectedindexchanging="grvData_SelectedIndexChanging">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="GroupID" HeaderText="组ID" SortExpression="GroupID" 
                ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="GroupName" HeaderText="组名称" SortExpression="GroupName"
                ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="GroupDescription" HeaderText="组说明" SortExpression="GroupDescription"
                ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="PreID" HeaderText="所属ID" SortExpression="PreID" 
                ItemStyle-HorizontalAlign="Center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    
    <br />
    <asp:Button ID="btnSetPms" runat="server" Text="设置权限" 
        onclick="btnSetPms_Click" />
    
    <hzh:PmsSelect ID="wucPmsSelect" runat="server" />
    <asp:GridView ID="grvGroupAndPms" runat="server">
    </asp:GridView>
</asp:Content>
