
//重写Alert
var triggers, curTimeout;
var alert = function (msg, type, autoClose,fn) {
    if (triggers && curTimeout) {
        clearTimeout(curTimeout);
        $("#alertDialog").remove();
    }
    var dialogHTML = new StringBuilder();
    dialogHTML.Append("<span id='alertDialog'");
    dialogHTML.Append("><span></span><a href='javascript:void(0);' class='close' onclick='triggers.remove();clearTimeout(curTimeout);'>×</a></span>");
    $(document.body).append(dialogHTML.toString());
    $("#alertDialog").removeClass();
    if (type) {
        if (type == 2) {
            $("#alertDialog").addClass("errorDialog");
        }
        else if (type == 3) {
            $("#alertDialog").addClass("warningDialog");
        } else if (type == 4) {
            $("#alertDialog").addClass("warningErrorDialog");
        }
    }
    $("#alertDialog span").html(msg);
    var clientW = document.documentElement.clientWidth;
    var clientH = document.documentElement.clientHeight || window.innerHeight || document.body.clientHeight;
    var scrollH = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
    $("#alertDialog").css({
        top: "30%",
        left: ($(window).width() - $("#alertDialog").width()) / 2 + "px"
    });

    triggers = $("#alertDialog");
    if (typeof (autoClose) != "undefined") {
        if (autoClose) {
            curTimeout = setTimeout(function(){
                triggers.remove();
                clearTimeout(curTimeout);
                if (typeof (fn) != "undefined") {
                    fn();
                }
            }, 3000);
        }
    } else {
        curTimeout = setTimeout(function () {
            triggers.remove();
            clearTimeout(curTimeout);
            if (typeof (fn) != "undefined") {
                fn();
            }
        }, 3000);
    }
}
//封装StringBuilder
function StringBuilder() {
    this._string_ = new Array();
}
StringBuilder.prototype.Append = function (str) {
    this._string_.push(str);
}
StringBuilder.prototype.toString = function () {
    return this._string_.join("");
}