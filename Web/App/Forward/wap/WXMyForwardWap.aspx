<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXMyForwardWap.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Forward.wap.WXMyForwardWap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我的转发</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/artic1ecommv1.css" rel="stylesheet" type="text/css" />
    <link href="/css/easyresource.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/gzptcommon.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
</head>
<body>
 <section class="box">
      <div class="searchbox">
        <input type="text" id="txtActivityName" placeholder="输入标题"/>
        <button class="searchbtn" type="button" id="btnSearch">搜索</button>
    </div>
    <ul class="mainlist articlelist currentlist" id="needList">
    </ul>
</section>
    
</body>
<script type="text/javascript">
    var PageIndex = 1; //第几页
    var PageSize = 5; //每页显示条数
    var Category = ""; //分类

    $(function () {
        LoadNeed();
        $("#ddlCategory").change(function () {
            PageIndex = 1;
            Category = $(this).val();
            $("#txtActivityName").val("");
            $("#btnSearch").html("正在搜索...");
            LoadNeed();

        })
        $("#btnSearch").click(function () {
            PageIndex = 1;
            $(this).html("正在搜索...");
            LoadNeed();

        })
    });

    function BtnClick() {

        PageIndex++;
        LoadNeed();


    }




    //加载列表分页
    function LoadNeed() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXForwardHandler.ashx",
            data: { Action: 'GetMyForwars', ActivityName: $("#txtActivityName").val(), PageIndex: PageIndex, PageSize: PageSize },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
                $("#btnSearch").html("搜索");
                if (resp.ExObj == null) { return; }
                var listHtml = '';
                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.length; i++) {
                    //构造视图模板
                    str.AppendFormat('<li>');
                    str.AppendFormat('<a href="#">', resp.ExObj[i].RealLink);
                    str.AppendFormat('<img src="{0}" >', resp.ExObj[i].ThumbnailsPath);
                    str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].ActivityName);
                    str.AppendFormat('<div class="article">');
                    str.AppendFormat('<p class="graytext">报名人数:{0}</p>', resp.ExObj[i].ActivitySignUpCount);
                    str.AppendFormat('<p class="datap">阅读量:<span class="bluetext">{0}</span></p>', resp.ExObj[i].OpenCount);
                    str.AppendFormat('<input type="button" onclick="OnHref({0},{1})" value="查看报名"> ', resp.ExObj[i].MonitorPlanID, resp.ExObj[i].ActivityId);
                    str.AppendFormat('</div>');
                    str.AppendFormat('</a>');
                    str.AppendFormat('</li>');

                };


                if (PageIndex == 1) {
                    if (resp.ExStr == "1") {
                        //显示下一页按钮
                        str.AppendFormat('<li>');
                        str.AppendFormat('<a id="btnNext" onclick="BtnClick()">');
                        str.AppendFormat('<div style="text-align:center;">显示下{0}条</div>', PageSize);
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                        //

                        listHtml += str.ToString();
                        $("#needList").html(listHtml);

                    }
                    else {
                        listHtml += str.ToString();
                        if (listHtml == "") {
                            listHtml = "暂时没有活动";
                        }
                        $("#needList").html(listHtml);

                    }



                }
                else {
                    listHtml += str.ToString();
                    if (listHtml != "") {
                        $("#btnNext").before(listHtml);
                    }
                    else {

                        $("#btnNext").remove();

                    }

                }



            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("加载超时，请刷新页面");

                }
                $("#btnSearch").html("搜索");
            }
        });

    }
    function OnHref(mid, aid) {
        window.location.href = "ForwardSignUpData.aspx?Mid=" + mid + "&Aid=" + aid + ""
    }
</script>
</html>
