<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WxPay.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Shop.WxPay" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>微信支付</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link rel="stylesheet" href="/css/vote/style.css?v=0.0.1" />
</head>
<body>
    <section class="box">
    <div id="divprocess" style="text-align:center;margin-top:30%;"><img src="/img/wxmall/process.gif"  /></div>

    <div class="backbar">
        <a href="javascript:window.history.go(-1)" class="back"><span class="icon"></span></a>
        
    
    </div>
</section>
</body>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script type="text/javascript">

        var returnUrlSuccess="<%=ReturnUrlSuccess %>";//支付成功跳转地址
        var returnUrlFail="<%=ReturnUrlFail %>";//支付失败或取消跳转地址
        var payDataJson=window.sessionStorage.getItem("payData");

        if (payDataJson!=null&&payDataJson!="") {//

        
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            var payData=$.parseJSON(payDataJson);
            $.ajax({
                type: 'post',
                url: payData.handerUrl,
                data: payData,
                dataType: "json",
                success: function (resp) {
                    if (resp.status==true) {
                        
                        // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。
                        WeixinJSBridge.invoke('getBrandWCPayRequest', resp.result.pay_req,
                            function (res) {
                
                            // 返回res.err_msg,取值 

                            // get_brand_wcpay_request:cancel 用户取消 

                            // get_brand_wcpay_request:fail 发送失败 

                            // get_brand_wcpay_request:ok 发送成功 
                            if (res.err_msg=="get_brand_wcpay_request:ok") {
                                if (payData.successUrl != "" && payData.successUrl != null) {
                                    if (payData.successsMsg!="") {
                                        alert(payData.successMsg);
                                    }
                                    window.location.href = payData.successUrl;
                                }
                                else {
                                    $("#divprocess").html("<h1 style=\"font-size:30px;color:Green;\">您已经成功支付！</h1>");
                                }
                            }
                            else {
                     
                                //if (returnUrlFail!=""&&returnUrlFail!=null) {
                                //    window.location.href=returnUrlFail;
                                //}
                                //else {
                                    $("#divprocess").html("<h1 style=\"font-size:30px;color:red;\">支付已经取消</h1>");
                               // }

                            }

                        });
                        //


                    }
                    else {
               
                        $("#divprocess").html("<h1 >"+resp.msg+"</h1>");
                    }
                    //



                }
            });
            
         })

    }
    else {//默认的
    
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {

            $.ajax({
                type: 'post',
                url: "/serv/api/mall/payment.ashx",
                data: { action:"BrandWcPayRequest",order_id:"<%=Request["order_id"] %>" },
            dataType: "json",
            success: function (resp) {
                if (resp.errcode==0) {
                    // 当微信内置浏览器完成内部初始化后会触发WeixinJSBridgeReady事件。
                    WeixinJSBridge.invoke('getBrandWCPayRequest',resp.pay_req, function (res) {
                
                        // 返回res.err_msg,取值 

                        // get_brand_wcpay_request:cancel 用户取消 

                        // get_brand_wcpay_request:fail 发送失败 

                        // get_brand_wcpay_request:ok 发送成功 
                        if (res.err_msg=="get_brand_wcpay_request:ok") {

                            if (returnUrlSuccess!=""&&returnUrlSuccess!=null) {
                                window.location.href=returnUrlSuccess;
                            }
                            else {
                                $("#divprocess").html("<h1 style=\"font-size:30px;color:Green;\">您已经成功支付！</h1>");
                            }
                        }
                        else {
                     
                            if (returnUrlFail!=""&&returnUrlFail!=null) {
                                window.location.href=returnUrlFail;
                            }
                            else {
                                $("#divprocess").html("<h1 style=\"font-size:30px;color:red;\">支付已经取消</h1>");
                            }

                        }

                    });
                    //


                }
                else {
               
                    $("#divprocess").html("<h1 >"+resp.errmsg+"</h1>");
                }
                //



            }
        });
            
                })


    }



</script>
</html>
