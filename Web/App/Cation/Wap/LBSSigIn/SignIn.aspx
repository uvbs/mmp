<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.LBSSigIn.SignIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/App/Cation/Wap/Style/styles/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/Plugins/LayerM/need/layer.css" rel="stylesheet" type="text/css" />
    <title><%=model.Address %></title>
</head>
<body class="box padding10">
    <div style="padding:15px;font-size:16px;">
        <%=model.Address %>
    </div>
    <div style="padding:15px;font-size:14px;">
        当前时间：<span id="time"></span>
    </div>
   <%--  <div class="sendinfobox2" style="margin-top:40px;">
            <span class="mainbtn submitbtn SignBtn" onclick="signIn()">签到</span>
    </div>--%>


     <div class="sendinfobox2" style="margin-top:40px;">
            <span class="mainbtn submitbtn SignBtn" onclick="signIn()">签到</span>
    </div>
</body>
</html>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=224bd7222ce22c01673ff105ffb93fda"></script>
<script type="text/javascript">
    var geolocation;
    var geocoder;

    $(function () {

        window.alert = function (msg) {
            layer.open({
                content: msg,
                btn: ['OK'],
                time: 2
            });
        };
        $(time).html(new Date().toLocaleString().replace('GMT+8', '') + ' 星期' + '日一二三四五六'.charAt(new Date().getDay()));
        setInterval("document.getElementById('time').innerHTML=new Date().toLocaleString().replace('GMT+8','')+' 星期'+'日一二三四五六'.charAt(new Date().getDay());", 1000);

        AMap.plugin(['AMap.Geolocation'], function () {
            geolocation = new AMap.Geolocation({
                enableHighAccuracy: true, //是否使用高精度定位，默认:true
                convert: true,           //自动偏移坐标，偏移后的坐标为高德坐标，默认：true
                maximumAge: 0,           //定位结果缓存0毫秒，默认：0
                timeout: 3000          //超过10秒后停止定位，默认：无穷大
            });
            AMap.event.addListener(geolocation, 'complete', onComplete); //返回定位信息
            AMap.event.addListener(geolocation, 'error', onError);      //返回定位出错信息
        });

    })

    function onComplete(data) {
        //签到
        $.ajax({
            type: 'post',
            url: "/serv/api/signin/add.ashx",
            data: { address_id: '<%=addressId%>', longitude: data.position.getLng(), latitude: data.position.getLat() },
            dataType: "json",
            success: function (resp) {
                alert(resp.msg);
                if (resp.status) {
                    setTimeout(function () {
                        toLocation();
                    }, 2000);

                }
            }
        });
    }

    function GetDate() {
        var time = new Date();
        var date = new Date();
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var hour = date.getHours();
        var minute = date.getMinutes();
        var second = date.getSeconds();
        $("#time").text(year + '年' + month + '月' + day + '日  ' + hour + ':' + minute + ':' + second);
    }
    function onError(data) {    //解析定位错误信息
        switch (data.info) {
            case 'PERMISSION_DENIED':
                alert('浏览器拒绝定位');
                break;
            case 'POSITION_UNAVAILBLE':
                alert('浏览器无法获取当前位置');
                break;
            case 'NOT_SUPPORTED':
                alert('当前浏览器不支持定位功能');
                break;
            case 'TIMEOUT':
                alert('定位超时');
                break;
            case 'UNKNOWN_ERROR':
            default:
                alert('未知错误');
                break;
        }
    }
    var addressId = '<%=addressId%>';
    function signIn() {
        geolocation.getCurrentPosition();
    }
    function toLocation() {
        window.location.href = '<%=model.SignInSuccessUrl%>';
    }

</script>