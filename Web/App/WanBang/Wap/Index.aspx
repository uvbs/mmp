<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.Index" %>

<!DOCTYPE html>
<html lang="zh-CN">
    <head>
        <meta charset="UTF-8">
        <meta http-equiv="Content-Type" content="text/html;charset=UTF-8">
        <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0;">
		<title>万邦</title>
		 <link href="../Css/wanbang.css" rel="stylesheet" type="text/css" />
	</head>
	<body>
		<!-- Top -->
        <header class="head">
        	<a id="logo" href="#"><img src="../Images/logo.png"></a>
            
        	<form class="
            3
            
            login" method="post" action="Login.aspx">
            
            <%if (ZentCloud.JubitIMP.Web.DataLoadTool.CheckWanBangLogin())
             {
                 Response.Write("<input type=\"submit\" value=\"退出登录\"/>");
             }
            else
            {
                 Response.Write("<input type=\"submit\" value=\"立即登录\"/>");
           } %>

        	</form>
          
        </header>
		<!--/ Top -->
		<!-- Banner -->
        <div class="banner">

        <div id="slider">
        <a href="#" class="slider">
            <img src="/App/WanBang/Images/huandeng1.jpg" width="320" height="130" alt="" class="pic">
            
        </a>
        <a href="#" class="slider">
            <img src="/App/WanBang/Images/huandeng2.jpg" width="320" height="130" alt="" class="pic">
        </a>
        <a href="#" class="slider">
            <img src="/App/WanBang/Images/huandeng3.jpg" width="320" height="130" alt="" class="pic">
        </a>

    </div>
    

        </div>
		<!--/ Banner -->
        <table style="width:100%;"><tr><td style="width:50%;text-align:center;"><h3>项目总数:<%=ProjectCount+JointProjectCount%>个</h3> </td><td  style="width:50%;text-align:center;"><h3>对接成功:<%=JointProjectCount  %>个</h3></td></tr></table>
		<!-- List -->
        <ul class="product">
                    <li>
            	    <a href="ProjectList.aspx" style="border-right:2px solid #cfcfcf;border-bottom:2px solid #cfcfcf">
            	    	<span class="pic"><img src="../Images/product2.png"></span>
            	    	<span class="text" style="color:#cd772c;">寻找项目</span>

            	    </a>
            	</li>
                 <li>
            	    <a href="JointProjectList.aspx" style="border-bottom:2px solid #cfcfcf">
            	    	<span class="pic"><img src="../Images/product4.png"></span>
                        <span class="text" style="color:#6196b8;">对接成果</span>

            	    </a>
            	</li>
                <li>
            	    <a href="ProjectAddEdit.aspx" style="border-right:2px solid #cfcfcf;">
            	    	<span class="pic"><img src="../Images/product1.png"></span>
            	    	<span class="text" style="color:#985b57;">发布项目</span>

            	    </a>
            	</li>
            	<li>
            	    <a href="BaseList.aspx">
            	    	
                        <span class="pic"><img src="../Images/product3.png"></span>
            	    	<span class="text" style="color:#268d80;">关注基地</span>

            	    </a>
            	</li>



        </ul>
		<!--/ List -->
        <div class="blank"></div>
		<!-- Nav -->
        <nav class="nav">
        	<a class="on" href="Index.aspx">
        		<span class="pic">
                    <span class="iconfont icon-shouye"></span>
                </span>
        		<span class="t">首页</span>
        	</a>
        	<a href="/web/list.aspx?cateid=164">
        		<span class="pic">
                    <span class="iconfont icon-huodong"></span>
                </span>
        		<span class="t">新闻</span>
        	</a>
            <%if (ZentCloud.JubitIMP.Web.DataLoadTool.CheckWanBangLogin())
       {
         
           if (HttpContext.Current.Session[ZentCloud.JubitIMP.Web.SessionKey.WanBangUserType].ToString().Equals("0"))
           {
               Response.Write("<a href=\"BaseCenter.aspx\">");
           }
           else
           {
               Response.Write("<a href=\"CompanyCenter.aspx\">");
           }
       }
       else
       {
           Response.Write("<a href=\"Login.aspx\">");
       }
         %>
        		<span class="pic">
                    <span class="iconfont icon-wode"></span>      
                </span>
        		<span class="t">我的</span>
        	</a>



        </nav> 
		<!--/ Nav -->
	</body>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.slides.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $("#slider").slidesjs({
            width: 300,
            height: 130,
            play: {
                active: true,
                auto: true,
                interval: 4000,
                swap: true
            }
        });

        var desc = "我们致力于为上海市165个残疾人就业援助机构“阳光基地”的4499位残疾人士寻找手工劳动项目。";
        var title = "上海万邦关爱服务中心";
        var imgUrl = "http://" + window.location.host + "/App/WanBang/Images/baselogo.jpg";
        var shareUrl = window.location.href;

        // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            // 发送给好友

            WeixinJSBridge.on('menu:share:appmessage', function (argv) {
                WeixinJSBridge.invoke('sendAppMessage', {
                    //"appid": appId,
                    "img_url": imgUrl,
                    "img_width": "700",
                    "img_height": "420",
                    "link": shareUrl,
                    "desc": desc,
                    "title": title
                }, function (res) {
                    //if (res.err_msg == 'send_app_msg:ok') {
                    AddLog(0);
                    //}
                })
            });

            WeixinJSBridge.on('menu:share:timeline', function (argv) {
                WeixinJSBridge.invoke('shareTimeline', {
                    //"appid": appId,
                    "img_url": imgUrl,
                    "img_width": "700",
                    "img_height": "420",
                    "link": shareUrl,
                    "desc": desc,
                    "title": title
                }, function (res) {

                })
            });

            WeixinJSBridge.on('menu:share:weibo', function (argv) {
                WeixinJSBridge.invoke('shareWeibo', {
                    "content": title,
                    "url": shareUrl
                }, function (res) {

                });
            });

        }, false)


    </script>
</html>