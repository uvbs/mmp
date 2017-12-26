<%@ Page Language="C#" AutoEventWireup="true" EnableSessionState="ReadOnly" CodeBehind="question.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SportsShow.question" %>

<!DOCTYPE html>

<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0">
    <title>体商测试</title>
    <link href="/lib/ionic/ionic.css" rel="stylesheet">
    <link href="css/base.css" rel="stylesheet">
    <link href="css/question.css" rel="stylesheet">
    <script src="/lib/jquery/jquery-2.1.1.min.js"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script src="js/SportsShow.js"></script>
</head>
<body class="bg<% = ListQuestion.Count %>">
    <form id="form1" runat="server">
        <header class="content">
            <div class="row">
                <div class="col-67 col-offset-4">
                    <img src="images/header/logo.png" class="full-image">
                </div>
            </div>
        </header>
        <section class="content">
            <div class="row">
                <div class="col">
                    <div class="main-content">
                        <% if (ListQuestion.Count > 0)
                           { %>
                        <p class="content-head">问题<%= ListQuestion.Count %></p>
                        <p><%= ListQuestion[ListQuestion.Count-1].Question %></p>
                        <% if (!string.IsNullOrWhiteSpace(ListQuestion[ListQuestion.Count - 1].AnswerA))
                           { %>
                        <div class="option" onclick="setValue(this,'A')">
                            <p>A.<%=ListQuestion[ListQuestion.Count - 1].AnswerA %></p>
                        </div>
                        <%} %>
                        <% if (!string.IsNullOrWhiteSpace(ListQuestion[ListQuestion.Count - 1].AnswerB))
                           { %>
                        <div class="option" onclick="setValue(this,'B')">
                            <p>B.<%=ListQuestion[ListQuestion.Count - 1].AnswerB %></p>
                        </div>
                        <%} %>
                        <% if (!string.IsNullOrWhiteSpace(ListQuestion[ListQuestion.Count - 1].AnswerC))
                           { %>
                        <div class="option" onclick="setValue(this,'C')">
                            <p>C.<%=ListQuestion[ListQuestion.Count - 1].AnswerC %></p>
                        </div>
                        <%} %>
                        <% if (!string.IsNullOrWhiteSpace(ListQuestion[ListQuestion.Count - 1].AnswerD))
                           { %>
                        <div class="option" onclick="setValue(this,'D')">
                            <p>D.<%=ListQuestion[ListQuestion.Count - 1].AnswerD %></p>
                        </div>
                        <%} %>
                        <% if (!string.IsNullOrWhiteSpace(ListQuestion[ListQuestion.Count - 1].AnswerE))
                           { %>
                        <div class="option" onclick="setValue(this,'E')">
                            <p>E.<%=ListQuestion[ListQuestion.Count - 1].AnswerE %></p>
                        </div>
                        <%} %>
                        <% if (!string.IsNullOrWhiteSpace(ListQuestion[ListQuestion.Count - 1].AnswerF))
                           { %>
                        <div class="option" onclick="setValue(this,'F')">
                            <p>F.<%=ListQuestion[ListQuestion.Count - 1].AnswerF %></p>
                        </div>
                        <%} %>
                        <% if (!string.IsNullOrWhiteSpace(ListQuestion[ListQuestion.Count - 1].AnswerG))
                           { %>
                        <div class="option" onclick="setValue(this,'G')">
                            <p>G.<%=ListQuestion[ListQuestion.Count - 1].AnswerG %></p>
                        </div>
                        <%} %>
                        <div class="div-btn-next" onclick="postData(this)">
                            <%if (ListQuestion.Count >= NeedCount)
                              { %>
                            完成
                        <%}
                              else
                              {  %>
                            下一题
                        <%} %>
                        </div>
                        <%} %>
                    </div>
                </div>
            </div>
        </section>
        <div style="display: none">
            <input id="hdnPost" type="hidden" value="0" />
            <asp:Button ID="btnPost" runat="server" Text="提交" OnClick="btnPost_Click" />
            <asp:HiddenField ID="hdnValue" runat="server" Value="" />
            <asp:HiddenField ID="hdnResultID" runat="server" Value="" />
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
    });
    function setValue(obj, val) {
        $("#hdnValue").val(val);
        $(".option").removeClass("selected");
        $(obj).addClass("selected");
    }
    function postData(obj) {
        var Value = $("#hdnValue").val();
        if (Value == "") {
            alert("选择答案！");
            return;
        }
        var hdnPostValue = $("#hdnPost").val();
        if (hdnPostValue == 0) {
            $("#hdnPost").val(1);
            $("#btnPost").click();
        }
    }
</script>
