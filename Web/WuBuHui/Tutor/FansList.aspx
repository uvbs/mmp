<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FansList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Tutor.FansList" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title></title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="../css/wubu.css?v=0.0.1">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body>
    <div class="mainlist bottom50" id="needload">
        <!-- style="background-color:#000;http://www.baidu.com" -->
        <p class="loadnote" style="text-align: center;">
        </p>
        <!-- listbox -->
    </div>
    <!-- mainlist -->
    <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:history.go(-1)">
                <span class="iconfont icon-back"></span></a>
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
<script src="../js/jquery.js" type="text/javascript"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="../js/bootstrap.js" type="text/javascript"></script>
<script src="../js/bottomload.js?v=0.0.2"></script>
<script src="../js/comm.js" type="text/javascript"></script>
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script>
    var PageIndex = 1;
    var PageSize = 10;
    var FromOrTo="";
    $(function () {
        FromOrTo=GetParm("fromorto");
        InitData();
    });
    function InitData() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "GetFanAttentionList", PageIndex: PageIndex, PageSize: PageSize, FromOrTo: FromOrTo, UserId: GetParm("userid") },
            dataType: 'json',
            success: function (resp) {
                if (resp.ExObj.length == 0) {
                    $(".loadnote").text("没有更多");
                    return;
                }

                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.length; i++) {

                    var linkurl = "";
                    var headimg = ""; //头像
                    var name = ""; //导师姓名—职位名称
                    var companyname = ""; //公司名称
                    var summary = ""; //个人简介
                    var istotor = "";
                    var userid = "";
                    if ((FromOrTo == "to") && (resp.ExObj[i].FromTutorInfo != null)) {//导师
                        //粉丝是导师
                        linkurl = "/WuBuHui/Tutor/WxTutorInfo.aspx?UserId=" + resp.ExObj[i].FromTutorInfo.AutoId;
                        headimg = resp.ExObj[i].FromTutorInfo.TutorImg;
                        name = resp.ExObj[i].FromTutorInfo.TutorName + "-" + resp.ExObj[i].FromTutorInfo.Position;
                        companyname = resp.ExObj[i].FromTutorInfo.Company;
                        summary = resp.ExObj[i].FromTutorInfo.Signature;
                        istotor = "<div class=\"wbtn_fly wbtn_flytr wbtn_green \">导师</div>";

                    }
                    else if ((FromOrTo == "from") && (resp.ExObj[i].ToTutorInfo != null)) {//导师
                        //关注的人是导师
                        linkurl = "/WuBuHui/Tutor/WxTutorInfo.aspx?UserId=" + resp.ExObj[i].ToTutorInfo.AutoId;
                        headimg = resp.ExObj[i].ToTutorInfo.TutorImg;
                        name = resp.ExObj[i].ToTutorInfo.TutorName + "-" + resp.ExObj[i].ToTutorInfo.Position;
                        companyname = resp.ExObj[i].ToTutorInfo.Company;
                        summary = resp.ExObj[i].ToTutorInfo.Signature;
                        istotor = "<div class=\"wbtn_fly wbtn_flytr wbtn_green \">导师</div>";
                        userid = resp.ExObj[i].ToTutorInfo.UserId;
                    }
                    else {//会员是粉丝
                        headimg = resp.ExObj[i].FromUserInfo.WXHeadimgurlLocal; //
                        name = resp.ExObj[i].FromUserInfo.TrueName; //
                        linkurl = "javascript:void(0);";
                    }

                    str.AppendFormat('<div class="listbox nopaddinglistbox" >');
                    str.AppendFormat('<a href="{0}" class="blocklink">', linkurl);
                    str.AppendFormat(' <div class="touxiang wbtn_round">');
                    str.AppendFormat('<img src="{0}">', headimg);
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="textbox">');
                    str.AppendFormat('<h3>');
                    str.AppendFormat('{0}</h3>', name);
                    str.AppendFormat('<p>');
                    str.AppendFormat('{0}<br>', companyname);
                    str.AppendFormat('{0}</p>', summary);
                    str.AppendFormat('</div>');
                    //                    str.AppendFormat('<div class="wbtn_fly wbtn_flytr wbtn_green ">');
                    //                    str.AppendFormat('导师');
                    //                    str.AppendFormat('</div>');
                    str.AppendFormat(istotor);
                    str.AppendFormat('</a>');
                    if (FromOrTo == "from") {
                        str.AppendFormat('<div class="wbtn_fly wbtn_flybr wbtn_red" data-touserid="{0}" onclick="AddFollowChain(this)">取消关注</div>', userid);

                    }
                    str.AppendFormat('</div>');


                }

                var html = str.ToString();
                $(".loadnote").before(html);
                if (str.toString() == "") {
                    $(".loadnote").text("没有更多");
                }
            },
            complete: function () {
                $("#needload").bottomLoad(function () {
                    $(".loadnote").text("正在加载...");
                    PageIndex++;
                    InitData();
                })


            }
        });
    };

    //获取Get参数
    function GetParm(parm) {
        //获取当前URL
        var local_url = window.location.href;

        //获取要取得的get参数位置
        var get = local_url.indexOf(parm + "=");
        if (get == -1) {
            return "";
        }
        //截取字符串
        var get_par = local_url.slice(parm.length + get + 1);
        //判断截取后的字符串是否还有其他get参数
        var nextPar = get_par.indexOf("&");
        if (nextPar != -1) {
            get_par = get_par.slice(0, nextPar);
        }
        return get_par;
    }
    //获取参数


    function AddFollowChain(obj) {
        var obj = $(obj);
        $.ajax({ type: 'post',
            url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
            data: { Action: "AddFollowChain", toUserId: obj.data("touserid"), isFollow: 0 },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 0) {

                 obj.closest(".nopaddinglistbox").remove();

                }
                else {
                    alert("操作失败");
                }

            }
        });
        }
       


</script>
</html>
