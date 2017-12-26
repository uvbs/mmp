<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallStoresMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallStoresMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>门店管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加新门店</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑门店信息</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
                 <div>
                <span style="font-size: 12px; font-weight: normal">门店名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 390px;
       padding: 15px;line-height:30px;">
        <table width="100%">
            <tr>
                <td>
                    门店名称:
                </td>
                <td>
                    <input id="txtStoreName" type="text" style="width: 250px;" />
                </td>
            </tr>
               <tr>
                <td>
                    门店地址:
                </td>
                <td>
                    <textarea id="txtStoreAddress"  style="width: 250px;height:80px;" ></textarea>
                </td>
            </tr>
            <tr>
                <td>
                    是否默认:
                </td>
                <td>
                   <input type="radio" name="rdostore" value="1" id="rdodefaultstore" /><label for="rdodefaultstore">默认门店</label>
                   <input type="radio" name="rdostore" value="0" id="rdonotdefaultstore" /><label for="rdonotdefaultstore">否</label>
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
	                queryParams: { Action: "QueryWXMallStores" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'StoreName', title: '门店名称', width: 30, align: 'left' },
                                { field: 'StoreAddress', title: '门店地址', width: 65, align: 'left' },
                                { field: 'IsDefaultStore', title: '是否默认门店', width: 65, align: 'left', formatter: function (value) {

                                    var str = new StringBuilder();
                                    if (value == "1") {
                                        str.AppendFormat('<font color="green">默认门店<font>');
                                    }
                                    else {
                                        str.AppendFormat('<font>否<font>');
                                    }

                                    return str.ToString();
                                } 
                                }

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
                             StoreName: $.trim($('#txtStoreName').val()),
                             StoreAddress: $.trim($('#txtStoreAddress').val()),
                             IsDefaultStore: $(":input[name=rdostore]:checked").val()

                         }

                         if (dataModel.StoreName == '') {

                             Alert('请输入门店名称');
                             return;
                         }
                         if (dataModel.StoreAddress == '') {

                             Alert('请输入门店地址');
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
             var StoreName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryWXMallStores", StoreName: StoreName} });
         });



     });

     function ShowAdd() {
         currAction = 'AddWXMallStore';
         $("#txtStoreName").val("");
         $("#txtStoreAddress").val("");
         $("#dlgInput textarea").val("");
         $("#rdodefaultstore").attr("checked", "checked");
         $('#dlgInput').dialog({ title: '添加新门店' });
         $('#dlgInput').dialog('open');


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
                         Action: 'DeleteWXMallStore',
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


         currAction = 'EditWXMallStore';
         currSelectID = rows[0].AutoID;
         $('#txtStoreName').val(rows[0].StoreName);
         $('#txtStoreAddress').val(rows[0].StoreAddress);

         if (rows[0].IsDefaultStore == "1") {
             $("#rdodefaultstore").attr("checked", "checked");
         }
         else {
             $("#rdonotdefaultstore").attr("checked", "checked");
         }
         $('#dlgInput').dialog({ title: '编辑门店信息' });
         $('#dlgInput').dialog('open');
     }


    </script>
</asp:Content>
