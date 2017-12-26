requirejs.config({
	baseUrl:"src",
	paths:{
		jquery:"http://dev.comeoncloud.net/test/lib/jquery/jquery-2.1.1.min"
	}
})

requirejs(["jquery"],function($){
	
	$(".weixinsharebtn").bind("touchend",function(){
		var c=".weixinshareshade";
		if(!$(c)[0]){
			$("body").append("<div class='weixinshareshade'></div>");
			console.log($(c))
			$(c).css({
				"background-color":"rgba(0,0,0,0.8)",
				"position":"fixed",
				"width":"100%",
				"height":"100%",
				"top":"0px",
				"left":"0px",
				"z-index":"99",
				"background-image":"url(http://dev.comeoncloud.net/web/template/template8/styles/images/sharetext.png)",
				"background-size":"50%",
				"background-repeat":"no-repeat",
				"background-position":"right top"
			})
			$(c).bind("touchend",function(){
				$(c).css({"display":"none"})
			})
		}else{
			$(c).css({"display":"block"})
		}
	})

})