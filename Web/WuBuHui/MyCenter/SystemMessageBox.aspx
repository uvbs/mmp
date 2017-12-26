<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemMessageBox.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WordsQuestions.MessageBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html >
<html lang="zh-cn">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>消息</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://at.alicdn.com/t/font_1413272586_8236315.css">
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.4">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <script>        var IsHaveUnReadMessage = "<%=IsHaveUnReadMessage%>"; </script>
</head>
<body>
<div class="mainlist bottom50" id="needload">
<!-- style="background-color:#000;http://www.baidu.com" -->
	<a href="SystemMessageList.aspx?type=1" class="listbox" >
		<div class="touxiang wbtn_round wbtn_main">
			<span class="iconfont icon-24"></span>
		</div>
		<div class="textbox">
			<h3>系统消息</h3>
			<p>您有<%=SystemMessageCount%>条未读的系统消息</p>
		</div>
		<%--<div class="wbtn_fly wbtn_flybr wbtn_main timetag">
			<%=SysteMessageTime%>
		</div>--%>
	</a><!-- listbox -->
	<a href="SystemMessageList.aspx?type=11" class="listbox" >
		<div class="touxiang wbtn_round wbtn_yellow">
			<span class="iconfont icon-34"></span>
		</div>
		<div class="textbox">
			<h3>话题</h3>
			<p>您有<%=ReviewCount%>条未读的话题回复消息</p>
		</div>
		<%--<div class="wbtn_fly wbtn_flybr wbtn_yellow timetag">
			<%=ReviewTime%>
		</div>--%>
	</a><!-- listbox -->
	<a href="SystemMessageList.aspx?type=21" class="listbox" >
		<div class="touxiang wbtn_round wbtn_greenyellow">
			<span class="iconfont icon-59"></span>
		</div>
		<div class="textbox">
			<h3>问卷</h3>
			<p>您有<%=QuestionaryCount%>条未读的问卷消息</p>
		</div>
		<%--<div class="wbtn_fly wbtn_flybr wbtn_greenyellow timetag">
			<%=QuestionaryTime%>
		</div>--%>
	</a><!-- listbox -->
</div><!-- mainlist -->

<!-- footerbar -->
<script type="text/javascript" src="../js/footer.js"></script>

</body>
<script src="../js/jquery.js"></script>
<script src="../js/bootstrap.min.js"></script>
</html>
