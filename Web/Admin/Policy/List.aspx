<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Policy.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    政策管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="30" checkbox="true"></th>
                <th field="k5" width="100" formatter="FormatterTitle">政策文号</th>
                <th field="title" width="100" formatter="FormatterTitle">政策名称</th>
                <th field="k2" width="100" formatter="FormatterTitle">对象</th>
                <th field="action" width="30" formatter="FormatAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" title="添加政策" onclick="AddItem()" id="btnAdd" runat="server">添加政策</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-cancel" plain="true" title="批量删除" onclick="DelItem()" id="A3" runat="server">批量删除</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="二维码" onclick="ShowQRCode()" id="A4" runat="server">二维码</a>
            <br />
            对象:<select id="selectPolicyObject">
                <option value=""></option>
                <option value="单位">单位</option>
                <option value="个人">个人</option>
            </select>
            名称:<input id="txtKeyword" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 360px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/policy/';
        var handlerOldUrl = "/Handler/App/CationHandler.ashx";
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl + "list.ashx",
                height: document.documentElement.clientHeight - 70,
                toolbar: '#divToolbar',
                pagination: true,
                striped: true,
                loadFilter: pagerFilter,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                },
                onBeforeLoad: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loading');
                },
            });
        });

        function FormatAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="Edit.aspx?id={0}"><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>&nbsp;', rowData['id']);
            return str.ToString();
        }
        function AddItem() {
            document.location.href = "Edit.aspx?id=0";
        }

        function DelItem() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id);
            }
            if (ids.length == 0) return;
            $.messager.confirm('系统提示', '确定删除所选？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerOldUrl,
                        data: { Action: "DeleteJuActivity", ids: ids.join(','), type: "Outlets" },
                        dataType: "json",
                        success: function (resp) {
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
        function Search() {
            var model = {
                policy_object: $.trim($("#selectPolicyObject").val()),
                keyword: $.trim($("#txtKeyword").val())
            }
            $('#grvData').datagrid('load',model);
        }
        function ShowQRCode() {
            var linkurl = "http://" + domain + "/App/PoliceInquiry/search.html";
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
    </script>
</asp:Content>
