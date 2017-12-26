define(["jquery"],function($){

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

	var AudioPlay=function(container,controlbtn){
		this.cr=$(container);
		this.isplay=false;
		this.musici=[0,0];
		this.mainbtn=$(controlbtn);
	}
	AudioPlay.prototype.init = function() {
		var _this=this;
		//设置循环播放
		_this.loopplay();
		//播放音乐
		_this.playmusic();
		//开关控制
		_this.mainbtn.bind("click",function(){
			_this.isplay?_this.stopmusic():_this.playmusic()
		})
	};
	//设置循环播放
	AudioPlay.prototype.loopplay=function(){
		//安卓 循环播放 兼容
		if(RegExp("Android").test(navigator.userAgent)){
			this.cr[0].addEventListener('ended', function(){
				this.currentTime = 0;
				this.play();
			}, false);
		}else{
			this.cr[0].loop=true;
		}
	}
	//播放音乐
	AudioPlay.prototype.playmusic=function(){
		var _this=this;
		_this.cr[0].play();
		_this.isplay=true;
		_this.animation();
	}
	//暂停音乐
	AudioPlay.prototype.stopmusic=function(){
		var _this=this;
		_this.cr[0].pause();
		_this.isplay=false;
	}
	//音乐播放动画
	AudioPlay.prototype.animation=function(){
		var _this=this;
		setTimeout(ani,100);
		function ani(){
			if(!_this.isplay){
				_this.musici=[0,0];
				_this.mainbtn.css({"background-position":0+"px "+0+"px"});
				return;
			}
			var x=_this.musici[0];
			var y=_this.musici[1];
			_this.mainbtn.css({"background-position":x+"px "+y+"px"});
			_this.musici[0]-=64;
			if(_this.musici[0]==(-960)){
				_this.musici[0]=0;
				_this.musici[1]=-64;
			}else if(_this.musici[0]==-512&&_this.musici[1]==-64){
				_this.musici[1]=-0;
				_this.musici[0]=0;
			}
			setTimeout(ani,200);
		}
	}

	return AudioPlay;

})






