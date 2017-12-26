<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="Card.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Card" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">会员卡</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section class="box">

    <%if (boolIsSupplementUserInfo){%>
        <div class="m_card m_nocard">
        <div class="card" style="background-image: url(/img/wxmall/cardbg.png)">
            <span class="cardicon"></span>
            <h2 class="shopname"><%=currentWebsiteInfo.WXMallName==null?"":currentWebsiteInfo.WXMallName %></h2>
        </div>
        <div class="overshadow"></div>
    </div>
    <h2 class="m_cardtext">消费前出示此卡获优惠<br/>
    
    <a href="/App/Cation/Wap/Mall/Personinfo.aspx?redirecturl=/App/Cation/Wap/Mall/Card.aspx" class="btn orange">免费申请</a></h2>

		 
	<%} %>
    <%else{%>
	

        <div class="m_card" onclick="window.location.href='CardCode.aspx'">
        <div class="card" style="background-image: url(/img/wxmall/cardbg.png)">
            <span class="cardicon"></span>
            <h2 class="shopname"><%=currentWebsiteInfo.WXMallName==null?"":currentWebsiteInfo.WXMallName %></h2>
            <span class="codeicon"></span>
            <span class="cardinfo"><%=userInfo.TrueName==null?"":userInfo.TrueName %><br/>No.<%=userInfo.AutoID%></span>
        </div>
    </div>
    <h2 class="m_cardtext">消费前出示此卡获优惠</h2> 

          
	<%} %>
    
    <div class="m_listbox">
        <a href="CardShop.aspx" class="list">
            <span class="mark purple"><span class="icon address"></span></span>
            <h2>查看分店</h2>
            <span class="righticon"></span>
        </a>
        <div href="#" class="list biglist">
            <span class="mark redd"><span class="icon card"></span></span>
            <h2>会员专享</h2>
            <div class="concent">
            <p><%=currentWebsiteInfo.WXMallMemberCardMessage == null ? "" : currentWebsiteInfo.WXMallMemberCardMessage.Replace("\n","<br/>")%></p> 
            </div>
        </div>
    </div>

    <div class="backbar">
        <a href="MyCenter.aspx" class="back"><span class="icon"></span></a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
</asp:Content>
