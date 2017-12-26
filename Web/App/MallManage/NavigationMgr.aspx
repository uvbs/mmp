<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="NavigationMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.NavigationMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;首页<span>导航管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="NavigationAddEdit.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                id="btnAdd">添加</a> <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete"
                    plain="true" onclick="Delete()">批量删除</a>
        </div>
        类型:
        <select id="ddlType">
         <option value="">全部</option>
            <option value="top">顶部导航</option>
            <option value="left">左侧导航</option>
            <option value="bottom">底部导航</option>
        </select>
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
	                queryParams: { Action: "QueryNavigation" },
	                height: document.documentElement.clientHeight - 160,
	                pagination: false,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },

                                { field: 'NavigationLinkType', title: '类型', width: 10, align: 'center', formatter: function (value) {
                                    if (value == "top") {

                                        return "<font color='red'>顶部导航</font>";
                                    }
                                    if (value == "left") {
                                        return "<font color='green'>左侧导航</font>";
                                    }

                                    if (value == "bottom") {
                                        return "<font color='blue'>底部导航</font>";
                                    }
                                }
                                },
                                { field: 'NavigationName', title: '名称', width: 10, align: 'left' },
                                { field: 'NavigationImage', title: '图片', width: 5, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" width="200" height="100" />', value);
                                    return str.ToString();
                                }
                                },

                            { field: 'NavigationLink', title: '链接', width: 20, align: 'center', formatter: function (value) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="{0}" target="_blank">{0}</>', value);
                                return str.ToString();
                            }
                            },
                            { field: 'Sort', title: '排序', width: 10, align: 'center' },
                            { field: 'EditCloum', title: '操作', width: 10, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a title="点击修改" href="NavigationAddEdit.aspx?id={0}">编辑</a>', rowData.AutoID);
                                return str.ToString();
                            }
                            }




                             ]]
	            }
            );

        //类型
         $("#ddlType").change(function () {

             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryNavigation", NavigationType: $(ddlType).val()} });
            
            
            })


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
                        data: { Action: "DeleteNavigation", ids: GetRowsIds(rows).join(',') },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                Show(resp.Msg);
                                $('#grvData').datagrid('reload');
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
                ids.push(rows[i].AutoID
                 );
            }
            return ids;
        }



    </script>
</asp:Content>
