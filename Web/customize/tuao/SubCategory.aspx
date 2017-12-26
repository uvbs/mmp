<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="SubCategory.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.SubCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    分类
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        .gohomemenu img
        {
            width: auto;
        }
        #header
        {
            position: relative;
        }
        #tabsmenu img
        {
            width: 50px;
            height: 50px;
            border-radius: 55px;
        }
        .subcategory
        {
            margin-bottom: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <%ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
      List<ZentCloud.BLLJIMP.Model.Slide> IconList = bllMall.GetList<ZentCloud.BLLJIMP.Model.Slide>(string.Format("WebsiteOwner='{0}' And Type=1 Order by Sort DESC", bllMall.WebsiteOwner));
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
    <ul class="subcategory">
        <%
            System.Text.StringBuilder SbWhere = new StringBuilder();
            SbWhere.AppendFormat(" WebsiteOwner='{0}' And PreID={1}", bllMall.WebsiteOwner, Request["cid"]);
            List<ZentCloud.BLLJIMP.Model.WXMallCategory> SecondCategory = bllMall.GetList<ZentCloud.BLLJIMP.Model.WXMallCategory>(SbWhere.ToString());
            foreach (var item in SecondCategory)
            {
                Response.Write("<li>");
                Response.Write(string.Format("<a href=\"ProductList.aspx?cid={0}\"><img src=\"{1}\" style=\"max-width:100%;\" /></a> ", item.AutoID, item.CategoryImg));
                Response.Write("</li>");
            }

                                    
        %>
        
    </ul>
    <br />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
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
</asp:Content>
