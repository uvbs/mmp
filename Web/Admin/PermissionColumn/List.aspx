<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.PermissionColumn.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;权限栏目管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="list_data" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="id" width="5">栏目编号</th>
                <th field="name" width="50">栏目名称</th>
                <th field="order_num" width="10">同级排序</th>
                <th field="has_menu" width="10" formatter="FormatterMenu">菜单</th>
                <th field="has_permission" width="10" formatter="FormatterPermission">权限</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                title="添加栏目" onclick="ShowAdd()" id="btnAdd" runat="server">添加栏目</a>
            <a href="javascript:;"
                class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑栏目" onclick="ShowEdit()"
                id="btnEdit" runat="server">编辑栏目</a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-remove" plain="true" title="删除栏目" onclick="Delete()" id="btnDelete"
                runat="server">删除栏目</a>
            <br />
            <span>注意：操作菜单数据可能需要必要的开发支持，请慎重操作</span>
        </div>
    </div>
    <div id="dlgMenuInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 500px; padding: 15px">
        <table style="width:100%;">
            <tr>
                <td height="25" width="100" align="left">所属栏目：
                </td>
                <td height="25" width="*" align="left">
                    <span id="sp_menu" style="width: 90%;"></span>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">栏目名称<span style="color:red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtName" style="width: 90%;" class="easyui-validatebox"
                        required="true" missingmessage="请输入栏目名称" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">同级排序<span style="color:red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtSort" style="width: 90%;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"
                         class="easyui-validatebox" required="true" missingmessage="请输入排序"/>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="padding-top:15px;">
                    <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">保 存</a>
                    <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">关 闭</a>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgPmsSet" class="easyui-dialog" title="权限设置" modal="true" closed="true"
        style="width: 800px; height: 500px; top: 20px;">
        <div id="divPmss" class="easyui-panel" style="height: 425px; width: 786px;">
        </div>
        <div style="float: right; padding: 5px;">
            <a href="javascript:void(0)" id="btnSavePmsSet" class="easyui-linkbutton">保 存</a> <a href="javascript:void(0)" id="btnCancelPmsSet" class="easyui-linkbutton">关 闭</a>
        </div>
    </div>
    <div id="dlgMenuSet" class="easyui-dialog" title="菜单设置" modal="true" closed="true"
        style="width: 800px; height: 500px; top: 20px;">
        <div id="divMenus" class="easyui-panel" style="height: 425px; width: 786px;">
        </div>
        <div style="float: right; padding: 5px;">
            <a href="javascript:void(0)" id="btnSaveMenuSet" class="easyui-linkbutton">保 存</a> <a href="javascript:void(0)" id="btnCancelMenuSet" class="easyui-linkbutton">关 闭</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/permissioncolumn/';
        var handlerPermissionUrl = '/serv/api/admin/permission/';
        var grid;
        var currSelectID = 0;
        var currSetPerID = 0;
        var currSetMenuID = 0;
        var columCount = 3;
        var _w = 740 / columCount;
        var mColumCount = 4;
        var _mw = 740 / mColumCount;
        $(function () {
            LoadMenuSelectList();
            GetWebsiteMenuList();
            GetWebsitePermissionList();
            grid = jQuery("#list_data").datagrid({
                method: "Post",
                height: document.documentElement.clientHeight - 50,
                toolbar: '#divToolbar',
                pagination: true,
                rownumbers: true,
                pageSize: 20,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    //$('#grvData').datagrid('loaded');
                }
            });

            $('#list_data').datagrid('getPager').pagination({
                onSelectPage: function (pPageIndex, pPageSize) {
                    //改变opts.pageNumber和opts.pageSize的参数值，用于下次查询传给数据层查询指定页码的数据   
                    loadData();
                }
            });
            //初始加载
            loadData();

            //窗体关闭按钮---------------------
            $("#dlgMenuInfo").find("#btnExit").bind("click", function () {
                $("#dlgMenuInfo").window("close");
            });
            //窗体保存按钮---------------------
            $("#btnSave").bind("click", function () {
                var tag = jQuery.trim(jQuery(this).attr("tag"));
                if (tag == "add") {
                    //添加
                    Add();
                    return;
                }
                else {
                    //修改
                    Edit();
                    return;
                }
            });

            //权限设置按钮
            $(btnCancelPmsSet).bind("click", function () { $(dlgPmsSet).dialog('close'); })
            $(btnSavePmsSet).bind("click", function () { PmsSet(); })
            //菜单设置按钮
            $(btnCancelMenuSet).bind("click", function () { $(dlgMenuSet).dialog('close'); })
            $(btnSaveMenuSet).bind("click", function () { MenuSet(); })


            $(".checkPerType").live("click", function () {
                $(this).closest("fieldset").find(".checkPer").attr("checked", this.checked);
            })
            $(".checkMenuParent").live("click", function () {
                $(this).closest("fieldset").find(".checkMenu").attr("checked", this.checked);
            })
            $(".checkMenu").live("click", function () {
                if (this.checked) {
                    $(this).closest("fieldset").find(".checkMenuParent").attr("checked", this.checked);
                }
                else if ($(this).closest("fieldset").find(".checkMenu:checked").length == 0) {
                    $(this).closest("fieldset").find(".checkMenuParent").attr("checked", this.checked);
                }
            })
        });

        function loadData() {
            var gridOpts = $('#list_data').datagrid('options');
            $('#list_data').datagrid('loading');//打开等待div   
            $.post(
                handlerUrl + "list.ashx",
                { page: gridOpts.pageNumber, rows: gridOpts.pageSize },
                function (data, status) {
                    $('#list_data').datagrid('loaded');//关闭等待div   
                    if (data.status && data.result.list) {
                        $('#list_data').datagrid('loadData', { "total": data.result.totalcount, "rows": data.result.list });
                    }
                });
        }
        function ShowAdd() {
            ClearWinDataByTag('input', dlgMenuInfo);
            //加载菜单
            $('#dlgMenuInfo').window(
            {
                title: '添加栏目'
            });

            $('#dlgMenuInfo').dialog('open');
            $(btnSave).attr('tag', 'add');
        }

        function ShowEdit() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            ClearWinDataByTag('input', dlgMenuInfo);

            $('#dlgMenuInfo').window(
            {
                title: '编辑栏目'
            });

            $('#dlgMenuInfo').dialog('open');
            $(btnSave).attr('tag', 'edit');

            //加载编辑数据
            currSelectID = rows[0].id;
            $('#ddlPermissionColumn').val(rows[0].pre_id);
            $(txtName).val($.trim(rows[0].name).replace('└', ''));
            $(txtSort).val(rows[0].order_num);
        }
        function ShowMenus(relationID) {
            if (currSetMenuID == relationID) {
                $(dlgMenuSet).dialog('open');
                return;
            }
            $.messager.progress({ text: '正在加载。。。' });
            $.ajax({
                type: "Post",
                url: handlerUrl + "add.ashx",
                url: handlerPermissionUrl + "checkedmenulist.ashx",
                data: { relation_id: relationID },
                success: function (data) {
                    $.messager.progress('close');
                    if (data.status == true) {
                        currSetMenuID = relationID;
                        $("#divMenus .ckMenu").attr("checked", false);
                        for (var i = 0; i < data.result.length; i++) {
                            var nmenu_id = data.result[i];
                            $("#divMenus .cbMenu_" + nmenu_id).attr("checked", true);
                        }
                        $(dlgMenuSet).dialog('open');
                    } else {
                        $.messager.alert("系统提示", data.msg);
                    }
                }
            });
        }
        function FormatterMenu(value, rowData) {
            var str = new StringBuilder();
            var color = value == false ? "color:red;" : "color:green;";
            str.AppendFormat('<a style="{0}" href="javascript:ShowMenus({1})">菜单</a>', color, rowData["id"]);
            return str.ToString();
        }
        function ShowPermissions(relationID) {
            if (currSetPerID == relationID) {
                $(dlgPmsSet).dialog('open');
                return;
            }
            $.messager.progress({ text: '正在加载。。。' });
            $.ajax({
                type: "Post",
                url: handlerPermissionUrl + "checkedpermissionlist.ashx",
                data: { relation_id: relationID },
                success: function (data) {
                    $.messager.progress('close');
                    if (data.status == true) {
                        currSetPerID = relationID;
                        $("#divPmss .ckPer").attr("checked", false);
                        for (var i = 0; i < data.result.length; i++) {
                            var npms_id = data.result[i];
                            $("#divPmss .ckPer_" + npms_id).attr("checked", true);
                        }
                        $(dlgPmsSet).dialog('open');
                    } else {
                        $.messager.alert("系统提示", data.msg);
                    }
                }
            });
        }
        function FormatterPermission(value, rowData) {
            var str = new StringBuilder();
            var color = value == false ? "color:red;" : "color:green;";
            str.AppendFormat('<a style="{0}" href="javascript:ShowPermissions({1})">权限</a>', color, rowData["id"]);
            return str.ToString();
        }
        function PmsSet() {
            $.messager.confirm('系统提示', '确定设定选中权限?', function (o) {
                if (o) {
                    try {
                        var pmsIdsStr = GetSelectPms();
                        $.ajax({
                            type: "post",
                            url: handlerPermissionUrl + "setpermissioncheckedlist.ashx",
                            data: { relation_id: currSetPerID, pms_ids: pmsIdsStr },
                            success: function (result) {
                                if (result.status == true) {
                                    $.messager.show({title: '系统提示',msg: '设置权限成功'});
                                    loadData();
                                } else {
                                    $.messager.alert("系统提示", result.msg);
                                }
                            }
                        });

                    } catch (e) {
                        alert(e);
                    }
                } else {
                }
                $(dlgPmsSet).dialog('close');
            });
        }

        //获取选中权限
        function GetSelectPms() {
            var ids = [];
            $('.checkPer:checked').each(function () {
                var id = $(this).val();
                ids.push(id);
            });
            return ids.join(',');
        }

        function MenuSet() {
            $.messager.confirm('系统提示', '确定设定选中菜单?', function (o) {
                if (o) {
                    try {
                        var menuIdsStr = GetSelectMenus();
                        $.ajax({
                            type: "post",
                            url: handlerPermissionUrl + "setmenucheckedlist.ashx",
                            data: { relation_id: currSetMenuID, menu_ids: menuIdsStr },
                            success: function (result) {
                                if (result.status == true) {
                                    $.messager.show({ title: '系统提示', msg: '设置菜单成功' });
                                    loadData();
                                } else {
                                    $.messager.alert("系统提示", result.msg);
                                }
                            }
                        });

                    } catch (e) {
                        alert(e);
                    }
                } else {
                }
                $(dlgMenuSet).dialog('close');
            });
        }

        //获取选中菜单
        function GetSelectMenus() {
            var ids = [];
            $('input[type="checkbox"][name="checkmenu"]:checked').each(function () {
                var id = $(this).val();
                ids.push(id);
            });
            return ids.join(',');
        }
        

        function Add() {
            try {
                var model = GetDlgModel();
                if (!CheckDlgInput(model)) {
                    return false;
                }

                $.ajax({
                    type: "Post",
                    url: handlerUrl+"add.ashx",
                    data: model,
                    success: function (result) {
                        if (result.status == true) {
                            $.messager.show({
                                title: '系统提示',
                                msg: '添加成功.'
                            });
                            LoadMenuSelectList();
                            loadData();
                            $("#dlgMenuInfo").window("close");
                        } else {
                            $.messager.alert("系统提示", "添加失败：" + result.msg);
                        }
                    }
                });

            } catch (e) {
                alert(e);
            }
        }

        function Edit() {
            try {
                var model = GetDlgModel();
                if (!CheckDlgInput(model)) {
                    return false;
                }
                $.ajax({
                    type: "Post",
                    url: handlerUrl + "update.ashx",
                    data: model,
                    success: function (result) {
                        if (result.status == true) {
                            $.messager.show({
                                title: '系统提示',
                                msg: '编辑成功.'
                            });
                            LoadMenuSelectList();
                            loadData();
                            $("#dlgMenuInfo").window("close");
                        } else {
                            $.messager.alert("系统提示", "编辑失败：" + result.msg);
                        }
                    }
                });

            } catch (e) {
                alert(e);
            }

        }

        //批量删除
        function Delete() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm('系统提示', '确定删除选中栏目(将递归删除子栏目，菜单，权限)？', function (o) {
                if (o) {
                    var ids = new Array();
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].id);
                    }
                    $.ajax({
                        type: "Post",
                        url: handlerUrl+"delete.ashx",
                        data: { ids: ids.join(',') },
                        success: function (result) {
                            if (result.status == true) {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: result.msg
                                });
                                LoadMenuSelectList();
                                loadData();
                            } else {
                                $.messager.alert("系统提示", "编辑失败：" + result.msg);
                            }
                        }
                    });
                }
            });
        }

        //加载菜单选择列表
        function LoadMenuSelectList() {
            $.post(handlerUrl + "selectlist.ashx", {}, function (data) {
                if (data.status && data.result) {
                    $("#sp_menu").html(data.result);
                }
            });
        }
        //获取对话框数据实体
        function GetDlgModel() {
            var model =
            {
                "id": currSelectID,
                "name": $.trim($(txtName).val()),
                "pre_id": $.trim($('#ddlPermissionColumn').val()),
                "order_num": $.trim($(txtSort).val())
            }
            return model;
        }

        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['name'] == '') {
                $(txtName).val("");
                $(txtName).focus();
                return false;
            }
            if (model["order_num"] == '') {
                model["order_num"] = 0;
            }
            if (model["pre_id"] == '') {
                model["pre_id"] = 0;
            }
            return true;
        }

        function GetWebsiteMenuList() {
            $.ajax({
                type: "Post",
                url: handlerPermissionUrl + "websitemenulist.ashx",
                success: function (result) {
                    if (result.status == true) {
                        var str = new StringBuilder();
                        for (var i = 0; i < result.result.length; i++) {
                            str.AppendFormat('<fieldset style="padding: 0px 10px 10px 10px; margin-top:10px; ">');
                            str.AppendFormat('<legend><input id="cbMenu_{0}" title="{1}" type="checkbox" name="checkmenu" class="positionTop2 checkMenuParent ckMenu cbMenu_{0}" {2} value="{0}" /> <label title="{1}" for="cbMenu_{0}">{1}</label></legend>',
                                result.result[i].menu_id, result.result[i].menu_name,
                                    result.result[i].menu_checked ? 'checked="checked"' : "");
                            str.AppendFormat('<ul style="width:100%;">');
                            for (var j = 0; j < result.result[i].children.length; j++) {
                                str.AppendFormat('<li style="width:{0}px;float:left;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;">', _mw);
                                str.AppendFormat('<input id="cbMenu_{0}" title="{1}" type="checkbox" name="checkmenu" class="positionTop2 checkMenu ckMenu cbMenu_{0}" {2} value="{0}" /><label title="{1}" for="cbMenu_{0}">{1}</label><br />',
                                    result.result[i].children[j].menu_id, result.result[i].children[j].menu_name,
                                    result.result[i].children[j].menu_checked ? 'checked="checked"' : "");
                                str.AppendFormat('</li>');
                            }
                            str.AppendFormat("</ul>");
                            str.AppendFormat('</fieldset>');
                        }
                        $("#divMenus").html("");
                        $("#divMenus").append(str.ToString());
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                }
            });
        }

        function GetWebsitePermissionList() {
            $.ajax({
                type: "Post",
                url: handlerPermissionUrl + "websitepermissionlist.ashx",
                success: function (result) {
                    if (result.status == true) {
                        var str = new StringBuilder();
                        for (var i = 0; i < result.result.length; i++) {
                            str.AppendFormat('<fieldset style="padding: 0px 10px 10px 10px; margin-top:10px; ">');
                            str.AppendFormat('<legend><input id="cbPerType_{0}" title="{1}" type="checkbox" name="checkPerType" class="positionTop2 checkPerType ckPer " value="{0}" /> <label title="{1}" for="cbPerType_{0}">{1}</label></legend>', result.result[i].cate_id, result.result[i].cate_name);
                            str.AppendFormat('<ul style="width:100%;">');
                            for (var j = 0; j < result.result[i].permission_list.length; j++) {
                                str.AppendFormat('<li style="width:{0}px;float:left;overflow:hidden;white-space: nowrap;text-overflow: ellipsis;">', _w);
                                str.AppendFormat('<input id="cbPer_{0}" title="{1}" type="checkbox" name="checksingle" class="positionTop2 checkPer ckPer ckPer_{0}" {2} value="{0}" /><label title="{1}" for="cbPer_{0}">{1}</label><br />',
                                    result.result[i].permission_list[j].permission_id, result.result[i].permission_list[j].permission_name,
                                    result.result[i].permission_list[j].permission_checked ? 'checked="checked"' : "");
                                str.AppendFormat('</li>');
                            }
                            str.AppendFormat("</ul>");
                            str.AppendFormat('</fieldset>');
                        }
                        $("#divPmss").html("");
                        $("#divPmss").append(str.ToString());
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                }
            });
        }
    </script>
</asp:Content>
