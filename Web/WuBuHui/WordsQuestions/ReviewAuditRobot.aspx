<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" 
CodeBehind="ReviewAuditRobot.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WordsQuestions.ReviewAuditRobot" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/kindeditor-4.1.10/themes/default/default.css" rel="stylesheet" type="text/css" />
    <script src="/kindeditor-4.1.10/kindeditor.js" type="text/javascript"></script>
    <script src="/kindeditor-4.1.10/lang/zh_CN.js" type="text/javascript"></script>
    <script type="text/javascript">
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
                                { field: 'Receivers', title: '接收列表', width: 5, align: 'left' },
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


        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;话题审核 &nbsp;&gt;&nbsp;&nbsp;<span>自动审核设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <span>自动审核关键字：（每行一个）</span>

    <div id="divEditor">
        <div id="txtNcontent" style="width: 100%; height: 400px;">
        </div>
    </div>

</asp:Content>