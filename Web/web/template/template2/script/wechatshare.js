define(function(require, exports, module) {

    function Wechatshare(){}
    module.exports=Wechatshare;
    Wechatshare.prototype.init = function(title,desc,imgurl,shareurl) {
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.on('menu:share:appmessage', function (argv) {
                WeixinJSBridge.invoke('sendAppMessage', {
                    //"appid": appId,
                    "img_url": arr.imgUrl,
                    "img_width": "700",
                    "img_height": "420",
                    "link": arr.shareUrl,
                    "desc": arr.desc,
                    "title": arr.title
                }, function (res) {
                })
            });
            WeixinJSBridge.on('menu:share:timeline', function (argv) {
                WeixinJSBridge.invoke('shareTimeline', {
                    //"appid": appId,
                    "img_url": arr.imgUrl,
                    "img_width": "700",
                    "img_height": "420",
                    "link": arr.shareUrl,
                    "desc": arr.desc,
                    "title": arr.title
                }, function (res) {
                })
            });
            WeixinJSBridge.on('menu:share:weibo', function (argv) {
                WeixinJSBridge.invoke('shareWeibo', {
                    "content": arr.title,
                    "url": shareUrl
                }, function (res) {
                });
            });
        }, false)
    };

})


var desc = "我们是一家专注于微营销科技公司,我们现在的产品有微营销秘书，公众号定制开发，公众号代运营，至云之家。";
var title = "至云信息科技有限公司";
var imgUrl = "http://hf.jubit.cn/img/albee/top.png";
var shareUrl = "http://hf.jubit.cn/hongfeng/wap/GuaGuaalbee.aspx";
