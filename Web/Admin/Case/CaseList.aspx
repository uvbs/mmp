<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CaseList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Case.CaseList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;案例管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;案例列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnDelItem" onclick="DelItem()">删除</a>
            标题:<input id="txtKeyword" style="width: 200px;"  placeholder="标题" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/CaseHandler.ashx";
        var domain = '<%=Request.Url.Host%>';
        $(function () {
            $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getCaseList" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'id', title: '编号', width: 30, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'img', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                return str.ToString();
                            }
                        },
                        { field: 'title', title: '标题', width: 120, align: 'left', formatter: FormatterTitle },
                        { field: 'province', title: '地区', width: 50, align: 'left', formatter: FormatterTitle },
	                    { field: 'favNum', title: '收藏数', width: 30, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'cmtNum', title: '评论数', width: 30, align: 'left', formatter: function (value, rowData) {
                                if (value > 0) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a style="color:blue;" href="/Admin/Review/ReviewList.aspx?ActId={0}&Pfolder=Case">' + value + '</a>', rowData['id']);
                                    return str.ToString();
                                }
                                else {
                                    return value;
                                }
                            }
                        },
                        { field: 'pv', title: '点击数', width: 30, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'hide', title: '隐藏', width: 30, align: 'left', formatter: function (value, rowData) {
                                var str = value == 1 ? "是" : "否";
                                return str;
                            }
                        },
                        {
                            field: 'edit', title: '编辑', width: 20, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="CaseEdit.aspx?id={0}"><img alt="编辑该案例" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑该案例" /></a>', rowData['id']);
                                return str.ToString();
                            }
                        }
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
         $.messager.confirm("系统提示", "确认删除该案例？", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DelCase", ids: GetRowsIds(rows).join(',') },
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
             queryParams: { Action: "getCaseList", keyword: $("#txtKeyword").val() }
         });
     }
    </script>
</asp:Content>
