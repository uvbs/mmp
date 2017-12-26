<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.MyCenter.Index" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <meta name="format-detection" content="telephone=no" />
    <title>五步会</title>
    <!-- Bootstrap -->
   <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=1.0.9">
    <script data-main="/WuBuHui/js/indexmain" src="/WuBuHui/js/require.js"></script>
    <script>        var IsHaveUnReadMessage = "<%=IsHaveUnReadMessage%>";</script>
</head>
<body>

    <div class="wcontainer mainnav">
	<a href="/WuBuHui/WordsQuestions/WXDiscussList.aspx" class="col-xs-3 wbtn_line_lightyellow">
		<span class="iconfont icon-xuanchu wbtn_lightyellow"></span>
		<span class="text">话题</span>
	</a>
	<a href="/WubuHui/Position/WXPositionList.aspx" class="col-xs-3 wbtn_line_lightblue">
		<span class="iconfont icon-zhiwei wbtn_lightblue"></span>
		<span class="text">职位</span>
	</a>
	<a href="/WubuHui/Partner/WXPartnerList.aspx" class="col-xs-3 wbtn_line_lightpurple">
		<span class="iconfont icon-wubanhui wbtn_lightpurple"></span>
		<span class="text">五伴会</span>
	</a>
	<a href="/WubuHui/Member/ScoreTop.aspx" class="col-xs-3 wbtn_line_lightgreenyellow">
		<span class="iconfont icon-paihangbang wbtn_lightgreenyellow"></span>
		<span class="text">排行榜</span>
	</a>
	<a href="/App/Cation/Wap/Mall/ScoreMall.aspx" class="col-xs-3 wbtn_line_lightredpurple">
		<span class="iconfont icon-jifenshangcheng wbtn_lightredpurple"></span>
		<span class="text">积分商城</span>
	</a>
	<a href="/WuBuHui/Tutor/TutorList.aspx" class="col-xs-3 wbtn_line_orange">
		<span class="iconfont icon-wushihui wbtn_orange"></span>
		<span class="text">五师会</span>
	</a>
	<a href="/WuBuHui/Activity/ActivityList.aspx" class="col-xs-3 wbtn_line_lightgreen">
		<span class="iconfont icon-huodong wbtn_lightgreen"></span>
		<span class="text">活动</span>
	</a>
	<a href="/WuBuHui/HRLove/HRLoveIndex.aspx" class="col-xs-3 wbtn_line_red">
		<span class="iconfont icon-hrlove wbtn_red"></span>
		<span class="text">HRLove</span>
	</a>
</div>
    <div class="wcontainer">
        <div class="slider" id="slider">
            <!-- <div class="sliderbox"> -->
            <%=BannerStr %>
            <!-- </div> -->
        </div>
    </div>
    <div class="wcontainer indexlist">
        <div class="tagbar">
            <span class="wbtn wbtn_greenyellow" id="newslistbtn"><span class="iconfont icon-78">
            </span><span class="title">市场新闻</span> </span>
            <span class="wbtn" id="discusslistbtn">
                <span class="iconfont icon-34"></span><span class="title">最新话题</span> </span>

             <span class="wbtn"  id="followarticlebtn">
			<span class="iconfont icon-78"></span>
			<span class="title">关注圈</span>
		</span>

        </div>
        <div class="mainlist bottom50 discusslist">
            <div id="disussList">
            </div>
            <span class="watchmore"><a href="../WordsQuestions/WXDiscussList.aspx" class="wbtn wbtn_yellow">
                <span class="title">查看更多</span> </a></span>
        </div>
        <!-- mainlist -->
        <div class="mainlist bottom50 articlelist">
            <div id="articleList">
            </div>
            <span class="watchmore"><a href="../News/MarketNews.aspx?id=237" class="wbtn wbtn_greenyellow">
                <span class="title">查看更多</span> </a></span>
        </div>
        <!-- mainlist -->
       <!-- mainlist -->
        	<div class="mainlist bottom50 followarticlelist">
            
            <div id="guanzhudiscusslist">
            </div>
            
            
        </div>
        <!-- mainlist -->
    </div>
    
    
    <!-- footerbar -->
    <script type="text/javascript" src="../js/footer.js?v=2.2"></script>

    <div class="modal fade bs-example-modal-sm" id="gnmdb" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                    </p>
                </div>
                <div class="modal-footer textcenter">
                <a class="wbtn wbtn_main"  href ="http://mp.weixin.qq.com/s?__biz=MzA4OTAyOTE5OA==&mid=201343726&idx=1&sn=166fb03c9823c5b4f198792c15a3d877&scene=1&from=groupmessage&isappinstalled=0#rd">立即关注</a>
                    <span class="wbtn wbtn_main" data-dismiss="modal">继续浏览</span>
                    <!-- <a href="#" class="wbtn wbtn_main" data-dismiss="modal">确认</a> -->
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</body>
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script type="text/javascript">

       
  require.config({

	shim: {
		'bootstrap': ['jquery'],
		
		
	},
	
	urlArgs: "v=2.2.1.1"
})
require(["bootstrap"],function() {


 var handlerUrl = "/Handler/App/WXWuBuHuiUserHandler.ashx";

    $(function () {

        LoadNewList();
        LoadDiscussList();
        LoadMyAttentionDiscussList();
    })
    //加载新闻列表
    function LoadNewList() {
        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { Action: 'GetNewsList', CategoryId: "<%=MarketNewsIds%>", PageIndex: 1, PageSize: 10 },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {

                if (resp.ExObj == null) { return; }
                var listHtml = '';
                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    //
                    str.AppendFormat('<a href="../News/NewsDetail.aspx?id={0}" class="listbox">', resp.ExObj[i].JuActivityID);
                    str.AppendFormat('<div class="textbox">');
                    str.AppendFormat('<h3 class="shorttitle">{0}</h3>', resp.ExObj[i].ActivityName);
                    str.AppendFormat('<p>{0}</p>', resp.ExObj[i].Summary);
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="tagbox">');
                    str.AppendFormat('<span class="wbtn_tag wbtn_red">');
                    str.AppendFormat('<span class="iconfont icon-eye"></span>{0}', resp.ExObj[i].PV);
                    str.AppendFormat('</span>');
                    str.AppendFormat('<span class="wbtn_tag wbtn_main">');
                    str.AppendFormat(resp.ExObj[i].CategoryName);
                    str.AppendFormat('</span>');
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="wbtn_fly wbtn_flybr wbtn_greenyellow timetag">');
                    str.AppendFormat(FormatDate(resp.ExObj[i].LastUpdateDate));
                    str.AppendFormat('</div>');
                    str.AppendFormat('</a>');

                };

                listHtml += str.ToString();
                if (listHtml == "") {
                    listHtml = "没有最新新闻";
                }
                $("#articleList").html(listHtml);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    //$('#alertconcent').text("加载超时，请刷新页面");
                    // $('#alertbox').modal('show');
                }

            }
        });




    }


    //加载话题列表
    function LoadDiscussList() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: 'GetReviewInfoList', UserId: "", PageIndex: 1, PageSize: 10, HaveReply: "1", Sort: "Newhf" },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
               if (resp.ExObj==null) {
                    $("#disussList").html("没有最新话题");
                    return;
                }
                var listHtml = '';
                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    str.AppendFormat('<a href="../WordsQuestions/WXDiscussInfo.aspx?AutoId={0}" class="listbox">', resp.ExObj[i].AutoId);
                    str.AppendFormat('<div class="textbox">');
                    str.AppendFormat('<h3 class="shorttitle">{0}</h3>', resp.ExObj[i].ReviewTitle);
                    str.AppendFormat('<p>{0}</p>', resp.ExObj[i].ReviewContent);
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="smalltagbox">');
                    str.AppendFormat('<span class="wbtn_tag wbtn_red">');
                    str.AppendFormat('<span class="iconfont icon-xin2"></span>{0}', resp.ExObj[i].PraiseNum);
                    str.AppendFormat('</span>');

                    if (resp.ExObj[i].actegory != null) {
                        $.each(resp.ExObj[i].actegory, function (Index, cItem) {
                            str.AppendFormat('<span class="wbtn_tag wbtn_main">{0}</span>', cItem.CategoryName);
                        });
                    }

                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="wbtn_fly wbtn_flybr wbtn_yellow timetag">');
                    str.AppendFormat(FormatDate(resp.ExObj[i].ReplyDateTiem));
                    str.AppendFormat('</div>');
                    str.AppendFormat('</a>');

                };

                listHtml += str.ToString();
                if (listHtml == "") {
                    listHtml = "没有最新话题";
                }
                $("#disussList").html(listHtml);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    //$('#alertconcent').text("加载超时，请刷新页面");
                    // $('#alertbox').modal('show');
                }

            }
        });




    }




   //加载我关注的导师的话题
    function LoadMyAttentionDiscussList() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: 'GetReviewInfoList', UserId: "", PageIndex: 1, PageSize: 10, MyAttention: "1", Sort: "Newhf" },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
             if (resp.ExObj==null) {
                    $("#guanzhudiscusslist").html("没有关注圈话题");
                    return;
                }
                var listHtml = '';
                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    str.AppendFormat('<a href="../WordsQuestions/WXDiscussInfo.aspx?AutoId={0}" class="listbox">', resp.ExObj[i].AutoId);
                    str.AppendFormat('<div class="textbox">');
                    str.AppendFormat('<h3 class="shorttitle">{0}</h3>', resp.ExObj[i].ReviewTitle);
                    str.AppendFormat('<p>{0}</p>', resp.ExObj[i].ReviewContent);
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="smalltagbox">');
                    str.AppendFormat('<span class="wbtn_tag wbtn_red">');
                    str.AppendFormat('<span class="iconfont icon-xin2"></span>{0}', resp.ExObj[i].PraiseNum);
                    str.AppendFormat('</span>');

                    if (resp.ExObj[i].actegory != null) {
                        $.each(resp.ExObj[i].actegory, function (Index, cItem) {
                            str.AppendFormat('<span class="wbtn_tag wbtn_main">{0}</span>', cItem.CategoryName);
                        });
                    }

                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="wbtn_fly wbtn_flybr wbtn_yellow timetag">');
                    str.AppendFormat(FormatDate(resp.ExObj[i].ReplyDateTiem));
                    str.AppendFormat('</div>');
                    str.AppendFormat('</a>');

                };

                listHtml += str.ToString();
                if (listHtml == "") {
                    listHtml = "没有最新话题";
                }
                $("#guanzhudiscusslist").html(listHtml);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                  
                }

            }
        });




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





       var isWeixinFollwer = "<%=isWeixinFollower%>";
       isWeixinFollwer="true";
       if (isWeixinFollwer == "False") {
           $('#gnmdb').find("p").text("您还没有关注五步会公众号，立即关注获得50积分和更多功能！");
           $('#gnmdb').modal('show');
       }




})
</script>

</html>
