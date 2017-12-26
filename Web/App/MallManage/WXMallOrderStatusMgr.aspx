<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallOrderStatusMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallOrderStatusMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>订单状态管理</span>
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
        height:200px; ">
        <table width="100%">
            <tr>
                <td>
                    订单状态:
                </td>
                <td>
                    <input id="txtOrderStatu" type="text" style="width: 250px;" />
                </td>
            </tr>
          <tr>
                <td>
                    通知消息:
                </td>
                <td>
                    <textarea id="txtOrderMessage" type="text" style="width: 250px;height:50px;" rows="2"></textarea>
                   
                   
                </td>
            </tr>
            <tr><td>提示: </td><td> $ORDERID$ 表示订单号</td></tr>
             <tr>
                <td>
                    排序(从大到小):
                </td>
                <td>
                    <input id="txtSort" type="text" style="width: 100px;" />
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
	                queryParams: { Action: "QueryWXMallOrderStatu" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Sort', title: '排序', width: 10, align: 'left' },
                                { field: 'OrderStatu', title: '状态', width: 50, align: 'left' },
                                { field: 'OrderMessage', title: '通知消息', width: 85, align: 'left' }

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
                            OrderStatu: $.trim($('#txtOrderStatu').val()),
                            OrderMessage: $.trim($('#txtOrderMessage').val()),
                            Sort: $.trim($('#txtSort').val())

                        }

                        if (dataModel.OrderStatu == '') {

                            Alert('请输入订单状态');
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


        //            $("#btnSearch").click(function () {
        //                var CategoryName = $("#txtName").val();
        //                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryWXMallOrderStatu", CategoryName: CategoryName} });
        //            });



    });

    function ShowAdd() {
        currAction = 'AddWXMallOrderStatu';
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
                        Action: 'DeleteWXMallOrderStatu',
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


        currAction = 'EditWXMallOrderStatu';
        currSelectID = rows[0].AutoID;
        $('#txtOrderStatu').val(rows[0].OrderStatu);
        $('#txtOrderMessage').val(rows[0].OrderMessage);
        $('#txtSort').val(rows[0].Sort);
        $('#dlgInput').dialog({ title: '编辑' });
        $('#dlgInput').dialog('open');
    }


    </script>
</asp:Content>
