<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Booking.Doctor.Category.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;专家预约&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>科室管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
                 <div>
                <span style="font-size: 12px; font-weight: normal">名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加新" style="width: 370px;
         padding: 15px;line-height:30px;">
        <table width="100%">
                 <tr>
                <td style="width:70px;">
                    上级:
                </td>
                <td>
                     <span id="sp_menu"></span>
                </td>
            </tr>
            <tr>
                <td>
                    名称:
                </td>
                <td>
                    <input id="txtCategoryName" type="text" style="width: 250px;" maxlength="20"/>
                </td>
            </tr>

          <tr>
                <td>
                   描述:
                </td>
                <td>
                  <textarea id="txtDescription" style="width: 250px;height:50px;" maxlength="50"></textarea>
                   
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
     var type = "BookingDoctor";
     if ("<%=Request["type"]%>"!="") {
         type = "<%=Request["type"]%>";
     }
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallCategory", type: type},
	                height: document.documentElement.clientHeight - 112,
	                pagination: false,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'CategoryName', title: '名称', width: 20, align: 'left' },
                                { field: 'Description', title: '描述', width: 20, align: 'left' }


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
                             CategoryName: $.trim($('#txtCategoryName').val()),
                             Description: $.trim($('#txtDescription').val()),
                             PreID: $('#ddlPreMenu').val(),
                             Type: type

                         }

                         if (dataModel.CategoryName == '') {

                             Alert('请输入名称');
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
                                     LoadCategorySelectList();
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
             var categoryName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryWXMallCategory", CategoryName: categoryName, Type: type } });
         });


         //加载
         LoadCategorySelectList();

       

     });

     function ShowAdd() {
         currAction = 'AddWXMallCategory';
         $('#dlgInput').dialog({ title: '添加' });
         $('#dlgInput').dialog('open');
         $("#dlgInput input").val("");
         $("#txtDescription").val("");


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
                         Action: 'DeleteWXMallCategory',
                         ids: ids.join(',')
                     }

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         success: function (result) {
                             Alert(result);
                             $('#grvData').datagrid('reload');
                             LoadCategorySelectList();
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


         currAction = 'EditWXMallCategory';
         currSelectID = rows[0].AutoID;
         $("#txtCategoryName").val($.trim(rows[0].CategoryName).replace('└', ''));
         $('#txtDescription').val(rows[0].Description);
         $("#imgThumbnailsPath").attr("src", rows[0].CategoryImg);
         $('#ddlPreMenu').val(rows[0].PreID);
         $('#dlgInput').dialog({ title: '编辑' });
         $('#dlgInput').dialog('open');
     }

     //加载选择列表
     function LoadCategorySelectList() {
         $.post(handlerUrl, { Action: "GetWXMallCategorySelectList",Type:type }, function (data) {
             $("#sp_menu").html(data);
         });
     }
    </script>
</asp:Content>
