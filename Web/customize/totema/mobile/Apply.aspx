<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Apply.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.mobile.Apply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>报名页面</title>
    <link type="text/css" rel="stylesheet" href="css/style.css?v=1.0.0.1" />
    <link type="text/css" rel="stylesheet" href="css/green.css" />
    <%-- <link type="text/css" rel="stylesheet" href="css/idangerous.swiper.css" />
    <link type="text/css" rel="stylesheet" href="css/swipebox.css" />
    --%>
    <style type="text/css">
        select
        {
            height: 30px;
            border-radius: 5px;
            width: 95%;
        }
        .menu5 ul li
        {
            width: 33.33%;
        }
    </style>
</head>
<body>
    <!--Page 1 content-->
    <div class="swiper-slide sliderbg">
        <div class="swiper-container swiper-nested">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="slide-inner">
                        <div class="pages_container">
                            <div id="main_panels">
                                <div class="form">
                                    <form class="cmxform" id="CommentForm" method="post" action="">
                                    <h2 class="page_title">
                                        请准确填写以下信息报名参赛
                                    </h2>
                                    <table width="96%" style="margin-left: 2%">
                                        <tr>
                                            <td align="center" style="">
                                                <div style="margin-top: 10px">
                                                </div>
                                                <table class="enroll-tb">
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                上传我们班的照片：<a href="javascript:;" id="btnAngle" class="hidden" onclick="ChangeAngle()">点击旋转图片</a>
                                                            </dd>
                                                            <img src="images/upload.png" id="imgclass" class="mAll50" onclick="setTimeout(function(){$('#btnAngle').show()},2000);txtThumbnailsPath.click()" />
                                                            <input type="file" id="txtThumbnailsPath" name="file1" style="display: none;" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                我们班的参赛口号：
                                                            </dd>
                                                            <input type="text" name="name" id="txtIntroduction" value="" placeholder=" 一句话，20个字以內"
                                                                class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                我们的班级名称：</dd>
                                                            <input type="text" name="name" id="txtVoteObjectName" value="" placeholder="如:复旦蓝生1204班"
                                                                class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dl>
                                                                （以上为公开信息 ）</dl>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div style="margin-top: 30px">
                                                </div>
                                                <table class="enroll-tb1" style="margin-bottom: 50px;">
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                我们的学校全称：
                                                            </dd>
                                                            <input type="text" name="name" id="txtSchoolName" value="" placeholder="" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                所在区县：
                                                            </dd>
                                                            <select id="ddlArea">
                                                                <option value="黄浦区">黄浦区</option>
                                                                <option value="长宁区">长宁区</option>
                                                                <option value="徐汇区">徐汇区</option>
                                                                <option value="静安区">静安区</option>
                                                                <option value="杨浦区">杨浦区</option>
                                                                <option value="虹口区">虹口区</option>
                                                                <option value="闸北区">闸北区</option>
                                                                <option value="普陀区">普陀区</option>
                                                                <option value="浦东新区">浦东新区</option>
                                                                <option value="宝山区">宝山区</option>
                                                                <option value="闵行区">闵行区</option>
                                                                <option value="金山区">金山区</option>
                                                                <option value="嘉定区">嘉定区</option>
                                                                <option value="青浦区">青浦区</option>
                                                                <option value="松江区">松江区</option>
                                                                <option value="奉贤区">奉贤区</option>
                                                                <option value="崇明县">崇明县</option>
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                我们的地址：</dd>
                                                            <input type="text" name="name" id="txtAddress" value="" placeholder=" 学校地址" class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                我们的联系人：</dd>
                                                            <input type="text" name="name" id="txtContact" value="" placeholder="班主任或某位家长，该信息经加密处理"
                                                                class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dd>
                                                                我们的联系人电话：</dd>
                                                            <input type="text" name="name" id="txtPhone" value="" placeholder="班主任或某位家长的手机号码"
                                                                class="form_input radius4" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <dl>
                                                                （以上为加密信息，仅作获奖联络之用）</dl>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <input type="button" name="submit" class="form_submit radius4 blue" id="btnSumbit"
                                                                value="提交报名" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="footer">
        <div class="menu5">
            <ul>
                <li><a href="eventdetails.aspx"><span>
                    <img src="images/en1.png" alt="" title="" />活动详情</span></a></li>
                <li><a href="index.aspx"><span>
                    <img src="http://jkbp.comeoncloud.net/open/jkbp/images/home1.png" />
                </span></a></li>
                <li><a href="MyClass.aspx"><span>
                    <img src="images/en2.png" alt="" title="" />我的报名</span></a></li>
            </ul>
        </div>
    </div>
    <%--    <script type="text/javascript" src="js/jquery.swipebox.js"></script>
    <script type="text/javascript" src="js/idangerous.swiper-2.1.min.js"></script>
    <script type="text/javascript" src="js/idangerous.swiper.scrollbar-2.1.js"></script>
    <script type="text/javascript" src="js/jquery.tabify.js"></script>
    <script type="text/javascript" src="js/jquery.fitvids.js"></script>
    <script type="text/javascript" src="js/code.js"></script>
    <script type="text/javascript" src="js/load.js"></script>
    --%>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>
 <script type="text/javascript" src="/Scripts/Common.js"></script>
    <script type="text/javascript" src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
</body>
<script type="text/javascript">
    var imgAngle = 0,
        angleArr = [0, 90, 180, 270];

    $(function () {
        $("#btnSumbit").click(function () {

            Apply();

        });

        $("#txtThumbnailsPath").live('change', function () {
            try {


                $.ajaxFileUpload({
                    url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                    secureuri: false,
                    fileElementId: 'txtThumbnailsPath',
                    dataType: 'json',
                    success: function (resp) {
                        if (resp.Status == 1) {
                            $("#imgclass").attr("src", resp.ExStr);
                            $("#imgclass").css({ "width": "200px" });

                        } else {
                            alert(resp.Msg);
                        }
                    }
                });

            } catch (e) {
                alert(e);
            }
        });

    })
    var ai = 0;
    function ChangeAngle() {
        ai++;
        if (ai > 3)
            ai = 0;
        imgAngle = angleArr[ai];
        $('#imgclass').removeClass('transform0 transform90 transform180 transform270');
        $('#imgclass').addClass('transform' + imgAngle);

    }

    function Apply() {
        var model = {
            Action: "AddVoteObjectInfo",
            VoteObjectHeadImage: $("#imgclass").attr("src"),
            VoteObjectName: $(txtVoteObjectName).val(),
            Area: $(ddlArea).val(),
            Introduction: $(txtIntroduction).val(),
            SchoolName: $(txtSchoolName).val(),
            Address: $(txtAddress).val(),
            Contact: $(txtContact).val(),
            Phone: $(txtPhone).val(),
            imgAngle: imgAngle,
            ComeonShareId: GetQueryString("comeonshareid")

        }
        $.ajax({
            type: "post",
            url: "/Handler/customize/totema/Handler.ashx",
            data: model,
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                if (resp.errcode == 0) {
                    alert("报名成功！");
                } else {
                    alert(resp.errmsg);
                }
            }
        })


    }

    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
</script>
    <script type="text/javascript">
        var pageData = {

            currUserOpenId: '<%=ZentCloud.BLLJIMP.BLLStatic.bll.GetCurrentUserInfo().WXOpenId%>', //当前用户的wxopenId
            currUserId: '<%=ZentCloud.BLLJIMP.BLLStatic.bll.GetCurrentUserInfo().UserID%>', //当前用户的userId
            title: 'TOTEMA为我们班投一票', //标题
            summary: 'TOTEMA为我们班投一票', //描述
            shareImgUrl: 'http://<%=Request.Url.Host%>/customize/totema/mobile/images/logo.jpg', //分享缩略图
            shareUrl: window.location.href, //分享链接

            tempShareId: CreateGUID(),
            preShareId: GetParm('comeonshareid'),
            callback: callback
        };

        var shareCallBackFunc = {
            timeline_s: function () {
                submitShare('timeline_s');
            },
            timeline_c: function () {
                //朋友圈分享取消
            },
            message_s: function () {
                //分享给朋友
                submitShare('message_s');
            },
            message_c: function () {
                //朋友分享取消
            }
        }

        var processUrl = function (url) {
            url = DelUrlParam(url, 'comeonshareid');
            url = DelUrlParam(url, 'from');
            url = DelUrlParam(url, 'isappinstalled');
            return url;
        }

        var callback = function (data) { }

        var submitShare = function (WxMsgType) {
            var reqData = {
                Action: 'ShareSubmit',
                url: processUrl(pageData.shareUrl),
                shareId: pageData.tempShareId,
                preId: pageData.preShareId,
                userId: pageData.currUserId,
                userWxOpenId: pageData.currUserOpenId,
                wxMsgType: WxMsgType
            }

            //分享到朋友圈
            $.ajax({
                type: 'post',
                url: '/serv/pubapi.ashx',
                data: reqData,
                dataType: 'jsonp',
                success: function (data) {
                    pageData.tempShareId = CreateGUID();
                }
            });
        }

        //移除原有参数 comeonshareid from isappinstalled
        pageData.shareUrl = processUrl(pageData.shareUrl);
        pageData.shareUrl = pageData.shareUrl + '?comeonshareid=' + pageData.tempShareId;

        wx.ready(function () {
            wxapi.wxshare({
                title: pageData.title,
                desc: pageData.summary,
                link: pageData.shareUrl,
                imgUrl: pageData.shareImgUrl
            }, shareCallBackFunc)
        });

    </script>

</html>
