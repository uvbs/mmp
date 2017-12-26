<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddV.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.AddV" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta content="width=device-width,initial-scale=1,user-scalable=no" name="viewport" />
    <title>微信加V</title>
    <link href="/css/master/master.css?v=020" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>

</head>
<body>

   

    <div class="mycenter" >
<% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel(); %>
<section class="box">
    <div class="header">
        
       <%
         if (currUser != null)
          {
            if (string.IsNullOrWhiteSpace(currUser.WXHeadimgurl))
                                Response.Write(@"<img alt=""图片未显示"" src=""/img/offline_user.png"" width=""52"" height=""52""  style=""border-radius:50px;"" />");
                
                            else
                                Response.Write(string.Format(@"<img alt=""图片未显示"" src=""{0}"" width=""52"" height=""52""  style=""border-radius:50px;"" />", currUser.WXHeadimgurlLocal));
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
        <p>请长按图片保存到手机。 <br/>
        </p>
        <a href="#" id="btnReload" class="btn">更新头像</a><div class="line"></div>
    </div>
    <div class="concent">
       <select id="ddladdv" style="width:95%;height:30px;">
       <%=sbCategory.ToString()%>
        </select>
        <br />
        <div id="divMsg" style="text-align:center;display:none;">
       <label style="text-align:center;">载入中...</label>
       </div>
       <div style="width: 100%; padding: 20px;" id="divAddV">
            
        </div>
        
   
   
   
    </div>
</section>



</div>

 <script type="text/javascript">
     var handlerurl = "/Handler/OpenGuestHandler.ashx";
     var dirs = []; //加V目录集合
     $(function () {
         dirs.push("");
         dirs.push("");
         dirs.push("");
         dirs.push("");
         dirs.push("");
         dirs.push("");
         dirs.push("");
         dirs.push("");
         dirs.push("");
         LoadAddV(0);
         $("#ddladdv").change(function () {
             var dirname = $(this).val();
             if (dirs[dirname] == "") {
                 LoadAddV(dirname);
             }
             else {

                 $("#divAddV").html(dirs[dirname]);
             }

         });

     })

     function LoadAddV(dir) {
         //$.mobile.loading('show', { textVisible: true, text: '载入中...' });
         $("#divMsg").show();
         $.ajax({
             type: 'post',
             url: handlerurl,
             data: { Action: "GetAddVImageWap", dir: dir },
             timeout: 60000,
             success: function (result) {
                 //$.mobile.loading('hide');
                 $("#divMsg").hide();
                 dirs[dir] = result;
                 $("#divAddV").html(result);
             },
             error: function () {
                 $("#divMsg").hide();
                 $("#divAddV").html("加载超时，请刷新重试");
             }
         });




     }
    </script>

<script type="text/javascript">
    $(document).delegate('#btnReload', 'click', function () {
        
        $.ajax({
            type: 'post',
            url: '/Handler/OpenGuestHandler.ashx',
            data: { Action: 'UpdateToLogoutSessionIsRedoOath' },
            success: function (result) {
                window.location.href = "/App/Cation/Wap/AddV.aspx";
            }
        });
    })
</script>

</body>
</html>
