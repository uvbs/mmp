require.config({
	paths:{
		AudioPlay:"commonjs/AudioPlay.min",
		CheckBrowser:"commonjs/CheckBrowser.min"
	}
})
require(["AudioPlay","CheckBrowser","jquery"],function(AudioPlay,CheckBrowser,$) {
	//PC禁止访问
	var cb=new CheckBrowser();
	// if(cb.isPC){
	// 	document.body.innerHTML="请用微信打开";
	// 	return;
	// }

	//锁屏结束 开始动画
	var loadingscreen=function (container) {
		$(container).css({"opacity":"0","-webkit-transition":"all 0.5s ease-out"});
		$(container).bind("webkitTransitionEnd",function(){
			$(container).css({"display":"none"});
			// touchpic.init();
		})
	}
	//有音乐 音乐加载完 解锁屏幕
	if($("#myaudio")[0]){
		var audioplay=new AudioPlay("#myaudio","#musicbutton");
		//小米 iPhone 在微信浏览器可自动播放
		// if(cb.isWexin&&(cb.isiPhone||cb.isMI)&&($(window).width()<=360)){
		// 	$(".loadtext").html("Loading...")
		// 	audioplay.init();
		// }else{
		//屏幕触发播放
			// $(".loadtext").html("Loading...<br/>轻触屏幕开始动画")
			// $(document).one("touchstart",function(){
			// 	audioplay.init();
			// })
			setTimeout(function () {
				audioplay.init();
				// touchpic.init();
			},1000);
		// }
		//音乐播放解锁屏幕
		var musicloadfun=function(){
			this.removeEventListener('playing',musicloadfun,false);
			//播放音乐后开始加载内容
			loadingscreen("#loadingscreen");
		}
		$("#myaudio")[0].addEventListener("playing",musicloadfun,false);   

	}else{
	//无音乐 直接解锁屏幕
		loadingscreen("#loadingscreen");
	}

})