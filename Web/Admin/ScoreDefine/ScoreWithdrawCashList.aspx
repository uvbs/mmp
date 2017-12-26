<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ScoreWithdrawCashList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.ScoreDefine.ScoreWithdrawCashList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%=moduleName %>提现
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-ok" plain="true" id="btnPassItem" onclick="PassItem()">审核通过</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-cancel" plain="true" id="btnRefuseItem" onclick="RefuseItem()">审核不通过</a>
            <span style="color:red;">（只有待审核申请才能处理）</span>
            <br />
            状态:
            <select id="sltStutas" class="easyui-combobox" editable="false">
                <option value="0" selected="selected">待审核</option>
                <option value="2">审核通过</option>
                <option value="3">审核不通过</option>
                <option value="">全部</option>
            </select>&nbsp; 
            申请人:<input type="text" id="txtTrueNames" placeholder="用户名，手机，昵称(模糊)" style="width: 200px" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Serv/API/Admin/ScoreDefine/";
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl+'GetWithdrawCashList.ashx',
                height: document.documentElement.clientHeight - 112,
                loadFilter: pagerFilter,
                pagination: true,
                striped: true,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                queryParams: { type: "ScoreOnLine", status: 0 },
                columns: [[
                    { title: 'ck', width: 5, checkbox: true },
                    { field: 'id', title: '申请编号', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'score', title: '消耗<%=moduleName %>', width: 50, align: 'left', formatter: FormatterTitle },
                    {
                        field: 'amount', title: '提现金额', width: 30, align: 'left', formatter: function (value) {
                            if ($.trim(value) == '') return '';
                            return value + '元';
                        }
                    },
                    { field: 'username', title: '申请人', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'status', title: '状态', width: 30, align: 'left', formatter: FormatterTitle }, 
                    {
                        field: 'time', title: '申请时间', width: 100, align: 'left', formatter: function (value) {
                            return new Date(value).format('yyyy-MM-dd hh:mm:ss');
                        }
                    }
                ]]
            });
        });
        function Search() {
            $('#grvData').datagrid('load', { type: "ScoreOnLine", status: $('#sltStutas').combobox('getValue'), keyword: $('#txtTrueNames').val() });
        }

        function PassItem() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var ids = new Array();
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id);
            }
            $.messager.confirm("系统提示", "确认所选申请审核通过？", function (o) {
                if (o) {
                    $.ajax({
                        type: 'post',
                        url: handlerUrl + 'PassWithdrawCash.ashx',
                        data: { ids: ids.join(','), type: "ScoreOnLine", module_name: '淘股币' },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                $('#grvData').datagrid('reload');
                            }
                            $.messager.alert('系统提示', resp.msg);
                        }
                    });
                }
            });
        }

        function RefuseItem() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var ids = new Array();
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id);
            }
           
            $.messager.confirm("系统提示", "确认所选申请审核不通过？", function (o) {
                if (o) {
                    $.ajax({
                        type: 'post',
                        url: handlerUrl + 'RefuseWithdrawCash.ashx',
                        data: { ids: ids.join(','), type: "ScoreOnLine", module_name: '淘股币' },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                $('#grvData').datagrid('reload');
                            }
                            $.messager.alert('系统提示', resp.msg);
                        }
                    });
                }
            });
        }

    </script>
</asp:Content>
