<%@ Page Language="C#" Title="个人中心" AutoEventWireup="true" CodeBehind="UserHub.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.UserHub" %>

<!DOCTYPE html>
<html>
<head>
    <title>个人中心</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link href="/css/mycenterv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/gzptcommon.js" type="text/javascript"></script>

</head>
<body class="mycenter">
<section class="box">
 <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel();%>
    <div class="header">
         <%
                        if (currUser != null)
                        {
                            if (string.IsNullOrWhiteSpace(currUser.WXHeadimgurl))
                            {
                                Response.Write("<img alt=\"\" src=\"/img/offline_user.png\" width=\"52\" height=\"52\" />");
                            }
                            else
                            {
                                Response.Write(string.Format("<img alt=\"\" src=\"{0}\" width=\"52\" height=\"52\"/>", currUser.WXHeadimgurlLocal));

                            }
                        }
          %>
        <h2>
        <% 
           string str = "{还没有填写姓名}";
           //显示排序：真实姓名、微信昵称、登录名、手机
           if (currUser != null)
            {
              if (!string.IsNullOrWhiteSpace(currUser.WXNickname))
              str = currUser.WXNickname;
              else if (!string.IsNullOrWhiteSpace(currUser.TrueName))
              str = currUser.TrueName;
              else if (!string.IsNullOrWhiteSpace(currUser.LoginName))
              str = currUser.LoginName;
              else if (!string.IsNullOrWhiteSpace(currUser.Phone))
              str = currUser.Phone;
           }
           Response.Write(str);
         %>
        </h2>
       <a href="#" id="btnReload" class="nomalbtn">更新头像</a><div class="line"></div>
    </div>
    <div class="concent">
        <a href="/App/Cation/Wap/MyGreetingCard.aspx" class="btn">新年贺卡</a>
<%--        <a href="#" onclick="alert('请用电脑登录底部网址')" class="btn"> 微秀制作</a>
        <a href="/App/Cation/Wap/NewActivity.aspx" class="btn">组织活动</a>
        <a href="/App/Cation/Wap/NewArticle.aspx" class="btn">发表文章</a>
        <a href="/App/Cation/Wap/MyPub.aspx" class="btn">我的发布</a>
        <a href="/App/Cation/Wap/BusinessCardMgr.aspx" class="btn">名片管理</a>
        <a href="/App/Cation/Wap/UserEdit.aspx" class="btn">账户管理</a>--%>
        
    </div>
</section>


<section class="navbar">
     
     <a  class="publish mainbtn" href="http://cloudnewthinking.comeoncloud.net/248f9/details.chtml">
        至云移动营销平台
    </a>

</section>
</body>
  <script type="text/javascript">
      $(document).delegate('#btnReload', 'click', function () {
          $.ajax({
              type:'post',
              url: '/Handler/OpenGuestHandler.ashx',
              data: { Action:'UpdateToLogoutSessionIsRedoOath' },
              success: function (result) {
                  window.location.href = "/App/Cation/Wap/UserHub.aspx";
              }
          });
      })
    </script>
</html>
