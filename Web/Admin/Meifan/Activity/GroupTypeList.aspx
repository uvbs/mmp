<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="GroupTypeList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Meifan.Activity.GroupTypeList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;组别管理
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
                <span style="font-size: 12px; font-weight: normal">名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="组别" style="width: 370px; padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td>组别:
                </td>
                <td>
                    <input id="txtTagName" type="text" style="width: 250px;" maxlength="20" />
                </td>
            </tr>
           
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx",
            currSelectID = 0,
            currAction = '',
            currTagType = '<%=Request["activity_type"]?? "All"%>';

            $(function () {
                $('#grvData').datagrid(
                      {
                          method: "Post",
                          url: handlerUrl,
                          queryParams: { Action: "QueryMemberTag", TagType: currTagType },
                          height: document.documentElement.clientHeight - 112,
                          pagination: true,
                          striped: true,
                          rownumbers: true,
                          rowStyler: function () { return 'height:25px'; },
                          columns: [[
                                      { title: 'ck', width: 5, checkbox: true },
                                      { field: 'TagName', title: '组别', width: 20, align: 'left' }
                                      


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
                                    TagName: $.trim($('#txtTagName').val()),
                                    TagType: currTagType,
                                    AccessLevel:0
                                }


                                if (dataModel.TagName == '') {

                                    Alert('请输入名称');
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
                    $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryMemberTag", TagName: TagName, TagType: currTagType } });
                });



            });

            function ShowAdd() {
                currAction = 'AddMemberTag';
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

                            var TagName = [];
                            for (var i = 0; i < rows.length; i++) {
                                ids.push(rows[i].AutoId);
                                TagName.push(rows[i].TagName);

                            }


                            var dataModel = {
                                Action: 'DeleteMemberTag',
                                ids: ids.join(','),
                                TagName: TagName.join(',')
                            }
                            console.log(dataModel.TagName);

                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status == 3) {
                                        Show(resp.Msg);
                                    }
                                    else if (resp.Status == 1) {
                                        Show(resp.Msg);
                                        $('#grvData').datagrid('reload');
                                    } else {
                                        $('#grvData').datagrid('reload');
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


                currAction = 'EditMemberTag';
                currSelectID = rows[0].AutoId;
                $('#txtTagName').val(rows[0].TagName);
                $("#txtAccessLevel").val(rows[0].AccessLevel);
                $('#dlgInput').dialog({ title: '编辑' });
                $('#dlgInput').dialog('open');
            }

    </script>
</asp:Content>