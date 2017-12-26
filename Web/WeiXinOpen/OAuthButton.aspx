<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="OAuthButton.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.WeiXinOpen.OAuthButton" %>

<html>
<head>
    <link href="/css/buttons2.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <%
        Response.Write("公众号名称:" + tagetWebsiteInfo.AuthorizerNickName);
        Response.Write("<br/>");
        Response.Write("账号类型:");
        switch (tagetWebsiteInfo.AuthorizerServiceType)
        {
            case "0":
                Response.Write("订阅号");
                break;
            case "1":
                Response.Write("老账号升级的订阅号");
                break;
            case "2":
                Response.Write("服务号");
                break;
            default:
                break;
        }
        Response.Write("<br/>");

        Response.Write("账号认证:");
        switch (tagetWebsiteInfo.AuthorizerVerifyType)
        {
            case "-1":
                Response.Write("未认证");
                break;
            case "0":
                Response.Write("微信认证");
                break;
            case "1":
                Response.Write("新浪微博认证");
                break;
            case "2":
                Response.Write("腾讯微博认证");
                break;
            case "3":
                Response.Write("已资质认证通过但还未通过名称认证");
                break;
            case "4":
                Response.Write("已资质认证通过、还未通过名称认证，但通过了新浪微博认证");
                break;
            case "5":
                Response.Write("已资质认证通过、还未通过名称认证，但通过了腾讯微博认证");
                break;
            default:
                break;
        }
        Response.Write("<br/>");
        Response.Write("<br/>");
    %>
    <a href="http://<%=OauthDomain%>/weixinopen/oauth.aspx" class="button button-3d button-action button-pill">
        <%if (string.IsNullOrEmpty(tagetWebsiteInfo.AuthorizerAppId))
          {


              Response.Write("你还未授权,现在去授权");
          }
          else
          {


              Response.Write("你已经授权,点击重新授权");
          }%>
    </a>
</body>
</html>
