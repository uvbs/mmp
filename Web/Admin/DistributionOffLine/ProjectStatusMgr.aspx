<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ProjectStatusMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.ProjectStatusMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=moduleNmae %>状态管理</span>
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
                    状态名称:
                </td>
                <td>
                    <input id="txtStatus" type="text" style="width: 250px;" />
                </td>
            </tr>
                 <tr>
                <td>
                    执行动作:
                </td>
                <td>
                   <select id="ddlStatusAction">
                   <option value="">无</option>
                   <option value="DistributionOffLineCommission">分佣</option>
                   </select>
                </td>
            </tr>
             <tr>
                <td>
                    排序号(数字越大越靠前显示):
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

    var handlerUrl = "Handler/ProjectStatus/";
    var currSelectID = 0;
    var currAction = '';
    var moduleType = '<%=moduleType%>';

    $(function () {
        $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl + "List.ashx",
	                queryParams: { module_type: moduleType },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'OrderStatu', title: '状态', width: 50, align: 'left' },
                                { field: 'Sort', title: '排序', width: 10, align: 'left' },
                                { field: 'StatusAction', title: '执行动作', width: 10, align: 'left', formatter: function (value) {
                                    if (value == "DistributionOffLineCommission") {
                                        return "分佣";
                                    }
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
                            Status: $.trim($('#txtStatus').val()),
                            Sort: $.trim($('#txtSort').val()),
                            StatusAction: $.trim($('#ddlStatusAction').val()),
                            ModuleType:moduleType
                        }

                        if (dataModel.Status == '') {

                            Alert('请输入状态');
                            return;
                        }
                        var operaHandler = handlerUrl;
                        if (currAction == "Add") {
                            operaHandler += "Add.ashx";
                        }
                        else {
                            operaHandler += "Update.ashx";
                        }
                        $.ajax({
                            type: 'post',
                            url: operaHandler,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status == true) {
                                    Show("操作成功");
                                    $('#dlgInput').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert("操作失败");
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

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            $.messager.confirm("系统提示", "确认删除?", function (r) {
                if (r) {
                    var ids = [];

                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].AutoID);
                    }

                    var dataModel = {
                        autoId: ids.join(',')
                    }

                    $.ajax({
                        type: 'post',
                        url: handlerUrl+"Delete.ashx",
                        data: dataModel,
                        success: function (resp) {
                            if (resp.status==true) {
                                 Show("删除成功");
                                 $('#grvData').datagrid('reload');
                            }else {
                                 Show("删除失败");
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


        currAction = 'Update';
        currSelectID = rows[0].AutoID;
        $('#txtStatus').val(rows[0].OrderStatu);
        $('#txtSort').val(rows[0].Sort);
        $('#ddlStatusAction').val(rows[0].StatusAction);
        $('#dlgInput').dialog({ title: '编辑' });
        $('#dlgInput').dialog('open');
    }


</script>
</asp:Content>

