<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="StatusesList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Statuses.StatusesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;社区管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;社区列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/StatusesHandler.ashx";
        var domain = '<%=Request.Url.Host%>';
        $(function () {
            $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getStatusesList",  preId: 0 },
	                height: document.documentElement.clientHeight - 170,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'AutoID', title: '编号', width: 50, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'ImgSrc', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                return str.ToString();
                            }
                        },
                        { field: 'CategoryName', title: '名称', width: 80, align: 'left', formatter: FormatterTitle },
                        { field: 'Summary', title: '描述', width: 120, align: 'left', formatter: FormatterTitle },
	                    {
	                        field: 'actNum', title: '动态数', width: 50, align: 'left', formatter: function (value, rowData) {
	                            if (value > 0) {
	                                var str = new StringBuilder();
	                                str.AppendFormat('<a href="StatusesArticleList.aspx?cateId={0}">' + value + '</a>', rowData['AutoID']);
	                                return str.ToString();
	                            }
	                            else {
	                                return value;
	                            }
	                        }
	                    },
                        { field: 'Sort', title: '排序', width: 50, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'edit', title: '编辑', width: 20, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="StatusesEdit.aspx?id={0}"><img alt="编辑该社区" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑该社区" /></a>', rowData['AutoID']);
                                return str.ToString();
                            }
                        }
	                ]]
	            }
            );
        });

     function ActionEvent(action,msg) {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", msg, function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: action, ids: GetRowsIds(rows).join(',') },
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
     function EditItem() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $('#txtID').val(rows[0].AutoID);
         $('#txtName').val(rows[0].CategoryName);
         $('#txtSort').val(rows[0].Sort);
         txtID.readOnly = true;
     }

     function AddItem() {
         $('#txtID').val(0);
         $('#txtName').val("");
         $('#txtSort').val(0);
         txtID.readOnly = true;
         $('#dlgInfo').dialog({ title: '新增' });
         $('#dlgInfo').dialog('open');
     }
    </script>
</asp:Content>
