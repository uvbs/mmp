<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PmsGroupManagerV2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.PmsGroupManagerV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>权限组管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                
                <th field="GroupID" width="10">权限组名称
                </th>
                <th field="GroupName" width="20">权限组名称
                </th>

                <th field="GroupDescription" width="75">权限组说明
                </th>
                <th field="GroupType" width="15" formatter="FormatGroupType">权限组类型
                </th>
                <th field="has_column" width="15" formatter="FormatColumn">权限组栏目
                </th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                title="添加权限组" onclick="ShowAdd()" id="btnAdd" runat="server">添加权限组</a>
            <a href="javascript:;"
                class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑权限组" onclick="ShowEdit()"
                id="btnEdit" runat="server">编辑权限组 </a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-delete" plain="true" title="删除权限组" onclick="Delete()" id="btnDelete"
                runat="server">删除权限组 </a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-redo" plain="true" title="复制权限组" onclick="Copy()" id="btnCopy"
                runat="server">复制权限组 </a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit"
                plain="true" title="权限组权限分配" onclick="ShowPmsSetDig()" id="btnPmsSet" 
                runat="server">权限组权限分配 </a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit"
                plain="true" title="权限组菜单设置" onclick="ShowMenuSetDig()" id="btnMenuSet" 
                runat="server">权限组菜单设置 </a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit"
                plain="true" title="权限组栏目设置" onclick="ShowPmsColumnDig()" id="btnPmsColumnSet" 
                runat="server">权限组栏目设置 </a>
        </div>
        <div>
            权限组类型:
            <select id="ddlSelectGroupType">
                <option value="" selected="selected">全部</option>
                <option value="0">普通权限组</option>
                <option value="1">版本权限组</option>
                <option value="2">角色权限组</option>
                <option value="3">管理员</option>
                <option value="4">渠道</option>
            </select>
            权限组站点:
            <select id="ddlSelectWebsite">
                <option value="">全部</option>
                <option value="1" selected="selected">基础</option>
                <option value="2">本站</option>
            </select>
            权限组名称:
            <input id="txtSearch" type="text" style="width: 180px">
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查找</a>
        </div>
        <br />
        <span>注意：操作权限组数据可能需要必要的开发支持，请慎重操作</span>
        <br />
    </div>
    <div id="dlgPmsInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 450px; padding: 10px">

        <table>
            <tr>
                <td height="25" width="25%" align="left">权限组名称：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtGroupName" style="width: 280px;" class="easyui-validatebox"
                        required="true" missingmessage="请输入权限组名称" />
                </td>
            </tr>
            <tr>
                <td height="25" width="25%" align="left">权限组说明：
                </td>
                <td height="25" width="*" align="left">
                    <textarea rows="4" style="width: 280px;" id="txtGroupDescription"></textarea>
                </td>
            </tr>
            <tr>
                <td height="25" width="25%" align="left">权限组类型：
                </td>
                <td height="25" width="*" align="left">
                    <input type="radio" id="rdoGroupType1" name="rdoGroupType" class="positionTop2" value="0" data-value="0" /><label for='rdoGroupType1'>普通权限组</label>
                    <input type="radio" id="rdoGroupType2" name="rdoGroupType" class="positionTop2" value="1" data-value="1"/><label for='rdoGroupType2'>版本权限组</label>
                    <input type="radio" id="rdoGroupType3" name="rdoGroupType" class="positionTop2" value="2" data-value="2"/><label for='rdoGroupType3'>角色权限组</label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <br />
                    <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton">保 存</a>
                    <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton">关 闭</a>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="dlgPmsSet" class="easyui-dialog" title="分配权限" modal="true" closed="true"
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
    <div id="dlgPmsColumnSet" class="easyui-dialog" title="权限组栏目设置" modal="true" closed="true"
        style="width: 800px; height: 500px; top: 20px;">
        <div id="divPmsColumns" class="easyui-panel" style="height: 425px; width: 786px;">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
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
    <script type="text/javascript">

        var handlerUrl = '/Handler/Permission/PmsGroupManager.ashx';
        var handlerPermissionColumnUrl = '/serv/api/admin/permissioncolumn/';
        var grid;
        var currSelectID = 0;
        var currSetGroupID = 0;
        var PermissionHtml = '<% = PermissionHtml %>';
        var MenuHtml = '<% = MenuHtml %>';
        $(function () {

            //          $(window).resize(function () {
            //              $(grvData).datagrid('resize',
            //	            {
            //	                width: document.body.clientWidth,
            //	                height: document.documentElement.clientHeight
            //	            });
            //          });
            //加载栏目选择页
            $("#divPmss").html(PermissionHtml);
            $("#divMenus").html(MenuHtml);
            loadDivPmsColumns();
            //加载datagrid
            grid = $("#grvData").datagrid(
                  {
                      method: "Post",
                      url: handlerUrl,
                      queryParams: { Action: "Query", group_type: $("#ddlSelectGroupType").val(), this_site: $("#ddlSelectWebsite").val() },
                      height: document.documentElement.clientHeight - 110,
                      pagination: true,
                      
                      toolbar: '#divToolbar',
                      rownumbers: true,
                      singleSelect: true

                  }
              );


            //窗体关闭按钮---------------------
            $("#dlgPmsInfo").find("#btnExit").bind("click", function () {
                $("#dlgPmsInfo").window("close");
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
            $("input[name='checktype']").bind("click", function () {
                $(this).closest("fieldset").find("input[name='checksingle']").attr("checked", this.checked);
            })
            $("input[name='checkmenutype']").bind("click", function () {
                $(this).closest("fieldset").find("input[name='checkmenu']").attr("checked", this.checked);
            })
            $("input[name='checkmenu']").bind("click", function () {
                if (this.checked) {
                    $(this).closest("fieldset").find("input[name='checkmenutype']").attr("checked", this.checked);
                }
            })
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

            //权限设置按钮
            $(btnCancelPmsSet).bind("click", function () { $(dlgPmsSet).dialog('close'); })
            $(btnSavePmsSet).bind("click", function () { PmsSet(); })
            //菜单设置按钮
            $(btnCancelMenuSet).bind("click", function () { $(dlgMenuSet).dialog('close'); })
            $(btnSaveMenuSet).bind("click", function () { MenuSet(); })

            //查询按钮点击绑定
            $("#btnSearch").click(function () {
                var searchReq = $.trim($(txtSearch).val());
                grid.datagrid({ url: handlerUrl, queryParams: { Action: "Query", SearchReq: searchReq, group_type: $("#ddlSelectGroupType").val(), this_site: $("#ddlSelectWebsite").val() } });

            });
            $('#dlgPmsColumnSet').dialog({
                buttons: [{
                    text: "提交",
                    handler: function () {
                        $.messager.confirm('系统提示', '确定设定权限组栏目?', function (o) {
                            if (o) {
                                try {
                                    var colIdsStr = GetSelectPermissionColumns();
                                    $.ajax({
                                        type: "post",
                                        url: handlerPermissionColumnUrl + "setpermissioncolumncheckedlist.ashx",
                                        data: { group_id: currSetGroupID, col_ids: colIdsStr },
                                        success: function (result) {
                                            if (result.status == true) {
                                                $.messager.show({ title: '系统提示', msg: '设定权限组栏目成功' });
                                                grid.datagrid('reload');
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
                            $(dlgPmsColumnSet).dialog('close');
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
        function LoadData() {
            grid.datagrid({ url: handlerUrl, queryParams: { Action: "Query" } });
        }


        function ShowAdd() {
            try {
                ClearWinDataByTag('input|textarea', dlgPmsInfo);
                $('#ddlPreMenu').val("0");
                $('#dlgPmsInfo').window(
                          {
                              title: '添加权限组'
                          }
                      );
                $('#rdoGroupType1').attr('checked', true);
                $('#dlgPmsInfo').dialog('open');
                $(btnSave).attr('tag', 'add');

            } catch (e) {
                alert(e);
            }
        }

        function ShowEdit() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            ClearWinDataByTag('input|textarea', dlgPmsInfo);

            $('#dlgPmsInfo').window(
              {
                  title: '编辑权限组'
              }
              );

            $('#dlgPmsInfo').dialog('open');
            $(btnSave).attr('tag', 'edit');

            //加载编辑数据
            currSelectID = rows[0].GroupID;
            $(txtGroupName).val(rows[0].GroupName);

            $('input[name="rdoGroupType"]').each(function () {
                if ($(this).attr("data-value") == rows[0].GroupType) this.checked = true;
            })

            $(txtGroupDescription).val(rows[0].GroupDescription);

        }
        function ShowPmsSetDig() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return;
            }

            $('input[name="checksingle"]').each(function () {
                this.checked = false;
            });
            if (rows.length > 1) {
                $(dlgPmsSet).window({ title: '批量编辑权限组' });
            }
            else {
                $(dlgPmsSet).window({ title: '编辑权限组：' + rows[0].GroupName });

                var pmsids = [];
                if (rows[0].PmsIdsStr != null && rows[0].PmsIdsStr != "") {
                    pmsids = rows[0].PmsIdsStr.split(',');
                }
                //alert(rows[0].PmsIdsStr);
                //alert(pmsids);

                for (var i = 0; i < pmsids.length; i++) {
                    $('input[name="checksingle"]').each(function () {
                        if (pmsids[i] == $(this).val()) {
                            this.checked = true;
                        }
                    });
                }

            }

            $(dlgPmsSet).dialog('open');
        }
        function ShowMenuSetDig() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }

            $('input[name="checkmenutype"]').each(function () {
                this.checked = false;
                $(this).closest("fieldset").find('input[name="checkmenu"]').each(function () {
                    this.checked = false;
                });
            });
            $(dlgMenuSet).window({ title: '设置菜单：' + rows[0].GroupName });
            var menuids = [];
            if (rows[0].MenuIdsStr != null && rows[0].MenuIdsStr != "") {
                menuids = rows[0].MenuIdsStr.split(',');
            }
            //alert(rows[0].PmsIdsStr);
            //alert(pmsids);
            $('input[name="checkmenutype"]').each(function () {
                this.checked = $.inArray($(this).val(), menuids) >= 0;
                $(this).closest("fieldset").find('input[name="checkmenu"]').each(function () {
                    this.checked = $.inArray($(this).val(), menuids) >= 0;
                });
            });
            $(dlgMenuSet).dialog('open');
        }

        function ShowPmsColumnDig() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            ShowPmsColumnSetDig(rows[0].GroupID, rows[0].GroupName, rows[0].PmsColumnIdsStr);
        }
        function ShowPmsColumnSetDigIndex(n_index) {
            var rows = grid.datagrid('getData').rows;
            var row = rows[n_index];
            ShowPmsColumnSetDig(row.GroupID, row.GroupName, row.PmsColumnIdsStr);
        }
        function ShowPmsColumnSetDig(nGroupID, nGroupName, nPmsColumnIdsStr) {
            if (currSetGroupID == nGroupID) {
                $(dlgPmsColumnSet).dialog('open');
                return;
            }
            currSetGroupID = nGroupID;
            $(dlgPmsColumnSet).window({ title: '设置权限组栏目：' + nGroupName });
            var pmscolumnids = [];
            if (nPmsColumnIdsStr != null && nPmsColumnIdsStr != "") {
                pmscolumnids = nPmsColumnIdsStr.split(',');
            }
            $(".menu_id").attr("checked", false);
            for (var i = 0; i < pmscolumnids.length; i++) {
                $("#menu_"+pmscolumnids[i]).attr("checked",true);
            }
            $(dlgPmsColumnSet).dialog('open');
        }
        function loadDivPmsColumns() {
            $.ajax({
                type: "Post",
                url: handlerPermissionColumnUrl + "checkedpermissioncolumnlist.ashx",
                success: function (result) {
                    $.messager.progress('close');
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
                        $("#divPmsColumns").html(str.ToString());
                    } else {
                        $.messager.alert("系统提示", result.msg);
                    }
                }
            });
        }
        function Add() {
            try {
                var model = GetDlgModel();
                if (!CheckDlgInput(model)) {
                    return false;
                }

                $.ajax({
                    type: "Post",
                    url: handlerUrl,
                    data: { Action: "Add", JsonData: JSON.stringify(model).toString() },
                    success: function (result) {
                        if (result == "true") {
                            $.messager.show({
                                title: '系统提示',
                                msg: '添加成功.'
                            });
                            grid.datagrid('reload');
                            $("#dlgPmsInfo").window("close");
                        } else {
                            $.messager.alert("系统提示", "添加失败：" + result);
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
                    url: handlerUrl,
                    data: { Action: "Edit", JsonData: JSON.stringify(model).toString() },
                    success: function (result) {
                        if (result == "true") {
                            $.messager.show({
                                title: '系统提示',
                                msg: '编辑成功.'
                            });
                            grid.datagrid('reload');
                            $("#dlgPmsInfo").window("close");
                        } else {
                            $.messager.alert("系统提示", "编辑失败：" + result);
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
            $.messager.confirm('系统提示', '确定删除选中权限组？', function (o) {
                if (o) {
                    var ids = new Array();
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].GroupID);
                    }
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "Delete", ids: ids.join(',') },
                        success: function (result) {
                            $.messager.show({
                                title: '系统提示',
                                msg: '已删除数据' + result + '条'
                            });
                            grid.datagrid('reload');
                        }
                    });
                }
            });
        }
        //复制
        function Copy() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            $.messager.confirm('系统提示', '确定复制选中权限组？', function (o) {
                if (o) {
                    $.messager.progress({
                        title: '系统提示',
                        msg: '正在复制'
                    });
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "Copy", id: rows[0].GroupID },
                        success: function (result) {
                            $.messager.progress("close");
                            if (result == "true") {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: '已复制完成'
                                });
                                grid.datagrid('reload');
                            }
                            else {
                                $.messager.alert({
                                    title: '系统提示',
                                    msg: result
                                });
                            }
                        }
                    });
                }
            });
        }

        function PmsSet() {
            $.messager.confirm('系统提示', '确定设定选中权限?', function (o) {
                if (o) {
                    try {
                        var rows = grid.datagrid('getSelections');
                        if (!EGCheckIsSelect(rows)) {
                            return;
                        }
                        var groupIds = new Array();
                        for (var i = 0; i < rows.length; i++) {
                            groupIds.push(rows[i].GroupID);
                        }

                        var groupIdsStr = groupIds.join(',');
                        var pmsIdsStr = GetSelectPms();

                        $.ajax({
                            type: "post",
                            url: handlerUrl,
                            data: { Action: 'SetPms', groupIds: groupIdsStr, pmsIds: pmsIdsStr },
                            success: function (result) {
                                if (result == "true") {

                                    $.messager.alert("系统提示", "设置权限成功");
                                    LoadData();
                                } else {
                                    $.messager.alert("系统提示", result);
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
        function MenuSet() {
            $.messager.confirm('系统提示', '确定设定选中权限?', function (o) {
                if (o) {
                    try {
                        var rows = grid.datagrid('getSelections');
                        if (!EGCheckNoSelectMultiRow(rows)) {
                            return;
                        }
                        var menuIdsStr = GetSelectMenu();

                        $.ajax({
                            type: "post",
                            url: handlerUrl,
                            data: { Action: 'SetMenu', groupId: rows[0].GroupID, menuIds: menuIdsStr },
                            success: function (result) {
                                if (result == "true") {
                                    $.messager.alert("系统提示", "设置菜单成功");
                                    LoadData();
                                } else {
                                    $.messager.alert("系统提示", result);
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
        //获取对话框数据实体
        function GetDlgModel() {
            var GroupType = $('input[name="rdoGroupType"]:checked').attr("data-value");
            var model =
              {
                  "GroupID": currSelectID,
                  "GroupName": $.trim($(txtGroupName).val()),
                  "GroupDescription": $.trim($(txtGroupDescription).val()),
                  "GroupType": GroupType,
                  "PreID": 0
              }
            return model;
        }

        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['GroupName'] == '') {
                $(txtGroupName).val("");
                $(txtGroupName).focus();
                return false;
            }
            return true;
        }
        //获取选中权限
        function GetSelectPms() {
            var ids = [];
            $('input[name="checksingle"]:checked').each(function () {
                var id = $(this).val();
                ids.push(id);
            });
            return ids.join(',');
        }
        //获取选中权限
        function GetSelectMenu() {
            var ids = [];
            $('input[name="checkmenutype"]:checked').each(function () {
                var id = $(this).val();
                ids.push(id);
                $(this).closest("fieldset").find('input[name="checkmenu"]:checked').each(function () {
                    var cid = $(this).val();
                    ids.push(cid);
                });
            });
            return ids.join(',');
        }
        function FormatGroupType(_Num) {
            var str = new StringBuilder();
            if (_Num == 1) {
                str.AppendFormat('<span style="color:red;">版本权限组</span>');
            }
            else if (_Num == 2) {
                str.AppendFormat('<span style="color:blue;">角色权限组</span>');
            }
            else if (_Num ==3) {
                str.AppendFormat('<span style="color:blue;">管理员</span>');
            }
            else if (_Num ==4) {
                str.AppendFormat('<span style="color:blue;">渠道</span>');
            }
            else {
                str.AppendFormat('普通权限组');
            }
            return str.ToString();
        }
        function FormatColumn(value, rowData, index) {
            var str = new StringBuilder();
            var color = value == false ? "color:red;" : "color:green;";
            str.AppendFormat('<a style="{0}" href="javascript:ShowPmsColumnSetDigIndex({1})">编辑</a>', color, index);
            return str.ToString();
        }
    </script>
</asp:Content>