<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXDiscussInfo.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WordsQuestions.WXDiscussInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <meta name="format-detection" content="telephone=no" />
    <title>
        <%=reviewInfo.ReviewTitle%>
    </title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://at.alicdn.com/t/font_1413272586_8236315.css">
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.7">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <style>
        .loadmore
        {
            text-align: center;
            width: 98%;
            height: 35px;
            border: 1px solid #ccc;
            margin-top: 5px;
            margin-left: 1%;
            line-height: 35px;
            margin-bottom: 50px;
        }
    </style>
</head>
<body class="whitebg">
    <div class="wcontainer discusscontainer maxh100">
        <div class="title">
            <%=reviewInfo.ReviewTitle%>
        </div>
        <div class="description">
            <%=reviewInfo.ReviewContent%>
        </div>
        <div class="bottombar">
            <span class="time">
                <%=reviewInfo.InsertDate.ToString("yyyy-MM-dd HH:mm")%>
            </span><span class="wbtn wbtn_tag wbtn_red"><span class="iconfont icon-eye"></span>
                <%=reviewInfo.NumCount%>
            </span><span class="dropdownicon"><span class="iconfont icon-back"></span></span>
        </div>
    </div>
    <div class="wcontainer praisebox">
        <%--        <span class="picture wbtn_round discussauthor">
            <img src="<%=TiWenUserInfo.WXHeadimgurl%>" alt="">
        </span>
        <span class="authorname"><%=TiWenUserInfo.TrueName%></span>--%>
        <span class="wbtn wbtn_red" onclick="Praise()">
            <%if (!isPraise)
              { %>
            <span id="spzan" class="iconfont icon-xin2"></span>
            <%}
              else
              { %>
            <span id="spzan" class="iconfont icon-xin"></span>
            <%} %>
            <span id="txtPraiseNum">
                <%=reviewInfo.PraiseNum%></span> </span>
    </div>
    <div class="wcontainer answerlistbox bottom50" id="needload">
        <div id="divNext" class="loadmore" onclick="NextPage()">
            正在加载...</div>
    </div>
    <div class="fixbox closethis" id="creatdiscuss">
        <form class="creatdiscuss_form" action="">
        <textarea class="secondtextarea" placeholder="回复内容" name="" id="tContent"></textarea>
        <div class="discuss_contral">
            <span class="wbtn wbtn_red discuss_submit" onclick="Reply()" id="btnSubmit">提交 </span>
            <span class="wbtn wbtn_main discuss_exit" id="discuss_exit">取消</span>
        </div>
        </form>
    </div>
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="WXDiscussList.aspx"><span class="iconfont icon-back">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            <span class="wbtn wbtn_line_main" id="jointhisdiscuss"><span class="iconfont icon-34 smallicon">
            </span>回复</span>
        </div>
        <%--        <!-- /.col-lg-10 -->
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="WXDiscussList.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>--%>
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
<script src="/WuBuHui/js/jquery.js"></script>
<%--<script src="../js/comm.js" type="text/javascript"></script>
--%><!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="/WuBuHui/js/bootstrap.js"></script>
<script src="/WuBuHui/js/quo.js?v=0.0.2"></script>
<script src="/WuBuHui/js/fixbox.js?v=0.0.4"></script>
<script src="/WuBuHui/js/discussinfo.js?v=0.0.3"></script>
<script src="/WuBuHui/js/bottomload.js?v=0.0.3"></script>
<!-- <script src="js/teachertag.js?v=0.0.3"></script> -->
<script type="text/javascript">
    var handlerUrl = "/Handler/App/WXWuBuHuiTutorHandler.ashx";
    var autoId = '<%=autoId %>';
    var isSumbit = false; //是否正在提交
    var pageIndex = 1;
    var pageSize = 10;
    $(function () {
        LoadData(); //回复列表
    });

    //回复列表
    function LoadData() {
        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { Action: "GetReplyReviewInfo", AutoID: autoId, PageIndex: pageIndex, PageSize: pageSize },
            dataType: 'json',
            success: function (resp) {
                var html = "";
                if (resp.Status == 0) {
                    $.each(resp.ExObj, function (Index, Item) {
                        html += '<div class="answerlist"><div class="authorinfo"><span class="picture wbtn_round">';
                        //html += '<img src="' + Item.Img + '" alt=""></span><span class="wbtn wbtn_tag wbtn_yellow"><span class="iconfont icon-zuanshi"></span>' + Item.UserLevel + '</div>';

                        html += '<img src="' + Item.Img + '" alt=""></span></span></div>';
                        //                        var isuser = "";
                        //                        if (Item.IsTutor == 0) {
                        //                            isuser = "<span class=\"iconfont icon-36\"></span>";
                        //                        }
                        //                        html += '<div class="answerinfo"><h3><span class="wbtn_round wbtn_green teachermark">' + isuser + '师</span>' + Item.UserName + '</h3>';

                        html += '<div class="answerinfo"><h3>' + Item.UserName + '</h3>';
                        html += '<span class="time">' + FormatDate(Item.InsertDate) + '</span>';
                        html += '<p>' + Item.ReplyContent + '</p></div>';
                        html += '<div class="bottombar">';
                        html += '<span class="dropdownicon">';
                        html += '<span class="iconfont icon-back"></span>';
                        html += '</span>';
                        html += '</div>';


                        html += '</div>';
                    });
                    $("#divNext").before(html);
                    $("#divNext").text("加载更多");
                    if (pageIndex == 1 && resp.ExObj.length == 0) {
                        $("#divNext").html("暂时没有数据");
                    }
                    if (pageIndex > 1 && resp.ExObj.length == 0) {
                        $("#divNext").html("没有更多了");
                        $("#divNext").removeAttr("onclick");
                    }
                    var answerlis = new answerlisthide()
                    answerlis.init()

                }
                else {
                    $('#gnmdb').find("p").text(resp.Msg);
                    $('#gnmdb').modal('show');
                }
            },
            complete: function () {
                $("#needload").bottomLoad(function () {
                    $("#divNext").text("正在加载...");
                    pageIndex++;
                    LoadData();
                })


            }
        });

    }

    //回复问题
    function Reply() {

        try {


            var content = $.trim($("#tContent").val());
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
                url: handlerUrl,
                data: { Action: "SaveReplyReviewInfo", AutoID: autoId, Context: content },
                dataType: 'json',
                success: function (resp) {
                    if (resp.Status == 0) {
                        //$('#gnmdb').find("p").text(resp.Msg);
                        //$('#gnmdb').modal('show');
                        window.location.reload();
                    }
                    else {
                        $('#gnmdb').find("p").text(resp.Msg);
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

    //下一页
    function NextPage() {

        pageIndex++;
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

    //赞
    function Praise() {
        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { Action: "SavePraise", AutoID: autoId },
            success: function (result) {
                var resp = $.parseJSON(result);
                if (resp.Status == 0) {

                    $("#txtPraiseNum").text(resp.ExInt)
                    if (resp.ExStr == "1") {
                        $("#spzan").attr("class", "iconfont icon-xin")
                    }
                    if (resp.ExStr == "0") {
                        $("#spzan").attr("class", "iconfont icon-xin2")
                    }
                }
                else {
                    alert(resp.Msg);
                }
            }
        });
    }
</script>
</html>
