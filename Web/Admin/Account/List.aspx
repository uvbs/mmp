<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Account.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;账户管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="avatar" width="50" formatter="FormartAvatar">头像</th>
                <th field="username" width="100">账户</th>
                <th field="truename" width="50">姓名</th>
                <th field="company" width="100">公司</th>
                <th field="postion" width="100">职位</th>
                 <th field="phone" width="100">联系手机</th>
                <th field="email" width="100">邮箱</th>
                <th field="role_name" width="100">角色</th>
                <th field="is_disable" width="50" formatter="FormartIsDisable">状态</th>
                <th field="password" width="50" formatter="FormatPassword">改密码</th>
                <th field="edit" width="50" formatter="FormatRole">编辑</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                title="新增账户" onclick="ShowAdd()" id="btnAdd" runat="server">新增账户</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="DisableUser(1)">批量禁用账户</a> <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="DisableUser(0)">批量启用账户</a>
            <br />
            姓名:<input type="text" id="txtTrueNames" style="width: 200px" />&nbsp; 
            <input type="radio" name="rdo" id="start" checked="checked" value="0" class="positionTop2" /><label for="start">启用</label>
            <input type="radio" name="rdo" id="stop" class="positionTop2" value="1"/><label for="stop">禁用</label>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>头像
                </td>
                <td>
                    <img alt="缩略图" src="http://open-files.comeoncloud.net/img/ico/def_user.png" width="80px" height="80px" id="imghead" /><br />
                    <a id="auploadThumbnails"
                        href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                        onclick="fileheadimg.click()">上传</a>
                    <input type="file" id="fileheadimg" name="file1" style="display: none;" />

                </td>
            </tr>
            <tr class="accountUserID">
                <td>账户名
                </td>
                <td>
                    <input id="txtUserID" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr class="accountPassword">
                <td>登录密码
                </td>
                <td>
                    <input id="txtPwd" type="password" style="width: 90%;" />
                </td>
            </tr>
            <tr class="accountConfrimPassword">
                <td>确认密码
                </td>
                <td>
                    <input id="txtConfrimPwd" type="password" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>角色
                </td>
                <td>
                    <span id="sp_menu"></span>
                </td>
            </tr>
            <tr>
                <td>真实姓名
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>公司名称
                </td>
                <td>
                    <input id="txtCompany" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>职位
                </td>
                <td>
                    <input id="txtPostion" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>联系手机
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 90%;" />
                </td>
            </tr>
            <tr>
                <td>邮箱
                </td>
                <td>
                    <input id="txtEmail" type="text" style="width: 90%;" />
                </td>
            </tr>
        </table>
         <%--<input id="txtPhone" type="text" style="width: 90%; display:none;" />--%>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/account/';
        var handlerPermissionUrl = '/serv/api/admin/permissioncolumn/';
        var handlerCationUrl = "/Handler/App/CationHandler.ashx";
        
        var grid;
        var currSelectID = 0;
        var Action = "";
        var isDisable = 0;
        var keyWord = "";
        $(function () {
            LoadSelect();
            //加载datagrid
            grid = $("#grvData").datagrid({
                method: "Post",
                url: handlerUrl + "list.ashx",
                queryParams: { is_disable: isDisable,true_name:keyWord },
                height: document.documentElement.clientHeight - 110,
                pagination: true,
                pageSize: 20,
                toolbar: '#divToolbar',
                rownumbers: true,
                singleSelect: false
            });
        
            var dlgInputLock = false;
            $('#dlgInput').dialog({
                buttons: [{
                    text: "确定",
                    handler: function () {
                        if (dlgInputLock) {
                            return;
                        }
                        dlgInputLock = true;
                        var model = {};
                        var actionStr = "";
                        if (Action == "add") {
                            model = GetDlgModel();
                            actionStr = "add.ashx";
                            model.UserID = $.trim($("#txtUserID").val());
                            model.Password = $.trim($("#txtPwd").val());
                        }
                        else if (Action == "edit") {
                            model = GetDlgModel();
                            model.AutoID = currSelectID;
                            actionStr = "update.ashx";
                        }
                        else if (Action == "edit_password") {
                            model.AutoID = currSelectID;
                            model.Password = $.trim($("#txtPwd").val());
                            var cPassword = $.trim($("#txtConfrimPwd").val())
                            if (model.Password != cPassword) {
                                $(txtConfrimPwd).val("");
                                $(txtConfrimPwd).focus();
                                $.messager.alert("系统提示", "密码不一致");
                                dlgInputLock = false;
                                return false;
                            }
                            actionStr = "updatepassword.ashx";
                        }
                        if (!CheckDigModel(model)) {
                            dlgInputLock = false;
                            return;
                        }
                        if (actionStr == "") {
                            $.messager.alert("系统提示", "执行出错");
                            dlgInputLock = false;
                            return;
                        }
                        $.ajax({
                            type: "post",
                            url: handlerUrl + actionStr,
                            data: model,
                            success: function (result) {
                                if (result.status == true) {
                                    $.messager.show({ title: '系统提示', msg: '提交成功' });
                                    $(dlgInput).dialog('close');
                                    loadData();
                                } else {
                                    $.messager.alert("系统提示", result.msg);
                                }
                                dlgInputLock = false;
                            }
                        });
                    }
                }, {
                    text: "取消",
                    handler: function () {
                        $('#dlgInput').dialog('close');
                    }
                }]
            });
            $("#fileheadimg").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                        {
                            url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                            secureuri: false,
                            fileElementId: 'fileheadimg',
                            dataType: 'json',
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    $("#imghead").attr("src", resp.ExStr);

                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        }
                       );

                } catch (e) {
                    alert(e);
                }
            });
        });
        function loadData() {
            $('#grvData').datagrid({ url: handlerUrl + "list.ashx", queryParams: { is_disable: isDisable,true_name:keyWord } });
        }
        function Search() {
            keyWord = $("#txtTrueNames").val();
            isDisable = $("input[name=rdo]:checked").val();
            var gridOpts = $('#grvData').datagrid('options', { pageNumber: 1, is_disable: isDisable, true_name: keyWord });
            loadData();
        }
        function ShowAdd() {
            var groupLength = $("#ddlPermissionGroup").find("option").length;
            if (groupLength <= 1) {
                $.messager.alert('系统提示', "请先添加角色");
                return;
            }
            $("#dlgInput tr").show();
            ClearWinDataByTag('input', dlgInput);
            Action = 'add';
            $("#txtUserID").attr("disabled", false);
            //加载菜单
            $("#dlgInput").dialog("restore");
            $('#dlgInput').window({
                title: '新增账户'
            });

            $('#dlgInput').dialog('open');
        }

        function ShowEdit(n_index) {
            var rows = grid.datagrid('getData').rows;
            var row = rows[n_index];
            ClearWinDataByTag('input', dlgInput);
            //加载编辑数据
            currSelectID = row.id;
            $("#txtUserID").val(row.username);
            $("#txtTrueName").val(row.truename);
            $('#ddlPermissionGroup').val(row.pre_id);
            $("#txtCompany").val(row.company);
            $("#txtPostion").val(row.postion);
            $("#txtPhone").val(row.phone);
            $("#txtEmail").val(row.email);
            if (row.avatar != "") {
                $("#imghead").attr("src", row.avatar);
            }
            Action = 'edit';
            
            $("#txtUserID").attr("disabled", true);
            $("#dlgInput tr").show();
            $("#dlgInput .accountPassword").hide();
            $("#dlgInput .accountConfrimPassword").hide();
            //加载菜单
            $("#dlgInput").dialog("restore");
            $('#dlgInput').window(
            {
                title: '编辑账户'
            });

            $('#dlgInput').dialog('open');
        }
        function ShowEditPwd(n_index) {
            var rows = grid.datagrid('getData').rows;
            var row = rows[n_index];
            ClearWinDataByTag('input', dlgInput);
            //加载编辑数据
            currSelectID = row.id;
            $('#ddlPermissionGroup').val(0);
            $("#txtUserID").val(row.username);
            Action = 'edit_password';
            $("#txtUserID").attr("disabled", true);
            $("#dlgInput tr").hide();
            $("#dlgInput .accountUserID").show();
            $("#dlgInput .accountPassword").show();
            $("#dlgInput .accountConfrimPassword").show();
            //加载菜单
            $("#dlgInput").dialog("restore");
            $('#dlgInput').window(
            {
                title: '修改账户密码'
            });

            $('#dlgInput').dialog('open');
        }

        function GetDlgModel() {
            var model = {
                WXHeadimgurl: $("#imghead").attr("src"),
                PermissionGroupID: $.trim($("#ddlPermissionGroup").val()),
                TrueName: $.trim($("#txtTrueName").val()),
                Company: $.trim($("#txtCompany").val()),
                Postion: $.trim($("#txtPostion").val()),
                Phone: $.trim($("#txtPhone").val()),
                //Phone3: $.trim($("#txtPhone3").val()),
                Email: $.trim($("#txtEmail").val())
            }
            return model;
        }
        function CheckDigModel(model) {
            if (Action == "add") {
                if (model.UserID == "") {
                    $(txtUserID).val("");
                    $(txtUserID).focus();
                    $.messager.alert("系统提示", "账户不能为空");
                    return false;
                }
                if (model.Password == "") {
                    $(txtPwd).val("");
                    $(txtPwd).focus();
                    $.messager.alert("系统提示", "密码不能为空");
                    return false;
                }
                var cPassword = $.trim($("#txtConfrimPwd").val())
                if (model.Password != cPassword) {
                    $(txtConfrimPwd).val("");
                    $(txtConfrimPwd).focus();
                    $.messager.alert("系统提示", "密码不一致");
                    return false;
                }
                
            } else {
                if (model.AutoID == "") {
                    $.messager.alert("系统提示", "账户编号未找到");
                    return false;
                }
                if (model.UserID == "") {
                    $(txtUserID).val("");
                    $(txtUserID).focus();
                    $.messager.alert("系统提示", "账户不能为空");
                    return false;
                }
            }
            if (model.PermissionGroupID == "" || model.PermissionGroupID == "0") {
                $.messager.alert("系统提示", "请选择角色");
                return false;
            }
            return true;
        }
        
        function FormatPassword(value, rowData, index) {
            var str = new StringBuilder();
            str.AppendFormat('<a style="color:blue;" href="javascript:ShowEditPwd({0})">改密码</a>', index);
            return str.ToString();
        }
        function FormatRole(value, rowData, index) {
            var str = new StringBuilder();
            str.AppendFormat('<a style="color:green;" href="javascript:ShowEdit({0})">编辑</a>', index);
            return str.ToString();
        }
        function LoadSelect() {
            $.post(handlerPermissionUrl + "selectrolelist.ashx", {}, function (data) {
                if (data.status && data.result) {
                    $("#sp_menu").html(data.result);
                }
            });
        }
        //批量禁用
        function DisableUser(disableValue) {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }

            currSelectUsers = new Array();

            for (var i = 0; i < rows.length; i++) {
                currSelectUsers.push(rows[i]);
            }

            var ids = new Array();

            for (var i = 0; i < currSelectUsers.length; i++) {
                ids.push("'" + currSelectUsers[i].username + "'");
            }

            var modelData = {
                Action: 'DisableUser',
                userIds: ids.join(','),
                disableValue: disableValue
            }

            $.messager.confirm("系统提示", "确定" + (disableValue == 1 ? "禁用" : "启用") + "当前选择的用户？", function (r) {
                if (r) {
                    $.ajax({
                        type: 'post',
                        url: handlerCationUrl,
                        data: modelData,
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                loadData();
                            }
                            else {
                                $.messager.alert("系统提示", "操作失败");
                            }
                        }
                    });
                }
            });
        }
        function FormartIsDisable(value) {
            if (value == "0") {
                return "<font color='green'>启用</font>";
            }
            else {
                return "<font color='red'>禁用</font>"
            }
        }
        function FormartAvatar(value, row) {
            if (value == '' || value == null)
                return "";
            var str = new StringBuilder();
            str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
            return str.ToString();
        }
    </script>
</asp:Content>
