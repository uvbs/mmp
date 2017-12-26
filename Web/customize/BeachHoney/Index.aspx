<%@ Page Title="" Language="C#" MasterPageFile="~/customize/BeachHoney/Master.Master"
    AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.BeachHoney.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
沙滩宝贝-首页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        body
        {
            background-color: White;
        }

    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="image_single">
        <img src="images/index_01.jpg" alt="" title="" border="0" />
        <img src="images/index_02.jpg" alt="" title="" border="0" />
        <img src="images/index_03.jpg" alt="" title="" border="0" /></div>
    <div class="page_padding0">
        <div class="index-txt radius8">
            <p>
                一、价值8000元的沙巴+美人鱼5天4日游（3个名额）</p>
            <p>
                二、Victoria Gossip品牌包包100支（总价值10万元）</p>
            <p>
                三、M-Studio 兑换券（总价值40,000元）</p>
            <p>
                四、热带风暴电子门票1140张总价值228,000元）</p>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    //分享
    var shareTitle = "前1000名沙滩宝贝选手热带风暴电子门票领取";
    var shareDesc = "前1000名沙滩宝贝选手热带风暴电子门票领取";
    var shareImgUrl = "http://<%=Request.Url.Host %>/customize/beachhoney/images/match_01.jpg";
    var shareLink = window.location.href;
    //分享
</script>
</asp:Content>
