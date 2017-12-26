requirejs.config({
	baseUrl:"src",
	paths:{
		jquery:"http://dev.comeoncloud.net/test/lib/jquery/jquery-2.1.1.min"
	}
})

requirejs(["jquery"],function($){

	var showcategory={
		categorybtnclick:0,
		show:function(){
			$(".box").css({"overflow-x":"hidden"})
			$(".topbar").css({"-webkit-transform":"translate3d(45%,0,0)","-webkit-transition":"0.5s ease-out"})
			$(".listbox").css({"-webkit-transform":"translate3d(45%,0,0)","-webkit-transition":"0.5s ease-out"})
			$(".submit").css({"-webkit-transform":"translate3d(45%,0,0)","-webkit-transition":"0.5s ease-out"})
			$("#screentouch").css({"-webkit-transform":"translate3d(45%,0,0)","display":"block"})
			this.categorybtnclick=1;
		},
		hide:function(){
			$(".topbar").css({"-webkit-transform":"translate3d(0%,0,0)","-webkit-transition":"0.5s ease-out"})
			$(".listbox").css({"-webkit-transform":"translate3d(0%,0,0)","-webkit-transition":"0.5s ease-out"})
			$(".submit").css({"-webkit-transform":"translate3d(0%,0,0)","-webkit-transition":"0.5s ease-out"})
			$("#screentouch").css({"display":"none"})

			this.categorybtnclick=0;			
		},
		run:function(){
			if(!this.categorybtnclick){
				this.show();
			}else{
				this.hide();
			}
		},
		main:function(){
			var _this=this;
			$("#categorybtn").bind("click",function(e){
				_this.run()
			})
			$("#categorybtn").bind("touchend",function(e){
				e.preventDefault()
				$("#categorybtn").unbind("click")
				_this.run()
			})
			$("#screentouch").bind("click",function(e){
				_this.run()
			})
			$("#screentouch").bind("touchstart",function(e){
				e.preventDefault()
				$("#categorybtn").unbind("click")
				_this.run()
			})
		},
		init:function(){
			this.main();
		}
	}


	showcategory.init()
})