<%@ Page Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="FilterWordManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.FilterWordManage.FilterWordManage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;其他管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>敏感词管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加敏感词</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑敏感词</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
            <div>
                <span style="font-size: 12px; font-weight: normal">敏感词：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加敏感词" style="width: 370px; padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td>敏感词:
                </td>
                <td>
                    <input id="txtTagName" type="text" style="width: 250px;" />
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/FilterWordHandler.ashx",
            currSelectID = 0,
            currAction = '',
            currTagType = '<%=Request["type"]%>';

        $(function () {
            $('#grvData').datagrid(
                  {
                      method: "Post",
                      url: handlerUrl,
                      queryParams: { Action: "QueryFilterWord", FilterType: currTagType },
                      height: document.documentElement.clientHeight - 112,
                      pagination: true,
                      striped: true,
                      pageSize: 50,
                      rownumbers: true,
                      rowStyler: function () { return 'height:25px'; },
                      columns: [[
                                  { title: 'ck', width: 5, checkbox: true },
                                  { field: 'Word', title: '敏感词', width: 20, align: 'left' }


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
                                FilterWord: $.trim($('#txtTagName').val()),
                                FilterType: currTagType

                            }
                            if (dataModel.FilterWord == '') {
                                Alert('请输入敏感词');
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
                                    } else if (resp.Status == 3) {
                                        Show(resp.Msg);
                                    } else {
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
                var TagName = $("#txtName").val();
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryFilterWord", FilterType: currTagType, FilterWord: TagName } });
            });



        });

        function ShowAdd() {
            currAction = 'AddFilterWord';
            $('#dlgInput').dialog({ title: '添加敏感词' });
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
                            Action: 'DeleteFilterWord',
                            ids: ids.join(',')
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    Show("删除完成");
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

            if (!EGCheckNoSelectMultiRow(rows))
                return;
            currAction = 'UpdateFilterWord';
            currSelectID = rows[0].AutoID;
            $('#txtTagName').val(rows[0].Word);
            $('#dlgInput').dialog({ title: '修改敏感词' });
            $('#dlgInput').dialog('open');
        }

    </script>
</asp:Content>

