<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/jqm/themes/jquery.mobile-1.3.2.min.css" rel="stylesheet"
        type="text/css" />
    <link href="/css/jqm/themes/jquery.mobile.structure-1.3.2.min.css" rel="stylesheet"
        type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.mobile-1.3.2.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {

            $("#btnLogin").click(function () {
                var userID = $("#txtUserName").val();
                var pwd = $("#txtPwd").val();
                if (userID == "") {
                    $("#txtUserName").focus();
                    return false;
                }
                if (pwd == "") {
                    $("#txtPwd").focus();
                    return false;

                }
                login(userID,pwd);

            });



        });
        function login(userID, pwd) {

            $.ajax({
                type: 'post',
                url: "/Handler/OpenGuestHandler.ashx",
                data: { Action: 'Login', userID: userID, pwd: pwd },
                success: function (result) {
                    var resp = $.parseJSON(result);
                    if (resp.Status == 1) {
                        window.location = "/App/Cation/Wap/Mall/OrderDelivery.aspx";
                    }
                    else {
                        alert(resp.Msg);
                    }


                }
            });
        
        
        }

    </script>
</head>
<body>
    <div data-role="page" data-theme="b">
    
    <div style="margin-left:10px;margin-right:10px;margin-top:10px;">
    
     用户名：
     <input type="text" id="txtUserName" />
     密码：
     <input type="password" id="txtPwd" />
     <input type="button" id="btnLogin" value="登录"/>
    
    </div>
    </div>



</body>
</html>
