<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TongJi.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.TongJi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html >
<html lang="zh-cn">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>统计</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://at.alicdn.com/t/font_1413272586_8236315.css">
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.4">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <style>
    
    body{font-size:20px;font-weight:bold;text-align:center;}
    </style>
</head>
<body>
<div class="mainlist bottom50" id="needload">

<div>
昨天(<%=model.PreDate.ToString("yyyy-MM-dd")%>)
<br />
访客:<%=model.PreTotalGuestCount%>
<br />
注册:<%=model.PreTotalRegCount%>
<br />
访客增长:<%=model.PreAddGuestTount%>
<br />
注册增长:<%=model.PreAddRegTount%>
</div>


<hr />
<div>
今天(<%=model.ToDay.ToString("yyyy-MM-dd")%>)
<br />
访客:<%=model.ToDayTotalGuestCount%>
<br />
注册:<%=model.ToDayTotalRegCount%>
<br />
访客增长:<%=model.ToDayAddGuestTount%>
<br />
注册增长:<%=model.ToDayAddRegTount%>

</div>

</div>


</body>
</html>