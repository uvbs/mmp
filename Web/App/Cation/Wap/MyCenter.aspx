<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyCenter.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.MyCenter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <title></title>
    <link rel="stylesheet" href="Style/styles/css/style.css?v=0.0.3">
    <link href="Style/styles/css/green.css" rel="stylesheet" type="text/css" />
    <link href="/css/buttons2.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .donation
        {
            text-align: center;
        }
        .headimg
        {
            width: 50px;
            height: 50px;
        }
        #imgused
        {
            position: absolute;
            top: 120px;
            left: 150px;
            width: 100px;
        }
        body
        {
            font-family: Microsoft YaHei;
        }
    </style>
</head>
<body>
    <section class="box padding10">
    <header class="header">
        <h1><asp:literal text="text" ID="txtTitle" runat="server" /></h1>
    </header>
    <div class="maincontext mycenterbox"> 
        <h2><%= ActivityConfig.TicketShowName%></h2>
        <div class="RQcode" >
        <img id="imgcode" src="" alt="加载中..."/>
        <%if (IsSignIn)
          {%><img id="imgused" src="Images/used.png" /><%} %>
        </div>
        <h3 class="personalinfo">参会人：<%=Name%></h3>
        <h3 class="personalinfo">签到码：<%=CodeStr %></h3>
        <div class="maincontent">
            <span class="time"><asp:literal text="text" ID="txtStartDate" runat="server" /></span>
            <span class="time"><asp:literal text="text" ID="txtEndDate" runat="server" /></span>
            <span class="adress"><asp:literal text="text" ID="txtAddress" runat="server" /></span>
            
        </div>
        <div class="note">温馨提示：请到会场后出示此二维码</div>
      
        <%if (ShowDonation)
          {%>
              <%-- <div class="donation"><a id="btnDonation" class="button button-block button-rounded button-action button-large">转赠</a></div>--%>
          <%} %> 

          <%if (ReceiveUserInfo != null)
            {%>
          <img  src="<%=ReceiveUserInfo.WXHeadimgurl%>" style="width:50px;height:50px;" >
           <span class="personalinfo"><%=ReceiveUserInfo.TrueName%>已接收转赠</span>

          <%} %>
           <%if (FromUserInfo != null)
             {%>
          <img  src="<%=FromUserInfo.WXHeadimgurl%>" style="width:50px;height:50px;" >
           <span class="personalinfo">来自<%=FromUserInfo.TrueName%>的转赠</span>

          <%} %>
            <br>
        <div class="donation wrapGotoDetail hide" style="margin-bottom: 60px;"><a href="javascript:gotoDetail();" class="button button-block button-rounded button-action button-large">活动详情</a></div>

    </div>

   
    <div class="submit" >
        <a href="javascript:history.go(-1)" class="return"></a>
    </div>
       
               <div style="width: 100%; height: 100%; display: none; background: #000; opacity: 0.7;
            position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
            &nbsp;
        </div>
        <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; text-align: right;
            display: none;" id="sharebox">
            <img src="/img/sharetip.png" width="100%" />
        </div>


</section>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/QCode.ashx";
        var code = "<%=Code%>";
        var activityid = '<%=activityid %>';
        var jid = '<%=jid %>';
        $(function () {
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { code: code },
                success: function (result) {
                    $("#imgcode").attr("src", result);
                }
            });


            $("#btnDonation").click(function () {
                $("#sharebg,#sharebox").show();
                $("#sharebox").css({ "top": $(window).scrollTop() })
            });

            $("#sharebg,#sharebox,#followbox").click(function () {
                $("#sharebg,#sharebox").hide();

            });

            if (jid) {
                $('.wrapGotoDetail').show();
            }

        });

        function gotoDetail() {
            var jid16 = parseInt(jid).toString(16);
            window.location.href = '/' + jid16 + '/details.chtml';
        }

        wx.ready(function () {
            wxapi.wxshare({
                title: "<%=juActivityInfo.ActivityName%>转赠",
                desc: "<%=juActivityInfo.ActivityName%>转赠",
                link: "http://<%=Request.Url.Host %>/App/Cation/Wap/ActivityDonation.aspx?activityid=<%=activityid %>&fromuserautoId=<%=CurrentUserInfo.AutoID %>",
                imgUrl: "<%=juActivityInfo.ThumbnailsPath %>"
            }, null)
        });


    </script>
</body>
</html>
