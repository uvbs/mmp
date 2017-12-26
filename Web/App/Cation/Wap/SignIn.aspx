<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.SignIn" %>
<!DOCTYPE html>
<html>
<head lang="en">
    <meta charset="UTF-8">
    <title><%=juActivityInfo.ActivityName %></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width"/>
    <link href="/lib/ionic/ionic.css" rel="stylesheet"/>
    <link href="Style/styles/css/signin.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div class="wrapCheckIn">

<%if (IsSignInSuccess)
  {%>
    <!--签到成功页面-->
    <div class="sign-success">
        <div class="row icon-row">
            <div class="col icon-col">
                <i class="icon iconfont icon-gougou gougou"></i>
            </div>
        </div>
        <div class="row tips-row">
            <div class="col tips-col">
                签到成功，欢迎参加本次活动！
            </div>
        </div>
    </div>
  <%} %>

  <%if (IsHaveSignIn)
  {%>
    <!--已经签过到页面-->
    <div class="signed">
        <div class="row icon-row">
            <div class="col icon-col">
                <i class="icon iconfont icon-xiaoliankong kulian"></i>
            </div>
        </div>
        <div class="row tips-row">
            <div class="col tips-col">
                您已经签过到了！
            </div>
        </div>
    </div>
  <%} %>

  <%if (NeedSignUp)
  {%>
    <!--未报名页面-->
        <div class="regist-first">
        <div class="row icon-row">
            <div class="col icon-col">
                <i class="icon iconfont icon-xiaoliankong kulian"></i>
            </div>
        </div>
        <div class="row tips-row">
            <div class="col tips-col">
                您还未报名参加本次活动！
            </div>
        </div>
        <div class="padding">
            <button type="submit" class="button button-block button-energized change"
                    onclick="window.location.href='/<%=juActivityInfo.JuActivityIDHex %>/detail.chtml?gotosigninpage=1'">
                现场报名并签到
            </button>
        </div>
    </div>
    <%} %>

</div>
</body>
</html>
