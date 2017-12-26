<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXTempmsgMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.WXTempmsg.WXTempmsgMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微信模板&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>微信模板管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>

        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 370px;
        padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td>
                    模板类型:
                </td>
                <td>
                    <select id="ddlTemplateType">
                        <option value="notify">服务通知</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    名称:
                </td>
                <td>
                    <input id="txtTemplateName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    微信模板ID:
                </td>
                <td>
                    <input id="txtTemplateId" type="text" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

     var handlerUrl = "Handler/WXTempmsgHandler.ashx";
     var currSelectID = 0;
     var currAction = '';
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "List" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                               
                                { field: 'TemplateType', title: '类型', width: 20, align: 'left' },
                                { field: 'TemplateName', title: '名称', width: 20, align: 'left' },
                                { field: 'TemplateID', title: '微信模板ID', width: 20, align: 'left' }

                             ]]
	            }
            );



         $('#dlgInput').dialog({
             buttons: [{
                 text: '保存',
                 handler: function () {
                     try {
                        
                         var dataModel = {
                             Action: currAction,
                             AutoID: currSelectID,
                             TemplateName: $(txtTemplateName).val(),
                             TemplateId: $(txtTemplateId).val(),
                             TemplatyType: $(ddlTemplateType).val()
                         }
                         
                         if (dataModel.TemplateName == '') {

                             Alert('请输入模板名称');
                             return;
                         }
                         if (dataModel.TemplateId == '') {

                             Alert('请输入模板ID');
                             return;
                         }

                         $.ajax({
                             type: 'post',
                             url: handlerUrl,
                             data: dataModel,
                             dataType: "json",
                             success: function (resp) {
                                 if (resp.isSuccess) {
                                     Show("操作成功");
                                     $('#dlgInput').dialog('close');
                                     $('#grvData').datagrid('reload');

                                 }
                                 else {
                                     Alert(resp.errmsg);
                                 }


                             }
                         });

                     } catch (e) {
                         alert(e);
                     }
                 }
             }, {
                 text: '取消',
                 handler: function () {

                     $('#dlgInput').dialog('close');
                 }
             }]
         });


         $("#btnSearch").click(function () {
             var templateName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "List", TemplateName: templateName} });
         });

     });

     function ShowAdd() {
         currAction = 'Add';
         $('#dlgInput').dialog({ title: '添加' });
         $('#dlgInput').dialog('open');
         $("#dlgInput input").val("");

     }

     function Delete() {
         try {

             var rows = $('#grvData').datagrid('getSelections');

             if (!EGCheckIsSelect(rows))
                 return;

             $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                 if (r) {
                     var ids = [];

                     for (var i = 0; i < rows.length; i++) {
                         ids.push(rows[i].AutoID);
                     }

                     var dataModel = {
                         Action: 'Delete',
                         ids: ids.join(',')
                     }

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         success: function (resp) {
                             if (resp.isSuccess) {
                                 Alert("删除成功");
                             }
                             $('#grvData').datagrid('reload');

                         }
                     });
                 }
             });

         } catch (e) {
             Alert(e);
         }
     }

     function ShowEdit() {
         var rows = $('#grvData').datagrid('getSelections');

         if (!EGCheckIsSelect(rows))
             return;

         if (!EGCheckNoSelectMultiRow(rows))
             return;


         currAction = 'Update';
         currSelectID = rows[0].AutoID;
         $(txtTemplateName).val($.trim(rows[0].TemplateName));
         $(txtTemplateId).val(rows[0].TemplateID);
         $(ddlTemplateType).val(rows[0].TemplateType);
         $('#dlgInput').dialog({ title: '编辑' });
         $('#dlgInput').dialog('open');
     }


     
    </script>
</asp:Content>
