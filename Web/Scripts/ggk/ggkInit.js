$(function () {
    $("#divcontent").show();
    var obj = $("#divcontent");
    var offset = obj.offset();
    var offsetleft = offset.left;
    var offsetright = offset.right;
    var useragent = window.navigator.userAgent.toLowerCase();
    var statu = "enable";
    var sp;
    $("#divcontent").hide();
    sp = $("#scratchpad").wScratchPad({
        width: $(window).width(),
        height: obj.height() + 9,
        image2: "/ggk/image/overlay1.jpg",
        color: "#a9a9a7",
        size: 50,
        scratchMove: function (e, percent) {
            if (percent > 50) {
                this.clear();
                $("#scratchpad").remove();
            }
        }
    });
    $("#scratchpad").css({ "top": offset.top });
   
    setInterval("setloc()", 100);


    $('#bgmusic').change(function () {
        if ($(this).attr('checked') == "checked" || $(this).attr('checked') == "true") {
            $('audio').get(0).play();
        }
        else {
            $('audio').get(0).pause();
        }
    });
});


function setloc() {
    $("#divcontent").show();
    var obj = $("#divcontent");
    var offset = obj.offset();
    var offsetleft = offset.left;
    var offsetright = offset.right;
    $("#scratchpad").css({ "top": offset.top });

}