<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallOrderDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallOrderDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        

        .title
        {
            font-size:14px;
            font-weight:bold;
            font-family:"微软雅黑";
            border: 1px solid #EED97C;
            padding: 0 5px;
            background: #FFFCEB;
           
         }
         .orderstatus
         {
             
            font-size:14px;
            font-weight:bold;
            font-family:"微软雅黑";
            border: 1px solid #EED97C;
            padding: 0 5px;
            background: #FFFCEB;
            margin-top:10px;

             
         }
         .ordercontent
         {
             
            border: 1px solid #DADADA;
            padding: 0 5px 10px;
            background: #EDEDED;
            overflow: visible;
            margin-top:10px;
            min-height:500px;
            line-height:30px;
             
         }
         .orderinnercontent
         {
             
             margin 15 5 5 5;
             padding: 5px 8px;
             background: #fff;
             overflow: visible;
             
             
        }
        .p-list
        {
            
            overflow: hidden;
margin-right: -1px;
border-left: 1px solid #DEDEDE;
            
            
        }
         .p-list table{
            
         border-collapse: collapse;
margin-left: -1px;
 border: 1px solid #DADADA;
border-width: 0 1px;   
         }
         th
         {
          font-weight:normal;
          text-align:center;    
             
         }
         .td
         {
             
              text-align:center;   
         }
         .bottom
         {
             
             
           float:right;
           text-align:right;  
           margin-top:20px;
           }
           .headtitle
           {
            font-size:12px;
            font-weight:bold;
           }
           .head
           {
               font-size:12px;
               font-weight:bold;
               
           }
         input[type=text],select
         {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
            font-weight:bold;
             
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<a href="/App/MallManage/WXMallOrderMgr.aspx" title="返回订单列表" >订单管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;订单: <span><%=orderInfo.OrderID%> </span>

<%--        <a href="WXMallOrderMgr.aspx" style="float: right; margin-right: 20px;" title="返回订单列表"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>--%>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
          <div class="title">
          <strong style=" margin-left:10px;">
          订单号:&nbsp;&nbsp;<%=orderInfo.OrderID%> 
          </strong>
          
          </div>
          <div  class="orderstatus">
          <%if (orderInfo.PaymentType.Equals(1)||orderInfo.PaymentType.Equals(2))
            {%>

            <strong style=" margin-left:10px;">付款状态:<%=orderInfo.PaymentStatus==1?"<font color='green'>已付款</font>":"<font color='red'>未付款</font>"%> </strong>

                
            <%} %>
          <strong style=" margin-left:10px;">订单状态:</strong>
          
          <select id="ddlOrderStatus">
           <%=sbOrderStatuList%> 
           </select>
<%--          <span id="spDeliverStaff" style="display:none;">&nbsp;&nbsp;配送员<select id="ddlDeliverStaff"><%=sbDeliveryStaffList%></select></span>--%>
 
          <span id="spExpress" style="display:none;">&nbsp;
                 快递公司:
                 <select id="ddlExpress">
                 <option value="shunfeng">顺丰速运</option>
                 <option value="ems">EMS</option>
                 <option value="yuantong">圆通速递</option>
                 <option value="yunda">韵达快运</option>
                 <option value="zhongtong">中通快递</option>
                 <option value="huitongkuaidi">汇通快运</option>
                 <option value="tiantian">天天快递</option>
                 <option value="zhaijisong">宅急送</option>
                 <option value="youzhengguonei">邮政国内包裹</option>
                 <option value="fangzhousuti">澳洲方舟国际速递</option>
                  <option value="shentong">申通</option>
                 </select>
                 &nbsp;运单号码：
                 <input id="txtExpressNumber" type="text" value="<%=orderInfo.ExpressNumber%>" />
          </span>
          <a href="javascript:;" id="btnUpdateOrderStatus" style="font-weight:bold;" class="button button-rounded button-primary">更新订单状态</a>
           <a target="_blank" href="PrintOrder.aspx?oid=<%=orderInfo.OrderID%>" id="A1" style="font-weight: bold;" class="button button-rounded button-primary">打印</a> 
           <br />
           <%if (new ZentCloud.BLLJIMP.BLL().GetWebsiteInfoModel().IsDistributionMall==1){%>
           <br />
             分销订单状态:
             <select id="ddlDistributionStatus">
             <option value="0">未付款</option>
             <option value="1">已付款</option>
             <option value="2">已收货</option>
             <option value="3">已审核</option>
             </select>
             <a href="javascript:;" id="btnUpdateDistributionStatus" style="font-weight:bold;"  class="button button-rounded button-primary">更新分销订单状态</a>
          </div>
            <% } %>

          <div class="ordercontent">
          <div><strong style="font-weight:bold;font-size:14px; margin-left:10px;">订单信息</strong></div>

           <div class="orderinnercontent">
           
           <div style=" margin-left:10px;">

            <strong>配送方式:</strong>

              <%
                 switch (orderInfo.DeliveryType)
                    {
                        case 0:
                        Response.Write("<div class=\"headtitle\">快递</div>");
                        if (!string.IsNullOrEmpty(orderInfo.ExpressNumber))
                        {
                          Response.Write(string.Format("<a target=\"_blank\" style=\"color:blue;\" href=\"http://www.kuaidi100.com/chaxun?com={0}&nu={1}\">查看物流状态</a><br/>",orderInfo.ExpressCompanyCode,orderInfo.ExpressNumber));

                        }
                        break;
                        case 1:
                        Response.Write("<div class=\"headtitle\">上门自取</div>");
               
                        break;
                        case 2:
                        Response.Write("<div class=\"headtitle\">卖家承担</div>");                         
                        break;
                         
                        default:
                        Response.Write("<div class=\"headtitle\">卖家承担</div>");
                        break;
                    }
                 Response.Write("<strong>收货人信息</strong>");
                 Response.Write(string.Format("<div>收货人姓名:{0}</div>", orderInfo.Consignee));
                 Response.Write(string.Format("<div>电话:{0}</div>", orderInfo.Phone));
                 Response.Write(string.Format("<div>收货地址:{0}</div>", orderInfo.Address));
                 Response.Write("<strong>留言</strong>");
                 Response.Write(string.Format("<div>{0}</div>", orderInfo.OrderMemo));
                 if (orderInfo.DeliveryTime!=null)
                 {
                     Response.Write(string.Format("<div>配送时间:{0}</div>", orderInfo.DeliveryTime.ToString()));
                 }
              
              %>
               <strong>支付方式:
               
                  <%
                  switch (orderInfo.PaymentType)
                    {
                        case 0:
                        Response.Write("线下支付");
                        
                        break;
                        case 1:
                        Response.Write("支付宝");
               
                        break;
                        case 2:
                        Response.Write("微信支付");                         
                        break;
                         
                        default:
                        Response.Write("线下支付");
                        break;
                    }
              
              %>
               </strong>
      <div><strong>商品清单</strong></div>
     <table cellpadding="0" cellspacing="0" width="100%" rules="cols" style="border: 1px solid #DADADA; " >
      <tbody><tr style="background: #EDEDED;">
        <th width="10%"> 商品编号 </th>
        <th width="12%"> 商品图片 </th>
        <th width="42%"> 商品名称 </th>
        <th width="10%"> 商品价 </th>
        <th width="7%"> 商品数量 </th>
        
      </tr>
      <%foreach (var item in orderDetailList)
        { %>
        
        <% ZentCloud.BLLJIMP.Model.WXMallProductInfo  productInfo = new ZentCloud.BLLJIMP.BLL("").Get<ZentCloud.BLLJIMP.Model.WXMallProductInfo>(string.Format("PID='{0}'", item.PID));%>

        <tr style="border-bottom:1px solid #DADADA;">
        <td class="td"><%=item.PID %></td>
        <td class="td">
         <a target="_blank" title="点击查看商品详情" href="/App/Cation/wap/mall/Showv1.aspx?action=show&pid=<%=productInfo== null ? "" : productInfo.PID%>">
       <img width="50" height="50" src="<%=productInfo== null ? "" : productInfo.RecommendImg%>" >
       </a>
        </td>
        <td class="td">
	
          <a target="_blank" title="点击查看商品详情" href="/App/Cation/wap/mall/Showv1.aspx?action=show&pid=<%=productInfo== null ? "" : productInfo.PID%>">
          <%=productInfo== null ? "该商品不存在" : productInfo.PName%>
          </a>
		</td>
		
        <td class="td">
        <span style="color:Red"> 

        ￥<%=item.OrderPrice %>
        </span>
        </td>
       <td class="td">
       <%=item.TotalCount %>
       </td>

      </tr>


      <%} %>
       
      </tbody>
      </table>
           
           </div>
           
           </div>
           <div style="margin-top:10px;margin-right:20px;margin-bottom:500px;">
           
           <div style="text-align:right;float:right;font-size:14px;">总商品金额:&nbsp;&nbsp;<label style="color:red;">￥<%=orderInfo.Product_Fee %></label>
           
           
            <%if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))
             {%>
               <br />
            优惠券号码:&nbsp;&nbsp;<label style="color:red;"><%=orderInfo.MyCouponCardId %></label>
          
         

             <%} %>
              <br />
             物流费用:&nbsp;&nbsp;<label style="color:red;">￥<%=orderInfo.Transport_Fee %></label>
           

            <br />
          
          
           <strong  style="text-align:right;float:right;font-size:16px;font-weight:bold;">应付总额：&nbsp;<label style="color:Red;font-size:24px;font-weight:bold;">￥<%=orderInfo.TotalAmount%></label> </strong>
           </div>
           
           <div>
           
          

            </div>

           </div>
           
          
          </div>


        </div>
    </div>

   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     $(function () {

         $("#btnUpdateOrderStatus").click(function () {

             var orderstatus = $("#ddlOrderStatus").find("option:selected").text();
             if (orderstatus == "") {
                 Alert("请输入订单状态");
                 return false;
             }
             if ($("#ddlOrderStatus").val() == "已发货" && $("#txtExpressNumber").val() == "") {
                 Alert("请输入运单号码");
                 $("#txtExpressNumber").focus();
                 return false;
             }
             $.messager.confirm("系统提示", "确定将订单状态更改为:" + orderstatus, function (o) {
                 if (o) {
                     $.ajax({
                         type: "Post",
                         url: "/Handler/App/CationHandler.ashx",
                         data: { Action: "UpdateOrderStatus", OrderID: "<%=orderInfo.OrderID%>", Status: orderstatus, ExpressCompanyCode: $("#ddlExpress").val(), ExpressCompanyName: $("#ddlExpress").find("option:selected").text(), ExpressNumber: $("#txtExpressNumber").val() },
                         dataType: "json",
                         success: function (resp) {
                             if (resp.Status == 1) {
                                 Alert("订单状态修改成功!");

                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }

                     });
                 }
             });





         });

         var orderstatus = "<%=orderInfo.Status%>";
         $("#ddlOrderStatus").val(orderstatus);
         if (orderstatus=="已发货") {
             $("#spExpress").show();
         }
         //         $("#ddlOrderStatus").change(function () {

         //             if ($(this).val() == "已配送") {
         //                 $("#spDeliverStaff").show();
         //             }
         //             else {
         //                 $("#spDeliverStaff").hide();
         //             }



         //         });
         $("#ddlOrderStatus").change(function () {

             if ($(this).val() == "已发货") {
                 $("#spExpress").show();
             }
             else {
                 $("#spExpress").hide();
             }


         });

         $("#ddlExpress").val("<%=orderInfo.ExpressCompanyCode%>")
         var distributionStatus = "<%=orderInfo.DistributionStatus%>";
         $("#ddlDistributionStatus").val(distributionStatus);
         $("#btnUpdateDistributionStatus").click(function () {
             var orderstatus = $("#ddlDistributionStatus").find("option:selected").text();

             if ($("#ddlDistributionStatus").val() == "3") {
                 orderstatus += "<br/>修改为已审核后，分销佣金将会充入上级用户的账户中，是否继续";
             }
             $.messager.confirm("系统提示", "确定将订单分销状态更改为:" + orderstatus, function (o) {
                 if (o) {
                     $.ajax({
                         type: "Post",
                         url: "/Handler/App/CationHandler.ashx",
                         data: { Action: "UpdateDistributionOrderStatus", OrderID: "<%=orderInfo.OrderID%>", Status: $("#ddlDistributionStatus").val() },
                         dataType: "json",
                         success: function (resp) {
                             if (resp.Status == 1) {

                                 Alert("订单分销状态修改成功!");

                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }

                     });
                 }
             });





         });



     });
        
    
 </script>
</asp:Content>
