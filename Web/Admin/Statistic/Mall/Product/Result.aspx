<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Statistic.Mall.Product.Result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style>

       .lbTip {
    padding: 3px 6px;
    background-color: #5C5566;
    color: #fff;
    font-size: 14px;
    border-radius: 50px;
    cursor: pointer;
    margin-left: 20px;
}
       </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;统计&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>商品统计</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <input type="text" id="txtKeyWord" style="width: 200px;" placeholder="商品名称" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search"
                onclick="Search();">查询</a>
        </div>
    </div>


    <table id="grvData" fitcolumns="true">
    </table>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Serv/Api/Admin/Mall/Statistics/Product/GetByTaskId.ashx";
        $(function () {


            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { task_id: "<%=Request["taskId"]%>", sort: "order_total_amount", order: "desc" },
	                height: document.documentElement.clientHeight - 100,
	                loadFilter: pagerFilter,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
                    singleSelect:true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[

                               {
                                   field: 'time_range', title: '统计时间', width: 30, align: 'left', formatter: function (value, rowData) {
                                       var str = new StringBuilder();
                                       str.AppendFormat('{0}&nbsp;至&nbsp;{1}', rowData.from_date, rowData.to_date);
                                       return str.ToString();
                                   }
                               },
                                { field: 'product_name', title: '商品名称', width: 20, align: 'left', sortable: true },
                                { field: 'order_total_count', title: '总订单数', width: 20, align: 'left', sortable: true },
                                 { field: 'product_total_count', title: '商品总件数', width: 20, align: 'left', sortable: true },
                                 { field: 'order_total_order_price', title: '商品总价值', width: 20, align: 'left', sortable: true },
                               
                                { field: 'order_total_amount', title: '货币支付（均摊 ) <span class="lbTip" data-tip-msg="<b>说明</b><br>例如：一个订单有 A商品50元  B商品100元 <br/>  使用优惠券抵扣40元  实付60<br/>均摊在每个商品的金额<br/>  A商品=60*（50/150）；<br/>B商品=60*(100/150)">?</span>', width: 20, align: 'left', sortable: true },
                                { field: 'total_refund_amount', title: '退款总金额(元)', width: 15, align: 'left' }
                                //{ field: 'order_base_total_amount', title: '商品成本(元)', width: 20, align: 'left', sortable: true },
                                //{ field: 'profit', title: '订单总利润(元)', width: 20, align: 'left', sortable: true }
	                ]]
	            }
            );


            $('.lbTip').click(function () {
                var msg = $(this).attr('data-tip-msg');
                layer.tips(msg, $(this));
            });



     });

         function Search() {

            $('#grvData').datagrid(
            {
           method: "Post",
           url: handlerUrl,
           queryParams: { task_id: "<%=Request["taskId"]%>",keyword:$(txtKeyWord).val() },
            });


            }

    </script>
</asp:Content>
