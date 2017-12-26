<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Score.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.score.Score" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>积分首页</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.4">
    <!--<script data-main="/WuBuHui/js/score_v2" src="/WuBuHui/js/require.js"></script>-->
</head>
<body class="whitebg">
    <div class="scorebox">
        <div class="numbox">
            <span class="num" id="scoreshownum">
               <%=uinfo.TotalScore%>
                <span class="historyscore">(<%=uinfo.HistoryTotalScore%>)</span> </span>积分
        </div>
        <span  class="wbtn wbtn_flytr wbtn_fly wbtn_line_yellow">
         <%for (int i =1; i <=UserLevel; i++){%>
         <span class="iconfont icon-zuanshi"></span>
         <% } %>
        </span>
        <a href="HistoryScore.aspx" class="wbtn wbtn_red"
            id="earnscorebtn"><span class="iconfont icon-54"></span>积分规则 </a>
    </div>
    <!--<div class="earnscore">
        <a href="../News/NewsDetail.aspx?id=234218#1" class="col-xs-4 wbtn_line_red"><span
            class="iconfont icon-b55"></span>呼朋唤友</a> <a href="../News/NewsDetail.aspx?id=234218#2"
                class="col-xs-4 wbtn_line_greenyellow"><span class="iconfont icon-12"></span>分享活动
            </a><a href="../News/NewsDetail.aspx?id=234218#3" class="col-xs-4 wbtn_line_green"><span
                class="iconfont icon-78"></span>分享文章</a> <a href="../News/NewsDetail.aspx?id=234218#4"
                    class="col-xs-4 wbtn_line_main"><span class="iconfont icon-39"></span>分享职位</a>
        <a href="../News/NewsDetail.aspx?id=234218#5" class="col-xs-4 wbtn_line_green"><span
            class="iconfont icon-36"></span>分享导师</a> <a href="../News/NewsDetail.aspx?id=234218#6"
                class="col-xs-4 wbtn_line_yellow"><span class="iconfont icon-34"></span>导师回复</a>
        <span class="col-xs-4 wbtn_line_yellow"></span>
    </div>-->
    <div class="scorelist bottom50" id="needload">
        <div class="listbar">
            <span class="col-xs-4 current">全部</span> <span class="col-xs-4" v="1">支出</span>
            <span class="col-xs-4" v="2">收入</span>
        </div>
        <p class="loadnote" style="text-align: center;">
        </p>
    </div>
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:pagegoback('../MyCenter/Index.aspx')">
                <span class="iconfont icon-back"></span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            <a href="/App/Cation/Wap/Mall/ScoreMall.aspx" class="wbtn wbtn_line_main" type="button" id="earnscorebtn2"><span class="iconfont icon-76 smallicon">
            </span>积分商城 </a>
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
<script src="../js/comm.js" type="text/javascript"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="../js/bootstrap.js" type="text/javascript"></script>
<script src="../js/behindbar.js" type="text/javascript"></script>
<script src="../js/bottomload.js" type="text/javascript"></script>
<script>
    var pageIndex = 1;
    var pageSize = 10;
    var type = "";
    $(function () {

        InitData();

        $(".listbar>span").click(function () {
            pageIndex = 1;
            $(".listbar>span").removeClass("col-xs-4 current").addClass("col-xs-4");
            $(this).addClass("col-xs-4 current");
            type = $(this).attr("v");
            $("#needload>.listbox").remove();
            InitData();

        });


    })
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
            data: { Action: "GetScoreRecordS", type: type, PageIndex: pageIndex, PageSize: pageSize },
            dataType: 'json',
            success: function (resp) {
                var html = "";

                if (resp.Status == 0) {

                    if (resp.ExObj == null) {
                        $(".loadnote").text("没有更多");
                        return;
                    }
                    $.each(resp.ExObj, function (index, data) {
                        html += '<div class="listbox">';
                        html += '<span class="wbtn_round wbtn_yellow"><span class="iconfont icon-' + data.Nums + '"></span></span>'
                        html += '<div class="textbox"><h3>' + data.NameStr + '</h3>';
                        html += '<span class="time">' + FormatDate(data.InsertDate) + '</span></div>';
                        html += '<div class="scorenum addscore">' + data.ScoreNum + '</div></div>'
                    });
                    //$("#needload").append(html);
                    $(".loadnote").before(html);
                    if (html == "") {
                        $(".loadnote").text("没有更多");
                    }
                }
                else {
                    $(".loadnote").text("没有更多");
                }
            },
            complete: function () {
                $("#needload").bottomLoad(180, function () {
                    $(".loadnote").text("正在加载...");
                    pageIndex++;
                    InitData();
                });
            }
        });

    };

</script>
</html>
