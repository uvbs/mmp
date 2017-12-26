<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Activity.ActivityInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title><%=model.ActivityName %></title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.1">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body class="whitebg">
    <div class="wcontainer activetitle">
        <h1>
            <asp:Literal Text="text" ID="txtTitle" runat="server" /></h1>
        <div class="tagbox">
            <span class="wbtn_tag wbtn_red"><span class="iconfont icon-eye"></span><%=model.PV.ToString()%></span>
            <span class="wbtn_tag wbtn_orange"><span class="iconfont icon-36"></span><%=model.SignUpTotalCount.ToString()%> </span>



        </div>
    </div>
    <div class="mainlist activelist activeinfomainlist">
        <!-- style="background-color:#000;http://www.baidu.com" -->
        <div class="<%=classStr %>">
            <div class="mainimg">
                <img src="<%=model.ThumbnailsPath%>"> 
            </div>
            <span class="wbtn_fly wbtn_flytr wbtn_greenyellow">费用：
            <asp:Literal Text="0" ID="txtActivityIntegral"
                runat="server" />
                积分
              <%if (model.MaxSignUpTotalCount>0)
              {%>
                 余位:<%=model.MaxSignUpTotalCount-model.SignUpTotalCount%>
              <%}%>


                 </span>
                 <span class="baomingstatus"><span class="text">
                    <asp:Literal Text="text" ID="txtStart" runat="server" /></span>
                    <svg class="sanjiao" version="1.1" viewbox="0 0 100 100">
				<polygon points="100,100 0.2,100 100,0.2" />
			</svg>
                </span>
            <div class="activeconcent">
                <div class="textbox">
                    <p>
                        <span class="iconfont icon-clock"></span><span class="text">时间:<asp:Literal Text="text"
                            ID="txtTime" runat="server" /></span></p>
                    <p>
                        <span class="iconfont icon-adress"></span><span class="text">地址:<asp:Literal Text="text"
                            ID="txtAddress" runat="server" /></span></p>
                </div>
                <div class="tagbox">
                    <span class="wbtn_tag wbtn_main">
                        <asp:Literal  ID="txtType" runat="server" /></span>
                </div>
            </div>
            <div class="wcontainer applyactive">
                <div class="applyactiveinbox">
                    <form action="" id="formsignin">
                    <div class="input-group">
                        <span class="input-group-addon"><span class="iconfont icon-b24"></span></span>
                        <input type="text" class="form-control" id="txtName" name="Name" placeholder="姓名" value="<%=UserInfo.TrueName%>">
                    </div>
                    <div class="input-group">
                        <span class="input-group-addon"><span class="iconfont icon-b47"></span></span>
                        <input type="number" class="form-control" id="txtPhone" name="Phone" placeholder="电话" value="<%=UserInfo.Phone%>">
                    </div>
                    </form>
                    <div class="input-group activeinfosubmitbtn">
                        <span class="wbtn wbtn_main"><span class="text" onclick="InsertData()">确认报名</span>
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <!-- listbox -->
    </div>

    <!-- mainlist -->
    <div class="wcontainer articlebox">
        <asp:Literal Text="text" ID="txtContent" runat="server" />
    </div>
            <div class="wcontainer articlebox bottom50">

<%--        <div class="sharebtn">
            <span class="wbtn wbtn_main weixinsharezhidao">分享给好友</span> <span class="wbtn wbtn_main weixinsharezhidao">
                分享到朋友圈</span>
        </div>--%>
    </div>
    <div class="wcontainer bottom50 applypeoplelist">
        <div class="listtitle">
            <h3 class="wbtn_line_orange">
                参报人员</h3>
            <span class="wbtn_tag wbtn_orange"><span class="iconfont icon-36"></span>
                <asp:Literal Text="text" ID="txtNum" runat="server" />
            </span>
        </div>
        <div class="outerlistbox ">
            <div class="listbox loadmorebtn" id="append">
                <span class="wbtn wbtn_main" id="btnloadmore" onclick="LoadRPInfo()">显示更多</span>
            </div>
        </div>
    </div>
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="ActivityList.aspx"><span class="iconfont icon-back">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">

           <%
               if ((int)model.IsHide==-1)
               {

                   Response.Write(string.Format("<span class=\"wbtn wbtn_line_main\" ><span class=\"iconfont icon-b55 smallicon\"></span>待开始</span>"));

               }
            else if (model.MaxSignUpTotalCount > 0 && (model.SignUpTotalCount >= model.MaxSignUpTotalCount)&&(model.IsHide==0))
             {
                
                 Response.Write(string.Format("<span class=\"wbtn wbtn_line_main\" ><span class=\"iconfont icon-b55 smallicon\"></span>报名已满</span>"));
                 
             }

             else if (IsSubmit)
             {
              
                  Response.Write(string.Format("<span class=\"wbtn wbtn_line_main\" ><span class=\"iconfont icon-b55 smallicon\"></span>已报名</span>"));
                    
             }

             else if (model.ActivityIntegral > 0 && (UserInfo.TotalScore < model.ActivityIntegral))
             {
                
                 Response.Write(string.Format("<span class=\"wbtn wbtn_line_main\" ><span class=\"iconfont icon-b55 smallicon\"></span>积分不足</span>"));
                
             }
             else if (model.IsHide.Equals(1))
             {

                 Response.Write(string.Format("<span class=\"wbtn wbtn_line_main\" ><span class=\"iconfont icon-b55 smallicon\"></span>活动已结束</span>"));

             }
             else if (model.ActivityEndDate != null&&(DateTime.Now >= (DateTime)model.ActivityEndDate))
             {

                Response.Write(string.Format("<span class=\"wbtn wbtn_line_main\" ><span class=\"iconfont icon-b55 smallicon\"></span>已停止报名</span>"));
                 
             }

             else if (!PerActivity)
             {

                 Response.Write(string.Format("<span class=\"wbtn wbtn_line_main\" ><span class=\"iconfont icon-b55 smallicon\"></span>只接受特定用户报名</span>"));

             }
             else
             {
                 Response.Write(string.Format("<span class=\"wbtn wbtn_line_main\" id=\"applyactivebtn\"><span class=\"iconfont icon-b55 smallicon\"></span>立即报名</span>"));

             }
             %>


        </div>
        <!-- /.col-lg-10 -->
           <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="../MyCenter/Index.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->

    </div>
    <!-- footerbar -->
    <div class="wcontainer discusscontainer maxh100">
        <div class="modal fade " id="gnmdb" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-body textcenter">
                        <p>
                            提交成功</p>
                    </div>
                    <div class="modal-footer textcenter">
                        <span class="wbtn wbtn_main" data-dismiss="modal">确认</span>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->
    </div>


    <!--未注册引导注册-->
    <div class="modal fade bs-example-modal-sm" id="gnmdbReg" tabindex="-1" role="dialog"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body textcenter">
                    <p>
                    </p>
                </div>
                <div class="modal-footer textcenter">
                    <span class="wbtn wbtn_main" data-dismiss="modal" onclick="gotomp()">立即注册</span>
                    <span class="wbtn wbtn_main" data-dismiss="modal">继续浏览</span>
                    <!-- <a href="#" class="wbtn wbtn_main" data-dismiss="modal">确认</a> -->
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>


</body>
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="/WuBuHui/js/jquery.js"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="/WuBuHui/js/bootstrap.js"></script>
<script src="/WuBuHui/js/gotopageanywhere.js"></script>
<script src="../js/weixinsharebtn.js" type="text/javascript"></script>
<script src="/WuBuHui/js/partyinfo.js"></script>
<script src="/Scripts/jquery.form.js" type="text/javascript"></script>
<script type="text/javascript">
    var signUpId = '<%=model.SignUpActivityID %>';
    var PageIndex = 1;
    $(function () {
        InitSignUpDataInfo();
        LoadRPInfo();
    });

    function LoadRPInfo() {
      
        $.ajax({
            type: 'post',
            url: "/Handler/OpenGuestHandler.ashx",
            data: { Action: "GetADInfos", PageIndex: PageIndex, ActivityId: signUpId },
            dataType:'json',
            success: function (resp) {
                PageIndex++;
                var html = "";
                if (resp.Status == 0) {
                    if (resp.ExObj.length ==0) {
                        $("#btnloadmore").html("没有更多");
                        return;
                    }
                    $.each(resp.ExObj, function (index, Item) {
                        html += '<div class="listbox">';
                        html += '<span class="wbtn_round">';
                        html += '<img src="' + Item.K1 + '" alt="">';
                        html += '</span><span class="text">' + Item.Name + '</span><span class="time">' + Item.K2 + '</span>';
                        html += ' </div>';
                    });
                    $("#append").before(html);
                   
                }
                else {
                    $("#btnloadmore").html("没有更多");
                }
            }
        });
    };

    function InsertData() {



        $("#formsignin").ajaxSubmit({
            url: "/serv/ActivityApiJson.ashx",
            type: "post",
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 0) {//清空
                    // $('input:text').val("");
                    //$('textarea').val("");
                    //取得返回的Activity和ActivityData
                    //                    activity = resp.ExObj["Activity"];
                    //                    activityData = resp.ExObj["ActivityData"];
                    //                    activityFieldMapping = resp.ExObj["ActivityFieldMapping"];
                    SaveScore();
                    return;

                }
                else if (resp.Status == 1) {
                    //该用户已提交过数据
                    $('#gnmdb').find("p").text("该用户已提交过数据!");
                    $('#gnmdb').modal('show');

                }
                else {
                    $('#gnmdb').find("p").text(resp.Msg);
                    $('#gnmdb').modal('show');
                }
                $(".weixinshareshade").hide();
            }
        });
        return false;
    };

    function SaveScore() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiActivityHandler.ashx",
            data: { Action: "SavaUserScore", signUpId: signUpId },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 0) {

                    var ActivityIntegral = "<%=model.ActivityIntegral%>";
                    if (ActivityIntegral != "0") 
                    {
                        $('#gnmdb').find("p").text("报名成功!扣除" + ActivityIntegral+"积分");
                    }
                    else {
                        $('#gnmdb').find("p").text("报名成功");
                    }
                    $('#gnmdb').modal('show');
                    setTimeout("window.location.reload();", 2000);
                } else {
                    $('#gnmdb').find("p").text(resp.Msg);
                    $('#gnmdb').modal('show');
                }
            }
        });
    }


    function InitSignUpDataInfo() {
        $.ajax({
            type: 'post',
            url: "/Handler/OpenGuestHandler.ashx",
            data: { Action: "SignUpDataInfo", signUpId: signUpId },
            dataType: 'json',
            success: function (resp) {
                var html = "";
                if (resp.Status == 0) {
                    $.each(resp.ExObj, function (index, item) {
                        html += '<div class="input-group">';
                        html += '<span class="input-group-addon">' + item.MappingName + '</span>';
                        if (item.IsMultiline == "1") {
                            html += '<textarea rows=\"5\" type=\"text\" name=\" ' + item.FieldName + '\" id=\"' + item.FieldName + '\" value=\"\" placeholder=\"' + item.MappingName + '\" </textarea>';
                        } else {
                            html += '<input class=\"form-control\"  name=\"' + item.FieldName + '\" id=\"' + item.FieldName + '\" placeholder=\"' + item.MappingName + '\" type=\"text\">';
                        }
                        html += '</div>';
                    })
                    html += '<input id=\"activityID\" type=\"hidden\" value=\"' + signUpId + '\" name=\"ActivityID\" />';
                    $("#formsignin").append(html);
                    $("#formsignin").append(resp.ExStr);
                    LoadUserData();
                }
                else {
                    Alert(resp.Msg);
                }
            }
        });
    };


    ///加载当前用户信息 如果可用
    function LoadUserData() {
        $.ajax({
            type: 'post',
            url: "/Handler/OpenGuestHandler.ashx",
            data: { Action: "GetCurrentUserInfo" },
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 1) {
                    $("#formsignin").find("input[type='text'],teararea").each(function () {

                        var placeholder = $(this).attr("placeholder");
                        if (placeholder.indexOf("邮箱") >= 0 || placeholder.indexOf("邮件") >= 0 || placeholder.indexOf("email") >= 0 || placeholder.indexOf("Email") >= 0) {
                            $(this).val(resp.ExObj.Email);
                        }
                        if (placeholder.indexOf("公司") >= 0) {
                            $(this).val(resp.ExObj.Company);
                        }
                        if (placeholder.indexOf("职位") >= 0 || placeholder.indexOf("职务") >= 0) {
                            $(this).val(resp.ExObj.Postion);
                        }


                    });

                }


            }
        });


    }


</script>

 <script type="text/javascript">

     function SaveJiFen() {
         $.ajax({
             type: 'post',
             url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
             data: { Action: "SaveShareActivity", Id:"<%=model.JuActivityID%>", wxsharetype: "0" },
             dataType:'json',
             success: function (resp) {
                 if (resp.Status ==1) {
                     $('#gnmdb').find("p").text(resp.Msg);
                     $('#gnmdb').modal('show');
                 }
                 else {
                     //$('#gnmdb').find("p").text(resp.Msg);
                     //$('#gnmdb').modal('show');
                 }
                 $(".weixinshareshade").hide();
             }
         });
     };

     function SaveJiFenTimeLine() {
         $.ajax({
             type: 'post',
             url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
             data: { Action: "SaveShareActivity",Id:"<%=model.JuActivityID%>", wxsharetype: "1" },
             dataType:'json',
             success: function (resp) {
                 if (resp.Status ==1) {

                 }
                 else {

                 }
                 $(".weixinshareshade").hide();
             }
         });
     };

     $("#applyactivebtn").bind("touchstart", function () {

         var isHide = <%=model.IsHide%>;
         if (isHide==-1) {
         $('#gnmdbReg').find("p").text("活动还未开始");
         $('#gnmdbReg').modal('show');
         return false;
}

         var isRegUser = "<%=isUserRegistered %>";
         if (isRegUser == "False") {
             setTimeout(function () {
                 $('#gnmdbReg').find("p").text("您还没有注册五步会，立即注册获得25积分和更多功能！");
                 $('#gnmdbReg').modal('show');
             }, 500);
             return;
         }

         var userScoreEnough = "<%=isUserScoreEnough %>";
         if (isRegUser == "False") {
             setTimeout(function () {
                 $('#gnmdb').find("p").text("积分不足，您无法报名此活动！");
                 $('#gnmdb').modal('show');

             }, 500);
             return;
         }

         var isActivityStopped = "<%=isActivityStopped %>";
         if (isActivityStopped == "True") {
             setTimeout(function () {
                 $('#gnmdb').find("p").text("活动已结束，无法报名！");
                 $('#gnmdb').modal('show');

             }, 500);
             return;
         }

         gotopageanywhere('.activeinfomainlist', function () {
             if (!$(".applyactive").attr("style")) {
                 var applyheight = $(".applyactiveinbox").height() + 10
                 $(".applyactive").css({ "height": applyheight })
             }
         })
     })

     function gotomp() {
         window.location.href = "../MyCenter/MyCenter.aspx";

     }

 </script>

    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script>
        var lineLink = "http://<%=Request.Url.Host %>/WuBuHui/Activity/ActivityInfo.aspx?id=<%=model.JuActivityID%>";   //同样，必须是绝对路径
        var descContent = "<%=model.ActivityAddress %>";
        var shareTitle = '<%=model.ActivityName%>';
        var imgUrl = "http://" + window.location.host + "<%=model.ThumbnailsPath%>";
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
            //alert(res.errMsg);
        });

        //
    </script>

</html>
