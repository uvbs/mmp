var zymmp = {
    gaodeCompentKey:"224bd7222ce22c01673ff105ffb93fda",
    gaodeMapKey:"a4991a4564e9e02be98fb781f4ddcd65"
};
/**
 * Created by add on 2016/4/7.
 */
Date.prototype.format = function(format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1,
                RegExp.$1.length == 1 ? o[k] :
                    ("00" + o[k]).substr(("" + o[k]).length));
    return format;
};

var ua = navigator.userAgent.toLowerCase();
var isPlus = ua.indexOf('html5plus') >= 0;
var layerWaiting = false;
if(isPlus){
	if(typeof(layer)=='undefined') layer = {};
	layer.open = function(option){
		var dialog = false;
		if(option.type == 2){
			if(option.content){
				dialog =plus.nativeUI.showWaiting(option.content);
			}else{
				dialog =plus.nativeUI.showWaiting();
			}
		}else if(option.btn){
			dialog = plus.nativeUI.confirm(option.content,function(e){
				if(option.yes) option.yes();
			}, option.btn);
		}else{
			if(!option.time) option.time = 3
			dialog =plus.nativeUI.showWaiting(option.content,{loading:{display:"none"}});
			setTimeout(function(){
				if(option.end) option.end();
				if(dialog) dialog.close();
			},option.time * 1000);
		}
		return dialog;
	}
	layer.close = function(dialog){
		if(dialog) dialog.close();
	}
	layer.closeAll = function(){
		plus.nativeUI.closeWaiting();
	}
}
window.alert = function (msg, theme, time, endfn) {
    if (!time) {
        time = 3;
    }
    layer.open({
        content: msg,
        shadeClose: false,
        time: time,
        end: function () {
            if (typeof (endfn) == 'function')
                endfn();
        }
    });
};

window.zcAlert = function (msg, skin, time, endfn) {
    if (!time) {
        time = 3;
    }
    layer.open({
        content: msg,
        skin: skin, //'','msg','footer'
        shadeClose: false,
        time: time,
        end: function () {
            if (typeof (endfn) == 'function')
                endfn();
        }
    });
};


window.zcConfirm = function (msg, yesText, noText, yesfn) {
    var btn = yesText;
    if (noText) btn = [yesText, noText];
    //询问框
    layer.open({
        content: msg,
        btn: btn,
        shadeClose: false,
        yes: function (index) {
            if (typeof (yesfn) == 'function') yesfn();
            layer.close(index);
      }
    });
};

/**
 * [serializeData 序列化对象数据]
 * @param  {[type]} data [description]
 * @return {[type]}      [description]
 */
function serializeData(data) {
    // If this is not an object, defer to native stringification.
    if (!angular.isObject(data)) {
        return ((data == null) ? "" : data.toString());
    }

    var buffer = [];

    // Serialize each key in the object.
    for (var name in data) {
        if (!data.hasOwnProperty(name)) {
            continue;
        }

        var value = data[name];
        if(name=='action')
        {
            buffer.push(
                encodeURIComponent((value == null) ? "" : value)+"?"
            );
        }else
        {
            buffer.push(
                encodeURIComponent(name) + "=" + encodeURIComponent((value == null) ? "" : value)
            );
        }
    }
    // Serialize the buffer and clean it up for transportation.
    var source = buffer.join("&").replace(/%20/g, "+");
    var source=source.replace(/\?&/,'?');
    return (source);
}

var base64EncodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
var base64DecodeChars = new Array(-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63, 52, 53, 54, 55, 56, 57,
    58, 59, 60, 61, -1, -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6,
    7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
    25, -1, -1, -1, -1, -1, -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36,
    37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1);

function base64encode(str) {
    var out, i, len;
    var c1, c2, c3;
    len = str.length;
    i = 0;
    out = "";

    while (i < len) {
        c1 = str.charCodeAt(i++) & 0xff;

        if (i == len) {
            out += base64EncodeChars.charAt(c1 >> 2);
            out += base64EncodeChars.charAt((c1 & 0x3) << 4);
            out += "==";
            break
        }

        c2 = str.charCodeAt(i++);

        if (i == len) {
            out += base64EncodeChars.charAt(c1 >> 2);
            out += base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
            out += base64EncodeChars.charAt((c2 & 0xF) << 2);
            out += "=";
            break
        }

        c3 = str.charCodeAt(i++);
        out += base64EncodeChars.charAt(c1 >> 2);
        out += base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
        out += base64EncodeChars.charAt(((c2 & 0xF) << 2) | ((c3 & 0xC0) >> 6));
        out += base64EncodeChars.charAt(c3 & 0x3F)
    }

    return out
}

function base64decode(str) {
    var c1, c2, c3, c4;
    var i, len, out;
    len = str.length;
    i = 0;
    out = "";

    while (i < len) {
        do {
            c1 = base64DecodeChars[str.charCodeAt(i++) & 0xff]
        } while (i < len && c1 == -1);

        if (c1 == -1)
            break;

        do {
            c2 = base64DecodeChars[str.charCodeAt(i++) & 0xff]
        } while (i < len && c2 == -1);

        if (c2 == -1)
            break;

        out += String.fromCharCode((c1 << 2) | ((c2 & 0x30) >> 4));

        do {
            c3 = str.charCodeAt(i++) & 0xff;

            if (c3 == 61)
                return out;

            c3 = base64DecodeChars[c3]
        } while (i < len && c3 == -1);

        if (c3 == -1)
            break;

        out += String.fromCharCode(((c2 & 0XF) << 4) | ((c3 & 0x3C) >> 2));

        do {
            c4 = str.charCodeAt(i++) & 0xff;

            if (c4 == 61)
                return out;

            c4 = base64DecodeChars[c4]
        } while (i < len && c4 == -1);

        if (c4 == -1)
            break;

        out += String.fromCharCode(((c3 & 0x03) << 6) | c4)
    }

    return out;
}

function CreateGUID() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}

/**
 * @turl 链接
 * 获取链接的参数，json对象返回
 */
function GetParms(turl){
    var tParms = {};
    turl = turl.substr(turl.indexOf("?")+1);
    if(turl!="") {
        var plit = turl.split('&');
        for (var i = 0; i < plit.length; i++) {
            var nli = plit[i];
            var pnlit = nli.split('=');
            if(pnlit.length==2){
                tParms[pnlit[0]] = pnlit[1];
            }
        }
    }
    return tParms;
}
/**
 * @wurl  window链接
 * @turl 所配链接
 * 比较2个链接，是否有何所配链接的参数和值是否都在window链接上
 */
function ExistsParm(wurl,turl) {
    //获取当前URL
    var wParms = GetParms(wurl);
    var tParms = GetParms(turl);

    var tParmList = Object.getOwnPropertyNames(tParms); //所配参数为空则返回true;
    if(tParmList.length ==0) return true;

    var wParmList = Object.getOwnPropertyNames(wParms); //当前链接参数少于所配返回 false;
    if(wParmList.length < tParmList.length) return false;

    for (var i = 0; i < tParmList.length; i++) {
        var tParm = tParmList[i];
        if(!wParms[tParm] || wParms[tParm]!=tParms[tParm]) return false; //当前链接不存在某参数，或参数值不等于所配值 返回false;
    }
    return true;
}

//获取Get参数
function GetParm(parm) {
    //获取当前URL
    var local_url = window.location.href;

    //获取要取得的get参数位置
    var get = local_url.indexOf(parm + "=");
    if (get == -1) {
        return "";
    }
    //截取字符串
    var get_par = local_url.slice(parm.length + get + 1);
    //判断截取后的字符串是否还有其他get参数
    var nextPar = get_par.indexOf("&");
    if (nextPar != -1) {
        get_par = get_par.slice(0, nextPar);
    }
    return get_par;
}
//获取参数

function DelUrlParam(url, name) {
    var str = "";
    if (url.indexOf('?') != -1) {
        str = url.substr(url.indexOf('?') + 1);
    }
    else {
        return url;
    }
    var arr = "";
    var returnurl = "";
    var setparam = "";
    if (str.indexOf('&') != -1) {
        arr = str.split('&');
        for (i in arr) {
            if (isNaN(i)) {
                continue;
            }
            if (arr[i].split('=')[0] != name) {
                returnurl = returnurl + arr[i].split('=')[0] + "=" + arr[i].split('=')[1] + "&";
            }
        }
        return url.substr(0, url.indexOf('?')) + "?" + returnurl.substr(0, returnurl.length - 1);
    }
    else {
        arr = str.split('=');
        if (arr[0] == name) {
            return url.substr(0, url.indexOf('?'));
        }
        else {
            return url;
        }
    }
}

function IsURL(str_url) {
    var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
        + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //ftp的user@
        + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184
        + "|" // 允许IP和DOMAIN（域名）
        + "([0-9a-z_!~*'()-]+\.)*" // 域名- www.
        + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名
        + "[a-z]{2,6})" // first level domain- .com or .museum
        + "(:[0-9]{1,4})?" // 端口- :80
        + "((/?)|" // a slash isn't required if there is no file name
        + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
    var re = new RegExp(strRegex);
    //re.test()
    if (re.test(str_url)) {
        return (true);
    } else {
        return (false);
    }
}

////跳转  item为要跳转的链接例如：‘index.html’；id为后面要带的参数
//function goUrlChange(item,id){
//    if(id!=''){
//        window.location.href=item+'?orderId='+id;
//    }else{
//        window.location.href=item;
//    }
//}
//ajax请求，data为一个对象dataobj{type:请求类型；url:请求链接;data:请求参数}
function ajaxReq(dataobj, callback, failback, showlayer) {
    var basePath='';
    //if (location.hostname.match('localhost') || location.hostname.match('192.168.')) {
    //    basePath = 'http://dev1.comeoncloud.net';
    //}
    dataobj.url=basePath+dataobj.url;

    var layerIndex;
    if(showlayer === undefined || showlayer===true) layerIndex = layer.open({type:2,shadeClose:false});

    $.ajax({
        type: dataobj.type,  //提交方式
        url: dataobj.url,//路径
        data: dataobj.data,//数据，这里使用的是Json格式进行传输
        dataType: "json",
        success: function (result) {//返回数据根据结果进行相应的处理            
            if(showlayer === undefined || showlayer===true) layer.close(layerIndex);
            callback(result);
            
        },
        error: function (result) {
            if(showlayer === undefined || showlayer===true) layer.close(layerIndex);
            failback(result);
            
        }
    });
}

function PadLeft(num, long) {
    var temp = "0000000000" + num;
    return temp.substr(temp.length - long);
}

function SetPageTitle(title) {
	if (window.plus){
		var ws=plus.webview.getLaunchWebview();
		ws.evalJS("$('#title').text('"+title+"');");
	}
    document.title = title;
    var $iframe = document.createElement('iframe');
    $iframe.src = '/favicon.ico';
    $iframe.style = 'position:relative;width:0px;height:0px;';
    $iframe.onload = function () {
        setTimeout(function () {
            document.body.removeChild($iframe);
        }, 0);
    }
    document.body.appendChild($iframe);
}
window.zcToUrl = function (url,reload) {
    if (url.substr(0, 1) == '#') {
    	window.location.hash = url;
    	return;
    }
    var check = url.indexOf('javascript:') == 0;
    if(!check) check = url.indexOf('tel:')== 0;
    if(!check) check = url.indexOf('sms:')== 0;
    if (window.plus && !check) {
        //var ws = plus.webview.getTopWebview();
        if (url.indexOf('http') != 0){
            if(url.substr(0, 1) == '/'){
                url = window.location.origin + url;
            } else {
                url = window.location.href.substr(0, window.location.href.lastIndexOf('/') + 1) + url;
            }
        }
		var ws=plus.webview.getLaunchWebview();
		var wp=plus.webview.currentWebview();
		if(!reload) reload = false;
		ws.evalJS("$.addWebview('"+url+"','"+wp.id+"',"+reload+");");
    } else {
        window.location.href = url;
    }
}
if(typeof(indexDomain)=='undefined') indexDomain = window.location.origin;
indexUrl = indexDomain + '/customize/comeoncloud/Index.aspx?key=MallHome';
bindUrl = indexDomain + '/customize/shop/?v=1.0&ngroute=/bindPhone/#/bindPhone/';
userCenter = indexDomain+ '/customize/comeoncloud/Index.aspx?key=PersonalCenter';

window.zcBack = function (reload) {
    if (window.plus) {
		var ws=plus.webview.getLaunchWebview();
		var wp=plus.webview.getTopWebview();
        wp.canBack(function (e) {
            if (e.canBack) {
                wp.back();
            } else {
				if(!reload) reload = false;
				ws.evalJS("$.backWebview("+reload+");");
            }
        });
    } else {
        window.history.go(-1);
    }
}
/*
 xhrOs = {parentIndex:1,fileIndex:1}
 */
window.zcUpload = function (file, maxWidth, maxHeight, progress, complete, error, xhrOs) {
    if (!maxWidth) maxWidth = 0;
    if (!maxHeight) maxHeight = 0;
    var upUrl = '/Serv/API/Common/File.ashx';
        
    if (location.hostname.match('localhost') || location.hostname.match('192.168.')) {
        upUrl = "http://localhost:28241" + upUrl;
        // $rootScope.mainsiteurl = location.origin;
    }else{
        upUrl = indexDomain + upUrl;
    }
    if (!maxWidth && !maxHeight) {
        var fd = new FormData();//创建表单数据对象
        fd.append('file1', file);//将文件添加到表单数据中
        fd.append('action', 'Add');
        fd.append('dir', 'image');
        var xhr = new XMLHttpRequest();
        if(xhrOs){
        	var keys = Object.keys(xhrOs);
        	for (var i = 0; i < keys.length; i++) {
        		xhr[keys[i]] = xhrOs[keys[i]];
        	}
        }
        xhr.upload.addEventListener("progress", progress, false);//监听上传进度
        xhr.addEventListener("load", complete, false);
        xhr.addEventListener("error", error, false);
        xhr.open("POST", upUrl);
        xhr.send(fd);
    } else {
        var fReader = new FileReader();
        fReader.onload = function (e) {
            var base64 = this.result;
            var img = new Image();
            img.onload = function () {
                var hRatio;
                var wRatio;
                var Ratio = 1;
                var w = img.width;
                var h = img.height;

                if (!maxWidth) maxWidth = 0;
                if (!maxHeight) maxHeight = 0;

                wRatio = maxWidth / w;
                hRatio = maxHeight / h;

                var fileType = file.type;
                var fileName = file.name;
                if (!fileType) {
                    fileType = 'image/png';
                    fileName = Math.random() + '.png';
                }

                if (maxWidth == 0 && maxHeight == 0) {
                    Ratio = 1;
                } else if (maxWidth == 0) {//
                    if (hRatio < 1) Ratio = hRatio;
                } else if (maxHeight == 0) {
                    if (wRatio < 1) Ratio = wRatio;
                } else if (wRatio < 1 || hRatio < 1) {
                    Ratio = (wRatio <= hRatio ? wRatio : hRatio);
                }
                if (Ratio < 1) {
                    w = w * Ratio;
                    h = h * Ratio;

                    // 获取 canvas DOM 对象
                    var canvas = $('<canvas></canvas>').get(0);
                    // 获取 canvas的 2d 环境对象,
                    // 可以理解Context是管理员，canvas是房子
                    var ctx = canvas.getContext("2d");
                    // canvas清屏
                    ctx.clearRect(0, 0, canvas.width, canvas.height);
                    // 重置canvas宽高
                    canvas.width = w;
                    canvas.height = h;
                    // 将图像绘制到canvas上
                    ctx.drawImage(img, 0, 0, w, h);
                    base64 = canvas.toDataURL(fileType);
                }

                var bytes = window.atob(base64.split(',')[1]);        //去掉url的头，并转换为byte
                //处理异常,将ascii码小于0的转换为大于0
                var ab = new ArrayBuffer(bytes.length);
                var ia = new Uint8Array(ab);
                for (var i = 0; i < bytes.length; i++) {
                    ia[i] = bytes.charCodeAt(i);
                }
                var blob = new Blob([ab], { type: fileType });

                var fd = new FormData();//创建表单数据对象
                fd.append("file1", blob, fileName);
                fd.append('action', 'Add');
                fd.append('dir', 'image');
                fd.append('maxWidth', maxWidth);
                fd.append('maxHeight', maxHeight);
                var xhr = new XMLHttpRequest();
		        if(xhrOs){
		        	var keys = Object.keys(xhrOs);
		        	for (var i = 0; i < keys.length; i++) {
		        		xhr[keys[i]] = xhrOs[keys[i]];
		        	}
		        }
                xhr.upload.addEventListener("progress", progress, false);//监听上传进度
                xhr.addEventListener("load", complete, false);
                xhr.addEventListener("error", error, false);
                xhr.open("POST", upUrl);
                xhr.send(fd);
            }
            img.src = base64;
        };
        fReader.readAsDataURL(file);
    }
}
window.zcUploadImageSelect = function(complete){
	plus.nativeUI.actionSheet({buttons:[{title:"拍照"},{title:"相册"}]}, function(e){
		if(e.index ==1){
			zcCamera(function(path){
				zcPlusUploadImage('image',path,800,0,function(u){
					if(complete)complete(u);
				},function(u){
            		plus.nativeUI.alert("上传失败！");
				});
			});
		}else if(e.index ==2){
			zcPick({filter:'image',multiple:false},function(path){
				zcPlusUploadImage('image',path,800,0,function(u){
					if(complete)complete(u);
				},function(u){
            		plus.nativeUI.alert("上传失败！");
				});
			},function(path){
        		//plus.nativeUI.alert("取消选择！");
				//console.log(data);
			});
		}
	});
}
window.zcPlusUploadImage = function(type,path,maxWidth,maxHeight,complete,error){
	var lis = path.split('/');
	var tname = lis[lis.length-1];
	var ext = tname.substr(tname.indexOf('.'));
    var upUrl = '/Serv/API/Common/File.ashx';
    //upUrl = "http://192.168.50.234" + upUrl;
    
    if (location.hostname.match('localhost') || location.hostname.match('192.168.')) {
        upUrl = "http://localhost:28241" + upUrl;
        // $rootScope.mainsiteurl = location.origin;
    }else{
        upUrl = indexDomain + upUrl;
    }
    plus.nativeUI.showWaiting();
    var img = new Image();
	img.onload = function () {
        var hRatio;
        var wRatio;
        var Ratio = 1;
        var w = img.width;
        var h = img.height;

        if (!maxWidth) maxWidth = 0;
        if (!maxHeight) maxHeight = 0;

        wRatio = maxWidth / w;
        hRatio = maxHeight / h;

        var fileType = 'image/png';
        var fileName = tname;
        if (ext.toLowerCase() == '.jpg' || ext.toLowerCase() == '.jpeg') {
            fileType = 'image/jpeg';
        }
		if(!fileName) fileName = Math.random() + '.png';
        if (maxWidth == 0 && maxHeight == 0) {
            Ratio = 1;
        } else if (maxWidth == 0) {//
            if (hRatio < 1) Ratio = hRatio;
        } else if (maxHeight == 0) {
            if (wRatio < 1) Ratio = wRatio;
        } else if (wRatio < 1 || hRatio < 1) {
            Ratio = (wRatio <= hRatio ? wRatio : hRatio);
        }
        if (Ratio < 1) {
            w = w * Ratio;
            h = h * Ratio;

            // 获取 canvas DOM 对象
            var canvas = $('<canvas></canvas>').get(0);
            // 获取 canvas的 2d 环境对象,
            // 可以理解Context是管理员，canvas是房子
            var ctx = canvas.getContext("2d");
            // canvas清屏
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            // 重置canvas宽高
            canvas.width = w;
            canvas.height = h;
            // 将图像绘制到canvas上
            ctx.drawImage(img, 0, 0, w, h);
            base64 = canvas.toDataURL(fileType);
        }

        var bytes = window.atob(base64.split(',')[1]);        //去掉url的头，并转换为byte
        //处理异常,将ascii码小于0的转换为大于0
        var ab = new ArrayBuffer(bytes.length);
        var ia = new Uint8Array(ab);
        for (var i = 0; i < bytes.length; i++) {
            ia[i] = bytes.charCodeAt(i);
        }
        var blob = new Blob([ab], { type: fileType });

        var fd = new FormData();//创建表单数据对象
        fd.append("file1", blob, fileName);
        fd.append('action', 'Add');
        fd.append('dir', 'image');
        fd.append('maxWidth', maxWidth);
        fd.append('maxHeight', maxHeight);
        var xhr = new plus.net.XMLHttpRequest();
        xhr.onprogress=function(e){
            if (e.lengthComputable) {
                var percentComplete = Math.round(e.loaded * 100 / e.total);
	    		plus.nativeUI.closeWaiting();
	    		plus.nativeUI.showWaiting(percentComplete.toString()+'%');
            }
		}
        xhr.onload=function(e){
			plus.nativeUI.closeWaiting();
			var data = JSON.parse(e.target.responseText);
        	if(complete) complete(data);
		}
        xhr.onerror=function(e){
			plus.nativeUI.closeWaiting();
        	if(error) error(e);
		}
        xhr.open("POST", upUrl);
        xhr.send(fd);
    }
    img.src = path;
}
//照相机文件
window.zcCamera = function(complete){
	plus.camera.getCamera().captureImage(complete);	
}
//选择相册文件
window.zcPick = function(option,complete,error){
	//option.filter 格式 图片文件（“image”）、视频文件（“video”）或所有文件（“none”）
	//option.multiple 是否支持多选上传
	//option.maximum option.multiple为true有效，取值范围为1到Infinity，默认值为Infinity，即不限制选择的图片数
	if(option.maximum && option.maximum!='Infinity'){
		option.onmaxed = function(){
			plus.nativeUI.alert('最多只能选择'+ option.maximum +'张');
		}
	}
	plus.gallery.pick(complete, error,option);
}
//下载任务
window.zcDownload = function(url,complete,error, hideWaiting,option){
    if (window.plus) {
	    if(!hideWaiting) plus.nativeUI.showWaiting();
	    if(!option) option ={};
    	var downAct = plus.downloader.createDownload(url,option);
    	downAct.addEventListener("statechanged",function(d, status){
    		if (d.state == 4) { 
    			if(status == 200){
        			if(!hideWaiting) plus.nativeUI.closeWaiting();
    				if(complete) complete(d);
    			}else{
        			if(!hideWaiting) plus.nativeUI.closeWaiting();
    				if(error) error(d);
    			}
	       } else if(d.state == 3 ){
        		if(!hideWaiting) plus.nativeUI.closeWaiting();
	    		if(!hideWaiting) plus.nativeUI.showWaiting(Math.round(d.downloadedSize/d.totalSize*100) +'%');
	        }
    	},false);
    	downAct.start();
    }
    else{
    	window.location.href = url;
    }
}
var downCacheFiles = [];
var inDownload =false;
window.zcCheckCache = function(url){
    if (window.plus) {
		//console.log(e.url);
		if(typeof(localInitFile) == 'undefined') localInitFile=[];
	 	var lis = url.split('?');
        var url1 = lis[0];
        var ulis = url1.split('/');
        var fname = ulis[ulis.length-1];
        var ext = fname.lastIndexOf('.') == -1 ? '' : fname.substr(fname.lastIndexOf('.'));
        
        if (ext == "") return
		var limitExts = ['.js','.css', '.jpg', '.jpeg', '.png', '.bmp', '.eot', '.svg', '.ttf', '.woff', '.woff2'];
        if (limitExts.indexOf(ext.toLowerCase()) == -1) return;
		var dlis = ulis.splice(0,3);
        var domain = dlis.join('/');
        url1 = url1.substr(domain.length);
        var url2 = "";
        if (lis.length > 1){
        	lis.splice(0,1);
        	url2 = lis.join('?');
        }
        var downFile ={url:url};
        if(url2.length==0){
        	downFile.nurl = "[A-Za-z]+://[-.A-Za-z0-9]*" + url1;
            downFile.fname = url1;
        }else{
        	var nurl2 = url2.replace(new RegExp('[?]','gmi'), '[?]').replace(new RegExp('[,]','gmi'), '[,]');
        	var fname2 = url2.replace(new RegExp('[/]','gmi'), '{l}').replace(new RegExp('[?]','gmi'), '{w}').replace(new RegExp('[,]','gmi'), '{d}');
        	downFile.nurl = "[A-Za-z]+://[-.A-Za-z0-9]*" + url1+ "[?]" + nurl2;
            downFile.fname = url1 + "." + fname2 + ext;
        }
		var files = $.grep(localInitFile, function (cur, i) {
            return cur['match'] == downFile.nurl;
       	});
		if(files.length ==0){
			//console.log('file:'+JSON.stringify(downFile));
			downCacheFiles.push(downFile);
			zcDownCacheFile();
		}
    }
}
var zcDownCacheFile = function(){
	if(inDownload) return;
	if(downCacheFiles.length ==0) return;
	var fpath1 = '_downloads/cache/';
	var downFile;
	try{
		var downFiles = downCacheFiles.splice(0,1);
		downFile = downFiles[0];
	}catch(e){
		return;
	}
	inDownload = true;
	console.log('findfile:'+downFile.fname);
	$.cacheFindFile(fpath1+downFile.fname,function(){
		localInitFile.push({match:downFile.nurl,redirect:fpath1+downFile.fname});
		plus.storage.setItem('cacheFile',JSON.stringify(localInitFile));
		inDownload = false;
		zcDownCacheFile();
		},function(){
			console.log('findfile:nofind');
			console.log('downfilename:'+fpath1+downFile.fname);
			console.log('downfileurl:'+downFile.url);
			zcDownload(downFile.url,function(){
				console.log('downfile:ok');
				localInitFile.push({match:downFile.nurl,redirect:fpath1+downFile.fname});
				plus.storage.setItem('cacheFile',JSON.stringify(localInitFile));
				inDownload = false;
				zcDownCacheFile();
			}, function(d) {
				console.log('downfile:error');
		    	$.zcDelCacheFile(fpath1+downFile.fname);
				inDownload = false;
				zcDownCacheFile();
			},true,{filename:fpath1+downFile.fname});
		})
}
window.zcLoginOAuth = function(id,complete) {
	plus.nativeUI.showWaiting();
	plus.oauth.getServices(function(services) {
		var service = false;
		for(var i = 0; i < services.length; i++) {
			if(services[i].id == id) service = services[i];
		}
		if(!service){
			plus.nativeUI.closeWaiting();
			plus.nativeUI.toast('该授权登录方式未开通');
			return;
		}
		service.login(function(e) {
			plus.nativeUI.closeWaiting();
			if(complete) complete(e.target);
		}, function(e) {
			plus.nativeUI.closeWaiting();
			plus.nativeUI.toast(e.message);
		});
	}, function(e) {
			plus.nativeUI.closeWaiting();
			plus.nativeUI.toast('获取授权登录列表失败');
			//alert( "获取分享服务列表失败："+e.message+" - "+e.code );
	});
}

window.plusPost = function(url,data,complete,error,progress){
	if(window.plus){
		var xhr = new plus.net.XMLHttpRequest();
		xhr.onprogress=function(e){
            if (e.lengthComputable) {
                //var percentComplete = Math.round(e.loaded * 100 / e.total);
        		if(progress) progress(e);
            }
		}
        xhr.onload=function(e){
			//plus.nativeUI.closeWaiting();
			var data = JSON.parse(e.target.responseText);
        	if(complete) complete(data);
		}
        xhr.onerror=function(e){
			//plus.nativeUI.closeWaiting();
        	if(error) error(e);
		}
		xhr.open("POST", url);
		xhr.send(JSON.stringify(data));
	}
}

window.zcWxShare = function(title,content,pictures,href,addScoreKey,addScoreId){
	plus.nativeUI.showWaiting();
	plus.share.getServices(function(services){
		var service = false;
		for(var i=0;i<services.length;i++){
			if(services[i].id == 'weixin') service = services[i];
		}
		if(!service){
			plus.nativeUI.closeWaiting();
			plus.nativeUI.toast('未配置微信分享');
			return;
		}
		if(!service.authenticated){
			service.authorize(function(rs){
				plus.nativeUI.closeWaiting();
				plus.nativeUI.actionSheet({buttons:[{title:"分享给朋友"},{title:"分享到朋友圈"}]}, function(e){
					plus.nativeUI.showWaiting();
					if(e.index ==1 || e.index ==2){
						var toScene = e.index ==1?'WXSceneSession':'WXSceneTimeline';
						rs.send({content:content,pictures:pictures,href:href,title:title,extra:{scene:toScene}},function(){
							if(addScoreKey){
								if(addScoreKey =='ShareWeixin') addScoreKey = toScene;
								addShareSocre(addScoreKey,addScoreId);
							}
							plus.nativeUI.closeWaiting();
							plus.nativeUI.toast('分享完成');
						}, function(e){
							plus.nativeUI.closeWaiting();
							//plus.nativeUI.toast('取消分享');
						});
					}
				});
			},function(){
				plus.nativeUI.closeWaiting();
				plus.nativeUI.toast('微信分享授权失败');
			});
		}else{
			plus.nativeUI.closeWaiting();
			plus.nativeUI.actionSheet({buttons:[{title:"分享给朋友"},{title:"分享到朋友圈"}]}, function(e){
				plus.nativeUI.showWaiting();
				if(e.index ==1 || e.index ==2){
					var toScene = e.index ==1?'WXSceneSession':'WXSceneTimeline';
					service.send({content:content,pictures:pictures,href:href,title:title,extra:{scene:toScene}},function(){
						plus.nativeUI.closeWaiting();
						plus.nativeUI.toast('分享完成');
						if(addScoreKey){
							if(addScoreKey =='ShareWeixin') addScoreKey = toScene;
							addShareSocre(addScoreKey,addScoreId);
						}
					}, function(e){
						plus.nativeUI.closeWaiting();
						//plus.nativeUI.alert("code："+e.code+",message："+e.message, function() {}, "系统提示", "关闭");
						//plus.nativeUI.toast('取消分享');
					});
				}
			});
		}
	},function(e){
		plus.nativeUI.closeWaiting();
		plus.nativeUI.toast('获取分享服务列表失败');
		//alert( "获取分享服务列表失败："+e.message+" - "+e.code );
	});
}
window.addShareSocre = function(addScoreKey,addScoreId){
	var url = indexDomain+'/Serv/API/Score/Add.ashx';
	var data = {type:addScoreKey,id:addScoreId};
	plusPost(url,data);
}
window.payChannelCheck = function (id,end) {
	plus.payment.getChannels(function(channels) {
		for(var i = 0; i < channels.length; i++) {
			if(channels[i].id == id) {
				if(end) end(true);
			}
		}
	}, function(e) {
		//alert( "获取支付通道列表失败："+e.message );
	});
}

window.zcPayment = function(id, statement,complete) {
	plus.nativeUI.showWaiting();
	plus.payment.getChannels(function(channels) {
		var channel = null;
		for(var i = 0; i < channels.length; i++) {
			if(channels[i].id == id) channel = channels[i];
		}
		if(!channel){
			plus.nativeUI.toast('该支付通道未开通!');
			return;
		}
		plus.payment.request(channel, statement, function(result) {
			plus.nativeUI.closeWaiting();
			if(complete) complete(result);
		}, function(e) {
			plus.nativeUI.closeWaiting();
			plus.nativeUI.toast(e.message);
		});
	}, function(e) {
		plus.nativeUI.closeWaiting();
		//alert( "获取支付通道列表失败："+e.message );
	});
}
