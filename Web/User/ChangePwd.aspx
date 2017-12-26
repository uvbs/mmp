<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ChangePwd.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.ChangePwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td width="100px" align="center">
                        用户名:
                    </td>
                    <td width="*" align="left">
                        <%= ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetCurrUserID() %>
                    </td>
                </tr>
                <tr>
                    <td width="100px" align="center">
                        密码:
                    </td>
                    <td width="*" align="left">
                        <asp:TextBox runat="server" ID="txtPwd" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPwd" runat="server" ErrorMessage="*" ControlToValidate="txtPwd" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100px" align="center">
                        确认密码:
                    </td>
                    <td width="*" align="left">
                        <asp:TextBox runat="server" ID="txtPwdConfirm" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPwdConfirm" runat="server" ErrorMessage="*" ControlToValidate="txtPwdConfirm" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvdPwd" runat="server" ErrorMessage="两次输入的密码不一致!" ControlToCompare="txtPwd"
                            ControlToValidate="txtPwdConfirm" ForeColor="Red"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td width="100px" align="center">
                    </td>
                    <td width="*" align="left">
                        <asp:Button runat="server" ID="btnUpdate" Text="保存" OnClick="btnUpdate_Click" />
                        <hzh:BackBtn runat="server" />
                        <br />
                        <span style="color: Red;">注：更改密码后重新登录</span>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
