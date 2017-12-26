


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXPositionList.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Position.WXPositionList" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>职位列表</title>
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
        <div class="title wbtn_line_lightblue">
            <span class="iconfont icon-39"></span>职位
        </div>
        <ul class="catlist">

        </ul>
    </div>
    <div class="wtopbar">
<%--        <div class="col-xs-2">
            <span class="wbtn  wbtn_line_lightblue" id="categorybtn"><span class="iconfont icon-39 bigicon">
            </span></span>
        </div>--%>
        <div class="col-xs-12">
            <span class="wbtn wbtn_main"><span class="iconfont icon-111" onclick="OnSearch()"></span>
            </span>
            <input type="text" class="searchtext" id="txtTitle">
        </div>
        <!-- /.col-lg-6 -->
    </div>
    <!-- /.container -->
    <div class="paixu">
        <div class="col-xs-4">
            <a href="#" class="wlink" onclick="SortType('time')">最新发布</a>
        </div>
        <div class="col-xs-4">
            <a href="#" class="wlink" onclick="SortType('ppv')">最多浏览</a>
        </div>
        <div class="col-xs-4" id="filterbtn">
            <span class="wlink">筛选<span class="iconfont icon-back shaxuan"></span></span>
        </div>
        <div class="filterbox hidefilterbox" id="filterbox">
        <h3>行业</h3>
            <%=sbTrade.ToString() %>
            <div class="clearfix">
            </div>
            <h3>专业</h3>
            <%=sbProfessional.ToString() %>
            <div class="clearfix">
            </div>
            <span class="wbtn wbtn_main" onclick="OnClear()">清除筛选</span> <span class="wbtn wbtn_main"
                onclick="OnType()">确&nbsp;&nbsp;&nbsp;&nbsp;定</span>
        </div>
    </div>

    <div class="mainlist top86 bottom50" id="needload">
        <!-- style="background-color:#000;http://www.baidu.com" -->
        <!-- listbox -->
        <p class="loadnote" style="text-align: center;">　</p>
    </div>
    <!-- mainlist -->
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-back">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            <a href="../Position/MyWXPositionList.aspx" class="wbtn wbtn_line_main" id="askthisteacher">
                <span class="iconfont icon-39 smallicon"></span>我的职位</a>
        </div>
        <!-- /.col-lg-10 -->
      <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>
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
</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="../js/jquery.js" type="text/javascript"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="../js/bootstrap.js" type="text/javascript"></script>
<script src="/WuBuHui/js/bottomload.js?v=0.0.3"></script>
<script src="/WuBuHui/js/filterbox.js?v=0.0.4"></script>
<script>
    var PageIndex = 1;
    var PageSize =10;
    var My = "";
    var title = "";
    var ProfessionalIds = "";
    var TradeIds = "";
    var Sort = "time";
    $(function () {
        InitData();
        $(".catlist>li").click(function () {
            PageIndex = 1;
            $("#needload>a").remove();
            InitData();
            $(".catlist>li").removeClass("catli current").addClass("catli");
            $(this).addClass("catli current");
        });
    })

    function OnSearch() {
        My = "";
        title = "";
        ProfessionalIds = "";
        TradeIds = "";
        Sort = "time";
        PageIndex = 1;
        $("#needload>a").remove();
        title = $("#txtTitle").val();
        InitData();

    }
    function OnType() {
        try {
            My = "";
            title = "";
            ProfessionalIds = "";
            TradeIds = "";
            Sort = "time";
            $("#needload>a").remove();
            PageIndex = 1;
            var Professional = [];
            var Trade = [];
            $("input[name='cbtrade']:checked").each(function () {
                Trade.push($(this).val());
            });
            $("input[name='cbprofessionals']:checked").each(function () {
                Professional.push($(this).val());
            });
            TradeIds = Trade.join(',');
            ProfessionalIds = Professional.join(',');
            $(".filterbox").addClass("hidefilterbox");
            InitData();
        } catch (e) {
        alert(e);
        }

}

function SortType(sort) {
    My = "";
    title = "";
    ProfessionalIds = "";
    TradeIds = "";
    Sort = "time";
    $("#txtTitle").val("");
    $("#needload>a").remove();
    PageIndex = 1;
    Sort = sort;
    InitData();


}

    $(".paixu>div>a").click(function () {
//        $("#needload>a").remove();
//        PageIndex = 1;
        $(".paixu>div>a").removeClass("wlink current").addClass("wlink");
        $(this).addClass("wlink current");
       // InitData();
    });

    function OnClear() {
        $("#filterbox>input").attr("checked", false)
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
        return date.getFullYear() + "-" + month + "-" + currentDate ;
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
            url: "/Handler/App/WXWuBuHuiPosintionHandler.ashx",
            data: { Action: "GetPositiInfoList", PageIndex: PageIndex, PageSize: PageSize, Title: title, My: "", Sort: Sort, TradeIds: TradeIds, ProfessionalIds: ProfessionalIds },
            dataType:'json',
            success: function (resp) {
                $(".loadnote").text("　");
                if (resp.Status == 0) {
                    data = resp.ExObj;
                    if (data.length < 1) {
                        //$('#gnmdb').find("p").text("没有数据");
                        //$('#gnmdb').modal('show');
                        //$(".loadnote").text("　");
                        $(".loadnote").text("没有更多");
                        return;
                    };
                    var html = "";
                    $.each(data, function (Index, Item) {
                        html += '<a href="WXPositionInfo.aspx?Id=' + Item.AutoId + '" class="listbox"><div class="wbtn_fly companylogo">';
                        html += '<img src="' + Item.IocnImg + '" alt=""></div>';
                        html += ' <div class="textbox"><h3 class="positiontitle">' + Item.Title + '</h3><p class="jobtitlebox"><span class="jobtitle">' + Item.City + '</span><span class="jobtitle">' + Item.Education + '</span><span class="jobtitle">' + Item.WorkYear + '</span><span class="company">' + Item.Company + '</span></p></div>';
                        //html += '<div class="salarybox">' + Item.SalaryRange + '</div>';
                        html += '<div class="wbtn_fly wbtn_flybr wbtn_main timetag">' + FormatDate(Item.InsertDate) + '</div>';
                        html += '<div class="tagbox">';
                        html += '<span class="wbtn_tag wbtn_red"><span class="iconfont icon-eye"></span>' + Item.Pv + '</span>';
                        if (Item.Ctype != null) {
                            $.each(Item.Ctype, function (i, it) {
                                html += '<span class="wbtn_tag wbtn_main">' + it.CategoryName + '</span>';
                            });
                        }

                        html += '</div></a>'
                    });


                    $(".loadnote").before(html);
                    if (html=="") {
                        $(".loadnote").text("没有更多");
                    }


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
    };

</script>
</html>
