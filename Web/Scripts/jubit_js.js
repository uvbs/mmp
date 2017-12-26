function DIVAlert(str,wid){
    var msgw,msgh,bordercolor;
    msgw=300;//提示窗口的宽度
    msgh=100;//提示窗口的高度
    titleheight=25 //提示窗口标题高度
    bordercolor="#336699";//提示窗口的边框颜色
    titlecolor="#458246";//提示窗口的标题颜色
 
    var sWidth,sHeight;
    sWidth=document.body.scrollWidth;
    sHeight=document.body.scrollHeight;

    var bgObj=document.createElement("div");
    bgObj.setAttribute('id','bgDiv');
    bgObj.style.position="absolute";
    bgObj.style.top="0";
    bgObj.style.background="#777";
    bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75";
    bgObj.style.opacity="0.6";
    bgObj.style.left="0";
    bgObj.style.width=sWidth + "px";
    bgObj.style.height=sHeight + "px";
    bgObj.style.zIndex = "10000";
    document.body.appendChild(bgObj);
 
    var msgObj=document.createElement("div")
    msgObj.setAttribute("id","msgDiv");
    msgObj.setAttribute("align","center");

    msgObj.style.background="white";
    msgObj.style.border="1px solid " + bordercolor;
		msgObj.style.position="absolute";
             msgObj.style.left = "45%";
             msgObj.style.top = wid;
             msgObj.style.font="12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
             msgObj.style.marginLeft = "-125px" ;
             msgObj.style.marginTop = -50+document.body.scrollTop+"px";
             msgObj.style.width = msgw + "px";
             msgObj.style.height =msgh + "px";
             msgObj.style.textAlign = "center";
             msgObj.style.lineHeight ="25px";
             msgObj.style.zIndex = "10001";
 
      var title=document.createElement("h4");
      title.setAttribute("id","msgTitle");
      title.setAttribute("align","right");
      title.style.margin="0";
      title.style.padding="3px";
      title.style.background=bordercolor;
      title.style.filter="progid:DXImageTransform.Microsoft.Alpha(startX=20, startY=20, finishX=100, finishY=100,style=1,opacity=75,finishOpacity=100);";
      title.style.opacity="0.75";
      title.style.border="1px solid " + bordercolor;
      title.style.height="18px";
      title.style.font="12px Verdana, Geneva, Arial, Helvetica, sans-serif";
      title.style.color="white";
      title.style.cursor="pointer";
      title.innerHTML="关闭";
      title.onclick=function(){
           document.body.removeChild(bgObj);
                 document.getElementById("msgDiv").removeChild(title);
                 document.body.removeChild(msgObj);
                 }
      document.body.appendChild(msgObj);
      document.getElementById("msgDiv").appendChild(title);
      var txt=document.createElement("p");
      txt.style.margin="1em 0"
      txt.setAttribute("id","msgTxt");
      txt.innerHTML=str;
      document.getElementById("msgDiv").appendChild(txt);
}

function WXDIVAlert(str,wid){
    var msgw,msgh,bordercolor;
    msgw=200;//提示窗口的宽度
    msgh=100;//提示窗口的高度
    titleheight=25 //提示窗口标题高度
    bordercolor="#336699";//提示窗口的边框颜色
    titlecolor="#99CCFF";//提示窗口的标题颜色
 
    var sWidth,sHeight;
    sWidth=document.body.scrollWidth;
    sHeight=document.body.scrollHeight;

    var bgObj=document.createElement("div");
    bgObj.setAttribute('id','bgDiv');
    bgObj.style.position="absolute";
    bgObj.style.top="0";
    bgObj.style.background="#777";
    bgObj.style.filter="progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75";
    bgObj.style.opacity="0.6";
    bgObj.style.left="0";
    bgObj.style.width=sWidth + "px";
    bgObj.style.height=sHeight + "px";
    bgObj.style.zIndex = "10000";
    document.body.appendChild(bgObj);
 
    var msgObj=document.createElement("div")
    msgObj.setAttribute("id","msgDiv");
    msgObj.setAttribute("align","center");

    msgObj.style.background="white";
    msgObj.style.border="1px solid " + bordercolor;
		msgObj.style.position="absolute";
             msgObj.style.left = "50%";
             msgObj.style.top = wid;
             msgObj.style.font="12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
             msgObj.style.marginLeft = "-100px" ;
             msgObj.style.marginTop = -50+document.body.scrollTop+"px";
             msgObj.style.width =msgw+"px";
             msgObj.style.height =msgh+"px";
             msgObj.style.textAlign = "center";
             msgObj.style.lineHeight ="25px";
             msgObj.style.zIndex = "10001";
 
      var title=document.createElement("h4");
      title.setAttribute("id","msgTitle");
      title.setAttribute("align","right");
      title.style.margin="0";
      title.style.padding="3px";
      title.style.background ="#458246";
      title.style.filter="progid:DXImageTransform.Microsoft.Alpha(startX=20, startY=20, finishX=100, finishY=100,style=1,opacity=75,finishOpacity=100);";
      title.style.opacity="0.75";
      title.style.border = "1px solid " + "#458246";
      title.style.height="18px";
      title.style.font="12px Verdana, Geneva, Arial, Helvetica, sans-serif";
      title.style.color="white";
      title.style.cursor="pointer";
      title.innerHTML="关闭";
      title.onclick=function(){
           document.body.removeChild(bgObj);
                 document.getElementById("msgDiv").removeChild(title);
                 document.body.removeChild(msgObj);
                 }
      document.body.appendChild(msgObj);
      document.getElementById("msgDiv").appendChild(title);
      var txt=document.createElement("p");
      txt.style.margin="1em 0"
      txt.setAttribute("id","msgTxt");
      txt.innerHTML=str;
      document.getElementById("msgDiv").appendChild(txt);
}




function getArgs() { 
    var args = {};
    var query = location.search.substring(1);
    var pairs = query.split("&"); 
     for(var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('=');
             // Look for "name=value"
            if (pos == -1) continue;
                    // If not found, skip
                var argname = pairs[i].substring(0,pos);// Extract the name
                var value = pairs[i].substring(pos+1);// Extract the value
                value = decodeURIComponent(value);// Decode it, if needed
                args[argname] = value;
                        // Store as a property
        }
    return args;// Return the object 
 }
 
 
 
function Message(){
	var args=getArgs();
	if(args.message)
	{
	    //WXDIVAlert(args.message,'200px');
	    alert(args.message);
	}							
}
		 
		
