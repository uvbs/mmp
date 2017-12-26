<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="MyCenter.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.MyCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    个人中心</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmall20150110.css" rel="stylesheet" type="text/css" />
    <style>
        .h1, h2, h4, h5, h6
        {
            clear: none;
        }
        .m_header img
        {
            border-radius: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="m_header">
        <%
            string HeadImg = currentUserInfo.WXHeadimgurlLocal;
            if (string.IsNullOrEmpty(HeadImg))
            {
                HeadImg = "/img/offline_user.png";
            }

            Response.Write(string.Format("<img src=\"{0}\"  class=\"portrait\">", HeadImg));
        %>
        <h2 class="name">
        <% 
            if (!string.IsNullOrEmpty(currentUserInfo.WXNickname))
            {
                Response.Write(currentUserInfo.WXNickname);
            }
         %>
        </h2>
        <a id="btnUpdateHeadNick" href="javascript:" class="btn orange">更新头像昵称</a>
    </div>
    <div class="m_listbox">
        <a href="Personinfo.aspx" class="list">
            <span class="mark purpler"><span class="icon people"></span></span>
            <h2>个人资料</h2>
            <span class="righticon"></span>
        </a>
         <a href="ScoreRecord.aspx" class="list">
            <span class="mark purple"><span class="icon vipscore"></span></span>
            <h2>积分</h2>
            <span class="righticon"></span>
        </a>
    </div>

    <div class="m_listbox">
        <a href="MyOrderList.aspx" class="list">
            <span class="mark green"><span class="icon order"></span></span>
            <h2>我的订单</h2>
            <span class="righticon"></span>
        </a>
        <a href="MyAddressList.aspx" class="list">
            <span class="mark blue"><span class="icon address"></span></span>
            <h2>收货地址</h2>
            <span class="righticon"></span>
        </a>
        <a href="MyProductCollect.aspx" class="list">
           <span class="mark blue"><span class="icon vipscore"></span></span>
           
            <h2>我的收藏</h2>
            <span class="righticon"></span>
        </a>
        <%if ((!string.IsNullOrEmpty(currentUserInfo.TagName)) && (currentUserInfo.TagName.StartsWith("JXS")))
          {%>
             <a href="CouponMgr.aspx" class="list">
            <span class="mark yellow"><span class="icon people"></span></span>
            <h2>优惠券管理</h2>
            <span class="righticon"></span>
        </a>
         <%} %>
        
    </div>


</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#btnUpdateHeadNick").click(function () {
                $.ajax({
                    type: 'post',
                    url: '/Handler/OpenGuestHandler.ashx',
                    data: { Action: 'UpdateToLogoutSessionIsRedoOath' },
                    dataType: 'json',
                    success: function (result) {
                        window.location.href = "MyCenter.aspx";
                    }
                });



            });
        })
   
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
