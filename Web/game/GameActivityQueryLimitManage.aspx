<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="GameActivityQueryLimitManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.game.GameActivityQueryLimitManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;数据管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>允许查询游戏</span>
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
                <span style="font-size: 12px; font-weight: normal">名称或活动ID：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加新分类" style="width: 370px;
        height:200px; padding: 15px;line-height:30px;">
        <table width="100%">
        <tr>
                <td>
                    活动ID:
                </td>
                <td>
                    <input id="txtActivityID" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    名称:
                </td>
                <td>
                    <input id="txtActivityName" type="text" style="width: 250px;" />
                </td>
            </tr>
          
        </table>
    </div>
 
   
    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currSelectID = 0;
        var currAction = '';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryGameActivityQueryLimit" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ActivityID', title: '活动ID', width: 10, align: 'left' },
                                { field: 'ActivityName', title: '活动', width: 85, align: 'left' }

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
                                GameActivityID: $.trim($('#txtActivityID').val()),
                                GameActivityName: $.trim($('#txtActivityName').val())

                            }

                            if (dataModel.GameActivityID == '') {

                                Alert('请输入活动ID');
                                return;
                            }

                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                success: function (result) {
                                    var resp = $.parseJSON(result);
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
                var Param = $("#txtName").val();
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryGameActivityQueryLimit", Param: Param} });
            });


        });

        function ShowAdd() {
            currAction = 'AddGameActivityQueryLimit';
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
                            Action: 'DeleteGameActivityQueryLimit',
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


            currAction = 'EditGameActivityQueryLimit';
            currSelectID = rows[0].AutoID;
            $('#txtActivityID').val(rows[0].ActivityID);
            $('#txtActivityName').val(rows[0].ActivityName);
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }


    </script>
</asp:Content>