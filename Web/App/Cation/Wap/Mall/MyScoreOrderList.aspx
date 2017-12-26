<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyScoreOrderList.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.MyScoreOrderList" %>

<!DOCTYPE html>
<html>
<head>
    <title>积分订单</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="/WuBuHui/css/wubu.css" rel="stylesheet" type="text/css" />
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/zepto.min.js" type="text/javascript"></script>
    <script src="/Ju-Modules/Common/Cookie.Min.js" type="text/javascript"></script>
    <script src="/Scripts/json2.min.js" type="text/javascript"></script>
    <script src="/Scripts/StringBuilder.Min.js" type="text/javascript"></script>
    <script src="/WuBuHui/js/comm.js" type="text/javascript"></script>
    <style type="text/css">
    .ordernum{font-weight:bold;}
    </style>
</head>
<body>
    <section class="box">
    <div class="orderbox">
        
           <% if (orderList.Count > 0)
              {
                  ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
                  foreach (var order in orderList)
                  {
                      Response.Write(string.Format("<a href=\"MyScoreOrderDetails.aspx?oid={0}\" class=\"order\">", order.OrderID));
                      List<ZentCloud.BLLJIMP.Model.WXMallScoreOrderDetailsInfo> orderDetailList = bllMall.GetScoreOrderDetailsList(order.OrderID);
                      Response.Write(string.Format("<span class=\"orderdata\" >订单编号:<label class=\"ordernum\">{0}</label>&nbsp;|&nbsp;{2}&nbsp;|&nbsp;<label status-oid=\"{0}\">{1}</label></span>", order.OrderID, order.Status, order.InsertDate.ToString("yyyy-MM-dd HH:mm")));


                      foreach (var orderdetail in orderDetailList)
                      {
                          ZentCloud.BLLJIMP.Model.WXMallScoreProductInfo productInfo = bllMall.Get<ZentCloud.BLLJIMP.Model.WXMallScoreProductInfo>(string.Format("AutoID='{0}'", orderdetail.PID));
                          if (productInfo != null)
                          {
                              Response.Write("<div class=\"product\">");
                              Response.Write(string.Format("<img src=\"{0}\" >", productInfo.RecommendImg));
                              Response.Write("<div class=\"info\">");
                              Response.Write(string.Format("<span class=\"text\">{0}</span>", productInfo.PName));
                              Response.Write(string.Format("<span class=\"price\">{0}积分<span class=\"num\">x{1}</span></span>",(int)orderdetail.OrderPrice, orderdetail.TotalCount));
                              Response.Write("</div>");
                              Response.Write("</div>");



                          }



                      }

                      Response.Write("<div class=\"total\">");
                      Response.Write("共计");
                      Response.Write(string.Format("<span class=\"totalnum\">{0}</span>件商品,", order.ProductCount));
                      Response.Write(string.Format("<span class=\"totalprice\">{0}</span>", (int)order.TotalAmount));
                      Response.Write("<span class=\"orangetext\">积分</span>");
                      Response.Write("</div>");
                      Response.Write("</a>");

                      if (order.Status.Equals("等待处理")&&(!bllMall.WebsiteOwner.Equals("wubuhui")))
                      {
                          Response.Write("<a class=\"order\" style=\"background-color:#eef1f4;box-shadow:0 0 0;text-align:right;\">");
                          Response.Write(string.Format("<div data-oid=\"{0}\"  href=\"javascript:void(0);\"  class=\"btn orange\">取消订单</div>", order.OrderID));

                          Response.Write("</a>");

                      }



                  }



              }
              else
              {

                  Response.Write("<div class=\"order\">暂时没有订单.</div>");

              } %>

        
    </div>

      <%if (ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner.Equals("wubuhui"))
        {%>
                 <div class="footerbar">
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="javascript:pagegoback('/Wubuhui/MyCenter/Index.aspx')">
                <span class="iconfont icon-back"></span></a>
        </div>
        <!-- /.col-lg-2 -->
        <div class="col-xs-8">
            
        </div>
        <!-- /.col-lg-10 -->
        <div class="col-xs-2 ">
            <a class="wbtn wbtn_main" type="button" href="/Wubuhui/MyCenter/Index.aspx"><span class="iconfont icon-b11">
            </span></a>
        </div>
        <!-- /.col-lg-2 -->
    </div>
    <!-- footerbar -->
     
     <%}%>
     <%else
         {%>
      <div class="backbar">
        <a href="ScoreManage.aspx" class="back"><span class="icon"></span></a>
    </div>


     <%} %>

</section>
</body>
<script type="text/javascript">

    var handlerurl = "/Handler/App/WXMallHandler.ashx";
    var orderid = "";
    $(function () {

        $("[data-oid]").click(function () {

            orderid = $(this).attr("data-oid");
            if (confirm("确定取消订单 " + orderid)) {
                $.ajax({
                    type: 'post',
                    url: handlerurl,
                    data: { Action: 'CancelScoreOrder', orderid: orderid },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 1) {

                            $("[data-oid]").each(function () {
                                if ($(this).attr("data-oid") == orderid) {
                                    $(this).remove();
                                }

                            })
                            $("[status-oid]").each(function () {
                                if ($(this).attr("status-oid") == orderid) {
                                    $(this).html("已取消");
                                }

                            })
                            return false;




                        }
                        else {
                            alert(resp.Msg);
                            return false;
                        }
                    }
                });
            }
            else {
                return false;
            }


        })

    })
</script>
</html>
