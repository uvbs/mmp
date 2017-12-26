<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="TypeList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Open.TypeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;公开课管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;分类列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnAddItem" onclick="AddItem()">新增</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnEditItem" onclick="EditItem()">修改</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px;padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    编号:
                </td>
                <td>
                    <input id="txtID" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    名称:
                </td>
                <td>
                    <input id="txtName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    排序:
                </td>
                <td>
                    <input id="txtSort" type="text" style="width: 250px;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')"/>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/OpenHandler.ashx";
        var domain = '<%=Request.Url.Host%>';
        var cateId;
        $(function () {
            type = GetParm("type");
            cateId = GetParm("cateId");
            $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getTypeList", type: type, preId: cateId },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'AutoID', title: '编号', width: 50, align: 'left', formatter: FormatterTitle },
                        { field: 'CategoryName', title: '名称', width: 100, align: 'left', formatter: FormatterTitle },
                        { field: 'Sort', title: '排序', width: 50, align: 'left', formatter: FormatterTitle }
	                ]]
	            }
            );
             $('#dlgInfo').dialog({
                 buttons: [{
                     text: '保存',
                     handler: function () {
                         var dataModel = {
                             Action: "addCate",
                             autoId: $.trim($('#txtID').val()),
                             preId: cateId,
                             name: $('#txtName').val(),
                             sort: $('#txtSort').val()
                         }

                         if (dataModel.name == '') {
                             Alert("名称不能为空!");
                             return;
                         }
                         if (dataModel.sort == '') {
                             Alert("排序不能为空!");
                             return;
                         }

                         $.ajax({
                             type: 'post',
                             url: handlerUrl,
                             data: dataModel,
                             dataType: "json",
                             success: function (resp) {
                                 if (resp.Status == 1) {
                                     $('#dlgInfo').dialog('close');
                                     $('#grvData').datagrid('reload');
                                 }
                                 else {
                                     Alert(resp.Msg);
                                 }
                             }
                         });

                     }
                 }, {
                     text: '取消',
                     handler: function () {
                         $('#dlgInfo').dialog('close');
                     }
                 }]
             });
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
         $('#dlgInfo').dialog({ title: '修改' });
         $('#dlgInfo').dialog('open');
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
