<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="KeyVauleDataManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.KeyVauleData.KeyVauleDataManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DivTextarea {
            padding: 5px;
            min-height: 94px;
            line-height: 21px;
            border: 1px solid grey;
            overflow-y:auto;
            overflow-x:hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="javascript:;">设置</a>
    <a href="<%=redirect %>" style="float: right; margin-right: 20px; color: Black;" title="返回" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>

            <%
                if (!isHide)
                {
            %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
            <%
                }     
            %>
        </div>
    </div>
    <table id="grvData" fitcolumns="false">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 700px; height:440px; padding: 15px; line-height: 30px;">
        <table width="100%">

            <%
                if (isAutoKey != "1")
                {
            %>
            <tr>
                <td>唯一代码:
                </td>
                <td>
                    <input id="txtKey" type="text" style="width: 250px;" />
                </td>
            </tr>
            <%
                }     
            %>
            <tr>
                <td style="width: 80px;">名称:
                </td>
                <td>
                    <input id="txtName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>内容:
                </td>
                <td>
                    <textarea id="txtValue" style="width: 98%;height:300px;"></textarea>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/KeyVauleDataHandler.ashx",
            currSelectID = 0,
            currAction = '',
            currKey = '',
            currDataType = '<%=Request["type"]%>',
            preKey = '<%=Request["preKey"]%>',
            isAutoKey = '<%=isAutoKey%>',
            websiteowner = "<%=this.Request["websiteowner"]%>",
            editor;

        $(function () {
            KindEditor.ready(function (K) {
                editor = K.create('#txtValue', {
                    uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                    items: [
                        'source', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                        'removeformat', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist', 'video',
                        'image', 'link', 'unlink', 'lineheight', 'table', 'cleardoc'
                    ],
                    filterMode: false,
                    cssPath: '/Plugins/zcWeixinKindeditor/zcWeixinKindeditor.css'
                });
            });
            $('#grvData').datagrid(
                  {
                      method: "Post",
                      url: handlerUrl,
                      queryParams: { Action: "Query", type: currDataType, preKey: preKey, websiteowner: websiteowner },
                      height: document.documentElement.clientHeight - 112,
                      pagination: true,
                      striped: true,

                      pageSize: 50,
                      rownumbers: true,
                      rowStyler: function () { return 'height:25px'; },
                      columns: [[
                                  { title: 'ck', width: 5, checkbox: true },
            <%
                if (isAutoKey != "1")
                {
            %>
                                  { field: 'DataKey', title: '代码', width: 100, align: 'left' },
            <%
                }     
            %>
                                  { field: 'DataName', title: '名称', width: 300, align: 'left', formatter: FormatterTitle },
                                  { field: 'DataValue', title: '内容', width: 500, align: 'left', formatter: FormatterTitle }
                      ]]
                  }
              );



            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        var dataModel = {
                            Action: currAction,
                            AutoId: currSelectID,
                            key: $.trim($('#txtKey').val()),
                            dataName: $.trim($('#txtName').val()),
                            type: currDataType,
                            value: editor.html(),
                            preKey: preKey,
                            websiteowner: websiteowner
                        }
                        if (dataModel.AutoId != 0) {
                            dataModel.key = currKey;
                        }
            <%
                if (isAutoKey != "1")
                {
            %>
                        if (dataModel.key == '') {
                            Alert('请输入唯一代码');
                            return;
                        }
            <%
                }     
            %>
                        if (dataModel.dataName == '') {
                            Alert('请输入名称');
                            return;
                        }
                        if (dataModel.value == '') {
                            Alert('请输入内容');
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
                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgInput').dialog('close');
                    }
                }]
            });


            $("#btnSearch").click(function () {
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "Query", type: currDataType } });
            });
        });

        function ShowAdd() {
            currAction = 'PutKeyValue';
            currSelectID = 0;
            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");
            editor.html('');
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
                            ids.push(rows[i].AutoId);
                        }
                        var dataModel = {
                            Action: 'DeleteKeyValue',
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
            currAction = 'PutKeyValue';
            currSelectID = rows[0].AutoId;
            currKey = rows[0].DataKey;
            <%
                if (isAutoKey != "1")
                {
            %>
            $('#txtKey').val(rows[0].DataKey);
            <%
                }     
            %>

            $('#txtName').val(rows[0].DataName);
            editor.html(rows[0].DataValue);
            $('#dlgInput').dialog({ title: '修改' });
            $('#dlgInput').dialog('open');
        }
    </script>
</asp:Content>
