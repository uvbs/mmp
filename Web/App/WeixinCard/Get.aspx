<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Get.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WeixinCard.Get" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <script src="/Scripts/jquery2.1.1.js"></script>
    <script>

        $.ajax({
            url: "http://" + location.host + "/serv/wxapi.ashx",
            data: {
                action: "getjsapiconfig",
                cardid: "p99IZtxeq7bi7Df3jG17JPmEOBAA",
                url: location.href
            },
            dataType: "json",
            success: function (wxapidata) {
                wx.config({
                    debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                    appId: wxapidata.appId, // 必填，公众号的唯一标识
                    timestamp: wxapidata.timestamp, // 必填，生成签名的时间戳
                    nonceStr: wxapidata.nonceStr, // 必填，生成签名的随机串
                    signature: wxapidata.signature,// 必填，签名，见附录1
                    jsApiList: [
                        "onMenuShareTimeline",
                        "onMenuShareAppMessage",
                        "onMenuShareQQ",
                        "onMenuShareWeibo",
                        "startRecord",
                        "stopRecord",
                        "onVoiceRecordEnd",
                        "playVoice",
                        "pauseVoice",
                        "stopVoice",
                        "onVoicePlayEnd",
                        "uploadVoice",
                        "downloadVoice",
                        "chooseImage",
                        "previewImage",
                        "uploadImage",
                        "downloadImage",
                        "translateVoice",
                        "getNetworkType",
                        "openLocation",
                        "getLocation",
                        "hideOptionMenu",
                        "showOptionMenu",
                        "hideMenuItems",
                        "showMenuItems",
                        "hideAllNonBaseMenuItem",
                        "showAllNonBaseMenuItem",
                        "closeWindow",
                        "scanQRCode",
                        "chooseWXPay",
                        "openProductSpecificView",
                        "addCard",
                        "chooseCard",
                        "openCard"
                    ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                });

                wx.addCard({
                    cardList: [{
                        cardId: 'p99IZtxeq7bi7Df3jG17JPmEOBAA',
                        cardExt: {
                            card_id: "p99IZtxeq7bi7Df3jG17JPmEOBAA",
                            timestamp: wxapidata.timestamp,
                            nonce_str: wxapidata.nonceStr,
                            api_ticket:wxapidata.cardTicket,
                            signature: wxapidata.cardSign
                        }
                    }], // 需要添加的卡券列表
                    success: function (res) {
                        var cardList = res.cardList; // 添加的卡券列表信息

                    },
                    fail: function (res) {

                        alert(res);
                    }
                });








            },
            error: function (errmes) {
                console.dir(errmes)
            }
        })






    </script>
</body>

</html>
