<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderCode.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.OrderCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>提货二维码</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/zepto.min.js" type="text/javascript"></script>
    <script src="/Ju-Modules/Common/Cookie.Min.js" type="text/javascript"></script>
    <script src="/Scripts/json2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>

</head>

<body>
<section class="box">
    <div class="m_codebox">
        <div class="code">
            <img id="imgcode" src="" class="codepic">
            
        </div>
    </div>

    <div class="backbar">
        <a href="javascript:history.go(-1);" class="back"><span class="icon"></span></a>
    </div>
</section>
</body>
<script type="text/javascript">

    var handlerUrl = "/Handler/QCode.ashx";
    var code = "<%=QCode.ToString()%>";
    $(function () {

        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { code: code },
            success: function (result) {
                $("#imgcode").attr("src", result);
            }
        });


    });

</script>
</html>
