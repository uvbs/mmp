<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXDiscussList.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WordsQuestions.WXDiscussList" %>

<!DOCTYPE html >
<html lang="zh-cn">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>话题列表</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://at.alicdn.com/t/font_1413272586_8236315.css">
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.4">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body>
    <div class="behindbar">
        <div class="title wbtn_line_yellow">
            <span class="iconfont icon-34"></span>分类
        </div>
        <ul class="catlist">
            <li class="catli current"><a>全部</a></li>
            <%=DisussStr %>
        </ul>
    </div>
    <div class="wtopbar">
        <div class="col-xs-2">
            <span class="wbtn wbtn_line_yellow" id="categorybtn"><span class="iconfont icon-fenlei bigicon">
            </span></span>
        </div>
        <div class="col-xs-10">
            <span class="wbtn wbtn_main" onclick="Search()"><span class="iconfont icon-111"></span>
            </span>
            <input type="text" class="searchtext" id="txtTitle">
        </div>
        <!-- /.col-lg-6 -->
    </div>
    <!-- /.container -->
    <div class="paixu">
        <div class="col-xs-4">
            <a href="#" class="wlink current" v="Newhf">最新回复</a>
        </div>
        <div class="col-xs-4">
            <a href="#" class="wlink" v="Mosthf">最多回复</a>
        </div>
        <div class="col-xs-4">
            <a href="#" class="wlink" v="Mosthp">最高人气</a>
        </div>
    </div>
    <div class="mainlist top86 bottom50" id="needload">
        <!-- listbox -->
        <p class="loadnote" style="text-align: center;">
        </p>
    </div>
    <!-- mainlist -->
<%--    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="/WuBuHui/MyCenter/Index.aspx"><span
                class="iconfont icon-back"></span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            <a class="wbtn wbtn_line_main" type="button" href="MyWXDiscussList.aspx"><span class="iconfont icon-34 smallicon">
            </span>我的话题 </a>
        </div>
        <!-- /.col-lg-10 -->
                <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
    </div>
    <!-- footerbar -->--%>
</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="http://cdn.bootcss.com/jquery/1.11.1/jquery.min.js"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="http://cdn.bootcss.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
<script src="/WuBuHui/js/behindbar.js?v=0.0.3"></script>
<script src="/WuBuHui/js/bottomload.js?v=0.0.3"></script>
<script>
    var PageIndex = 1;
    var PageSize = 10;
    var My = "";
    var title = "";
    var ctype = "";
    var sort = "";
    $(function () {
        LoadData(); //加载数据
        $(".catlist>li").click(function () {
            My = "";
            title = ""
            ctype = ""
            sort = "";
            PageIndex = 1;
            $("#needload>a").remove();
            ctype = $(this).attr("v");
            $(".catlist>li").removeClass("catli current").addClass("catli");
            $(this).addClass("catli current");
            Reset();
            LoadData();
        }); //分类点击

        $(".paixu>.col-xs-3>a").click(function () {
        });



    })

    //排序
    $(".paixu>div>a").click(function () {
        My = "";
        title = ""
        ctype = ""
        sort = "";
        $("#needload>a").remove();
        PageIndex = 1;
        sort = $(this).attr("v");
        $(".paixu>div>a").removeClass("wlink current").addClass("wlink");
        $(this).addClass("wlink current");
        LoadData();
    });

    //搜索
    function Search() {
        My = "";
        title = ""
        ctype = ""
        sort = "";
        PageIndex = 1;
        $("#needload>a").remove();
        title = $("#txtTitle").val();
        LoadData();
    }

    //格式化时间
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

    //截断字符
    function padLeft(str, min) {
        if (str >= min)
            return str;
        else
            return "0" + str;
    }
    //加载数据
    function LoadData() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "GetReviewInfoList", PageIndex: PageIndex, PageSize: PageSize, UserId: My, Title: title, type: ctype, Sort: sort },
            dataType: 'json',
            success: function (resp) {
                $(".loadnote").text("　");
                if (resp.Status == 0) {
                    if (resp.ExObj == null) {
                     //$('#gnmdb').find("p").text("没有数据");
                        // $('#gnmdb').modal('show');
                        $(".loadnote").text("没有更多");
                        return;
                    }
                    var html = "";
                    $.each(resp.ExObj, function (Index, Item) {
                        html += '<a href="WXDiscussInfo.aspx?AutoId=' + Item.AutoId + '" class="listbox"><div class="textbox">';
                        html += '<h3>' + Item.ReviewTitle + '</h3><p>' + Item.ReviewContent + '</p>';
                        html += '</div> <div class="smalltagbox">';
                        html += '<span class="wbtn_tag wbtn_red"><span class="iconfont icon-xin2"></span>' + Item.PraiseNum + '</span>';
                        //                        html += '<span class="wbtn_tag wbtn_greenblue"><span class="iconfont icon-cai"></span>' + Item.StepNum + '</span>'
                        if (Item.actegory != null) {
                            $.each(Item.actegory, function (Index, cItem) {
                                html += '<span class="wbtn_tag wbtn_main">' + cItem.CategoryName + '</span>';
                            });
                        }
                        html += '</div><div class="wbtn_fly wbtn_flybr wbtn_yellow timetag">';
                        html += FormatDate(Item.ReplyDateTiem) + '</div></a>';
                    });
                    $(".loadnote").before(html);
                    if (html=="") {
                        $(".loadnote").text("没有更多");
                    }
                   


                } else {
                   // $('#gnmdb').find("p").text(resp.Msg);
                    //$('#gnmdb').modal('show');
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
    var arr = [".footerbar", ".wtopbar", ".mainlist", ".paixu"];
    $("#categorybtn").controlbehindbar(arr, ".mainlist");

    function Reset() {

//        //复位动画禁点击
//        $(window).bind("click", function (e) {
//            e.preventDefault();
//        })
//        //动画完 取消禁点击
//        $(arr[0])[0].addEventListener("webkitTransitionEnd", cc)

//        function cc() {
//            $(arr[0])[0].removeEventListener("webkitTransitionEnd", cc)
//            $(".behindbar").hide();
//            setTimeout(function () {
//                $(window).unbind("click");
//            }, 500)
//        }

        //复位动画
        for (var i in arr) {
            $(arr[i]).removeClass("sdiebartranslate")
        }
        $(".sidebarhidebtn").hide();


    
    }


</script>
</html>
