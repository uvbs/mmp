<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="ProductList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    商品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        #header
        {
            position: fixed;
        }
        #productList
        {
            margin-bottom: 50px;
        }
        .index_container1
        {
            margin: 0;
        }
        .tabsmenu4 li:last-child
        {
            margin-bottom: 50px;
        }
        .menu
        {
            position: fixed;
            top: 50px;
            right: 0;
            z-index: 999999999;
            width: 100px;
            background: #fff;
            display: none;
            border: 1px #ccc solid;
            padding: 5px 5px 5px 5px;
        }
        .menu table
        {
            width: 100%;
        }
        .menu table tr
        {
            border-bottom: 1px #ccc solid;
            height: 30px;
        }
        .menu table tr td
        {
            vertical-align: center;
            margin-top: 2px;
        }
        .menu table tr:last-Child
        {
            border-bottom: none;
        }
        #productList:last-Child
        {
            margin-bottom: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div id="header">
        <div class="gohomemenu">
            <a href="javascript:history.go(-1)">
                <img src="images/item1.png" /></a></div>
        <div class="hometext" style="text-align: left">
            <%
                if (!string.IsNullOrEmpty(Request["cid"]))
                {
                    ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
                    System.Text.StringBuilder SbWhere = new StringBuilder();
                    SbWhere.AppendFormat(" WebsiteOwner='{0}' And AutoID={1}", bllMall.WebsiteOwner, Request["cid"]);
                    ZentCloud.BLLJIMP.Model.WXMallCategory Category = bllMall.Get<ZentCloud.BLLJIMP.Model.WXMallCategory>(SbWhere.ToString());
                    if (Category != null)
                    {
                        Response.Write(Category.CategoryName);
                    }
                }
             
            %>
        </div>
        <div class="gomenu">
            <a href="javascript:void(0)" onclick="$('.menu').toggle();">
                <img src="images/item2.png" />
            </a>
        </div>
    </div>
    <div class="menu" onclick="$('.menu').toggle();">
        <table>
            <tr>
                <td>
                    <span onclick="Sort('priceasc');">价格从低到高</span>
                </td>
            </tr>
            <tr>
                <td>
                    <span onclick="Sort('pricedesc');">价格从高到低</span>
                </td>
            </tr>
            <tr>
                <td>
                    <span onclick="Sort('pv');">浏览量</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="tab1" class="tabcontent">
        <div class="tabsmenu4">
            <ul id="productList">
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="js/tuao.js" type="text/javascript"></script>
    <script type="text/javascript">
        var keyWord = "<%=Request["KeyWord"] %>";
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
</asp:Content>
