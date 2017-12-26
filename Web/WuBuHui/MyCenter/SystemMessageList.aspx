<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemMessageList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.MyCenter.SystemMessageList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html >
<html lang="zh-cn">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>消息</title>
    <!-- Bootstrap -->
    <link rel="stylesheet" href="http://at.alicdn.com/t/font_1413272586_8236315.css">
    <link rel="stylesheet" href="/WuBuHui/css/wubu.css?v=0.0.4">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
		<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
		<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
</head>
<body>
<div class="mainlist bottom50" id="needload">
<p class="loadnote" style="text-align: center;">
        </p>
</div>

<!-- footerbar -->
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
<script src="/WuBuHui/js/behindbar.js?v=0.0.3"></script>
<script src="../js/bottomload.js" type="text/javascript"></script>
<script src="../js/comm.js" type="text/javascript"></script>
<script>
    var PageIndex = 1;
    var PageSize = 10;
    $(function () {
        InitData();
    });
    function InitData() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/SystemNoticeHandler.ashx",
            data: { Action: "GetSystemNoticeList", PageIndex: PageIndex, PageSize: PageSize, noticeType: GetParm("type") },
            dataType: 'json',
            success: function (resp) {
                $(".loadnote").text("　");
                if (resp.Status == 0) {
                    if (resp.ExObj == null) {
                        $(".loadnote").text("没有更多");
                        return;
                    }
                    var html = "";
                    $.each(resp.ExObj, function (Index, Item) {
//                        if (Item.RedirectUrl == null) {
//                            html += '<a onclick=\"SetRead(' + Item.AutoID + ')\" data-href=\"\" class="listbox"><div class="textbox">';
//                        }
//                        else {
                        html += '<a id=\"a' + Item.AutoID + '\" onclick=\"SetRead(' + Item.AutoID + ')\" data-href="' + Item.RedirectUrl + '" class="listbox"><div class="textbox">';
                       // }

                        var ishaveread = "";
                        if ($.trim(Item.InsertTimeString) == "") {
                        
                            ishaveread = "<div class=\"wbtn_fly wbtn_flytr wbtn_yellow\">未读</div>";
                        }
                        html += '<h3>' + Item.Title + '</h3><p>' + Item.Ncontent + '</p>'+ishaveread+'</div></a>';
                    });
                    $(".loadnote").before(html);
                    if (html == "") {
                        $(".loadnote").text("没有更多");
                    }



                } else {
                    // $('#gnmdb').find("p").text(resp.Msg);
                    //$('#gnmdb').modal('show');
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

    //设置消息为已读
    function SetRead(id) {

        $.ajax({
            type: 'post',
            url: "/Handler/App/SystemNoticeHandler.ashx",
            data: { Action: "SetRead", Id: id },
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 1) {
                    if ($("#a" + id).data("href") != null || $("#a" + id).data("href") != "") {
                        window.location = $("#a" + id).data("href");
                    }
                }

            },
            complete: function () {



            }
        });
    
    }
</script>
</html>