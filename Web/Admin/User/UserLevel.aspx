<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="UserLevel.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.UserLevel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&nbsp;会员管理 &nbsp;&nbsp;&gt;&nbsp;&nbsp; 会员等级
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加新等级</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 370px;
        padding: 35px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td>
                    等级数字:
                </td>
                <td>
                    <input id="txtLevelNumber" type="text" style="width: 150px;" onkeyup="value=value.replace(/[^\d]/g,'')"
                        maxlength="9" />
                </td>
            </tr>
            <tr>
                <td>
                    等级名称:
                </td>
                <td>
                    <input id="txtLevelString" type="text" style="width: 150px;" />
                </td>
            </tr>
            <tr>
                <td>
                    积分最小值:
                </td>
                <td>
                    <input id="txtFromHistoryScore" type="text" style="width: 150px;" onkeyup="value=value.replace(/[^\d]/g,'')"
                        maxlength="9" />
                </td>
            </tr>
            <tr>
                <td>
                    积分最大值:
                </td>
                <td>
                    <input id="txtToHistoryScore" type="text" style="width: 150px;" onkeyup="value=value.replace(/[^\d]/g,'')"
                        maxlength="9" />
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
        var levelType = "CommonScore";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryUserLevelConfig", type: levelType },
	                height: document.documentElement.clientHeight - 120,
	                pagination: true,
	                striped: true,
	                pageSize: 20, 
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'LevelNumber', title: '等级', width: 5, align: 'left' },
                                { field: 'LevelString', title: '等级名称', width: 50, align: 'left' },
                                { field: 'FromHistoryScore', title: '积分最小值', width: 20, align: 'left' },
                                { field: 'ToHistoryScore', title: '积分最大值', width: 20, align: 'left' }


                             ]]
	            }
            );



            $('#dlgInput').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: currAction,
                                AutoID: currSelectID,
                                LevelType: levelType,
                                LevelNumber: $.trim($('#txtLevelNumber').val()),
                                LevelString: $.trim($('#txtLevelString').val()),
                                FromHistoryScore: $.trim($('#txtFromHistoryScore').val()),
                                ToHistoryScore: $.trim($('#txtToHistoryScore').val())

                            }


                            if (dataModel.LevelNumber == '') {
                                $('#txtLevelNumber').focus();
                                return false;
                            }
                            if (dataModel.LevelString == '') {
                                $('#txtLevelString').focus();
                                return false;
                            }
                            if (dataModel.FromHistoryScore == '') {
                                dataModel.FromHistoryScore = 0;
                            }
                            if (dataModel.ToHistoryScore == '') {
                                dataModel.ToHistoryScore = 0;

                            }
                            
                            if (parseInt(dataModel.FromHistoryScore) >= parseInt(dataModel.ToHistoryScore))
                            {
                                Alert("积分最小值不能大于等于最大值");
                                return false;
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




        });

        function ShowAdd() {
            currAction = 'AddUserLevelConfig';
            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");


        }
        function Delete() {
            try {

                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;
                $.messager.confirm("系统提示", "确认删除选中等级?", function (result) {
                    if (result) {
                        var ids = [];
                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoId);
                        }
                        var dataModel = {
                            Action: 'DeleteUserLevelConfig',
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

            currAction = 'EditUserLevelConfig';
            currSelectID = rows[0].AutoId;
            $('#txtLevelNumber').val(rows[0].LevelNumber);
            $('#txtLevelString').val(rows[0].LevelString);
            $('#txtFromHistoryScore').val(rows[0].FromHistoryScore);
            $('#txtToHistoryScore').val(rows[0].ToHistoryScore);
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }


    </script>
</asp:Content>