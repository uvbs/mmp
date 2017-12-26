<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    首页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
   
    <style>
        #tab1
        {
            margin-bottom: 50px;
        }
        
        img
        {
            max-width: 100%;
        }
        #tabsmenu img
        {
            width: 50px;
            height: 50px;
            border-radius: 55px;
        }
        .image_single
        {
            padding-top: 0px;
        }
         .image_single:last-child{margin-bottom:50px;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <%ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();

      List<ZentCloud.BLLJIMP.Model.Slide> SlideList = bllMall.GetList<ZentCloud.BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='0' Order by Sort DESC", bllMall.WebsiteOwner));//轮播图片
        
      List<ZentCloud.BLLJIMP.Model.Slide> BannerList = bllMall.GetList<ZentCloud.BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='2' Order by Sort DESC", bllMall.WebsiteOwner));//banner
        
        
      List<ZentCloud.BLLJIMP.Model.Slide> IconList = bllMall.GetList<ZentCloud.BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type='1' Order by Sort DESC", bllMall.WebsiteOwner));//icon

    %>
    <div id="header">
        <div class="gohome">
            <a href="Index.aspx">
                <img src="images/index6.png" /></a></div>
        <div class="homelogo">
            <a href="Index.aspx">
                <img src="images/index7.png" /></a></div>
        <div class="gomenu">
            <a href="Category.aspx">
                <img src="images/index8.png" />
            </a>
        </div>
    </div>

       <div class="panels_slider">
        <ul class="slides">
        <%foreach (var item in SlideList){%>
              <li><a href="<%=item.Link %>"><img src="<%=item.ImageUrl %>"  title="" border="0" /></a></li>
         <% }%>
        

         </ul>
        </div>


    <div class="menu4">
        <ul id="tabsmenu">
            <%foreach (var item in IconList.Skip(0).Take(5))
              {
                  Response.Write(string.Format("<li id=\"li1\" >"));
                  Response.Write(string.Format("<a href=\"{0}\"><img src=\"{1}\" /><span>{2}</span></a>", item.Link, item.ImageUrl, item.LinkText));
                  Response.Write("</li>");

              }%>
        </ul>
    </div>
    <div id="tab1" class="tabcontent">
        <%
            //banner
            for (int i = 0; i < BannerList.Count; i++)
            {
                Response.Write("<div class=\"image_single\">");
                Response.Write(string.Format("<a href=\"{0}\">", BannerList[i].Link));
                Response.Write(string.Format("<img src=\"{0}\"  title=\"\" border=\"0\" />", BannerList[i].ImageUrl));
                Response.Write("</a>");
                Response.Write("</div>");
            }
        %>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script src="js/jquery.flexslider.js" type="text/javascript"></script>
<script type="text/javascript">
    $('.panels_slider').flexslider({
        animation: "slide",
        directionNav: false,
        controlNav: true,
        animationLoop: true,
        slideToStart: 0,
        slideshowSpeed: 3000,
        animationDuration: 300,
        slideshow: true
    });
</script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "土澳网，精心甄选源自澳洲商品的电商平台",
                desc: "土澳网，精心甄选源自澳洲商品的电商平台",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>/customize/tuao/images/logo.png"
            })
        })
    </script>
    <script>
            var _hmt = _hmt || [];
            (function () {
                var hm = document.createElement("script");
                hm.src = "//hm.baidu.com/hm.js?815fb228f9eb74b02fe9360ded639528";
                var s = document.getElementsByTagName("script")[0];
                s.parentNode.insertBefore(hm, s);
            })();
</script>
</asp:Content>
