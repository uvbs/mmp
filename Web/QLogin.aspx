<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QLogin.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.QLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <title>请先登录</title>
    <meta charset="utf-8">
    <link href="/customize/kuanqiao/css/SubmitInfoWeb.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div class="toper">
    <div class="header">
        <div class="logo"></div>
        <div class="describe"></div>
        <a href="#" class="mainbtn"></a>
    </div>
</div>
<div class="twocodebox" style="width:100%;">
<div class="twodimensioncode" style="">

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="TimerCheck" runat="server" OnTick="TimerCheck_Tick" Interval="3000"
                Enabled="false">
            </asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>

   <img alt="正在加载。。。" src="" id="imgQrCode" runat="server" />
    
  </form>
</div>
<div class="hint">请用微信扫描二维码以登录</div>
</div>

   <div class="footer">
        ICP备 11027501 号 公网安备 11010802012285 号 
        <a href="https://www.sgs.gov.cn/shaic/" class="wangjinicon"></a>
        Copyright © 2014  宽桥企业帮. All Rights Reserved.
    </div>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        changeheight();
        $(window).resize(function () {
            changeheight();
        })
        function changeheight() {
            var dd = $(window).height();
            dd = dd - 270;
            $(".twocodebox").css("min-height", dd);
        }

    })

</script>
</html>
