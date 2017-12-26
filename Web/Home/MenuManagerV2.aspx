<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="MenuManagerV2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Home.MenuManagerV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>菜单管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="list_data" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="NodeName" width="20">节点名称</th>
                <th field="TargetBlank" width="10" formatter="FormatTargetBlank">新页显示</th>
                <th field="IsHide" width="10" formatter="FormatHide">显示隐藏</th>
                <th field="ShowLevel" width="10" formatter="FormatShowLevel">显示级别</th>
                <th field="MenuSort" width="3" formatter="FormatterTitle">同级排序</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">

            <%if (isAddMenu)
              {%>

            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                title="添加菜单" onclick="ShowAdd()" id="btnAdd" runat="server">添加菜单</a>

            <%} %>

            <%if (isEditMenu)
              {%>

            <a href="javascript:;"
                class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑菜单" onclick="ShowEdit()"
                id="btnEdit" runat="server">编辑菜单 </a>
            <%} %>

            <%if (isDeleteMenu)
              {%>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-remove" plain="true" title="删除菜单" onclick="Delete()" id="btnDelete"
                runat="server">删除菜单 </a>
            <%} %>

            <br />
            <br />
            <span>注意：操作菜单数据可能需要必要的开发支持，请慎重操作</span>
            <br />
        </div>
    </div>
    <div id="dlgMenuInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 500px; padding: 15px">
        <table style="width:100%;">
            <tr>
                <td height="25" width="100" align="left">所属菜单：
                </td>
                <td height="25" width="*" align="left">
                    <span id="sp_menu" style="width: 90%;"></span>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">节点名称<span style="color:red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtNodeName" style="width: 90%;" class="easyui-validatebox"
                        required="true" missingmessage="请输入节点名称" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">链接：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtUrl" style="width: 90%;" />
                </td>
            </tr>

            <tr>
                <td height="25" align="left">图标样式：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtICOCSS" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td height="25" align="left">同级排序<span style="color:red;">*</span>：
                </td>
                <td height="25" width="*" align="left">
                    <input type="text" id="txtMenuSort" style="width: 90%;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"
                         class="easyui-validatebox" required="true" missingmessage="请输入排序"/>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">是否新标签显示：
                </td>
                <td height="25" width="*" align="left">
                    <input type="radio" id="rdtb0" checked="checked" name="rdo" class="positionTop2" /><label for='rdtb0'>本页</label>
                    <input type="radio" id="rdtb1" name="rdo" class="positionTop2" /><label for='rdtb1'>新页</label>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">是否显示菜单：
                </td>
                <td height="25" width="*" align="left">
                    <input type="radio" id="rd0" checked="checked" name="rdotb" class="positionTop2" /><label for='rd0'>显示</label>
                    <input type="radio" id="rd1" name="rdotb" class="positionTop2" /><label for='rd1'>隐藏</label>
                </td>
            </tr>
            <tr>
                <td height="25" align="left">显示级别：
                </td>
                <td height="25" width="*" align="left">
                    <% if(CurrentUserInfo!=null && CurrentUserInfo.UserType==1) {%>
                    <input type="radio" id="rdLevel1" name="rdoShowLevel" class="positionTop2" /><label for='rdLevel1'>超级管理员可见</label>
                    <%} %>
                    <input type="radio" id="rdLevel2" name="rdoShowLevel" class="positionTop2" /><label for='rdLevel2'>站点管理员可见</label>
                    <input type="radio" id="rdLevel3" name="rdoShowLevel" class="positionTop2" checked="checked" /><label for='rdLevel3'>站点子帐号可见</label>
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = '/Handler/Permission/MenuManager.ashx';
        var grid;
        var currSelectID = 0;

        $(function () {


            //$(window).resize(function () {
            //    $(list_data).datagrid('resize',
	        //    {
	        //        width: document.body.clientWidth,
	        //        height: document.documentElement.clientHeight
	        //    });
            //});
            //-----------------加载gridview
            grid = jQuery("#list_data").datagrid({
                method: "Post",
                url: handlerUrl,
                height: document.documentElement.clientHeight - 110,
                toolbar: '#divToolbar',
                pagination: false,
                rownumbers: true,
                //pageSize: 20,
                //singleSelect: true,
                //loadFilter: pagerFilter,
                queryParams: { Action: "Query", menuType: 1, showLevel: 1, showHide: 1 }
            });
            //------------加载gridview

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

            //查询按钮点击绑定
            $("#btnSearch").click(function () {
                var searchReq = $.trim($(txtSearchUserID).val());
                grid.datagrid({ url: handlerUrl, queryParams: { Action: "Query", menuType: 1, showLevel: 1, showHide: 1 } });

            });

            //加载菜单
            LoadMenuSelectList();

        });

        function LoadData() {
            grid.datagrid({ url: handlerUrl, queryParams: { Action: "Query", menuType: 1, showLevel: 1, showHide: 1 } });
        }

        function pagerFilter(data) {
            if (typeof data.length == 'number' && typeof data.splice == 'function') {	// is array
                data = {
                    total: data.length,
                    rows: data
                }
            }
            var dg = $(this);
            var opts = dg.datagrid('options');
            var pager = dg.datagrid('getPager');
            pager.pagination({
                onSelectPage: function (pageNum, pageSize) {
                    opts.pageNumber = pageNum;
                    opts.pageSize = pageSize;
                    pager.pagination('refresh', {
                        pageNumber: pageNum,
                        pageSize: pageSize
                    });
                    dg.datagrid('loadData', data);
                }
            });
            if (!data.originalRows) {
                data.originalRows = (data.rows);
            }
            var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
            var end = start + parseInt(opts.pageSize);
            data.rows = (data.originalRows.slice(start, end));
            return data;
        }

        function ShowAdd() {
            ClearWinDataByTag('input', dlgMenuInfo);
            $("#rd0").attr("checked", true);
            $("#rdtb0").attr("checked", true);
            $("#rdLevel3").attr("checked", true);
            //加载菜单
            LoadMenuSelectList();
            $('#dlgMenuInfo').window(
            {
                title: '添加菜单'
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
                title: '编辑菜单'
            }
            );

            $('#dlgMenuInfo').dialog('open');
            $(btnSave).attr('tag', 'edit');

            //加载编辑数据
            currSelectID = rows[0].MenuID;
            $('#ddlPreMenu').val(rows[0].PreID);
            $(txtNodeName).val($.trim(rows[0].NodeName).replace('└', ''));
            $(txtUrl).val(rows[0].Url);
            $(txtICOCSS).val(rows[0].ICOCSS);
            $(txtMenuSort).val(rows[0].MenuSort);
            var ishide = rows[0].IsHide;
            if (ishide == "1") {
                $("#rd1").attr("checked", true);
            } else {
                $("#rd0").attr("checked", true);
            }
            var targetblank = rows[0].TargetBlank;
            if (targetblank == "1") {
                $("#rdtb1").attr("checked", true);
            } else {
                $("#rdtb0").attr("checked", true);
            }
            
            var showLevel = rows[0].ShowLevel;
            if (showLevel == "1") {
                if (rdLevel1) $("#rdLevel1").attr("checked", true);
            } else if(showLevel == "2"){
                $("#rdLevel2").attr("checked", true);
            } else {
                $("#rdLevel3").attr("checked", true);
            }
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
                            $("#dlgMenuInfo").window("close");
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
                            $("#dlgMenuInfo").window("close");
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
                        ids.push(rows[i].MenuID);
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
            var showLevel = "";
            if (rdLevel1 && rdLevel1.checked) {
                showLevel = "1";
            }
            else if (rdLevel2.checked) {
                showLevel = "2";
            }
            else if (rdLevel3.checked) {
                showLevel = "3";
            }

            var model =
            {
                "MenuID": currSelectID,
                "NodeName": $.trim($(txtNodeName).val()),
                "Url": $.trim($(txtUrl).val()),
                "PreID": $('#ddlPreMenu').val(),
                "ICOCSS": $.trim($(txtICOCSS).val()),
                "MenuSort": $.trim($(txtMenuSort).val()),
                "IsHide": rd0.checked ? 0 : 1,
                "TargetBlank": rdtb0.checked ? 0 : 1,
                "MenuType": 1
            }
            if(showLevel!="") model["ShowLevel"] = showLevel;
            return model;
        }

        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['NodeName'] == '') {
                $(txtNodeName).val("");
                $(txtNodeName).focus();
                return false;
            }
            if (model["MenuSort"] == '') {
                $(txtMenuSort).val("");
                $(txtMenuSort).focus();
                return false;
            }
            return true;
        }

        //加载菜单选择列表
        function LoadMenuSelectList() {
            $.post(handlerUrl, { Action: "GetMenuSelectList", menuType: 1, showPreMenu: 1, showLevel: 1 , showHide:1}, function (data) {
                $("#sp_menu").html(data);
            });
        }
        function FormatHide(_Num) {
            var str = new StringBuilder();
            if (_Num == 1) {
                str.AppendFormat('<span style="color:red;">隐藏</span>');
            }
            else {
                str.AppendFormat('显示');
            }
            return str.ToString();
        }
        function FormatTargetBlank(_Num) {
            var str = new StringBuilder();
            if (_Num == 1) {
                str.AppendFormat('<span style="color:red;">新页</span>');
            }
            else {
                str.AppendFormat('本页');
            }
            return str.ToString();
        }
        
        function FormatShowLevel(_Num) {
            var str = new StringBuilder();
            if (_Num == 1) {
                str.AppendFormat('<span style="color:red;">超级管理员可见</span>');
            }
            else if (_Num == 2) {
                str.AppendFormat('<span style="color:blue;">站点管理员可见</span>');
            }
            else {
                str.AppendFormat('所有帐号可见');
            }
            return str.ToString();
        }
    </script>
</asp:Content>
