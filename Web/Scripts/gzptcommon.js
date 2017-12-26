$(function(){
	$(".articleform")[0]?$("input").bind("focus",function(){$(".articleform").addClass("movedown")}):"";
	$(".articleform")[0]?$("textarea").bind("focus",function(){$(".articleform").addClass("movedown")}):"";
	$(".morebtn").click(function(){
		//添加遮罩层
		if(!$(".closemore")[0]){
			$(".navbar").append("<div class='closemore'></div>");
			$("body").append("<div class='closemore'></div>");
			//遮罩层点击 隐藏.moreul跟遮罩层
			$(".closemore").bind("click",function(){
				$(".hovermore").removeClass("hovermore");
				$(".closemore").css({"display":"none"})
			})
		}
		//.morebtn点击 显示.moreul跟遮罩层
		$(this).addClass("hovermore");
		$(".closemore").css({"display":"block"})
	});

	document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
		WeixinJSBridge.call('hideToolbar');
	});
})
var editinfo = {
    init: function () {
        var s = $('.noedit');
        if (s.attr("readonly")) {
            s.removeAttr("readonly");
            $(".noeditli").addClass("editli");

            $("#txtArticleSourceWXHao").removeAttr("readonly");
            $("#txtArticleSourceWebSite").removeAttr("readonly");
            $(":radio").removeAttr("disabled");
            $(".text").text("取消编辑")
            $(".saveedit").css("display", "block");
            $(".saveedit").addClass("saveeditshow");


        } else {
            s.attr("readonly", "readonly");
            $("#txtArticleSourceWXHao").attr("readonly", "readonly");
            $("#txtArticleSourceWebSite").attr("readonly", "readonly");
            $(":radio").attr("disabled", "disabled");
            $(".editli").removeClass("editli");
            $(".text").text("编辑");
            $(".saveedit").removeClass("saveeditshow");
            setTimeout(function () { $(".saveedit").css("display", "none") }, 300);
        }
    }
}

//function sourcetype() {
//    $(".radio").each(function (index) {
//        if ($(this)[0].checked) {
//            $(".sourcetype").css("display", "none")
//            $(".sourcetype:eq(" + index + ")").css("display", "block")
//        }
//    })
//}




