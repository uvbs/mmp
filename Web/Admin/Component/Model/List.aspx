<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Component.Model.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    组件库列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="component_model_id" width="100" formatter="FormatterTitle">组件库Id</th>
                <th field="component_model_name" width="100" formatter="FormatterTitle">组件库名称</th>
                <th field="is_delete" width="20" formatter="FormatterStatus">状态</th>
                <th field="Action" width="20" formatter="FormatAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" title="添加组件库" onclick="AddModel()" id="btnAdd" runat="server">添加组件库</a>
            <br />
            组件库分类:<select id="dllModelType" style="width:200px;"></select>
            名称:<input id="txtKeyword" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/component/model/';
        var keyvalue_list = <% =keyvalue_list%>;
        $(function () {
            //加载分类
            loadTypeList();

            $('#grvData').datagrid({
                method: "Post",
                height: document.documentElement.clientHeight - 100,
                toolbar: '#divToolbar',
                pagination: true,
                striped: true,
                
                rownumbers: true,
                singleSelect: true,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                }
            });
            $('#grvData').datagrid('getPager').pagination({
                onSelectPage: function (pPageIndex, pPageSize) {
                    //改变opts.pageNumber和opts.pageSize的参数值，用于下次查询传给数据层查询指定页码的数据   
                    loadData();
                }
            });
            //初始加载
            loadData();
        });
        function FormatAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="Edit.aspx?component_model_id={0}"><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>&nbsp;', rowData['component_model_id']);
            str.AppendFormat('<a href="javascript:void(0);" onclick="CopyItem(\'{0}\',\'{1}\')"><img class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/article_add.png" title="复制" /></a>&nbsp;', rowData['component_model_id'], rowData['component_model_name']);
            str.AppendFormat('<a href="javascript:void(0);" onclick="DelItem(\'{0}\',\'{1}\')"><img alt="删除" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a>', rowData['component_model_id'], rowData['component_model_name']);
            return str.ToString();
        }
        function FormatterStatus(value){
            if(value==1){
                return '<span style="color:red;">作废</span>';
            }
            return "正常";
        }
        function AddModel() {
            document.location.href = "Edit.aspx?component_model_id=0";
        }
        
        function CopyItem(modelId, modelName) {
            $.messager.confirm('系统提示', '确定复制[' + modelName + ']？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "copy.ashx",
                        data: { component_model_id: modelId },
                        success: function (result) {
                            if (result.status) {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: '复制[' + modelName + ']完成'
                                });
                                loadData();
                            }
                            else {
                                $.messager.alert('系统提示', result.msg);
                            }
                        }
                    });
                }
            });
        }
        function DelItem(modelId, modelName) {
            $.messager.confirm('系统提示', '确定删除组件库[' + modelName + ']？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "delete.ashx",
                        data: { component_model_id: modelId },
                        success: function (result) {
                            if (result.status) {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: '删除[' + modelName + ']完成'
                                });
                                loadData();
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
            var gridOpts = $('#grvData').datagrid('options', { pageNumber: 1 });
            loadData();
        }
        function loadData() {
            var gridOpts = $('#grvData').datagrid('options');
            var keyword = $('#txtKeyword').val();
            $('#grvData').datagrid('loading');//打开等待div   
            $.post(
                handlerUrl + "list.ashx",
                { page: gridOpts.pageNumber, rows: gridOpts.pageSize,component_model_type:$.trim($("#dllModelType").val()), keyword: keyword,show_delete:1 },
                function (data, status) {
                    if (data.status && data.result.list) {
                        $('#grvData').datagrid('loadData', { "total": data.result.totalcount, "rows": data.result.list });
                    }
                });
        }
        function loadTypeList(){
            var appendhtml = new StringBuilder();
            for (var i = 0; i < keyvalue_list.length; i++) {
                appendhtml.AppendFormat('<option value="{0}" {2}>{1}</option>', keyvalue_list[i].value, keyvalue_list[i].name,i==0?'selected="selected"':'');
            }
            $("#dllModelType").html("");
            $("#dllModelType").append(appendhtml.ToString());
        }
    </script>
</asp:Content>
