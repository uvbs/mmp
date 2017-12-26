
(function($) {
$.fn.alert = function(param) {
            var defaults = {
                msg: "",
                type: "1",
                timeout: 0,
                callback: function() {}
            };

            var options = $.extend(defaults, param);

            this.each(function() {
                var _this = $(this);

                _this.timer = null;
                _this.init = function() {
                    _this.clearTimer();
                    var alterDialog = $("<span/>").attr("class", "zyAlertDialog");
                    if (options.type) {
                        if (options.type == 2)
                            alterDialog.addClass("errorDialog");
                        else if (options.type == 3)
                            alterDialog.addClass("warningDialog");
                    }
                    var message = $("<span/>").html(options.msg).appendTo(alterDialog);
                    var close = $("<a/>").attr("href", "javascript:;").addClass("mdClose").text("×").click(function() {
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
                    $(".zyAlertDialog").css({
                        top: "30%",
                        left: ($(window).width() - $(".zyAlertDialog").width()) / 2 + "px"
                    });
                    if (options.timeout != 0) {
                        _this.timer = setTimeout(function() {
                            _this.close();
                        }, options.timeout);
                    }
                };
                _this.close = function(isInit) {
                    $(".zyAlertDialog").remove();
                    _this.callback();
                };
                _this.clearTimer = function() {
                    if (_this.timer != null) {
                        clearTimeout(_this.timer);
                        _this.timer = null;
                    }
                };
                _this.callback = function() {
                    if (options.callback)
                        options.callback();
                };
                _this.init();
            });
        };
})($);

