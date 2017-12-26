<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.mobile.ClassList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>为我们班投一票</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/green.css" />
  
<style>
    .pages_container{margin-bottom:100px;}
    .menu5 ul li{width:33.33%;}
    #btnNext{width:99%;margin-top:10px;}
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
                           <h2 class="page_title">
                                为“我们班”投一票
                                <div>
                                 
                                    <table class="form form_input1 radius4" style="margin-top:-23px">
                                        <tr>
                                            <td valign="middle"  >
                                                <input type="text" name="keyword" id="txtKeyWord" value="" placeholder="按学校名称搜索" class="search-text" style="width: 140px"/>
                                            </td>
                                            <td valign="middle" align="right" style="width: 20px">
                                            
                                                <input type="image" name="image" src="images/search.png"  onclick="Search()" style="vertical-align: middle;
                                                    width: 20px" />
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </div>
                            </h2>
                           <input type="button" name="submit" class="form_submit radius4 blue" id="btnNext" value="努力加载中..." />
                           <div style="height:30px;"></div>
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

                <li><a href="<%=new ZentCloud.BLLJIMP.BllVote().GetTotemaApplyLingOrMyClassLink()%>"><span>
                    <img src="images/en3.png" alt="" title="" />我要报名</span></a></li>
            </ul>
        </div>
    </div>

</body>
<script type="text/javascript" src="js/jquery-1.10.1.min.js"></script>
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script type="text/javascript">

    var pageindex = 1;
    var pagesize = 15;
    var name = "";
    var number = "";
    var handlerurl = "/Handler/customize/totema/Handler.ashx";
    //投票
    function Vote(classid) {
        $.ajax({
            type: 'post',
            url: handlerurl,
            data: { Action: "UpdateVoteObjectVoteCount", classid: classid },
            timeout: 30000,
            dataType: "json",
            success: function (resp) {
                if (resp.errcode == 0) {
                    //投票成功
                    var oldcount = $("#span" + classid).text().replace('票', '');
                    var newcount = parseInt(oldcount) + 1;
                    $("#span" + classid).html(newcount + "票");
                    alert("投票成功");
                }
                else {
                    alert(resp.errmsg);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("投票超时，请重新投票");

                }
            }
        });


    }

    $(document).ready(function () {

        LoadData();

        $("#btnNext").click(function () {
            pageindex++;
            LoadData();

        });

    });

    function LoadData() {

        $("#btnNext").val("努力加载中...");
        $.ajax({
            type: 'post',
            url: handlerurl,
            data: { Action: 'GetVoteObjectVoteList', pageindex: pageindex, pagesize: pagesize, name: name, number: number },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {

                var str = new StringBuilder();
                for (var i = 0; i < resp.list.length; i++) {
                    var classname = "toogle_wrap gray";
                    if (i % 2 == 0) {
                        classname = "toogle_wrap white";
                    }

                    str.AppendFormat('<div class="{0}" data-item="0" >', classname);
                    str.AppendFormat('<div class="trigger1">');
                    str.AppendFormat('<div class="service_box">');
                    var link = "window.location.href='classdetail.aspx?id=" + resp.list[i].classid + "'";
                    str.AppendFormat('<div class="services_icon" onclick="{0}">', link);
                    str.AppendFormat('<img src="{0}" style="max-height:100px;" />', resp.list[i].classimage);
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="service_content" onclick="{0}">', link);
                    str.AppendFormat('<h4>{0}号<span>第{1}名</span><span id="span{2}">{3}票</span></h4>', resp.list[i].classnumber, resp.list[i].rank, resp.list[i].classid, resp.list[i].votecount);
                    str.AppendFormat('<p>{0}</p>', resp.list[i].classname);
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="services_icon2">');
                    str.AppendFormat('<a href="javascript:">');
                    str.AppendFormat('<img src="images/vote.png" class="grad" onclick="Vote({0})" classid="{0}"/><span class="vote">投一票</span>', resp.list[i].classid);
                    str.AppendFormat('</a>');
                    str.AppendFormat('</div>');
                    str.AppendFormat('</div>');
                    str.AppendFormat('</div>');
                    str.AppendFormat('</div>');



                };

                var listhtml = str.ToString();
                if (listhtml == "") {
                    $("#btnNext").val("没有更多了");
                }
                else {
                    $("#btnNext").val("显示更多");
                }
                $("#btnNext").before(str.ToString());
                $("#txtKeyWord").attr("placeholder", "按学校名称搜索 " + resp.totalcount + "个班级");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("加载超时，请刷新页面");

                }
            }
        });




    }

    function Search() {
        pageindex = 1;
        name = $("#txtKeyWord").val();
        $("div[data-item=0]").remove();
        LoadData();
    }

</script>
    <script type="text/javascript" src="/Scripts/Common.js"></script>
    <script type="text/javascript" src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        var pageData = {

            currUserOpenId: '<%=ZentCloud.BLLJIMP.BLLStatic.bll.GetCurrentUserInfo().WXOpenId%>', //当前用户的wxopenId
            currUserId: '<%=ZentCloud.BLLJIMP.BLLStatic.bll.GetCurrentUserInfo().UserID%>', //当前用户的userId
            title: 'TOTEMA新国际主义校服为我们班投一票', //标题
            summary: 'TOTEMA新国际主义校服为我们班投一票', //描述
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
        if (pageData.shareUrl.indexOf('?') > -1) {
            pageData.shareUrl += '&';
        } else {
            pageData.shareUrl += '?';
        }

        pageData.shareUrl = pageData.shareUrl + 'comeonshareid=' + pageData.tempShareId;

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