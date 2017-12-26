<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="TagsMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.TagsMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&nbsp;<span>标签管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd()">添加</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit()">修改</a><a href="javascript:void(0)"
                        class="easyui-linkbutton" iconcls="icon-delete" onclick="Delete()" plain="true">删除</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 320px;
        padding: 10px">
        <table>
            <tr>
                <td height="25" align="left">
                    名称：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtName" style="width: 150px;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">
                    类别：
                </td>
                <td height="25" width="*" align="left">
                    <select id="ddltype">
                        <option value="Professional">理财师标签</option>
                    </select>
                </td>
            </tr>
            
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXWuBuHuiTutorHandler.ashx";
     var currentSelectId;
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetTradeInfos" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'CategoryName', title: '标签名称', width: 10, align: 'left' },
                                { field: 'CategoryType', title: '类别', width: 10, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (value == "Professional") {
                                        return "理财师标签";
                                    } else {
                                        return "";
                                    }
                                }
                                },
                             ]]
	            }
            );
       $('#dlgInput').dialog({
                            buttons: [{
                                text: '保存',
                                handler: function () {
                                    try {
                                        var dataModel = {
                                            Action: "UTradeInfo",
                                            cName: $("#txtName").val(),
                                            AutoId: currentSelectId,
                                            CategoryType: $.trim($('#ddltype').val())
                                        }
                                        if (dataModel.cName == '') {

                                            Alert('请输入标签名称');
                                            return;
                                        }

                                        $.ajax({
                                            type: 'post',
                                            url: handlerUrl,
                                            data: dataModel,
                                            dataType: "json",
                                            success: function (resp) {
                                                if (resp.Status ==0) {
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


     //删除
     function Delete() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确定删除选中?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DelTradeInfo", ids: GetRowsIds(rows).join(',') },
                     dataType:"json",
                     success: function (resp) {
                         if (resp.Status == 0) {
                             $('#grvData').datagrid('reload');
                             Show(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });
     }
     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].AutoID);
         }
         return ids;
     }


     function ShowAdd() {
         currentSelectId = 0;
         $("#txtName").val("");
         $('#dlgInput').window(
            {
                title: '添加标签'
            }
            );
         $('#dlgInput').dialog('open');
     }
    
     function ShowEdit() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         if (!EGCheckNoSelectMultiRow) {

             return;
         }
         $("#txtName").val(rows[0].CategoryName);
         currentSelectId = rows[0].AutoID;
         $('#dlgInput').window(
            {
                title: '编辑'
            }
            );
         $('#dlgInput').dialog('open');
     }

    </script>
</asp:Content>