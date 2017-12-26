<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignInV1.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.SignInV1" %>

<!DOCTYPE html>
<html>
<head>
    <title>签到</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .sigindata
        {
            width: 100%;
            box-sizing: border-box;
            padding: 10px;
            position: absolute;
            top: 50%;
            font-size: 20px;
            left: 0px;
        }
    </style>
</head>
<body>
    <section class="box">
    <div class="notebox">
        <div class="noteinfo">签到成功</div>
        <p class="text"><span class="icon"> </span>您已经成功签到</p>
        
        <div class="rightbox">
           
           <span class="righticon">
            <span class="icon"></span>
            </span>
        </div>
             
    </div>

    <div class="sigindata">
    <h1><%=juActivity.ActivityName%></h1><br />
    姓名:<%=data.Name%><br />
    手机:<%=data.Phone%><br />
    <%
        Type type = data.GetType();
        foreach (var item in Mapping)
        {
            if (item.ExFieldIndex > 0)
            {
                Response.Write(string.Format("{0}:{1}<br/>", item.MappingName, type.GetProperty("K" + item.ExFieldIndex.ToString()).GetValue(data, null)));
            }

        } %>


    </div>
</section>
</body>
<script src="/Scripts/zepto.min.js" type="text/javascript"></script>
</html>
