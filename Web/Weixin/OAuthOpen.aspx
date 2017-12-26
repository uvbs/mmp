<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="OAuthOpen.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.OAuthOpen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        h1
        {
            margin-left: 50px;
        }
        .button-rounded
        {
            margin-left: 50px;
        }
        .centent_r_btm
        {
            min-height: 600px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&gt;微信开放平台授权
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <%if (string.IsNullOrEmpty(currentWebsiteInfo.AuthorizerAppId))
      {
          Response.Write("<h1>你还未授权</h1><br/>");
      }
      else
      {
          Response.Write("<h1>你已经授权</h1><br/>");
      }%>
    <a href="http://oauth.comeoncloud.net/weixinopen/oauth.aspx?callbackdomain=<%=Request.Url.Host%>"
        class="button button-3d button-primary button-rounded">
        <%if (string.IsNullOrEmpty(currentWebsiteInfo.AuthorizerAppId))
          {

              Response.Write("现在去授权");
          }
          else
          {

              Response.Write("重新授权");
          }%>
    </a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
</asp:Content>
