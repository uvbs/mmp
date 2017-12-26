<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyActivityInfo.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Activity.MyActivityInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>活动列表</title>
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
            <span class="wbtn  wbtn_line_greenyellow" id="categorybtn"><span class="iconfont icon-12 bigicon">
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
            <a href="#" class="wlink current" onclick="SortType('time')">最新添加</a>
        </div>
        <div class="col-xs-6">
            <a href="#" class="wlink" onclick="SortType('num')">报名人数</a>
        </div>
    </div>
    <div class="mainlist top86 bottom50 activelist" id="needload">
        <p class="loadnote" style="text-align: center;">
        </p>
    </div>
    <!-- mainlist -->
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-back">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
        </div>
        <!-- /.col-lg-10 -->
                   <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
    </div>
    <!-- footerbar -->
</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="../js/jquery.js" type="text/javascript"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="../js/bootstrap.js" type="text/javascript"></script>
<script src="/WuBuHui/js/behindbar.js?v=0.0.8"></script>
<script src="../js/bottomload.js" type="text/javascript"></script>

<script>

    var pageIndex = 1;
    var pageSize = 4;
    var value = "";
    var type = "";
    var title = "";
    $(function () {

        InitData();
        $(".paixu>div>a").click(function () {
            $("#needload>a").remove();
            $(".paixu>div>a").removeClass("wlink current").addClass("wlink");
            $(this).addClass("wlink current");
            pageIndex = 1;
            type = "";
            //value = $(this).attr("v");
            //InitData();
        });

        $(".catlist>li").click(function () {
            value = "";
            type = "";
            title = "";
            $("#needload>a").remove();
            $(".catlist>li").removeClass("list current").addClass("list");
            $(this).addClass("list current");
            value = "";
            type = $(this).attr('v');
            InitData();

        });
    });

    function OnMyList() {
        value = "";
        type = "";
        title = "";
        $("#needload>a").remove();
        title = "";
        InitData();
    }

    function OnSearch() {
        value = "";
        type = "";
        title = "";
        pageIndex = 1;
        $("#needload>a").remove();
        title = $("#txtTitle").val();
        InitData();
    }
    function SortType(sort) {
        value = "";
        type = "";
        title = "";
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
        return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minute + ":" + second;
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
            data: { Action: "GetActivityList", pageIndex: pageIndex, pageSize: pageSize, value: value, type: type, currUser: "my" },
            dataType:'json',
            success: function (resp) {
                $(".loadnote").text("没有更多");
                var html = "";
                if (resp.Status == 0) {
                    if (resp.ExObj == null) {
                       
                        //$('#gnmdb').find("p").text("没有数据");
                        //$('#gnmdb').modal('show');
                        return;
                    }
                    $.each(resp.ExObj, function (index, data) {
                        if (data.IsHide == 0) {
                            html += '<a href="ActivityInfo.aspx?id=' + data.JuActivityID + '" class="listbox "><div class="mainimg">';
                        } else {
                            html += '<a href="ActivityInfo.aspx?id=' + data.JuActivityID + '" class="listbox partyover"><div class="mainimg">';
                        }
                        html += '<img src="' + data.ThumbnailsPath + '" alt=""></div>';
                        html += '<span class="wbtn_fly wbtn_flytr wbtn_greenyellow">费用：' + data.ActivityIntegral + ' 积分 </span><span class="baomingstatus">';
                        //                        html += '<span class="text">进行中</span>';
                        if (data.IsHide ==0) {
                            html += ' <span class="text">进行中</span>';
                        } else {
                            html += ' <span class="text">已结束</span>';
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
//                    $('#gnmdb').find("p").text(resp.Msg);
                    //                    $('#gnmdb').modal('show');
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
</script>
</html>
