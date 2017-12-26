<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/WXMall.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.Mall.Show" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <header class="TopToolbar">
		<a href="" class="BtnBack" onclick="javascript:history.go(-1);return false;">返回</a>
		<a href="list.aspx" class="BtnGoHome">&nbsp;</a>
		<span id="lbPName">商品详情</span>
	</header>
    <div class="ProInfo">
        <div class="ProRecommendImg">
            <%--<img src="http://wd.geilicdn.com/vshop882137-1392008118-1.jpg?w=480&h=360&cp=1" />--%>
        </div>
        <div class="ProDeatils">
            <p class="ProDescription">
                <%--泰国ele睡眠面膜保湿补水美白面膜细嫩滑面膜.ELE含有丰富竹炭，从而消除在你脸上的毒素从而达到清洁皮肤使皮肤光滑的目的。 刺激血液循环，增加氧气的皮肤，让脸部进行有氧呼吸。即时提升肌肤的弹性，深层美白，极度深层护理的效果是普通面膜的10倍
                增强和抑制胶原蛋白的破坏 有助于减少皱纹，抗氧化剂 平滑滋润肌肤，减少黑眼圈。消除斑纹 舒缓肌肤，在睡眠期间为肌肤导入源源不断的水份与活力，在睡眠中为你的肌肤保驾护航
                ELE属于睡眠面膜睡前涂抹面霜，放置约15-20分钟。可以清洗或翌日清洗。 面霜将会在睡眠期间深度渗透到脸部皮肤组织，深入滋养肌肤。在早晨醒来。 你的脸会会立刻感觉到皮肤光滑明亮白皙水嫩。--%>
            </p>
            <p class="ProPrince">
                <%--￥168.00--%>
            </p>
        </div>
    </div>
    <a id="Car" href="Order.aspx"><em id="CarBg"></em><em id="CarIcon"></em><i id="CarCount">
        </i> </a>
    <div class="OpeateBox">
        <a id="btnAddInOrder" href="javascript:;" class="BtnBottomOpeate BtnLeft">加入购物车</a> <a href="javascript:;"
          id="btnBuyNow"  class="BtnBottomOpeate BtnRight">立即购买</a>
    </div>
</asp:Content>
