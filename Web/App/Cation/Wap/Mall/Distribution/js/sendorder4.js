$(function(){

// check("#delivery","#deliverybox",".checktype");
// check("#saddress","#saddressbox",".moreinfo");

var delivery=new checkinfo($("#delivery"),$("#deliverybox"));
delivery.clickshow(controlbox(delivery,".checktype"));


var saddress=new checkinfo($("#saddress"),$("#saddressbox"));
saddress.clickshow(controlbox(saddress,".moreinfo"));


var paytype=new checkinfo($("#paytype"),$("#paytypebox"));
paytype.clickshow(controlbox(paytype,".checktype"));

var person=new checkinfo($("#person"),$("#personbox"));
person.clickshow(controlbox(person,{".name":".name",".phone":".phone",".address":".address"}));

var addadress=new checkinfo($("#addadress"),$("#addadressbox"));
addadress.clickshow(addmyaddress(addadress));

function addmyaddress(obj){
	var ack=obj.cb.find(".submit");
	ack.click(function(){
		$.ajax({
			url:"http://192.168.1.181/bbb/mall/test.php",
			data:{ddd:"ddd"},
			complete:function( event, xhr, settings){
				//console.log(event+"-"+xhr+"-"+settings);
			}
		}).done(function(msg){
			console.log(msg);
		})

	})
	
}


})



//显示隐藏选择框
var checkinfo=function(a,b){
	this.ca=a;
	this.cb=b;
}
checkinfo.prototype.show = function() {
	this.cb.addClass("showcheck");
};
checkinfo.prototype.hide = function() {
	this.cb.removeClass("showcheck");
};

//"点击"显示  "取消"隐藏 f是选择框内的自定义方法
checkinfo.prototype.clickshow = function(f) {
	var _this=this;
	_this.ca.click(function(){
		_this.show()
		f;
	})
	_this.cb.find(".close").click(function(){
		_this.hide()
	})
};


//内容选择与替换
function controlbox(obj,infobox){
	var ack=obj.cb.find(".radiocheck");
	ack.unbind("change");
	ack.bind("change",function(){
		if(typeof infobox=="string"){
			var t=$(obj.cb).find(".value:eq("+$(this).index()/2+")").text();
			obj.ca.find(infobox).text(t);
		}else if(typeof infobox=="object"){
			for(dd in infobox){
				var t=$(obj.cb).find(".value:eq("+$(this).index()/2+")").find(dd).text();
				obj.ca.find(infobox[dd]).text(t);
			}
		}else{
			console.log("controlbox(obj,infobox)————infobox应该是字符或者数组");
		}
		obj.hide();
	})
}