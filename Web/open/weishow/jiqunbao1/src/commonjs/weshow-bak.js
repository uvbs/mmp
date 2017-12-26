
define(["jquery"],function($){

function WeShow(container,animacallback,pagenum){
	this.container=$(container);
	this.startX=0;
	this.startX=0;
	this.mainh=this.container.height();
	this.msize=this.container.children().length;
	this.mchange=0;//翻页状态
	this.touchcontrol=true;//翻页功能
	this.maini=0;//current位置
	this.maininitstate=0;
	this.animacallback=animacallback;
	if(pagenum) this.pagenum=pagenum;
	this.loadimg(0);
}

WeShow.prototype.init = function() {
	var _this=this;

	//加上动画初始状态class  此过程会关闭翻页功能
	for (var i = 0; i < _this.msize; i++) {
		_this.picanimation(i);
	};

	//初始化标签
	this.classinit();
	_this.container.children().each(function(index){
		index=index+1;
		$(this).addClass("listli"+index);
	})
	//第一张图片动画
	this.picanimation();

	//开启翻页功能
	_this.touchcontrol=true;

	this.container.bind("touchstart",function(e){
		_this.startX=e.originalEvent.targetTouches[0].clientX;
		_this.startY=e.originalEvent.targetTouches[0].clientY;
		//console.log("startX:"+_this.startX+" startY:"+_this.startY)
	})
	this.container.bind("touchmove",function(e){
		if(!_this.touchcontrol) return;
		e.preventDefault();
		_this.mfun(e);
	})
	this.container.bind("touchend",function(e){
		if(!_this.touchcontrol) return;
		_this.endfun(e);
	})
	//测试专用
	if(_this.pagenum){
		this.nextpage(_this.pagenum);
		console.log(_this.pagenum);
		this.loadimg(_this.pagenum);
	}

};
WeShow.prototype.mfun = function(e) {
	var _this=this;
	var currentX=e.originalEvent.targetTouches[0].clientX;
	var currentY=e.originalEvent.targetTouches[0].clientY;
	var dy=currentY-_this.startY;
	var scalesize=(_this.mainh-Math.abs(dy))/_this.mainh;
	scalesize=scalesize<0.85?0.85:scalesize;
	//是否向上向下移动
	//console.log(_this.maini+"--"+this.msize)
	if(scalesize==0.85){
		if(dy>0&&_this.maini>0){
			this.mchange=1
		}else if(dy<0&&_this.maini<(this.msize-1)){
			this.mchange=2;
		}
	}else{
		this.mchange=0;
	}
	$(".prebox").css({"-webkit-transform":"translate3d(0px,"+dy+"px,0px)"})
	$(".nextbox").css({"-webkit-transform":"translate3d(0px,"+dy+"px,0px)"})
	$(".current").css({"-webkit-transform":"scale("+scalesize+") translate3d(0px,"+Math.floor(dy*0.3)+"px,0px)"});
	//console.log("startX:"+_this.startX+" currentX:"+currentX+" startY:"+_this.startY+" currentY:"+currentY);
}
WeShow.prototype.endfun = function(e) {
	var _this=this;
	//var endX=e.originalEvent.changedTouches[0].clientX;
	//var endY=e.originalEvent.changedTouches[0].clientY;
	switch(this.mchange){
		case 1:
			$(".prebox").css({"-webkit-transform":"translate3d(0px,"+(_this.mainh)+"px,0px)","-webkit-transition":"all 0.4s ease-out"});
			$(".current").css({"-webkit-transform":"scale("+0.85+") translate3d(0px,"+Math.floor(_this.mainh*0.3)+"px,0px)","-webkit-transition":"all 0.4s ease-out"});
        	_this.touchcontrol=false;
			_this.maini--;
		break;
		case 2:
			$(".nextbox").css({"-webkit-transform":"translate3d(0px,"+(-_this.mainh)+"px,0px)","-webkit-transition":"all 0.4s ease-out"});
			$(".current").css({"-webkit-transform":"scale("+0.85+") translate3d(0px,"+Math.floor(-_this.mainh*0.3)+"px,0px)","-webkit-transition":"all 0.4s ease-out"});
			_this.touchcontrol=false;
			_this.maini++;
		break;
		default:
			$(".prebox").css({"-webkit-transform":"translate3d(0px,"+0+"px,0px)","-webkit-transition":"all 0.2s ease-out"});
			$(".nextbox").css({"-webkit-transform":"translate3d(0px,"+0+"px,0px)","-webkit-transition":"all 0.2s ease-out"});
			$(".current").css({"-webkit-transform":"scale("+1+") translate3d(0px,"+0+"px,0px)","-webkit-transition":"all 0.2s ease-out"});
			_this.touchcontrol=false;
	}
	var currentend=function(){
    	$(".current")[0].removeEventListener("webkitTransitionEnd",currentend,false);
    	//console.log("webkitTransitionEnd");
    	//恢复翻页功能
		_this.touchcontrol=true;
		//初始化页面并开始动画
        _this.classinit();
    }
    $(".current")[0].addEventListener("webkitTransitionEnd",currentend,false);
	//console.log("endX:"+endX+" endY:"+endY);
}
//更新位置,class,翻页动画
WeShow.prototype.classinit = function() {
	var _this=this;
	var maini=this.maini;
	_this.loadimg(maini+1);
	//更新位置，翻页动画
	_this.container.children().each(function(index){
		$(this).attr("style","")
		var index=index-_this.maini;
		$(this).css({"top":(_this.mainh*index)})
	})
	//更新class
	$(".current").removeClass("current");
	$(".prebox").removeClass("prebox");
	$(".nextbox").removeClass("nextbox");
	$(".listli:eq("+maini+")").addClass("current");
	if(maini>0){
		$(".listli:eq("+(maini-1)+")").addClass("prebox");
	}
	if(maini<(this.msize-1)){
		$(".listli:eq("+(maini+1)+")").addClass("nextbox");
	}
    if(_this.mchange==0) return;
	//清除动画样式
	for (var i = 1; i < 25; i++) {
		var pica=$(".picanimate"+i);
		var overa=$(".overanimate"+i)
		if(pica[0])pica.removeClass("picanimate"+i);
		if(overa[0])overa.removeClass("overanimate"+i);
	};
    //图片动画
    _this.picanimation();
	//初始化翻页动画类型
	_this.mchange=0;

}
//图片动画
WeShow.prototype.picanimation = function(num) {
	var _this=this;
	var snum=num?num:_this.maini
	_this.animacallback(_this,snum)

}
//动画引擎
WeShow.prototype.animation=function(container,num,callback){
	var _this=this;
	var animateclass="picanimate"+num;
	var overclass="overanimate"+num;
	var startclass="startanimate"+num;
	if(!container.hasClass(startclass)){
		container.addClass(startclass);
		if(typeof callback =="function") callback();
		// console.log("aaa");
		return;
	}
	// console.log("bbb");
	//动画开始
	container.addClass(animateclass);
	//动画结束
	var picanimate1end=function(){
		container[0].removeEventListener("webkitAnimationEnd",picanimate1end,false);
		//console.log("picanimate1end");
		//停止动画，停留在最后一帧
		container.removeClass(animateclass);
		container.addClass(overclass);
		if(typeof callback =="function") callback();
	}
	container[0].addEventListener("webkitAnimationEnd",picanimate1end,false);	
}
WeShow.prototype.nextpage=function(num){
	var _this=this;
	_this.mchange=2;
	if(!num){
		_this.endfun();
	}else{
		$(".current").css({"opacity":"0","-webkit-transition":"all 0.4s ease-out"});
		_this.touchcontrol=false;
		_this.maini=num;
		var currentend=function(){
	    	$(".current")[0].removeEventListener("webkitTransitionEnd",currentend,false);
	    	//console.log("webkitTransitionEnd");
	    	//恢复翻页功能
			_this.touchcontrol=true;
			//初始化页面并开始动画
			_this.classinit();
		}
		$(".current")[0].addEventListener("webkitTransitionEnd",currentend,false);
	}
}

WeShow.prototype.loadimg=function(num){
	var img=$(".listli:eq("+num+") .img");
	if(!img.attr("style")){
		img.attr("style",img.attr("data-original"));
	}
}

return WeShow;

})