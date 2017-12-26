<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallCategoryMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallCategoryMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>商品分类管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加新分类</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑分类信息</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
                 <div>
                <span style="font-size: 12px; font-weight: normal">分类名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加新分类" style="width: 370px;
         padding: 15px;line-height:30px;">
        <table width="100%">
                 <tr>
                <td style="width:70px;">
                    上级分类:
                </td>
                <td>
                     <span id="sp_menu"></span>
                </td>
            </tr>
            <tr>
                <td>
                    分类名称:
                </td>
                <td>
                    <input id="txtCategoryName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    分类图片:
                </td>
                <td>
                      <img alt="图片" id="imgThumbnailsPath" style="max-width:200px;"/>
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                         onclick="txtThumbnailsPath.click()">上传图片</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片
                        <input type="file" id="txtThumbnailsPath" name="file1" style="display:none;" />
                </td>
            </tr>
          <tr>
                <td>
                   描述:
                </td>
                <td>
                  <textarea id="txtDescription" style="width: 250px;height:50px;"></textarea>
                   
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
	                queryParams: { Action: "QueryWXMallCategory" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoID', title: '分类ID', width: 20, align: 'left' },
                                { field: 'CategoryName', title: '分类名称', width: 20, align: 'left' },
                                { field: 'Description', title: '描述', width: 20, align: 'left' }
//                                ,
//                                { field: 'CategoryUrl', title: '分类链接', width: 55, align: 'left', formatter: function (value, rowData) {
//                                    var str = new StringBuilder();
//                                    str.AppendFormat('<a target="_blank" href="http://<%=Request.Url.Host %>/App/Cation/Wap/Mall/<%=WXMallIndex%>?cid={0}" >http://<%=Request.Url.Host %>/App/Cation/Wap/Mall/<%=WXMallIndex%>?cid={0}</a>', rowData.AutoID);
//                                    return str.ToString();
//                                }
//                                }

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
                             CategoryImg: $("#imgThumbnailsPath").attr("src")

                         }

                         if (dataModel.CategoryName == '') {

                             Alert('请输入分类名称');
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
             var CategoryName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryWXMallCategory", CategoryName: CategoryName} });
         });


         //加载分类
         LoadCategorySelectList();

         $("#txtThumbnailsPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });
                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {

                                 $('#imgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

     });

     function ShowAdd() {
         currAction = 'AddWXMallCategory';
         $('#dlgInput').dialog({ title: '添加分类' });
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
         $('#dlgInput').dialog({ title: '编辑分类' });
         $('#dlgInput').dialog('open');
     }

     //加载分类选择列表
     function LoadCategorySelectList() {
         $.post(handlerUrl, { Action: "GetWXMallCategorySelectList" }, function (data) {
             $("#sp_menu").html(data);
         });
     }
    </script>
</asp:Content>