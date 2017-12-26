<%@ Page Language="C#" AutoEventWireup="true" EnableSessionState="ReadOnly" CodeBehind="beforCanceled.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SportsShow.beforCanceled" %>

<!DOCTYPE html>

<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0">
    <meta name="format-detection" content="telephone=no">
    <title>兑奖页</title>
    <link href="/lib/ionic/ionic.css" rel="stylesheet">
    <link href="/Plugins/LayerM/need/layer.css" rel="stylesheet" />
    <link href="css/base.css" rel="stylesheet">
    <link href="css/canceled.css" rel="stylesheet">
    <script src="/lib/jquery/jquery-2.1.1.min.js"></script>
    <script src="/Plugins/LayerM/layer.m.js"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script src="js/SportsShow.js"></script>
</head>
<body class="bg">
<div style="width: 100%; height: 1500px; opacity: 0.7; position: absolute; top: 0px; left: 0px; z-index: 999999; text-align: right; display: none; background: rgb(0, 0, 0);" id="sharebg">
    &nbsp;
</div>
<div style="position: absolute; z-index: 1000000; right: 0px; width: 100%; height: 1500px; text-align: right; top: 700px; display: none;" id="sharebox">
    <img src="images/footer/sharetip.png" class="full-image">
</div>
    <header class="content">
        <div class="row">
            <div class="col-67 col-offset-4">
                <img src="images/header/logo.png" class="full-image">
            </div>
        </div>
        <div class="row">
            <div class="col">
                <img src="images/header/header-bg.png" class="full-image header-bg">
            </div>
        </div>
    </header>
    <%for (int i = 0; i < ListQuestionResult.Count; i++)
      { %>
    <section class="content">
        <div class="row">
            <div class="col">
                <div class="main-content">
                    <p class="font-bold">第<%= GetNumZn(i) %>次测试：</p>
                    <p class="text-center">我的体商指数：<span class="test-number"><%= ListQuestionResult[i].TotalScore %></span></p>
                    <p class="text-center font-bold test-result"><span>DUANG!</span><%= GetScoreReview(ListQuestionResult[i].TotalScore) %></p>

                    <%
          bool haveMenPiao = ListQuestionResult[i].CreateDate < Convert.ToDateTime("2015-11-05");
          if (haveMenPiao)
          {%>
                    <div class="row">
                        <div class="col-50 margin-top-12 fix-width-50">
                            <p class="font-bold">恭喜你获得：</p>
                            <p class="font-bold">体博会门票一张</p>
                        </div>
                        <div class="col-50 fix-width-50">
                            <p class="text-center">核销码</p>
                            <%if (string.IsNullOrWhiteSpace(ListQuestionResult[i].CancelCode))
                              { %>
                            <div class="cancel-code-befor content">
                                <input id="txtCode<%=i %>" type="password" class="code-input">
                                <div class="code-switch" onclick="setValue('<%=i %>')">确认</div>
                            </div>
                            <%}
                              else
                              { %>
                            <div class="cancel-code text-center">
                                已核销
                            </div>
                            <%}%>
                        </div>
                    </div>
                    <div class="row">
                        <p class="code-info col text-center">入场时出示本页面，由工作人员填写核销码</p>
                    </div>
                    <%}
          bool haveGift = !string.IsNullOrWhiteSpace(ListQuestionResult[i].GiftCode);
          if (haveGift && haveMenPiao)
          {
                    %>
                    <div class="row main-hr">
                        <strong class="col text-center">现场有一份神秘礼品等您来领取</strong>
                    </div>
                    <div class="row main-footer">
                        <p class="col text-center p-reformat main-footer">兑换编码：<%=ListQuestionResult[i].GiftCode %></p>
                    </div>
                    <%}
          else if (haveGift && !haveMenPiao)
          {%>
                    <div class="row margin-top-12">
                        <strong class="col text-center">现场有一份神秘礼品等您来领取</strong>
                    </div>
                    <div class="row main-footer margin-bottom-12">
                        <p class="col text-center p-reformat main-footer">兑换编码：<%=ListQuestionResult[i].GiftCode %></p>
                    </div>
                    <%}
          else if (!haveGift && haveMenPiao)
          {%>
                    <div class="row main-hr main-footer">
                        <strong class="col text-center">很遗憾，满分才能领取现场神秘大礼</strong>
                    </div>
                    <%}
          else
          {%>
                    <div class="row margin-top-12 margin-bottom-12 main-footer">
                        <strong class="col text-center">很遗憾，满分才能领取现场神秘大礼</strong>
                    </div>
                    <%}%>
                </div>
            </div>
        </div>
    </section>
    <% } %>
    <section class="content">
        <div class="row padding-center">
            <div class="col text-center div-btn" id="btnShare">
                <span class="ico ico-zhuanfa ico-fix"></span>
                <span>邀请好友</span>
            </div>
        </div>
        <% if (ListQuestionResult.Count < 2 && DateTime.Now < Convert.ToDateTime("2015-11-09"))
           { %>
        <div class="row padding-center margin-top">
            <div class="col text-center div-btn" onclick="ToQuestionPage()">
                <span class="ico ico-shuaxin ico-fix"></span>
                <span>再测一次</span>
            </div>
        </div>
        <% }%>
    </section>
    <footer class="content margin-top">
        <div class="row">
            <div class="col-80 col-offset-15 footer-text">
                <p class="p-reformat">时间：2015.11.5-2015.11.8</p>
                <p class="p-reformat">地址：上海世博展览馆</p>
                <p class="p-reformat">地铁8号线中华艺术宫下车（2号口出）</p>
                <p class="p-reformat">地铁7、8号线耀华路站下车（4号口出）</p>
            </div>
        </div>
    </footer>
    <form id="form1" runat="server">
        <div style="display: none">
            <input id="hdnPost" type="hidden" value="0" />
            <asp:Button ID="btnPost" runat="server" Text="提交" OnClick="btnPost_Click" />
            <asp:Button ID="btnToQuestionPage" runat="server" Text="再测一次" OnClick="btnToQuestionPage_Click" />
            <asp:HiddenField ID="hdnIndex" runat="server" Value="" />
            <asp:HiddenField ID="hdnCode" runat="server" Value="" />
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    wx.ready(function () {
        wxapi.wxshare({
            title: "你的体商有多少？测一测，赢取免费好礼！",
            desc: "这里有马拉松，也有lol等电竞大赛，2015上海体博会，更多精彩，等你来战！",
            link: "http://<%=Request.Url.Host%>/customize/SportsShow/Index.aspx",
            imgUrl: "http://<%=Request.Url.Host%>/customize/SportsShow/images/share.png"
        })
    })
    $(function () {
        $("#btnShare").click(function () {
            $("#sharebg,#sharebox").show();
            $("#sharebox").css({ "top": $(window).scrollTop() });
            $(document.body).css("overflow", "hidden");
        });
        $("#sharebg,#sharebox").click(function () {
            $(document.body).css("overflow", "auto");
            $("#sharebg,#sharebox").hide();
        });
    });
    function setValue(val) {
        var Code = $.trim($("#txtCode" + val).val());
        if (Code == "") {
            alert("请输入核销码！");
            return;
        }
        $("#hdnCode").val(Code);
        $("#hdnIndex").val(val);
        $("#btnPost").click();
    }
    function ToQuestionPage() {
        var hdnPostValue = $("#hdnPost").val();
        if (hdnPostValue == 0) {
            $("#hdnPost").val(1);
            $("#btnToQuestionPage").click();
        }
    }
</script>
