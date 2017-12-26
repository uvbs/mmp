<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MySignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.HaiMa.MySignUp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <title>我的报名</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <link type="text/css" rel="stylesheet" href="css/basic.css" />
    <style>
        .index_container3
        {
            text-align: left;
            font-size: 16px;
            line-height: 30px;
        }
        p
        {
            margin-left: 0px;
            margin-right: 0px;
        }
        .form_div
        {
            margin-left: 4%;
            margin-right:4%;
            width: 90%;
            padding:5 0 5 5
        }
        body
        {
          background: url(images/bgmysignup.jpg);
          background-size:cover;
            
         }
        
    </style>
</head>
<body>
    <!--Page 1 content-->
    <div class="pages_container  sliderbg10">
        <img src="images/logo.png" style="width: 100%;" />
        <div class="index_container3">
            <%
                int Count = 1;
                StringBuilder SbResp = new StringBuilder();
                switch (CurrentUserInfo.Postion)
                {
                    case "销售顾问":
                        Count = 2;
                        SbResp.AppendFormat("<div class=\"form_div radius4\">1.{0}</div>", CurrentUserInfo.Ex5);
                        SbResp.AppendFormat("<div class=\"form_div radius4\">2.{0}</div>", CurrentUserInfo.Ex6);
                        break;
                    case "销售经理":
                        SbResp.AppendFormat("<div class=\"form_div radius4\">1.{0}</div>", CurrentUserInfo.Ex5);
                        break;
                    case "市场经理":
                        SbResp.AppendFormat("<div class=\"form_div radius4\">1.{0}</div>", CurrentUserInfo.Ex5);
                        break;

                }%>
            <br />
            <div class="form_div radius4">
                <b>恭喜您成功报名海马汽车2015经销商营销竞赛,系统随机为您抽取了<%=Count %>个题目:</b>
            </div>
            <%
                Response.Write(SbResp.ToString());
            %>
            <div class="form_div radius4">
                注意:<br />
                1.请根据上述题目,分别录制视频,共计<%=Count %>段。
                <br />
                2.请于2015年7月11日17点前完成拍摄,并以邮件附件的形式发送到竞赛专用邮箱:<br />
                jyczpt@haima.com。
            </div>
        </div>
    </div>
</body>
<script type="text/javascript" src="js/jquery-1.10.1.min.js"></script>
<script src="js/comm.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "我的报名",
            desc: "海马精英成长平台",
            //link: '', 
            imgUrl: "http://<%=Request.Url.Host%>/customize/HaiMa/images/logo.jpg"
        })
    })
</script>
</html>
