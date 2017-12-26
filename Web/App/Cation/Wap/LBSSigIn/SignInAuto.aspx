<%@ Page Language="C#" AutoEventWireup="false" EnableSessionState="ReadOnly" CodeBehind="SignInAuto.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.LBSSigIn.SignInAuto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <title></title>
    <link href="/App/Cation/Wap/Style/styles/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/Plugins/LayerM/need/layer.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
</head>
<body class="box padding10">
    <div class="sendinfobox2">
            <span class="mainbtn submitbtn SignBtn" onclick="signIn()">签到</span>
    </div>
</body>
</html>
<script type="text/javascript">
    Window.alert = function (msg) {
        layer.open({
            content: msg,
            btn: ['OK'],
            time:2
        });
    };
    function signIn() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition, showError, { timeout: 10000 });
        }
        else { alert("浏览器不支持Geolocation"); }
    }
    function showPosition(position) {
        var appendhtml = new StringBuilder();
        appendhtml.AppendFormat('http://api.map.baidu.com/geoconv/v1/?coords={0},{1}&from=1&to=5&ak=yuDmAsx9cGNUbdEGVwuQgz1f'
            , position.coords.longitude, position.coords.latitude);
        $.ajax({
            type: 'get',
            url: appendhtml.ToString(),
            dataType: "jsonp",
            success: function (data) {
                if (data.status == 0 && data.result && data.result.length > 0) {
                    $.ajax({
                        type: 'post',
                        url: "/serv/api/signin/addauto.ashx",
                        data: { longitude: data.result[0].x, latitude: data.result[0].y },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.errmsg) alert(resp.errmsg);
                            if (resp.msg) alert(resp.msg);
                        }
                    });
                }
                else {
                    alert("位置转换百度地址失败。");
                }
            }
        });
    }
    function showError(error) {
        switch (error.code) {
            case error.PERMISSION_DENIED:
                alert("用户拒绝对获取地理位置的请求。");
                break;
            case error.POSITION_UNAVAILABLE:
                alert("位置信息是不可用的。");
                break;
            case error.TIMEOUT:
                alert("请求用户地理位置超时。");
                break;
            case error.UNKNOWN_ERROR:
                alert("未知错误。");
                break;
        }
    }
</script>
