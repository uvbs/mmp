function setFocus(id, yes) {
    var obj = document.getElementById(id);
    if (yes) {
        obj.focus();
        obj.value = obj.value;
    } else {
        obj.blur();
    }
}

function removeHtmlTag(str) {
    //return input.replace(/<[^>]+>/g, "");

    //正则调不通，通过字符串方式移除script内容
    str = str.toLowerCase();

    while (str.indexOf('</script>') > 0) {
        str = str.substring(str.indexOf('</script>') + 9);
    }
    while (str.indexOf('</style>') > 0) {
        str = str.substring(str.indexOf('</style>') + 8);
    }

    str = str.replace(/<script.*?>.*?<\/script>/ig, '');
    str = str.replace(/<\/?[^>]*>/g, ''); //去除HTML tag
    str = str.replace(/[ | ]*\n/g, '\n'); //去除行尾空白
    str = str.replace(/\n[\s| | ]*\r/g, '\n'); //去除多余空行
    str = str.replace(/&nbsp;/ig, ''); //去掉&nbsp;
    return str;
}

function briefingStr(str,maxLength){
    if (str.length>maxLength)
        str = str.substr(0,maxLength)+"...";
    return str;
}

(function ($) {
    $.fn.alert = function (param) {
        var defaults = {
            msg: "",
            type: "1",
            timeout: 0,
            callback: function () { }
        };

        var options = $.extend(defaults, param);

        this.each(function () {
            var _this = $(this);

            _this.timer = null;
            _this.init = function () {
                _this.clearTimer();
                var alterDialog = $("<span/>").attr("class", "zyAlertDialog");
                if (options.type) {
                    if (options.type == 2)
                        alterDialog.addClass("errorDialog");
                    else if (options.type == 3)
                        alterDialog.addClass("warningDialog");
                }
                var message = $("<span/>").html(options.msg).appendTo(alterDialog);
                var close = $("<a/>").attr("href", "javascript:;").addClass("mdClose").text("×").click(function () {
                    _this.close();
                    _this.clearTimer();
                }).appendTo(alterDialog);
                $(document.body).append(alterDialog);
                var clientW = document.documentElement.clientWidth;
                var clientH = document.documentElement.clientHeight || window.innerHeight || document.body.clientHeight;
                var scrollH = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
                $(".zyAlertDialog").css({
                    top: "30%",
                    left: ($(window).width() - $(".zyAlertDialog").width()) / 2 + "px"
                });
                if (options.timeout != 0) {
                    _this.timer = setTimeout(function () {
                        _this.close();
                    }, options.timeout);
                }
            };
            _this.close = function (isInit) {
                $(".zyAlertDialog").remove();
                _this.callback();
            };
            _this.clearTimer = function () {
                if (_this.timer != null) {
                    clearTimeout(_this.timer);
                    _this.timer = null;
                }
            };
            _this.callback = function () {
                if (options.callback)
                    options.callback();
            };
            _this.init();
        });
    };
})($);

window.alert = function (msg, type, timeout, fn) {
    $('body').alert({
        msg: msg,
        type: (typeof type) == 'undefined' ? '1' : type,
        timeout: (typeof timeout) == 'undefined' ? 2500 : timeout,
        callback: fn
    });
};

Date.prototype.format = function(format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1,
                RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
    return format;
};
