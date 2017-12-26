<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxTutorInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Tutor.WxTutorInfo" %>

<!DOCTYPE html >
<html lang="zh-cn">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>导师详情</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://at.alicdn.com/t/font_1413272586_8236315.css">
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.7">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <div class="teacherheader">
        <div class="col-xs-4">
          <%if (FansNumInt>0){%>
            <a href="fanslist.aspx?userid=<%=tInfo.UserId%>&fromorto=to" class="wbtn followteacher wbtn_yellow">
            <%}else{%>
            <a href="javascript:void(0)" class="wbtn followteacher wbtn_yellow">
            <%}%>
            粉丝:<label id="FansNum" runat="server"></label>
            </a>
        </div>
        <div class="col-xs-4">
            <div class="touxiang wbtn_round">
                <img src="" runat="server" id="IheadImg">
            </div>


        </div>
        <div class="col-xs-4">
            <div class="followteacher wbtn wbtn_red" onclick="Follow()">
            <span id="IsFollowedString" class="iconfont icon-xin2"></span>
            <label id="lblguanzhu"><%=IsFollowedString%></label>
            </div>
        </div>
        <span class="teachername">
            <%=tInfo.TutorName%>
            <br />
            <%for (int i = 1; i <= UserLevel; i++)
              {%>
            <span class="iconfont icon-zuanshi"></span>
            <% } %>
        </span>
    </div>
    <div class="teacherinfotagbox">
        <div class="teacherinfotag">
            <div class="col-xs-4 change0">
                <span class="iconfont icon-14"></span>简介
            </div>
            <div class="col-xs-4">
                <span class="iconfont icon-34"></span>话题
            </div>
            <div class="col-xs-4">
                <span class="iconfont icon-78"></span>文章
            </div>
        </div>
    </div>
    <div class="teacherinfobox bottom50 teacherpagetag teacherpagetagshow">
        <div class="tagbox">
            <span class="tagtitle">就职企业:</span> <span class="wbtn_tag wbtn_yellow wbtn_bigtag">
                <%=tInfo.Company%>
            </span>
        </div>

        <div class="tagbox">
            <span class="tagtitle">导师职称:</span> <span class="wbtn_tag wbtn_yellow wbtn_bigtag">
                <%=tInfo.Position%>
            </span>
        </div>

       <%if (!string.IsNullOrEmpty(tInfo.City))
         {%>
        <div class="tagbox">
            <span class="tagtitle">城市:</span> <span class="wbtn_tag wbtn_yellow wbtn_bigtag">
                <%=tInfo.City%>
            </span>
        </div>

         <%} %>

        <%if (!string.IsNullOrEmpty(tInfo.Signature))
          {%>
        <div class="tagbox">
            <span class="tagtitle">导师签名:</span> <span class="wbtn_tag wbtn_greenyellow wbtn_bigtag">
                <%=tInfo.Signature%>
            </span>
        </div>
        <%} %>
        <div class="tagbox">
            <span class="tagtitle">相关行业:</span>
            <asp:Literal ID="txtTrade" runat="server" />
        </div>
        <div class="tagbox">
            <span class="tagtitle">专业技能:</span><asp:Literal ID="txtProfessiona" runat="server" />
        </div>
        <div class="introduction">
            <strong>导师简介：</strong>
            <asp:Literal Text="text" ID="txtExplain" runat="server" />
        </div>
    </div>
    <!-- mainlist -->
    <div class="mainlist bottom50 teacherpagetag " id="rHtml">
        <!-- listbox -->
        <p class="loadnote" style="text-align: center;" id="loadnotereview">
        </p>
    </div>
    <!-- mainlist -->
    <div class="mainlist bottom50 teacherpagetag" id="NHtml">
        <!-- listbox -->
        <p class="loadnote" style="text-align: center;" id="loadnotenews">
        </p>
    </div>
    <div class="fixbox closethis" id="creatdiscuss">
        <form class="creatdiscuss_form" action="">
        <textarea placeholder="请输入您要咨询的内容" name="txtTitle" id="txtTitle" class="discusstitleinfo"
            rows="2"></textarea>
        <textarea class="secondtextarea" placeholder="详细描述" name="txtReviewContent" id="txtReviewContent"></textarea>
        <div class="discuss_tagbox">
            <div class="discuss_inbox">
                <%=TagStr%>
            </div>
        </div>
        <div class="discuss_contral">
            <input class="checkbox" type="checkbox" checked name="ckPower" id="discusstag5">
            <label for="discusstag5" class="discusstag">
                <span class="wbtn wbtn_gary"><span class="iconfont"></span></span>公开话题
            </label>
            <span class="wbtn wbtn_red discuss_submit" id="discuss_Save" onclick="SavaReviewInfo()">
                提交 </span><span class="wbtn wbtn_main discuss_exit" id="discuss_exit">取消 </span>
        </div>
        </form>
    </div>
    <!-- mainlist -->
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:pagegoback('../MyCenter/Index.aspx')">
                <span class="iconfont icon-back"></span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            <span class="wbtn wbtn_line_main" id="askthisteacher"><span class="iconfont icon-34 smallicon">
            </span>咨询导师 </span>
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
<script src="../js/jquery.js" type="text/javascript"></script>
<script src="../js/comm.js" type="text/javascript"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="../js/bootstrap.js" type="text/javascript"></script>
<script src="/WuBuHui/js/fixbox.js?v=0.0.4"></script>
<script src="/WuBuHui/js/teachertag.js?v=0.0.3"></script>
<script src="/WuBuHui/js/bottomload.js?v=0.0.3"></script>
<script src="/WuBuHui/js/filterbox.js?v=0.0.3"></script>
<script>
var IsFollowedString;
$(function () {
    InitR();
    InitN();

    IsFollowedString = "<%=IsFollowedString%>";
    if (IsFollowedString == "关注") {
        $("#IsFollowedString").attr("class", "iconfont icon-xin2");
    }
    else {
        $("#IsFollowedString").attr("class", "iconfont icon-xin");
    }

});
    var UserId = '<%=tInfo.UserId %>';
    var RPageIndex = 1; //话题当前页数
    var RPageSize = 10; //话题当前条数
    var NPageIndex = 1; //文章当前页数
    var NPageSize = 10; //文章当前条数
    var IsSumbit = false;
    function SavaReviewInfo() {
        var Power = 0;
        if (document.getElementById("discusstag5").checked == true) {
            Power = 0
        } else {
            Power = 1;
        }
        var CategoryType = "";
        $("input[name='word']:checked").each(function () {
            CategoryType += $(this).val() + ",";
        });

        if ($("#txtTitle").val() == "") {
            $('#gnmdb').find("p").text("请输入标题");
            $('#gnmdb').modal('show');
            return false;

        }

        if (IsSumbit) {
            //$('#gnmdb').find("p").text("请不要重复提交");
            //$('#gnmdb').modal('show');
            return;
        }
        IsSumbit = true;
        $("#discuss_Save").text("正在提交...");
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "SaveReviewInfo", UserId: UserId, CategoryType: CategoryType, Power: Power, Title: $.trim($("#txtTitle").val()), ReviewContent: $.trim($("#txtReviewContent").val()) },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 0) {
                    window.location.href = "/WuBuHui/WordsQuestions/MyWXDiscussList.aspx";
                }
                else {
                    $('#gnmdb').find("p").text(resp.Msg);
                    $('#gnmdb').modal('show');
                }

            },
            complete: function () {
                IsSumbit = false;
                $("#discuss_Save").text("提交");
            }
        });

    }

    ///是否喜欢
    function OnLike() {
        $.ajax({ type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "SavaLike", UserId: UserId },
            dataType: 'json',
            success: function (resp) {
                $("#txtLikeNum").text(resp.ExInt);
                if (resp.ExStr == "0") {
                    $("#spzan").attr("class", "iconfont icon-xin2")
                }
                if (resp.ExStr == "1") {
                    $("#spzan").attr("class", "iconfont icon-xin")
                }
            }
        });
    };

    ///是否喜欢
    function Follow() {
        var isFollow = 1;
        if ($.trim(IsFollowedString) == "已关注") {
            isFollow = 0;
        }
        $.ajax({ type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "AddFollowChain", toUserId: UserId, isFollow: isFollow },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 0) {
                    if (isFollow == 1) {
                        $("#IsFollowedString").attr("class", "iconfont icon-xin");
                        $("#FansNum").text(parseInt($("#FansNum").text()) + 1)
                    }
                    else {
                        $("#IsFollowedString").attr("class", "iconfont icon-xin2");
                        var num = parseInt($("#FansNum").text()) - 1;
                        if (num < 0) {
                            num = 0;
                        }
                        $("#FansNum").text(num);
                    }

                    if (isFollow == 1) {
                        IsFollowedString = "已关注";
                    }
                    else {
                        IsFollowedString = "关注";
                    }
                    $("#lblguanzhu").text(IsFollowedString);


                }

            }
        });
    };

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

    function InitR() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "GetReviewInfos", PageIndex: RPageIndex, PageSize: RPageSize, UserId: UserId },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 0) {
                    data = resp.ExObj;
                    if (data == null) {
                        $("#loadnotereview").text("没有更多");
                        return;
                    };
                    var html = "";
                    $.each(data, function (Index, Item) {
                        html += '<a href="/WuBuHui/WordsQuestions/WXDiscussInfo.aspx?AutoId=' + Item.AutoId + '" class="listbox"><div class="textbox"><h3 class="maxh3">';
                        html += Item.ReviewTitle + '</h3><p>' + Item.ReviewContent + '</p>';
                        html += '</div><div class="tagbox">';
                        html += '<span class="wbtn_tag wbtn_red"><span class="iconfont icon-xin2"></span>' + Item.PraiseNum + '</span>';
                        //html += '<span class="wbtn_tag wbtn_greenblue"><span class="iconfont icon-cai"></span>' + Item.StepNum + '</span>';
                        if (Item.actegory != null) {
                            $.each(Item.actegory, function (intex, da) {
                                html += '<span class="wbtn_tag wbtn_main">' + da.CategoryName + '</span>';
                            })
                        }

                        html += '</div><div class="wbtn_fly wbtn_flybr wbtn_yellow timetag">' + FormatDate(Item.ReplyDateTiem) + '</div></a>';
                    });
                    //$("#rHtml").append(html);
                    $("#loadnotereview").before(html);
                    if (html == "") {
                        $("#loadnotereview").text("没有更多");
                    }

                }
                else {
                    //                    $('#gnmdb').find("p").text(resp.Msg);
                    //                    $('#gnmdb').modal('show');
                    $("#loadnotereview").text("没有更多");
                }
            },
            complete: function () {

                $("#rHtml").bottomLoad(190, function () {
                    RPageIndex++;
                    $("#loadnotereview").text("正在加载...");
                    InitR();
                })
            }

        })
    };

    function InitN() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "GetNewInfos", PageIndex: NPageIndex, PageSize: NPageSize, UserId: UserId },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 0) {
                    data = resp.ExObj;
                    if (data == null) {
                        $("#loadnotenews").text("没有更多");
                        return;
                    };
                    var html = "";
                    $.each(data, function (Index, Item) {
                        html += '<a href="/WuBuHui/News/NewsDetail.aspx?id=' + Item.JuActivityID + '" class="listbox"><div class="textbox"><h3 class="maxh3">';
                        html += Item.ActivityName + '</h3><p>' + Item.Summary + '</p>';
                        html += '</div><div class="tagbox">';
                        html += '<span class="wbtn_tag wbtn_red"><span class="iconfont icon-eye"></span>' + Item.PV + '</span>'
                        html += '<span class="wbtn_tag wbtn_main">' + Item.CategoryName + '</span></div>'
                        html += '<div class="wbtn_fly wbtn_flybr wbtn_greenyellow timetag">' + FormatDate(Item.CreateDate) + '</div></a>'

                    });
                    //$("#NHtml").append(html);
                    $("#loadnotenews").before(html);
                    if (html == "") {
                        $("#loadnotenews").text("没有更多");
                    }


                }
                else {
                    //                    $('#gnmdb').find("p").text(resp.Msg);
                    //                    $('#gnmdb').modal('show');
                    $("#loadnotenews").text("没有更多");
                }
            },
            complete: function () {
                $("#NHtml").bottomLoad(190, function () {
                    NPageIndex++;
                    $("#loadnotenews").text("正在加载...");
                    InitN();

                })
            }

        })

    }

    $("#askthisteacher").bind("touchend", function () {
        var isRegUser = "<%=isUserRegistered %>";
        if (isRegUser == "False") {
            setTimeout(function () {
                $('#gnmdbReg').find("p").text("您还没有注册五步会，立即注册获得25积分和更多功能！");
                $('#gnmdbReg').modal('show');

            }, 500);
            $(".fixbox").addClass("closethis");

        }
        checktagwidth()

    })
    function checktagwidth() {
        var tagwidth = 0
        $(".discuss_inbox").find(".discusstag").each(function () {
            tagwidth += $(this).outerWidth()
        })
        $(".discuss_inbox").css({ "width": tagwidth })
    }

    function gotomp() {
        window.location.href = "../MyCenter/MyCenter.aspx";

    }
    
</script>
<script type="text/javascript">

    function SaveJiFen() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiUserHandler.ashx",
            data: { Action: "SaveShareTotor", Id: "<%=tInfo.AutoId%>", wxsharetype: "0" },
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
            data: { Action: "SaveShareTotor", Id: "<%=tInfo.AutoId%>", wxsharetype: "1" },
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





</script>

    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script>
        var lineLink =window.location.href;
        var descContent ="<%=tInfo.Company%>\n<%=tInfo.Position%>\n<%=tInfo.Signature%>";
        var shareTitle = "<%=tInfo.TutorName%>";
        var imgUrl = "http://" + window.location.host + "<%=tInfo.TutorImg%>";
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
