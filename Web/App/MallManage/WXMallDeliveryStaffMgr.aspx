<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallDeliveryStaffMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallDeliveryStaffMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>配送员管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">新增配送员</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑配送员信息</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
                 <div>
                <span style="font-size: 12px; font-weight: normal">配送员姓名：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 370px;
         padding: 15px;line-height:15px;">
        <table width="100%">
            <tr>
                <td>
                    配送员姓名:
                </td>
                <td>
                    <input id="txtStaffName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    手机号:
                </td>
                <td>
                    <input id="txtStaffPhone" type="text" style="width: 250px;" />
                </td>
            </tr>
          
        </table>
    </div>
  
   
   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currSelectID = 0;
     var currAction = '';
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallDeliveryStaff" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'StaffName', title: '配送员姓名', width: 85, align: 'left' },
                                { field: 'StaffPhone', title: '手机号', width: 85, align: 'left' }

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
                             StaffName: $.trim($('#txtStaffName').val()),
                             StaffPhone: $.trim($('#txtStaffPhone').val())

                         }

                         if (dataModel.StaffName == '') {

                             Alert('请输入配送员姓名');
                             return;
                         }

                         $.ajax({
                             type: 'post',
                             url: handlerUrl,
                             data: dataModel,
                             dataType: "json",
                             success: function (resp) {
                                 if (resp.Status == 1) {
                                     Show(resp.Msg);
                                     $('#dlgInput').dialog('close');
                                     $('#grvData').datagrid('reload');
                                 }
                                 else {
                                     Alert(resp.Msg);
                                 }


                             }
                         });

                     } catch (e) {
                         Alert(e);
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
             var StaffName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryWXMallDeliveryStaff", StaffName: StaffName} });
         });



     });

     function ShowAdd() {
         currAction = 'AddWXMallDeliveryStaff';
         $('#dlgInput').dialog({ title: '新增配送员' });
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
                         Action: 'DeleteWXMallDeliveryStaff',
                         ids: ids.join(',')
                     }

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         success: function (result) {
                             Alert(result);
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


         currAction = 'EditWXMallDeliveryStaff';
         currSelectID = rows[0].AutoID;
         $('#txtStaffName').val(rows[0].StaffName);
         $('#txtStaffPhone').val(rows[0].StaffPhone);
         $('#dlgInput').dialog({ title: '编辑' });
         $('#dlgInput').dialog('open');
     }


    </script>
</asp:Content>
