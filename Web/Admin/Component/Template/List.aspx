<%@ Page Title="" EnableSessionState="ReadOnly" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Component.Template.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    页面模板列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="id" width="50" formatter="FormatterTitle">模板Id</th>
                <th field="img" width="80" formatter="FormatterImage60_90">模板图片</th>
                <th field="name" width="200" formatter="FormatterTitle">模板名称</th>
                <th field="cate_name" width="100" formatter="FormatterTitle">分类</th>
                <th field="sort" width="40" formatter="FormatterTitle">排序</th>
                <th field="web" width="70" formatter="FormatterFromWebsite">来源站点</th>
                <th field="Action" width="50" formatter="FormatAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            分类:<select id="ddlType" style="width: 200px;"></select>
            名称:<input id="txtKeyword" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/component/template/';
        var curWebsite = '<%= new ZentCloud.BLLJIMP.BLL().WebsiteOwner %>';
        $(function () {
            //加载分类下拉选择
            GetCateSelect();

            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl + "list.ashx",
                height: document.documentElement.clientHeight - 100,
                toolbar: '#divToolbar',
                pagination: true,
                striped: true,
                rownumbers: true,
                loadFilter: pagerFilter,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                },
                onBeforeLoad: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loading');
                }
            });
        });
        function FormatterFromWebsite(value) {
            var str = new StringBuilder();
            var color = "style='color:red;'";
            if (curWebsite == value)  color = "style='color:green;'";
            str.AppendFormat("<span {0} title=' " + value + "'>" + value + "</span>", color);
            return  str.ToString();
        }
        function FormatAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="EditPage.aspx?id={0}"><img class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>&nbsp;', rowData['id']);
            str.AppendFormat('<a href="javascript:void(0);" onclick="DelItem(\'{0}\',\'{1}\')"><img class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a>', rowData['id'], rowData['name']);
            return str.ToString();
        }
        function AddTemplate() {
            document.location.href = "EditPage.aspx?id=0&backlist=1";
        }
        function DelItem(modelId, modelName) {
            $.messager.confirm('系统提示', '确定删除[' + modelName + ']？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "delete.ashx",
                        data: { id: modelId },
                        success: function (result) {
                            if (result.status) {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: '删除[' + modelName + ']完成'
                                });
                                $('#grvData').datagrid('reload');
                            }
                            else {
                                $.messager.alert('系统提示', result.msg);
                            }
                        }
                    });
                }
            });
        }
        function Search() {
            $('#grvData').datagrid('load', { keyword: $.trim($("#txtKeyword").val()), cate: $.trim($("#ddlType").val()) });
        }
        function GetCateSelect() {
            $.ajax({
                type: "Post",
                url: "/serv/api/article/category/selectlist.ashx",
                data: { type: 'CompTempType', websiteowner: 'Common' },
                success: function (result) {
                    var str = new StringBuilder();
                    str.AppendFormat('<option value="" selected="selected"></option>');
                    if (result.status) {
                        for (var i = 0; i < result.result.length; i++) {
                            str.AppendFormat('<option value="{0}" >{1}</option>', result.result[i].value, result.result[i].text);
                        }
                    }
                    $('#ddlType').html(str.ToString());
                }
            });
        }
    </script>
</asp:Content>
