<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QCodePage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.QCodePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=pageTitle %> </title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="css/fenxiao.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            background-color: #f5f5f5;
        }

        html, body, .qrcodeBox {
            height: 100%;
        }

        .qrcodeBox {
            text-align: center;
            background-size: cover;
            background-image: url(<%=website.DistributionShareQrcodeBgImg%>?x-oss-process=image/resize,w_553,h_986);
        }

        .wrapQrcode {
            position: absolute;
            bottom: 16%;
            width: 100%;
            vertical-align: middle;
            text-align: left;
        }


            .wrapQrcode h2 {
                margin-top: 16px;
                font-size: 16px;
                font-weight: bolder;
                margin-left: -90px;
            }

        .codepic {
            width: 40%;
            position: relative;
            left: -106px;
        }

        .wrapUserInfo {
            padding-top: 48px;
        }

            .wrapUserInfo img {
                width: 68px;
                height: 68px;
                border-radius: 50px;
            }


        .pTop8 {
            padding-top: 8px;
            position: relative;
            left: -35px;
        }

        .userName {
            font-size: 18px;
            color: #640101;
            font-weight: bolder;
        }

        .pTop8User {
            padding-top: 8px;
            text-align: center;
        }
    </style>
</head>
<body>
    <div class="qrcodeBox">
        <div class="wrapUserInfo">
            <div>
                <%
                    if (config.IsHideHeadImg == 0)
                    {
                %>
                <img src="<%=bllUser.GetUserDispalyAvatar(currUser) %>" />
                <%
                    }
                    if (config.WXNickShowPosition == 0)
                    {
                %>
                <div class="pTop8User">

                    <%if (currUser.WebsiteOwner == "lanyueliang")
                      { %>
                    <span class="userName" style="color: #fff !important;">我是&nbsp;&nbsp;<%=bllUser.GetUserDispalyName(currUser) %> </span>
                    <%}
                      else
                      { %>
                    <span class="userName">我是&nbsp;&nbsp;<%=bllUser.GetUserDispalyName(currUser) %> </span>
                    <%} %>
                </div>
                <%
                    }
                %>
            </div>

        </div>
        <div class="wrapQrcode">
            <img id="img1" src="<%=qrcondeUrl %>" class="codepic">

            <%
                if (config.WXNickShowPosition == 1)
                {
            %>
            <div class="pTop8">

                <%if (currUser.WebsiteOwner == "lanyueliang")
                  { %>
                <span class="userName" style="color: #fff !important;">我是&nbsp;&nbsp;<%=bllUser.GetUserDispalyName(currUser) %> </span>
                <%}
                  else
                  { %>

                <span class="userName">我是&nbsp;&nbsp;<%=bllUser.GetUserDispalyName(currUser) %> </span>

                <%} %>
            </div>
            <%
                } 
                
            %>

            <%--<h2 class="text">
                长按此图，识别图中二维码</h2>--%>
        </div>
    </div>
</body>
</html>
