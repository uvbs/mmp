<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="Category.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    分类
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        #header
        {
            position: relative;
        }
        .image_single:last-child{margin-bottom:50px;}
        .tabcontent{margin-bottom:50px;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div id="header">
        <div class="gohome">
            <a href="Index.aspx">
                <img src="images/index6.png" /></a></div>
        <div class="homelogo">
            <a href="Index.aspx">
                <img src="images/index7.png" /></a></div>
        <div class="gomenu">
            <a href="">
                <img src="images/index8.png" />
            </a>
        </div>
    </div>
    <div class="div-search form">
        <div class="top-search radius20">
            <form method="post" action="ProductList.aspx" id="form1">
            <img src="images/search.png" onclick="$('#form1').submit();" />
            <input type="text" name="KeyWord" placeholder="商品名称" />
            </form>
        </div>
    </div>
    <div id="tab1" class="tabcontent">
        <%ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
          string KeyWord = Request["KeyWord"];
          System.Text.StringBuilder SbWhere = new StringBuilder();
          SbWhere.AppendFormat(" WebsiteOwner='{0}' And (PreID is null or PreID=0)", bllMall.WebsiteOwner);

          if (!string.IsNullOrEmpty(KeyWord))
          {
              SbWhere.AppendFormat(" And CategoryName like '%{0}%'", KeyWord);
          }

          List<ZentCloud.BLLJIMP.Model.WXMallCategory> FirstCategory = bllMall.GetList<ZentCloud.BLLJIMP.Model.WXMallCategory>(SbWhere.ToString());

          for (int i = 0; i < FirstCategory.Count; i++)
          {

              Response.Write("<div class=\"image_single\">");
              Response.Write(string.Format("<a href=\"SubCategory.aspx?cid={0}\">", FirstCategory[i].AutoID));
              Response.Write(string.Format("<img src=\"{0}\"  title=\"\" border=\"0\" />", FirstCategory[i].CategoryImg));
              Response.Write("</a>");
              Response.Write("</div>");

          }
        %>
    </div>
    <!--Page 1 content-->
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
