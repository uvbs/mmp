<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Info.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Outlets.Comm.Info" %>
 <% 
     StringBuilder strHtml = new StringBuilder();
    %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-app-status-bar-style" content="default" />
    <meta name="format-detection" content="telephone=no" />
    <title><%=nInfoJtoken["ActivityName"].ToString() %></title>
    <link href="http://file.comeoncloud.net/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="http://file.comeoncloud.net/lib/layer.mobile/need/layer.css" rel="stylesheet" />
    <link href="http://file.comeoncloud.net/css/global-m.css" rel="stylesheet" />
    <link href="/App/Outlets/Outlets.css" rel="stylesheet"/>
    <script src="http://file.comeoncloud.net/lib/zepto/zepto.min.js"></script>
    <script src="http://file.comeoncloud.net/lib/vue/vue.min.js"></script>
    <script src="http://file.comeoncloud.net/lib/layer.mobile/layer.m.js" charset="utf-8"></script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script src="http://file.comeoncloud.net/Scripts/global-m.js" charset="utf-8"></script>
    <script src="http://file.comeoncloud.net/Scripts/StringBuilder.Min.js" charset="utf-8"></script>
</head>
<body v-bind:class="{wrapQQBrowser:isQQBrowser}">
<div class="wrapOffer">
    <div class="wrapInfo">
        <div class="list mBottom0">
            <div class="item item-divider item-reset-detail">
                <h2 class="whitespace" v-text="ActivityName"></h2>
                <p v-if="Tags"><span class="font12 whitespace" v-text="Tags"></span></p>
            </div>
        </div>
        <div class="list">
            <div class="item avatar" v-if="ThumbnailsPath">
                <img class="full-image" :src="ThumbnailsPath">
            </div>
            <div class="item item-icon-left item-icon-right item-reset-two font14" 
        <% if ((typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2) && nInfoJtoken["UserLongitude"] != null && !string.IsNullOrWhiteSpace(nInfoJtoken["UserLongitude"].ToString()))
        {%>
               @click="showMap()"
        <%}%>
                >
                <i class="icon iconfont icon-place place"></i>
                <span class="color5BD5BE" v-if="ActivityAddress" v-text="ActivityAddress"></span>
                <i class="icon iconfont icon-arrowright arrow"></i>
            </div>
            <div class="item item-icon-left item-icon-right item-reset-two" v-if="ActivityPhone">
                <i class="icon iconfont icon-leftphoneshi place"></i>
                <a class="color5BD5BE" href="tel:{{ActivityPhone}}" v-text="ActivityPhone"></a>
            </div>
        </div>
    <% 
    strHtml = new StringBuilder();
    List<string> others = new List<string>() { "JuActivityID", "ThumbnailsPath", "UserLongitude", "UserLatitude", "ActivityName", "Tags", "ActivityAddress", "ActivityPhone" };
    foreach (var item in formField.Where(p=>!others.Contains(p.Field)))
    {
        if (nInfoJtoken[item.Field] != null && !string.IsNullOrWhiteSpace(nInfoJtoken[item.Field].ToString()))
        {
            strHtml.AppendLine("<div class=\"list\"><div class=\"item item-reset-service\">");
            strHtml.AppendLine(string.Format("{0}</div><div class=\"item whitespace font14 item-reset-main\" v-text=\"{1}\">", item.MappingName, item.Field));
            strHtml.AppendLine("</div></div>");
        } 
    }
    this.Response.Write(strHtml.ToString());
    %>
    </div>

</div>
</body>
</html>
<script type="text/javascript">
    var vm;
    $(function () {
        vm = new Vue({
            el: 'body',
            data: {
                isQQBrowser: navigator.userAgent && navigator.userAgent.indexOf('MQQBrowser') > 0,
    <% 
    strHtml = new StringBuilder();
    foreach (var item in formField)
    {
        if (nInfoJtoken[item.Field] != null) strHtml.AppendLine(string.Format("{0}:'{1}',", item.Field, nInfoJtoken[item.Field].ToString()));
    }
    this.Response.Write(strHtml.ToString());
    %>
                shareInfo: {
                    title: '<%=nInfoJtoken["ActivityName"].ToString() %>', // 分享标题
                    desc: '<%= nInfoJtoken["Summary"] ==null ?"":nInfoJtoken["Summary"].ToString()  %>', // 分享描述
                    link: '', // 分享链接
                    imgUrl: '<%= nInfoJtoken["ThumbnailsPath"] ==null ?"":nInfoJtoken["ThumbnailsPath"].ToString() %>', // 分享图标
                    type: '', // 分享类型,music、video或link，不填默认为link
                    dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                },
                rd:0
            },
            methods: {
        <% if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
                showMap: showMap,
    <%}%>
                init: init
            }
        });
        vm.init();
    });
    function init() {
        if (RegExp("MicroMessenger").test(navigator.userAgent)) {
            WXInit();
        }
    }
    function WXInit() {
        $.ajax({
            url: "http://" + location.host + "/serv/wxapi.ashx",
            data: {
                action: "getjsapiconfig",
                url: location.href
            },
            dataType: "json",
            success: function (wxapidata) {
                if (wxapidata.appId && wxapidata.timestamp &&
                        wxapidata.nonceStr && wxapidata.signature) {
                    wx.config({
                        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
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
                    WXReadly();
                }
            },
            error: function (errmes) {
            }
        })
    }
    function WXReadly() {
        //重新配置
        wx.ready(function () {
            //重写朋友分享
            wx.onMenuShareTimeline({
                title: vm.shareInfo.title, // 分享标题
                link: vm.shareInfo.link, // 分享链接
                imgUrl: vm.shareInfo.imgUrl, // 分享图标
                success: function () {
                },
                cancel: function () {
                }
            });
            //重写朋友圈分享
            wx.onMenuShareAppMessage({
                title: vm.shareInfo.title, // 分享标题
                desc: vm.shareInfo.desc, // 分享描述
                link: vm.shareInfo.link, // 分享链接
                imgUrl: vm.shareInfo.imgUrl, // 分享图标
                type: vm.shareInfo.type, // 分享类型,music、video或link，不填默认为link
                dataUrl: vm.shareInfo.dataUrl, // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                },
                cancel: function () {
                }
            });
            //重写QQ分享
            wx.onMenuShareQQ({
                title: vm.shareInfo.title, // 分享标题
                desc: vm.shareInfo.desc, // 分享描述
                link: vm.shareInfo.link, // 分享链接
                imgUrl: vm.shareInfo.imgUrl, // 分享图标
                success: function () {
                },
                cancel: function () {
                }
            });
            //重写微博分享
            wx.onMenuShareWeibo({
                title: vm.shareInfo.title, // 分享标题
                desc: vm.shareInfo.desc, // 分享描述
                link: vm.shareInfo.link, // 分享链接
                imgUrl: vm.shareInfo.imgUrl, // 分享图标
                success: function () {
                },
                cancel: function () {
                }
            });
        });
    }
        <% if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
    function showMap(){
        var strHtml = new StringBuilder();
        strHtml.AppendFormat('http://m.amap.com/navi/?dest={0},{1}', vm.UserLongitude, vm.UserLatitude);
        if (vm.ActivityName) strHtml.AppendFormat('&destName={0}', encodeURIComponent(vm.ActivityName));
        if (vm.ActivityPhone) strHtml.AppendFormat('||电话：{0}', encodeURIComponent(vm.ActivityPhone));
        if (vm.ActivityAddress) strHtml.AppendFormat('||地址：{0}', encodeURIComponent(vm.ActivityAddress));
        strHtml.AppendFormat('&key={0}', zymmp.gaodeCompentKey);
        window.location.href = strHtml.ToString();
    }
    <%}%>
</script>
