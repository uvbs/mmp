<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.test.test" %>
<html>
<head runat="server">
    <script src="http://static-files.socialcrmyun.com/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" style="height: 21px" />
    </form>



    <input type="button"  value="测试订单" onclick="addOrder()" />
    
</body>
 </html>

<script type="text/javascript">
    function addOrder() {

        var data = {
            "requestId": "100001946356300504",
            "type": 10,
            "appId": 22588462,
            "message": "{\"id\":\"1206118570348344502\",\"orderId\":\"1206118570348344502\",\"address\":\"森林滑草北沿公路2288号东平国家森林公园内\",\"createdAt\":\"2017-05-02T10:30:53\",\"activeAt\":\"2017-05-02T10:30:53\",\"deliverFee\":0.01,\"deliverTime\":null,\"description\":\"\",\"groups\":[{\"name\":\"1号篮子\",\"type\":\"normal\",\"items\":[{\"id\":622441876,\"skuId\":237740481240,\"name\":\"玫瑰花\",\"categoryId\":1,\"price\":0.01,\"quantity\":1,\"total\":0.01,\"additions\":[],\"newSpecs\":[],\"attributes\":[],\"extendCode\":\"\",\"barCode\":\"\",\"weight\":1.0,\"userPrice\":0.0,\"shopPrice\":0.0}]},{\"name\":\"其它费用\",\"type\":\"extra\",\"items\":[{\"id\":-70000,\"skuId\":-1,\"name\":\"餐盒\",\"categoryId\":102,\"price\":0.01,\"quantity\":1,\"total\":0.01,\"additions\":[],\"newSpecs\":null,\"attributes\":null,\"extendCode\":\"\",\"barCode\":\"\",\"weight\":null,\"userPrice\":0.0,\"shopPrice\":0.0}]}],\"invoice\":null,\"book\":false,\"onlinePaid\":true,\"railwayAddress\":null,\"phoneList\":[\"17301820715\"],\"shopId\":155430921,\"shopName\":\"花到家测试店铺\",\"daySn\":3,\"status\":\"unprocessed\",\"refundStatus\":\"noRefund\",\"userId\":94067082,\"totalPrice\":0.03,\"originalPrice\":0.03,\"consignee\":\"渣蓝哇 女士\",\"deliveryGeo\":\"121.48382993,31.68402004\",\"deliveryPoiAddress\":\"森林滑草北沿公路2288号东平国家森林公园内\",\"invoiced\":false,\"income\":0.03,\"serviceRate\":0.0,\"serviceFee\":-0.0,\"hongbao\":0.0,\"packageFee\":0.01,\"activityTotal\":-0.0,\"shopPart\":-0.0,\"elemePart\":-0.0,\"downgraded\":false,\"vipDeliveryFeeDiscount\":0.0,\"openId\":\"\"}",
            "shopId": 155430921,
            "timestamp": 1493692253179,
            "signature": "3AA1BB4344B46A6C008F32D64055D36B",
            "userId": 97016705788981980
        }

        $.ajax({
            type: "POST",
            url: '/TakeOutNotify/elemenotify.aspx',
            data: { data: JSON.stringify(data) },
            dataType: 'JSON',
            success: function (data) {
                alert(data);
            }
        });


    }
</script>