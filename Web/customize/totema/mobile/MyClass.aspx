<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyClass.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.mobile.MyClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>我们班的资料</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/green.css" />
<%--    <link type="text/css" rel="stylesheet" href="css/idangerous.swiper.css" />
    <link type="text/css" rel="stylesheet" href="css/swipebox.css" />
--%>   
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>


<%--    <script src="js/jquery.validate.min.js" type="text/javascript"></script>
--%>
    <style type="text/css">
        input[type=text],select
        {
            height: 30px;
            border-radius: 5px;
            width: 95%;
        }
      .image_single img{height:auto;}
      .menu4{margin-bottom:150px;}
      .menu4 ul li{border-bottom:none;}
      .image_caption{width:88.7%;}
      .menu5 ul li{width:33.33%;}
      #imgclass{width:100%;margin-top:20px;margin-bottom:50px;}
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
                            <div class="index_container radius4">
                                <div class="image_single">
                                    <img src="<%=model.VoteObjectHeadImage%>" id="imghead" title="" border="0" /></div>
                                <div class="image_caption black " style="padding-left: 5px">
                                    <%= model.VoteObjectName%>
                                    <span style="float: right; padding-right: 5px">
                                        <%=model.Number%>号</span>
                                </div>
                            </div>
                            <!--Menu page-->
                            <div class="menu3">
                                <ul>
                                    <li><a href="javascript:void(0)">
                                        <img src="images/bm01.png" alt="" title="" /><span><%=model.VoteCount%>票</span></a></li>
                                    <li><a href="javascript:void(0)">
                                        <img src="images/bm02.png" alt="" title="" /><span>第<%=model.Rank%>名</span></a></li>
                                    <li><a href="javascript:void(0)">
                                        <img src="images/bm03.png" alt="" class="sentofriend" /><span>邀请朋友投票</span></a></li>
                                    <li><a href="javascript:void(0)">
                                        <img src="images/bm04.png" alt="" class="sharetofriend" /><span>分享至朋友圈</span></a></li>
                                </ul>
                                <div class="clearfix">
                                </div>
                            </div>
                            <div class="form menu4">
                                <ul>
                                <li>
                                我们班的照片(提示:点击下面图片可重新上传)
                                </li>
                                <li>
                                <img src="<%=model.VoteObjectHeadImage %>" id="imgclass" onclick="setTimeout(function(){$('#btnAngle').show()},2000);txtThumbnailsPath.click()" />
                                <input type="file" id="txtThumbnailsPath" name="file1" style="display: none;" />
                                </li>
                                <li>
                                <a href="javascript:;" id="btnAngle" class="hidden" onclick="ChangeAngle()"> 旋转图片</a>
                                </li>
                                    <li><a href="javascript:void()"><span>我们班的参赛口号：</span>
                                        <p>
                                            <input type="text" id="txtIntroduction" value="<%=model.Introduction %>" class="inputenroll-edit-text" />
                                        </p>
                                    </a></li>
                                    <li><a href="javascript:void()"><span>我们的班级名称：</span>
                                        <p>
                                            <input type="text" id="txtVoteObjectName" value="<%=model.VoteObjectName %>" class="inputenroll-edit-text" />
                                        </p>
                                    </a></li>
                                    <li><a href="javascript:void()"><span>我们的学校全称：</span>
                                        <p>
                                            <input type="text" id="txtSchoolName" value="<%=model.SchoolName %>" class="inputenroll-edit-text" />
                                        </p>
                                    </a></li>
                                    <li><a href="javascript:void()"><span>所在区县：</span>
                                        <p>
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
                                        </p>
                                    </a></li>
                                    <li><a href="javascript:void()"><span>我们的地址：</span>
                                        <p>
                                            <input type="text" id="txtAddress" value="<%=model.Address %>" class="inputenroll-edit-text" />
                                        </p>
                                    </a></li>
                                    <li><a href="javascript:void()"><span>我们的联系人：</span>
                                        <p>
                                            <input type="text" id="txtContact" value="<%=model.Contact %>" class="inputenroll-edit-text" />
                                        </p>
                                    </a></li>
                                    <li><a href="javascript:void()"><span>我们的联系人电话：</span>
                                        <p>
                                            <input type="text" id="txtPhone" value="<%=model.Phone %>" class="inputenroll-edit-text" />
                                        </p>
                                    </a></li>
                                </ul>
                                <div class="clearfix">
                                </div>
                                <br />
                                <input id="btnUpdate" type="button" name="submit" class="form_submit radius4 blue" value="更新资料" />
                                    
                            </div>
                        </div>
                        <!--End of page container-->
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
                <li><a href="ClassList.aspx"><span>
                    <img src="images/en4.png" alt="" title="" />我要投票</span></a></li>
            </ul>
        </div>
    </div>
    <div style="width: 100%; height: 100%; display: none; background: #000; opacity: 0.7;
        position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
        &nbsp;
    </div>
    <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; text-align: right;
        display: none;" id="sharebox">
        <img src="images/sharetip.png" width="100%" />
    </div>
<%--    <script type="text/javascript" src="js/jquery.swipebox.js"></script>
    <script type="text/javascript" src="js/idangerous.swiper-2.1.min.js"></script>
    <script type="text/javascript" src="js/idangerous.swiper.scrollbar-2.1.js"></script>
    <script type="text/javascript" src="js/jquery.tabify.js"></script>
    <script type="text/javascript" src="js/jquery.fitvids.js"></script>
    <script type="text/javascript" src="js/code.js"></script>
    <script type="text/javascript" src="js/load.js"></script>
    <script src="js/jquery.flexslider.js"></script>
--%></body>

<script type="text/javascript">
    $(document).ready(function () {
        $(".sentofriend,.sharetofriend").click(function () {
            $("#sharebg,#sharebox").show();
            $("#sharebox").css({ "top": $(window).scrollTop() })
        });

        $("#sharebg,#sharebox").click(function () {
            $("#sharebg,#sharebox").hide();
        });
        $("#btnUpdate").click(function () {

            UpdateMyClassInfo();

        })
        $(ddlArea).val("<%=model.Area%>");

    });

      
</script>

    <script type="text/javascript" src="/Scripts/Common.js"></script>
    <script type="text/javascript" src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        var pageData = {

            currUserOpenId: '<%=ZentCloud.BLLJIMP.BLLStatic.bll.GetCurrentUserInfo().WXOpenId%>',//当前用户的wxopenId
            currUserId: '<%=ZentCloud.BLLJIMP.BLLStatic.bll.GetCurrentUserInfo().UserID%>',//当前用户的userId
            title: 'TOTEMA新国际主义校服为我们班投一票', //标题
            summary: 'TOTEMA新国际主义校服为我们班投一票', //描述
            shareImgUrl: 'http://<%=Request.Url.Host%>/customize/totema/mobile/images/logo.jpg',//分享缩略图
            shareUrl: 'http://<%=Request.Url.Host%>/customize/totema/mobile/ClassDetail.aspx?id=<%=model.AutoID%>',//分享链接

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
    
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>
    <script type="text/javascript">
        var imgAngle = 0,
        angleArr = [0, 90, 180, 270];
        var ai = 0;
        function ChangeAngle() {
            ai++;
            if (ai > 3)
                ai = 0;
            imgAngle = angleArr[ai];
            $('#imgclass').removeClass('transform0 transform90 transform180 transform270');
            $('#imgclass').addClass('transform' + imgAngle);

        }
        $(function () {

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
    
    </script>
    <script type="text/javascript">
        function UpdateMyClassInfo() {
            var model = {
                Action: "EditVoteObjectInfo",
                VoteObjectName: $(txtVoteObjectName).val(),
                Area: $(ddlArea).val(),
                Introduction: $(txtIntroduction).val(),
                SchoolName: $(txtSchoolName).val(),
                Address: $(txtAddress).val(),
                Contact: $(txtContact).val(),
                Phone: $(txtPhone).val(),
                VoteObjectHeadImage: $("#imgclass").attr("src"),
                imgAngle: imgAngle
            }
            $.ajax({
                type: "post",
                url: "/Handler/customize/totema/Handler.ashx",
                data: model,
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        alert("保存成功");
                        window.location.href = window.location.href;
                    } else {
                        alert(resp.errmsg);
                    }
                }
            })


        }
</script>

</html>
