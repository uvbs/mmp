<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreRank.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.ScoreRank" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>积分排行榜</title>
    <!-- Bootstrap -->
    <link href="/WuBuHui/css/wubu.css" rel="stylesheet" type="text/css" />
    <style>
        .loading
        {
            text-align: center;
        }
        .nodata
        {
            text-align: center;
        }
    </style>
</head>
<body class="whitebg">
    <div class="mainlist">
        <div class="tagbar">
            <a href="#" class="wbtn wbtn_greenyellow" id="btnYear">
                <!-- <span class="iconfont icon-78"></span> -->
                <span class="title">年度排行</span> </a><a href="#" class="wbtn" id="btnMonth">
                    <!-- <span class="iconfont icon-78"></span> -->
                    <span class="title">本月排行</span> </a>
        </div>
    </div>
    <div class="scoretoplist" id="datalist">
    <h5 class="loading">加载中...</h5>
    </div>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script type="text/javascript">

    var year = 2016;
    var month = "";
    var currentTime = new Date();
    $(function () {
        year = currentTime.getFullYear();   //获取完整的年份(4位,1970-????)  
        LoadData();
        $("#btnYear").click(function () {
            $(this).addClass("wbtn_greenyellow");
            $(btnMonth).removeClass("wbtn_greenyellow");
            $("#datalist").html("<h5 class='loading'>加载中...</h5>");
            year = currentTime.getFullYear();   //获取完整的年份(4位,1970-????)  
            month = "";
            LoadData();
        })
        $("#btnMonth").click(function () {
            $(this).addClass("wbtn_greenyellow");
            $(btnYear).removeClass("wbtn_greenyellow");
            $("#datalist").html("<h5 class='loading'>加载中...</h5>");
            month = currentTime.getMonth() + 1;      //获取当前月份(0-11,0代表1月) 
            year = "";
            LoadData();
        })


    })


    //加载列表
    function LoadData() {
        try {
            jQuery.ajax({
                type: "Get",
                url: "/serv/api/score/rank.ashx",
                data: { year: year, month: month },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    var sb = new StringBuilder();
                    for (var i = 0; i < resp.result.length; i++) {

                        sb.AppendFormat('<div class="listbox">');
                        sb.AppendFormat('<pan class="listnum">{0}</pan>', i + 1);
                        sb.AppendFormat('<span class="wbtn_round touxiang">');
                        sb.AppendFormat('<img src="{0}" >', resp.result[i].head_img_url);
                        sb.AppendFormat('</span>');
                        sb.AppendFormat('<span class="name">{0}</span>', resp.result[i].show_name);
                        sb.AppendFormat('<span class=\"score\">{0}</span>', resp.result[i].score);
                        sb.AppendFormat('</div>');

                    }
                    if (resp.result.length == 0) {
                        sb.AppendFormat('<h5 class="nodata">暂时没有数据</h5>');
                    }

                    $("#datalist").html(sb.ToString());

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时，请刷新重试");
                    }
                }
            })
        } catch (e) {
            alert(e);
        }
    }

</script>
</html>
