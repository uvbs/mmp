<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="success.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.success" %>

<!DOCTYPE html>
<html>
<head>

    <title>订单提交成功</title>
     <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/zepto.min.js" type="text/javascript"></script>
</head>
<body>
<section class="box">
    <div class="notebox">
        <div class="noteinfo">订单提交成功</div>
        <p class="text"><span class="icon"> </span>我们将尽快处理您的订单</p>
        
        <div class="rightbox">
            <span class="righticon"><span class="icon"></span></span>
        </div>
             
    </div>
    <div class="btnbox">
        <a href="/App/Cation/Wap/Mall/<%=OrderDetailPage%>?oid=<%=Request["oid"]%>" class="btn orange">查看订单</a>
        <%if ((Order.PaymentType.Equals(1)||Order.PaymentType.Equals(2))&&(Order.PaymentStatus.Equals(0)))
          {
              if (Order.PaymentType.Equals(1))
              {
                  Response.Write(string.Format("<a href=\"DoAliPay.aspx?oid={0}\" class=\"btn red\">去付款</a>", Order.OrderID));
 
              }
              else if (Order.PaymentType.Equals(2))
              {
                  Response.Write(string.Format("<a href=\"DoWxPay.aspx?oid={0}\" class=\"btn red\">去付款</a>",Order.OrderID));
              }
          }
          else
          {
              Response.Write(string.Format("<a href=\"/App/Cation/Wap/Mall/{0}\" class=\"btn red\">继续购物</a>",GoPage));
          } %>
       
    </div>
    <div class="shopcomment"><%=currentWebsiteInfo.SumbitOrderPromptInformation%></div>
</section>
</body>

</html>