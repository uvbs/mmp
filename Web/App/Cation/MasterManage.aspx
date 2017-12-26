<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="MasterManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.MasterManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript">
        $(function () {
            var myMenu;
            myMenu = new SDMenu("my_menu");
            myMenu.init();
            var firstSubmenu = myMenu.submenus[2];
            myMenu.expandMenu(firstSubmenu);
        });

        App/Cation

    </script>--%>
    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currDlgAction = '';
        var currSelectAcvityID = 0;
        var domain = 'hf.jubit.cn';

        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuMasterForWeb" },
	                height: document.documentElement.clientHeight - 145,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'HeadImg', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" /></a>', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'MasterName', title: '讲师姓名', width: 160, align: 'left' },
                                { field: 'Gender', title: '性别', width: 50, align: 'left' },
                                { field: 'Title', title: '讲师头衔', width: 100, align: 'left' },
                                { field: 'AddUserID', title: '发布人', width: 40, align: 'left' },
                                { field: 'IsSignUpJubit', title: '操作', width: 50, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" onclick="ShowEdit(\'{0}\');"><img alt="编辑讲师资料" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑讲师资料" /></a>', rowData['MasterID']);
                                    return str.ToString();
                                }
                                }
                             ]]
	            }
            );


        });

        function ShowEdit(mid) {
            window.location.href = 'MasterCompile.aspx?Action=edit&mid=' + mid;
        }

        function ShowAdd() {
            window.location.href = 'MasterCompile.aspx?Action=add';
        }


        //删除
        function Delete() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteJuMasterInfo", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            //
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
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
                ids.push(rows[i].MasterID
                 );
            }
            return ids;
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>专家团管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加新专家</a> <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除专家</a>
            <%--
            <div style="display: none;">
                <span style="font-size: 12px">活动名称:</span>
                <input type="text" id="txtActivityName" style="width: 200px;" />
            </div>
            <a href="#" class="button button-rounded button-flat-primary" iconcls="icon-search" id="btnSearch">查询</a>--%>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>

