<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;用户管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;用户列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnSetVIP" onclick="setVip()">批量设置VIP</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnDelVIP" onclick="ActionEvent('delVIP','确定所选用户取消VIP?')">批量取消VIP</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnSetTutor" onclick="ActionEvent('setTutor','确定所选用户设置为专家?')">批量设置专家</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ActionEvent('DisableUser','确定禁用所选账户?')">批量禁用账户</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ActionEvent('EnableUser','确定启用所选账户?')">批量启用账户</a>
            <br />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnSetScore" onclick="EditScore()">修改积分</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnSendNotice" onclick="SendNotice()">发系统消息（给所有用户）</a>
            关键字匹配:<input id="txtKeyword" style="width: 200px;" placeholder="用户名，姓名，邮箱" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>用户名:
                </td>
                <td>
                    <input id="txtUserID" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>类型:
                </td>
                <td>
                    <select id="selectType" style="width: 250px;">
                        <option value="-" selected="selected">减</option>
                        <option value="+">加</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>积分:
                </td>
                <td>
                    <input id="txtScore" type="text" style="width: 250px;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                </td>
            </tr>
            <tr>
                <td>说明:
                </td>
                <td>
                    <textarea id="txtRmk" style="width: 250px;" rows="4"></textarea>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgInfo1" class="easyui-dialog" closed="true" title="系统消息发送" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>消息:
                </td>
                <td>
                    <textarea id="txtNotice" style="width: 300px;" rows="8"></textarea>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgInfo2" class="easyui-dialog" closed="true" title="设置VIP到期时间" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    VIP到期时间:
                </td>
                <td>
                    <input class="easyui-datebox" id="txtVipEndDate" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/UserHandler.ashx";
        var currDlgAction = '';
        var domain = '<%=Request.Url.Host%>';

        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "getUserList" },
                height: document.documentElement.clientHeight - 112,
                pagination: true,
                striped: true,
                pageSize: 20,
                rownumbers: true,
                singleSelect: false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                    { title: 'ck', width: 5, checkbox: true },
                    //{ field: 'userId', title: '用户名', width: 100, align: 'left', formatter: FormatterTitle },
                    {
                        field: 'avatar', title: '头像', width: 50, align: 'center', formatter: function (value) {
                            if (value == '' || value == null)
                                return "";
                            var str = new StringBuilder();
                            str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                            return str.ToString();
                        }
                    },
                    {
                        field: 'name', title: '姓名', width: 50, align: 'left', formatter: function (value, rowData) {
                            var str = new StringBuilder();
                            var classStr = rowData["isDisable"] == 1 ? 'style="color:red"' : '';
                            str.AppendFormat('<a href="/#/userspace/{1}" class="Red" title="{0}" {2} target="_blank">{0}</a>', value, rowData.id, classStr);
                            return str.ToString();
                        }
                    },
                    { field: 'email', title: '邮箱', width: 80, align: 'left', formatter: FormatterTitle },
                    { field: 'phone', title: '手机', width: 50, align: 'left', formatter: FormatterTitle },
                    { field: 'company', title: '公司', width: 80, align: 'left', formatter: FormatterTitle },
                    { field: 'postion', title: '职位', width: 50, align: 'left', formatter: FormatterTitle },
                    { field: 'userTotalScore', title: '积分', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'userType', title: '用户类型', width: 50, align: 'left', formatter: FormatterTitle },
                    {
                        field: 'isVip', title: 'VIP', width: 50, align: 'left', formatter: function (value, rowData) {
                            var str = new StringBuilder();
                            str.AppendFormat('{0}{1}{2}', value,value==''?'': '<br />', rowData["vipEndDate"]);
                            return str.ToString();
                        }
                    },
                    { field: 'regtime', title: '注册时间', width: 30, align: 'left', formatter: FormatterTitle }
                ]]
            }
               );
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        var dataModel = {
                            Action: "addScore",
                            UserId: $.trim($('#txtUserID').val()),
                            Type: $('#selectType').val(),
                            Score: $.trim($('#txtScore').val()),
                            Rmk: $.trim($('#txtRmk').val())
                        }

                        if (dataModel.UserID == '') {
                            Alert("用户名不能为空!");
                            return;
                        }
                        if (dataModel.Score == '') {
                            Alert("积分不能为空!");
                            return;
                        }
                        if (dataModel.Rmk == '') {
                            Alert("说明不能为空!");
                            return;
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    $('#dlgInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.Msg);
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

            $('#dlgInfo1').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        var dataModel = {
                            Action: "SendNotice",
                            Notice: $.trim($('#txtNotice').val())
                        }
                        if (dataModel.Notice == '') {
                            Alert("内容不能为空!");
                            return;
                        }
                        $.messager.progress({
                            title: '温馨提示',
                            msg: '正在发送...'
                        });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    $('#dlgInfo1').dialog('close');
                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgInfo1').dialog('close');
                    }
                }]
            });

            $('#dlgInfo2').dialog({
                buttons: [{
                    text: '提交',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
                        if (!EGCheckIsSelect(rows)) {
                            return;
                        }
                        var dataModel = {
                            Action: "setVip",
                            ids: GetRowsIds(rows).join(','),
                            VipEndDate: $.trim($('#txtVipEndDate').datebox('getValue'))
                        }
                        if (dataModel.VipEndDate == '') {
                            Alert("VIP到期时间不能为空!");
                            return;
                        }
                        $.messager.progress({
                            title: '温馨提示',
                            msg: '正在提交...'
                        });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    $('#dlgInfo2').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgInfo2').dialog('close');
                    }
                }]
            });
        });

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].userId);
            }
            return ids;
        }

        //删除
        function ActionEvent(action, msg) {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", msg, function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: action, ids: GetRowsIds(rows).join(',') },
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
        function EditScore() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }

            $('#txtScore').val("");
            $('#selectType').val("-");
            $('#txtUserID').val(rows[0].userId);
            txtUserID.readOnly = true;
            $('#dlgInfo').dialog({ title: '积分增减' });
            $('#dlgInfo').dialog('open');
        }
        function SendNotice() {
            $('#txtNotice').val("");
            $('#dlgInfo1').dialog('open');
        }

        function setVip() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var vipEndDate = '<% = vipEndDate%>';
            $('#txtVipEndDate').datebox('setValue', vipEndDate);
            $('#dlgInfo2').dialog('open');
        }

        function Search() {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "getUserList", keyword: $("#txtKeyword").val() }
            });
        }
    </script>
</asp:Content>
