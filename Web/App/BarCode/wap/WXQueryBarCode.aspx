<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXQueryBarCode.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.BarCode.wap.WXQueryBarCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品真伪查询</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/artic1ecommv1.css" rel="stylesheet" type="text/css" />
    <link href="/css/easyresource.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/gzptcommon.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <style type="text/css">
        .mainlist li div
        {
            display: block;
            width: 100%;
            box-sizing: border-box;
            padding: 10px;
            background-color: #fff;
            border-radius: 4px;
            box-shadow: 0 0 6px rgba(0,0,0,0.2);
        }
    </style>
</head>
<body>
    <section class="box">
  <div class="searchbox">
        <input type="text" id="txtActivityName" placeholder="请输入产品条形码"/>
        <button class="searchbtn" type="button" id="btnSearch">搜索</button>
    </div>
    <ul class="mainlist articlelist currentlist" id="needList">
    </ul>
</section>
</body>
<script type="text/javascript">
    $(function () {
        $("#btnSearch").click(function () {
            $(this).html("正在搜索...");
            LoadNeed();
        })
    });


    //加载列表分页
    function LoadNeed() {
        $.ajax({
            type: 'post',
            url: "/Handler/App/WXGeneralHandler.ashx",
            data: { Action: "GetByBarCode", BarCode: $("#txtActivityName").val() },
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
                $("#btnSearch").html("搜索");
                var listHtml = '';
                var str = new StringBuilder();
                if (resp.Status == 0) {
                    if (resp.ExObj == "") {
                        str.AppendFormat('<li>');
                        str.AppendFormat('<div>{0}</div>', resp.Msg);
                        str.AppendFormat('</li>');
                    } else {
                        str.AppendFormat('<li>');
                        str.AppendFormat('<div>{0}</div>', resp.Msg);
                        str.AppendFormat('<div>', "");
                        str.AppendFormat('名称:{0}<br/>', resp.ExObj.CodeName);
                        str.AppendFormat('条形码:{0}<br/>', resp.ExObj.BarCode);
                        str.AppendFormat('经销商:{0}<br/>', resp.ExObj.Agency);
                        str.AppendFormat('型号:{0}<br/>', resp.ExObj.ModelCode);
                        str.AppendFormat('日期:{0}<br/>', resp.ExObj.InsetDatastr);
                        str.AppendFormat('</div>');
                        str.AppendFormat('</div>');
                        str.AppendFormat('</li>');
                    }
                    //构造视图模板

                    listHtml += str.ToString();
                    $("#needList").html(listHtml);
                }
                else {
                    $("#needList").html("");
                    alert(resp.Msg);
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
</script>
</html>
