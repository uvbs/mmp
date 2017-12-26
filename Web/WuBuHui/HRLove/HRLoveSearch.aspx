<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeBehind="HRLoveSearch.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.HRLove.HRLoveSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="zh-CN">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
	<title>HR LOVE</title>
	<!-- Bootstrap -->
	<link rel="stylesheet" href="../css/wubu.css?v=0.0.1">
	<!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
	<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
	<!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body>

<div class="wtopbar">
	<div class="col-xs-12">
		<span class="wbtn wbtn_main" ontouchend="Search()" >
			<span class="iconfont icon-111"></span>
		</span>
		<input type="text" id="txtTitle"  value="<%=Request["Title"] %>" class="searchtext">
	</div><!-- /.col-lg-6 -->
</div><!-- /.container -->

<div class="mainlist top50 bottom50 hrlovemainlist" id="needload">
<!-- style="background-color:#000;http://www.baidu.com" -->



    <p class="loadnote" style="text-align: center;">
        </p>
</div><!-- mainlist -->

<div class="footerbar">
	<div class="col-xs-2 ">
		<a  class="wbtn wbtn_main" type="button" href="javascript:history.go(-1)">
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
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script src="../js/bottomload.js" type="text/javascript"></script>
<script>

    var PageIndex = 1;
    var PageSize = 10;
    $(function () {
        LoadData();


       
    });

    function Search() {
        PageIndex = 1;

        $("#needload>a").remove();
        LoadData();
    }

   

   function LoadData() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/GameFriendChainHandler.ashx",
            data: { Action: "QueryFriendList", PageIndex: PageIndex, PageSize: PageSize, Title: $("#txtTitle").val() },
            dataType: 'json',
            success: function (resp) {
                var str = new StringBuilder();
                if (resp.ExObj.length==0) {
                    $(".loadnote").text("没有数据");
                   
                }
                $.each(resp.ExObj, function (index, item) {
                    str.AppendFormat('<a href="/WuBuHui/HRLove/HRLoveInfo.aspx?AutoId={0}" class="listbox" >', item.AutoId);
                    str.AppendFormat('<div class="touxiang ">');
                    str.AppendFormat('<img src="{0}" >', item.ThumbnailUrl);
                    str.AppendFormat('<svg   viewBox="0 0 400 400" class="picshade" >');
                    str.AppendFormat('<path d="M3.1,143.2C17.3,72.6,74.9,15.5,149.8,42.1c4.8,1.7,46.7,23,46.6,29.7c0,0.2,0.2,0.3,0.3,0.1');
                    str.AppendFormat('c26.8-30.3,76.8-47.6,116-35.4c54.6,17,86.7,73,87.3,128.3V0H0v171.8C0.1,162.4,1.1,152.8,3.1,143.2z"/>');
                    str.AppendFormat('<path d="M397.7,192.2c-7.9,42.5-35.8,78.4-66.8,107.2c-50.5,46.9-109.4,86.8-180,58.8');
                    str.AppendFormat('c-40.3-16-77.7-44.4-106.7-75.8C15.8,251.6-0.2,213.6,0,173.1V400h400V168.5C399.9,176.5,399.1,184.4,397.7,192.2z"/>');
                    str.AppendFormat('</svg>');
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="textbox">');
                    str.AppendFormat('<h3 class="hrlovelisttitle">{0}</h3>', item.Name);
                    str.AppendFormat('<h3 class="hrlovelisttitle">');
                    str.AppendFormat('{0} <span class="iconfont icon-{1}"></span>', item.StarSign, GetStarSignIcon(item.StarSign));
                    str.AppendFormat('</h3>');

                    str.AppendFormat('</div>');
                    str.AppendFormat('</a>');

                });
                $(".loadnote").before(str.ToString());
                


            },
            complete: function () {
                $("#needload").bottomLoad(function () {
                    $(".loadnote").text("正在加载...");
                    PageIndex++;
                    LoadData();
                })
            }
        });
    }


    function GetStarSignIcon(starsign){
    var icon="";
    switch (starsign) {
    case "巨蟹":
    icon="juxiezuo";
    break;
        case "射手":
    icon="sheshou";
    break;
    case "水瓶":
            icon = "shuipingzuo";
    break;
        case "天秤":
    icon="tianpingzuo";
    break;
        case "金牛":
    icon="jinniuzuo";
    break;
        case "天蝎":
    icon="tianxiezuo";
    break;
        case "白羊":
    icon="baiyangzuo";
    break;
        case "双鱼":
    icon="shuangyuzuo";
    break;
        case "双子":
    icon="shuangzizuo";
    break;
        case "摩羯":
    icon="mojiezuo";
    break;
        case "狮子":
    icon="shizizuo";
    break;
case "处女":
    icon = "chunvzuo";
    break;
default:


   
}
    
    
     return icon;
    
    
    }

    var arr = [".footerbar", ".wtopbar", ".mainlist", ".paixu"];
    $("#categorybtn").controlbehindbar(arr, ".mainlist");
</script>
