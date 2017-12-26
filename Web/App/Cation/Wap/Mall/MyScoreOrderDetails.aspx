<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyScoreOrderDetails.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.MyScoreOrderDetails" %>

<!DOCTYPE html>
<html>
<head>
    <title>积分订单详情</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/zepto.min.js" type="text/javascript"></script>
    <script src="/Ju-Modules/Common/Cookie.Min.js" type="text/javascript"></script>
    <script src="/Scripts/json2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
    <style type="text/css">
   .deliverytype .addressbox {
   height:auto;
}
.deliverytype .checktag{height:auto;}
.ordernum{font-weight:bold;}
.deliverytype .addressbox .name{width:100%; height:32px;overflow:hidden;}
.deliverytype .addressbox .phone{float:left;text-align:left;width:100%;padding-left:10px;}
.deliverytype .addressbox .address{height:auto;}

    </style>
</head>
<body>
    <section class="box">
    <div class="orderinfobox">
        <div class="order">
            <div class="orderdata">订单编号:<label class="ordernum"> <%=OrderInfo.OrderID %></label>&nbsp;|&nbsp;<%=OrderInfo.InsertDate.ToString("yyyy-MM-dd HH:mm")%>&nbsp;|&nbsp;<%=OrderInfo.Status%></div>
            <%
                ZentCloud.BLLJIMP.BLLMall bllMall= new ZentCloud.BLLJIMP.BLLMall();
                List<ZentCloud.BLLJIMP.Model.WXMallScoreOrderDetailsInfo> OrderDetailList = bllMall.GetScoreOrderDetailsList(OrderInfo.OrderID);
                foreach (var orderdetail in OrderDetailList)
                {
                    ZentCloud.BLLJIMP.Model.WXMallScoreProductInfo ProductInfo = bllMall.Get<ZentCloud.BLLJIMP.Model.WXMallScoreProductInfo>(string.Format("AutoID='{0}'", orderdetail.PID));
                    if (ProductInfo != null)
                    {
                        Response.Write("<div class=\"product\">");
                        Response.Write(string.Format("<img src=\"{0}\" >", ProductInfo.RecommendImg));
                        Response.Write("<div class=\"info\">");
                        Response.Write(string.Format("<span class=\"text\">{0}</span>", ProductInfo.PName));
                        Response.Write(string.Format("<span class=\"price\">￥{0}<span class=\"num\">x{1}</span></span>", orderdetail.OrderPrice, orderdetail.TotalCount));
                        Response.Write("</div>");
                        Response.Write("</div>");

                    }

                }
                
                
             %>
            <div class="total">共计<span class="totalnum"><%=OrderInfo.ProductCount %></span>件商品,<span class="totalprice"><%=OrderInfo.TotalAmount %></span><span class="orangetext">积分</span>
            </div>
        </div>
    </div>

   <%-- 配送方式--%>
   <div class="deliverytype">
   <%
       Response.Write("<div class=\"checktag\">");
       //Response.Write("<span class=\"mydeliverytype\">快递</span>");
       Response.Write("</div>");
       Response.Write("<div class=\"express\" >");
       Response.Write("<div href=\"#\" class=\"addressbox\">");
       Response.Write(string.Format("<span class=\"name\" >收货人:{0}</span>", OrderInfo.Consignee));
       Response.Write(string.Format("<span class=\"phone\">电话:{0}</span>", OrderInfo.Phone));
       Response.Write(string.Format("<span class=\"address\">地址 :{0}</span>", OrderInfo.Address));
       if (!string.IsNullOrEmpty(OrderInfo.OrderMemo))
       {
           Response.Write(string.Format("<span class=\"address\">买家留言 :{0}</span>", OrderInfo.OrderMemo));
       }
       if (!string.IsNullOrEmpty(OrderInfo.Remarks))
       {
           Response.Write(string.Format("<span class=\"address\">卖家留言 :{0}</span>", OrderInfo.Remarks));
       }
       Response.Write("</div>");
       Response.Write("</div>");       
   %>
     </div>
    <div class="backbar">
        <a href="MyScoreOrderList.aspx" class="back"><span class="icon"></span></a>
    </div>
</section>
</body>
</html>
