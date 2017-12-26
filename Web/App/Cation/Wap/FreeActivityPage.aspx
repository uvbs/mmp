<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreeActivityPage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.FreeActivityPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <title><%=model.ActivityName %></title>
     <script src="http://static-files.socialcrmyun.com/static%2Fjs%2Fjq%2Fjquery-1.8.2.min.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
<div style="max-width:600px!important;margin:0 auto;">
	<br />
<section class="zcWxEditor"><section style="border:0px;text-align:center;box-sizing:border-box;padding:0px;word-break:break-all;"><section style="display:inline-block;box-sizing:border-box;color:inherit;"><section class="135brush" data-brushtype="text" style="margin:0.2em 0px 0px;padding:0px 0.5em 5px;max-width:100%;color:#6B4D40;font-size:1.8em;line-height:1;border-bottom-width:1px;border-bottom-style:solid;border-color:#6B4D40;"><%=model.ActivityName %></section><section class="135brush" data-brushtype="text" style="margin:5px 1em;font-size:1em;line-height:1;color:#6B4D40;box-sizing:border-box;border-color:#6B4D40;">该活动仅支持在微信打开</section></section></section><section style="display:block;width:0;height:0;clear:both;"></section></section>
	<div style="clear:both;">
	</div>
	<p>
		<br />
	</p>
<section class="zcWxEditor" style="padding:2px;"><section class="layout" style="max-width:100%;margin:2px auto;padding:0px;word-break:break-all;"><section style="max-width:100%;margin-left:1em;line-height:1.4em;"><span style="font-size:14px;"><strong style="color:#3E3E3E;line-height:32px;white-space:pre-wrap;"><span class="135brush" data-brushtype="text" data-mce-style="color: #a5a5a5;" placeholder="关于135编辑器" style="background-color:#569F08;border-radius:5px;color:#FFFFFF;padding:4px 10px;font-size:12px;">您可以通过以下任何一种方式在微信打开该活动</span></strong></span></section><section class="135brush" data-style="color:rgb(89, 89, 89); font-size:14px; line-height:28px" style="font-size:1em;max-width:100%;margin-top:-0.7em;padding:10px 1em;border:1px solid #C0C8D1;border-radius:0.4em;color:#333333;background-color:#EFEFEF;">
	<ul style="margin-left:20px;">
		<li>
			<span style="font-size:1em;line-height:1.5;"></span><span style="font-size:1em;line-height:1.5;">用微信扫描二维码。</span> 
		</li>
		<li>
			<span style="font-size:1em;line-height:1.5;"></span><span style="font-size:1em;line-height:1.5;">长按图片保存到本地，在微信里面扫描打开。</span> 
		</li>
		<li>
			<span style="font-size:1em;line-height:1.5;"></span><span style="font-size:1em;line-height:1.5;">直接复制二维码底部的链接，在微信中打开。</span> 
		</li>
	</ul>
	<p>
		<br />
	</p>
	<p style="text-align:center;">
		<img alt="" id="imgQrcode" width="220" height="220" alt="正在加载" /> 
	</p>
	<p style="text-align:center;">
		<br />
	</p>
	<p style="text-align:center;">
		<a id="alinkurl"  target="_blank" title="点击查看" style="outline-style:none;color:#333333;text-decoration:none;font-family:宋体;line-height:normal;text-align:center;white-space:normal;"></a> 
	</p>
	<p style="text-align:center;">
		<br />
	</p>
</section></section><section style="display:block;width:0;height:0;clear:both;"></section></section>
	<div style="clear:both;">
	</div>
	<p>
		<br />
	</p>
	<p>
		<br />
	</p>
</div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var domain = '<%=Request.Url.Authority %>';
    var aid = '<%=model.JuActivityIDHex%>';
    $(function () {
        var linkurl = "http://" + domain + "/" + aid + "/" + "details.chtml";
        $.ajax({
            type: 'post',
            url: "/Handler/QCode.ashx",
            data: { code: linkurl },
            success: function (result) {
                $("#imgQrcode").attr("src", result);
            }
        });
        $("#alinkurl").html(linkurl);
        $("#alinkurl").attr("href", linkurl);
    })

</script>