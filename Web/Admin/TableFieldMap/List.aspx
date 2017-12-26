<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.TableFieldMap.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <span><%=module_name %>字段设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="divToolbar" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd()">添加字段</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit()">编辑字段</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除字段</a>
            <a href="javascript:void(0)" title="上移" class="easyui-linkbutton" plain="true" onclick="SetFieldSort(0)"><img src="/MainStyle/Res/easyui/themes/icons/up.png" />上移</a>
            <a href="javascript:void(0)" title="下移" class="easyui-linkbutton" plain="true" onclick="SetFieldSort(1)"><img src="/MainStyle/Res/easyui/themes/icons/down.png" />下移</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="ShowCopy()">批量复制字段到其他类型</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="ShowSetField('trSetFieldIsNotNull','批量设置是否必填')">批量设置是否必填</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="ShowSetField('trSetFieldIsShowInList','批量设置是否列表显示')">批量设置是否列表显示</a>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="ShowSetField('trSetFieldIsReadOnly','批量设置是否只读')">批量设置是否只读</a>
        </div>
    </div>
    <table id="grvData" cellspacing="0" cellpadding="0" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="Field" width="20">Field
                </th>
                <th field="MappingName" width="20">名称
                </th>
                <th field="FieldIsNull" formatter="changeisnull" width="10">必填项
                </th>
                <th field="FieldType" formatter="formartfieldtype" width="10">格式类型
                </th>
                <th field="FormatValiFunc" formatter="formartvalitype" width="20">验证格式
                </th>
                <th field="IsShowInList" formatter="formartshow" width="10">列表显示
                </th>
                <th field="IsReadOnly" formatter="formartReadOnly" width="10">是否只读
                </th>
            </tr>
        </thead>
    </table>
    <div id="winInputInfo" class="easyui-dialog" closed="true" title="字段设置" modal="true" style="width: 400px; padding: 10px;">
        <table style="margin: auto;">
            <tr>
                <td align="right">Field:
                </td>
                <td style="text-align: left">
                    <select id="ddlField">
                        <option value=""></option>
                        <% foreach (var item in fieldList)
                           { %>
                        <option value="<%=item %>"><%=item %></option>
                          <% } %>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right">名称:
                </td>
                <td style="text-align: left">
                    <input type="text" id="txtMappingName" class="easyui-validatebox" required="true"
                        missingmessage="请输入名称" invalidmessage="请输入名称" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td align="right">是否必填:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoIsNotNull" name="rd" class="positionTop2" checked="checked" data-value="1" /><label for='rdoIsNotNull'>是</label>
                    <input type="radio" id="rdoIsNull" name="rd" class="positionTop2" data-value="0" /><label for='rdoIsNull'> 否</label>
                </td>
            </tr>
            <tr>
                <td align="right">类型:
                </td>
                <td style="text-align: left">
                    <select id="ddlFieldtype" style="width: 200px;">
                        <option value="">默认</option>
                        <option value="number">数字</option>
                        <option value="img">图片</option>
                        <option value="sex">性别</option>
                        <option value="date">日期</option>
                        <option value="mult">多行文本</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right">格式验证:
                </td>
                <td style="text-align: left">
                    <select id="ddlFormatValiFunc" style="width: 200px;">
                        <option value="">无</option>
                        <option value="number">数字</option>
                        <option value="phone">手机</option>
                        <option value="email">电子邮箱</option>
                        <option value="url">网址</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right">列表显示:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoIsShowInList1" class="positionTop2" name="rdoIsShowInList" checked="checked" data-value="1" /><label for='rdoIsShowInList1'>显示</label>
                    <input type="radio" id="rdoIsShowInList2" class="positionTop2" name="rdoIsShowInList" data-value="0" /><label for='rdoIsShowInList2'>不显示</label>
                </td>
            </tr>
            <tr>
                <td align="right">是否只读:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoIsReadOnly1" class="positionTop2" name="rdoIsReadOnly" checked="checked" data-value="0" /><label for='rdoIsReadOnly1'>可编辑</label>
                    <input type="radio" id="rdoIsReadOnly2" class="positionTop2" name="rdoIsReadOnly" data-value="1" /><label for='rdoIsReadOnly2'>只读</label>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgMappingType" class="easyui-dialog" closed="true" title="目标类型" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>类型:
                </td>
                <td>
                    <input id="txtMappingType" type="number" style="width:200px;" onkeyup="value=value.replace(/[^\d]/g,'') "
                        placeholder="请输入数字" />
                </td>
            </tr>
        </table>
    </div>
     <div id="dlgSetField" class="easyui-dialog" closed="true" title="批量设置是否只读" style="width: 400px; padding: 15px;">
        <table>
            <tr id="trSetFieldIsNotNull" class="trSetField">
                <td align="right">是否必填:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoFieldIsNotNull" class="positionTop2" name="rdoFieldIsNull" checked="checked" data-value="1" /><label for='rdoFieldIsNotNull'>是</label>
                    <input type="radio" id="rdoFieldIsNull" class="positionTop2" name="rdoFieldIsNull" data-value="0" /><label for='rdoFieldIsNull'> 否</label>
                </td>
            </tr>
            <tr id="trSetFieldIsShowInList" class="trSetField">
                <td align="right">列表显示:
                </td>
                <td style="text-align: left">
                    <input type="radio" id="rdoFieldIsShowInList1" class="positionTop2" name="rdoFieldIsShowInList" checked="checked" data-value="1" /><label for='rdoFieldIsShowInList1'>显示</label>
                    <input type="radio" id="rdoFieldIsShowInList2" class="positionTop2" name="rdoFieldIsShowInList" data-value="0" /><label for='rdoFieldIsShowInList2'>不显示</label>
                </td>
            </tr>
            <tr id="trSetFieldIsReadOnly" class="trSetField">
                <td>
                    是否只读:
                </td>
                <td>
                    <input type="radio" id="rdoFieldIsReadOnly1" class="positionTop2" name="rdoFieldIsReadOnly" checked="checked" data-value="0" /><label for='rdoFieldIsReadOnly1'>可编辑</label>
                    <input type="radio" id="rdoFieldIsReadOnly2" class="positionTop2" name="rdoFieldIsReadOnly" data-value="1" /><label for='rdoFieldIsReadOnly2'>只读</label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/tablefieldmap/';
        var table_name = '<%= Request["table_name"] %>';
        var mapping_type = '<%= string.IsNullOrWhiteSpace(Request["mapping_type"])?"0":Request["mapping_type"] %>';
        var cur_AutoId = 0;
        var selectCurIndex = -1;
        var curSetField = '';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl + "list.ashx",
                queryParams: { mapping_type: mapping_type, table_name: table_name },
                height: document.documentElement.clientHeight - 70,
                toolbar: '#divToolbar',
                striped: true,
                loadFilter: pagerFilter,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                    if (selectCurIndex > -1) {
                        $('#grvData').datagrid('selectRow', selectCurIndex);
                        selectCurIndex = -1;
                    }
                },
                onBeforeLoad: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loading');
                },
            });
            $("#winInputInfo").dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        if (cur_AutoId == 0) {
                            Add();
                        }
                        else{
                            Edit();
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $("#winInputInfo").dialog('close');
                    }
                }]
            });
            $("#dlgMappingType").dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        Copy();
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $("#dlgMappingType").dialog('close');
                    }
                }]
            });
            $("#dlgSetField").dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        SetField();
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $("#dlgSetField").dialog('close');
                    }
                }]
            });
        });
        
        function changeisnull(value, rowData, rowIndex) {
            if (value == 1) return "必填";
            return "";
        }
        function formartfieldtype(value, rowData, rowIndex) {
            if (value == "number") return "数字";
            if (value == "img") return "图片";
            if (value == "sex") return "性别";
            if (value == "date") return "日期";
            if (value == "mult") return "多行文本";
            return "";
        }
        function formartvalitype(value, rowData, rowIndex) {
            if (value == "number") return "数字";
            if (value == "phone") return "手机";
            if (value == "email") return "电子邮箱";
            if (value == "url") return "网址";
            return "";
        }
        function formartshow(value, rowData, rowIndex) {
            if (value == 1) return "显示";
            return '<span style="color:red;">隐藏</span>';
        }
        function formartReadOnly(value,rowData,rowIndex) {
            if (value == 0) return "可编辑";
            return '<span style="color:red;">只读</span>';
        }
        function GetModel() {
            var model = {
                AutoId: cur_AutoId,
                Field: $.trim($("#ddlField").val()),
                MappingName: $.trim($("#txtMappingName").val()),
                FieldType: $.trim($("#ddlFieldtype").val()),
                FormatValiFunc: $.trim($("#ddlFormatValiFunc").val()),
                FieldIsNull: rdoIsNull.checked?0:1,
                IsShowInList: rdoIsShowInList2.checked?0:1,
                IsReadOnly: rdoIsReadOnly2.checked ? 1 : 0,
                TableName:table_name,
                MappingType:mapping_type
            }
            return model;
        }
        function Add() {
            var model = GetModel();
            if (model.Field == "") {
                $.messager.alert('系统提示', "请选择字段");
                return;
            }
            if (model.MappingName == "") {
                $.messager.alert('系统提示', "请输入名称");
                return;
            }
            $.ajax({
                type: "Post",
                url: handlerUrl + "add.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $('#grvData').datagrid('reload');
                        $("#winInputInfo").dialog('close');
                        $.messager.show({
                            title: '系统提示',
                            msg: resp.msg
                        });
                    }
                    else {
                        $.messager.alert('系统提示', resp.msg);
                    }
                }
            });
        }
        function Edit() {
            var model = GetModel();
            if (model.Field == "") {
                $.messager.alert('系统提示', "请选择字段");
                return;
            }
            if (model.MappingName == "") {
                $.messager.alert('系统提示', "请输入名称");
                return;
            }
            $.ajax({
                type: "Post",
                url: handlerUrl + "update.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $('#grvData').datagrid('reload');
                        $("#winInputInfo").dialog('close');
                        $.messager.show({
                            title: '系统提示',
                            msg: resp.msg
                        });
                    }
                    else {
                        $.messager.alert('系统提示', resp.msg);
                    }
                }
            });
        }
        function ShowEdit() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            cur_AutoId = rows[0].AutoId;
            $("#ddlField").val(rows[0].Field);
            $("#ddlField")[0].disabled = true;
            $("#txtMappingName").val(rows[0].MappingName);
            $("#ddlFieldtype").val(rows[0].FieldType);
            $("#ddlFormatValiFunc").val(rows[0].FormatValiFunc);
            if (rows[0].FieldIsNull == 0) { rdoIsNull.checked = true; }
            else { rdoIsNotNull.checked = true; }
            if (rows[0].IsShowInList == 0) {rdoIsShowInList2.checked = true;}
            else { rdoIsShowInList1.checked = true; }
            if (rows[0].IsReadOnly == 1) { rdoIsReadOnly2.checked = true; }
            else { rdoIsReadOnly1.checked = true; }

            $("#winInputInfo").dialog('open');
        }
        function ShowAdd() {
            $("#ddlField")[0].disabled = false;
            ClearWinData();
            cur_AutoId = 0;
            $("#winInputInfo").dialog('open');
        }
        function Copy() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var idList = [];
            for (var i = 0; i < rows.length; i++) {
                idList.push(rows[i].AutoId);
            }
            var tMappingType = $.trim($("#txtMappingType").val());
            if (tMappingType == "" || isNaN(tMappingType)) {
                $.messager.alert('系统提示', "请输入类型(数字)");
                return;
            }

            if (tMappingType == mapping_type) {
                $.messager.alert('系统提示', "不能复制到本身类型");
                return;
            }
            $.messager.confirm('系统提示', '确定复制所选字段到新类型？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "copy.ashx",
                        data: {
                            ids: idList.join(','),
                            mapping_type: mapping_type,
                            t_mapping_type: tMappingType,
                            table_name: table_name
                        },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                $("#dlgMappingType").dialog('close');
                                $.messager.show({
                                    title: '系统提示',
                                    msg: resp.msg
                                });
                            }
                            else {
                                $.messager.alert('系统提示', resp.msg);
                            }
                        }
                    });
                }
            });
        }
        function ShowCopy() {
            $("#txtMappingType").val("");
            $("#dlgMappingType").dialog('open');
        }
            
        //窗体清除数据
        function ClearWinData() {
            $("#ddlField").val("");
            $("#txtMappingName").val("");
            rdoIsNotNull.checked = true;
            $("#ddlFieldtype").val("");
            $("#ddlFormatValiFunc").val("");
            rdoIsShowInList1.checked = true;
            rdoIsReadOnly1.checked = true;
        }
        function SetFieldSort(num) {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            var nIndex = $("#grvData").datagrid('getRowIndex', rows[0]);
            var allData = $("#grvData").datagrid('getData');
            var nLength = allData.rows.length;
            var model = {
                id: rows[0].AutoId,
                other_sort: rows[0].Sort
            };
            var tempIndex = 0;
            if (num == 0){
                if (nIndex == 0)return;
                tempIndex = nIndex - 1;
                model.other_id = allData.rows[tempIndex].AutoId;
                model.sort = allData.rows[tempIndex].Sort;
            }
            else{
                if (nIndex == nLength - 1)return;
                tempIndex = nIndex + 1;
                model.other_id = allData.rows[tempIndex].AutoId;
                model.sort = allData.rows[tempIndex].Sort;
            }

            $.ajax({
                type: "Post",
                url: handlerUrl + "setsort.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $('#grvData').datagrid('reload');
                        $.messager.show({
                            title: '系统提示',
                            msg: resp.msg
                        });
                        selectCurIndex = tempIndex;
                    }
                    else {
                        $.messager.alert('系统提示', resp.msg);
                    }
                }
            });
        }
        function Delete() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var idList = [];
            for (var i = 0; i < rows.length; i++) {
                idList.push(rows[i].AutoId);
            }
            $.messager.confirm('系统提示', '确定删除所选？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "delete.ashx",
                        data: { ids: idList.join(',') },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                $('#grvData').datagrid('reload');
                                $.messager.show({
                                    title: '系统提示',
                                    msg: resp.msg
                                });
                            }
                            else {
                                $.messager.alert('系统提示', resp.msg);
                            }
                        }
                    });
                }
            });
        }
        function ShowSetField(trId, dlgTitle) {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            curSetField = trId;
            $("#rdoFieldIsNotNull")[0].checked = true;
            $("#rdoFieldIsShowInList1")[0].checked = true;
            $("#rdoFieldIsReadOnly1")[0].checked = true;
            $('.trSetField').hide();
            $('#' + curSetField).show();
            $('#dlgSetField').dialog({ title: dlgTitle });
            $("#dlgSetField").dialog('open');
        }
        function SetField() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var idList = [];
            for (var i = 0; i < rows.length; i++) {
                idList.push(rows[i].AutoId);
            }
            var model = { ids: idList.join(',') };
            if (curSetField == "trSetFieldIsNotNull"){
                model.field = "FieldIsNull";
                model.value = rdoFieldIsNull.checked ?0:1;
            }
            else if (curSetField == "trSetFieldIsShowInList") {
                model.field = "IsShowInList";
                model.value = rdoFieldIsShowInList2.checked ?0:1;
            }
            else if(curSetField == "trSetFieldIsReadOnly"){
                model.field = "IsReadOnly";
                model.value = rdoFieldIsReadOnly2.checked ?1:0;
            }
            else{
                $.messager.alert('系统提示', '该类型不支持');
                return;
            }
            console.log(curSetField);
            console.log(model);
            $.ajax({
                type: "Post",
                url: handlerUrl + "SetField.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $('#grvData').datagrid('reload');
                        $("#dlgSetField").dialog('close');
                        $.messager.show({
                            title: '系统提示',
                            msg: resp.msg
                        });
                    }
                    else {
                        $.messager.alert('系统提示', resp.msg);
                    }
                }
            });
        }
    </script>
</asp:Content>
