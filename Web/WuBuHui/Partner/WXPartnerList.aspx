<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXPartnerList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Partner.WXPartnerList" %>
<!DOCTYPE html >
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>五伴会列表</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WubuHui/css/wubu.css"/>
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body>
    <div class="behindbar">
        <div class="title wbtn_line_lightpurple">
            <span class="iconfont icon-44"></span>分类
        </div>
        <ul class="catlist">
            <li class="catli current"><a href="#">全部</a></li>
            <%=PartnerStr%>
        </ul>
    </div>
    <div class="wtopbar">
        <div class="col-xs-2">
            <span class="wbtn wbtn_line_lightpurple" id="categorybtn"><span class="iconfont icon-fenlei bigicon">
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
            <a href="#" class="wlink current" onclick="SortType('time')">最新加入</a>
        </div>
        <div class="col-xs-6">
            <a href="#" class="wlink" onclick="SortType('zan')">最高人气</a>
        </div>
    </div>
    <div class="mainlist top86 bottom50" id="needload">
        <p class="loadnote" style="text-align: center;" id="loadnote">
        </p>
    </div>
    <!-- mainlist -->
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-back"></span>
            </a>
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
<script src="/WubuHui/js/jquery.js"></script>
<script src="/WubuHui/js/bootstrap.js"></script>
<script src="/WubuHui/js/behindbar.js?v=0.0.3"></script>
<script src="/WubuHui/js/bottomload.js?v=0.0.2"></script>
<script src="../../Scripts/StringBuilder.js" type="text/javascript"></script>
<script type="text/javascript">
    var PageIndex = 1;
    var PageSize =10 ;
    var title = "";
    var ctype = "";
    var Sort = "time";
    $(function () {
        InitData();
        $(".catlist>li").click(function () {
            PageIndex = 1;
            title = "";
            $("#needload>a").remove();
            ctype = $(this).attr("v");
            InitData();
            $(".catlist>li").removeClass("catli current").addClass("catli");
            $(this).addClass("catli current");
        });
    })


    function OnSearch() {
        title = "";
        ctype = "";
        Sort = "time";
        PageIndex = 1;
        $("#needload a").remove();
        title = $("#txtTitle").val();
        InitData();

    }
    function SortType(sort) {
        title = "";
        ctype = "";
        $("#needload>a").remove();
        PageIndex = 1;
        Sort = sort;
        InitData();

    }
    $(".paixu>div>a").click(function () {
        //$("#needload a").remove();
        // PageIndex = 1;
        $(".paixu>div>a").removeClass("wlink current").addClass("wlink");
        $(this).addClass("wlink current");
        //InitData();
    });

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
        //return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minute + ":" + second;
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }

    function padLeft(str, min) {
        if (str >= min)
            return str;
        else
            return "0" + str;
    }

    function InitData() {

        try {
            $.ajax({
                type: 'post',
                url: "/Handler/App/WXWuBuHuiPartnerHandler.ashx",
                data: { Action: "GetPartnerInfoList", PageIndex: PageIndex, PageSize: PageSize, Type: ctype, Title: title, Sort: Sort },
                dataType: 'json',
                success: function (resp) {
                    $(".loadnote").text("　");
                    if (resp.Status == 0) {
                        var data = resp.ExObj;
                        if (data == null) {
                            $(".loadnote").text("没有更多");
                            return;
                        };
                        var html = new StringBuilder();
                        $.each(resp.ExObj, function (index, Item) {
                            html.AppendFormat('<a href="WXPartnerInfo.aspx?id={0}" class="listbox"><div class="touxiang wbtn_round">', Item.AutoId);
                            html.AppendFormat('<img src="{0}"></div>', Item.PartnerImg);
                            html.AppendFormat('<div class="textbox"><h3>{0}</h3><p>{1}<br/></p></div>', Item.PartnerName, Item.PartnerAddress);
                            html.AppendFormat('<div class="tagbox"><span class="wbtn_tag wbtn_red"><span class="iconfont icon-eye"></span>{0}</span>', Item.PartnerPv);
                            html.AppendFormat('<span class="wbtn_tag wbtn_red"><span class="iconfont icon-xin2"></span>{0}</span>', Item.ParTnerStep);

                            if (Item.Ctype != null) {
                                $.each(Item.Ctype, function (i, it) {
                                    html.AppendFormat('<span class="wbtn_tag wbtn_main">{0}</span>', it.CategoryName);
                                });
                            }
                            html.AppendFormat('</div>');
                            html.AppendFormat('<div class="wbtn_fly wbtn_flybr wbtn_lightpurple timetag">{0}</div>', FormatDate(Item.InsertDate));
                            html.AppendFormat('</a>');
                        });

                        $(".loadnote").before(html.ToString());
                        $(".loadnote").text("　");



                    }
                    else {
                        $(".loadnote").text("没有更多");
                    }
                },
                complete: function () {
                    $("#needload").bottomLoad(function () {
                        $(".loadnote").text("正在加载...");

                        PageIndex++;
                        InitData();
                    })
                }
            });
        } catch (e) {
            alert(e);
        }


    };



    var arr = [".footerbar", ".wtopbar", ".mainlist", ".paixu"];
    $("#categorybtn").controlbehindbar(arr, ".mainlist");

</script>
</html>

