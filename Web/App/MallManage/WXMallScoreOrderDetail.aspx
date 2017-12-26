<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXMallScoreOrderDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallScoreOrderDetail" %>

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
           #txtRemarks{height:30px;width:500px;}
           #ddlOrderStatus{min-width:200px;height:30px;}
           #ddlDeliverStaff{min-width:100px;height:30px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：<a href="WXMallScoreOrderMgr.aspx" title="返回订单列表"> 积分订单</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;订单:
    <span>
        <%=orderInfo.OrderID%>
    </span><a href="WXMallScoreOrderMgr.aspx" style="float: right; margin-right: 20px;"
        title="返回订单列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <div class="title">
                <strong style="margin-left: 10px;">订单号:&nbsp;&nbsp;<%=orderInfo.OrderID%>
                </strong>
            </div>
            <div class="orderstatus">
                <strong style="margin-left: 10px;">订单状态:</strong>
                <select id="ddlOrderStatus">
                    <%=sbOrderStatuList%>
                </select>
                <span id="spDeliverStaff" style="display: none;">&nbsp;&nbsp;配送员<select id="ddlDeliverStaff"><%=sbDeliveryStaffList%></select></span>
                <a href="javascript:;" id="btnUpdateOrderStatus" style="font-weight: bold;" class="button button-rounded button-primary">
                    修改订单状态</a>
                <br />
                <br />
                给客户留言:
                <input id="txtRemarks" value="<%=orderInfo.Remarks %>" placeholder="给客户留言" />
                <a href="javascript:;" id="btnRemark" class="button button-rounded button-primary">保存</a>
            </div>
            <div class="ordercontent">
                <div>
                    <strong style="font-weight: bold; font-size: 14px; margin-left: 10px;">订单信息</strong></div>
                <div class="orderinnercontent">
                    <div style="margin-left: 10px;">
                        <%
                            ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
                            Response.Write(string.Format("<strong>下单用户:{0}</strong>",orderInfo.OrderUserID));
                            Response.Write(string.Format("<div><img src=\"{0}\" width=\"50\" height=\"50\">{1}</div>", orderInfo.OrderUserInfo.WXHeadimgurlLocal, orderInfo.OrderUserInfo.WXNickname));
                            Response.Write("<strong>收货人信息</strong>");
                            Response.Write(string.Format("<div>收货人姓名:{0}</div>", orderInfo.Consignee));
                            Response.Write(string.Format("<div>收货地址:{0}</div>", orderInfo.Address));
                            Response.Write(string.Format("<div>电话:{0}</div>", orderInfo.Phone));
                            Response.Write("<strong>留言</strong>");
                            Response.Write(string.Format("<div>{0}</div>", orderInfo.OrderMemo));               

              
                        %>
                        <table cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #DADADA;">
                            <tbody>
                                <tr style="background: #EDEDED;">
                                    <th width="10%">
                                        商品编号
                                    </th>
                                    <th width="12%">
                                        商品图片
                                    </th>
                                    <th width="42%">
                                        商品名称
                                    </th>
                                    <th width="10%">
                                        商品积分
                                    </th>
                                    <th width="7%">
                                        商品数量
                                    </th>
                                </tr>
                                <%foreach (var item in orderDetailList)
                                  { %>
                                <% ZentCloud.BLLJIMP.Model.WXMallScoreProductInfo ProductInfo = bllMall.Get<ZentCloud.BLLJIMP.Model.WXMallScoreProductInfo>(string.Format("AutoID='{0}'", item.PID));%>
                                <tr style="border-bottom: 1px solid #DADADA;">
                                    <td class="td">
                                        <%=item.PID %>
                                    </td>
                                    <td class="td">
                                        <a target="_blank" title="点击查看商品详情" href="/App/Cation/wap/mall/scoreproductdetail.aspx?pid=<%=ProductInfo== null ? "" : ProductInfo.AutoID.ToString()%>">
                                            <img width="50" height="50" src="<%=ProductInfo== null ? "" : ProductInfo.RecommendImg%>">
                                        </a>
                                    </td>
                                    <td class="td">
                                        <a target="_blank" title="点击查看商品详情" href="/App/Cation/wap/mall/scoreproductdetail.aspx?pid=<%=ProductInfo== null ? "" : ProductInfo.AutoID.ToString()%>">
                                            <%=ProductInfo == null ? "该商品不存在" : ProductInfo.PName%>
                                        </a>
                                    </td>
                                    <td class="td">
                                        <span style="color: Red">
                                            <%=Convert.ToInt32(item.OrderPrice)%>
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
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(function () {
            //修改订单状态
            $("#btnUpdateOrderStatus").click(function () {

                var orderstatus = $("#ddlOrderStatus").find("option:selected").text();
                if (orderstatus == "") {
                    Alert("请输入订单状态");
                    return false;
                }

                $.messager.confirm("系统提示", "确定将订单状态更改为:" + orderstatus, function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: "/Handler/App/CationHandler.ashx",
                            data: { Action: "UpdateScoreOrderStatus", OrderID: "<%=orderInfo.OrderID%>", Status: orderstatus, StatusId: $("#ddlOrderStatus").val(), DeliverStaffId: $("#ddlDeliverStaff").val() },
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
            //修改备注
            $("#btnRemark").click(function () {
                $.ajax({
                    type: "Post",
                    url: "/Handler/App/CationHandler.ashx",
                    data: { Action: "UpdateScoreOrderRemark", OrderID: "<%=orderInfo.OrderID%>", Remarks: $("#txtRemarks").val() },
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 1) {
                            Alert("保存成功!");

                        }
                        else {
                            Alert(resp.Msg);
                        }
                    }

                });

            });
            var orderstatus = "<%=orderInfo.Status%>";
            $("#ddlOrderStatus").val(orderstatus);
            $("#ddlOrderStatus").change(function () {
                if ($(this).val() == "已配送") {
                   // $("#spDeliverStaff").show();
                }
                else {
                    $("#spDeliverStaff").hide();
                }



            });
        });
    </script>
</asp:Content>
