requirejs.config({
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
		var SCJT=0;
		var SCBZ=0;
		var SCJY=0;
		var YLBZ=0;
		var SHHL=0;

		var GSJB=parseInt($("#GSJB").val());
		var STZK=parseInt($("#STZK").val());
		var LDGX=parseInt($("#LDGX").val());
		var SEYP=parseInt($("#SEYP").val());
		var SEJF=parseInt($("#SEJF").val());
		var SHYP=parseInt($("#SHYP").val());

		switch(GSJB){
			case 1:
			SCJT=SEJF*0.90;
			SCBZ=SEJF*27;
			break;

			case 2: 
			SCJT=SEJF*0.85;
			SCBZ=SEJF*25;
			break;

			case 3:
			SCJT=SEJF*0.80;
			SCBA=SEJF*23;
			break;

			case 4:
			SCJT=SEJF*0.75;
			SCBZ=SEJF*21;
			break;

			case 5:
				if(LDGX){
					SCJT=SEJF*0.70;
					SCBZ=SEJF*18;
				}else{
					YLBZ=SEYP*18;
					SCJY=SEYP*18;
				}
			break;

			case 6:
				if(LDGX){
					SCJT=SEJF*0.60;
					SCBZ=SEJF*16;
				}else{
					YLBZ=SEYP*15;
					SCJY=SEYP*15;
				}
			break;

			case 7:
				if(LDGX){
					SCBZ=SEJF*13;
				}else{
					YLBZ=SEYP*12;
					SCJY=SEYP*12;
				}
			break;

			case 8:
				if(LDGX){
					SCBZ=SEJF*11;
				}else{
					YLBZ=SEYP*9;
					SCJY=SEYP*9;
				}
			break;

			case 9:
				if(LDGX){
					SCBZ=SEJF*9;
				}else{
					YLBZ=SEYP*6;
					SCJY=SEYP*6;
				}

			break;

			case 10:
				if(LDGX){
					SCBZ=SEJF*7;
				}else{
					YLBZ=SEYP*3;
					SCJY=SEYP*3;
				}
			break;
			default:;
		}

		switch(STZK){
			case 1:
			SHHL=SHYP*0.50;
			break;

			case 2:
			SHHL=SHYP*0.40;
			break;

			case 3:
			SHHL=SHYP*0.30;
			break;

			default:;
		}

		$("#SCJT").shownum(SCJT);
		$("#SCBZ").shownum(SCBZ);
		$("#SCJY").shownum(SCJY);
		$("#YLBZ").shownum(YLBZ);
		$("#SHHL").shownum(SHHL);
	}

	$(".mainbtn").bind("click",function(){
		cnm();
	})


})