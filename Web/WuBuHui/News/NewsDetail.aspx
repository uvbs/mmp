<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.News.NewsDetail" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>
        <%=model.ActivityName%></title>
    <!-- Bootstrap -->
    <link href="../css/wubu.css" rel="stylesheet" type="text/css" />
    <link href="/App/TheVote/wap/styles/css/style.css" rel="stylesheet" type="text/css" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <script src="/Weixin/ArticleTemplate/JS/TheVote.js" type="text/javascript"></script>
    <link href="/App/Cation/Wap/Vote/styles/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/articlereview.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/StringBuilder.Min.js" type="text/javascript"></script>
    <style>
        .delreview
        {
            width: 40px;
            height: 20px;
            font-size: 12px;
            color: #666;
            border-radius: 5px;
            text-align: center;
        }
        #BtnReview
        {
            display: block;
            width: 60px;
            height: 30px;
            line-height: 30px;
            float: right;
            background-color: #58aaf0;
            font-size: 16px;
            color: #fff;
            border-radius: 5px;
            text-align: center;
            border: 1px solid #2a7bdd;
        }
        .morebtn
        {
            width: 80px;
            height: 30px;
            line-height: 30px;
            float: center;
            background-color: #58aaf0;
            font-size: 16px;
            color: #fff;
            border-radius: 5px;
            text-align: center;
            border: 1px solid #2a7bdd;
        }
        .divmore
        {
            text-align: center;
        }
    </style>
</head>
<body class="whitebg">
    <div class="wcontainer activetitle">
        <h1>
            <%=model.ActivityName%></h1>
        <div class="tagbox">
            <span class="wbtn_tag wbtn_red"><span class="iconfont icon-eye"></span>
                <%=model.PV %>
            </span>
        </div>
    </div>
    <div class="wcontainer articlebox bottom50">
        <%=model.ActivityDescription%>
        <div class="wcontainer praisebox">
            <span class="wbtn wbtn_red" onclick="OnPraise()">
                <%if (zan == false)
                  { %>
                <span id="spxin" class="iconfont icon-xin2"></span>
                <%}
                  else
                  { %>
                <span id="spxin" class="iconfont icon-xin"></span>
                <%} %>
                <asp:Label ID="txtPraiseNum" Text="0" runat="server" />
            </span>
            <%-- <span class="wbtn wbtn_greenyellow" onclick="OnStep()"><span class="iconfont icon-cai">
        </span>
            <asp:Label ID="txtStepNum" Text="0" runat="server" />
        </span>--%>
        </div>
        <%--        <div class="sharebtn">
            <span class="wbtn wbtn_main weixinsharezhidao">分享给好友</span> <span class="wbtn wbtn_main weixinsharezhidao">
                分享到朋友圈</span>
        </div>--%>
        <div style="padding: 10px; box-sizing: border-box;">
            <textarea style="resize: none; width: 100%; height: 70px;" id="txtContent" placeholder="蜜，写评论吐个槽吧。"></textarea>
            <span id="BtnReview">评论</span>
            <div class="clear">
            </div>
        </div>
        <div id="discussbox">
        </div>
        <div class="divmore">
            <input type="button" class="morebtn" value="显示更多" onclick="LoadReviewInfo()" id="btnMore" /></div>
    </div>
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:pagegoback('MarketNews.aspx')">
                <span class="iconfont icon-back"></span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">

        <span class="wbtn wbtn_line_main"  onclick="$('#txtContent').focus();"><span class="iconfont icon-b55 smallicon"></span>评论</span>

        </div>
        <!-- /.col-lg-10 -->
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
                    <p></p>
                        
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
    <div id="textdialogbox" style="padding-top: 10px; padding-left: 10px; padding: 10px;">
        <div class="textdialog">
            <textarea name="" id="txtReplyContent" cols="30" rows="10" placeholder="输入回复内容"></textarea>
            <span id="btnExit" class="buttonm">取消</span> <span class="buttonm blue" id="btnSave">
                回复</span>
        </div>
    </div>
</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="../js/jquery.js" type="text/javascript"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="../js/bootstrap.js" type="text/javascript"></script>
<script src="../js/weixinsharebtn.js" type="text/javascript"></script>
<script src="../js/partyinfo.js" type="text/javascript"></script>
<script src="../js/comm.js" type="text/javascript"></script>
<script src="/Scripts/jquery.slides.min.js" type="text/javascript"></script>
<script src="/Scripts/vote/voteanimate.js" type="text/javascript"></script>
<script type="text/javascript">


    function SaveJiFen() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
            data: { Action: "SaveShareNew", Id: "<%=model.JuActivityID%>", wxsharetype: "0" },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 1) {
                    $('#gnmdb').find("p").text(resp.Msg);
                    $('#gnmdb').modal('show');
                }
                else {
                    //$('#gnmdb').find("p").text(resp.Msg);
                    // $('#gnmdb').modal('show');
                }
                $(".weixinshareshade").hide();
            }
        });
    };

    function SaveJiFenTimeLine() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
            data: { Action: "SaveShareNew", Id: "<%=model.JuActivityID%>", wxsharetype: "1" },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 1) {

                }
                else {

                }
                $(".weixinshareshade").hide();
            }
        });
    };

    function OnPraise() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "SaveWZPraise", JId: '<%=model.JuActivityID %>' },
            success: function (result) {
                var resp = $.parseJSON(result);
                if (resp.Status == 0) {
                    $("#txtPraiseNum").text(resp.ExInt)
                    if (resp.ExStr == "0") {
                        $("#spxin").attr('class', 'iconfont icon-xin2')
                    }
                    if (resp.ExStr == "1") {
                        $("#spxin").attr('class', 'iconfont icon-xin')
                    }

                }
                else {
                    ShowMsg(resp.Msg);
                }
            }
        });
    }



</script>
<script>
    var articleReivewHandler="/handler/commHandler.ashx";
    var PageIndex = 1;
    var PageSize = 10;
    $(function () {
        LoadReviewInfo();
    })
    //加载评论列表
    function LoadReviewInfo() {
        $.ajax({
            type: "post",
            url: articleReivewHandler,
            data: { Action: "getarticlereviewlist", articleid: "<%=model.JuActivityID%>", pageindex: PageIndex, pagesize: PageSize },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                 var strHtml = new StringBuilder();
                $.each(resp.list, function (index, data) {
                    strHtml.AppendFormat('<div class="commentbox">');
                    strHtml.AppendFormat('<p class="commentcb">');
                    strHtml.AppendFormat('<span class="portrait"><img src="{0}" alt=""></span>', data.headimg);
                    strHtml.AppendFormat('<span class="commentc"><span class="peoplename">{0}</span><span class="commentcbtime">{1}</span></span>',data.nickname,formatDate(data.time));
                    strHtml.AppendFormat('</p>');
                    strHtml.AppendFormat(' <div class="commentdealbox">');
                    if (data.reply!=null) {
                        strHtml.AppendFormat('<div class="commentfirsttxt">{0}<span class="commentfirstname">{1}</span></div>', data.reply.reviewcontent, data.reply.nickname);
                    }
                    strHtml.AppendFormat('<div class="commenttxt">{0}</div>', data.reviewcontent);
                    strHtml.AppendFormat('<div class="recommentbtn" >');

                    if (data.deleteflag==true) {
                        strHtml.AppendFormat('<span class="delreview" onclick="DeleteReviewInfo(' + data.id + ')">删除</span>&nbsp;');

}

                    strHtml.AppendFormat('<span onclick="ShowReply(' + data.id + ')">回复</span>');

                     strHtml.AppendFormat('</div>');
                    strHtml.AppendFormat(' </div>');
                    strHtml.AppendFormat('<div class="recommentbox">');
                    strHtml.AppendFormat('</div>');
                    strHtml.AppendFormat('</div>');


                })
                
                $("#discussbox").append(strHtml.ToString());
                if (PageIndex>1&&(resp.list.length==0)) {
                $(btnMore).val("没有更多");
                }
                else {
                $(btnMore).val("显示更多");
}
                PageIndex++;
            }
        });
    }
    var autoId = 0;
    function ShowReply(AutoId) {
        autoId = AutoId;
        $("#textdialogbox").show();
        $("#txtReplyContent").focus();
    }
    $("#btnExit").click(function () {
        $("#textdialogbox").hide();
    });
    //回复评论
    $("#btnSave").click(function () {
        var replyContent = $("#txtReplyContent").val();
        if (replyContent == "") {
            ShowMsg("请输入回复内容");
            return;
        }
        $.ajax({
            type: "post",
            url: articleReivewHandler,
            data: { Action: "replyarticlereview", id: autoId, replycontent: replyContent,articleid:<%=model.JuActivityID%> },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                if (resp.errcode ==0) {
                    $("#txtReplyContent").val("");
                    $("#textdialogbox").hide();
                    ShowMsg("回复成功");
                    ClearReviewData();
                    LoadReviewInfo();
                } else {
                    ShowMsg(resp.errmsg);
                }
            }
        })
    });

    ///发布评论
    $("#BtnReview").click(function () {
        var Content = $("#txtContent").val()
        if (Content == "") {
            ShowMsg("蜜，写评论吐个槽吧。");
            return;
        }
        $.ajax({
            type: "Post",
            url: articleReivewHandler,
            data: { Action: "addarticlereview", articleid: "<%=model.JuActivityID%>", content: Content },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                if (resp.errcode ==0) {
                    $("#txtContent").val("");
                    ShowMsg("发表成功");
                    ClearReviewData();
                    LoadReviewInfo();
                } else {
                    ShowMsg(resp.errmsg);
                }
            }
        });
    });

    //删除评论
    function DeleteReviewInfo(reviewid) {
        $.ajax({
            type: "Post",
            url: articleReivewHandler,
            data: { Action: "deletearticlereview", id: reviewid },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
               if (resp.errcode ==0) {
                 ClearReviewData();
                    LoadReviewInfo();
                } else {
                    ShowMsg(resp.errmsg);
                }
            }
        });


    }

    //清除评论数据
    function ClearReviewData(){
     PageIndex=1;
     $("#discussbox").html('');
    
    }


function padLeft(str, min) {
    if (str >= min)
        return str;
    else
        return "0" + str;
}

function formatDate(value) {

    if (value == null || value == "") {
        return "";
    }
    var date = new Date(value);
    var month = padLeft(date.getMonth() + 1, 10);
    var currentDate = padLeft(date.getDate(), 10);
    var hour = padLeft(date.getHours(), 10);
    var minute = padLeft(date.getMinutes(), 10);
    var second = padLeft(date.getSeconds(), 10);
    return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minute;
} 

function ShowMsg(msg){
$('#gnmdb').find("p").text(msg);
$('#gnmdb').modal('show');
}

</script>
<script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
<script>
    var lineLink = "http://<%=Request.Url.Host%>/WuBuHui/News/NewsDetail.aspx?id=<%=model.JuActivityID %>";
    var descContent = "<%= model.Summary%>";
    var shareTitle = '<%=model.ActivityName%>';
    var imgUrl = "http://" + window.location.host + "<%=model.ThumbnailsPath%>";
    var wxconfig = $.parseJSON('<%=new ZentCloud.BLLJIMP.BLLWeixin("").GetJSAPIConfig("")%>');
    wx.config({
        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端ShowMsg出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        appId: wxconfig.appId, // 必填，公众号的唯一标识
        timestamp: wxconfig.timestamp, // 必填，生成签名的时间戳
        nonceStr: wxconfig.nonceStr, // 必填，生成签名的随机串
        signature: wxconfig.signature, // 必填，签名，见附录1
        jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
    });
    wx.ready(function () {

        // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。
        wx.onMenuShareTimeline({
            title: shareTitle, // 分享标题
            link: lineLink, // 分享链接
            imgUrl: imgUrl, // 分享图标
            success: function () {
                // 用户确认分享后执行的回调函数
                SaveJiFenTimeLine();
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            }
        });
        wx.onMenuShareAppMessage({
            title: shareTitle, // 分享标题
            desc: descContent, // 分享描述
            link: lineLink, // 分享链接
            imgUrl: imgUrl, // 分享图标
            type: '', // 分享类型,music、video或link，不填默认为link
            dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
            success: function () {
                // 用户确认分享后执行的回调函数
                SaveJiFen();
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            }
        });


    });

    wx.error(function (res) {
        // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
        //ShowMsg(res.errMsg);
    });

    //
</script>
</html>
