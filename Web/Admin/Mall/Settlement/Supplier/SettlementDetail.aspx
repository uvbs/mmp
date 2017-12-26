<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SettlementDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Mall.Settlement.Supplier.SettlementDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .blue {
            color: blue;
        }

        #tbSett {
            border-collapse: collapse;
            width: 100%;
        }

            #tbSett tr td {
                border: 1px solid #ccc;
                padding-top:8px;
                padding-bottom:8px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;结算&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>结算详情</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <table id="tbSett">
        <tr>
            <td>&nbsp;商户名称:</td>
            <td>&nbsp;<%=model.SupplierName %></td>
            <td>&nbsp;结算单号:</td>
            <td>&nbsp;<%=model.SettlementId %></td>
            <td>&nbsp;结算起止时间</td>
            <td>&nbsp;<%=model.FromDate.ToString() %>至<%=model.ToDate.ToString() %></td>
        </tr>

        <tr>
            <td>&nbsp;产品结算金额:</td>
            <td>&nbsp;<%=model.TotalBaseAmount %></td>
            <td>&nbsp;代收运费:</td>
            <td>&nbsp;<%=model.TotalTransportFee %></td>
            <td>&nbsp;结算金额</td>
            <td>&nbsp;<%=model.SettlementTotalAmount %></td>
        </tr>

        <tr>
            <td>&nbsp;已结算订单数:</td>
            <td>&nbsp;<%=SettlementOrderCount %></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>

    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <input type="text" id="txtOrderId" style="width: 200px;" placeholder="输入订单号查询" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search"
                onclick="Search();">查询</a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-excel" plain="true" onclick="exportToExcel()">导出</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Serv/Api/Admin/Mall/Settlement/Supplier/SettlementDetail.ashx";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { settlement_id: "<%=Request["settlement_id"]%>" },
                       height: document.documentElement.clientHeight - 150,
                       loadFilter: pagerFilter,
                       pagination: true,
                       striped: true,
                       pageSize: 100,
                       rownumbers: true,
                       singleSelect: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                    //{ field: 'settlement_id', title: '结算单号', width: 15, align: 'left' },
                                   {
                                       field: 'order_id', title: '订单号', width: 20, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('{0}&nbsp;<a class="blue" href="/customize/mmpadmin/index.aspx?hidemenu=1#/index/orderDetail/{0}" target="_blank">查看订单<a>', rowData.order_id);
                                           return str.ToString();

                                       }
                                   },
                                   { field: 'baseamount', title: '产品结算金额', width: 20, align: 'left' },
                                   { field: 'transportfee', title: '代收运费', width: 20, align: 'left' },
                                   { field: 'settlement_amount', title: '结算金额(元)<br>结算金额=产品结算金额+代收运费', width: 20, align: 'left' }

                       ]]
                   }
            );






        });

               function Search() {

                   $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { settlement_id: "<%=Request["settlement_id"]%>", order_id: $("#txtOrderId").val() },
                   });


               }

               // 导出excel
               function exportToExcel() {


              var location = "/Serv/Api/Admin/Mall/Settlement/Supplier/ExportSettlementDetail.ashx?settlement_id=" + "<%=model.SettlementId%>";
              window.open(location);



        }

    </script>
</asp:Content>
