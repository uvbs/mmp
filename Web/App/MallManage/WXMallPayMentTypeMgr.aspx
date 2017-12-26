<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallPayMentTypeMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallPayMentTypeMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>支付方式管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加支付方式</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑支付方式</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除支付方式</a>
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="UpdateIsDisable(0)">启用</a>
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="UpdateIsDisable(1)">停用</a>
                 <div>
                <span style="font-size: 12px; font-weight: normal">支付方式名称：</span>
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
	                queryParams: { Action: "QueryWXMallPaymentType" },
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
                                { field: 'PaymentType', title: '支付类型', width: 10, align: 'left', formatter: function (value, rowData) {

                                    if (value == 0) {
                                        return "线下支付";
                                    }
                                    else if (value == 1) {
                                        return "支付宝";
                                    }
                                    else if (value == 2) {
                                        return "微信支付";
                                    }
                                    else {
                                        return "";
                                    }

                                }
                                },

                            { field: 'PaymentTypeName', title: '支付方式名称', width: 85, align: 'left' },
                            { field: 'IsDisable', title: '状态', width: 10, align: 'left', formatter: function (value, rowData) {

                                if (value == 0) {
                                    return "<font color='green'>启用</font>";
                                }
                                else if (value == 1) {
                                    return "<font color='red'>停用</font>";

                                }
                                else {
                                    return "";
                                }

                            }
                            }
                             ]]
	            }
            );

         $("#btnSearch").click(function () {
             var PaymentTypeName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryWXMallPaymentType", PaymentTypeName: PaymentTypeName} });
         });



     });

     function ShowAdd() {

         window.location.href = "WXMallPayMentTypeCompile.aspx?Action=add";

     }
     function Delete() {
         try {

             var rows = $('#grvData').datagrid('getSelections');

             if (!EGCheckIsSelect(rows))
                 return;

             $.messager.confirm("系统提示", "确认删除选中支付方式?", function (r) {
                 if (r) {
                     var ids = [];

                     for (var i = 0; i < rows.length; i++) {
                         ids.push(rows[i].AutoId);
                     }

                     var dataModel = {
                         Action: 'DeleteWXMallPaymentType',
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

     //启用 禁用
     function UpdateIsDisable(isdisable) {
         try {

             var rows = $('#grvData').datagrid('getSelections');

             if (!EGCheckIsSelect(rows))
                 return;
             var msg = "";
             if (isdisable == 1) {
                 msg = "停用?";
             }
             else {
                 msg = "启用?";
             }
             $.messager.confirm("系统提示", "确认" + msg, function (r) {
                 if (r) {
                     var ids = [];

                     for (var i = 0; i < rows.length; i++) {
                         ids.push(rows[i].AutoId);
                     }

                     var dataModel = {
                         Action: 'UpdateWXMallPaymentTypeStatu',
                         ids: ids.join(','),
                         IsDisable: isdisable
                     }

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         dataType: "json",
                         success: function (resp) {
                             if (resp.Status == 1) {
                                 Show("操作成功");
                                 $('#grvData').datagrid('reload');
                             }
                             else {
                                 Alert("操作失败");
                             }

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
         window.location.href = "WXMallPayMentTypeCompile.aspx?Action=edit&id=" + rows[0].AutoId;

     }


    </script>
</asp:Content>
