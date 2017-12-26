<%@ Page Language="C#" AutoEventWireup="true" EnableSessionState="ReadOnly" CodeBehind="QRCodeLogin.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Communal.QRCodeLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>二维码登录</title>
    <style type="text/css">
        html,body {
            margin: 0px;
            width:100%;
            height:100%;
        }
        table{
            width:100%;
            height:100%;
        }
        .info {
            font-size: 4em;
            text-align: center;
            vertical-align:middle;
            height:60%;
        }
        .ok{
            color: #21bd21;
        }
        .error {
            color: red;
        }
        .btn {
            text-align: center;
            vertical-align:middle;
        }

            .btn button {
                width: 60%;
                padding: 15px 10px;
                border-color: rgb(246, 131, 26);
                background-color: rgb(255, 142, 0);
                color: rgb(255, 255, 255);
                font-size: 3em;
                border-radius: 1em;
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
        <tr>
            <td class="btn">
    <%if (status) {%>
                <button type="button" class="btn-login" onclick="login()">登录</button>
    <%}%>
            </td>
        </tr>
    </table>
</body>
</html>
<script src="/lib/zepto/zepto.min.js"></script>
<script type="text/javascript">
    <%if (status)
      {%>
    var redis_key = '<%=redis_key%>';
    var ws;
    var port = '<% = port%>'
    function login() {
        var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);
        if (support == null) {
            $('.info').text('不支持WebSocket');
            $('.info').addClass('error').removeClass('ok');
            return;
        }
        // create a new websocket and connect
        // put id in path
        ws = new window[support]('ws://' + window.location.hostname + ':' + port + '/QRCodeLogin/' + redis_key);

        // when the connection is established, this method is called
        ws.onopen = function (e) {
            console.log(e);
        };

        ws.onmessage = function (evt) {
            var result = JSON.parse(evt.data);
            if (result.status === 1) {
                $('.info').text(result.msg);
                $('.btn-login').hide();
                ws.close();
            } else if (result.status === 2 || result.status === 9) {
                $('.info').text(result.msg);
                $('.info').addClass('error').removeClass('ok');
                $('.btn-login').hide();
                ws.close();
            }
        };
        // when the connection is closed, this method is called
        ws.onclose = function () { };
        ws.onerror = function (e) {
            //alert('WebSocket服务器连接失败', 5);
            //console.log('WebSocket服务器连接失败');
            $('.info').text('WebSocket服务器连接失败');
            $('.info').addClass('error').removeClass('ok');
        }
        return ws;
    }
    <%}%>
</script>
