<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Component.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    页面列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="component_id" width="70" formatter="FormatterTitle">页面Id</th>
                <th field="component_name" width="80" formatter="FormatterSysTitle">页面名称</th>
                <th field="component_model_name" width="70" formatter="FormatterTitle">所属模板</th>
                <th field="component_link_url" width="70" formatter="FormatterLinkUrlBlank">二维码</th>
                <th field="access_level" width="60" formatter="FormatterTitle">访问等级</th>
                <th field="Action" width="50" formatter="FormatAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" title="添加页面" onclick="AddComponent()" id="btnAdd" runat="server">添加页面</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="SetAccessLevel()">设置访问等级</a>
            <br />
            <%--类型:new ZentCloud.Common.MyCategoriesV2().GetSelectOptionHtml(new ZentCloud.BLLJIMP.BLLKeyValueData().GetKeyVauleDataInfoList("ComponentType", "0", "Common"), "DataKey", "PreKey", "DataValue", "0", "ddlType", "width:200px", "page")--%>
            名称:<input id="txtKeyword" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 450px; height: 400px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
     <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px;
        padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    访问等级:
                </td>
                <td>
                    <input id="txtAccessLevel" type="text" style="width: 250px;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/component/';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                height: document.documentElement.clientHeight - 100,
                toolbar: '#divToolbar',
                pagination: true,
                striped: true,
                rownumbers: true,
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
            //批量设置访问级别对话框
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var ids = [];
                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].component_id);
                        }
                        if (ids.length == 0) {
                            $.messager.alert("系统提示", "请选择页面");
                            return;
                        }

                        var dataModel = {
                            access_level: $.trim($('#txtAccessLevel').val()),
                            ids: ids.join(',')
                        }
                        if (dataModel.access_level == "") {
                            $.messager.alert("系统提示", "请输入级别");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl + "SetAccessLevel.ashx",
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgInfo').dialog('close');
                                    loadData();
                                    $.messager.show({ title: '系统提示', msg: '设置完成' });
                                }
                                else {
                                    $.messager.alert("系统提示", resp.msg);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgInfo').dialog('close');
                    }
                }]
            });
            //初始加载
            loadData();
        });
        function FormatterSysTitle(value, rowData) {
            var str = new StringBuilder();
            var colorRed = "";
            if (rowData['component_key'] != null && rowData['component_key'] != "") colorRed = "style='color:red;'";
            str.AppendFormat("<span  {0} title=' " + value + "'>" + value + "</span>", colorRed);
            return  str.ToString();
        }
        function FormatAction(value, rowData) {
            var str = new StringBuilder();
            if (rowData.component_model_id == 9) {
                str.AppendFormat('<a href="Edit.aspx?component_id={0}"><img class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="原编辑" /></a>&nbsp;', rowData['component_id']);
            }
            else {
                str.AppendFormat('<a href="EditPage.aspx?component_id={0}&backlist=1"><img class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>&nbsp;', rowData['component_id']);
            }
            str.AppendFormat('<a href="javascript:void(0);" onclick="CopyItem(\'{0}\',\'{1}\')"><img class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/article_add.png" title="复制" /></a>&nbsp;', rowData['component_id'], rowData['component_name']);
            if (rowData['component_key'] ==null || rowData['component_key'] == "") {
                str.AppendFormat('<a href="javascript:void(0);" onclick="DelItem(\'{0}\',\'{1}\')"><img class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a>', rowData['component_id'], rowData['component_name']);
            }
            return str.ToString();
        }
        function FormatterLinkUrlBlank(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a style="color:blue;" href="javascript:ShowQRcode(\'http://{1}{0}\');">查看二维码</a>', value,'<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Authority%>');
            return str.ToString();
        }

        function ShowQRcode(linkurl) {
            $.ajax({
                type: 'post',
                url: "/Handler/QCode.ashx",
                data: { code: linkurl },
                success: function (result) {
                    $("#imgQrcode").attr("src", result);
                }
            });
            $('#dlgSHowQRCode').dialog('open');
            $("#alinkurl").html(linkurl);
            $("#alinkurl").attr("href", linkurl);
        }

        function AddComponent() {
            document.location.href = "EditPage.aspx?component_id=0&backlist=1";
        }
        function DelItem(modelId, modelName) {
            $.messager.confirm('系统提示', '确定删除[' + modelName + ']？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "delete.ashx",
                        data: { component_id: modelId },
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
        function CopyItem(modelId, modelName) {
            $.messager.confirm('系统提示', '确定复制[' + modelName + ']？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "copy.ashx",
                        data: { component_id: modelId },
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
        function Search() {
            var gridOpts = $('#grvData').datagrid('options', { pageNumber: 1 });
            loadData();
        }
        function loadData() {
            var gridOpts = $('#grvData').datagrid('options');
            $('#grvData').datagrid('loading');//打开等待div   
            $.post(
                handlerUrl + "list.ashx",
                { page: gridOpts.pageNumber, rows: gridOpts.pageSize, type: "", name: $.trim($("#txtKeyword").val()) },
                function (data, status) {
                    if (data.status && data.result.list) {
                        $('#grvData').datagrid('loadData', { "total": data.result.totalcount, "rows": data.result.list });
                    }
                });
        }

        //设置访问等级
        function SetAccessLevel() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $("#dlgInfo").dialog({ title: "设置访问级别" });
            $("#dlgInfo").dialog("open");
        }
    </script>
</asp:Content>
