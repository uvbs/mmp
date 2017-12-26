<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoPms.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Error.NoPms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>臻云短信平台</title>
    <link rel="stylesheet" type="text/css" href="/css/theme.css" />
    <link rel="stylesheet" type="text/css" href="/css/style.css" />
</head>
<body>
    <form id="form1" style="padding: 0px;" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="container">
        <div id="header">
            <h2>
                臻云短信平台</h2>
            <div id="topmenu">
                <ul style="width: 960px;">
                    <li style="float: right;">
                        <asp:LinkButton ID="lbtnQuit" runat="server" CausesValidation="False" OnClick="lbtnQuit_Click">重新登录</asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
            <div id="content">
                <p>
                    <div id="box">
                        <table style="margin: auto;">
                            <tr>
                                <td style="text-align: center;">
                                    <span style="color: Red; font-weight: bolder; font-size: 18px;">对不起，您没有权限访问本栏目！</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:center;">
                                    <img alt="" src="/img/nopms.png" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </p>
                <p>
                    &nbsp;</p>
            </div>
        </div>
        <div id="footer1">
            <div>
                Power by <a target="_blank" href="http://www.comeoncloud.com">臻云科技</a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
