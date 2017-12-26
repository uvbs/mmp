<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallDeliveryMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallDeliveryMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>配送方式管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加配送方式</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑配送方式</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除配送方式</a>
                 <div>
                <span style="font-size: 12px; font-weight: normal">配送方式名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallDelivery" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'AutoId', title: '编号', width: 10, align: 'left' },
                                { field: 'DeliveryType', title: '配送类型', width: 10, align: 'left', formatter: function (value, rowData) {

                                    if (value == 0) {
                                        return "快递";
                                    }
                                    else if (value == 1) {
                                        return "上门自取";
                                    }
                                    else if (value == 2) {
                                        return "卖家承担";
                                    }
                                    else {
                                        return "";
                                    }

                                }
                                },
                            { field: 'DeliveryName', title: '配送方式名称', width: 85, align: 'left' }
                             ]]
	            }
            );

         $("#btnSearch").click(function () {
             var DeliveryName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryWXMallDelivery", DeliveryName: DeliveryName} });
         });



     });

     function ShowAdd() {

         window.location.href = "WXMallDeliveryCompile.aspx?Action=add";

     }
     function Delete() {
         try {

             var rows = $('#grvData').datagrid('getSelections');

             if (!EGCheckIsSelect(rows))
                 return;

             $.messager.confirm("系统提示", "确认删除选中配送方式?", function (r) {
                 if (r) {
                     var ids = [];

                     for (var i = 0; i < rows.length; i++) {
                         ids.push(rows[i].AutoId);
                     }

                     var dataModel = {
                         Action: 'DeleteWXMallDelivery',
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
         window.location.href = "WXMallDeliveryCompile.aspx?Action=edit&id=" + rows[0].AutoId;

     }


    </script>
</asp:Content>