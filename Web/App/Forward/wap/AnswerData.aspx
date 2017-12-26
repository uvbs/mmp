<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnswerData.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Forward.wap.AnswerData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>答题</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/artic1ecommv1.css" rel="stylesheet" type="text/css" />
    <link href="/css/easyresource.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/gzptcommon.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
    <link href="/lib/ionic/ionic.css" rel="stylesheet" />
     <style type="text/css">
        .mainlist li div {
            display: block;
            width: 100%;
            box-sizing: border-box;
            padding: 10px;
            background-color: #fff;
            border-radius: 4px;
            box-shadow: 0 0 6px rgba(0,0,0,0.2);
        }

        #needList .list {
            margin-bottom: 0px;
        }

        #needList {
            padding-top: 8px;
        }

        #btnNext {
            display: block;
            width: 98%;
            box-sizing: border-box;
            padding: 10px;
            background-color: #fff;
            border-radius: 4px;
            box-shadow: 0 0 6px rgba(0,0,0,0.2);
            margin-top:10px;
            margin-left:1%;
            
        }
    </style>
</head>
<body>
    <section class="box">
        <div id="needList">
        </div>
    </section>
</body>
    <script type="text/javascript">
        var PageIndex = 1; //第几页
        var PageSize = 5; //每页显示条数
        var articleId = "<%=articleId%>";
        var spreadUserId = "<%=spreadUserId%>";

    $(function () {

        LoadNeed();



    });




    //加载列表分页
    function LoadNeed() {

        $.ajax({
            type: 'get',
            url: "/Handler/App/WXForwardHandler.ashx",
            data: { Action: 'GeAnswerInfo', PageIndex: PageIndex, PageSize: PageSize, article_id: articleId,sid:spreadUserId },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
                if (resp.ExObj == null) {
                    $("#needList").html(resp.Msg);
                    return;
                }

                var listHtml = '';
                var str = new StringBuilder();
                for (var i = 0; i < resp.ExObj.length; i++) {

                    str.AppendFormat('<div class="list">');
                    str.AppendFormat('<a class="item item-thumbnail-left" href="#">');
                    if (resp.ExObj[i].head_img_url == null) {
                        str.AppendFormat('<img src="http://files.comeoncloud.net/img/europejobsites.png">');
                    } else {
                        str.AppendFormat('<img src="{0}">', resp.ExObj[i].head_img_url);
                    }
                    if (resp.ExObj[i].name!=null) {
                        str.AppendFormat('<h2>{0}</h2>', resp.ExObj[i].name);
                    } else {
                        str.AppendFormat('<h2>微信用户</h2>');
                    }
                    if (resp.ExObj[i].phone != null) {
                        str.AppendFormat('<p>{0}</p>', resp.ExObj[i].phone);
                    }

                    
                    //str.AppendFormat('<p>邮箱:{0}</p>', resp.ExObj[i].email==null?"":resp.ExObj[i].email);
                    //str.AppendFormat('<p>公司:{0}</p>', resp.ExObj[i].company==null?"":resp.ExObj[i].company);
                    str.AppendFormat('</a>');

                    str.AppendFormat('</div>');
                };
                if (PageIndex == 1) {
                    if (resp.ExStr == "1") {
                        str.AppendFormat('<li>');
                        str.AppendFormat('<a id="btnNext" onclick="BtnClick()">');
                        str.AppendFormat('<div style="text-align:center;">显示下{0}条</div>', PageSize);
                        str.AppendFormat('</a>');
                        str.AppendFormat('</li>');
                        listHtml += str.ToString();
                        $("#needList").html(listHtml);
                    }
                    else {
                        listHtml += str.ToString();
                        if (listHtml == "") {
                            listHtml = "暂时没有";
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

                        //

                        $("#btnNext").removeAttr("onclick");
                        $("#btnNext").before(listHtml);

                    }
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    alert("加载超时，请刷新页面");
                }
            }
        });
    }

    function BtnClick() {
        PageIndex++;
        LoadNeed();
    }
</script>
</html>
