<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Iluxday/Master.Master"
    AutoEventWireup="true" CodeBehind="Rule.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Iluxday.Rule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    规则
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/ruler.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapRuler">
        <div class="wrapInput pTop297">
            <div class="row">
                <div class="col">
                    <img src="images/rulers.png" class="imgStyle" />
                </div>
            </div>
        </div>
        <div class="wrapInput">
            <div class="row">
                <div class="col">
                    <img src="images/ruler_03.png" class="imgStyle">
                </div>
            </div>
        </div>
        <div class="contact">
            扫描二维码<br />
            获取活动最新进程<br />
            了解奖品领取方法<br />
            咨询活动相关事项
        </div>
        <div class="erweima">
            <img src="images/erweima.png" class="erweimaImg0" />
        </div>
        <div class="intro">
            “爱奢汇”-专业跨境进口电商平台，精致生活，<br />我们只做最赞的！<br />

            <span class="">长按二维码，关注爱奢汇马上明白</span><br />
            <span class="zhubanfang">活动主办方：跨境电商爱奢汇</span>
        </div>
        <div class="bottom">
            <div class="row">
                <div class="col borderLine pLeft0" onclick="window.location.href='Index.aspx'">
                    <i class="iconfont icon-shouye shouye"></i>
                </div>
                <div class="col col-80 pLeft0 pRight0">
                    <div class="row pLeft0 pRight0">
                        <div class="col borderLine" onclick="window.location.href='Rule.aspx'">
                            活动规则
                        </div>
                        <div class="col borderLine" onclick="window.location.href='SignUp.aspx'">
                            <%=signUpText %>
                        </div>
                        <div class="col pRight0" onclick="window.location.href='List.aspx'">
                            为TA点赞
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        //分享
        var shareTitle = "【爱奢汇】秀最赞微信，赢最赞时尚大奖";
        var shareDesc = "秀最赞微信，赢最赞时尚大奖。选取朋友圈获赞微信内容上传，即表示报名成功，小伙伴们可以火热开启拉票啦！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Iluxday/images/logo.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
