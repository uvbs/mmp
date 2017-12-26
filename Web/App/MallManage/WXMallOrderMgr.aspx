<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXMallOrderMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallOrderMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置:&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>订单管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowUpdateOrderStatusBatch()">批量修改订单状态</a>
            <%if (IsDistributionMall)
              {%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="ShowUpdateDistributionOrderStatusBatch()">批量修改分销订单状态</a>
            <%} %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除订单</a> <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-excel'"
                    id="btnExportToFile">导出订单</a> <a href="javascript:;"  class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                        onclick="EfastSyn()">同步订单</a> 订单状态:
            <%for (int i = 0; i < OrderStatusList.Count; i++)
              {
                  Response.Write(string.Format("<input type=\"checkbox\" name=\"cborderstatus\" id=\"cb_{0}\" value=\"{1}\" checked=\"checked\"><label id=\"lbl_{0}\" for=\"cb_{0}\">{1}</label>", i, OrderStatusList[i].OrderStatu));

              } %>
            <br />
            <label>
                付款状态</label>
            <select id="ddlpaymentstatus">
                <option value="">全部</option>
                <option value="0">未付款</option>
                <option value="1">已付款</option>
            </select>
            <%--            <label style="margin-left: 8px;">
                订单状态</label>
            <select id="ddlOrderStatus">
                <option value="">全部</option>
                
            </select>--%>
            <label style="margin-left: 8px;">
                分类</label>
            <select id="ddlCategory">
                <option value="">全部</option>
                <%=sbCategory.ToString()%>
            </select>
            <label style="margin-left: 8px;">
                订单编号:</label>
            <input type="text" id="txtOrderID" style="width: 200px;" />
            <label style="margin-left: 8px;">
                时间:</label>
            <input type="text" id="txtFrom" style="width: 100px;" readonly="readonly" class="easyui-datebox" />-
            <input type="text" id="txtTo" style="width: 100px;" readonly="readonly" class="easyui-datebox" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgOrderStatus" class="easyui-dialog" closed="true" title="批量修改订单状态" style="width: 300px;
        padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td style="width: 70px;">
                    订单状态:
                </td>
                <td>
                    <select id="ddlOrderStatusBatch">
                        <%=sbOrderStatuList.ToString()%>
                    </select>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgDistributionOrderStatus" class="easyui-dialog" closed="true" title="批量修改分销订单状态"
        style="width: 300px; padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td style="width: 80px;">
                    分销订单状态:
                </td>
                <td>
                    <select id="ddlDistributionOrderStatusBatch">
                        <option value="0">未付款</option>
                        <option value="1">已付款</option>
                        <option value="2">已收货</option>
                        <option value="3">已审核</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     $(function () {

         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallOrderInfo" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            { field: 'OrderID', title: '订单编号', width: 80, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a  style="color:#005ea7;" title="点击查看订单详情" href="/App/MallManage/WXMallOrderDetail.aspx?oid={0}">{1}</a>', rowData.OrderID, value);
                                return str.ToString();
                            }
                            },
                            { field: 'PaymentType', title: '支付方式', width: 100, align: 'center', formatter: function (value, rowData) {
                                switch (rowData.PaymentType) {
                                    case 0:
                                        return "线下支付";
                                        break;
                                    case 1:
                                        return "支付宝";
                                        break;
                                    case 2:
                                        return "微信支付";
                                        break;
                                    default:
                                        return "";
                                        break;

                                }

                            }
                            },
                            { field: 'PaymentStatus', title: '付款状态', width: 100, align: 'center', formatter: FormatterPaymentStatus },
                            { field: 'Status', title: '订单状态', width: 100, align: 'center', formatter: FormatterOrderStatus },
                            <%if (IsDistributionMall){%>
                            { field: 'DistributionStatus', title: '分销订单状态', width: 100, align: 'center', formatter: FormatterDistributionStatus },
                            <%}%>
                            { field: 'CategoryName', title: '分类', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Consignee', title: '下单人', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Phone', title: '手机号', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'TotalAmount', title: '总金额', width: 100, align: 'center', formatter: formartcolor },
                            { field: 'ProductCount', title: '商品总数量', width: 100, align: 'center' },
                            { field: 'InsertDate', title: '下单时间', width: 100, align: 'center', formatter: FormatDate },
                            { field: 'EditCloum', title: '详情', width: 50, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a target="_blank" href="/App/MallManage/WXMallOrderDetail.aspx?oid={0}">查看</a>', rowData.OrderID);
                                return str.ToString();
                            }
                            }




                             ]]
	            }
            );

         $(".datebox :text").attr("readonly", "readonly");



         //导出文件
         $("#btnExportToFile").click(function () {

             var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
             if (!EGCheckIsSelect(rows)) {
                 return;
             }
             $.messager.confirm('系统提示', '确认导出所选订单？', function (o) {
                 if (o) {

                     ExportOrder(GetRowsIds(rows).join(','));

                 }
             });

         });

         //批量修改订单状态
         $('#dlgOrderStatus').dialog({
             buttons: [{
                 text: '确定',
                 handler: function () {
                     try {
                         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
                         var dataModel = {
                             Action: "UpdateOrderStatusBatch",
                             orderids: GetRowsIds(rows).join(','),
                             Status:$("#ddlOrderStatusBatch").val()
                         }
                         if (dataModel.Status=="已发货") {
                            Alert("已发货状态不能批量修改,请进入订单详情修改");
                            return false;
}

                         $.ajax({
                             type: 'post',
                             url: handlerUrl,
                             data: dataModel,
                             dataType: "json", 
                             success: function (resp) {
                                 if (resp.Status == 1) {
                                     Show("操作成功");
                                     $('#dlgOrderStatus').dialog('close');
                                     $('#grvData').datagrid('reload');
                                 }
                                 else {
                                     Alert(resp.Msg);
                                 }


                             }
                         });

                     } catch (e) {
                         Alert(e);
                     }
                 }
             }, {
                 text: '取消',
                 handler: function () {
                     $('#dlgOrderStatus').dialog('close');
                 }
             }]
         });

         //批量修改分销订单状态
         $('#dlgDistributionOrderStatus').dialog({
             buttons: [{
                 text: '确定',
                 handler: function () {
             var orderstatus = $("#ddlDistributionOrderStatusBatch").find("option:selected").text();
             if ($("#ddlDistributionOrderStatusBatch").val() == "3") {
                 orderstatus += "<br/>修改为已审核后，分销佣金将会充入上级用户的账户中，是否继续";
             }
             $.messager.confirm("系统提示", "确定将所选订单分销状态更改为:" + orderstatus, function (o) {
                 if (o) {
                         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
                         var dataModel = {
                             Action: "UpdateDistributionOrderStatusBatch",
                             orderids: GetRowsIds(rows).join(','),
                             Status: $("#ddlDistributionOrderStatusBatch").val()
                         }
                         $.ajax({
                             type: 'post',
                             url: handlerUrl,
                             data: dataModel,
                             dataType: "json",
                             success: function (resp) {
                                 if (resp.Status == 1) {
                                     Show("操作成功");
                                     $('#dlgDistributionOrderStatus').dialog('close');
                                     $('#grvData').datagrid('reload');
                                 }
                                 else {
                                     Alert(resp.Msg);
                                 }


                             }
                         });

                     } 
                 //

                 }
             );
                 }
             }, {
                 text: '取消',
                 handler: function () {
                     $('#dlgDistributionOrderStatus').dialog('close');
                 }
             }]
         });

         $("input[name='cborderstatus']").click(function(){
          Search();
         });

     });

     //导出订单
     function ExportOrder(oids) {
         window.open(handlerUrl + "?Action=ExportOrder&oids=" + oids);

     }

     //删除
     function Delete() {

         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确定删除选中订单?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DeleteWXMallOrderInfo", oid: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             Alert(resp.Msg);
                             $('#grvData').datagrid('reload');

                         }
                         else {
                             Alert(resp.Msg);
                         }


                     }

                 });
             }
         });


     }

     //efast订单同步
     function EfastSyn() {

         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }

         $.messager.confirm("系统提示", "确定同步选中订单?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "EfastSynWXMallOrderInfo", orderId: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             Alert(resp.Msg);
                            

                         }
                         else {
                             Alert(resp.Msg);
                         }


                     }

                 });
             }
         });


     }

     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].OrderID
                 );
         }
         return ids;
     }


     function Search() {

         var FromDate = $("#txtFrom").datebox('getValue');
         var ToDate = $("#txtTo").datebox('getValue');
         //var OrderStatus = $("#ddlOrderStatus").val();
         var OrderStatus=GetSelectOrderStatus();
         var PaymentStatus = $("#ddlpaymentstatus").val();
         //var StoreId = $("#ddlStores").val();
         var CategoryId = $("#ddlCategory").val();
         //            if (FromDate != "") {
         //                if (!CheckDateTime(FromDate)) {
         //                    $.messager.alert("系统提示", "开始时间不正确", "warning");
         //                    return;
         //                }
         //            }
         //            if (ToDate != "") {
         //                if (!CheckDateTime(ToDate)) {
         //                    $.messager.alert("系统提示", "结束时间不正确", "warning");
         //                    return;
         //                }

         //            }
         //            if (FromDate != "" && ToDate != "") {

         //                if (From > To) {
         //                    $.messager.alert("系统提示", "开始时间不能大于结束时间", "warning");
         //                    return;
         //                }
         //            }



         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallOrderInfo", OrderID: $("#txtOrderID").val(), FromDate: FromDate, ToDate: ToDate, OrderStatus: OrderStatus, CategoryId: CategoryId, PaymentStatus: PaymentStatus }
	            });
     }


     //验证长时间
     //function CheckDateTime(str) {
         //            var r ＝ str.match(/^(/d{1,4})(-|//)(/d{1,2})/2(/d{1,2})$/); 
         //            if(r＝＝null)return false; 
         //            var d＝ new Date(r[1], r[3]-1, r[4]); 
         //            return (d.getFullYear()＝＝r[1]&&(d.getMonth()+1)＝＝r[3]&&d.getDate()＝＝r[4]);


    // }

     function formartcolor(value) {

         return "<font color='red'>" + value + "</font>";
     }

     function FormatterPaymentStatus(value) {

        
         switch (value) {
             case 0:
                 return "<font >未付款 </font>";
                 break;
             case 1:
                 return "<font color='green'>已付款 </font>";
                 break;
             default:


         }


     }

    function FormatterDistributionStatus(value, rowData) {


         switch (value) {
             case 0:
                 return "<font>未付款</font>";
                 break;
             case 1:
                 return "<font>已付款</font>";
                 break;
              case 2:
                 return "<font>已收货</font>";
                 break;
              case 3:
                 return "<font>已审核</font>";
                 break;
             default:


         }


     }

     function FormatterOrderStatus(value, rowData) {
         switch (value) {
             case "已付款":
              return "<font color='green'>"+value+"</font>";
              break;
             case "已发货":
              return "<font color='green'>"+value+"</font>";
              break;
             case "交易成功":
                 return "<font color='green'>"+value+"</font>";
                 break;
              case "交易完成":
                 return "<font color='green'>"+value+"</font>";
                 break;
               case "等待处理":
                 return "<font color='red'>"+value+"</font>";
                 break;
               case "待发货":
                 return "<font color='red'>"+value+"</font>";
                 break;
                case "已取消":
                 return "<del>"+value+"</del>";
                 break;
             default:
             return value;
             break;

         }


     }
     //显示
     function ShowUpdateOrderStatusBatch() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $('#dlgOrderStatus').dialog('open');
     }
     function ShowUpdateDistributionOrderStatusBatch() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $('#dlgDistributionOrderStatus').dialog('open');
     }
     //获取选中订单状态
     function GetSelectOrderStatus(){
      if ($("input[name='cborderstatus']:checked").val() != undefined) {
      var strorderstatus="";
     $("input[name='cborderstatus']:checked").each(function () {
      strorderstatus+="'"+$(this).val()+"'"+",";
         })
         strorderstatus=strorderstatus.substring(0,strorderstatus.length-1);
         return strorderstatus;
        }
        else {
            return "";
}
     
     }


     
    </script>
</asp:Content>
