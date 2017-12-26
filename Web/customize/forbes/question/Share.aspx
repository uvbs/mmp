<%@ Page Title="" Language="C#" MasterPageFile="~/customize/forbes/question/Master.Master" AutoEventWireup="true" CodeBehind="Share.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.question.Share" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="css/share.css" rel="stylesheet" type="text/css" />
    <style>
    #btnShare{font-size:18px;}
    .wrapForbesChooseResult .erweima .erweimaImg {
  width:48%;
}
.color29221E{margin-left:5px;margin-right:5px;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<div class="wrapForbesChooseResult">
    <div class="medal">
            <img class="medalImg" src="<%=CurrentUserInfo.WXHeadimgurlLocal %>" />
    </div>
    <div class="scoreWord">
        本次答题得分等级是
    </div>
    <div class="score">
        <%=level%>
    </div>
    <div class="card">
        <div class="item item-text-wrap">
            评语：<%=levelDesc %>
        </div>

    </div>
            <div class="shareBtn">
        <button  class="button button-block button-positive" onclick="window.location.href='http://forbes.comeoncloud.net/App/Lottery/wap/ScratchV1.aspx?id=470246'">
            参与抽奖
        </button>
    </div>
    <br />
    <div class="shareBtn">
        <button id="btnShare" class="button button-block button-positive">
            一键分享至朋友圈
        </button>
    </div>
    <div class="desc pTop8 color29221E">
      关注【创富荟（微信号：forbeswealth）】，参与模拟答题活动，答题完成后点击评语下面的【参与抽奖】即有机会赢取《福布斯》杂志中文版半年免费赠阅。抽奖分三轮进行：8月24日-9月30日，10月1日-10月31日，11月1日-11月30日，在9月30日、10月31日、11月30日三天分别抽出10位幸运用户。
    </div>
    <div class="erweima">
        <img class="erweimaImg" src="images/erweima.jpg"/>
    </div>
    <div class="adv">
        <img class="advImg" src="images/adv.jpg"/>

    </div>
</div>
          <div style="width: 100%; height: 100%; display: none; background: #000; opacity: 0.7;
            position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
            &nbsp;
        </div>
        <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; height: 100%;text-align: right;
            display: none;" id="sharebox">
            <img src="images/sharetip.png" width="100%" />
        </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

<script type="text/javascript">
    $(document).ready(function () {
        $("#btnShare").click(function () {
            $("#sharebg,#sharebox").show();
            $("#sharebox").css({ "top": $(window).scrollTop() })
        });

        $("#sharebg,#sharebox").click(function () {
            $("#sharebg,#sharebox").hide();
        });



    });
    </script>
 <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
<script type="text/javascript">
    wx.ready(function () {

        wxapi.wxshare({
            title: '我击败了全国<%=percent%>%的理财师。你也来参加吧',
            desc: '福布斯',
            link: 'http://<%=Request.Url.Host%>/customize/forbes/question/Index.aspx',
            imgUrl: 'http://forbes.comeoncloud.net/customize/forbes/images/login_logo.png'
        }

        )
    })
</script>

</asp:Content>
