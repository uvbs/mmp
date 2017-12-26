<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Alipay.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.Pay.Alipay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="width=device-width,initial-scale=1,user-scalable=no" name="viewport" />
    <title>支付宝付款</title>
    <link href="//res.wx.qq.com/open/libs/weui/1.1.0/weui.min.css" rel="stylesheet" />
    <link href="//static-files.socialcrmyun.com/css/global-m.css" rel="stylesheet" />
    <style type="text/css">
        .head{
            background-color:#1aad19;
            color:#fff;
            padding:10px 6px;
        }
        .bottom{
            padding:0 2%;
        }
    </style>
</head>
<body style="background-color:#ffffff;">
    <div class="txtCenter font14">        
        <%if (orderPay == null || !string.IsNullOrWhiteSpace(errorMsg))
            { %>
        <div class="font20">
            <br />
            <br />
            <%=errorMsg %>
        </div>

        <%}
            else
            { %>

        
        <div class="head">
            请点击右上角，选择在浏览器中打开完成支付
        </div>
        <br />
        <br />
        <div class="">
            <div>
                订单号 <%=orderPay.OrderId %>
            </div>
            <br />
            <div class=" font20">
                需付金额 ￥<%=orderPay.Total_Fee %>
            </div>
            <br />
            <div>
                下单时间 <%=orderPay.InsertDate.ToString("yyyy-MM-dd hh:mm:ss") %>
            </div>
        </div>
<br />
        <div>成功付款后，请点击“已完成付款”</div>
        <br />
        <div class="bottom">
            <a href="javascript:void(0);" class="weui-btn weui-btn_primary" onclick="checkOrder()">已完成付款</a>
        </div>
        

        <%} %>
    </div>
</body>
</html>
<script src="//static-files.socialcrmyun.com/lib/zepto/zepto.min.js" type="text/javascript"></script>
<script src="//static-files.socialcrmyun.com/lib/layer.mobile/2.0/layer.js" type="text/javascript"></script>
<script src="//static-files.socialcrmyun.com/Scripts/global-m.js?v=2017030101" type="text/javascript"></script>
<script type="text/javascript">
    var order_id = '<%=orderPay==null?"":orderPay.OrderId%>';
    var inter = -1;
    $(function () {
    });
    function checkOrder() {
        $.ajax({
            type: 'post',
            url: '/Serv/API/User/Order/CheckPay.ashx',
            data: {
                order_id: order_id
            },
            dataType: 'json',
            success: function (resp) {
                if (resp.status) {
                    //zcConfirm('支付成功', '确定', '关闭', function () {
                        window.location.href = '/customize/comeoncloud/Index.aspx?key=PersonalCenter';
                    //});
                } else {
                    zcAlert(resp.msg);
                }
            }
        });
    }
</script>
