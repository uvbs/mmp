<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXPartnerInfo.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Partner.WXPartnerInfo" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <meta name="format-detection" content="telephone=no" />
    <title></title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WubuHui/css/wubu.css?v=0.0.1">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body class="whitebg">
    <div class="wcontainer partnerheader">
        <div class="partnerlogo wbtn_round">
        
            <img src="<%=PInfo.PartnerImg%>"id="ImgLogo" alt="">
                
        </div>
        <h3 class="partnertitle">
            <%=PInfo.PartnerName %>
        </h3>
        <p class="partnerdes">
          <%=PInfo.PartnerAddress %>
        </p>
    </div>
    <div class="teacherinfobox bottom50 teacherpagetag teacherpagetagshow">
        <div class="tagbox">
            <span class="tagtitle">企业标签:</span>
            <%=PartnerStr %>
        </div>
        <div class="introduction">
            <strong>公司简介：</strong>
            <%=PInfo.PartnerContext%>
        </div>
         <div class="wcontainer praisebox">
            <span class="wbtn wbtn_red" onclick="OnPraise()">
            <%if(zan==false){ %>
            <span id="spzan" class="iconfont icon-xin2"></span>
            <%}else{ %>
            <span id="spzan" class="iconfont icon-xin"></span>
            <%} %>

                <label id="lblPraiseNum"><%=PInfo.ParTnerStep.ToString()%></label>
            </span>
            
        </div>
    </div>

    <!-- mainlist -->
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="WXPartnerList.aspx"><span class="iconfont icon-back"></span>
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
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="/WubuHui/js/bootstrap.js"></script>
<script src="/WubuHui/js/gotopageanywhere.js"></script>
<script src="/WubuHui/js/weixinsharebtn.js"></script>
<script src="/WubuHui/js/partyinfo.js"></script>
<script>

    function OnPraise() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiPartnerHandler.ashx",
            data: { Action: "AddParnerPraiseNum", id:'<%=AutoId%>' },
            dataType:'json',
            success: function (resp) {
                if (resp.Status == 1) {
                    $("#lblPraiseNum").text(resp.ExInt)
                    if (resp.ExStr == "0") {
                    $("#spzan").attr("class","iconfont icon-xin2")
                }
                if (resp.ExStr == "1") {
                    $("#spzan").attr("class", "iconfont icon-xin")
                }
                }
                else {
                    alert(resp.Msg);
                }
            }
        });
    }
</script>
<script type="text/javascript">


    var desc = "<%=PInfo.PartnerAddress%>";
    var title = '<%=PInfo.PartnerName%>';
    var imgUrl ="<%=PInfo.PartnerImg%>";
    var shareUrl = window.location.href;
    // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。
    document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
        // 发送给好友
        WeixinJSBridge.on('menu:share:appmessage', function (argv) {
            WeixinJSBridge.invoke('sendAppMessage', {
                //"appid": appId,
                "img_url": imgUrl,
                "img_width": "100",
                "img_height": "100",
                "link": shareUrl,
                "desc": desc,
                "title": title
            }, function (res) {

                if (res.err_msg == "send_app_msg:ok" || res.err_msg == "send_app_msg:confirm") {

                   
                }
                return;

            })
        });

        WeixinJSBridge.on('menu:share:timeline', function (argv) {
            WeixinJSBridge.invoke('shareTimeline', {
                //"appid": appId,
                "img_url": imgUrl,
                "img_width": "100",
                "img_height": "100",
                "link": shareUrl,
                "desc": desc,
                "title": title
            }, function (res) {
                if (res.err_msg == "share_timeline:ok") {
                  
                }
            })
        });



    }, false)







    



    </script>
</html>
