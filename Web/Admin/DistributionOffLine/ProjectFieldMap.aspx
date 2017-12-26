<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ProjectFieldMap.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.ProjectFieldMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=moduleName %>字段管理</span>
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
        height: 200px;">
        <table width="100%">
            <tr>
                <td>
                    选择字段:
                </td>
                <td>
                    <select id="ddlField">
                        <option value="ProjectName">名称</option>
                        <option value="Contact">联系人</option>
                        <option value="Phone">手机号</option>
                        <option value="WeiXin">微信</option>
                        <option value="Company">公司</option>
                        <option value="Remark">备注</option>
                        <option value="Ex1">扩展字段1</option>
                        <option value="Ex2">扩展字段2</option>
                        <option value="Ex3">扩展字段3</option>
                        <option value="Ex4">扩展字段4</option>
                        <option value="Ex5">扩展字段5</option>
                        <option value="Ex6">扩展字段6</option>
                        <option value="Ex7">扩展字段7</option>
                        <option value="Ex8">扩展字段8</option>
                        <option value="Ex9">扩展字段9</option>
                        <option value="Ex10">扩展字段10</option>
                        <option value="Ex11">扩展字段11</option>
                        <option value="Ex12">扩展字段12</option>
                        <option value="Ex13">扩展字段13</option>
                        <option value="Ex14">扩展字段14</option>
                        <option value="Ex15">扩展字段15</option>
                        <option value="Ex16">扩展字段16</option>
                        <option value="Ex17">扩展字段17</option>
                        <option value="Ex18">扩展字段18</option>
                        <option value="Ex19">扩展字段19</option>
                        <option value="Ex20">扩展字段20</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    显示名称:
                </td>
                <td>
                    <input id="txtFieldShowName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    可为空:
                </td>
                <td>
                    <select id="ddlIsNull">
                        <option value="1">可为空</option>
                        <option value="0">不可为空</option>
                    </select>
                </td>
            </tr>
              <tr>
                <td>
                    排序号(数字越大越靠前显示):
                </td>
                <td>
                    <input id="txtSort" type="text" style="width: 250px;" />
                </td>
            </tr>
               <tr>
                <td>
                    是否显示在列表:
                </td>
                <td>
                  
                        <input type="radio" id="lisShow" checked="checked" class="positionTop3" name="rdoColumn" value="1" />
                        <label for="lisShow">显示</label>
                    
                        <input type="radio" id="listHide" class="positionTop3" name="rdoColumn" value="0" />
                        <label for="listHide">隐藏</label>
                    
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/ProjectFieldMap/";
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
                                { field: 'Field', title: '字段', width: 20, align: 'left' },
                                { field: 'MappingName', title: '显示名称', width: 80, align: 'left' },
                                {
                                    field: 'IsShowInList', title: '是否在列表显示', width: 20, align: 'left', formatter: function (value) {
                                        if (value == 1) {
                                            return "显示";
                                        }
                                        else if (value == 0) {
                                            return "隐藏";
                                        } else {
                                            return "";
                                        }
                                    }
                                },
                                {
                                    field: 'FieldIsNull', title: '可为空', width: 20, align: 'left', formatter: function (value) {
                                        if (value == 1) {
                                            return "可为空";
                                        }
                                        else if (value == 0) {
                                            return "不可为空";
                                        }
                                    }
                                },
                                { field: 'Sort', title: '排序号', width: 10, align: 'left' }
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
                                Field: $.trim($('#ddlField').val()),
                                FieldShowName: $.trim($('#txtFieldShowName').val()),
                                IsNull: $(ddlIsNull).val(),
                                Sort: $(txtSort).val(),
                                IsShowInList:$('[name="rdoColumn"]:checked').val(),
                                ModuleType:moduleType
                            }
                            if (dataModel.FieldShowName == "") {
                                alert("显示名称必填");
                                return false;
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

                

                if (rows[0].Field == "ProjectName") {
                    $.messager.alert("系统提示", rows[0].MappingName+"不能删除");
                    return;
                }
                


                    


                $.messager.confirm("系统提示", "确认删除?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoId);
                        }

                        var dataModel = {
                            autoIds: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl + "Delete.ashx",
                            data: dataModel,
                            success: function (resp) {
                                if (resp.status == true) {
                                    Show("删除成功");
                                    $('#grvData').datagrid('reload');
                                } else {
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

            if (!EGCheckIsSelect(rows)) return;
            if (!EGCheckNoSelectMultiRow(rows)) return;

            currAction = 'Update';
            currSelectID = rows[0].AutoId;
            $(ddlField).val(rows[0].Field);
            $(txtFieldShowName).val(rows[0].MappingName);
            $(ddlIsNull).val(rows[0].FieldIsNull);
            $(txtSort).val(rows[0].Sort);

            if (rows[0].IsShowInList == 0) {
                $('[name=rdoColumn][value=0]').attr("checked", "checked");
            } else {
                $('[name=rdoColumn][value=1]').attr("checked", "checked");
            }
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }


    </script>
</asp:Content>
