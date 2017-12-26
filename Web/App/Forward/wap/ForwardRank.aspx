<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForwardRank.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Forward.wap.ForwardRank" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <title>
        
        <%=ActivityName%>转发<%=website.SortType==0?"报名":"阅读量"%>排行榜 </title>
    <!-- Bootstrap -->
    <link href="/WuBuHui/css/wubu.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .mainlist .tagbar .wbtn
        {
            width: 100%;
        }
        .nodata
        {
            text-align: center;
        }
        .mainlist .tagbar .wbtn
        {
            height: 65px;
        }
    </style>
</head>
<body class="whitebg">
    <div class="mainlist">
        <div class="tagbar">
            <a href="javascript:window.location.href=window.location.href;" class="wbtn wbtn_greenyellow">
                <span class="title">
                    <%=ActivityName%>转发<%=website.SortType==0?"报名":"阅读量"%>排行榜</span> </a>
        </div>
    </div>
    <div class="scoretoplist">
        <%
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AddHeader("Pragma", "No-Cache");
            System.Text.StringBuilder sb = new StringBuilder();
            for (int i = 0; i < UserList.Count; i++)
            {
                sb.AppendLine("<div class=\"listbox\">");
                sb.AppendLine(string.Format("<pan class=\"listnum\">{0}</pan>", (i + 1).ToString()));
                sb.AppendLine("<span class=\"wbtn_round touxiang\">");
                sb.AppendLine(string.Format("<img src=\"{0}\" >", UserList[i].HeadImg));
                sb.AppendLine("</span>");
                sb.AppendLine(string.Format("<span class=\"name\">{0}</span>", UserList[i].ShowName));
                sb.AppendLine(string.Format("<span class=\"score\">{0}</span>", UserList[i].SpreadCount));
                sb.AppendLine("</div>");

            }
            if (UserList.Count == 0)
            {
                sb.AppendFormat("<h5 class=\"nodata\">暂时没有数据</h5>");
            }
            Response.Write(sb.ToString());
        
        %>
        </div>
        <!-- mainlist -->
        <!-- footerbar -->
</body>
</html>
