<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TutorList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Tutor.TutorList" %>

<!DOCTYPE html >
<html lang="zh-cn">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>五步会导师列表</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.5">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <style>
    .mainlist .listbox .textbox p{max-height:100px;}
    </style>
</head>
<body>
    <div class="behindbar">
        <div class="title wbtn_line_green">
            <span class="iconfont icon-36"></span>分类
        </div>
        <ul class="catlist" id="catlist">
            <li class="catli current"><a href="">全部</a></li>
        </ul>
    </div>
    <div class="wtopbar">
<%--        <div class="col-xs-2">
            <span class="wbtn  wbtn_line_green" id="categorybtn"><span class="iconfont icon-36 bigicon">
            </span></span>
        </div>--%>
        <div class="col-xs-12">
            <span class="wbtn wbtn_main"><span class="iconfont icon-111" onclick="searchName()">
            </span></span>
            <input type="text" class="searchtext" id="txtName" placeholder="搜索导师姓名/企业/城市">
        </div>
        <!-- /.col-lg-6 -->
    </div>
    <!-- /.container -->
    <div class="paixu">
        <div class="col-xs-3">
            <a href="#" class="wlink current" v="rdateTime">最新回复</a>
        </div>
        <div class="col-xs-3">
            <a href="#" class="wlink" v="rMany">最高人气</a>
        </div>
        <div class="col-xs-3">
            <a href="#" class="wlink" v="wzMany">最多文章</a>
        </div>
        <div class="col-xs-3" id="filterbtn">
            <span class="wlink">筛选<span class="iconfont icon-back shaxuan"></span></span>
        </div>
        <div class="hidefilterbox filterbox"  id="filterbox">
            <h3>
                专业</h3>
            <%=TutorStr %>
            <div class="clearfix">
            </div>
            <h3>
                行业</h3>
            <%=ProfessionalStr %>
            <div class="clearfix">
            </div>
            <span class="wbtn wbtn_main" onclick="OnClear()">清除筛选</span> <span class="wbtn wbtn_main"
                onclick="OnType()">确&nbsp;&nbsp;&nbsp;&nbsp;定</span>
        </div>
    </div>
    <div class="mainlist top86 bottom50" id="needload">
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
<%--<script src="/WuBuHui/js/behindbar.js?v=0.0.3"></script>
--%>
<script src="/WuBuHui/js/bottomload.js?v=0.0.3"></script>
<script src="/WuBuHui/js/filterbox.js?v=0.0.4"></script>
<script type="text/jscript">
    var PageIndex = 1;
    var PageSize =10;
    var type = "";
    var name = "";
    var px = "rdateTime";
    var ProfessionalIds = "";
    var TradeIds = "";
    $(function () {
        GetTutors();
    });

    function OnClear() {
        $("#filterbox>input").attr("checked", false)
    }

    function OnType() {

        type = "";
        name = "";
        px = "";
        ProfessionalIds = "";
        TradeIds = "";
        //$("#needload>a").remove();
        PageIndex = 1;
//        $("#filterbox>input:checked").each(function () {
//            type += $(this).attr("v") + ",";
//        });
//        GetTutors();
//        $(".filterbox").addClass("hidefilterbox");
        //
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
        GetTutors();

    }

    $(".paixu>div>a").click(function () {
        type = "";
        name = "";
        px = "";
        ProfessionalIds = "";
        TradeIds = "";
        $("#needload>a").remove();
        PageIndex = 1;
        px = $(this).attr("v");
        $(".paixu>div>a").removeClass("wlink current").addClass("wlink");
        $(this).addClass("wlink current");
        GetTutors();
    });
    $("#catlist>li").click(function () {
        type = "";
        name = "";
        px = "";
        ProfessionalIds = "";
        TradeIds = "";
        PageIndex = 1;
        $("#needload>a").remove();
        type = $(this).attr("v");

        GetTutors();
        $(".catlist>li").removeClass("catli current").addClass("catli");
        $(this).addClass("catli current");
    });

    function searchName() {
        type = "";
        name = "";
        px = "";
        ProfessionalIds = "";
        TradeIds = "";
        PageIndex = 1;
        name = $("#txtName").val();
        $("#needload>a").remove();
        GetTutors();
    }



    function GetTutors() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
            data: { Action: "TutorInfos", PageIndex: PageIndex, PageSize: PageSize, type: type, Title: name, Sort: px, Tradeids: TradeIds, ProfessionalIds: ProfessionalIds },
            dataType: 'json',
            success: function (resp) {
                $(".loadnote").text("　");
                if (resp.Status == 0) {
                    if (resp.ExObj == null) {
                        // $('#gnmdb').find("p").text("没有数据");
                        //$('#gnmdb').modal('show');

                        return;
                    }
                    var html = "";
                    $.each(resp.ExObj, function (Index, Item) {
                        html += ' <a href="WxTutorInfo.aspx?UserId=' + Item.AutoId + '"><div class="listbox"><div class="touxiang wbtn_round">';
                        html += '<img src="' + Item.TutorImg + '" alt="">';
                        html += '</div><div class="textbox"><h3>' + Item.TutorName + '&nbsp;&nbsp;—' + Item.Position + '</h3><p>' + Item.Company + '<br/>' + Item.Signature + '</p></div>';
                        html += '<div class="tagbox"><span class="wbtn_tag wbtn_yellow"><span class="iconfont icon-34"></span>' + Item.HTNums + '</span>';
                        html += '<span class="wbtn_tag wbtn_greenyellow"><span class="iconfont icon-78"></span>' + Item.WZNums + '</span>';
                        html += '<span class="wbtn_tag wbtn_red"><span class="iconfont icon-xin2"></span>' + Item.FlowerCount + '</span>';


                        if (Item.City != "") {
                            html += '<span class="wbtn_tag wbtn_greenyellow"><span class="iconfont icon-82"></span>' + Item.City + '</span>';
                        }
                        html += '</div>';

                        html += '<div class="tagbox">';
                        if (Item.actegory != null) {
                            $.each(Item.actegory, function (Index, cItem) {
                                html += '<span class="wbtn_tag wbtn_main">' + cItem.CategoryName + '</span>';
                            });
                        }

                        html += '</div> <div class="wbtn_fly wbtn_flybr wbtn_green consult">立即咨询 </div></div></a> ';
                    });
                    $(".loadnote").before(html);
                    if (html == "") {
                        $(".loadnote").text("没有更多");
                    }

                }
                else {
                    $('#gnmdb').find("p").text(resp.Msg);
                    $('#gnmdb').modal('show');
                }
            },
            complete: function () {
                $("#needload").bottomLoad(function () {
                    $(".loadnote").text("正在加载...");
                    PageIndex++;
                    GetTutors();
                })
            }
        });
    }
//    var arr = [".footerbar", ".wtopbar", ".mainlist", ".paixu"];
//    $("#categorybtn").controlbehindbar(arr, ".mainlist");

</script>

</html>
