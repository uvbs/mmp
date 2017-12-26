<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyCenter.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.Wap.CompanyCenter" %>
<!DOCTYPE html>
<html lang="zh-CN">
	<head>
		<meta charset="UTF-8">
		<meta http-equiv="Content-Type" content="text/html;charset=UTF-8">
		<meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0;">
		<title>企业中心</title>
		<link href="../Css/wanbang.css" rel="stylesheet" type="text/css" />
	</head>
	<body>
        <div style="padding:0 10px"><img width="100%"  src="../images/company_c_banner.png"></div>
        <div class="company_pub">
            <a class="s1" href="CompanyInfoEdit.aspx"><span class="iconfont icon-ziliao"></span><span class="t">企业资料</span><span class="more"><span class="iconfont icon-gengduo"></span></span></a>
            <a class="s2" href="ProjectAddEdit.aspx"><span class="iconfont icon-xinjian"></span><span class="t">新建发布</span><span class="more"><span class="iconfont icon-gengduo"></span></span></a>
            <a class="s3" href="MyPubProject.aspx"><span class="iconfont icon-fabu"></span><span class="t">我的发布</span><span class="more"><span class="iconfont icon-gengduo"></span></span></a>
            <a class="s4" href="AttentionList.aspx"><span class="iconfont icon-guanzhu"></span><span class="t">我的关注</span><span class="more"><span class="iconfont icon-gengduo"></span></span></a>

        </div>
        <div style="height:162px;"></div>
<!-- Nav -->
        <nav class="nav">
        	<a  href="Index.aspx">
        		<span class="pic">
                    <span class="iconfont icon-shouye"></span>
                </span>
        		<span class="t">首页</span>
        	</a>
        	<a href="/Web/list.aspx?cateid=164">
        		<span class="pic">
                    <span class="iconfont icon-huodong"></span>
                </span>
        		<span class="t">新闻</span>
        	</a>
            <a class="on" href="CompanyCenter.aspx">
        		<span class="pic">
                    <span class="iconfont icon-wode"></span>      
                </span>
        		<span class="t">我的</span>
        	</a>



        </nav> 
		<!--/ Nav -->   
	</body>
</html>