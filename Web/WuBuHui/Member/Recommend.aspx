<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recommend.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Member.Recommend" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <meta name="format-detection" content="telephone=no" />
    <title>呼朋唤友</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://cdn.bootcss.com/bootstrap/3.2.0/css/bootstrap.min.css">
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.8">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    
</head>
<body class="wbtn_red" style="background-color:#d43c8f;border-color:#d43c8f;">
<img src="../images/hupenghuanyou.jpg" width="100%" alt="">
<div class="wcontainer tuijianbox bottom50">
	<h3>已推荐<span class="num"><%=RecommCount%></span>位好友</h3>
	<p style="font-size:22px;text-align:center;">推荐微信好友、亲朋闺蜜加入五步会，一旦他们成功注册，即可获得50个积分，多推荐多积分哦～</p>
</div>
<div class="footerbar">
	<div class="col-xs-2 ">
		<a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx">
			<span class="iconfont icon-back"></span>
		</a>
	</div><!-- /.col-lg-2 -->
	<div class="col-xs-10">
		<span class="wbtn wbtn_line_main" id="jointhisdiscuss">
			
			
		</span>
	</div><!-- /.col-lg-10 -->
</div><!-- footerbar -->

</body>
<script src="/Scripts/jquery-2.1.1.min.js" type="text/javascript"></script>
 <script src="../js/comm.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    var uid = '<%=userInfo.UserID %>';
    var imgUrl = "http://" + window.location.host + "/WuBuHui/img/ccc.jpg";
    var shareUrl = "http://" + window.location.host + "/WuBuHui/Member/Registration.aspx?UserId=" + uid;
    wx.ready(function () {
        wxapi.wxshare({
            title: "呼朋唤友",
            desc: "五步会",
            link: shareUrl,
            imgUrl: imgUrl
        }

    )
    })
</script>
</html>
