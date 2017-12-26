<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreMall.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.ScoreMall" %>

<!DOCTYPE html >
<html lang="zh-cn">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>积分商城</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://at.alicdn.com/t/font_1413272586_8236315.css">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.4">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <style>
        .mainlist li:last-child
        {
            padding-bottom: 0px;
        }
        .col-xs-3{width:33.33%;}
    </style>
</head>
<body>
<%string WebSiteOwner = ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner; %>
    <%if (WebSiteOwner.Equals("qianwei"))
      {%>
    <div class="qianweinav">
        <span class="current"><a href="#">在线礼品</a></span> <span><a href="MallScoreOnline.aspx">
            合作商家</a></span>
    </div>
    <%}%>
    <%else
        {%>
    <%} %>
    <div class="behindbar">
        <div class="title wbtn_line_yellow">
            <span class="iconfont icon-34"></span>分类
        </div>
        <ul class="catlist">
            <li class="catli current" data-categorid=""><a>全部</a></li>
            <%=CategoryStr%>
        </ul>
    </div>
    <div class="wtopbar">
        <div class="col-xs-2">
            <span class="wbtn wbtn_line_yellow" id="categorybtn"><span class="iconfont icon-fenlei bigicon">
            </span></span>
        </div>
        <div class="col-xs-10">
            <span class="wbtn wbtn_main" onclick="OnSearch()"><span class="iconfont icon-111"></span>
            </span>
            <input type="text" class="searchtext" id="txtTitle" />
        </div>
        <!-- /.col-lg-6 -->
    </div>
    <!-- /.container -->
    <div class="paixu">
        <div class="col-xs-3">
            <a href="#" class="wlink current" data-orderby="score" >积分</a>
        </div>
        <div class="col-xs-3">
            <a href="#" class="wlink" data-orderby="pv" >人气</a>
        </div>
        <div class="col-xs-3">
            <a href="#" class="wlink" data-orderby="time">最新</a>
        </div>

    </div>
    <div class="mainlist top86 bottom50" id="needload">
       <ul class="mainlist score_mall" id="productlist">
        <!-- listbox -->
        
       </ul>
       <p class="loadnote" style="text-align: center;">
        </p>
    </div>
    <!-- mainlist -->
    <%if (WebSiteOwner.Equals("wubuhui") || WebSiteOwner.Equals("xixinxian"))
      {%>
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:history.go(-1)"><span class="iconfont icon-back">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            <a href="/Wubuhui/Score/Score.aspx" class="wbtn wbtn_line_main"><span class="iconfont icon-54 smallicon">
            </span>赚取积分</a>
        </div>
        <!-- /.col-lg-10 -->
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="/Wubuhui/MyCenter/Index.aspx"><span
                class="iconfont icon-b11"></span></a>
        </div>
        <!-- /.col-lg-2 -->
    </div>
    <!-- footerbar -->
    <%}%>
     <%else if (WebSiteOwner.Equals("haima") )
      {%>
      
      <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:history.go(-1)"><span class="iconfont icon-back">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            <a href="MyCenter.aspx" class="wbtn wbtn_line_main"><span class="iconfont icon-54 smallicon">
            </span>个人中心</a>
        </div>
        <!-- /.col-lg-10 -->
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="#"><span
                class="iconfont icon-b11"></span></a>
        </div>
        <!-- /.col-lg-2 -->
    </div>

      <%}%>
    <%else
        {%>
    <div class="backbar">

       <%if (WebSiteOwner.Equals("forbes")){%>
          <a href="/customize/forbes/index.html" class="back"><span class="icon"></span></a>
         <%}else
         {%>
          <a href="javascript:window.history.go(-1)" class="back"><span class="icon"></span></a>
         <%}%>
    </div>
    <%} %>
</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="http://cdn.bootcss.com/jquery/1.11.1/jquery.min.js"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="http://cdn.bootcss.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
<script src="/WuBuHui/js/behindbar.js?v=0.0.3"></script>
<script src="/WuBuHui/js/bottomload.js?v=0.0.3"></script>
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script>
    var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
    var PageIndex = 1;
    var PageSize = 10;
    var Title = ""; //商品名称
    var CategoryId = ""; //商品分类
//    var ScoreMin = ""; //最小积分
//    var ScoreMax = ""; //最大积分
    var OrderBy = "score";
    $(function () {
        LoadData();
        $(".catlist>li").click(function () {
            ResetSearchCondition();
            //$("#needload>ul").remove();
            $("#txtTitle").val("");
            CategoryId = $(this).data("categorid");
            $(".catlist>li").removeClass("catli current").addClass("catli");
            $(this).addClass("catli current");
            $(".paixu>div>a").removeClass("wlink current");
            Reset();
            LoadData();

        });
    })
    $(".paixu>div>a").click(function () {
        //$("#needload>ul").remove();
        $("#txtTitle").val("");
        ResetSearchCondition();
//        ScoreMin = $(this).data("scoremin");
//        ScoreMax = $(this).data("scoremax");
        OrderBy = $(this).data("orderby");
        $(".paixu>div>a").removeClass("wlink current").addClass("wlink");
        $(this).addClass("wlink current");
        LoadData();
    });

    function OnSearch() {
        ResetSearchCondition();
        //$("#needload>ul").remove();
        $(".paixu>div>a").removeClass("wlink current");
        Title = $("#txtTitle").val();
        LoadData();
    }
    function LoadData() {
        $.ajax({
            type: 'post',
            url: mallHandlerUrl,
            data: { Action: "QueryScoreProductsObjList", PageIndex: PageIndex, PageSize: PageSize, TypeId: CategoryId, OrderBy: OrderBy, Line: 0, Title: Title },
            dataType: 'json',
            success: function (resp) {
                $(".loadnote").text("　");
                if (resp.Status == 0) {
                    if (resp.ExObj.length == 0) {
                        $(".loadnote").text("没有更多");
                        return;
                    }
                    var listHtml = '';
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        var str = new StringBuilder();
                        str.AppendFormat('<li>');
                        str.AppendFormat('<div class="productbox">');

                        var href = "javascript:void(0);";
                        if (resp.ExObj[i].Stock > 0) {
                            href = "ScoreProductDetail.aspx?pid=" + resp.ExObj[i].AutoID;
                        }
                        str.AppendFormat('<a href="{0}" class="img">', href);
                        str.AppendFormat('<img src="{0}">', resp.ExObj[i].RecommendImg);
                        str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].PName);
                        str.AppendFormat('</a>');
                        if (resp.ExObj[i].DiscountScore > 0 && (resp.ExObj[i].DiscountScore != resp.ExObj[i].Score)) {
                            str.AppendFormat('<p class="price">原价：<del><span class="orangetext">{0}</span>积分</del></p>', resp.ExObj[i].Score);
                            str.AppendFormat('<p class="price">现价：<span class="orangetext">{0}</span>积分</p>', resp.ExObj[i].DiscountScore);
                        }
                        else {
                            str.AppendFormat('<p class="price">该商品不打折</p>', resp.ExObj[i].DiscountScore);
                            str.AppendFormat('<p class="price">现价：<span class="orangetext">{0}</span>积分</p>', resp.ExObj[i].Score);
                        }
                        str.AppendFormat('<p class="price">库存：<span class="orangetext">{0}</span></p>', resp.ExObj[i].Stock);
                        if (resp.ExObj[i].Stock == 0) {
                            str.AppendFormat('<span class="btn_min orange">库存不足</span>');
                        }
                        else {
                            str.AppendFormat('<a href="{0}" class="btn_min orange">我要兑换</a>', href);

                        }
                        str.AppendFormat('</div>');
                        str.AppendFormat('</li>');
                        listHtml += str.ToString();
                    };
                    listHtml += "";
                   // $(".loadnote").before(listHtml);
                    $(productlist).append(listHtml);

                } else {
                    $(".loadnote").text("没有更多");
                }
            },
            complete: function () {
                $("#needload").bottomLoad(function () {
                    $(".loadnote").text("正在加载...");
                    PageIndex++;
                    LoadData();
                })


            }
        });
    };

    function ResetSearchCondition() {
        PageIndex = 1;
        Title = "";
        CategoryId = "";
        ScoreMin = "";
        ScoreMax = "";
        $(productlist).html("");
    }
    var arr = [".footerbar", ".wtopbar", ".mainlist", ".paixu"];
    $("#categorybtn").controlbehindbar(arr, ".mainlist");

    function Reset() {

        //复位动画禁点击
//        $(window).bind("click", function (e) {
//            e.preventDefault();
        //        })
        //动画完 取消禁点击
        //$(arr[0])[0].addEventListener("webkitTransitionEnd", cc)

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
</html>
