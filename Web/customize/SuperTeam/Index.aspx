<%@ Page Title="" Language="C#" MasterPageFile="~/customize/SuperTeam/Master.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SuperTeam.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
首页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/homePage.css" rel="stylesheet" type="text/css" />
   <style>
   
   .wrapHomePage .content .imgStyle {
    
      bottom: 50px;
   
}

   </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">

 <div class="wrapHomePage">
        <div class="content">
            <img class="imgShouye" src="image/homepage.png" >
            <img class="imgStyle" src="Image/qrcode.png" style="width:100px;height:auto;text-align:center;margin-left:65%;"/>
           
        </div>
        <div class="bottom">
            <div class="row">
                <div class="col borderLine" onclick="window.location.href='Index.aspx'">
                    <i class="iconfont icon-shixinshouye shouye"></i>
                </div>
                <div class="col col-80">
                    <div class="row">
                        <div class="col borderLine" onclick="window.location.href='Rule.aspx'">
                            参赛细则
                        </div>
                        <div class="col borderLine" onclick="window.location.href='SignUp.aspx'">
                            <%=signUpText %>
                        </div>
                        <div class="col" onclick="window.location.href='Area.aspx'">
                            为Team投票
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    //分享
    var shareTitle = "SuperTeam";
    var shareDesc = "SuperTeam";
    var shareImgUrl = "http://<%=Request.Url.Host %>/customize/SuperTeam/image/logo.jpg";
    var shareLink = window.location.href;
    //分享
</script>
</asp:Content>
