<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SetTypeConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Outlets.Comm.SetTypeConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .panel-icon, .tabs-icon, .tree-icon {
            background: none;
            width: 18px;
            line-height: 18px;
            display: inline-block;
        }

        .tabSetMain input[type=text] {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    类型设置
	<a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'" style="margin-left: 20px; text-indent: 0px;" onclick="SaveSet()">提交</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="tabSetMain" class="easyui-tabs" data-options="toolPosition:'left',plain:true,fit:true" style="min-height: 520px;">
        <div title="基础设置" data-options="fit:true" style="padding: 20px;">
            <table>
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">类型：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtType" class="commonTxt" placeholder="类型(必填)" <%= !string.IsNullOrWhiteSpace(nCategoryTypeConfig.CategoryType)?"readonly=\"readonly\"":"" %> value="<%=nCategoryTypeConfig.CategoryType %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">类型名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtDispalyName" class="commonTxt" placeholder="类型名称(必填)" value="<%=nCategoryTypeConfig.CategoryTypeDispalyName %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">客户端地图：
                    </td>
                    <td width="*" align="left">
                        <input id="rdoTimeSetMethod0" name="TimeSetMethod" class="positionTop2" type="radio" data-value="0" <%=nCategoryTypeConfig.TimeSetMethod==0?"checked='checked'":"" %> /><label for="rdoTimeSetMethod0">无地图</label>
                        <input id="rdoTimeSetMethod1" name="TimeSetMethod" class="positionTop2" type="radio" data-value="1" <%=nCategoryTypeConfig.TimeSetMethod==1?"checked='checked'":"" %> /><label for="rdoTimeSetMethod1">详情跳转</label>
                        <input id="rdoTimeSetMethod2" name="TimeSetMethod" class="positionTop2" type="radio" data-value="2" <%=nCategoryTypeConfig.TimeSetMethod==2?"checked='checked'":"" %> /><label for="rdoTimeSetMethod2">列表直达</label>
                    </td>
                </tr>
                <%--<tr>
                    <td align="right" class="tdTitle">客户端页面地址：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAppPagePath" class="commonTxt" placeholder="客户端页面地址(必填)" value="<%=nCategoryTypeConfig.AppPagePath %>" />
                    </td>
                </tr>--%>
                <tr>
                    <td align="right" class="tdTitle">分享标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShareTitle" class="commonTxt" placeholder="分享标题(选填)" value="<%=nCategoryTypeConfig.ShareTitle %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分享图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" class="rounded" />
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" style="display: none;" />
                        <input type="text" id="txtShareImg" class="commonTxt" placeholder="分享图片" value="<%=nCategoryTypeConfig.ShareImg %>" />
                        <br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img alt="提示" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为140*140。
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分享描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShareDesc" class="commonTxt" placeholder="分享描述(选填)" value="<%=nCategoryTypeConfig.ShareDesc %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分享链接：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShareLink" class="commonTxt" placeholder="分享链接(选填)" value="<%=nCategoryTypeConfig.ShareLink %>" />
                    </td>
                </tr>
            </table>
        </div>
        <div title="后台编辑页" style="padding: 20px;">
            <table style="width: 100%;">
                <tr>
                    <td align="right" class="tdTitle" style="width: 100px;">字段：
                    </td>
                    <td width="*" align="left">
                        <div id="divToolbar" style="padding: 5px; height: auto;">
                            <div style="margin-bottom: 5px">
                                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd()">添加字段</a>
                                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit()">编辑字段</a>
                                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除字段</a>
                                <a href="javascript:void(0)" title="上移" class="easyui-linkbutton" plain="true" onclick="SetFieldSort(0)">
                                    <img src="/MainStyle/Res/easyui/themes/icons/up.png" />上移</a>
                                <a href="javascript:void(0)" title="下移" class="easyui-linkbutton" plain="true" onclick="SetFieldSort(1)">
                                    <img src="/MainStyle/Res/easyui/themes/icons/down.png" />下移</a>
                                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-list" plain="true" onclick="ShowSetField('trSetFieldIsNotNull','批量设置是否必填')">批量设置是否必填</a>
                            </div>
                        </div>
                        <table id="bgEditData">
                            <thead>
                                <tr>
                                    <th field="ck" width="5" checkbox="true"></th>
                                    <th field="Field" width="20" fitcolumns="true" formatter="FormatterTitle">字段</th>
                                    <th field="MappingName" width="20" formatter="FormatterTitle">名称</th>
                                    <th field="FieldType" width="10" formatter="formartfieldtype">格式类型</th>
                                    <th field="FieldIsNull" width="10" formatter="changeisnull">必填项</th>
                                    <th field="IsReadOnly" formatter="formartReadOnly" width="10">是否只读</th>
                                    <th field="FormatValiFunc" width="10" formatter="formartvalitype">验证格式</th>
                                </tr>
                            </thead>
                        </table>
                    </td>
                </tr>
            </table>
            <div id="dlgEditField" class="easyui-dialog" title="" style="padding: 10px;" data-options="width:400,closed:true,modal:true">
                <table style="margin: auto; width: 100%;">
                    <tr>
                        <td align="right" style="width: 70px;">Field:
                        </td>
                        <td style="text-align: left">
                            <select id="ddlField" style="width: 270px;">
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
                                missingmessage="请输入名称" invalidmessage="请输入名称" style="width: 270px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">是否必填:
                        </td>
                        <td style="text-align: left">
                            <input type="checkbox" id="rdoIsNull" class="positionTop2" checked="checked" value="1" /><label for='rdoIsNull'>必填</label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">输入类型:
                        </td>
                        <td style="text-align: left">
                            <select id="ddlFieldtype" style="width: 270px;">
                                <option value="">单行文本</option>
                                <option value="mult">多行文本框</option>
                                <option value="select">下拉框</option>
                                <option value="radio">单选框</option>
                                <option value="checkbox">多选框</option>
                                <option value="img">图片</option>
                            </select>
                        </td>
                    </tr>
                    <tr id="divOptions">
                        <td align="right">选项:
                        </td>
                        <td style="text-align: left">
                            <textarea rows="5" id="txtOptions" style="width: 270px;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">格式验证:
                        </td>
                        <td style="text-align: left">
                            <select id="ddlFormatValiFunc" style="width: 270px;">
                                <option value="">无</option>
                                <option value="number">数字</option>
                                <option value="phone">手机</option>
                                <option value="email">电子邮箱</option>
                                <option value="url">网址</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>可否编辑:
                        </td>
                        <td>
                            <input type="checkbox" id="rdoIsReadOnly" class="positionTop2" value="1" /><label for='rdoIsReadOnly'>禁止编辑(只读)</label>
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
                            <input type="checkbox" id="rdoFieldIsNull" class="positionTop2" value="1" /><label for='rdoFieldIsNull'>必填</label>
                        </td>
                    </tr>
                    <tr id="trSetFieldIsReadOnly" class="trSetField">
                        <td>可否编辑:
                        </td>
                        <td>
                            <input type="checkbox" id="rdoFieldIsReadOnly" class="positionTop2" value="1" /><label for='rdoFieldIsReadOnly'>禁止编辑(只读)</label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div title="后台列表页" style="padding: 20px;">
            <h3>查询条件</h3>
            <table>
                <tr>
                    <td align="right" class="tdTitle" style="width:100px;">
                        选项条件：
                    </td>
                    <td width="*" align="left">
                        <div id="divSearchOption" class="chkFields"></div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">字段条件：
                    </td>
                    <td width="*" align="left">
                        <div id="divSearchField" class="chkFields"></div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">关键字条件：
                    </td>
                    <td width="*" align="left">
                        <div id="divSearchKeyword" class="chkFields"></div>
                    </td>
                </tr>
            </table>
            <h3>列表字段</h3>
            <table>
                <tr>
                    <td align="right" class="tdTitle" style="width:100px;">列表字段：
                    </td>
                    <td width="*" align="left">
                        <div id="divListField" class="chkFields"></div>
                    </td>
                </tr>
            </table>
        </div>
        <div title="前端页面" style="padding: 20px;">
            <h3>查询条件</h3>
            <table>
                <tr>
                    <td align="right" class="tdTitle" style="width:100px;">
                        选项条件：
                    </td>
                    <td width="*" align="left">
                        <div id="divAppSearchOption" class="chkFields"></div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">关键字条件：
                    </td>
                    <td width="*" align="left">
                        <div id="divAppSearchKeyword" class="chkFields"></div>
                    </td>
                </tr>
            </table>
            <h3>列表字段</h3>
            <table>
                <tr>
                    <td align="right" class="tdTitle" style="width:100px;">列表字段：
                    </td>
                    <td width="*" align="left">
                        <div id="divAppListField" class="chkFields"></div>
                    </td>
                </tr>
            </table>
            <h3>详情字段</h3>
            <table>
                <tr>
                    <td align="right" class="tdTitle" style="width:100px;">详情字段：
                    </td>
                    <td width="*" align="left">
                        <div id="divAppDetailField" class="chkFields"></div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/tablefieldmap/';
        var handlerConfigUrl = '/serv/api/admin/outlets/comm/';
        var table_name = 'ZCJ_JuActivityInfo';
        var mapping_type = '0';
        var cur_AutoId = 0;
        var selectCurIndex = -1;
        var curSetField = '';
        var type = '<%= Request["type"] %>';

        var editFields = [];
        var searchOptions = '<% = nCategoryTypeConfig.ListFields %>';
        var searchFields = '<% = nCategoryTypeConfig.EditFields %>';
        var searcKeyword = '<% = nCategoryTypeConfig.NeedFields %>';
        var searchAppOptions = '<%= nCategoryTypeConfig.Ex1 %>';
        var searcAppKeyword = '<%= nCategoryTypeConfig.Ex2 %>';
        var listFields = '<%= nCategoryTypeConfig.Ex3 %>';
        var listAppFields = '<%= nCategoryTypeConfig.Ex4 %>';
        var detailAppFields = '<%= nCategoryTypeConfig.Ex5 %>';

        $(function () {
            checkImg();
            $('#bgEditData').datagrid({
                method: "Post",
                //url: handlerUrl + "list.ashx",
                //queryParams: { mapping_type: mapping_type, table_name: table_name },
                toolbar: '#divToolbar',
                striped: true,
                height: 430,
                fitColumns: true,
                //loadFilter: pagerFilter,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#bgEditData').datagrid('loaded');
                    if (selectCurIndex > -1) {
                        $('#bgEditData').datagrid('selectRow', selectCurIndex);
                        selectCurIndex = -1;
                    }
                },
                onBeforeLoad: function () {
                    //加载完数据关闭等待的div   
                    $('#bgEditData').datagrid('loading');
                }
            });
            GetOldData();
            $("#dlgEditField").dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        if (cur_AutoId == 0) {
                            Add();
                        }
                        else {
                            Edit();
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $("#dlgEditField").dialog('close');
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

            $('#ddlFieldtype').live('change', function () { changeFieldtype(); });

            $('#divSearchOption input[name="chkdivSearchOption"]').live('click', function () {
                searchOptions = getNewValue(this.checked, searchOptions, $.trim($(this).val()));
            });
            $('#divSearchField input[name="chkdivSearchField"]').live('click', function () {
                searchFields = getNewValue(this.checked, searchFields, $.trim($(this).val()));
            });
            $('#divSearchKeyword input[name="chkdivSearchKeyword"]').live('click', function () {
                searcKeyword = getNewValue(this.checked, searcKeyword, $.trim($(this).val()));
            });
            $('#divAppSearchOption input[name="chkdivAppSearchOption"]').live('click', function () {
                searchAppOptions = getNewValue(this.checked, searchAppOptions, $.trim($(this).val()));
            });
            $('#divAppSearchKeyword input[name="chkdivAppSearchKeyword"]').live('click', function () {
                searcAppKeyword = getNewValue(this.checked, searcAppKeyword, $.trim($(this).val()));
            });

            $('#divListField input[name="chkdivListField"]').live('click', function () {
                listFields = getNewValue(this.checked, listFields, $.trim($(this).val()));
            });
            $('#divAppListField input[name="chkdivAppListField"]').live('click', function () {
                listAppFields = getNewValue(this.checked, listFields, $.trim($(this).val()));
            });
            $('#divAppDetailField input[name="chkdivAppDetailField"]').live('click', function () {
                detailAppFields = getNewValue(this.checked, listFields, $.trim($(this).val()));
            });

            $("#txtShareImg").bind("change", function () {
                checkImg();
            });
            $('#txtThumbnailsPath').on('change', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });
                    $.ajaxFileUpload({
                        url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                        secureuri: false,
                        fileElementId: 'txtThumbnailsPath',
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                imgThumbnailsPath.src = resp.ExStr;
                                $("#txtShareImg").val(resp.ExStr);
                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }
            });
        });
        function getArryNewValue(nChecked, oldValue, nValue) {
            var temp = [];
            var hasOld = false;
            if (oldValue.length > 0) {
                for (var i = 0; i < oldValue.length; i++) {
                    if (oldValue[i] == nValue) {
                        hasOld = true;
                        if (nChecked) temp.push(nValue);
                    }
                    else {
                        temp.push(oldValue[i]);
                    }
                }
            }
            if (!hasOld && nChecked) temp.push(nValue);
            return temp;
        }
        function getNewValue(nChecked, oldValue, nValue) {
            var temp = [];
            var hasOld = false;
            if (oldValue != '') {
                var tempList = oldValue.split(',');
                for (var i = 0; i < tempList.length; i++) {
                    if (tempList[i] == nValue) {
                        hasOld = true;
                        if (nChecked) temp.push(nValue);
                    }
                    else {
                        temp.push(tempList[i]);
                    }
                }
            }
            if (!hasOld && nChecked) temp.push(nValue);
            return temp.join(',');
        }
        function GetOldData() {
            $.ajax({
                type: "Post",
                url: handlerUrl + "list.ashx",
                data: { table_name: table_name, mapping_type: mapping_type,foreignkey_id: type },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        editFields = [];
                        for (var i = 0; i < resp.result.list.length; i++) {
                            editFields.push(resp.result.list[i]);
                        }
                        loadGrid();
                    }
                }
            });
        }

        function loadGrid() {
            $('#bgEditData').datagrid('loadData', editFields);
            if (editFields.length > 0) {
                $(".chkFields").html("");
                $(".chkFields").each(function () {
                    var nid = $(this).attr("id");
                    var appendhtml = new StringBuilder();
                    appendhtml.AppendFormat('<ul>');
                    for (var j = 0; j < editFields.length; j++) {
                        if ((nid == 'divSearchOption' || nid == 'divAppSearchOption') && $.trim(editFields[j].Options) == "") continue;
                        appendhtml.AppendFormat('<li style="float:left;">');
                        appendhtml.AppendFormat('<input type="checkbox" name="{0}" id="{1}" class="positionTop2 hand" value="{3}" /><label class="hand" for="{1}">{2}</label>'
                                , 'chk' + nid, 'chk' + nid + j, editFields[j].MappingName, editFields[j].Field);
                        appendhtml.AppendFormat('</li>');

                    }
                    appendhtml.AppendFormat('</ul>');
                    $(this).append(appendhtml.ToString());
                });
                $.each(listFields.split(','), function () {
                    $('#divListField').find('input[name="chkdivListField"][value="' + this + '"]').attr('checked', true);
                });
                $.each(listAppFields.split(','), function () {
                    $('#divAppListField').find('input[name="chkdivAppListField"][value="' + this + '"]').attr('checked', true);
                });
                $.each(detailAppFields.split(','), function () {
                    $('#divAppDetailField').find('input[name="chkdivAppDetailField"][value="' + this + '"]').attr('checked', true);
                });
                $.each(searchOptions.split(','), function () {
                    $('#divSearchOption').find('input[name="chkdivSearchOption"][value="' + this + '"]').attr('checked', true);
                });
                $.each(searchFields.split(','), function () {
                    $('#divSearchField').find('input[name="chkdivSearchField"][value="' + this + '"]').attr('checked', true);
                });
                $.each(searcKeyword.split(','), function () {
                    $('#divSearchKeyword').find('input[name="chkdivSearchKeyword"][value="' + this + '"]').attr('checked', true);
                });
                $.each(searchAppOptions.split(','), function () {
                    $('#divAppSearchOption').find('input[name="chkdivAppSearchOption"][value="' + this + '"]').attr('checked', true);
                });
                $.each(searcAppKeyword.split(','), function () {
                    $('#divAppSearchKeyword').find('input[name="chkdivAppSearchKeyword"][value="' + this + '"]').attr('checked', true);
                });
            }
        }
        function checkImg() {
            var sImg = $.trim($("#txtShareImg").val());
            if (sImg != "") imgThumbnailsPath.src = sImg;
        }
        function changeFieldtype() {
            var field_type = $.trim($('#ddlFieldtype').val());
            if (field_type == "select" || field_type == "radio" || field_type == "checkbox") {
                $("#divOptions").show();
            }
            else {
                $("#divOptions").hide();
            }
        }
        function changeisnull(value, rowData, rowIndex) {
            if (value == 1) return "必填";
            return "";
        }
        function formartfieldtype(value, rowData, rowIndex) {
            if (value == "mult") return "多行文本框";
            if (value == "select") return "下拉框";
            if (value == "radio") return "单选框";
            if (value == "checkbox") return "多选框";
            if (value == "img") return "图片";
            return "单行文本框";
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
        function formartReadOnly(value, rowData, rowIndex) {
            if (value == 0) return "可编辑";
            return '<span style="color:red;">只读</span>';
        }
        function GetModel() {
            var model = {
                AutoId: cur_AutoId,
                MappingName: $.trim($("#txtMappingName").val()),
                FieldType: $.trim($("#ddlFieldtype").val()),
                FormatValiFunc: $.trim($("#ddlFormatValiFunc").val()),
                Options: $.trim($("#txtOptions").val()),
                FieldIsNull: rdoIsNull.checked ? 1 : 0,
                IsReadOnly: rdoIsReadOnly.checked ? 1 : 0,
                ForeignkeyId: type,
                TableName: table_name,
                MappingType: mapping_type
            }
            if (model.AutoId == 0) model.Field = $.trim($("#ddlField").val());
            return model;
        }
        function CheckModel(model) {
            if (model != 0 && model.Field == "") {
                $.messager.alert('系统提示', "请选择字段");
                return false;
            }
            if (model.MappingName == "") {
                $.messager.alert('系统提示', "请输入名称");
                return false;
            }
            return true;
        }
        function Add() {
            var model = GetModel();
            if (!CheckModel(model)) return;
            $.ajax({
                type: "Post",
                url: handlerUrl + "add.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        GetOldData();
                        $("#dlgEditField").dialog('close');
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
            if (!CheckModel(model)) return;
            $.ajax({
                type: "Post",
                url: handlerUrl + "update.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        GetOldData();
                        $("#dlgEditField").dialog('close');
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
            var rows = $("#bgEditData").datagrid('getSelections');//获取选中的行
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            cur_AutoId = rows[0].AutoId;
            $("#ddlField").val(rows[0].Field);
            $("#ddlField")[0].disabled = true;
            $("#txtMappingName").val(rows[0].MappingName);
            $("#ddlFieldtype").val(rows[0].FieldType);
            $("#ddlFormatValiFunc").val(rows[0].FormatValiFunc);
            $("#txtOptions").val(rows[0].Options);
            rdoIsNull.checked = rows[0].FieldIsNull == 1;
            rdoIsReadOnly = rows[0].IsReadOnly == 1;
            changeFieldtype();
            $("#dlgEditField").dialog({ title: '编辑字段' });
            $("#dlgEditField").dialog('open');
        }
        function ShowAdd() {
            $("#ddlField")[0].disabled = false;
            ClearWinData();
            cur_AutoId = 0;
            $("#dlgEditField").dialog({ title: '新增字段' });
            $("#dlgEditField").dialog('open');
        }
        //窗体清除数据
        function ClearWinData() {
            $("#ddlField").val("");
            $("#txtMappingName").val("");
            rdoIsNull.checked = true;
            $("#ddlFieldtype").val("");
            $("#ddlFormatValiFunc").val("");
            $("#txtOptions").val("");
            $("#divOptions").hide();
            rdoIsReadOnly = true;
        }
        function SetFieldSort(num) {
            var rows = $("#bgEditData").datagrid('getSelections');//获取选中的行
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            var nIndex = $("#bgEditData").datagrid('getRowIndex', rows[0]);
            var allData = $("#bgEditData").datagrid('getData');
            var nLength = allData.rows.length;
            var model = {
                id: rows[0].AutoId,
                other_sort: rows[0].Sort
            };
            var tempIndex = 0;
            if (num == 0) {
                if (nIndex == 0) return;
                tempIndex = nIndex - 1;
                model.other_id = allData.rows[tempIndex].AutoId;
                model.sort = allData.rows[tempIndex].Sort;
            }
            else {
                if (nIndex == nLength - 1) return;
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
                        GetOldData();
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
            var rows = $("#bgEditData").datagrid('getSelections');//获取选中的行
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
                                GetOldData();
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
            var rows = $("#bgEditData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            curSetField = trId;
            rdoFieldIsNull.checked = rows[0].FieldIsNull == 1;
            rdoFieldIsReadOnly.checked = rows[0].IsReadOnly == 1;
            $('.trSetField').hide();
            $('#' + curSetField).show();
            $('#dlgSetField').dialog({ title: dlgTitle });
            $("#dlgSetField").dialog('open');
        }
        function SetField() {
            var rows = $("#bgEditData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var idList = [];
            for (var i = 0; i < rows.length; i++) {
                idList.push(rows[i].AutoId);
            }
            var model = { ids: idList.join(',') };
            if (curSetField == "trSetFieldIsNotNull") {
                model.field = "FieldIsNull";
                model.value = rdoFieldIsNull.checked ? 1 : 0;
            }
            else if (curSetField == "trSetFieldIsReadOnly") {
                model.field = "IsReadOnly";
                model.value = rdoFieldIsReadOnly.checked ? 1 : 1;
            }
            else {
                $.messager.alert('系统提示', '该类型不支持');
                return;
            }
            $.ajax({
                type: "Post",
                url: handlerUrl + "SetField.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        GetOldData();
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
        function GetCheckedValue(objId) {
            var temp = [];
            $('#' + objId).find('input[name="chk' + objId + '"]:checked').each(function () {
                var val = $.trim($(this).val());
                if (val!='') temp.push($.trim($(this).val()));
            });
            return temp.join(',');
        }
        function GetConfigModel() {

            var model = {
                field: $.trim($("#txtType").val()),
                name: $.trim($("#txtDispalyName").val()),
                map_show: $.trim($('input[name="TimeSetMethod"]:checked').attr('data-value')),
                //app_page_path: $.trim($("#txtAppPagePath").val()),
                share_title: $.trim($("#txtShareTitle").val()),
                share_img: $.trim($("#txtShareImg").val()),
                share_desc: $.trim($("#txtShareDesc").val()),
                share_link: $.trim($("#txtShareLink").val()),
                search_options: GetCheckedValue('divSearchOption'),
                search_fields: GetCheckedValue('divSearchField'),
                searc_keyword: GetCheckedValue('divSearchKeyword'),
                search_app_options: GetCheckedValue('divAppSearchOption'),
                searc_app_keyword: GetCheckedValue('divAppSearchKeyword'),
                list_fields: GetCheckedValue('divListField'),
                list_app_fields: GetCheckedValue('divAppListField'),
                list_detail_fields: GetCheckedValue('divAppDetailField')
            }
            return model;
        }
        function CheckConfigModel(model) {
            if (model != 0 && model.field == "") {
                $.messager.alert('系统提示', "字段名不能为空");
                return false;
            }
            if (model.name == "") {
                $.messager.alert('系统提示', "请输入类型名称");
                return false;
            }
            if (model.app_page_path == "") {
                $.messager.alert('系统提示', "请输入客户端页面地址");
                return false;
            }
            return true;
        }
        function SaveSet() {
            var model = GetConfigModel();
            if (!CheckConfigModel(model)) return;
            $.ajax({
                type: "Post",
                url: handlerConfigUrl + "set.ashx",
                data: model,
                dataType: "json",
                success: function (resp) {
                    $.messager.alert('系统提示', resp.msg);
                }
            });
        }
    </script>
</asp:Content>
