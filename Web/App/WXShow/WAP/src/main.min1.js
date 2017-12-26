require.config({
    paths: {
        AudioPlay: "commonjs/AudioPlay.min",
        CheckBrowser: "commonjs/CheckBrowser.min"
    }
}), require(["AudioPlay", "CheckBrowser"], function (i, n) {
    new n;
    var o = function (i) {
        $(i).css({ opacity: "0", "-webkit-transition": "all 0.5s ease-out" }), $(i).bind("webkitTransitionEnd", function () {
            $(i).css({ display: "none" }), touchpic.init();
            startAutoPlay();
        });
    };
    if ($("#myaudio")[0]) {
        var e = new i("#myaudio", "#musicbutton");

        o("#loadingscreen"); //$(".loadtext").html("Loading...<br/>轻触屏幕开始动画");
        setTimeout(function () {
            e.init();
            startAutoPlay();
        }, 2000);

        //o("#loadingscreen"), $(".loadtext").html("Loading...<br/>轻触屏幕开始动画"), $(document).one("touchstart", function () {
        //    e.init()
        //})
    } else o("#loadingscreen")
});
