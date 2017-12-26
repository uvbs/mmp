<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reg.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.Reg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/css/theme.css" />
    <link rel="stylesheet" type="text/css" href="/css/style.css" />
</head>
<body>
    <form id="form1" style="padding: 0px;" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container">
        <div id="wrapper">
            <div id="content">
                <div id="box">
                    <h3>
                        新用户注册</h3>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td height="25" width="30%" align="right">
                                登录名 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtUserID" runat="server" Width="350px"></asp:TextBox><span style="color: Red;">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="不能为空!"
                                    ControlToValidate="txtUserID" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                登陆密码 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="350px"></asp:TextBox><span
                                    style="color: Red;">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="不能为空!"
                                    ControlToValidate="txtPassword" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                确认密码 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="350px"></asp:TextBox><span
                                    style="color: Red;">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="不能为空!"
                                    ControlToValidate="txtPassword2" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="确认密码不一致！"
                                    ForeColor="Red" ControlToValidate="txtPassword2" ControlToCompare="txtPassword"
                                    Type="String" Operator="Equal"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                电子邮箱 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtEmail" runat="server" Width="350px"></asp:TextBox><span style="color: Red;">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="不能为空!"
                                    ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="电子邮箱地址格式不正确!"
                                    ForeColor="Red" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                手机号码 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:TextBox ID="txtPhone" runat="server" Width="350px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="手机号码格式不正确!"
                                    ForeColor="Red" ControlToValidate="txtPhone" ValidationExpression="^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$"></asp:RegularExpressionValidator>
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
                            <td height="25" colspan="2" width="*" align="center">
                                <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" />
                                <input type="reset" />
                                <asp:Button ID="btnQuit" runat="server" Text="返回登录" OnClick="lbtnQuit_Click" Visible="false"
                                    CausesValidation="false" />
                                <asp:LinkButton ID="lbtnQuit" runat="server" CausesValidation="False" Visible="false"
                                    OnClick="lbtnQuit_Click">返回登录</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="footer1">
            <div>
                <a href="http://www.miibeian.gov.cn">备案号:沪ICP备13000474号-1</a>&nbsp;&nbsp;&nbsp;
                Copyright © <a href="http://www.comeoncloud.com/">上海至云信息科技有限公司</a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
