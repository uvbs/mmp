<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyCenter.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.MyCenter.MyCenter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>个人中心</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.9">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
    <script>        var IsHaveUnReadMessage = "<%=IsHaveUnReadMessage%>"; </script>
</head>
<body>
   
    <body>
        <a class="wcontainer mycenterheader" onclick="flush()">
        <span class="name">
            <%=uinfo.TrueName%>
        </span>
        <span class="name">
            <%for (int i = 1; i <= UserLevel; i++)
              {%>
            <span class="iconfont icon-zuanshi"></span>
            <% } %>
        </span>
        <span class="infoicon wbtn_line_main">
        <span class="iconfont icon-b16"></span>
        </span>
        <span class="touxiang wbtn_round">
            <img src="<%=uinfo.WXHeadimgurlLocal%>" id="imgHead">
        </span> 
       </a>
            <span class="fansandfollow">
                <%if (IsTutor)
                  { %>
                <a class="col-xs-6 fans" href="../Tutor/FansList.aspx?userid=<%=uinfo.UserID%>&fromorto=to">
                    粉丝<%=FlowerCount%></a>
                <a class="col-xs-6 follow" href="../Tutor/FansList.aspx?userid=<%=uinfo.UserID%>&fromorto=from">
                    已关注<%=AttentionCount%>位导师</a>
                <%}else {%> 
                
                <a class="
                col-xs-12 follow" href="../Tutor/FansList.aspx?userid=<%=uinfo.UserID%>&fromorto=from">
                    已关注<%=AttentionCount%>位导师</a>
                <%}%>
 </span>
        <div class="wcontainer hupenghuanyou">
            <a href="/WuBuHui/Member/Recommend.aspx" class="wbtn wbtn_red"><span class="iconfont icon-b55">
            </span><span class="text">呼朋唤友</span> <span class="number wbtn_yellow">
                <asp:Literal Text="0" ID="txtCount" runat="server" /></span> </a>
        </div>
        <div class="mycenterlist bottom50 ">
            <div class="listgroup">
                <a href="../Score/Score.aspx" class="listbox"><span class="listicon wbtn_line_red"><span
                    class="iconfont icon-53"></span></span><span class="title">我的积分 </span><span class="righticon">
                        <span class="titleinfo">
                            <%=uinfo.TotalScore.ToString()%>
                        </span><span class="goicon wbtn_line_main"><span class="iconfont icon-back"></span>
                        </span></span></a>
                        <a href="/App/Cation/Wap/Mall/MyScoreOrderList.aspx" class="listbox">
                            <span class="listicon wbtn_line_red"><span class="iconfont icon-18"></span></span>
                            <span class="title">积分订单 </span><span class="righticon"><span class="titleinfo"></span>
                                <span class="goicon wbtn_line_main"><span class="iconfont icon-back"></span></span>
                            </span></a>
                            <a href="../Member/ScoreTop.aspx" class="listbox"><span class="listicon wbtn_line_red">
                                <span class="iconfont icon-411"></span></span><span class="title">排行榜 </span><span
                                    class="righticon"><span class="goicon wbtn_line_main"><span class="iconfont icon-back">
                                    </span></span></span></a>
            </div>
            <div class="listgroup">
                <a href="/WuBuHui/Position/MyWXPositionList.aspx" class="listbox"><span class="listicon wbtn_line_yellow">
                    <span class="iconfont icon-39"></span></span><span class="title">职位申请 </span><span
                        class="righticon"><span class="goicon wbtn_line_main"><span class="iconfont icon-back">
                        </span></span></span></a>
                        <a href="/WuBuHui/WordsQuestions/MyWXDiscussList.aspx" class="listbox">
                            <span class="listicon wbtn_line_yellow"><span class="iconfont icon-34"></span></span>
                            <span class="title">话题参与 </span><span class="righticon">
                                <%if (IsShowRed)
                                  {%>
                                <span class="titleinfo wbtn_round wbtn_red"></span>
                                <%} %>
                                <span class="goicon wbtn_line_main"><span class="iconfont icon-back"></span></span>
                            </span></a>
                            
                            <a href="/WuBuHui/Activity/MyActivityInfo.aspx" class="listbox"><span
                                class="listicon wbtn_line_yellow"><span class="iconfont icon-12"></span></span><span
                                    class="title">活动报名 </span><span class="righticon"><span class="goicon wbtn_line_main">
                                        <span class="iconfont icon-back"></span></span></span></a>


<%--                            <a href="/WuBuHui/News/NewsDetail.aspx?id=267108" class="listbox">
                            <span class="listicon wbtn_line_greenyellow">
                <span class="iconfont icon-56"></span></span>
                                <span
                                    class="title">趣事投稿 </span><span class="righticon"><span class="goicon wbtn_line_main">
                                        <span class="iconfont icon-back"></span></span></span></a>--%>




            </div>
            <span id="synecticsbtn" class="listbox"><span class="listicon wbtn_line_greenyellow">
                <span class="iconfont icon-56"></span></span><span class="title">集思广益 </span><span
                    class="righticon"><span class="goicon wbtn_line_main"><span class="iconfont icon-back">
                    </span></span></span></span>
        </div>
        <!-- mainlist -->
        <script type="text/javascript" src="../js/footer.js"></script>
        <!-- footerbar -->
        <div class="fixbox closethis">
            <form class="creatdiscuss_form" action="">
            <textarea class="secondtextarea" placeholder="欢迎提交您的建议，您的想法" name="TxtContext" id="TxtContext"></textarea>
            <div class="discuss_tagbox">
                <div class="discuss_inbox">
                    <input type="radio" checked="checked" class="checkbox" name="RadType" value="我的意见"
                        id="radyj" />
                    <label for="radyj" class="discusstag">
                        <span class="wbtn wbtn_gary"><span class="iconfont"></span></span>我的意见</label>
                    <input type="radio" class="checkbox" name="RadType" value="我的投诉" id="radts" />
                    <label for="radts" class="discusstag">
                        <span class="wbtn wbtn_gary"><span class="iconfont"></span></span>我的投诉</label>
                    <input type="radio" class="checkbox" name="RadType" value="我的创意" id="radcy" />
                    <label for="radcy" class="discusstag">
                        <span class="wbtn wbtn_gary"><span class="iconfont"></span></span>我的创意</label>
                </div>
            </div>
            <div class="discuss_contral">
                <span class="wbtn wbtn_red discuss_submit" onclick="OnSave()" id="btnSubmit">提交
                </span><span class="wbtn wbtn_main discuss_exit" id="discuss_exit" onclick="javascript:window.location.href='MyCenter.aspx'">
                    取消 </span>
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
        <!-- /.modal -->
    </body>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="../js/jquery.js" type="text/javascript"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="../js/bootstrap.js" type="text/javascript"></script>
    <script src="../js/fixbox.js" type="text/javascript"></script>
    <script src="../js/mycenter.js" type="text/javascript"></script>
    <script type="text/javascript">
        var IsSumbit = false;
        function OnSave() {
            var context = $("#TxtContext").val();
            if (context == "") {
                $('#gnmdb').find("p").text("请输入内容");
                $('#gnmdb').modal('show');
                return;
            }
            var Otype = $("input[name='RadType']:checked").val();
            if (IsSumbit) {

                return;
            }
            IsSumbit = true;
            $("#btnSubmit").text("正在提交...");
            $.ajax({
                type: 'post',
                url: "/Handler/App/WXWuBuHuiPartnerHandler.ashx",
                data: { Action: "AUOpinionInfo", OContext: context, Otype: Otype },
                dataType: 'json',
                success: function (resp) {
                    if (resp.Status == 0) {
                        $('#gnmdb').find("p").text(resp.Msg);
                        $('#gnmdb').modal('show');
                        setTimeout("window.location.href = \"/WuBuHui/MyCenter/MyCenter.aspx\";", 2000);
                    }
                    else {
                        $('#gnmdb').find("p").text(resp.Msg);
                        $('#gnmdb').modal('show');
                    }
                },
                complete: function () {
                    IsSumbit = false;
                    $("#btnSubmit").text("提交");
                }
            });

        }



        function flush() {
            $.ajax({
                type: 'post',
                url: '/Handler/OpenGuestHandler.ashx',
                data: { Action: 'UpdateToLogoutSessionIsRedoOath' },
                success: function (result) {
                    window.location.href = "../Member/UMember.aspx";
                }
            });
        
        
        }
    </script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script>
        var imgUrl = "http://" + window.location.host + "/WuBuHui/img/ccc.jpg";  //注意必须是绝对路径
        var lineLink = window.location.href;   //同样，必须是绝对路径
        var descContent = "五步会是一个领域专注的人才社交平台，拥有中国最大的线上专业人才交流互动微信，为专业领域度身定制。"; //分享给朋友或朋友圈时的文字简介
        var shareTitle = "五步会首页";  //分享title
        var wxconfig = $.parseJSON('<%=new ZentCloud.BLLJIMP.BLLWeixin("").GetJSAPIConfig("")%>');
        wx.config({
            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: wxconfig.appId, // 必填，公众号的唯一标识
            timestamp: wxconfig.timestamp, // 必填，生成签名的时间戳
            nonceStr: wxconfig.nonceStr, // 必填，生成签名的随机串
            signature: wxconfig.signature, // 必填，签名，见附录1
            jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        });
        wx.ready(function () {

            // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。
            wx.showOptionMenu();
            wx.onMenuShareTimeline({
                title: shareTitle, // 分享标题
                link: lineLink, // 分享链接
                imgUrl: imgUrl, // 分享图标
                success: function () {
                    // 用户确认分享后执行的回调函数
                    //alert("分享到朋友圈成功");
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
                    //alert("发送给朋友成功");
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数
                }
            });


        });

        wx.error(function (res) {
            // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。
            //alert(res.errMsg);
        });

        //
    </script>
</html>
