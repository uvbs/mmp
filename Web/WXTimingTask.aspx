<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXTimingTask.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WXTimingTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var reviewHandlerUrl = "/Handler/App/CationHandler.ashx";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetTimingTasks" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                animate: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TaskTypeString', title: '任务类型', width: 10, align: 'left' },
                                { field: 'TaskInfoString', title: '任务信息', width: 10, align: 'left' },
                                { field: 'ReceiverTypeString', title: '接收人类型', width: 5, align: 'left' },
                                { field: 'Receivers', title: '接收列表', width: 5, align: 'left'},
                                { field: 'StatusString', title: '任务状态', width: 5, align: 'left' },
                                { field: 'InsertDateString', title: '建立时间', width: 10, align: 'left' },
                                { field: 'ScheduleDateString', title: '计划时间', width: 10, align: 'left' },
                                { field: 'FinishDateString', title: '完成时间', width: 10, align: 'left' }
                             ]]
	            }
            );
        })
        //删除
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DelTimingTasks", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 0) {
                                $('#grvData').datagrid('reload');
                                Show(resp.Msg);
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
                ids.push(rows[i].AutoId);
            }
            return ids;
        }

        //窗体关闭按钮---------------------
        $("#btnExit").live("click", function () {
            $("#dlgPmsInfo").window("close");
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;系统管理 &nbsp;&gt;&nbsp;&nbsp;<span>定时任务</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>

