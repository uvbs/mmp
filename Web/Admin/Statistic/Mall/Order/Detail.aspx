<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Statistic.Mall.Order.Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style>

        .blue {
            color: blue;
        }
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
    当前位置：&nbsp;统计&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>订单统计明细</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

<%--    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <input type="text" id="txtKeyWord" style="width: 200px;" placeholder="商品名称" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search"
                onclick="Search();">查询</a>
        </div>
    </div>--%>


    <table id="grvData" fitcolumns="true">
    </table>



</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Serv/Api/Admin/Mall/Statistics/Order/OrderDetail.ashx";
        $(function () {


            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { task_id: "<%=Request["taskId"]%>", type: "<%=Request["type"]%>", },
                       height: document.documentElement.clientHeight - 50,
                       loadFilter: pagerFilter,
                       pagination: true,
                       striped: true,
                       pageSize: 50,
                       rownumbers: true,
                       singleSelect: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[

                                  {
                                      field: 'order_id', title: '订单号', width: 100, align: 'left', formatter: function (value, rowData) {
                                          var str = new StringBuilder();
                                          str.AppendFormat('{0}&nbsp;<a class="blue" href="/customize/mmpadmin/index.aspx?hidemenu=1#/index/orderDetail/{0}" target="_blank">查看<a>', value);
                                          return str.ToString();
                                      }
                                  }
                                   
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

<%--               function Search() {

                   $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { task_id: "<%=Request["taskId"]%>", keyword: $(txtKeyWord).val() },
            });


   }--%>

    </script>
</asp:Content>
