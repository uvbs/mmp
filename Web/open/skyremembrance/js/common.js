/* *
* 调用此方法发送HTTP请求。
*
* @public
* @param   {string}    url           请求的URL地址
* @param   {mix}       data          发送参数
* @param   {Function}  callback      回调函数
* @param   {string}    type          请求的方式，有"GET"和"POST"两种
* @param   {boolean}   asyn          是否异步请求的方式,true：异步，false：同步,没有回调函数必须同步否则将发生错误
* @param   {string}    dataType      响应类型，有"JSON"、"XML"和"TEXT"三种
*/
function jq_ajax(url,data,callback,type,async,dataType)
{
	type = typeof(type) == 'undefined' ? 'POST' : type
	async = typeof(async) == 'undefined' ? false : async;
	dataType = typeof(dataType) == 'undefined' ? 'json' : dataType;
	
	if (async == false)
	{	
		$("a").blur();$(".buttons").blur();
	}
	var jq_ajax_result = new Object;	
	if (typeof(data) == 'object')
	{
		var date_str = '';
		for(var key in data ) date_str += key+'='+encodeURIComponent(data[key])+'&';		
		data = date_str;
	}
	$.ajax({
       url:  url,
       type: type,
       data: data,
       dataType: dataType,
	   async: async,
       success: function(result)
	   {   	  
		   
		   jq_ajax_result = result;		  
		   if (callback == '') return false;
	   	   if (typeof(callback) == 'function') return callback(result);
		   if (typeof(callback) != 'undefined') return eval(callback+'(result)');
		      	
       },
	   error: function()
	   {		  
	   	   
		   jq_ajax_result.status = 0;
		   jq_ajax_result.info = '请求失败请重新尝试，多次失败请联系技术部！';
		   if (callback == '') return false;
	   	   if (typeof(callback) == 'function') return callback(jq_ajax_result);
		   if (typeof(callback) != 'undefined') return eval(callback+'(jq_ajax_result)');
	   }
     });
	
	return jq_ajax_result;
}



/* *
* 区域联动
*/
var region_url = '?g=public&m=region&a=get_region';
function loadRegion(sel,selid,p_selid){	
	var nextsel = $("#"+sel).attr("nextsel");
	if (typeof(nextsel) == 'undefined') return false;
	nextsel = nextsel.split("|"); //字符分割  
	for (i=0;i<nextsel.length ;i++ )   
	{   
		$("#"+nextsel[i]+" option").each(function(){if ($(this).val() != 0) $(this).remove();});	
	}
	p_selid = p_selid > 0 ? p_selid : $("#"+sel).val();
	if(p_selid==0) return;
	if(p_selid<0) return $("<option value='-1'>其它</option>").appendTo($("#"+nextsel[0]));	
	$.getJSON(region_url,{parent_id:p_selid},
		function(data){
		    var selected = '';
			if(data){
				$.each(data,function(idx,item){
					selected = (selid == item.region_id) ? 'selected' : '';
					$("<option value="+item.region_id+" "+selected+">"+item.region_name+"</option>").appendTo($("#"+nextsel[0]));
				});				
			}			
			selected = (selid == -1) ? 'selected' : '';
			$("<option value='-1' "+selected+">其它</option>").appendTo($("#"+nextsel[0]));
		}
	);	
}
//星级评分
function pRate(obj){	
	var B = $(obj);
	for(var i=0;i<B.attr('num');i++){
		$("<i></i>").appendTo(B);
	}	
	var rate = B.children("i"),
	w = rate.width(),
	n = rate.length,
	selid = B.attr('selectd')-1;	
	for(var i=0;i<n;i++){
		rate.eq(i).css({
			'width':w*(i+1),
			'z-index':n-i
		});
	}
	B.css({width:w*n});
	rate.eq((selid >= 0 ? selid : 0)).addClass("select");	
	if (B.attr('nohover')) return true;
	rate.hover(function(){
		var S = B.children("i.select");
		$(this).addClass("hover").siblings().removeClass("hover");
		if($(this).index()>S.index()){
			S.addClass("hover");
		}
	},function(){
		rate.removeClass("hover");
	})
	rate.click(function(){
		rate.removeClass("select hover");
		$(this).addClass("select");
		B.find('input').val($(this).index());		
	})
}


$(function(){
	$(".p_rate").each(function(){pRate(this);});
	if (!$(".region_sel").attr('id')) return false;		
	$(".region_sel").each(function(){
		var obj = this;
		var sel_val = $(this).attr("sel_val");
		var selected = "";
		if (typeof(sel_val) != 'undefined')
		{
			sel_val = sel_val.split("|"); //字符分割
			$.getJSON(region_url,{parent_id:sel_val[0]},
				function(data){
					if(data){
						$.each(data,function(idx,item){
							selected = (sel_val[1] == item.region_id) ? "selected" : "";
							$("<option value="+item.region_id+" "+selected+">"+item.region_name+"</option>").appendTo(obj);
						});				
					}
					if (sel_val[0] > 1)
					{
						selected = (sel_val[1] == -1) ? "selected" : "";
						$("<option value='-1' "+selected+">其它</option>").appendTo(obj);
					}
					
				}
			);
		}
	});
	$(".region_sel").change(function(){loadRegion(this.id);});
}); 


//字数监控
var textLength = function(){}
textLength.prototype = {	
	init:function(text_obj,mun,title_class,bnt_obj){
		var Interval;		
		var smun = 60;			
		$("#"+text_obj).keypress(function(){
			textLength.checkInputLength(this,mun,smun,title_class,bnt_obj);
		}).blur(function(){
			clearInterval(Interval);
			textLength.checkInputLength(this,mun,smun,title_class,bnt_obj);
		}).focus(function(){
			//字数监控
			clearInterval(Interval);
			Interval = setInterval(function(){
				textLength.checkInputLength('#'+text_obj,mun,smun,title_class,bnt_obj);
				},300);
		});
		textLength.checkInputLength('#'+text_obj,mun,smun,title_class,bnt_obj);
	},
	//检查字数输入
	checkInputLength:function(obj,num,snum,title_class,bnt_obj){
		//var len = getLength( $(obj).val() );
		var len = $(obj).val().length;
		var wordNumObj = $('.'+title_class);
			
		if(len==0){
			wordNumObj.css('color','').html('你还可以输入<strong style="color:#FF0000;">'+ (num-len) + '</strong>字');
			//textLength.textareaStatus('off');
		}else if( len < 8 ){
			wordNumObj.css('color','red').html('超过8个字才允许发送,你还可以输入<strong style="color:#FF0000;">'+ (num-len) +'</strong>字');
			textLength.textareaStatus('off',bnt_obj);
		}else if( len > num ){
			wordNumObj.css('color','red').html('已超出<strong style="color:#FF0000;">'+ (len-num) +'</strong>字');
			textLength.textareaStatus('off',bnt_obj);
		}else if( len > snum ){
			wordNumObj.css('color','').html('已超出长度，将划分为两条短信发送，你还可以输入<strong style="color:#FF0000;">'+ (num-len) +'</strong>字');
			textLength.textareaStatus('on',bnt_obj);			
		}else if( len <= num ){
			wordNumObj.css('color','').html('你还可以输入<strong style="color:#FF0000;">'+ (num-len) + '</strong>字');
			textLength.textareaStatus('on',bnt_obj);			
		}
	},
	//发布按钮状态
	textareaStatus:function(type,bnt_obj){
		var obj = $('#'+bnt_obj);
		var obj_class = $('.'+bnt_obj);		
		if(type=='on'){
			obj.removeAttr('disabled');	
			obj_class.removeAttr('disabled');		
		}else{
			obj.attr('disabled','true');
			obj_class.attr('disabled','true');
		}
	}
	
}

//插入文本
function insertAtCursor(input,myValue) { 
	var myField = document.getElementById(input);
	//IE support  
	if (document.selection) {
	   myField.focus();
	   sel = document.selection.createRange(); 
	   sel.text = myValue;
	   sel.select();
	}
	//MOZILLA/NETSCAPE support  
	else if (myField.selectionStart || myField.selectionStart == '0') {  
		var startPos = myField.selectionStart; 
		var endPos = myField.selectionEnd;
		// save scrollTop before insert
		var restoreTop = myField.scrollTop;  
		myField.value = myField.value.substring(0, startPos) + myValue + myField.value.substring(endPos, myField.value.length);  
		if (restoreTop > 0) {
		   myField.scrollTop = restoreTop;
		}  
		myField.focus();
		myField.selectionStart = startPos + myValue.length; 
		myField.selectionEnd = startPos + myValue.length;
	}else{  
		myField.value += myValue;  
		myField.focus();
	}
}

//操作指定元素内的复选框，全选、不选
function checkbox_all_click(obj,boxid)
{
	var s = false
	if ($(obj).attr("checked") == 'checked') s = true;	
	$("#"+boxid+$(obj).val()).find("input").each(function(){	
		$(this).attr("checked",s);
	});
}