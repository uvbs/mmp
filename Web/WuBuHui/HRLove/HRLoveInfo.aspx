<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HRLoveInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.HRLove.HRLoveInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="zh-CN">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
	<title>爱心活动</title>
	<!-- Bootstrap -->
	<link rel="stylesheet" href="/wubuhui/css/wubu.css?v=0.0.4">
	<!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
	<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
	<!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->

</head>
<body >
<div class="wcontainer donatebox">
	<h2>我是<%=gameFriend.Name%>,我捐了</h2>
	<!-- <span class="donateicon donateicon1"></span> -->
	<!-- <span class="donateicon donateicon10"></span> -->
	<!-- <span class="donateicon donateicon15"></span> -->
	<span class="donateicon donateicon<%=gameFriend.DonateCount%>"></span>

	<img src="<%=gameFriend.PhotoUrl%>" alt="">
	<p class="constellation"><%=gameFriend.StarSign%>
    <%
        string icon = "";
        switch (gameFriend.StarSign)
      {
          case "巨蟹":
              icon = "juxiezuo";
              break;
          case "射手":
              icon = "sheshou";
              break;
          case "水瓶":
              icon = "shuipingzuo";
              break;
          case "天秤":
              icon = "tianpingzuo";
              break;
          case "金牛":
              icon = "jinniuzuo";
              break;
          case "天蝎":
              icon = "tianxiezuo";
              break;
          case "白羊":
              icon = "baiyangzuo";
              break;
          case "双鱼":
              icon = "shuangyuzuo";
              break;
          case "双子":
              icon = "shuangzizuo";
              break;
          case "摩羯":
              icon = "mojiezuo";
              break;
          case "狮子":
              icon = "shizizuo";
              break;
          case "处女":
              icon = "chunvzuo";
              break;   
         
      }
        Response.Write(string.Format(" <span class=\"iconfont icon-{0}\"></span>", icon));
         %>
   
    
    
    
     

    
    </p>
</div><!-- mainlist -->
<div class="wcontainer bottom50 lovelinkbox">
	<div class="col-xs-3">
	    <%if(gameFriendPrevious!=null){%>
        <a href="<%=gameFriendPreviousUrl%>" class="lovelink lovelinkp">
			<svg   viewBox="0 0 60 60" class="picshade" style="background-image:url(<%=gameFriendPrevious.ThumbnailUrl%>)" >
				<path d="M0.5,21.5c2.1-10.6,10.8-19.2,22-15.2c0.7,0.3,7,3.4,7,4.5c0,0,0,0,0,0c4-4.5,11.5-7.1,17.4-5.3C55.1,8,59.9,16.4,60,24.7V0H0v25.8C0,24.4,0.2,22.9,0.5,21.5z"/>
				<path d="M59.7,28.8c-1.2,6.4-5.4,11.8-10,16.1c-7.6,7-16.4,13-27,8.8c-6-2.4-11.7-6.7-16-11.4C2.4,37.7,0,32,0,26v34h60V25.3C60,26.5,59.9,27.7,59.7,28.8z"/>
			</svg>
       </a>
		 <%} %>
	</div>
	<div class="col-xs-6">
		<span class="arrowicon lefticon">
			<span class="iconfont icon-back"></span>
		</span>
		<h3>接力好友</h3>
		<p>点击靓照进入</p>
		<span class="arrowicon righticon">
			<span class="iconfont icon-iconfontback2"></span>
		</span>
	</div>
	<div class="col-xs-3">

        <%if (gameFriendNext1 != null){%>
               
        <a href="<%=gameFriendNext1Url%>" class="lovelink lovelinkp">
       
			<svg   viewBox="0 0 60 60" class="picshade" style="background-image:url(<%=gameFriendNext1.ThumbnailUrl%>)" >
				<path d="M0.5,21.5c2.1-10.6,10.8-19.2,22-15.2c0.7,0.3,7,3.4,7,4.5c0,0,0,0,0,0c4-4.5,11.5-7.1,17.4-5.3C55.1,8,59.9,16.4,60,24.7V0H0v25.8C0,24.4,0.2,22.9,0.5,21.5z"/>
				<path d="M59.7,28.8c-1.2,6.4-5.4,11.8-10,16.1c-7.6,7-16.4,13-27,8.8c-6-2.4-11.7-6.7-16-11.4C2.4,37.7,0,32,0,26v34h60V25.3C60,26.5,59.9,27.7,59.7,28.8z"/>
			</svg>
       </a>
		 <%} %>

       <%if (gameFriendNext2 != null){%>
               
        <a href="<%=gameFriendNext2Url%>" class="lovelink lovelinkp">

			<svg   viewBox="0 0 60 60" class="picshade" style="background-image:url(<%=gameFriendNext2.ThumbnailUrl%>)" >
				<path d="M0.5,21.5c2.1-10.6,10.8-19.2,22-15.2c0.7,0.3,7,3.4,7,4.5c0,0,0,0,0,0c4-4.5,11.5-7.1,17.4-5.3C55.1,8,59.9,16.4,60,24.7V0H0v25.8C0,24.4,0.2,22.9,0.5,21.5z"/>
				<path d="M59.7,28.8c-1.2,6.4-5.4,11.8-10,16.1c-7.6,7-16.4,13-27,8.8c-6-2.4-11.7-6.7-16-11.4C2.4,37.7,0,32,0,26v34h60V25.3C60,26.5,59.9,27.7,59.7,28.8z"/>
			</svg>
       </a>
		 <%} %>


	</div>
</div>
<div class="footerbar">
	<div class="col-xs-2 ">
		<a href="javascript:history.go(-1)" class="wbtn wbtn_main" type="button" >
			<span class="iconfont icon-back"></span>
		</a>
	</div><!-- /.col-lg-2 -->
	<div class="col-xs-4">
		<a href="/Wubuhui/HRLove/HRLoveJoin.aspx" class="wbtn wbtn_line_main deletemessage" type="button" href="javascript:showdelete();">
			<span class="text">我要参与</span>
		</a>
	</div><!-- /.col-lg-10 -->
	<div class="col-xs-4">
		<a href="/Wubuhui/HRLove/HRLoveIndex.aspx" class="wbtn wbtn_line_main deletemessage" type="button" href="javascript:showdelete();">
			<span class="text">回到首页</span>
		</a>
	</div><!-- /.col-lg-10 -->
	<div class="col-xs-2 ">
		<a href="/WubuHui/MyCenter/Index.aspx" class="wbtn wbtn_main" type="button">
			<span class="iconfont icon-b11"></span>
		</a>
	</div><!-- /.col-lg-2 -->
</div><!-- footerbar -->
</body>
<script src="../js/jquery.js"></script>
<script src="../js/bootstrap.js"></script>

</html>
