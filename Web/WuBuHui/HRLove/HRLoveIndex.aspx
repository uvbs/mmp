<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HRLoveIndex.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Game.HRLoveIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="zh-CN">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
	<title>HR LOVE</title>
	<!-- Bootstrap -->
	<link rel="stylesheet" href="/wubuhui/css/wubu.css?v=0.0.1">
	<!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
	<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
	<!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body class="whitebg" >
<img src="../images/hrloveshare.jpg" style="position: absolute;opacity: 0;top: 0;left: 0;z-index: -99;width: 1px;height: 1px;border: none;" alt="">
<div class="jiesao"></div>
<div class="wtopbar">
	<div class="col-xs-12">
		<span class="wbtn wbtn_main" onclick="javascript: document.forms[0].submit();" >
			<span class="iconfont icon-111" ></span>
		</span>
        <form name="form1" method="post" action="HRLoveSearch.aspx">
        <input type="text" name="Title" class="searchtext">
        </form>
		



	</div><!-- /.col-lg-6 -->
    
</div><!-- /.container -->
<div class="hrlovelist top50 bottom50">
<p class="loadnote" style="text-align: center;">
        </p>
</div>
<div class="footerbar">
	<div class="col-xs-2 ">
		<a href="javascript:history.go(-1)" class="wbtn wbtn_main" type="button" href="#">
			<span class="iconfont icon-back"></span>
		</a>
	</div><!-- /.col-lg-2 -->
	<div class="col-xs-8">
		<a href="/Wubuhui/HRLove/HRLoveJoin.aspx" class="wbtn wbtn_line_main deletemessage" type="button" href="javascript:showdelete();">
			<span class="iconfont icon-34 smallicon"></span>
			<span class="text">我要参与</span>
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
<script src="../js/quo2.js" type="text/javascript"></script>
<script>
    var UserCount = 40;
    $(function () {
        InitData();
    });
    function InitData() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/GameFriendChainHandler.ashx",
            data: { Action: "GetRandomFriendList", count: UserCount},
            dataType: 'json',
            success: function (resp) {
                $(".loadnote").text("　");
                if (resp.Status == 0) {
                    if (resp.ExObj == null) {
                        $(".loadnote").text("没有更多");
                        return;
                    }
                    var html = "";
                     
                    $.each(resp.ExObj, function (Index, Item) {
                        var redirectUrl = "/WuBuHui/HRLove/HRLoveInfo.aspx?AutoId=" + Item.AutoId;
                        html += '<a class=\"hrloveavatar\" href=\"' + redirectUrl + '\">';
                        html += '<img src=\"' + Item.ThumbnailUrl + '\" class=\"wbtn_round\" alt=\"\">';
                        html+= '</a>';
                    });
                    $(".loadnote").before(html);
                    if (html == "") {
                        $(".loadnote").text("没有数据");
                    }
                    var hrloveheight = ($(window).height() - $(".hrlovelist").height()) / 3
                    $(".hrlovelist").css({ "margin-top": parseInt(hrloveheight) })
                    $(".jiesao").bind("tap", function () {
                        $(this).addClass("jiesaohide");
                    })
                    $(".jiesao")[0].addEventListener('webkitTransitionEnd', function () {
                        $(".jiesao").remove();
                    }, false); 




                } else {
                    $(".loadnote").text("没有数据");
                }
            },
            complete: function () {
            }
        });
    };
   
</script>
<script src="http://dev.comeoncloud.net/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: '五步会HR Love',
            desc: '五步会是一个领域专注的人才社交平台，拥有中国最大的线上专业人才交流互动微信，为专业领域度身定制。',
            //link: '', 
            imgUrl: "http://" + window.location.host + "/WuBuHui/img/ccc.jpg"
        }
        //    ,{
        //		message_s:function(){
        //			alert("好友分享成功")
        //		},
        //		message_c:function(){
        //			alert("好友分享取消")
        //		},
        //		timeline_s:function(){
        //			alert("朋友圈分享成功")
        //		},
        //		timeline_c:function(){
        //			alert("朋友圈分享取消")
        //		}
        //	}
    )
    })
</script>
</html>
