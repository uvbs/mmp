define(function(){

	var CheckBrowser=function(){
		this.init()
	}

	CheckBrowser.prototype.init = function() {
		this.isPC=this.IsPC();
		this.isWexin=RegExp("MicroMessenger").test(navigator.userAgent);
		this.isiPhone=RegExp("iPhone").test(navigator.userAgent);
		this.isMI=RegExp("MI").test(navigator.userAgent);
		this.isNexus5=RegExp("Nexus 5").test(navigator.userAgent);
	};

	CheckBrowser.prototype.IsPC=function() {
	    var system = {
	        win : false,
	        mac : false,
	        xll : false
	    };
	    //检测平台
	    var p = navigator.platform;
	    system.win = p.indexOf("Win") == 0;
	    system.mac = p.indexOf("Mac") == 0;
	    system.x11 = (p == "X11") || (p.indexOf("Linux") == 0);
	    //跳转语句
	    if (system.win || system.mac || system.xll) { //转向电脑端
	    	//是电脑
	        return true; 
	    } else {
	    	//是手机
	        return false; 
	    }
	}
	return CheckBrowser;
})
