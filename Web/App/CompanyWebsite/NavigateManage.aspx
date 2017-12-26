<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="NavigateManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CompanyWebsite.NavigateManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微网站&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>模块管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="/App/CompanyWebSite/NavigateCompile.aspx?Action=add" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                id="btnAdd">添加</a> <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除</a>
            <br />
            
           

            <label style="margin-left:8px;">名称</label>
           <input type="text" id="txtName" style="width:200px;" />

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
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
	                queryParams: { Action: "QueryCompanyWebsiteNavigate" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'NavigateImage', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                            { field: 'NavigateName', title: '名称', width: 160, align: 'center' },
                             { field: 'PlayIndex', title: '播放顺序', width: 160, align: 'center' },
                            { field: 'NavigateType', title: '类型', width: 100, align: 'center', formatter: FormatterTitle },

                            { field: 'IsShow', title: '是否显示', width: 100, align: 'center', formatter: function (value, rowData) {
                                if (value == "1") {
                                    return "<font color='green'>显示</font>";
                                }
                                else {
                                    return "<font color='red'>不显示</font>";
                                }

                            }
                            },
                            { field: 'EditCloum', title: '操作', width: 50, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a title="点击修改" href="/App/CompanyWebSite/NavigateCompile.aspx?Action=edit&aid={0}">编辑</a>', rowData.AutoID);
                                return str.ToString();
                            }
                            }




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
                    data: { Action: "DeleteCompanyWebsiteNavigate", ids: GetRowsIds(rows).join(',') },
                    success: function (result) {
                        Alert(result);
                        $('#grvData').datagrid('reload');
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


    function Search() {

        $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryCompanyWebsiteNavigate", NavigateName: $("#txtName").val() }
	            });
    } 
    </script>
</asp:Content>