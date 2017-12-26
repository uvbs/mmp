<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/WXMall.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.Mall.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%
        
        ZentCloud.BLLJIMP.BLLUser userBll = new ZentCloud.BLLJIMP.BLLUser("");
        ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo = userBll.GetUserInfo(ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner);
    %>
    <%if (string.IsNullOrWhiteSpace(currWebSiteUserInfo.WXMallBannerImage))
      { %>
    <header class="MallBanner" style="background-image: url(/img/wxmall/top.jpg);">
    <%}
      else
      {
          Response.Write(
              string.Format(@"<header class=""MallBanner"" style=""background-image: url({0});"">",
                currWebSiteUserInfo.WXMallBannerImage
                )
             );
      } %>
    
    </header>
    <div class="TopCategory">
        <%=sbCategory.ToString()%>  
    </div>
    <div class="ProListBox">

        
        <ul class="ProList">
           
        </ul>
        <div class="Clear">
        </div>
    </div>
    <footer>
	    <a id="Car" href="Order.aspx">
		    <em id="CarBg"></em>
		    <em id="CarIcon"></em>
		    <i id="CarCount">0</i>
	    </a>
    </footer>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderScript" runat="server">
<script src="/Scripts/wxmall/index.js" type="text/javascript"></script>
</asp:Content>
