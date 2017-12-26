<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoryScore.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.score.HistoryScore" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
	<title>历史积分</title>
	<!-- Bootstrap -->
	<link rel="stylesheet" href="../css/wubu.css?v=0.0.4">
</head>
<body class="whitebg">
<div class="scorebox">
	<span class="currentscore">当前积分：<%=uinfo.TotalScore %></span>
<!-- 	<div class="numbox">
		<span class="num" id="scoreshownum">400 <span class="historyscore">(2014)</span></span>积分
		<span id="scorehidenum">20</span>
	</div> -->
	<a href="#" class="wbtn wbtn_flytr wbtn_fly wbtn_line_yellow">
        <%for (int i =1; i <=UserLevel; i++){%>
         <span class="iconfont icon-zuanshi"></span>
         <% } %>

	</a>
	<span class="scorebar">
		<span class="currentscorenum" style="width:<%=Percent%>%;"><%=uinfo.HistoryTotalScore %></span>
		<span class="currentscorebar" style="width:<%=Percent%>%;"></span>
		<span class="nextlevel">
			<span class="scorenum"><%=NextUserLevel.FromHistoryScore%></span>
			<span class="level">
         <%for (int i =1; i <=UserLevel+1; i++){%>
         <%--<span class="iconfont icon-zuanshi"></span>--%>
         <% } %>
			</span>
		</span>
	</span>
</div>
<div class="scorelist bottom50">
	<div class="article wcontainer">
		<style>
	a[name]{color:#fff;}
	h1,h2{text-align: center;}
	.bigbox{background-color:#cd3fd3;color:#fff;padding:10px;font-size: 14px;}
</style>
<div  id="divrule">

</div>
	</div>
</div>

<div class="footerbar">
	<div class="col-xs-2 ">
		<a class="wbtn wbtn_main" type="button" href="javascript:history.go(-1)">
			<span class="iconfont icon-back"></span>
		</a>
	</div><!-- /.col-lg-2 -->
	<div class="col-xs-8">
		<a href="/App/Cation/Wap/Mall/ScoreMall.aspx" class="wbtn wbtn_line_main" type="button"  id="earnscorebtn2">
			<span class="iconfont icon-76 smallicon"></span>
			积分商城
		</a>
	</div><!-- /.col-lg-10 -->
	<div class="col-xs-2 ">
		<a class="wbtn wbtn_main" type="button" href="/Wubuhui/MyCenter/Index.aspx">
			<span class="iconfont icon-b11"></span>
		</a>
	</div><!-- /.col-lg-2 -->
</div><!-- footerbar -->
</body>
<script src="../js/jquery.js"></script>
<script src="../js/bootstrap.js"></script>
<script>
    // requirejs.config({
    //     shim: {
    //         'bottomload': ['jquery'],
    //     }
    // });
    // requirejs(["jquery","bottomload"], function($) {

    // 	$(".scorelist").bottomLoad(180,function(){
    // 		console.log(0)
    // 	})


    // });

    $(function () {

        $("#divrule").load("http://step5.comeoncloud.com.cn/392ea/details.chtml");

    })
</script>
</html>
