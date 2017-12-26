<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="OrderDelivery.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.OrderDelivery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">提交订单</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<style>
.total{height:auto;}
.orderinfobox .order{margin-bottom:100px;}
</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box">
    <div class="deliverytype nopaddingbottom">
        <div href="#" class="addressbox noaddressbox" id="person">
            <span class="noaddress">点击添加收货人信息</span>
            <span class="name">收货人:<span class="nameinfo"><%=currentUserInfo.TrueName==null?"":currentUserInfo.TrueName%></span></span>
            <span class="phone"><%=currentUserInfo.Phone == null ? "" : currentUserInfo.Phone%></span>
            <span class="address">地址 : <span class="addressinfo"><%=currentUserInfo.Address == null ? "" : currentUserInfo.Address%></span></span>
            <span class="icon"></span>
        </div>
        <div class="orderinfo">
            <div class="checkbox" id="delivery1">
                <h2>配送方式</h2>
               <select id="ddldelivery" style="margin-left:10px;">
                <%=sbDelivery.ToString()%>
                </select>   
            </div>
            <div class="checkbox" id="paytype1">
                <h2>支付方式</h2>
                <select id="ddlpaymenttype" style="margin-left:10px;">
                <%=sbPaymentType.ToString()%>
                </select>
            </div>
            <textarea name="" id="txtOrderMemo" placeholder="给卖家留言" rows="2"></textarea>
        </div>
    </div>
    <div class="checkinfo" id="personbox">
        <input id="txtLinkerName" class="textinput personname" type="text" placeholder="姓名" value="<%=currentUserInfo.TrueName==null?"":currentUserInfo.TrueName%>"/>
        <input id="txtLinkerPhone" class="textinput persontell" type="tel" placeholder="联系电话" value="<%=currentUserInfo.Phone==null?"":currentUserInfo.Phone%>"/>
        <textarea id="txtLinkerAddress" class="personaddress" placeholder="收货地址"><%=currentUserInfo.Address == null ? "" : currentUserInfo.Address%></textarea>
        <div class="btn orange close">取消</div>
        <div class="btn orange close saveaddress">确定</div>
    </div>
    <div class="orderinfobox paddingbottom">
        <div class="order" id="orderconfirm">
        </div>
    </div>
    <div class="total">
        共计<span class="totalnum" id="lbltotalnum">0</span>件商品.商品金额<span class="totalprice" id="lbltotalprice">0.00</span>元.物流费用<span class="totalnum" id="sptransportFee">0</span>元.应付金额:<span class="totalnum" id="sptotal">0</span>元
    </div>
    <div class="backbar">
        <a href="OrderV1.aspx" class="back"><span class="icon"></span></a>
        <a href="javascript:" id="btnSumbitOrder" class="btn orange">提交订单</a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script src="/Scripts/wxmall/initv1.js" type="text/javascript"></script>
<script src="/Scripts/wxmall/orderdelivery.js" type="text/javascript"></script>
</asp:Content>