<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customize_BXGT.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Customize_BXGT1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="description" content="" />
<meta name="keywords" content="" />
<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
<link type="text/css" href="Customize/BXGT/css/base.css" rel="stylesheet" />
<link type="text/css" href="Customize/BXGT/css/page.css?v=0.2" rel="stylesheet" />
</head>
<body style="background:#f2f2f2;height:100%;">
	<p class="banner">
		<img src="Customize/BXGT/images/banner.png"/>
	</p>
	<div class="address">
		<div class="adCon">
			<h2>演出地点</h2>
			<ul class="xyUl">
				<li id="addr1" class="cur" data-address="1">
					上海兰心大戏院（茂名南路57号）
				</li>
				<li id="addr2" data-address="2">
					上海大宁剧院（平型关路1222号）
				</li>
			</ul>
			<div class="data">
				<h2>演出时间</h2>
				<ul class="rqUl clearfix">
					<li id = "t1" class="cur" data-time="2014-12-24" >
						<p class="pp1">24</p>
						<p class="pp2">2014.12</p>
					</li>
					<li id = "t2" data-time="2014-12-25">
						<p class="pp1">25</p>
						<p class="pp2">2014.12</p>
					</li>
					<li id = "t3" data-time="2014-12-26">
						<p class="pp1">26</p>
						<p class="pp2">2014.12</p>
					</li>
					<li id = "t4" data-time="2014-12-27">
						<p class="pp1">27</p>
						<p class="pp2">2014.12</p>
					</li>
					<li id = "t5" data-time="2014-12-28">
						<p class="pp1">28</p>
						<p class="pp2">2014.12</p>
					</li>
					<li id = "t6" data-time="2015-01-08" style="display:none">
						<p class="pp1">08</p>
						<p class="pp2">2015.01</p>
					</li>
					<li id = "t7" data-time="2015-01-09" style="display:none">
						<p class="pp1">09</p>
						<p class="pp2">2015.01</p>
					</li>
					<li id = "t8" data-time="2015-01-10" style="display:none">
						<p class="pp1">10</p>
						<p class="pp2">2015.01</p>
					</li>
				</ul>
			</div>
		</div>
	</div>
	<div class="price">
		<p class="jgp"><span id="spprice">￥880.00</span></p>
		<p class="kc">快递包邮 </p>
	</div>
	<div class="XingHao">
		<div class="clearfix">
			<p class="fl xh">型号：</p>
			<ul class="fr xhUl">
				<li class="cur" data-price="880">
					880元VIP
				</li>
				<li data-price="580">
					580元
				</li>
				<li data-price="480">
					480元
				</li>
				<li data-price="380">
					380元
				</li>
				<li data-price="280">
					280元
				</li>
				<li data-price="180">
					180元
				</li>
				<li data-price="120">
					120元
				</li>
                

			</ul>
		</div>
    <div style="text-align:center;"><h1>一经售票，不予退票</h1></div>
	</div>

    
	<div class="GouMai">
		<p class=""><a title="" id="btnSubmit"><img src="Customize/BXGT/images/gm.png"/></a></p>
	</div>

</body>
<script type="text/javascript" src="Customize/BXGT/js/jquery.js"></script>
<script type="text/javascript" src="Customize/BXGT/js/huaping.js"></script>
<script type="text/javascript" src="Customize/BXGT/js/home.js"></script>
<script type="text/javascript">
    var IsSubmit = false;
    var ShowAddress = "1";
    var ShowTime = "2014-12-24";
    var Price = "880";
    $(function () {
        $(".xyUl li").click(function () {
            $(".xyUl li").removeClass("cur");
            $(this).addClass("cur");
            ShowAddress = $(this).data("address");
            if ($(this).attr("id") == "addr1") {
                $("#t1").show();
                $("#t2").show();
                $("#t3").show();
                $("#t4").show();
                $("#t5").show();
                $("#t6").hide();
                $("#t7").hide();
                $("#t8").hide();
                $("#t1").click();
            }
            else {
                $("#t1").hide();
                $("#t2").hide();
                $("#t3").hide();
                $("#t4").hide();
                $("#t5").hide();
                $("#t6").show();
                $("#t7").show();
                $("#t8").show();
                $("#t6").click();
            }

        })

        $(".rqUl li").click(function () {
            $(".rqUl li").removeClass("cur");
            $(this).addClass("cur");
            ShowTime = $(this).data("time");

        })

        $(".fr li").click(function () {
            $(".fr li").removeClass("cur");
            $(this).addClass("cur");
            $("#spprice").text("￥" + $(this).data("price"));
            Price = $(this).data("price");

        })

        $("#btnSubmit").click(function () {
            window.location.href = "Customize_BXGTConfirm.aspx?showaddress=" + ShowAddress + "&showtime=" + ShowTime + "&price=" + Price;

        })

    })



</script>
</html>
