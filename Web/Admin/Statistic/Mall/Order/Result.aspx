<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Statistic.Mall.Order.Result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .blue {
            color: blue;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;统计&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>订单统计</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <table id="grvData" fitcolumns="true">
    </table>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Serv/Api/Admin/Mall/Statistics/Order/GetByTaskId.ashx";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { task_id: "<%=Request["taskId"]%>" },
                       height: document.documentElement.clientHeight - 50,
                       loadFilter: pagerFilter,
                       pagination: true,
                       striped: true,
                       pageSize: 50,
                       //rownumbers: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[

                                  {
                                      field: 'time_range', title: '统计时间', width: 30, align: 'left', formatter: function (value, rowData) {
                                          var str = new StringBuilder();
                                          str.AppendFormat('{0}&nbsp;至&nbsp;{1}', rowData.from_date, rowData.to_date);
                                          return str.ToString();
                                      }
                                  },
                                   {
                                       field: 'total_count', title: '交易成功总订单数', width: 15, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('{0}&nbsp;<a class="blue" target="_blank" href="Detail.aspx?taskid={1}&type=All" >查看<a>', value,rowData.task_id);
                                           return str.ToString();
                                       }
                                   },
                                   { field: 'total_product_count', title: '商品总件数', width: 15, align: 'left' },
                                   { field: 'total_product_fee', title: '商品总价值', width: 15, align: 'left' },
                                   { field: 'total_transport_fee', title: '商品总运费(元)', width: 15, align: 'left' },
                                   { field: 'total_amount', title: '货币支付(元)', width: 15, align: 'left' },
                                   { field: 'total_refund_amount', title: '退款金额(元)', width: 15, align: 'left' },
                                   //{ field: 'base_total_amount', title: '订单总基价(元)', width: 15, align: 'left' },
                                   { field: 'total_coupon_exchang_amount', title: '优惠券支付(元)', width: 15, align: 'left' },
                                   { field: 'total_storecard_exchang_amount', title: '储值卡支付(元)', width: 15, align: 'left' },
                                   { field: 'total_score_exchang_amount', title: '积分支付(元)', width: 15, align: 'left' },
                                   { field: 'total_accountamount_exchang_amount', title: '余额支付(元)', width: 15, align: 'left' },
                                   {
                                       field: 'should_commission_order_count', title: '应该分佣订单订单数', width: 30, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();

                                           str.AppendFormat('{0}&nbsp;<a class="blue" target="_blank" href="Detail.aspx?taskid={1}&type=ShouldCommission" >查看<a>', value, rowData.task_id);
                                           return str.ToString();
                                       }
                                   },
                                   {
                                       field: 'real_commission_order_count', title: '实际分佣订单订单数', width: 30, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('{0}&nbsp;<a class="blue" target="_blank" href="Detail.aspx?taskid={1}&type=RealCommission" >查看<a>', value, rowData.task_id);

                                           return str.ToString();
                                       }
                                   },
                                   {
                                       field: 'last_receiving_time', title: '预计最后一个订单确认收货时间', width: 30, align: 'left'
                                   },

                                   {
                                       field: 'refund_order_count', title: '订单交易成功且在退款中的订单数', width: 30, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('{0}&nbsp;<a class="blue" target="_blank" href="Detail.aspx?taskid={1}&type=Refund" >查看<a>', value, rowData.task_id);
                                           return str.ToString();
                                       }
                                   },
                                   {
                                       field: 'wait_process_order_count', title: '待处理订单数', width: 30, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('{0}&nbsp;<a class="blue" target="_blank" href="Detail.aspx?taskid={1}&type=WaitProcess" >查看<a>', value, rowData.task_id);
                                           return str.ToString();
                                       }
                                   }




                       ]]
                   }
            );






        });





    </script>
</asp:Content>
