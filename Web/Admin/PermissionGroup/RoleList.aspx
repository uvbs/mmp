<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.PermissionGroup.RoleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        @charset "utf-8";
        @import url('//at.alicdn.com/t/font_1454412950_7469454.css');
        .first-right{
            display: block;
        }
        .first-right p{
            padding: 0 0 6px 10px;
            background: #D7D7D7;
        }

        .first-right input[type="checkbox"] {
            position: relative;
            width: 13px;
            height: 13px;
            background: white;
            border: 1px solid #dcdcdc;
            -webkit-border-radius: 1px;
            -moz-border-radius: 1px;
            border-radius: 1px;
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
            top:5px;
            cursor: pointer;
            margin-right: 8px;
        }
        .first-right input[type="checkbox"]:checked::after{
            content: url(/img/checkmark.png);
            display: block;
            position: absolute;
            top: -6px;
            left: -5px;
        }
        .lower-right{
            margin-left:25px;
            display: block;
        }
        .lower-right:after{
            clear: both;
            content: '.';
            height: 0;
            visibility: hidden;
            display: block;
        }
        .lower-right li{
            list-style-type: none;
            margin: 0;
            padding: 0;
            float: left;
            width: 200px;
            line-height: 20px;
        }
        .third-right{
            padding-left:20px;
        }
        .node-expand{
            font-size:13px;
            color:#cccccc;
            cursor: pointer;
        }
        .pull-right{
            float:right;
            line-height: 27px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;系统管理&nbsp;>&nbsp;角色管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="GroupName" width="50">角色</th>
                <th field="GroupDescription" width="10">角色说明</th>
                <th field="has_column" width="10" formatter="FormatColumn">编辑</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                title="添加角色" onclick="ShowPmsColumnSetDig(-1)" id="btnAdd" runat="server">新增角色</a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-delete" plain="true" title="删除角色" onclick="Delete()" id="btnDelete"
                runat="server">删除角色</a>
            <br />
        </div>
    </div>
    <div id="dlgPmsColumnSet" class="easyui-dialog" title="权限组栏目设置" modal="true" closed="true" style="width: 800px; height: 500px; top: 20px;">
        <div class="easyui-panel" style="height: 425px; width: 786px;">
            <div style="padding:0px;">
            <table>
            <tr>
                <td height="25" width="70" align="center">角色名称：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtGroupName" style="width: 140px;" />
                </td>
                <td height="25" width="70" align="center">上级角色：
                </td>
                <td height="25" width="*" align="left">
                    <span id="sp_menu"></span>
                </td>
                <td height="25" width="70" align="center">角色说明：
                </td>
                <td height="25" align="left">
                    <input type="text" id="txtGroupDescription" style="width: 200px;" />
                </td>
            </tr>
            </table>
            </div>
            <div id="divPmsColumns">

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/permissioncolumn/';
        var oldgrouphandlerUrl = '/handler/permission/pmsgroupmanager.ashx';
        var grid;
        var currSelectID = 0;

        $(function () {
            LoadSelect();
            LoadPermissionColumns();
            //加载datagrid
            grid = $("#grvData").datagrid({
                      method: "Post",
                      url: handlerUrl + "rolelist.ashx",
                      height: document.documentElement.clientHeight -50,
                      pagination: true,
                      pageSize: 20,
                      toolbar: '#divToolbar',
                      rownumbers: true,
                      singleSelect: true
                  }
              );
            $(".node-expand").live("click", function () {
                if ($(this).hasClass("icon-arrowdown")) {
                    $(this).removeClass("icon-arrowdown");
                    $(this).addClass("icon-arrowup");
                    $(this).closest(".lower-right").next(".third-right").show();
                }
                else {
                    $(this).removeClass("icon-arrowup");
                    $(this).addClass("icon-arrowdown");
                    $(this).closest(".lower-right").next(".third-right").hide();
                }
            })
            $(".menu_id").live("click", function () {
                var nid = $(this).val();
                $(".menu_" + nid).attr("checked", this.checked);
                var fid = $(this).attr("fid");
                if (fid != undefined) {
                    if (this.checked) {
                        $("#" + fid).attr("checked", this.checked);
                        var pfid = $("#" + fid).attr("fid");
                        if (pfid != undefined) {
                            $("#" + pfid).attr("checked", this.checked);
                        }
                    } else if ($("." + fid + ":checked").length == 0) {
                        $("#" + fid).attr("checked", this.checked);
                        var pfid = $("#" + fid).attr("fid");
                        if (pfid != undefined && $("." + pfid + ":checked").length == 0) {
                            $("#" + pfid).attr("checked", this.checked);
                        }
                    }
                }
            });

            $('#dlgPmsColumnSet').dialog({
                buttons: [{
                    text: "确定",
                    handler: function () {
                        var group_name = $.trim($(txtGroupName).val());
                        if (group_name == '') {
                            $(txtGroupName).val("");
                            $(txtGroupName).focus();
                            $.messager.alert("系统提示", "角色名称不能为空");
                            return;
                        }
                        var pre_id = $.trim($("#ddlPermissionGroup").val());
                        if (pre_id == '') pre_id = 0;
                        var group_describe = $.trim($(txtGroupDescription).val());
                        var colIdsStr = GetSelectPermissionColumns();
                        $.ajax({
                            type: "post",
                            url: handlerUrl + "rolesetpermissioncolumncheckedlist.ashx",
                            data: { group_id: currSelectID, group_name: group_name, group_describe: group_describe, pre_id: pre_id, col_ids: colIdsStr },
                            success: function (result) {
                                if (result.status == true) {
                                    $.messager.show({ title: '系统提示', msg: '操作成功' });
                                    $(dlgPmsColumnSet).dialog('close');
                                    grid.datagrid('reload');
                                } else {
                                    $.messager.alert("系统提示", result.msg);
                                }
                            }
                        });
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgPmsColumnSet').dialog('close');
                    }
                }]
            });

        });

        function GetSelectPermissionColumns() {
            var ids = [];
            $("#divPmsColumns .menu_id:checked").each(function () {
                var id = $(this).val();
                ids.push(id);
            });
            return ids.join(',');
        }
        function ShowPmsColumnSetDig(n_index) {
            if (n_index != -1) {
                var nGroupID = 0;
                var rows = grid.datagrid('getData').rows;
                var row = rows[n_index];
                nGroupID = row.GroupID;
                nGroupName = row.GroupName;
                if (row && currSelectID == row.GroupID && row.GroupID != 0) {
                    $(dlgPmsColumnSet).dialog('open');
                    return;
                }
                $.messager.progress({ text: '正在加载。。。' });
                $("#txtGroupName").val($.trim(row.GroupName.replace('└', '')));
                $("#txtGroupDescription").val(row.GroupDescription);
                $("#ddlPermissionGroup").val(row.PreID);
                $.ajax({
                    type: "Post",
                    url: handlerUrl + "rolecheckedpermissioncolumnlist.ashx",
                    data: { group_id: nGroupID },
                    success: function (result) {
                        $.messager.progress('close');
                        if (result.status == true) {
                            currSelectID = nGroupID;
                            $("#divPmsColumns .third-right").hide();
                            $("#divPmsColumns .menu_id").attr("checked", false);
                            for (var i = 0; i < result.result.length; i++) {
                                var ncol_id = result.result[i];
                                $("#divPmsColumns #menu_" + ncol_id).attr("checked", true);
                            }
                            $(dlgPmsColumnSet).window({ title: '编辑角色：' + nGroupName });
                            $(dlgPmsColumnSet).dialog('open');
                        } else {
                            $.messager.alert("系统提示", result.msg);
                        }
                    }
                });
            }
            else {
                currSelectID = 0;
                $("#txtGroupName").val("");
                $("#txtGroupDescription").val("");
                $("#ddlPermissionGroup").val(0);
                $("#divPmsColumns .menu_id").attr("checked", false);
                $(dlgPmsColumnSet).window({ title: '新增角色' });
                $(dlgPmsColumnSet).dialog('open');
            }
        }
        //批量删除
        function Delete() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].GroupType == 3 || rows[i].GroupType == 4) {
                    $.messager.alert("系统提示", "系统角色不能删除");
                    return false;

                }
                
            }
            $.messager.confirm('系统提示', '确定删除选中角色？', function (o) {
                if (o) {
                    var ids = new Array();
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].GroupID);
                    }
                    $.ajax({
                        type: "Post",
                        url: oldgrouphandlerUrl,
                        data: { Action: "Delete", ids: ids.join(',') },
                        success: function (result) {
                            if (!isNaN(result)) {
                                if (Number(result) > 0) {
                                    $.messager.show({
                                        title: '系统提示',
                                        result: '删除完成'
                                        //msg: '已删除数据' + result + '条'
                                    });
                                    grid.datagrid('reload');
                                    LoadSelect()
                                }
                            }
                        }
                    });
                }
            });
        }
        function FormatColumn(value, rowData, index) {

            if (rowData.GroupType == 3 || rowData.GroupType ==4) {
                return "";
            }
            var str = new StringBuilder();
            var color = value == false ? "color:red;" : "color:green;";
            str.AppendFormat('<a style="{0}" href="javascript:ShowPmsColumnSetDig({1})">编辑</a>', color, index);
            return str.ToString();
        }
        function LoadSelect() {
            $.post(handlerUrl + "selectrolelist.ashx", {}, function (data) {
                if (data.status && data.result) {
                    $("#sp_menu").html(data.result);
                }
            });
        }
        function LoadPermissionColumns() {
            $.ajax({
                type: "Post",
                url: handlerUrl + "userpermissioncolumnlist.ashx",
                success: function (result) {
                    if (result.status == true) {
                        var str = new StringBuilder();
                        for (var i = 0; i < result.result.length; i++) {
                            var i_col = result.result[i];
                            str.AppendFormat('<div class="first-right">');
                            str.AppendFormat('<p><label class="checkbox inline"><input type="checkbox" class="first-menu menu_id" id="menu_{0}" {2} value="{0}">{1}</label></p>',
                                i_col.col_id, i_col.col_name, i_col.col_checked ? 'checked="checked"' : '');
                            if (i_col.children.length > 0) {
                                for (var j = 0; j < i_col.children.length; j++) {
                                    var j_col = i_col.children[j];
                                    str.AppendFormat('<ul class="lower-right">');
                                    str.AppendFormat('<li>');
                                    str.AppendFormat('<label class="checkbox inline"><input type="checkbox" id="menu_{1}" class="second-menu menu_id menu_info menu_{0}" fid="menu_{0}" {3} value="{1}">{2}</label>',
                                        i_col.col_id, j_col.col_id, j_col.col_name, j_col.col_checked ? 'checked="checked"' : '');

                                    if (j_col.children.length > 0) {
                                        str.AppendFormat('<label class="checkbox inline pull-right"><i title="展开/收缩" class="iconfont icon-arrowdown node-expand"></i></label>');
                                    }
                                    str.AppendFormat('</li>');
                                    str.AppendFormat('</ul>');
                                    if (j_col.children.length > 0) {
                                        str.AppendFormat('<div class="third-right" style="display:none;">');
                                        str.AppendFormat('<ul class="lower-right">');
                                        for (var t = 0; t < j_col.children.length; t++) {
                                            var t_col = j_col.children[t];
                                            str.AppendFormat('<li>');
                                            str.AppendFormat('<label class="checkbox inline"><input type="checkbox" id="menu_{2}" class="third-menu menu_id menu_info menu_{0} menu_{1}" fid="menu_{1}" {4} value="{2}">{3}</label>',
                                                i_col.col_id, j_col.col_id, t_col.col_id, t_col.col_name, t_col.col_checked ? 'checked="checked"' : '');
                                            str.AppendFormat('</li>');
                                        }
                                        str.AppendFormat('</ul>');
                                        str.AppendFormat('</div>');
                                    }
                                }
                            }
                            str.AppendFormat('</div>');
                        }
                        $("#divPmsColumns").html("");
                        $("#divPmsColumns").append(str.ToString());
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                }
            });
        }
    </script>
</asp:Content>
