<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="UnSettlementList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Mall.Settlement.Supplier.UnSettlementList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .blue {
            color: blue;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;平台结算&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>未结算</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            
           
            <div>


            订单状态:
            <input type="radio" name="rdoOrderStatus" id="rdoOrderStatusAll" checked="checked" value=""/><label for="rdoOrderStatusAll">全部</label>
            <input type="radio" name="rdoOrderStatus" id="rdoOrderStatus0"  value="待发货"/><label for="rdoOrderStatus0">待发货</label>
            <input type="radio" name="rdoOrderStatus" id="rdoOrderStatus1"  value="已发货"/><label for="rdoOrderStatus1">待收货</label>
            <input type="radio" name="rdoOrderStatus" id="rdoOrderStatus2"  value="退款退货"/><label for="rdoOrderStatus2">退款中</label>
            <input type="radio" name="rdoOrderStatus" id="rdoOrderStatus3"  value="交易成功"/><label for="rdoOrderStatus3">确认收货七天内</label>


            </div>
            商户:
            <select id="ddlSupplier" onchange="searchs()">
                <option value="">全部</option>
                <%foreach (var item in supplierList)
                  {%>
                <option value="<%=item.UserID %>"><%=item.Company %></option>
                <%} %>
            </select>
            下单时间:
             <input class="easyui-datebox" id="txtFrom" />&nbsp;至
             <input class="easyui-datebox" id="txtTo" />
            <input type="text" id="txtKeyWord" style="width: 200px;" placeholder="输入订单号" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search"
                onclick="searchs();">查询</a>
            <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-excel" plain="true" onclick="exportToExcel()">导出</a>
        </div>
    </div>


    <table id="grvData" fitcolumns="true">
    </table>

   

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
       
        var handlerUrl = "/Serv/Api/Admin/Mall/Settlement/Supplier/UnSettlementList.ashx";
       
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { },
                       height: document.documentElement.clientHeight - 150,
                       loadFilter: pagerFilter,
                       pagination: true,
                       striped: true,
                       pageSize: 50,
                       rownumbers: true,
                       singleSelect: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[

                                  { field: 'supplier_name', title: '商户名称', width: 20, align: 'left' },
                                  {
                                    field: 'order_id', title: '订单号', width: 20, align: 'left', formatter: function (value, rowData) {
                                      var str = new StringBuilder();
                                       str.AppendFormat('{0}&nbsp;<a class="blue" href="/customize/mmpadmin/index.aspx?hidemenu=1#/index/orderDetail/{0}" target="_blank">查看订单<a>', rowData.order_id);
                                       return str.ToString();

                                    }
                                  },
                                  { field: 'order_status', title: '订单状态', width: 20, align: 'left' },
                                  { field: 'order_date', title: '下单时间', width: 20, align: 'left' },
                                  { field: 'settlement_baseamount', title: '产品结算金额', width: 20, align: 'left' },
                                  { field: 'settlement_sale_amount', title: '销售额', width: 20, align: 'left' },
                                  { field: 'settlement_server_amount', title: '服务费=销售额-产品结算额', width: 20, align: 'left' },
                                  { field: 'transportfee', title: '代收运费', width: 20, align: 'left' },
                                  { field: 'settlement_amount', title: '结算金额=产品结算金额+代收运费', width: 20, align: 'left' }
                                   
                       ]]
                   }
            );

            $("input[name='rdoOrderStatus']").click(function () {

                searchs();


            })


            //

        });

        //搜索
        function searchs() {

            $('#grvData').datagrid(
            {
                method: "Post",
                url: handlerUrl,
                queryParams: { order_status: $("input[name='rdoOrderStatus']:checked").val(), order_from_date: $("#txtFrom").datebox('getValue'), order_to_date: $("#txtTo").datebox('getValue'), order_id: $("#txtKeyWord").val(), supplier_user_id: $("#ddlSupplier").val() },
            });


        }

      


        // 导出excel
        function exportToExcel() {

            $.messager.confirm("系统提示", "确认按所选条件导出?", function (r) {
                if (r) {
                    var location = "/Serv/Api/Admin/Mall/Settlement/Supplier/ExportUnSettlementList.ashx?order_status=" + $("input[name='rdoOrderStatus']:checked").val() + "&order_from_date=" + $("#txtFrom").datebox('getValue') + "&order_to_date=" + $("#txtTo").datebox('getValue') + "&order_id=" + $("#txtKeyWord").val() + "&supplier_user_id=" + $("#ddlSupplier").val();
                    window.open(location);

                }
            });

        }
    </script>
</asp:Content>