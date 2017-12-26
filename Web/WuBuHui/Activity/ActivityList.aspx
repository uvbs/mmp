<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Activity.ActivityList" %>

<!DOCTYPE html >
<html lang="zh-cn">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>五步会活动列表</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.1">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body>
    <div class="behindbar">
        <div class="title wbtn_line_greenyellow">
            <span class="iconfont icon-12"></span>分类
        </div>
        <ul class="catlist">
            <%=sbCategory.ToString() %>
        </ul>
    </div>
    <div class="wtopbar">
        <div class="col-xs-2">
            <span class="wbtn  wbtn_line_greenyellow" id="categorybtn"><span class="iconfont icon-fenlei bigicon">
            </span></span>
        </div>
        <div class="col-xs-10">
            <span class="wbtn wbtn_main" onclick="OnSearch()"><span class="iconfont icon-111"></span>
            </span>
            <input type="text" class="searchtext" id="txtTitle">
        </div>
        <!-- /.col-lg-6 -->
    </div>
    <!-- /.container -->
    <div class="paixu">
        <div class="col-xs-6">
             <a href="#" class="wlink current" onclick="SortType('starttime')">最近活动</a>
        </div>
        <div class="col-xs-6">
           

            <a href="#" class="wlink" onclick="SortType('time')">最新添加</a>
        </div>
    </div>
    <div class="mainlist top86 bottom50 activelist" id="needload">
        <p class="loadnote" style="text-align: center;">
        </p>
    </div>
    <!-- mainlist -->
    <div class="footerbar">
	<div class="col-xs-2 ">
		<a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx">
			<span class="iconfont icon-back"></span>
		</a>
	</div><!-- /.col-lg-2 -->
	<div class="col-xs-8">

	</div><!-- /.col-lg-10 -->
            <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->

    </div>
    <!-- footerbar -->
    <div class="modal fade bs-example-modal-sm" id="gnmdb" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                        提交成功</p>
                </div>
                <div class="modal-footer textcenter">
                    <span class="wbtn wbtn_main" data-dismiss="modal">确认</span>
                    <!-- <a href="#" class="wbtn wbtn_main" data-dismiss="modal">确认</a> -->
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->
</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="../js/jquery.js" type="text/javascript"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="../js/bootstrap.js" type="text/javascript"></script>
<script src="/WuBuHui/js/behindbar.js?v=0.0.8"></script>
<script src="../js/bottomload.js" type="text/javascript"></script>
<script>

    var pageIndex = 1;
    var pageSize =100;
    var value = "starttime";
    var type = "";
    var title = "";
    $(function () {
        InitData();
        $(".paixu>div>a").click(function () {
            $("#needload>a").remove();
            // pageIndex = 1;
            type = "";
            $(".paixu>div>a").removeClass("wlink current").addClass("wlink");
            $(this).addClass("wlink current");
            // value = $(this).attr("v");
            //InitData();
        });

        $(".catlist>li").click(function () {
            title = "";
            value = "";
            type = "";
            pageIndex = 1;
            $("#needload>a").remove();
            $(".catlist>li").removeClass("list current").addClass("list");
            $(this).addClass("list current");
            type = $(this).attr('v');
            Reset();
            InitData();
        });
    });

    function OnSearch() {
        title = "";
        value = "";
        type = "";
        pageIndex = 1;
        $("#needload>a").remove();
        title = $("#txtTitle").val();
        InitData();
    }

    function SortType(sort) {
        title = "";
        value = "";
        type = "";
        pageIndex = 1;
        $("#needload>a").remove();
        value = sort;
        InitData();
    }


    function FormatDate(value) {
        if (value == null || value == "") {
            return "";
        }
        var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
        var month = padLeft(date.getMonth() + 1, 10);
        var currentDate = padLeft(date.getDate(), 10);
        var hour = padLeft(date.getHours(), 10);
        var minute = padLeft(date.getMinutes(), 10);
        var second = padLeft(date.getSeconds(), 10);
        return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minute;
    }


    function padLeft(str, min) {
        if (str >= min)
            return str;
        else
            return "0" + str;
    }

    function InitData() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiActivityHandler.ashx",
            data: { Action: "GetActivityList", pageIndex: pageIndex, pageSize: pageSize, value: value, Title: title, currUser: "", type: type },
            dataType: 'json',
            success: function (resp) {

                var html = "";
                if (resp.Status == 0) {
                    if (resp.ExObj == null) {
                        $(".loadnote").text("没有更多");
                        return;
                    }
                    $.each(resp.ExObj, function (index, data) {
                        if ((data.SignUpTotalCount >= data.MaxSignUpTotalCount) && (data.MaxSignUpTotalCount > 0) && (data.IsHide == 0)) {

                            html += '<a href="ActivityInfo.aspx?id=' + data.JuActivityID + '" class="listbox partyfull"><div class="mainimg">';
                        }
                        else if (data.IsHide == 0 || data.IsHide == -1) {
                            html += '<a href="ActivityInfo.aspx?id=' + data.JuActivityID + '" class="listbox "><div class="mainimg">';
                        }
                        else {
                            html += '<a href="ActivityInfo.aspx?id=' + data.JuActivityID + '" class="listbox partyover"><div class="mainimg">';
                        }


                        html += '<img src="' + data.ThumbnailsPath + '" alt=""></div>';
                        html += '<span class="wbtn_fly wbtn_flytr wbtn_greenyellow">费用：' + data.ActivityIntegral + ' 积分 </span><span class="baomingstatus">';
                        if (data.IsHide == -1) {

                            html += ' <span class="text">待开始</span>';
                        }
                        else if (data.IsHide == 1) {
                            html += ' <span class="text">已结束</span>';
                        }
                        else if ((data.SignUpTotalCount >= data.MaxSignUpTotalCount) && (data.MaxSignUpTotalCount > 0)) {
                            html += ' <span class="text">已满员</span>';
                        }

                        else {
                            html += ' <span class="text">进行中</span>';
                        }
                        html += '<svg class="sanjiao" version="1.1" viewbox="0 0 100 100">';
                        html += '<polygon points="100,100 0.2,100 100,0.2" /></svg></span>';
                        html += '<div class="activeconcent"><div class="textbox"><h3>';
                        html += data.ActivityName + '</h3><p>';
                        html += '<span class="iconfont icon-clock"></span><span class="text">时间:' + FormatDate(data.ActivityStartDate) + '</span></p>';
                        html += '<p> <span class="iconfont icon-adress"></span><span class="text">地址:' + data.ActivityAddress + '</span></p>';
                        html += '</div><div class="tagbox">';
                        html += '<span class="wbtn_tag wbtn_red"><span class="iconfont icon-eye"></span>' + data.PV + '</span>';
                        html += '<span class="wbtn_tag wbtn_orange"><span class="iconfont icon-36"></span>' + data.SignUpTotalCount + ' </span>';
                        html += '</div><div class="tagbox">';
                        html += '<span class="wbtn_tag wbtn_main">' + data.CategoryName + '</span>';
                        html += '</div></div></a>';
                    });
                    $(".loadnote").before(html);
                    $(".loadnote").text("　");

                }
                else {
                    $(".loadnote").text("没有更多");
                }
            },
            complete: function () {
                $("#needload").bottomLoad(function () {
                    $(".loadnote").text("正在加载...");
                    pageIndex++;
                    InitData();
                })
            }
        });
    }
    // alert(navigator.userAgent)
    var arr = [".footerbar", ".wtopbar", ".mainlist", ".paixu"];
    $("#categorybtn").controlbehindbar(arr, ".mainlist");
    function Reset() {

        //复位动画禁点击
        $(window).bind("click", function (e) {
            e.preventDefault();
        })
        //动画完 取消禁点击
        $(arr[0])[0].addEventListener("webkitTransitionEnd", cc)

        function cc() {
            $(arr[0])[0].removeEventListener("webkitTransitionEnd", cc)
            $(".behindbar").hide();
            setTimeout(function () {
                $(window).unbind("click");
            }, 500)
        }

        //复位动画
        for (var i in arr) {
            $(arr[i]).removeClass("sdiebartranslate")
        }
        $(".sidebarhidebtn").hide();


    }

</script>
 <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
 <script>
     var imgUrl = "http://" + window.location.host + "/WuBuHui/img/ccc.jpg";  //注意必须是绝对路径
     var lineLink = window.location.href;   //同样，必须是绝对路径
     var descContent = "五步会是一个领域专注的人才社交平台，拥有中国最大的线上专业人才交流互动微信，为专业领域度身定制。"; //分享给朋友或朋友圈时的文字简介
     var shareTitle = "五步会活动列表";  //分享title
     var wxconfig = $.parseJSON('<%=new ZentCloud.BLLJIMP.BLLWeixin("").GetJSAPIConfig("")%>');
     wx.config({
         debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
         appId: wxconfig.appId, // 必填，公众号的唯一标识
         timestamp: wxconfig.timestamp, // 必填，生成签名的时间戳
         nonceStr: wxconfig.nonceStr, // 必填，生成签名的随机串
         signature: wxconfig.signature, // 必填，签名，见附录1
         jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
     });
     wx.ready(function () {

         // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。
         wx.onMenuShareTimeline({
             title: shareTitle, // 分享标题
             link: lineLink, // 分享链接
             imgUrl: imgUrl, // 分享图标
             success: function () {
                 // 用户确认分享后执行的回调函数
                 //alert("分享到朋友圈成功");
             },
             cancel: function () {
                 // 用户取消分享后执行的回调函数
             }
         });
         wx.onMenuShareAppMessage({
             title: shareTitle, // 分享标题
             desc: descContent, // 分享描述
             link: lineLink, // 分享链接
             imgUrl: imgUrl, // 分享图标
             type: '', // 分享类型,music、video或link，不填默认为link
             dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
             success: function () {
                 // 用户确认分享后执行的回调函数
                 // alert("发送给朋友成功");
             },
             cancel: function () {
                 // 用户取消分享后执行的回调函数
             }
         });


     });

     wx.error(function (res) {
         // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
         //alert(res.errMsg);
     });

     //
    </script>
</html>
