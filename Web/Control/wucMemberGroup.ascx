<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMemberGroup.ascx.cs"
    Inherits="ZentCloud.JubitIMP.Web.Control.wucMemberGroup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Panel ID="panelBtn" runat="server">
    <asp:LinkButton ID="lbtnAddGroup" runat="server" OnClick="lbtnAddGroup_Click">+增加分组</asp:LinkButton>
    <asp:LinkButton ID="lbtnDeleteGroup" runat="server" OnClick="lbtnDeleteGroup_Click">-删除分组</asp:LinkButton>
</asp:Panel>
<asp:Panel ID="panelAdd" Visible="false" runat="server" Font-Size="12px">
    <asp:TextBox ID="txtGroupName" runat="server"  Font-Size="12px"></asp:TextBox>
    <asp:TextBoxWatermarkExtender ID="txtGroupName_TextBoxWatermarkExtender" runat="server"
        Enabled="True" TargetControlID="txtGroupName" WatermarkText="点击输入新组名">
    </asp:TextBoxWatermarkExtender>
    <br />
    <asp:RequiredFieldValidator ID="rfvGroupName" runat="server" ValidationGroup="groupSave"
        ErrorMessage="*不能为空!" ForeColor="Red"  Font-Size="12px" ControlToValidate="txtGroupName"></asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="lbMsg" runat="server" Text="" ForeColor="Red"  Font-Size="12px"></asp:Label>
    <br />
    <asp:Button ID="btnSave" runat="server" ValidationGroup="groupSave" Text="保存"  Font-Size="12px" OnClick="btnSave_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="取消"  Font-Size="12px" OnClick="btnCancel_Click" />
</asp:Panel>
<br />
<asp:CheckBoxList ID="chkData" runat="server" RepeatDirection="Vertical">
</asp:CheckBoxList>

