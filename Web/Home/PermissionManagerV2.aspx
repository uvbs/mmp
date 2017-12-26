<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PermissionManagerV2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.PermissionManagerV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>权限管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>

                <th field="PermissionName" width="25" formatter="FormatterTitle">权限名称
                </th>
                <th field="Url" width="35" formatter="FormatterTitle">权限链接
                </th>
                <th field="PermissionAction" width="15" formatter="FormatterTitle">方法
                </th>
                <th field="PermissionKey" width="20" formatter="FormatterTitle">代码键值
                </th>
                <th field="PermissionType" width="10" formatter="FormatPermissionType">类型
                </th>
                <th field="PermissionCate" width="10" formatter="FormatterTitle">分类
                </th>
            </tr>
        </thead>
    </table>

    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                title="添加权限" onclick="ShowAdd()" id="btnAdd" runat="server">添加权限</a>
            <a href="javascript:;"
                class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑权限" onclick="ShowEdit()"
                id="btnEdit" runat="server">编辑权限 </a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-delete" plain="true" title="删除权限" onclick="Delete()" id="btnDelete"
                runat="server">删除权限 </a>
            批量设置分类:
              <%=sbCategory2.ToString()%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="BatchSetCategory()">批量设置分类</a> 
        </div>
        <div>
            类型:
            <select id="ddltype">
                <option value="">全部</option>
                <option value="0">页面</option>
                <option value="1">处理器</option>
                <option value="2">操作</option>
            </select>
            分类:
            <%=sbCategory1.ToString() %>
            权限名称或链接:
            <input id="txtSearch" type="text" style="width: 160px">
            代码键值:
            <input id="txtKey" type="text" style="width: 160px">
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查找</a>
        </div>
        <br />
        <span>注意：操作权限数据可能需要必要的开发支持，请慎重操作</span>
        <br />
    </div>
    <div id="dlgPmsInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 320px; padding: 10px">

        <table>
            <tr>
                <td height="25" align="left">权限名称：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtPermissionName" style="width: 200px;" class="easyui-validatebox"
                        required="true" missingmessage="请输入权限名称" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">权限链接：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtUrl" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">键值代码：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtPermissionKey" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">类型：
                </td>
                <td height="25" width="*" align="left">
                    <input type="radio" id="rd0" checked="checked" name="rdotb" class="positionTop2" data-value="0" /><label for='rd0'>页面</label>
                    <input type="radio" id="rd1" name="rdotb" class="positionTop2" data-value="1" /><label for='rd1'>处理器</label>
                    <input type="radio" id="rd2" name="rdotb" class="positionTop2" data-value="2" /><label for='rd2'>操作</label>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">分类：
                </td>
                <td height="25" width="*" align="left">
                    <%=sbCategory.ToString() %>
                </td>
            </tr>
            <tr>
                <td height="25"  align="left">
                    方法(Action)：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtPermissionAction" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">权限说明：
                </td>
                <td height="25" width="*" align="left">
                    <textarea rows="4" style="width: 200px;" id="txtPermissionDescription"></textarea>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton">保 存</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton">关 闭</a>

                </td>
            </tr>
        </table>


    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
     <script type="text/javascript">

        var handlerUrl = '/Handler/Permission/PermissionInfoManage.ashx';
        var grid;
        var currSelectID = 0;

        $(function () {

            //加载datagrid
            grid = $("#grvData").datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "Query", type: $('#ddltype').val(), cateID: $('#ddlPermissionCate1').val() },
	                height: document.documentElement.clientHeight - 110,
	                nowrap: false,
	                pagination: true,
	                toolbar: '#divToolbar',
	                rownumbers: true


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

            //查询按钮点击绑定
            $("#btnSearch").click(function () {
                var searchReq = $.trim($(txtSearch).val());
                var permissionKey = $.trim($(txtKey).val());
                
                grid.datagrid({ url: handlerUrl, queryParams: { Action: "Query", type: $('#ddltype').val(), cateID: $('#ddlPermissionCate1').val(), key: permissionKey, SearchReq: searchReq } });

            });

            //LoadMenuSelectList();
        });

        function LoadData() {
            grid.datagrid({ url: handlerUrl, queryParams: { Action: "Query", type: $('#ddltype').val(), cateID: $('#ddlPermissionCate1').val() } });
        }


        function ShowAdd() {
            try {
                ClearWinDataByTag('input|textarea', dlgPmsInfo);
                $('#dlgPmsInfo').window(
                        {
                            title: '添加权限'
                        }
                    );

                $('#dlgPmsInfo').dialog('open');
                $(btnSave).attr('tag', 'add');

                $('input[name="rdotb"]').each(function () {
                    if ($(this).attr("data-value") == 0) this.checked = true;
                });
                $('#ddlPermissionCate').val(0);
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
                title: '编辑权限'
            }
            );

            $('#dlgPmsInfo').dialog('open');
            $(btnSave).attr('tag', 'edit');

            //加载编辑数据
            currSelectID = rows[0].PermissionID;
            $('#ddlPreMenu').val(rows[0].MenuID);
            $(txtPermissionName).val(rows[0].PermissionName);
            $(txtUrl).val(rows[0].Url);
            $(txtPermissionDescription).val(rows[0].PermissionDescription);
            $(txtPermissionKey).val(rows[0].PermissionKey);
            $('input[name="rdotb"]').each(function () {
                if ($(this).attr("data-value") == rows[0].PermissionType) this.checked = true;
            });
            $('#ddlPermissionCate').val(rows[0].PermissionCateId);
            $(txtPermissionAction).val(rows[0].PermissionAction);
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
            $.messager.confirm('系统提示', '确定删除选中数据？', function (o) {
                if (o) {
                    var ids = new Array();
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].PermissionID);
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

        //获取对话框数据实体
        function GetDlgModel() {
            var PermissionType = $('input[name="rdotb"]:checked').attr("data-value");
            var model =
            {
                "PermissionID": currSelectID,
                "PermissionName": $.trim($(txtPermissionName).val()),
                "Url": $.trim($(txtUrl).val()),
                "PermissionCateId": $('#ddlPermissionCate').val(),
                "PermissionType": PermissionType,
                "MenuID": 0,
                "PermissionDescription": $(txtPermissionDescription).val(),
                "PermissionKey": $(txtPermissionKey).val(),
                "PermissionAction": $(txtPermissionAction).val()
            }
            return model;
        }

        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['PermissionName'] == '') {
                $(txtPermissionName).val("");
                $(txtPermissionName).focus();
                return false;
            }
            //            if (model['PermissionKey'] == '') {
            //                $(txtPermissionKey).val("");
            //                $(txtPermissionKey).focus();
            //                return false;
            //            }
            return true;
        }
        function FormatPermissionType(_Num) {
            var str = new StringBuilder();
            if (_Num == 0) {
                str.AppendFormat('页面');
            }
            else if (_Num == 1) {
                str.AppendFormat('<span style="color:blue;">处理器</span>');
            }
            return str.ToString();
        }
        function BatchSetCategory() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            if ($("#ddlPermissionCate2").val() == "0") {
                Alert("请选择要设置的分类");
                return;
            }

            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].PermissionID);
            }
            
            var categoryname = $("#ddlPermissionCate2").find("option:selected").text().replace('└', '');
            //
            $.messager.confirm("系统提示", "确定将所选权限的分类修改为 " + categoryname, function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "BatchSetCategory", ids: ids.join(','), PermissionCateId: $("#ddlPermissionCate2").val() },
                        dataType: "json",
                        success: function (resp) {
                            if (resp) {
                                $('#grvData').datagrid('reload');
                                Alert("修改完成");

                            }
                            else {
                                Alert("修改失败");
                            }
                        }

                    });
                }
            });
        }
    </script>
</asp:Content>