/********************************************
 消息提示:MessageBox.show(msg,t,_timeout,_fn)
 msg:消息内容，t:消息类型（0-正常提示，1-正确，2-错误），_timeout：自动隐藏时间，单位为毫秒；_fn:消息隐藏后执行的方法
 创建人：belyn
 创建时间：2010/11/14

********************************************/

var MessageBox = new function(){
	var msgbox = new Object();
	msgbox.count = 0;
	msgbox.index=0;
	//msgbox.isloaded = false;//是否已经加载过了
	msgbox.id_pre = "div_tip_";
	msgbox.hide = function (id,d){ 
		var dom_obj = document.getElementById(id);
		var d = d || false;
		if(dom_obj){
			if(d == true){
				dom_obj.style.display = "none";
			}else{
				dom_obj.parentNode.removeChild(dom_obj);
			} 
		}
	}
	msgbox.$ = function(id){ return document.getElementById(id); }
	msgbox.getid = function(i){
	    if(i<10){
	        return msgbox.id_pre + "00" + i;
	    }else if(i< 100){
	        return msgbox.id_pre + "0" + i;
	    }else{
	        return msgbox.id_pre + i;
	    }
	}
	msgbox.newid = function (){
	    var id_temp = "";
	    for (var i=0; i<msgbox.count; i++){
	        id_temp = msgbox.getid(i); 
	        if(msgbox.$(id_temp)){
	            continue;
	        } 
	        msgbox.index = i;
	        return id_temp;
	    } 
	    id_temp = msgbox.getid(msgbox.count);
	    msgbox.index = msgbox.count;
	    msgbox.count = msgbox.count + 1;
	    
	    return id_temp;
	}
	
	//t:默认为0-提示,1-成功消息,2-错误消息
	this.show = function (msg,t,_timeout,_fn){ 
		var type = t || 0;
	 	if(type > 2) type = 0;
	 	
	 	var timeout = _timeout || 5000;
	 	var nullFn = function (){};
	 	var fn = _fn || nullFn;
	 	
		var type_classes = new Array();
		type_classes[0] = "gtl_ico_hits";
		type_classes[1] = "gtl_ico_succ";
		type_classes[2] = "gtl_ico_fail";
		
		var newlayer_id =  msgbox.newid(); 
		
	 	var newlayer = document.createElement("div");
	 	newlayer.id = newlayer_id;
	 	newlayer.className = "gb_tip_layer";  
	 	newlayer.style.marginTop = 50 * parseInt(msgbox.index ) + "px";
	 
	 	
	 	var tip_b = document.createElement("span");
	 	tip_b.className = type_classes[type];
	 	newlayer.appendChild(tip_b);
	 	
	 	var tip_c = document.createElement("span"); 
	 	tip_c.innerHTML = msg;
	 	newlayer.appendChild(tip_c);
	 	
	 	var tip_e = document.createElement("span");
	 	tip_e.className = "gtl_end";
	 	newlayer.appendChild(tip_e);
	 	 
	 	document.body.appendChild(newlayer);
	 	
	 	if(timeout > 0){
	 	    setTimeout(function(){ newlayer.parentNode.removeChild(newlayer);fn(); }, timeout);
	 	}
	 	
        return newlayer;
	}
	
	this.remove = function(obj){
	    obj.parentNode.removeChild(obj);
	}
	
	
}