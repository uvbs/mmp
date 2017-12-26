<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Iluxday/Master.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Iluxday.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
首页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
  <link href="Styles/index.css" rel="stylesheet" type="text/css" />
  <style>

  .wrapLoveExchangIndex {
    background-size: 100%;
    background-repeat: no-repeat;
}

  </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<div class="wrapLoveExchangIndex">
    <div class="null">

    </div>
    <div class="wrapInput">
        <div class="row pAll0">
            <div class="col pAll0">
                <img src="images/intro.png" class="prizeImg"/>
            </div>
        </div>
    </div>

<%--    <div class="wrapInput" >
        <div class="row">
            <div class="col pTop11">
                <div class="">
                    <div class="line"></div>
                    <div class="wrapBtn register">
                        <a href="javascript:;">参与方式</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="wrapInput">
        <div class="row pTopBottom0">
            <div class="col one">
                <div class="wrapBtn register textL">
                    <a href="javascript:;" class="itemOne">选取自己获赞的微信内容

                    </a>
                    <div class="outside">
                        <div class="inside">
                            <span class="oneWord">1</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="wrapInput">
        <div class="row pTopBottom0">
            <div class="col one">
                <div class="wrapBtn register textL">
                    <a href="javascript:;" class="itemOne">写出点赞理由

                    </a>
                    <div class="outside top381">
                        <div class="inside">
                            <span class="oneWord">2</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="wrapInput">
        <div class="row pTopBottom0">
            <div class="col one">
                <div class="wrapBtn register textL">
                    <a href="javascript:;" class="itemOne">截屏上传

                    </a>
                    <div class="outside top441">
                        <div class="inside">
                            <span class="oneWord">3</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="wrapInput">
        <div class="row pTopBottom0">
            <div class="col one">
                <div class="wrapBtn register textL">
                    <a href="javascript:;" class="itemOne">评选最赞微信

                    </a>
                    <div class="outside top501">
                        <div class="inside">
                            <span class="oneWord">4</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>


    <div class="wrapInput">
        <div class="row mTop30">
            <div class="col pTop15">
                <div class="">
                    <div class="line"></div>
                    <div class="wrapBtn register">
                        <a href="javascript:;">奖项设置</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="wrapInput">
        <div class="row pAll0">
            <div class="col pAll0">
                <img src="images/prize.png" class="prizeImg"/>
            </div>
        </div>
    </div>
    <div class="contact">
        扫描二维码<br/>获取活动最新进程<br/>了解奖品领取方法<br/>咨询活动相关事项
    </div>
    <div class="erweima">
        <img src="images/erweima.png" class="erweimaImg0"/>
    </div>
    <div class="intro">
    “爱奢汇”-专业跨境进口电商平台，精致生活，<br />我们只做最赞的！<br />
      
        <span class="">长按二维码，关注爱奢汇马上明白</span><br/>
        <span class="zhubanfang">活动主办方：跨境电商爱奢汇</span>
    </div>
    <div class="bottom">
        <div class="row">
            <div class="col borderLine">
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
