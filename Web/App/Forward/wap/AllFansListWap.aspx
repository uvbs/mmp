<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllFansListWap.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Forward.wap.AllFansListWap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>微吸粉</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/artic1ecommv1.css" rel="stylesheet" type="text/css" />
    <link href="/css/buttons2.css" rel="stylesheet" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/gzptcommon.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <style>
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
            border:solid 0px #fafafa !important;
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
            border:solid 0px #fafafa !important;
        }
        .tab{
            background-color:#fafafa !important;
            background: url("http://open-files.comeoncloud.net/www/hf/jubit/image/20160314/AB22C69BD456421299DF64D5C9156578.png") no-repeat;
            background-size:100% 100%;
        }
        .current{
            background-color:#fafafa !important;
            background: url("http://open-files.comeoncloud.net/www/hf/jubit/image/20160314/493E8050E6C8473C9755BBD8612AB0E8.png") no-repeat;
            background-size:100% 100%;
        }
    </style>
</head>
<body>
    <section class="box">
    <div class="articletag">
    <table style="width:100%;">
    <tr>
    <td><span class="article tab tabarticle current"  id="rdoArticle">所有文章</span></td>
    <td><span class="active tab tabactive"  id="rdoActivity">我的吸粉</span></td>
    <td><span class="activerank tab tabactiverank"  id="rdoActivityRank">吸粉排行</span></td>
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
<script type="text/javascript">
    var atMyPageIndex = 1; //个人活动
    var atMyPageSize = 5; //个人活动

    var atListPageIndex = 1; //所有活动
    var atListPageSize = 5; //所有活动

    var activityRankPageIndex = 1; //活动转发页码
    var activityRankPageSize = 5; //活动转发页数

    $(function () {
        LoadDataMyActivity();
        LoadDataActivityList();
        LoadDataActivityListRank();


        $("#btnNextActivityList").live("click", function () {  //所有活动
            atListPageIndex++;
            LoadDataActivityList();
        });

        $("#btnNextActivityListRank").live("click", function () {  //活动转发
            activityRankPageIndex++;
            LoadDataActivityListRank();
        });

        $("#btnNextActivity").live("click", function () { //我的转发
            atMyPageIndex++;
            LoadDataMyActivity();

        });

        $("#rdoArticle").click(function () {//所有活动
            if (!$(".articlelist")[0]) { return; }
            $(".currentlist").removeClass("currentlist");
            $(".articlelist").addClass("currentlist");
            $(".current").removeClass("current");
            $(".tabarticle").addClass("current");
        });


        $("#rdoActivity").click(function () {//我的转发
            if (!$(".activelist")[0]) { return; }
            $(".currentlist").removeClass("currentlist");
            $(".activelist").addClass("currentlist");
            $(".current").removeClass("current");
            $(".tabactive").addClass("current");
        });



        $("#rdoActivityRank").click(function () {//转发排行
            if (!$(".activelistrank")[0]) { return; }
            $(".currentlist").removeClass("currentlist");
            $(".activelistrank").addClass("currentlist");
            $(".current").removeClass("current");
            $(".tabactiverank").addClass("current");
        });

    });

    //加载所有活动列表
    function LoadDataActivityList() {
        var listHtml = "";
        try {
            jQuery.ajax({
                type: "Post",
                url: "/Handler/App/WXForwardHandler.ashx",
                data: {
                    Action: "GetForwars", ActivityName: "", PageIndex: atListPageIndex, PageSize: atListPageSize,forward_type:"fans"
                },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.ExObj == null) { return; }
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        str.AppendFormat('<li>');
                        str.AppendFormat('<a href="#" onclick="CkClick({0})"  >', resp.ExObj[i].ActivityId);
                        str.AppendFormat('<img src="{0}" >', resp.ExObj[i].ThumbnailsPath);
                        str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].ActivityName);
                        str.AppendFormat('<div class="article">');
                        str.AppendFormat('<p class="graytext">时间:{0}</p>', resp.ExObj[i].InsertDateStr);
                        str.AppendFormat('<p class="datap">阅读量:<span class="bluetext">{0}</span></p>', resp.ExObj[i].ReadNum);
                        str.AppendFormat('<p class="datap"><span class="bluetext">{0}</span></p>', resp.ExObj[i].IsForwar);
                        str.AppendFormat('<span class="btnInto"  onclick="CkClick({0})">点击进入转发</span>', resp.ExObj[i].ActivityId);
                        //str.AppendFormat('<a href="javascript:CkClick({0});" class="btnInto button button-primary button-rounded button-small">点击进入转发</a>', resp.ExObj[i].ActivityId);
                        str.AppendFormat('</div>');
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                    };
                    if (atListPageIndex == 1) {
                        if (resp.ExStr == "1") {
                            //显示下一页按钮
                            str.AppendFormat('<li>');
                            str.AppendFormat('<a id="btnNextActivityList" onclick="BtnClick()">');
                            str.AppendFormat('<div style="text-align:center;">显示下{0}条</div>', atListPageSize);
                            str.AppendFormat('</a>');
                            str.AppendFormat('</li>');
                            //
                            listHtml += str.ToString();
                            $("#ulActivityList").html(listHtml);
                        }
                        else {
                            listHtml += str.ToString();
                            if (listHtml == "") {
                                listHtml = "暂时没有文章";
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
                data: { Action: "GetMyForwars", ActivityName: "", PageIndex: atMyPageIndex, PageSize: atMyPageSize,forward_type:"fans" },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.ExObj == null) { return; }
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        str.AppendFormat('<li>');
                        str.AppendFormat('<a onclick="OnHref({0},{1})">', resp.ExObj[i].MonitorPlanID, resp.ExObj[i].ActivityId);
                        str.AppendFormat('<img src="{0}" >', resp.ExObj[i].ThumbnailsPath);
                        str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].ActivityName);
                        str.AppendFormat('<div class="article">');
                        str.AppendFormat('<p class="graytext">吸粉人数:{0}</p>', resp.ExObj[i].FansCount);
                        str.AppendFormat('<p class="datap">阅读量:<span class="bluetext">{0}</span></p>', resp.ExObj[i].OpenCount);

                        str.AppendFormat('<span class="btnInto"  onclick="OnHref({0},{1})">查看粉丝</span>', resp.ExObj[i].MonitorPlanID, resp.ExObj[i].ActivityId);
                        //                        str.AppendFormat('<p onclick="OnHref({0},{1})" >查看报名 </p>', resp.ExObj[i].MonitorPlanID, resp.ExObj[i].ActivityId);

                        //                        str.AppendFormat('<p onclick="OnHrefRank({0})" >查看转发排行 </p>', resp.ExObj[i].ActivityId);

                        str.AppendFormat('</div>');
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                    };
                    if (atMyPageIndex == 1) {
                        if (resp.ExStr == "1") {
                            //显示下一页按钮
                            str.AppendFormat('<li>');
                            str.AppendFormat('<a id="btnNextActivity" onclick="BtnClick()">');
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
                                listHtml = "暂时没有吸粉";
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
                data: {
                    Action: "GetForwars", ActivityName: "", PageIndex: activityRankPageIndex, PageSize: activityRankPageSize,forward_type:"fans"
                },
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    if (resp.ExObj == null) { return; }
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        //构造视图模板
                        str.AppendFormat('<li>');
                        str.AppendFormat('<a href="FansRank.aspx?activityid={0}" >', resp.ExObj[i].ActivityId);
                        str.AppendFormat('<img src="{0}" >', resp.ExObj[i].ThumbnailsPath);
                        str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].ActivityName);
                        str.AppendFormat('<div class="article">');
                        str.AppendFormat('<p class="graytext">时间:{0}</p>', resp.ExObj[i].InsertDateStr);
                        str.AppendFormat('<p class="datap">阅读量:<span class="bluetext">{0}</span></p>', resp.ExObj[i].ReadNum);
                        //str.AppendFormat('<p class="datap"><span class="bluetext">{0}报名</span></p>', resp.ExObj[i].ForwarNum);
                        str.AppendFormat('<span class="btnInto btnInto2"  onclick="OnHrefRank({0})">查看吸粉排行</span>', resp.ExObj[i].ActivityId);

                        str.AppendFormat('</div>');
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                    };
                    if (activityRankPageIndex == 1) {
                        if (resp.ExStr == "1") {
                            //显示下一页按钮
                            str.AppendFormat('<li>');
                            str.AppendFormat('<a id="btnNextActivityListRank" onclick="BtnClick()">');
                            str.AppendFormat('<div style="text-align:center;">显示下{0}条</div>', activityRankPageSize);
                            str.AppendFormat('</a>');
                            str.AppendFormat('</li>');
                            //
                            listHtml += str.ToString();
                            $("#ulActivityListRank").html(listHtml);
                        }
                        else {
                            listHtml += str.ToString();
                            if (listHtml == "") {
                                listHtml = "暂时没有文章";
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

    function OnHref(mid, aid) {
        window.location.href = "FansFollowData.aspx?Aid=" + aid + ""
    }
    function OnHrefRank(activityId) {
        window.location.href = "FansRank.aspx?activityid=" + activityId;
    }
    function CkClick(aid) {
        $.post("/Handler/App/WXForwardHandler.ashx", { Action: "CheckUserInfo" }, function (data) {
            pres = $.parseJSON(data);
            if (pres.Status == 0) {
                window.location.href = "WXForwardWap.aspx?id=" + aid;
            } else {
                if (confirm("此功能仅对注册会员开放，请先完善会员信息再使用此功能")) {
                    window.location.href = "/App/Cation/Wap/UserEdit.aspx";
                }
            }
        });
    }


</script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script>
     wxapi.wxshare({
            title: "微转发——活动/课程邀约、文章吸粉利器",
            desc: "打开页面，进入活动或文章，转发到朋友圈即可跟踪由您带来的报名人数人数或公众号粉丝人数",
            //link: "",
            imgUrl: "http://" + window.location.host + "/favicon.ico"
        })
    });
</script>
</html>
