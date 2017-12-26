<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoWxPay.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.DoWxPay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>微信支付</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link rel="stylesheet" href="/css/vote/style.css?v=0.0.1"/>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>

</head>
<body>
<section class="box">
    <div id="divprocess" style="text-align:center;margin-top:30%;"><img src="/img/wxmall/process.gif"  /></div>

    <div class="backbar">
        <a href="javascript:window.history.go(-1)" class="back"><span class="icon"></span></a>
        
    
    </div>
</section>
</body>
<script type="text/javascript">
            // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。
            document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            
              WeixinJSBridge.invoke('getBrandWCPayRequest',<%=WxPayReq%>, function (res) {
                
                // 返回res.err_msg,取值 

                // get_brand_wcpay_request:cancel 用户取消 

                // get_brand_wcpay_request:fail 发送失败 

                // get_brand_wcpay_request:ok 发送成功 
                if (res.err_msg=="get_brand_wcpay_request:ok") {

                    window.location.href="paysuccess.html";
                  
                    }
                    else {
                    // alert("您已取消支付,您还可以继续在个人中心-我的订单中进行支付");
                     $("#divprocess").html("<h1 style=\"font-size:30px;color:red;\">支付已取消</h1>");

                    }

            });
            
            
            })

</script>


</html>
