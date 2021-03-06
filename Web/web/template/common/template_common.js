var templateModule = function (e) {
    this.url = e,
    this.weixinsharedata = {
        desc: "",
        title: "",
        imgUrl: "",
        shareUrl: ""
    }
}
;
templateModule.prototype.getQueryString = function (e) {
    var t = new RegExp("(^|&)" + e + "=([^&]*)(&|$)", "i")
      , n = window.location.search.substr(1).match(t);
    return null != n ? unescape(n[2]) : null
}
,
templateModule.prototype.weixinShare = function () {
    var e = this;
    document.addEventListener("WeixinJSBridgeReady", function () {
        WeixinJSBridge.on("menu:share:appmessage", function () {
            WeixinJSBridge.invoke("sendAppMessage", {
                img_url: e.weixinsharedata.imgUrl,
                img_width: "400",
                img_height: "400",
                link: e.weixinsharedata.shareUrl,
                desc: e.weixinsharedata.desc,
                title: e.weixinsharedata.title
            }, function () { })
        }),
        WeixinJSBridge.on("menu:share:weibo", function () {
            WeixinJSBridge.invoke("shareWeibo", {
                content: e.weixinsharedata.title,
                url: e.weixinsharedata.shareUrl
            }, function () { })
        })
    }, !1)
}
,
templateModule.prototype.addfooternav = function (e) {
    var t = this.url;
    $.ajax({
        url: t,
        data: {
            action: "gettoolbarlist"
        },
        dataType: "jsonp",
        jsonpCallback: "addfooternav",
        success: function (t) {
            if ("function" == typeof e)
                e(t);
            else {
                var n = t.length
                  , r = 1;
                $("body").append("<nav class='footer' id='navbar'></nav>");

                //for (i in t)
                //    $("#navbar").append("<a class='navbtn navbtn" + r + "' href='" + (t[i].ToolBarTypeValue ? t[i].ToolBarTypeValue : "javascript:void(0);") + "'></a>"),
                //    $(".navbtn" + r).append("<span class='icon " + t[i].ToolBarImage + "' alt='" + t[i].ToolBarDescription + "' class='pic'/>"),
                //    $(".navbtn" + r).append("<span class='text'>" + t[i].ToolBarName + "</span>"),
                //    $(".navbtn" + r).css({
                //        width: 100 / n + "%"
                //    }),
                //    r++

                for (var i = 0; i < t.length; i++) {
                    $("#navbar").append("<a class='navbtn navbtn" + r + "' href='" + (t[i].ToolBarTypeValue ? t[i].ToolBarTypeValue : "javascript:void(0);") + "'></a>"),
                    $(".navbtn" + r).append("<span class='icon " + t[i].ToolBarImage + "' alt='" + t[i].ToolBarDescription + "' class='pic'/>"),
                    $(".navbtn" + r).append("<span class='text'>" + t[i].ToolBarName + "</span>"),
                    $(".navbtn" + r).css({
                        width: 100 / n + "%"
                    }),
                    r++
                }

            }
        }
    })
}
,
templateModule.prototype.addslider = function (e) {
    var t = this.url;
    $.ajax({
        url: t,
        data: {
            action: "getprojectorlist"
        },
        dataType: "jsonp",
        jsonpCallback: "addslider",
        success: function (t) {
            e(t)
        }
    })
}
,
templateModule.prototype.addmodule = function (e) {
    var t = this.url;
    $.ajax({
        url: t,
        data: {
            action: "getnavigatelist"
        },
        dataType: "jsonp",
        jsonpCallback: "addmodule",
        success: function (t) {
            if ("function" == typeof e)
                e(t);
            else {
                var n = 1;
                //for (i in t) {
                //    if ("链接" === t[i].NavigateType)
                //        var r = t[i].NavigateTypeValue;
                //    else if ("分类" === t[i].NavigateType)
                //        var r = document.location.href.match(/\S*\//) + "list.aspx?cateid=" + t[i].NavigateTypeValue;
                //    else if ("图文" === t[i].NavigateType)
                //        var r = t[i].NavigateTypeValue;
                //    $("#category").append("<a class='cat cat" + n + "' href='" + r + "'></a>"),
                //    $(".cat" + n).append("<img src='" + t[i].NavigateImage + "' alt='" + t[i].NavigateDescription + "' class='pic'/>"),
                //    $(".cat" + n).append("<span class='text'>" + t[i].NavigateName + "</span>"),
                //    n++
                //}

                for (var i = 0; i < t.length; i++) {
                    if ("链接" === t[i].NavigateType)
                        var r = t[i].NavigateTypeValue;
                    else if ("分类" === t[i].NavigateType)
                        var r = document.location.href.match(/\S*\//) + "list.aspx?cateid=" + t[i].NavigateTypeValue;
                    else if ("图文" === t[i].NavigateType)
                        var r = t[i].NavigateTypeValue;
                    else
                        var r = t[i].NavigateTypeValue;
                    $("#category").append("<a class='cat cat" + n + "' href='" + r + "'></a>"),
                    $(".cat" + n).append("<img src='" + t[i].NavigateImage + "' alt='" + t[i].NavigateDescription + "' class='pic'/>"),
                    $(".cat" + n).append("<span class='text'>" + t[i].NavigateName + "</span>"),
                    n++
                }

            }
        }
    })
}
,
templateModule.prototype.addarticlelist = function (e) {
    var t = this
      , n = t.url;
    $.ajax({
        url: n,
        data: {
            action: "getarticlelist",
            cateid: t.getQueryString("cateid")
        },
        dataType: "jsonp",
        jsonpCallback: "addmodule",
        success: function (t) {
            e(t)
        }
    })
}
,
templateModule.prototype.articlelisthref = function (e) {
    if (e)
        var e = e.split("/")
          , t = document.location.href.match(/\S*\//) + "detail.aspx?articleid=" + e[e.length - 2];
    else
        var t = "javascript:void(0);";
    return t
}
,
templateModule.prototype.addarticleconcent = function (e) {
    var t = this
      , n = t.url;
    $.ajax({
        url: n,
        data: {
            action: "getsinglearticle",
            articleid: t.getQueryString("articleid")
        },
        dataType: "jsonp",
        jsonpCallback: "getarticlelist",
        success: function (t) {
            e(t)
        }
    })
}
,
templateModule.prototype.addsiteinfo = function (e) {
    var t = this
      , n = t.url;
    $.ajax({
        url: n,
        data: {
            action: "getconfig"
        },
        dataType: "jsonp",
        jsonpCallback: "getsiteinfo",
        success: function (t) {
            e(t)
        }
    })
}
;
