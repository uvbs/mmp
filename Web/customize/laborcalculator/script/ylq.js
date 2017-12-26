﻿requirejs.config({
	baseUrl:"src",
	paths:{
		jquery:"http://dev.comeoncloud.net/test/lib/jquery/jquery-2.1.1.min"
	}
})
requirejs(["jquery"],function($){

(function(){
	var lastTime = 0;
	var vendors = ['webkit', 'moz'];
	for(var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
		window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
		window.cancelAnimationFrame = window[vendors[x] + 'CancelAnimationFrame'] ||	// Webkit中此取消方法的名字变了
									window[vendors[x] + 'CancelRequestAnimationFrame'];
	}
	if (!window.requestAnimationFrame) {
		window.requestAnimationFrame = function(callback, element) {
			var currTime = new Date().getTime();
			var timeToCall = Math.max(0, 16.7 - (currTime - lastTime));
			var id = window.setTimeout(function() {
				callback(currTime + timeToCall);
			}, timeToCall);
			lastTime = currTime + timeToCall;
			return id;
		};
	}
	if (!window.cancelAnimationFrame) {
		window.cancelAnimationFrame = function(id) {
			clearTimeout(id);
		};
	}
}());

	$.fn.shownum = function(num) {
		num=num?num:0;
		num>0?this.addClass("resultnum2"):this.removeClass("resultnum2");
		var _this=this;

		var addnum=function(){
			var curnum=parseFloat(_this.text());
			// console.log(curnum);
			
			if((num-curnum)>10){
				var d=num-curnum;
				d=(d!=0)?d/10:d;
				curnum+=d;
				requestAnimationFrame(addnum)
			}else{
				curnum=num;
			}
			curnum=Math.round(curnum);
	   		_this.text(curnum)
		}

	   	requestAnimationFrame(addnum)

	   	//滚动
		var scroll=function(){
			var curscroll=document.body.scrollTop;
			var lastscroll=$(".mainresult")[0].offsetTop;

			if((lastscroll-curscroll)>10){
				// console.log(lastscroll)
				curscroll+=5;
	   			requestAnimationFrame(scroll)
			}else{
				curscroll=lastscroll-10;
			}

			document.body.scrollTop=parseInt(curscroll);
		}

	   	requestAnimationFrame(scroll)

	    return this;
	};

	function cnm (argument) {
		var GL=0;
		var DWGL=0;
		var YL=0;

		var SHYP=parseInt($("#GL").val());
		var BQ=parseInt($("#DWGL").val());
		var GL=parseInt($("#YL").val());
		
		if (GL<=10)
		{
			if (DWGL <=5)
			{
				YL = 3;
			}
			else
			{
				YL = 6;
			}
		}
		else
		{
			if (DWGL <= 5)
			{
				YL=6;
			}
			else if (DWGL <=10)
			{
				YL = 9;
			}
			else if (DWGL <=15)
			{
				YL = 12;
			}
			else if (DWGL <=20)
			{
				YL = 18
			}
			else
			{
				YL = 24;
			}
		}
		
		$("#YL").shownum(YL);
	}

	$(".mainbtn").bind("click",function(){
		cnm();
	})


})