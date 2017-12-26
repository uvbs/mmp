<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="IllegalReviewList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Illegal.IllegalReviewList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;举报管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;被举报回答回复列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnDelItem" onclick="DelItem()">删除</a>
            关键字:<input id="txtKeyword" style="width: 200px;"  placeholder="关键字" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/IllegalHandler.ashx";
        var domain = '<%=Request.Url.Host%>';
        $(function () {
            $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getIllegalReviewList" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'name', title: '用户', width: 40, align: 'left', formatter: FormatterTitle },
                        { field: 'content', title: '内容', width: 200, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'type', title: '内容', width: 60, align: 'left', formatter: function (value) {
                                var t = value == "Answer" ? "回答" : "回复";
                                return t;
                            }
                        },
                        { field: 'iname', title: '举报人', width: 40, align: 'left', formatter: FormatterTitle },
                        { field: 'datetm', title: '举报时间', width: 60, align: 'left', formatter: FormatterTitle }
	                ]]
	            }
            );
        });

        //获取选中行ID集合
    function GetRowsIds(rows) {
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push(rows[i].id);
        }
        return ids;
    }
    function DelItem() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确认删除该记录？", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DelReview", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
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
    function Search() {
        $('#grvData').datagrid({
            method: "Post",
            url: handlerUrl,
            queryParams: { Action: "getIllegalReviewList", keyword: $("#txtKeyword").val() }
        });
    }
    </script>
</asp:Content>
