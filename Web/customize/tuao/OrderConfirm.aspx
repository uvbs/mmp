<%@ Page Title="" Language="C#" MasterPageFile="~/customize/tuao/Master.Master" AutoEventWireup="true"
    CodeBehind="OrderConfirm.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.tuao.OrderConfirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    确认订单
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/css/wxmall/wxmallv1.css" rel="stylesheet" type="text/css" />
    <style>
        #header
        {
            position: relative;
        }
        #txtCouponNumber
        {
            height: 30px;
        }
        .menu_order2 ul li
        {
            height: 60px;
        }
        .order_b_btn
        {
            margin-bottom: 150px;
            margin-top: 10px;
        }
        #btnSumbitOrder
        {
            float: right;
        }
        #uladdress li
        {
            line-height: 30px;
            height: 80px;
        }
        .bold
        {
            font-size: 16px;
            font-weight: bold;
        }
        .green
        {
            background-color: #2897AB;
        }
        .form_a
        {
            padding: 10px;
        }
        .order_price2
        {
            margin-top: 10px;
        }
        .form_a{width:auto;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <%
        ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
        List<ZentCloud.BLLJIMP.Model.WXConsigneeAddress> AddressList = bllMall.GetConsigneeAddressList(currentUserInfo.UserID);
    %>
    <div id="header">
        <div class="gohomemenu">
            <a href="javascript:history.go(-1);">
                <img src="images/left.png" /></a></div>
        <div class="homelogo">
            <a href="javascript:void(0)">确认结算</a></div>
    </div>
    <div class="page_padding9">
        <div class="order_price2">
            <table class="goodslist" id="tbproductlist">
            </table>
            <table class="goodslist1">
                <tr>
                    <td>
                        共计
                        <label id="lbltotalnum" class="color2">
                            0</label>
                        件商品
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="order_s_price_r">商品总金额： <span class="color2" id="lbltotalprice">￥123.00</span></span>
                    </td>
                    <td>
                        <%-- <span class="order_s_price_r">物流费用： <span class="color2">￥10.0</span></span>--%>
                    </td>
                    <td>
                        <%--                                                        <span class="order_s_price_r">应付金额： <span class="color2">￥133.00</span></span>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div class="menu_order2">
            <ul>
                <li>
                    <input type="checkbox" id="cbCouponNumber" /><label for="cbCouponNumber">优惠卡号</label><input
                        type="text" id="txtCouponNumber" /><div>
                        </div>
                </li>
            </ul>
        </div>
        <%if (AddressList.Count > 0)
          {%>
        请选择一个收货地址
        <%} %>
        <div class="menu_order2">
            <ul id="uladdress">
                <%
                                                   
                    if (AddressList.Count > 0)
                    {
                        for (int i = 0; i < AddressList.Count; i++)
                        {
                            Response.Write(string.Format("<li>"));
                            Response.Write(string.Format("<input  type=\"radio\" name=\"rdoaddress\" id=\"rdo{0}\" value=\"{0}\"/>", AddressList[i].AutoID));
                            Response.Write(string.Format("<label for=\"rdo{2}\"><span class=\"bold\">{0}</span>&nbsp;<span class=\"bold\">{1}</span></label>", AddressList[i].ConsigneeName, AddressList[i].Phone, AddressList[i].AutoID));
                            Response.Write(string.Format("<br />{0}", AddressList[i].Address));
                            Response.Write("</li>");

                        }
                    }
                    else
                    {
                        Response.Write("<div class=\"m_listbox\">");
                        // Response.Write("<a href=\"AddressinfoCompile.aspx?action=add\" class=\"list\">");
                        // Response.Write(" <span class=\"mark green\"><span class=\"icon add\"></span></span>");
                        Response.Write("<h1>你还没有添加收货地址,请先</h1> <a href=\"AddressinfoCompile.aspx?action=add&returnpath=orderconfirm\" class=\"btn orange\">添加收货地址</a>");
                        // Response.Write("<span class=\"righticon\"></span>");
                        // Response.Write("</a>");
                        Response.Write("</div>");


                    }
                                                        
                %>
            </ul>
        </div>
        <%if (AddressList.Count > 0)
          {%>
        <div class="order_b_btn">
            <a href="javascript:void()" id="btnSumbitOrder" class="form_a green radius6">确认支付</a>
        </div>
        <%} %>
        <div class="clearfix">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var addressid = "";
        $(function () {

            $("input[name='rdoaddress']").click(function () {
                addressid = $(this).val();
            })

        })

    </script>
    <script src="js/tuao.js" type="text/javascript"></script>
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
