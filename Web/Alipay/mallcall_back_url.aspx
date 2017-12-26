<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" CodeBehind="MallCall_Back_Url.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Alipay.mallcall_back_url" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            var result = "<%=payresult%>";
            if (result == "true") {
                $("#divsuccess").show();
               
            }
            else {
                $("#diverror").show();
               
               
            }

        })
    </script>
</head>
<body>
<section class="box">
    <div class="notebox" id="divsuccess" style="display:none;">
        <div class="noteinfo">支付成功!</div>
        <p class="text"><span class="icon"> </span>我们将尽快处理您的订单</p>
        
        <div class="rightbox">
            <span class="righticon"><span class="icon"></span></span>
        </div>
             
    </div>

    <div id="diverror" runat="server" style="display:none;"><h1 style="color:red;" id="msg" runat="server"></h1></div>
   
</section>
</body>

</html>
