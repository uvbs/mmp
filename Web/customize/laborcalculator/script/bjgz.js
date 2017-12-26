requirejs.config({
	baseUrl:"src",
	paths:{
		jquery:"http://dev.comeoncloud.net/test/lib/jquery/jquery-2.1.1.min"
	}
})
requirejs(["jquery"], function ($) {

    (function () {
        var lastTime = 0;
        var vendors = ['webkit', 'moz'];
        for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
            window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
            window.cancelAnimationFrame = window[vendors[x] + 'CancelAnimationFrame'] || // Webkit中此取消方法的名字变了
									window[vendors[x] + 'CancelRequestAnimationFrame'];
        }
        if (!window.requestAnimationFrame) {
            window.requestAnimationFrame = function (callback, element) {
                var currTime = new Date().getTime();
                var timeToCall = Math.max(0, 16.7 - (currTime - lastTime));
                var id = window.setTimeout(function () {
                    callback(currTime + timeToCall);
                }, timeToCall);
                lastTime = currTime + timeToCall;
                return id;
            };
        }
        if (!window.cancelAnimationFrame) {
            window.cancelAnimationFrame = function (id) {
                clearTimeout(id);
            };
        }
    } ());

    $.fn.shownum = function (num) {
        num = num ? num : 0;
        num > 0 ? this.addClass("resultnum2") : this.removeClass("resultnum2");
        var _this = this;

        var addnum = function () {
            var curnum = parseFloat(_this.text());
            // console.log(curnum);

            if ((num - curnum) > 10) {
                var d = num - curnum;
                d = (d != 0) ? d / 10 : d;
                curnum += d;
                requestAnimationFrame(addnum)
            } else {
                curnum = num;
            }
            curnum = Math.round(curnum);
            _this.text(curnum)
        }

        requestAnimationFrame(addnum)

        //滚动
        var scroll = function () {
            var curscroll = document.body.scrollTop;
            var lastscroll = $(".mainresult")[0].offsetTop;

            if ((lastscroll - curscroll) > 10) {
                // console.log(lastscroll)
                curscroll += 5;
                requestAnimationFrame(scroll)
            } else {
                curscroll = lastscroll - 10;
            }

            document.body.scrollTop = parseInt(curscroll);
        }

        requestAnimationFrame(scroll)

        return this;
    };

    function cnm(argument) {
        var SHYP = 0;
        var BQ = 0;
        var GL = 0;
        var BG = 0;

        var SHYP = parseInt($("#SHYP").val());
        var BQ = parseInt($("#BQ").val());
        var GL = parseInt($("#GL").val());
        var BG = parseInt($("#BG").val());

        switch (BQ) {
            case 1:
                if (GL == 1) {
                    BG = SHYP * 0.6;
                }
                else if (GL == 2 || GL == 3) {
                    BG = SHYP * 0.7;
                }
                else if (GL == 4 || GL == 5) {
                    BG = SHYP * 0.8;
                }
                else if (GL == 6 || GL == 7) {
                    BG = SHYP * 0.9;
                }
                else if (GL > 7) {
                    BG = SHYP * 1;
                }
                else {
                    BG = 0;
                }
                break;

            case 2:
                if (GL == 1) {
                    BG = SHYP * 0.4;
                }
                else if (GL == 2) {
                    BG = SHYP * 0.5;
                }
                else if (GL > 2) {
                    BG = SHYP * 0.6;
                }
                else {
                    BG = 0;
                }
                break;

            default: ;
                BG = 0;
        }

        $("#BG").shownum(BG);
    }

    $(".mainbtn").bind("click", function () {
        cnm();
    })


})