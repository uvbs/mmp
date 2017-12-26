<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.Add" %>

<%@ Register Src="../Control/wucUserType.ascx" TagName="wucUserType" TagPrefix="uc1" %>
<%@ Register src="../Control/wucCheckRight.ascx" tagname="wucCheckRight" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--<uc2:wucCheckRight ID="wucCheckRight1" Pms="1" runat="server" />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="box">
                <h3>
                    添加新用户</h3>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td height="25" width="30%" align="right">
                            用户类型 ：
                        </td>
                        <td height="25" width="*" align="left">
                            <uc1:wucUserType ID="wucUserType" runat="server" />
                            
                        </td>
                    </tr>
                    <tr>
                        <td height="25" width="30%" align="right">
                            登录名 ：
                        </td>
                        <td height="25" width="*" align="left">
                            <asp:TextBox ID="txtUserID" runat="server" Width="350px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="txtUserID" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" width="30%" align="right">
                            登陆密码 ：
                        </td>
                        <td height="25" width="*" align="left">
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="350px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                ControlToValidate="txtPassword" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" width="30%" align="right">
                            确认密码 ：
                        </td>
                        <td height="25" width="*" align="left">
                            <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="350px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                                ControlToValidate="txtPassword2" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="确认密码不一致！"
                                ForeColor="Red" ControlToValidate="txtPassword2" ControlToCompare="txtPassword"
                                Type="String" Operator="Equal"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="30%" align="right" class="style1">
                            姓名 ：
                        </td>
                        <td width="*" align="left" class="style1">
                            <asp:TextBox ID="txtTrueName" runat="server" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" width="30%" align="right">
                            公司名 ：
                        </td>
                        <td height="25" width="*" align="left">
                            <asp:TextBox ID="txtCompany" runat="server" Width="350px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" width="30%" align="right">
                            手机号码 ：
                        </td>
                        <td height="25" width="*" align="left">
                            <asp:TextBox ID="txtPhone" runat="server" Width="350px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="手机号码格式不正确!"
                                ForeColor="Red" ControlToValidate="txtPhone" ValidationExpression="^(13[0-9]|14[5|7]|15[0-9]|18[0-9])\d{8}$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" width="30%" align="right">
                            电子邮箱 ：
                        </td>
                        <td height="25" width="*" align="left">
                            <asp:TextBox ID="txtEmail" runat="server" Width="350px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="电子邮箱地址格式不正确!"
                                ForeColor="Red" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" colspan="2" width="*" align="center">
                            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" />
                            <input type="reset" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
