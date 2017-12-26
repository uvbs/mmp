<%@ Page Language="C#" AutoEventWireup="true" EnableSessionState="ReadOnly" CodeBehind="BindWXOpenId.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Communal.BindWXOpenId" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        html,body {
            margin: 0px;
            width:100%;
            height:100%;
        }
        table{
            width:100%;
            height:90%;
        }
        .info {
            font-size: 4em;
            text-align: center;
            vertical-align:middle;
        }
        .ok{
            color: #21bd21;
        }
        .error {
            color: red;
        }
    </style>
</head>
<body>
    <table>
        <tr>
            <td class="info <%= status? "ok":"error" %>">
                <%= msg %>
            </td>
        </tr>
    </table>
</body>
</html>
