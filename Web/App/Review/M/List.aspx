<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Review.M.List" %>

<!DOCTYPE html >
<html lang="zh-cn">
<head>
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
    <style type="text/css">
        .col-xs-8
        {
            width: 100%;
        }
        .creatdiscuss_form input
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <div class="behindbar">
        <div class="title wbtn_line_yellow">
            <span class="iconfont icon-34"></span>分类
        </div>
        <ul class="catlist">
            <li class="catli current"><a>全部</a></li>
            <%=CategoryHtml%>
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
            <input type="text" class="searchtext" id="txtKeyWord">
        </div>
        <!-- /.col-lg-6 -->
    </div>
    <!-- /.container -->
    <div class="paixu">
        <div class="col-xs-4">
            <a href="#" class="wlink current" sort="Newhf">最新回复</a>
        </div>
        <div class="col-xs-4">
            <a href="#" class="wlink" sort="Mosthf">最多回复</a>
        </div>
        <div class="col-xs-4">
            <a href="#" class="wlink" sort="Mosthp">最高人气</a>
        </div>
    </div>
    <div class="mainlist top86 bottom50" id="needload">
        <!-- listbox -->
        <p class="loadnote" style="text-align: center;">
        </p>
    </div>
    <div class="fixbox closethis" id="creatdiscuss">
        <form class="creatdiscuss_form" action="">
        <input type="text" id="txtTitle" placeholder="标题" />
        <br />
        <textarea class="secondtextarea" placeholder="话题内容" name="" id="txtContent"></textarea>
        <div class="discuss_contral">
            <span class="wbtn wbtn_red discuss_submit" onclick="Add()" id="btnSubmit">提交 </span>
            <span class="wbtn wbtn_main discuss_exit" id="discuss_exit">取消</span>
        </div>
        </form>
    </div>
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
    <%if (currentWebsiteInfo.IsEnableUserReleaseReview == 1)
      {%>
    <div class="footerbar">
        <div class="col-xs-8">
            <a class="wbtn wbtn_line_main" type="button" id="jointhisdiscuss"><span class="iconfont icon-34 smallicon">
            </span>发布话题 </a>
        </div>
    </div>
    <%} %>
    <!-- footerbar -->
</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="http://cdn.bootcss.com/jquery/1.11.1/jquery.min.js"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="http://cdn.bootcss.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
<script src="/WuBuHui/js/behindbar.js?v=0.0.3"></script>
<script src="/WuBuHui/js/bottomload.js?v=0.0.3"></script>
<script src="/WuBuHui/js/quo.js?v=0.0.2"></script>
<script src="/WuBuHui/js/fixbox.js?v=0.0.4"></script>
<script src="/WuBuHui/js/discussinfo.js?v=0.0.3"></script>
<script>
    var pageIndex = 1;
    var pageSize = 10;
    var title = "";
    var ctype = "";
    var sort = "";
    var isSumbit = false;
    $(function () {
        LoadData(); //加载数据
        $(".catlist>li").click(function () {
            title = ""
            ctype = ""
            sort = "";
            pageIndex = 1;
            $("#needload>a").remove();
            ctype = $(this).attr("v");
            $(".catlist>li").removeClass("catli current").addClass("catli");
            $(this).addClass("catli current");
            Reset();
            LoadData();
        });


    })

    //排序
    $(".paixu>div>a").click(function () {
        title = ""
        ctype = ""
        sort = "";
        $("#needload>a").remove();
        pageIndex = 1;
        sort = $(this).attr("sort");
        $(".paixu>div>a").removeClass("wlink current").addClass("wlink");
        $(this).addClass("wlink current");
        LoadData();
    });

    //搜索
    function Search() {
        title = ""
        ctype = ""
        sort = "";
        pageIndex = 1;
        $("#needload>a").remove();
        title = $("#txtKeyWord").val();
        LoadData();
    }

    //格式化时间
    function FormatDate(value) {
        if (value == null || value == "") {
            return "";
        }
        var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
        var month = PadLeft(date.getMonth() + 1, 10);
        var currentDate = PadLeft(date.getDate(), 10);
        var hour = PadLeft(date.getHours(), 10);
        var minute = PadLeft(date.getMinutes(), 10);
        var second = PadLeft(date.getSeconds(), 10);
        return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minute;
    }

    //截断字符
    function PadLeft(str, min) {
        if (str >= min)
            return str;
        else
            return "0" + str;
    }
    //加载数据
    function LoadData() {
        $.ajax({
            type: 'post',
            url: "Handler/Review/List.ashx",
            data: { pageIndex: pageIndex, pageSize: pageSize, title: title, type: ctype, sort: sort },
            dataType: 'json',
            success: function (resp) {
                $(".loadnote").text("　");
                if (resp.result == null || resp.result.length == 0) {
                    $(".loadnote").text("没有更多");
                    return;
                }
                var html = "";
                $.each(resp.result, function (Index, Item) {
                    html += '<a href="Detail.aspx?AutoId=' + Item.AutoId + '" class="listbox"><div class="textbox">';
                    html += '<h3>' + Item.ReviewTitle + '</h3><p>' + Item.ReviewContent + '</p>';
                    html += '</div> <div class="smalltagbox">';
                    html += '<span class="wbtn_tag wbtn_red"><span class="iconfont icon-xin2"></span>' + Item.PraiseNum + '</span>';

                    if (Item.actegory != null) {
                        $.each(Item.actegory, function (Index, cItem) {
                            html += '<span class="wbtn_tag wbtn_main">' + cItem.CategoryName + '</span>';
                        });
                    }
                    html += '</div><div class="wbtn_fly wbtn_flybr wbtn_yellow timetag">';
                    html += FormatDate(Item.ReplyDateTiem) + '</div></a>';
                });
                $(".loadnote").before(html);
                if (html == "") {
                    $(".loadnote").text("没有更多");
                }

            },
            complete: function () {
                $("#needload").bottomLoad(function () {
                    $(".loadnote").text("正在加载...");
                    pageIndex++;
                    LoadData();
                })


            }
        });
    };
    var arr = [".footerbar", ".wtopbar", ".mainlist", ".paixu"];
    $("#categorybtn").controlbehindbar(arr, ".mainlist");
    //重设动画
    function Reset() {

        //复位动画
        for (var i in arr) {
            $(arr[i]).removeClass("sdiebartranslate")
        }
        $(".sidebarhidebtn").hide();

    }

    //发布话题
    function Add() {

        try {
            var title = $.trim($("#txtTitle").val());
            var content = $.trim($("#txtContent").val());
            if (title == "") {
                $('#gnmdb').find("p").text("请输入标题");
                $('#gnmdb').modal('show');
                return false;
            }
            if (content == "") {
                $('#gnmdb').find("p").text("请输入内容");
                $('#gnmdb').modal('show');
                return false;
            }
            if (isSumbit) {
                return false;
            }
            isSumbit = true;
            $("#btnSubmit").text("正在提交...");
            $.ajax({
                type: 'post',
                url: "Handler/Review/Add.ashx",
                data: { Title: title, Context: content },
                dataType: 'json',
                success: function (resp) {
                    if (resp.status) {
                        window.location.reload();
                    }
                    else {
                        $('#gnmdb').find("p").text(resp.msg);
                        $('#gnmdb').modal('show');
                    }
                },
                complete: function () {
                    isSumbit = false;
                    $("#btnSubmit").text("提交");

                }
            });


        } catch (e) {
            alert(e);
        }


    }
</script>
</html>
