<%@ Page Language="C#" AutoEventWireup="true" EnableSessionState="ReadOnly" CodeBehind="info.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SportsShow.info" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0">
    <meta name="format-detection" content="telephone=no">
    <title>兑奖信息完善</title>
    <link href="/lib/ionic/ionic.css" rel="stylesheet">
    <link href="css/base.css" rel="stylesheet">
    <link href="css/info.css" rel="stylesheet">
    <script src="/lib/jquery/jquery-2.1.1.min.js"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script src="js/SportsShow.js"></script>
</head>
<body class="bg">
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
    <section class="content margin-top margin-bottom">
        <div class="row">
            <div class="col">
                <div class="main-content">
                    <p class="text-center">填完以下信息，查看你的体商指数</p>

                    <p class="text-center">
                        <%if(type=="2"){ %>
                        免费获得体博会门票
                    <small>（原价¥20）</small>
                        <%}
                        else if (type == "1")
                          { %>
                        赢取神秘大奖
                        <%} %>
                    </p>
                    <div class="row text-center">
                        <div class="col-20">
                            <label>性别：</label>
                        </div>
                        <div class="col-80 fix-width-80 Sex">
                            <span class="btn-info btn-size-1 <%= user.Ex1=="男"?"btn-selected":"" %>" onclick="setSelect(this)">男</span>
                            <span class="btn-info btn-size-1 <%= user.Ex1=="女"?"btn-selected":"" %>" onclick="setSelect(this)">女</span>
                        </div>
                    </div>
                    <div class="row text-center">
                        <div class="col-20">
                            <label>年龄：</label>
                        </div>
                        <div class="col-80 fix-width-80 Age">
                            <span class="btn-info btn-size-2 <%= user.Ex2=="20-30"?"btn-selected":"" %>" onclick="setSelect(this)">20-30</span>
                            <span class="btn-info btn-size-2 <%= user.Ex2=="30-40"?"btn-selected":"" %>" onclick="setSelect(this)">30-40</span>
                            <span class="btn-info btn-size-2 <%= user.Ex2=="40-50"?"btn-selected":"" %>" onclick="setSelect(this)">40-50</span>
                            <span class="btn-info btn-size-2 <%= user.Ex2=="50以上"?"btn-selected":"" %>" onclick="setSelect(this)">50以上</span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label>是否体育行业从业人员</label>
                        </div>
                    </div>
                    <div class="row text-center">
                        <div class="col-80 col-offset-20 fix-width-69 SportMember">
                            <span class="btn-info btn-size-1 <%= user.Ex3=="是"?"btn-selected":"" %>" onclick="setSelect(this)">是</span>
                            <span class="btn-info btn-size-1 <%= user.Ex3=="否"?"btn-selected":"" %>" onclick="setSelect(this)">否</span>
                        </div>
                    </div>
                    <div class="row text-center">
                        <div class="col-20">
                            <label>手机：</label>
                        </div>
                        <div class="col-80 fix-width-80">
                            <input id="txtPhone" type="tel" class="tel-input" value="<%=user.Phone %>">
                        </div>
                    </div>
                    <div class="row text-center">
                        <div class="col-80 col-offset-20 fix-width-69 text-center">
                            <a class="btn-info btn-size-3" onclick="postData()">确认</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <form id="form1" runat="server">
        <div style="display: none">
            <asp:Button ID="btnPost" runat="server" Text="提交" OnClick="btnPost_Click" />
            <asp:HiddenField ID="hdnSex" runat="server" Value="" />
            <asp:HiddenField ID="hdnAge" runat="server" Value="" />
            <asp:HiddenField ID="hdnSportMember" runat="server" Value="" />
            <asp:HiddenField ID="hdnPhone" runat="server" Value="" />
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
    function setSelect(obj) {
        $(obj).prevAll().removeClass("btn-selected");
        $(obj).nextAll().removeClass("btn-selected");
        $(obj).addClass("btn-selected");
    }
    function postData() {
        var Sex = $(".Sex .btn-selected").text();
        var Age = $(".Age .btn-selected").text();
        var SportMember = $(".SportMember .btn-selected").text();
        var Phone = $.trim($("#txtPhone").val());
        console.log(Sex);
        console.log(Age);
        console.log(SportMember);
        console.log(Phone);
        if (Sex == "") {
            alert("请完善性别！");
            return;
        }
        if (Age == "") {
            alert("请完善年龄！");
            return;
        }
        if (SportMember == "") {
            alert("请完善是否从事体育行业！");
            return;
        }
        if (Phone == "") {
            alert("请完善手机号码！");
            return;
        }
        if (!/^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$/i.test(Phone)) {
            alert('手机号码错误！');
            return;
        }
        $("#hdnSex").val(Sex);
        $("#hdnAge").val(Age);
        $("#hdnSportMember").val(SportMember);
        $("#hdnPhone").val(Phone);
        $("#btnPost").click();
    }
</script>

