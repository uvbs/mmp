<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="IndustryTemplateManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Sys.IndustryTemplateManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currDlgAction = '';
        var currSelectAcvityID = 0;
        var domain = '<%=Request.Url.Host %>';

        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryIndustryTemplate" },
	                height: document.documentElement.clientHeight - 145,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'IndustryTemplateName', title: '模板名称', width: 160, align: 'center'},
                                {field: 'CreateDate', title: '时间', width: 40, align: 'center', formatter: FormatDate  },
                                { field: 'IsSignUpJubit', title: '操作', width: 20, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" onclick="ShowEdit(\'{0}\');"><img alt="编辑该模板" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑该模板" /></a>', rowData['AutoID']);
                                    return str.ToString();
                                }
                                }
                             ]]
	            }
            );


        });

        function ShowEdit(aid) {
            window.location.href = 'IndustryTemplateCompile.aspx?Action=edit&aid=' + aid;
        }

        function ShowAdd() {
            window.location.href = 'IndustryTemplateCompile.aspx?Action=add'
           
        }



        //删除
        function Delete() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中模板?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteIndustryTemplate", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            Alert(result);
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>模板管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="IndustryTemplateCompile.aspx?Action=add" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                 id="btnAdd">增加新模板</a> 
                 <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除模板</a>
          
           
           
          
           
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

</asp:Content>
