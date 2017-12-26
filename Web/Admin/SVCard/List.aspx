<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.SVCard.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tagBtn {
            border: 1px solid #d2d2d2;
            text-decoration: none;
            background: #fff;
            display: inline-block;
            text-align: center;
            font-size: 14px;
            padding: 5px;
            margin: 5px 5px 0 0;
        }
        .selectTag{
            background-color: #18a689;
            border-color: #18a689;
        }
        .keyType{
            width: 100px;
            padding: 1px;
            height: 25px;
            border-radius: 0px;
            display: inline;
        }
        .easyui-textbox{
            width: 200px;
            display: inline;
            border-radius: 0px;
            padding: 2px;
            margin-left: 5px;
            width:300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;储值卡管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/svcard/list.ashx',pagination:true,striped:true,loadFilter: pagerFilter,rownumbers:true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="id" width="30" formatter="FormatterTitle">储值卡ID</th>
                <th field="bg_img" width="50" formatter="FormatBgImg">背景图</th>
                <th field="name" width="100" formatter="FormatterTitle">名称</th>
                <th field="amount" width="50" formatter="FormatterTitle">金额</th>
                <th field="valid_to" width="50" formatter="FormatterTitle">有效期</th>
                <th field="send_count" width="30" formatter="FormatSendCount">发放数</th>
                <th field="status" width="50" formatter="FormatterStatus">状态</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" title="新增储值卡" onclick="ShowAddItem()" id="btnAdd" runat="server">新增储值卡</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="发放储值卡" onclick="ShowSendCard()" id="btnSend" runat="server">发放储值卡</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="批量修改状态" onclick="ShowUpdateStatus()" id="btnStart" runat="server">批量修改状态</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="批量修改数量" onclick="ShowUpdateMaxCount()" id="btnUpdateMaxCount" runat="server">批量修改数量</a>
            <br />
            状态：<select id="sltStatus" class="easyui-combobox" data-options="editable:false,onSelect:Search">
                <option value="">全部</option>
                <option value="0">正常</option>
                <option value="1">停用</option>
            </select>
            名称：<input id="txtKeyword" class="easyui-textbox" />
            <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        </div>
    </div>

    <div id="dlgAddCard" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'新增储值卡',modal:true,width:380,height:360,buttons:'#dlgAddCardButtons'"
        style="padding: 10px; line-height: 30px;">
        <table style="margin: auto;">
            <tr>
                <td style="width: 70px;">名称：</td>
                <td>
                    <input id="txtName" class="easyui-validatebox" data-options="required:true" /></td>
            </tr>
            <tr>
                <td>金额：</td>
                <td>
                    <input id="txtAmount" class="easyui-numberbox" data-options="required:true,precision:0,min:1" /></td>
            </tr>
            <tr>
                <td>数量：</td>
                <td>
                    <input id="txtMaxCount" class="easyui-numberbox" data-options="required:true,precision:0,min:1" /></td>
            </tr>
            <tr>
                <td>有效期：</td>
                <td>
                    <input id="txtValidTo" class="easyui-datetimebox" data-options="showSeconds:false" /></td>
            </tr>
            <tr>
                <td>背景图：</td>
                <td>
                    <img id="imgBgImage" alt="" src="http://static-files.socialcrmyun.com/img/SVCard/1.png" width="147" height="80" />
                    <%--800*436--%>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgAddCardButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="AddCard()">新增</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgAddCard').dialog('close');">取消</a>
    </div>

    <div id="dlgSendCard" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'发放储值卡',modal:true,width:650,height:500,border:false,buttons:'#dlgSendCardButtons'">
        <div id="tabSendCard" class="easyui-tabs" data-options="fit:true,border:false">
            <div id="tTags" title="选择标签组" data-options="iconCls:'icon-usergroup',selected:true" style="padding: 10px">
            </div>
            <div id="tUsers" title="选择指定用户" data-options="iconCls:'icon-userSingel'">
                <table id="grvUser" class="easyui-datagrid"
                    data-options="fit:true,fitColumns:true,toolbar:'#divUserToolbar',border:false,method:'Post',
                    loader:pagerUserLoader,loadFilter:pagerFilterResp,pagination:true,striped:true,rownumbers:true">
                    <thead>
                        <tr>
                            <th field="ck" width="5" checkbox="true"></th>
                            <th field="autoid" width="25" formatter="FormatterTitle">会员ID</th>
                            <th field="wx_head_img_url" width="25" formatter="FormatterImage50">微信头像</th>
                            <th field="wx_nick_name" width="40" formatter="FormatterTitle">微信昵称</th>
                            <th field="true_name" width="30" formatter="FormatterTitle">姓名</th>
                            <th field="user_phone" width="30" formatter="FormatterTitle">手机</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
    <div id="divUserToolbar" style="padding: 5px; height: auto">
        关键字：
        <select class="form-control keyType" id="selKeyWordType">
            <option value="key_truename">姓名</option>
            <option value="key_nickname">昵称</option>
            <option value="key_phone">手机</option>
        </select><input id="txtUserKeyword" class="easyui-textbox form-control" placeholder="按昵称、姓名、手机号码模糊搜索" />
        <a id="btnSearchUser" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="SearchUser();">查询</a>
    </div>
    <div id="dlgSendCardButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="SendCard()">发放</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgSendCard').dialog('close');">取消</a>
    </div>
    <div id="dlgUpdateStatus" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'批量修改状态',modal:true,width:300,height:120"
        style="padding: 30px; text-align: center;">
        <a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-start'" onclick="UpdateStatus(0)">启用</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" data-options="iconCls:'icon-stop'" onclick="UpdateStatus(1)">停用</a>
    </div>
    <div id="dlgUpdateMaxCount" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'批量修改数量',modal:true,width:300,height:120,buttons:'#dlgUpdateMaxCountButtons'"
        style="padding: 10px; text-align: center;">
        数量：<input id="txtUpdateMaxCount" class="easyui-numberbox" data-options="required:true,precision:0,min:1" />
    </div>
    <div id="dlgUpdateMaxCountButtons">
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="UpdateMaxCount()">修改</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgUpdateMaxCount').dialog('close');">取消</a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/svcard/';
        $(function () {
            GetTags();
        });
        //加载出用户组标签
        function GetTags() {
            $.ajax({
                type: "Post",
                url: '/serv/api/admin/user/membertag/list.ashx',
                data: { pageindex: 1, pagesize: 1000 },
                dataType: "json",
                success: function (resp) {
                    if (resp.isSuccess && resp.returnObj && resp.returnObj.list && resp.returnObj.list.length > 0) {
                        var str = new StringBuilder();
                        for (var i = 0; i < resp.returnObj.list.length; i++) {
                            str.AppendFormat('<button class="tagBtn" onclick="selectTag(this)">{0}</button>', resp.returnObj.list[i].tag_name);
                        }
                        $('#tTags').html(str.ToString());
                    }
                }
            });
        }
        //搜索用户
        function SearchUser() {
            var model = {
                KeyWord: $.trim($("#txtUserKeyword").val()),
                key_word_type: $("#selKeyWordType").val()
            }
            $('#grvUser').datagrid('load', model);
        }
        //用户列表查询
        function pagerUserLoader(param, success, error) {
            var qParam = $.extend({}, param, {
                rows: param.rows,
                page: param.page
            })
            $.ajax({
                type: "Post",
                url: '/serv/api/admin/user/list.ashx',
                data: qParam,
                dataType: "json",
                success: success,
                error: error
            });
        }
        //清除选择的标签和用户
        function clearSelect() {
            $('#tTags .selectTag').removeClass('selectTag');//清除选中标签
            $("#grvUser").datagrid('clearSelections');//清除选中行
        }

        function pagerFilterResp(result) {
            var data = result.returnObj;
            if (data == null) {
                return {
                    total: 0,
                    rows: []
                };
            }
            return {
                total: data.totalcount,
                rows: data.list
            };
        }
        //选择标签
        function selectTag(obj) {
            if (!$(obj).hasClass('selectTag')) {
                $(obj).addClass('selectTag')
            } else {
                $(obj).removeClass('selectTag')
            }
        }
        //格式化背景图
        function FormatBgImg(value) {
            if (value == '' || value == null) return "";
            return '<img alt="" class="imgAlign" src="' + value + '" title="缩略图" height="50" width="92" />';
        }
        //格式化发送数
        function FormatSendCount(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="/Admin/SVCard/Record/List.aspx?card_id={1}" style="color:blue;">{0}</a>', value + '/' + rowData.max_count, rowData.id);
            return str.ToString();
        }
        //格式化状态
        function FormatterStatus(value) {
            if (value == 0) return '正常';
            return '<span style="color:red;">停用</span>';
        }
        //搜索
        function Search() {
            var model = {
                status: $.trim($("#sltStatus").combobox('getValue')),
                keyword: $.trim($("#txtKeyword").val())
            }
            $('#grvData').datagrid('load', model);
        }
        //显示新增框
        function ShowAddItem() {
            InitCard();
            $('#dlgAddCard').dialog('open');
        }
        //初始新增框
        function InitCard() {
            $('#txtName').val('');
            $('#txtAmount').numberbox('setValue', '');
            $('#txtMaxCount').numberbox('setValue', '');
            $('#txtValidTo').datetimebox('setValue', '');
        }
        //新增储值卡
        function AddCard() {
            if (!$('#dlgAddCard').form('validate')) {
                return;
            }
            var data = {
                name: $.trim($('#txtName').val()),
                amount: $.trim($('#txtAmount').numberbox('getValue')),
                max_count: $.trim($('#txtMaxCount').numberbox('getValue')),
                valid_to: $.trim($('#txtValidTo').datetimebox('getValue')),
                bg_img: $.trim($('#imgBgImage').attr('src'))
            }

            $.ajax({
                type: "Post",
                url: handlerUrl + 'add.ashx',
                data: data,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $('#grvData').datagrid('reload');
                        $('#dlgAddCard').dialog('close');
                        Show(resp.msg);
                    }
                    else {
                        Alert(resp.msg);
                    }
                }
            });
        }
        //显示发放框
        function ShowSendCard() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (rows.length == 0) {
                Alert("请选择储值卡");
                return;
            }
            else if (rows.length > 1) {
                Alert("一次仅能发放一种储值卡");
                return;
            }
            clearSelect();
            $('#dlgSendCard').dialog('open');
        }
        function SendCard() {
            var tab = $('#tabSendCard').tabs('getSelected');
            var index = $('#tabSendCard').tabs('getTabIndex', tab);

            var data = {};
            var strt = '';
            if (index == 0) {
                var tags = [];
                $('#tTags .selectTag').each(function () {
                    var tag = $.trim($(this).text());
                    if (tag != '') tags.push(tag);
                });
                if (tags.length == 0) {
                    Alert('请选择接受储值卡的用户标签组');
                    return;
                }
                data.tags = tags.join(',');
                data.type = 2;
                strt = '标签组用户';
            } else if (index == 1) {
                var rows = $("#grvUser").datagrid('getSelections');//清除选中行
                if (rows.length == 0) {
                    Alert("请选择接受储值卡的用户");
                    return;
                }
                var user_ids = [];
                for (var i = 0; i < rows.length; i++) {
                    user_ids.push(rows[i].UserID);
                }
                data.user_ids = user_ids.join(',')
                data.type = 1;
                strt = '用户';
            }
            $.messager.confirm('系统提示', '确定发放储值卡给所选' + strt + '？', function (o) {
                var rows = $("#grvData").datagrid('getSelections');//获取选中的行
                data.id = rows[0].id;
                $.ajax({
                    type: "Post",
                    url: handlerUrl + 'send.ashx',
                    data: data,
                    dataType: "json",
                    success: function (resp) {
                        if (resp.status) {
                            $('#grvData').datagrid('reload');
                            $('#dlgSendCard').dialog('close');
                            Show(resp.msg);
                        }
                        else {
                            Alert(resp.msg);
                        }
                    }
                });
            });
        }
        //显示修改状态框
        function ShowUpdateStatus() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (rows.length == 0) {
                Alert("请选择储值卡");
                return;
            }
            $('#dlgUpdateStatus').dialog('open');
        }
        function UpdateStatus(value) {
            var action = value == 0 ? '启用' : '停用';
            $.messager.confirm('系统提示', '确定' + action + '所选储值卡？', function (o) {

                var rows = $("#grvData").datagrid('getSelections');//获取选中的行
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].id);
                }
                if (ids.length == 0) return;
                var data = {
                    ids: ids.join(','),
                    status: value
                }
                $.ajax({
                    type: "Post",
                    url: handlerUrl + 'updatestatus.ashx',
                    data: data,
                    dataType: "json",
                    success: function (resp) {
                        if (resp.status) {
                            $('#grvData').datagrid('reload');
                            $('#dlgUpdateStatus').dialog('close');
                            Show(resp.msg);
                        }
                        else {
                            Alert(resp.msg);
                        }
                    }
                });
            });
        }
        //显示修改数量框
        function ShowUpdateMaxCount() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (rows.length == 0) {
                Alert("请选择储值卡！");
                return;
            }
            $('#txtUpdateMaxCount').numberbox('setValue', '');
            $('#dlgUpdateMaxCount').dialog('open');
        }
        //修改数量
        function UpdateMaxCount() {
            if (!$('#dlgUpdateMaxCount').form('validate')) {
                return;
            }
            var value = $.trim($('#txtUpdateMaxCount').numberbox('getValue'));
            if (value == '') Alert('请输入数量');

            $.messager.confirm('系统提示', '确定修改所选储值卡数量？', function (o) {
                if (o) {
                    UpdateField('max_count', value, function () {
                        $('#dlgUpdateMaxCount').dialog('close');
                    });
                }
            });

        }

        //修改字段
        function UpdateField(field, value, fn) {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id);
            }
            if (ids.length == 0) return;

            if (!$('#dlgUpdateMaxCount').form('validate')) {
                return;
            }
            var data = {
                ids: ids.join(','),
                field: field,
                value: value
            }
            $.ajax({
                type: "Post",
                url: handlerUrl + 'updatefield.ashx',
                data: data,
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $('#grvData').datagrid('reload');
                        fn();
                        Show(resp.msg);
                    }
                    else {
                        Alert(resp.msg);
                    }
                }
            });
        }
    </script>
</asp:Content>
