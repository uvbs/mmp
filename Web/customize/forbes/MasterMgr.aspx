<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="MasterMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.MasterMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXWuBuHuiTutorHandler.ashx";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetTutorInfos" },
	                height: document.documentElement.clientHeight - 160,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TutorImg', title: '理财师照片', width: 10, align: 'left', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'TutorName', title: '理财师姓名', width: 20, align: 'left' },
                                { field: 'Company', title: '公司', width: 10, align: 'left' },
                                { field: 'Position', title: '职位', width: 10, align: 'left' }
                             ]]
	            }
            );
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
                        data: { Action: "DelTutorInfos", ids: GetRowsIds(rows).join(',') },
                        dataType: "json",
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
                ids.push(rows[i].AutoId);
            }
            return ids;
        }



        function Search() {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetTutorInfos", Name: $("#txtName").val() }
	            });
        }


        function Edit() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            if (!EGCheckNoSelectMultiRow) {
                return false;
            }
            window.location.href = "MasterAddEdit.aspx?Id=" + rows[0].AutoId;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>理财师管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="MasterAddEdit.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true">
                添加</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit"
                    plain="true" onclick="Edit()">修改</a><a href="javascript:void(0)" class="easyui-linkbutton"
                        iconcls="icon-delete" onclick="Delete()" plain="true">删除</a>
                        <br />
                        关键字:<input id="txtName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
