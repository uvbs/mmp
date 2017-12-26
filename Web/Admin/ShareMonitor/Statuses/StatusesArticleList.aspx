<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="StatusesArticleList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Statuses.StatusesArticleList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;社区管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;动态列表
    <a href="StatusesList.aspx" style="float: right; margin-right: 20px; color: Black;" title="返回社区列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnDelItem" onclick="DelItem()">删除</a>
            关键字:<input id="txtKeyword" style="width: 200px;"  placeholder="" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/StatusesHandler.ashx";
        var domain = '<%=Request.Url.Host%>';
        var cateId;
        $(function () {
            cateId = GetParm("cateId");
            $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getStatusesArticleList", cateId: cateId },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'id', title: '编号', width: 50, align: 'left', formatter: FormatterTitle },
                        { field: 'type', title: '社区', width: 50, align: 'left', formatter: FormatterTitle },
                        { field: 'summary', title: '动态', width: 240, align: 'left', formatter: FormatterTitle },
                        { field: 'name', title: '用户', width: 50, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'cmtNum', title: '评论数', width: 30, align: 'left', formatter: function (value, rowData) {
                                if (value > 0) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="/Admin/Review/ReviewList.aspx?ActId={0}&Pfolder=Statuses&cateId={1}">' + value + '</a>', rowData['id'], cateId);
                                    return str.ToString();
                                }
                                else {
                                    return value;
                                }
                            }
                        }
	                ]]
	            }
            );
        });

     function Search() {
         $('#grvData').datagrid({
             method: "Post",
             url: handlerUrl,
             queryParams: { Action: "getStatusesArticleList",cateId:cateId, keyword: $("#txtKeyword").val() }
         });
     }
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
         $.messager.confirm("系统提示", "确认删除该动态？", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DelStatusesArticle", ids: GetRowsIds(rows).join(',') },
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
    </script>
</asp:Content>
