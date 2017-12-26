<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true"
    CodeBehind="MyCenter.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.MyCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    个人中心</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="m_header">
        <%
            string headimg = userInfo.WXHeadimgurl;
            if (string.IsNullOrEmpty(headimg))
            {
                headimg = "/img/offline_user.png";
            }
            Response.Write(string.Format("<img src=\"{0}\"  class=\"portrait\">", headimg));
        %>
        <h2 class="name">
        <% 
            string str = "";
            if (!string.IsNullOrWhiteSpace(userInfo.WXNickname))
                str = userInfo.WXNickname;
            else if (!string.IsNullOrWhiteSpace(userInfo.UserID))
                str = userInfo.UserID;
            else if (!string.IsNullOrWhiteSpace(userInfo.Phone))
                str = userInfo.Phone;
            Response.Write(str);
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
        <a href="Card.aspx" class="list">
            <span class="mark purpleb"><span class="icon card"></span></span>
            <h2>会员卡</h2>
            <span class="righticon"></span>
        </a>
         <a href="ScoreManage.aspx" class="list">
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

        <%if (new ZentCloud.BLLJIMP.BLL().GetWebsiteInfoModel().IsDistributionMall == 1)
          {%>
        <a href="Distribution/Index.aspx" class="list">
            <span class="mark yellow"><span class="icon people"></span></span>
            <h2>我的分销</h2>
            <span class="righticon"></span>
        </a>
        <% } %>
    </div>

    <div class="toolbar">
        <a href="<%=WXMallIndexUrl%>" class="left "><span class="homeicon"></span><br/>首页</a>
        <%if (!currWebSiteInfo.MallTemplateId.Equals(1))
          {%>
          <a href="/App/Cation/Wap/Mall/Orderv1.aspx" id="cart"><span class="carticon"></span><span class="cartnum" style="display:block;">0</span></a>
         <% } %>
        <a href="#" class="right current"><span class="mycenter"></span><br/>个人中心</a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/wxmall/initv1.js" type="text/javascript"></script>
    <script type="text/javascript">
        var type = '<%=websiteInfo.MallType %>'
        function Init() {
            if (type == '1') {
                $("#cart").hide();
            }
        }
        $(function () {
            Init();
            $("#btnUpdateHeadNick").click(function () {
                $.ajax({
                    type: 'post',
                    url: '/Handler/OpenGuestHandler.ashx',
                    data: { Action: 'UpdateToLogoutSessionIsRedoOath' },
                    success: function (result) {
                        window.location.href = "MyCenter.aspx";
                    }
                });



            });

        })
   
    </script>
</asp:Content>
