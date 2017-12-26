<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreRule.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.ScoreRule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>积分规则</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery2.1.1.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
</head>
<body>
    <section class="box">
    <div class="recordbox">

        <div class="recordlist" id="recordlist">
        <%=model== null ? "无":model.ScoreRule.Replace("\n","</br>")%>
        </div>

    </div>
    <div class="backbar">
        <a href="ScoreManage.aspx" class="back"><span class="icon"></span></a>
        
    </div>
</section>
</body>
</html>
