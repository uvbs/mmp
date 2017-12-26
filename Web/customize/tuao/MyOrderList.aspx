<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="MyOrderList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.MyOrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    我的订单</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="orderbox">
        
           <%  
               ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
               List<ZentCloud.BLLJIMP.Model.WXMallOrderInfo> orderList = bllMall.GetList<ZentCloud.BLLJIMP.Model.WXMallOrderInfo>(string.Format("OrderUserID='{0}' And WebsiteOwner='{1}' Order By InsertDate DESC", bllMall.GetCurrUserID(), bllMall.WebsiteOwner));
               if (orderList.Count > 0)
               {

                   foreach (var order in orderList)
                   {
                       Response.Write(string.Format("<a href=\"javascript:void(0)\" class=\"order\">"));
                       List<ZentCloud.BLLJIMP.Model.WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(order.OrderID);
                       string PaymentStatus = order.PaymentStatus == 0 ? "<font color='red'>未付款</font>" : "<font color='green'>已付款</font>";
                       Response.Write(string.Format("<span class=\"orderdata\" >订单编号:{0}&nbsp;{2}&nbsp;&nbsp;<label status-oid=\"{0}\">{1}&nbsp;{3}</label></span>", order.OrderID, order.Status, order.InsertDate.ToString("yyyy-MM-dd"), PaymentStatus));
                       foreach (var orderdetail in orderDetailList)
                       {
                           ZentCloud.BLLJIMP.Model.WXMallProductInfo productInfo = bllMall.Get<ZentCloud.BLLJIMP.Model.WXMallProductInfo>(string.Format("PID='{0}'", orderdetail.PID));
                           if (productInfo != null)
                           {
                               Response.Write("<div class=\"product\">");
                               Response.Write(string.Format("<img src=\"{0}\" >", productInfo.RecommendImg));
                               Response.Write("<div class=\"info\">");
                               Response.Write(string.Format("<span class=\"text\">{0}</span>", productInfo.PName));
                               Response.Write(string.Format("<span class=\"price\">￥{0}<span class=\"num\">x{1}</span></span>", orderdetail.OrderPrice, orderdetail.TotalCount));
                               Response.Write("</div>");
                               Response.Write("</div>");



                           }



                       }




                       Response.Write("<div class=\"total\">");
                       Response.Write("共计");
                       Response.Write(string.Format("<span class=\"totalnum\">{0}</span>件商品,", order.ProductCount));
                       Response.Write(string.Format("<span class=\"totalprice\">{0}</span>", order.TotalAmount));
                       Response.Write("<span class=\"orangetext\">元</span>");


                       Response.Write("</div>");
                       Response.Write("</a>");

                       if (order.Status.Equals("等待处理"))
                       {
                           //if (order.WebsiteOwner.Equals("qianwei"))
                           //{

                           //    Response.Write("<div class=\"order\" style=\"background-color:#eef1f4;box-shadow:0 0 0;text-align:right;\">");
                           //    if (order.PaymentStatus.Equals(0) && (order.PaymentType.Equals(1) || order.PaymentType.Equals(2)))
                           //    {
                           //        if (order.PaymentType.Equals(1))
                           //        {
                           //            Response.Write(string.Format("<a  class=\"btn orange\" href=\"DoAlipay.aspx?oid={0}\">付款</a>", order.OrderID));

                           //        }
                           //        else if (order.PaymentType.Equals(2))
                           //        {
                           //            Response.Write(string.Format("<a  class=\"btn orange\" href=\"DoWxPay.aspx?oid={0}\">付款</a>", order.OrderID));

                           //        }

                           //    }

                           //}
                           //else
                           //{
                           Response.Write("<div class=\"order\" style=\"background-color:#eef1f4;box-shadow:0 0 0;text-align:right;\">");
                           if (order.PaymentStatus.Equals(0))
                           {

                               Response.Write(string.Format("<a  class=\"btn orange\" href=\"DoWxPay.aspx?oid={0}\">付款</a>", order.OrderID));



                           }
                           if (order.Status == "等待处理" && order.PaymentStatus.Equals(0))
                           {
                               Response.Write(string.Format("<div data-oid=\"{0}\"  href=\"javascript:void(0);\"  class=\"btn orange\">取消订单</div>", order.OrderID));

                           }



                           Response.Write("</div>");

                           //}



                       }
                       if (order.Status.Equals("已发货"))
                       {


                           Response.Write("<div class=\"order\" style=\"background-color:#eef1f4;box-shadow:0 0 0;text-align:right;\">");
                           
                           Response.Write("快递公司:" + order.ExpressCompanyName);
                           Response.Write("<br/>运单号:"+order.ExpressNumber+"<br/>");
                           
                           Response.Write(string.Format("<a  class=\"btn orange\" href=\"http://m.kuaidi100.com/index_all.html?type={0}&postid={1}&callbackurl={2}\">查看物流</a>", order.ExpressCompanyCode, order.ExpressNumber, Request.Url.ToString()));
                           Response.Write(string.Format("<div data-receive=\"{0}\"  href=\"javascript:void(0);\"  class=\"btn orange\">确认收货</div>", order.OrderID));
                           Response.Write("</div>");

                       }
                   }

               }
               else
               {

                   Response.Write("<div class=\"order\">暂时没有订单.</div>");

               } %>

        
    </div>

</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
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
                        data: { Action: 'CancelOrder', orderid: orderid },
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
                                //alert(resp.Msg);
                                msgText.init(resp.Msg, 3000);

                                return false;
                            }
                        }
                    });
                }
                else {
                    return false;
                }


            })
            $("[data-receive]").click(function () {
                orderid = $(this).attr("data-receive");
                if (confirm("确定收货? 订单号: " + orderid)) {
                    $.ajax({
                        type: 'post',
                        url: handlerurl,
                        data: { Action: 'UpdateOrderComplete', orderid: orderid },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                $("[status-oid]").each(function () {
                                    $("[data-receive]").each(function () {
                                        if ($(this).attr("data-receive") == orderid) {
                                            $(this).remove();
                                        }

                                    })
                                    if ($(this).attr("status-oid") == orderid) {
                                        $(this).html("交易成功");
                                    }

                                })
                                return false;
                            }
                            else {
                                msgText.init(resp.Msg, 3000);
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
        <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "土澳网，精心甄选源自澳洲商品的电商平台",
                desc: "土澳网，精心甄选源自澳洲商品的电商平台",
                //link: '', 
                imgUrl: "http://<%=Request.Url.Host%>/customize/tuao/images/logo.png"
            })
        })
    </script>
</asp:Content>
