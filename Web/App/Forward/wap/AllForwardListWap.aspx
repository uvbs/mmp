<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllForwardListWap.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Forward.wap.AllForwardListWap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>微转发</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/artic1ecommv1.css" rel="stylesheet" type="text/css" />
    <link href="/css/buttons2.css" rel="stylesheet" />
    <style type="text/css">
        .btnInto
        {
            position: absolute;
            display: inline-block;
            width: 82px;
            right: 0;
            bottom: 0px;
            padding: 5px 7px 5px 15px;
            border-top-left-radius: 6px;
            color: #fff;
            background-color: #40A3E8;
        }
        .btnInto2
        {
            width: 100px;
        }
        .articletag
        {
            width: 100%;
        }
        .articletag span
        {
            width: 100%;
            position: relative;
            border: solid 0px #fafafa !important;
        }
        table
        {
            width: 100%;
        }
        table tr td
        {
            width: 33.3%;
        }
        .articletag .active
        {
            border-top-right-radius: 0px;
            border-bottom-right-radius: 0px;
            right: 0px;
        }
        
        .articletag span
        {
            border: solid 0px #fafafa !important;
        }
        .tab
        {
            background-color: #fafafa !important;
            background: url("http://open-files.comeoncloud.net/www/hf/jubit/image/20160314/AB22C69BD456421299DF64D5C9156578.png") no-repeat;
            background-size: 100% 100%;
        }
        .current
        {
            background-color: #fafafa !important;
            background: url("http://open-files.comeoncloud.net/www/hf/jubit/image/20160314/493E8050E6C8473C9755BBD8612AB0E8.png") no-repeat;
            background-size: 100% 100%;
        }
        .tag
        {
            -webkit-border-radius: 2px;
            float: right;
            padding: 5px;
            text-decoration: none;
            background: #cde69c;
            color: #638421;
            padding-left: 12px;
            padding-right: 12px;
            font-family: helvetica;
            font-size: 13px;
        }
        .tag1
        {
            -webkit-border-radius: 2px;
            float: right;
            padding: 5px;
            padding-left: 12px;
            padding-right: 12px;
            text-decoration: none;
            background: #FFCE44;
            color: #638421;
            font-family: helvetica;
            font-size: 13px;
        }
         .tag2
        {
            -webkit-border-radius: 2px;
            float: right;
            padding: 5px;
            padding-left: 12px;
            padding-right: 12px;
            text-decoration: none;
            background: #D55A48;
            color: #EEEEF1;
            font-family: helvetica;
            font-size: 13px;
        }
        .box
        {
            width: auto;
        }
    </style>
</head>
<body>
    <section class="box">
    <div class="articletag">
    <table style="width:100%;">
    <tr>
    <td><span class="article tab tabarticle current"  id="rdoArticle">所有转发</span></td>
    <td><span class="active tab tabactive"  id="rdoActivity">我的转发</span></td>
    <%if (website.IsShowForwardRank == 1)
      {
          %>
          <td><span class="activerank tab tabactiverank"  id="rdoActivityRank">转发排行</span></td>
          <%
      } 
     %>
       

    </tr>
    </table>
        
    </div>
    <ul class="mainlist articlelist currentlist" id="ulActivityList">
       
    </ul>
    <ul class="mainlist activelist" id="ulMyActivityList">
       
    </ul>
     <ul class="mainlist activelistrank " id="ulActivityListRank">
       
    </ul>
    </section>

</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="/Scripts/gzptcommon.js" type="text/javascript"></script>
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js" type="text/javascript"></script>
<script src="/lib/layer.mobile/layer.m.js"></script>
<script type="text/javascript">
    var atMyPageIndex = 1; //个人活动
    var atMyPageSize = 10000000; //个人活动

    var atListPageIndex = 1; //所有活动
    var atListPageSize = 10000000; //所有活动

    var activityRankPageIndex = 1; //活动转发页码
    var activityRankPageSize = 10000000; //活动转发页数

    var index = 1;

    $(function() {
        LoadDataMyActivity();
        LoadDataActivityList();
        LoadDataActivityListRank();


        $("#rdoArticle").click(function() {//所有活动
            if (!$(".articlelist")[0]) { return; }
            localStorage.setItem('forwards', 'forwards');
            localStorage.setItem('myforwards', '');
            localStorage.setItem('forwardrank', '');
            $(".currentlist").removeClass("currentlist");
            $(".articlelist").addClass("currentlist");
            $(".current").removeClass("current");
            $(".tabarticle").addClass("current");
        });


        $("#rdoActivity").click(function() {//我的转发
            if (!$(".activelist")[0]) { return; }
            localStorage.setItem('myforwards', 'myforwards');
            localStorage.setItem('forwards', '');
            localStorage.setItem('forwardrank', '');
            $(".currentlist").removeClass("currentlist");
            $(".activelist").addClass("currentlist");
            $(".current").removeClass("current");
            $(".tabactive").addClass("current");
        });



        $("#rdoActivityRank").click(function() {//转发排行
            if (!$(".activelistrank")[0]) { return; }
            localStorage.setItem('forwardrank', 'forwardrank');
            localStorage.setItem('myforwards', '');
            localStorage.setItem('forwards', '');
            $(".currentlist").removeClass("currentlist");
            $(".activelistrank").addClass("currentlist");
            $(".current").removeClass("current");
            $(".tabactiverank").addClass("current");
        });

       

        if (localStorage.getItem('forwards') == 'forwards') {
            $("#rdoArticle").click();
        } else if (localStorage.getItem('myforwards') == 'myforwards') {
            $("#rdoActivity").click();
        } else {
            $("#rdoActivityRank").click();
        }

        

    });





    $("#btnNextActivityList").click(function() {  //所有活动
        //                     atListPageIndex++;
        //                       LoadDataActivityList();
    });

    function btnNextActivityListClick() {
        atListPageIndex++;
        LoadDataActivityList();

    }


    $("#btnNextActivityListRank").click(function() {  //活动转发
        //                        activityRankPageIndex++;
        //                       LoadDataActivityListRank();
    });

    function btnNextActivityListRankClick() {
        activityRankPageIndex++;
        LoadDataActivityListRank();

    }

    $("#btnNextActivity").click(function() { //我的转发
        //            atMyPageIndex++;
        //            LoadDataMyActivity();

    });

    function btnNextActivityClick() {
        atMyPageIndex++;
        LoadDataMyActivity();
    }


    

    //加载所有活动列表
    function LoadDataActivityList() {
        var listHtml = "";
        try {
            jQuery.ajax({
                type: "Post",
                url: "/Handler/App/WXForwardHandler.ashx",
                data: { Action: "GetForwars", ActivityName: "", PageIndex: atListPageIndex, PageSize: atListPageSize
                },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.ExObj == null) { return; }
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        str.AppendFormat('<li>');
                        str.AppendFormat('<a href="#" onclick="CkClick({0},\'{1}\',\'{2}\')" >', resp.ExObj[i].ActivityId, resp.ExObj[i].ForwardType, resp.ExObj[i].CurrentUserId);
                        str.AppendFormat('<img src="{0}" >', resp.ExObj[i].ThumbnailsPath);

                        str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].ActivityName);

                        if (resp.ExObj[i].ForwardType == "活动") {
                            str.AppendFormat('<span class="tag">{0}</span>', resp.ExObj[i].ForwardType);
                        } else if (resp.ExObj[i].ForwardType == "问卷") {
                            str.AppendFormat('<span class="tag2">{0}</span>', resp.ExObj[i].ForwardType);
                        } else {
                            str.AppendFormat('<span class="tag1">{0}</span>', resp.ExObj[i].ForwardType);
                        }
                        str.AppendFormat('<div class="article">');
                        str.AppendFormat('<p class="graytext">时间:{0}</p>', resp.ExObj[i].InsertDateStr);
                        str.AppendFormat('<p class="datap">阅读量:<span class="bluetext">{0}</span></p>', resp.ExObj[i].PV);
                        str.AppendFormat('<p class="datap"><span class="bluetext">{0}</span></p>', resp.ExObj[i].IsForwar);
                        str.AppendFormat('<span class="btnInto"  onclick="CkClick({0},{1},{2})">点击进入转发</span>', resp.ExObj[i].ActivityId, resp.ExObj[i].ForwardType,resp.ExObj[i].CurrentUserId);
                        str.AppendFormat('</div>');


                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                    };
                    if (atListPageIndex == 1) {
                        if (resp.ExStr == "1") {
                            //显示下一页按钮
                            str.AppendFormat('<li>');
                            str.AppendFormat('<a id="btnNextActivityList" onclick="btnNextActivityListClick()">');
                            str.AppendFormat('<div style="text-align:center;">显示下一页</div>');
                            str.AppendFormat('</a>');
                            str.AppendFormat('</li>');
                            //
                            listHtml += str.ToString();
                            $("#ulActivityList").html(listHtml);
                        }
                        else {
                            listHtml += str.ToString();
                            if (listHtml == "") {
                                listHtml = "暂时没有数据";
                            }
                            $("#ulActivityList").html(listHtml);
                        }
                    }
                    else {
                        listHtml += str.ToString();
                        if (listHtml != "") {
                            $("#btnNextActivityList").before(listHtml);
                        }
                        else {
                            $("#btnNextActivityList").remove();
                        }
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时，请刷新重试");
                    }
                }
            })
        } catch (e) {
            alert(e);
        }
    }

    //加载我的转发活动列表
    function LoadDataMyActivity() {
        var listHtml = "";
        try {
            jQuery.ajax({
                type: "Post",
                url: "/Handler/App/WXForwardHandler.ashx",
                data: { Action: "GetMyForwars", ActivityName: "", PageIndex: atMyPageIndex, PageSize: atMyPageSize },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.ExObj == null) { return; }
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        str.AppendFormat('<li>');
                        if (resp.ExObj[i].ForwardType == "活动") {
                            str.AppendFormat('<a onclick="OnHref({0},{1})">', resp.ExObj[i].MonitorPlanID, resp.ExObj[i].ActivityId);
                        } else if (resp.ExObj[i].ForwardType == "文章") {
                            str.AppendFormat('<a onclick="OnHrefFans({0},{1})">', resp.ExObj[i].MonitorPlanID, resp.ExObj[i].ActivityId);
                        } else {
                            str.AppendFormat('<a onclick="OnHreAnswer({0},\'{1}\')">', resp.ExObj[i].ActivityId, resp.ExObj[i].LinkName);
                        }
                        str.AppendFormat('<img src="{0}" >', resp.ExObj[i].ThumbnailsPath);
                        str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].ActivityName);
                        if (resp.ExObj[i].ForwardType == "活动") {
                            str.AppendFormat('<span class="tag">{0}</span>', resp.ExObj[i].ForwardType);
                        } else if (resp.ExObj[i].ForwardType == "文章") {
                            str.AppendFormat('<span class="tag1">{0}</span>', resp.ExObj[i].ForwardType);
                        } else {
                            str.AppendFormat('<span class="tag2">{0}</span>', resp.ExObj[i].ForwardType);
                        }
                        str.AppendFormat('<div class="article">');
                        if (resp.ExObj[i].ForwardType == "活动") {
                            str.AppendFormat('<p class="graytext">报名人数:{0}</p>', resp.ExObj[i].ActivitySignUpCount);
                        } else if (resp.ExObj[i].ForwardType == "文章") {
                            str.AppendFormat('<p class="graytext">吸粉人数:{0}</p>', resp.ExObj[i].PowderCount);
                        } else {
                            str.AppendFormat('<p class="graytext">答题人数:{0}</p>', resp.ExObj[i].AnswerCount);
                        }

                        str.AppendFormat('<p class="datap">阅读量:<span class="bluetext">{0}</span></p>', resp.ExObj[i].OpenCount);
                        var type = '';
                        if (resp.ExObj[i].ForwardType == '文章') {
                            type = '粉丝';
                        } else if (resp.ExObj[i].ForwardType == '活动') {
                            type = '报名';
                        } else {
                            type = '答题';
                        }
                        if (resp.ExObj[i].ForwardType == "活动") {
                            str.AppendFormat('<span class="btnInto"  onclick="OnHref({0},{1})">查看{2}</span>', resp.ExObj[i].MonitorPlanID, resp.ExObj[i].ActivityId, type);
                        } else if (resp.ExObj[i].ForwardType == "文章") {
                            str.AppendFormat('<span class="btnInto"  onclick="OnHrefFans({0},{1})">查看{2}</span>', resp.ExObj[i].MonitorPlanID, resp.ExObj[i].ActivityId, type);
                        } else {
                            str.AppendFormat('<span class="btnInto"  onclick="OnHreAnswer({0},\'{1}\')">查看{2}</span>', resp.ExObj[i].ActivityId, resp.ExObj[i].LinkName,type);
                        }
                        str.AppendFormat('</div>');
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                    };
                    if (atMyPageIndex == 1) {
                        if (resp.ExStr == "1") {
                            //显示下一页按钮
                            str.AppendFormat('<li>');
                            str.AppendFormat('<a id="btnNextActivity" onclick="btnNextActivityClick()">');
                            str.AppendFormat('<div style="text-align:center;">显示下{0}条</div>', atMyPageSize);
                            str.AppendFormat('</a>');
                            str.AppendFormat('</li>');
                            //
                            listHtml += str.ToString();
                            $("#ulMyActivityList").html(listHtml);
                        }
                        else {
                            listHtml += str.ToString();
                            if (listHtml == "") {
                                listHtml = "暂时没有转发";
                            }
                            $("#ulMyActivityList").html(listHtml);
                        }
                    }
                    else {
                        listHtml += str.ToString();
                        if (listHtml != "") {
                            $("#btnNextActivity").before(listHtml);
                        }
                        else {
                            $("#btnNextActivity").remove();
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时，请刷新重试");
                    }
                }

            })

        } catch (e) {
            alert(e);
        }
    }


    //加载转发活动列表
    function LoadDataActivityListRank() {
        var listHtml = "";
        try {
            jQuery.ajax({
                type: "Post",
                url: "/Handler/App/WXForwardHandler.ashx",
                data: { Action: "GetForwars", ActivityName: "", PageIndex: activityRankPageIndex, PageSize: activityRankPageSize
                },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.ExObj == null) { return; }
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        str.AppendFormat('<li>');
                        if (resp.ExObj[i].ForwardType == "活动") {
                            str.AppendFormat('<a href="ForwardRank.aspx?activityid={0}" >', resp.ExObj[i].ActivityId);
                        } else if (resp.ExObj[i].ForwardType == "文章") {
                            str.AppendFormat('<a href="FansRank.aspx?activityid={0}" >', resp.ExObj[i].ActivityId);
                        } else {
                            str.AppendFormat('<a href="AnswerRank.aspx?activityid={0}&sid={1}" >', resp.ExObj[i].ActivityId, resp.ExObj[i].CurrentUserId);
                        }
                        str.AppendFormat('<img src="{0}" >', resp.ExObj[i].ThumbnailsPath);
                        str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].ActivityName);
                        if (resp.ExObj[i].ForwardType == "活动") {
                            str.AppendFormat('<span class="tag">{0}</span>', resp.ExObj[i].ForwardType);
                        } else if(resp.ExObj[i].ForwardType=="文章") {
                            str.AppendFormat('<span class="tag1">{0}</span>', resp.ExObj[i].ForwardType);
                        } else {
                            str.AppendFormat('<span class="tag2">{0}</span>', resp.ExObj[i].ForwardType);
                        }
                        str.AppendFormat('<div class="article">');
                        str.AppendFormat('<p class="graytext">时间:{0}</p>', resp.ExObj[i].InsertDateStr);
                        str.AppendFormat('<p class="datap">阅读量:<span class="bluetext">{0}</span></p>', resp.ExObj[i].PV);
                        if (resp.ExObj[i].ForwardType == "活动") {
                            str.AppendFormat('<span class="btnInto btnInto2"  onclick="OnHrefRank({0})">查看转发排行</span>', resp.ExObj[i].ActivityId);
                        } else if (resp.ExObj[i].ForwardType == "文章") {
                            str.AppendFormat('<span class="btnInto btnInto2"  onclick="OnHrefRankFans({0})">查看吸粉排行</span>', resp.ExObj[i].ActivityId);
                        } else {
                            str.AppendFormat('<span class="btnInto btnInto2"  onclick="OnHrefRankAnswer({0},{1})">查看答题排行</span>', resp.ExObj[i].ActivityId, resp.ExObj[i].CurrentUserId);
                        }
                        str.AppendFormat('</div>');
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                    };
                    if (activityRankPageIndex == 1) {
                        if (resp.ExStr == "1") {
                            //显示下一页按钮

                            str.AppendFormat('<li>');
                            str.AppendFormat('<a id="btnNextActivityListRank" onclick="btnNextActivityListRankClick()">');
                            str.AppendFormat('<div style="text-align:center;">显示下一页</div>');
                            str.AppendFormat('</a>');
                            str.AppendFormat('</li>');
                            //
                            listHtml += str.ToString();
                            $("#ulActivityListRank").html(listHtml);
                        }
                        else {
                            listHtml += str.ToString();
                            if (listHtml == "") {
                                listHtml = "暂时没有活动";
                            }
                            $("#ulActivityListRank").html(listHtml);
                        }
                    }
                    else {
                        listHtml += str.ToString();
                        if (listHtml != "") {
                            $("#btnNextActivityListRank").before(listHtml);
                        }
                        else {
                            $("#btnNextActivityListRank").remove();
                        }
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时，请刷新重试");
                    }
                }
            })
        } catch (e) {
            alert(e);
        }
    }
    ///微转发
    function OnHref(mid, aid) {
        window.location.href = "ForwardSignUpData.aspx?Mid=" + mid + "&Aid=" + aid + ""
    }
    //微吸粉
    function OnHrefFans(mid, aid) {
        window.location.href = "FansFollowData.aspx?Aid=" + aid + ""
    }
    function OnHrefRank(activityId) {
        window.location.href = "ForwardRank.aspx?activityid=" + activityId;
    }
    function OnHrefRankFans(activityId) {
        window.location.href = "FansRank.aspx?activityid=" + activityId;
    }
    function OnHrefRankAnswer(aid,sid) {
        window.location.href = "AnswerRank.aspx?activityid="+aid+"&&sid="+sid;
    }
    function OnHreAnswer(aid, sid) {
        window.location.href = "AnswerData.aspx?aid=" + aid + "&&sid=" + sid;
    }
    var lockCkClick = false;
    function CkClick(aid, type,uid) {
        if (!lockCkClick) {
            index++;
            lockCkClick = true;
            $.post("/Handler/App/WXForwardHandler.ashx", { Action: "CheckUserInfo" }, function (data) {
                pres = $.parseJSON(data);
                //alert(pres.Status);
                //return;
                lockCkClick = false;
                if (pres.Status == 0) {
                    index = 1;
                    if (type == "问卷") {
                        window.location.href = "/App/Questionnaire/wap/Questionnaire.aspx?id="+aid+"&&uid="+uid;
                    } else {
                        window.location.href = "WXForwardWap.aspx?id=" + aid;
                    }                    
                }else if(pres.Status == -2){
                    layer.open({
                        content: '<div style="text-align:center">该功能仅对代言人开放<br><span onclick="window.location.href=\'/customize/comeoncloud/Index.aspx?key=MallHome\'" style="color:#0289e4 !important;font-size: 16px;" >点击链接成为代言人</span></div>',
                        shadeClose: false
                    });

                } else {
                    layer.open({
                        content: '此功能仅对会员开放，请先完善会员信息再使用此功能',
                        shadeClose:false
                    });
                    setTimeout(function () {
                        window.location.href = "/App/Cation/Wap/UserEdit.aspx";
                    }, 2000);
                    //if (confirm("此功能仅对会员开放，请先完善会员信息再使用此功能")) {
                    //    window.location.href = "/App/Cation/Wap/UserEdit.aspx";
                    //}
                    
                }
            });



        }

    }


</script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "微转发——活动/课程邀约、文章吸粉利器",
            desc: "打开页面，进入活动或文章，转发到朋友圈即可跟踪由您带来的报名人数人数或公众号粉丝人数",
            //link: "",
            imgUrl: "http://" + window.location.host + "/favicon.ico"
        })
    });
</script>
</html>
