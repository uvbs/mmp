<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="OrderDeliveryV2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.OrderDeliveryV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">提交订单</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<style>select{margin-top:5px;}</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section class="box">
    <div class="orderinfobox">
        <div class="order">
            <div class="orderdata"><%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %></div>
            <div id="orderconfirm">


            </div>


           <div class="total">
            共计<span class="totalnum" id="lbltotalnum">0</span>件商品
            </div>
            <div class="total">商品总金额 <span class="totalprice" id="lbltotalprice">0.00</span><span class="orangetext">元</span>
            </div>
           <div class="total">
            物流费用<span class="totalprice" id="sptransportFee">0.00</span><span class="orangetext">元</span>
            </div>
             <div class="total">
            应付金额<span class="totalprice" id="sptotal">0.00</span><span class="orangetext">元</span>
            </div>

        </div>
    </div>

    <div class="deliverytype margintop10">
        <div class="orderinfo">
            <input id="txtLinkerName" class="textinput personname" type="text" placeholder="姓名" value="<%=currentUserInfo.TrueName==null?"":currentUserInfo.TrueName%>"/>
            <input id="txtLinkerPhone" class="textinput persontell" type="tel" placeholder="手机号" value="<%=currentUserInfo.Phone == null ? "" : currentUserInfo.Phone%>"/>
            <textarea id="txtLinkerAddress" class="personaddress" placeholder="收货地址"><%=currentUserInfo.Address == null ? "" : currentUserInfo.Address%></textarea>
            <textarea  id="txtOrderMemo" placeholder="给卖家留言" rows="2"></textarea>
               <%
                DateTime dtNow = DateTime.Now;
                Response.Write("<div class=\"deliverytime\">");
                Response.Write("就餐时间:");
                Response.Write(string.Format("<select id=\"ddlday\"><option value=\"{0}\">今天</option><option value=\"{1}\">明天</option></select>", dtNow.ToString("yyyy-MM-dd"), dtNow.AddDays(1).ToString("yyyy-MM-dd")));

                Response.Write("<select id=\"ddlhour\"><option value=\"00\">0</option><option value=\"01\">1</option><option value=\"02\">2</option><option value=\"03\">3</option><option value=\"04\">4</option><option value=\"05\">5</option><option value=\"06\">6</option><option value=\"07\">7</option><option value=\"08\">8</option><option value=\"09\">9</option><option value=\"10\">10</option><option value=\"11\">11</option><option value=\"12\" >12</option><option value=\"13\">13</option><option value=\"14\">14</option><option value=\"15\">15</option><option value=\"16\">16</option><option value=\"17\">17</option><option value=\"18\">18</option><option value=\"19\">19</option><option value=\"20\">20</option><option value=\"21\">21</option><option value=\"22\">22</option><option value=\"23\">23</option></select>&nbsp;点&nbsp;");

                Response.Write("<select id=\"ddlmin\"><option value=\"00\">00</option><option value=\"05\">05</option><option value=\"10\">10</option><option value=\"15\">15</option><option value=\"20\">20</option><option value=\"25\">25</option><option value=\"30\">30</option><option value=\"35\">35</option><option value=\"40\">40</option><option value=\"45\">45</option><option value=\"50\">50</option><option value=\"55\">55</option></select>&nbsp;分");


                Response.Write("</div>");
                
                
             %>
             <div class="deliverytime">
             配送方式:
             <select id="ddldelivery">
             <%=sbDelivery.ToString()%>
             
             </select>
             </div>
              <div class="deliverytime">
              支付方式:
              <select id="ddlpaymenttype">
             <%=sbPaymentType.ToString()%>
             </select>
             </div>
        </div>

    </div>

    <div class="backbar">
        <a href="indexv2.aspx" class="back"><span class="icon"></span></a>
        <a href="javascript:void(0)" class="btn orange" id="btnSumbitOrder">提交订单</a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script src="/Scripts/wxmall/initv2.js" type="text/javascript"></script>
</asp:Content>