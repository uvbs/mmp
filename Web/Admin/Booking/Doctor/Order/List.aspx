<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Booking.Doctor.Order.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置:&nbsp;专家预约&nbsp;&nbsp;&gt;&nbsp;&nbsp;预约管理<span></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">

            <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-edit'" onclick="UpdateStatus('已确认')">修改为 【已确认】</a>

            <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-edit'" onclick="UpdateStatus('失效')">修改为 【失效】</a>



            <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-excel'"
                id="btnExportToFile">导出</a>

            <br />
            类型
            <select id="ddlType">
                <option value="">全部</option>
                <%if (string.IsNullOrEmpty(Request["type"]))
                  {%>

                <option value="5">预约</option>
                <option value="6">推荐</option>
                <%}
                  else
                  {%>
                <option value="7">预约</option>
                <option value="8">推荐</option>

                <%} %>
            </select>
            确认状态
            <select id="ddlStatus">
                <option value="">全部</option>
                <option value="未确认">未确认</option>
                <option value="已确认">已确认</option>
                <option value="失效">已失效</option>
            </select>
            <label style="margin-left: 8px;">
                关键字搜索:</label>
            <input type="text" id="txtKeyWord" style="width: 200px;" placeholder="专家名字,科室" />
            <label style="margin-left: 8px;">
                时间:</label>
            <input type="text" id="txtFrom" style="width: 100px;" class="easyui-datebox" />-
            <input type="text" id="txtTo" style="width: 100px;" class="easyui-datebox" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "../Handler/Handler.ashx";
        var orderType = "<%=Request["type"]%>";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "OrderList", orderType: orderType },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       pageSize: 50,
                       rownumbers: true,
                       singleSelect: false,
                       columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            {
                                field: 'OrderType', title: '类型', width: 100, align: 'center', formatter: function (value) {


                                    switch (value) {
                                        case 5:
                                            return "预约";
                                        case 6:
                                            return "推荐";
                                        case 7:
                                            return "预约";
                                        case 8:
                                            return "推荐";
                                        default:

                                    }
                                }
                            },
                            { field: 'Status', title: '确认状态', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Consignee', title: '姓名', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Phone', title: '手机号', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Ex1', title: '年龄', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Ex2', title: '性别', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Ex3', title: '症状描述', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Ex5', title: '专家', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Ex6', title: '科室', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'InsertDate', title: '提交时间', width: 100, align: 'center', formatter: FormatDate },
                             <%=Columns %>

                       ]]
                   }
            );

            //$(".datebox :text").attr("readonly", "readonly");

            $("#ddlType").change(function () {

                Search();

            })
            $("#ddlStatus").change(function () {

                Search();

            })
            //导出文件
            $("#btnExportToFile").click(function () {

                //var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
                //if (!EGCheckIsSelect(rows)) {
                //    return;
                //}
                $.messager.confirm('系统提示', '确认按所选条件导出？', function (o) {
                    if (o) {

                        ExportOrder();

                    }
                });

            });


        });

        //导出订单
        function ExportOrder() {

            var fromDate = $("#txtFrom").datebox('getValue');
            var toDate = $("#txtTo").datebox('getValue');
            var type = $("#ddlType").val();
            var keyWord = $("#txtKeyWord").val();
            var status = $("#ddlStatus").val();
            window.open("../Handler/ExportOrder.ashx?type=" + type + "&fromdate=" + fromDate + "&todate=" + toDate + "&keyword=" + keyWord + "&status=" + status + "&ordertype=" + orderType);

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
                        data: { Action: "OrderDelete", ids: GetRowsIds(rows).join(',') },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status == true) {
                                Alert(resp.msg);
                                $('#grvData').datagrid('reload');

                            }
                            else {
                                Alert(resp.msg);
                            }


                        }

                    });
                }
            });


        }


        //删除
        function UpdateStatus(status) {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定修改状态为 " + status + "?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "UpdateOrderStatus", ids: GetRowsIds(rows).join(','), status: status },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status == true) {
                                Alert("操作成功");
                                $('#grvData').datagrid('reload');

                            }
                            else {
                                Alert(resp.msg);
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
            var fromDate = $("#txtFrom").datebox('getValue');
            var toDate = $("#txtTo").datebox('getValue');
            var type = $("#ddlType").val();
            var status = $("#ddlStatus").val();
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "OrderList", KeyWord: $("#txtKeyWord").val(), FromDate: fromDate, toDate: toDate, type: type, status: status, orderType: orderType }
                   });
        }
    </script>
</asp:Content>
