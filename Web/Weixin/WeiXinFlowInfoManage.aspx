<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true"
    CodeBehind="WeiXinFlowInfoManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeiXinFlowInfoManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        var grid;
        //处理文件路径
        var url = "/Handler/WeiXin/WeiXinFlowInfoManage.ashx";
        //加载文档
        jQuery().ready(function () {

            $(window).resize(function () {
                $(list_data).datagrid('resize',
	            {
	                width: document.body.clientWidth - 10,
	                height: document.documentElement.clientHeight - 75
	            });
            });
            //-----------------加载gridview
            grid = jQuery("#list_data").datagrid({
                method: "Post",
                url: url,
                pageSize: 10,
                height: document.documentElement.clientHeight - 75,
                fitCloumns: true,
                nowrap: true,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "Query", SearchTitle: "" }
            });
            //------------加载gridview

            //取消---------------------
            $("#win").find("#btnExit").bind("click", function () {
                $("#win").window("close");
            });
            //取消---------------------



            //搜索开始------------------------
            $("#btnSearch").click(function () {
                var SearchTitle = $("#txtName").val();
                grid.datagrid({ url: url, queryParams: { Action: "Query", SearchTitle: SearchTitle} });
            });
            //搜索结束---------------------



            //保存---------------------
            $("#btnSave").bind("click", function () {

//                var tag = jQuery.trim(jQuery(this).attr("tag"));

//                if (tag == "add") {
//                    //添加
//                    Add();
//                    return;
//                }
//                //修改
                Save();
            });
        });
        //保存---------------------


        //弹出添加或编辑框开始
        function ShowAddOrEdit(addoredit) {
            var title = ""; //弹出框标题
            var titleicon = "icon-" + addoredit; //弹出框标题图标
            //弹出添加框开始
            if (addoredit == "add") {
                //清除数据
                Clear("txtFlowName|txtFlowKeyword|txtFlowEndMsg|txtLimitMsg");
                //设置默认值
                $("#rd1").attr("checked", true);
                $("#rde1").attr("checked", true);
                //设置弹出框标题
                title = "添加";

            }
            //弹出添加框结束

            //弹出编辑框开始
            else if (addoredit == "edit") {
                // 只能选择一条记录操作
                var rows = grid.datagrid('getSelections');
                var num = rows.length;
                if (num == 0) {
                    messager('系统提示', "请选择一条记录进行操作！");
                    return;
                }
                if (num > 1) {
                    $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
                    return;
                }
                // 只能选择一条记录操作

                //加载信息开始
                $("#txtFlowName").val(rows[0].FlowName);
                var limitmsg = replacebrtag(rows[0].FlowLimitMsg);
                var endmsg = replacebrtag(rows[0].FlowEndMsg);
                $("#txtFlowKeyword").val(rows[0].FlowKeyword);
                $("#txtFlowEndMsg").val(endmsg);
                var state = rows[0].MemberLimitState;
                if (state == 1) {
                    $("#rd1").attr("checked", true);
                }
                else if (state == 0) {
                    $("#rd0").attr("checked", true);
                }
                var isenable = rows[0].IsEnable;
                if (isenable == 1) {
                    $("#rde1").attr("checked", true);
                }
                else if (isenable == 0) {
                    $("#rde0").attr("checked", true);
                }

                $("#txtLimitMsg").val(limitmsg);
                //加载信息结束

                //设置弹出框标题
                title = "编辑";


            }
            //弹出编辑框结束


            //弹出对话框
            $("#win").window({
                title: title,
                closed: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                iconCls: titleicon,
                resizable: false,
                width: 320,
                height: 350,
                top: ($(window).height() - 350) * 0.5,
                left: ($(window).width() - 320) * 0.5

            });
            //弹出对话框

            //设置保存按钮属性 add为添加，edit为编辑
            $("#btnSave").attr("tag", addoredit);


        }
        //展示添加或编辑框结束


        //添加或编辑操作开始---------
        function Save() {
            var FlowName = $("#txtFlowName").val();
            var FlowKeyword = $("#txtFlowKeyword").val();
            var FlowEndMsg = $("#txtFlowEndMsg").val();
            var FlowLimitMsg = $("#txtLimitMsg").val();
            if (FlowName == "") {
                $("#txtFlowName").focus();
                return false;

            }
            if (FlowKeyword == "") {
                $("#txtFlowKeyword").focus();
                return false;

            }
            if (FlowEndMsg == "") {
                $("#txtFlowEndMsg").focus();
                return false;

            }
            if (FlowLimitMsg == "") {
                $("#txtLimitMsg").focus();
                return false;

            }
            var MemberLimitState = "";
            if ($("#rd1").attr("checked")) {
                MemberLimitState = 1;
            }
            else if ($("#rd0").attr("checked")) {
                MemberLimitState = 0;
            }
            var IsEnable = "";
            if ($("#rde1").attr("checked")) {
                IsEnable = 1;
            }
            else if ($("#rde0").attr("checked")) {
                IsEnable = 0;
            }
            var action = $("#btnSave").attr("tag"); //获取添加或编辑属性
            //----------执行添加操作开始
            if (action == "add") {
                //------------添加
                jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "Add", FlowName: FlowName, FlowKeyword: FlowKeyword, FlowEndMsg: FlowEndMsg, FlowLimitMsg: FlowLimitMsg, MemberLimitState: MemberLimitState, IsEnable: IsEnable },
                    success: function (result) {
                        if (result == "true") {
                            messager("系统提示", "添加成功");
                            $("#win").window("close");
                            grid.datagrid('reload');
                        } else {
                            messager("系统提示", result);
                        }
                    }
                });
                //添加---------------
            }
            //-----------执行添加操作结束
            //-----------执行编辑操作开始
            else if (action == "edit") {
                //-----------修改
                var rows = grid.datagrid('getSelections');
                var FlowID = rows[0].FlowID;
                jQuery.ajax({
                    type: "Post",
                    url: url,
                    data: { Action: "Edit", FlowID: FlowID, FlowName: FlowName, FlowKeyword: FlowKeyword, FlowEndMsg: FlowEndMsg, FlowLimitMsg: FlowLimitMsg, MemberLimitState: MemberLimitState, IsEnable: IsEnable },
                    success: function (result) {
                        if (result == "true") {
                            messager("系统提示", "修改成功");
                            $("#win").window("close");
                            grid.datagrid('reload');
                        }
                        else {
                            messager("系统提示", result);
                        }
                    }
                });
                //修改
            }
            //--------------执行编辑操作结束

        }
        //添加或编辑操作结束---------

//        //添加信息输入框---------------------
//        function ShowAdd() {
//            $("#win").window({
//                title: "添加",
//                closed: false,
//                collapsible: false,
//                minimizable: false,
//                maximizable: false,
//                iconCls: "icon-add",
//                resizable: false,
//                width: 320,
//                height: 350,
//                top: ($(window).height() - 320) * 0.5,
//                left: ($(window).width() - 350) * 0.5

//            });
//            //设置保存按钮目标为添加
//            $("#btnSave").attr("tag", "add");
//            //清除数据
//            Clear();


//        }
//        //添加信息输入框---------------------


//        //保存添加的信息---------------------
//        function Add() {

//            var FlowName = $("#txtFlowName").val();
//            var FlowKeyword = $("#txtFlowKeyword").val();
//            var FlowEndMsg = $("#txtFlowEndMsg").val();
//            var FlowLimitMsg = $("#txtLimitMsg").val();
//            if (FlowName == "") {
//                $("#txtFlowName").focus();
//                return false;

//            }
//            if (FlowKeyword == "") {
//                $("#txtFlowKeyword").focus();
//                return false;

//            }
//            if (FlowEndMsg == "") {
//                $("#txtFlowEndMsg").focus();
//                return false;

//            }
//            var MemberLimitState = "";
//            if ($("#rd1").attr("checked")) {
//                MemberLimitState = 1;
//            }
//            else if ($("#rd0").attr("checked")) {
//                MemberLimitState = 0;
//            }
//            var IsEnable = "";
//            if ($("#rde1").attr("checked")) {
//                IsEnable = 1;
//            }
//            else if ($("#rde0").attr("checked")) {
//                IsEnable = 0;
//            }
//            jQuery.ajax({
//                type: "Post",
//                url: url,
//                data: { Action: "Add", FlowName: FlowName, FlowKeyword: FlowKeyword, FlowEndMsg: FlowEndMsg, FlowLimitMsg: FlowLimitMsg, MemberLimitState: MemberLimitState, IsEnable: IsEnable },
//                success: function (result) {
//                    if (result == "true") {
//                        messager("系统提示", "添加成功");
//                        $("#win").window("close");
//                        grid.datagrid('reload');
//                    } else {
//                        messager("系统提示", result);
//                    }
//                }
//            });
//        };
//        //保存添加的信息---------------------


//        // 修改信息---------------------
//        function Save() {
//            var rows = grid.datagrid('getSelections');
//            var FlowName = $("#txtFlowName").val();
//            var FlowKeyword = $("#txtFlowKeyword").val();
//            var FlowEndMsg = $("#txtFlowEndMsg").val();
//            var FlowLimitMsg = $("#txtLimitMsg").val();
//            var FlowID = rows[0].FlowID;
//            if (FlowName == "") {
//                $("#txtFlowName").focus();
//                return false;

//            }
//            if (FlowKeyword == "") {
//                $("#txtFlowKeyword").focus();
//                return false;

//            }
//            if (FlowEndMsg == "") {
//                $("#txtFlowEndMsg").focus();
//                return false;

//            }
//            var MemberLimitState = "";
//            if ($("#rd1").attr("checked")) {
//                MemberLimitState = 1;
//            }
//            else if ($("#rd0").attr("checked")) {
//                MemberLimitState = 0;
//            }
//            var IsEnable = "";
//            if ($("#rde1").attr("checked")) {
//                IsEnable = 1;
//            }
//            else if ($("#rde0").attr("checked")) {
//                IsEnable = 0;
//            }

//            jQuery.ajax({
//                type: "Post",
//                url: url,
//                data: { Action: "Edit", FlowID: FlowID, FlowName: FlowName, FlowKeyword: FlowKeyword, FlowEndMsg: FlowEndMsg, FlowLimitMsg: FlowLimitMsg, MemberLimitState: MemberLimitState, IsEnable: IsEnable },
//                success: function (result) {
//                    if (result == "true") {
//                        messager("系统提示", "修改成功");
//                        $("#win").window("close");
//                        grid.datagrid('reload');
//                    }
//                    else {
//                        messager("系统提示", result);
//                    }
//                }
//            });
//        }
//        // 修改信息---------------------


        // 删除---------------------
        function Delete() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择您要删除的记录");
                return;
            }
            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].FlowID);
            }

            $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "Delete", id: ids.join(',') },
                        success: function (result) {
                            if (result) {
                                messager('系统提示', "删除成功！");
                                grid.datagrid('reload');

                                return;
                            }
                            $.messager.alert("信息删除失败。");
                        }
                    });
                }
            });
        };
        // 删除---------------------


//        //展示编辑框---------------------
//        function Edit() {
//            var rows = grid.datagrid('getSelections');
//            var num = rows.length;
//            if (num == 0) {
//                messager('系统提示', "请选择一条记录进行操作！");
//                return;
//            }
//            if (num > 1) {
//                $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
//                return;
//            }
//            $("#win").window({
//                title: "修改",
//                closed: false,
//                collapsible: false,
//                minimizable: false,
//                maximizable: false,
//                iconCls: "icon-edit",
//                resizable: false,
//                width: 320,
//                height: 350,
//                top: ($(window).height() - 320) * 0.5,
//                left: ($(window).width() - 350) * 0.5

//            });
//            //加载信息
//            $("#txtFlowName").val(rows[0].FlowName);
//            var limitmsg = replacebrtag(rows[0].FlowLimitMsg);
//            var endmsg = replacebrtag(rows[0].FlowEndMsg);
//            $("#txtFlowKeyword").val(rows[0].FlowKeyword);
//            $("#txtFlowEndMsg").val(endmsg);
//            var state = rows[0].MemberLimitState;
//            if (state == 1) {
//                $("#rd1").attr("checked", true);
//            }
//            else if (state == 0) {
//                $("#rd0").attr("checked", true);
//            }
//            var isenable = rows[0].IsEnable;
//            if (isenable == 1) {
//                $("#rde1").attr("checked", true);
//            }
//            else if (isenable == 0) {
//                $("#rde0").attr("checked", true);
//            }

//            $("#txtLimitMsg").val(limitmsg);
//            $("#btnSave").attr("tag", "Edit");
//        }



        //展示编辑框--------------


//        //清除数据-----------
//        function Clear() {
//            $("#txtFlowName").val("");
//            $("#txtFlowKeyword").val("");
//            $("#txtFlowEndMsg").val("");
//            $("#txtLimitMsg").val("");
//            $("#rd1").attr("checked", true);
//        }

//        //清除数据-----------

        function operate(value, row, index) {

            return "<a href='#' title='管理流程步骤' onclick='e_dlg(" + row.FlowID + ")'>管理流程步骤</a>" + "&nbsp;&nbsp;" + "<a href='#' title='管理流程数据' onclick='flowstepdata(" + row.FlowID + ")'>管理流程数据</a>";


        }

        //流程步骤管理----------
        function e_dlg(id) {

            window.location.href = '/Weixin/WeiXinFlowStepInfo.aspx?FlowID=' + id;

        }
        //流程步骤管理----------

        function flowstepdata(id) {

            window.location.href = '/Weixin/WeixinFlowStepInfoData.aspx?FlowID=' + id;

        }
        function changestate(value) {

            if (value == 1) {
                return "<font color='red'>一次</font>";

            }
            else if (value == 0) {
                return "<font color='green'>多次</font>";

            }


        }
        function changeenable(value) {

            if (value == 1) {
                return "<font color='green'>启用</font>";

            }
            else if (value == 0) {
                return "<font color='red'>禁用</font>";

            }


        }


        //批量启用禁用
        function BatChangState(value) {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择您要修改的记录");
                return;
            }

            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].FlowID);
            }
            var msg = "启用";
            if (value == 0) {
                msg = "禁用";
            }
            $.messager.confirm("系统提示", "是否确定" + msg + "选中信息?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: url,
                        data: { Action: "BatChangState", id: ids.join(','), IsEnable: value },
                        success: function (result) {
                            if (result) {
                                messager('系统提示', "修改成功！");
                                grid.datagrid('reload');

                                return;
                            } else {
                                messager('系统提示', result);
                            }

                        }
                    });
                }
            });
        };
        //批量启用禁用
    </script>
    <style type="text/css">
        .style1
        {
            width: 30%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="center" style="margin: 5px;">
            <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
               
                   
                    <div>
                        <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="ShowAddOrEdit('add')">
                            添加流程</a> <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">
                                编辑</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="Delete()">
                                    删除</a> <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="BatChangState(1)">
                                        批量启用</a> <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="BatChangState(0)">
                                            批量禁用</a>
                    </div>
                     <div>
                        <span style="font-size: 12px; font-weight: normal">流程名称:</span>
                        <input id="txtName" style="width: 200px" />
                        <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
                    </div>
                
            </div>
            <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="ck" width="5" checkbox="true">
                        </th>
                        <th field="FlowName" width="20">
                            流程名称
                        </th>
                        <th field="FlowKeyword" width="20">
                            关键字
                        </th>
                        <th field="FlowEndMsg" width="20">
                            结束信息
                        </th>
                        <th field="FlowLimitMsg" width="20">
                            重复进入提示
                        </th>
                        <th field="MemberLimitState" formatter="changestate" width="10">
                            可执行次数
                        </th>
                        <th field="IsEnable" formatter="changeenable" width="10">
                            流程启用状态
                        </th>
                        <th field="action" formatter="operate" width="20">
                            操作
                        </th>
                    </tr>
                </thead>
            </table>
            <div id="win" class="easyui-window" modal="true" closed="true" style="padding: 10px;
                text-align: center;">
                <table style="margin: auto;">
                    <tr>
                        <td align="right" class="style1">
                            流程名称:
                        </td>
                        <td style="text-align: left">
                            <input type="text" id="txtFlowName" style="width: 150px;" class="easyui-validatebox"
                                required="true" missingmessage="请输入流程名称" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style1">
                            关键字:
                        </td>
                        <td style="text-align: left">
                            <input type="text" id="txtFlowKeyword" style="width: 150px;" class="easyui-validatebox"
                                required="true" missingmessage="请输入关键字" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style1">
                            结束信息:
                        </td>
                        <td style="text-align: left">
                            <%-- <input type="text" id="txtFlowEndMsg" style="width: 150px;"  />--%>
                            <textarea id="txtFlowEndMsg" style="width: 150px; height: 50px"  class="easyui-validatebox"
                                required="true" missingmessage="请输入结束信息"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style1">
                            重复进入提示:
                        </td>
                        <td style="text-align: left">
                            <textarea id="txtLimitMsg" style="width: 150px; height: 50px"  class="easyui-validatebox"
                                required="true" missingmessage="请输入重复进入提示信息"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style1">
                            可执行次数:
                        </td>
                        <td style="text-align: left">
                            <input type="radio" id="rd1" name="rdo" checked="checked" />
                            <label for='rd1'>
                                一次</label>
                            <input type="radio" id="rd0" name="rdo" />
                            <label for='rd0'>
                                多次</label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style1">
                            启用状态:
                        </td>
                        <td style="text-align: left">
                            <input type="radio" id="rde1" name="rd" checked="checked" />
                            <label for='rde1'>
                                启用</label>
                            <input type="radio" id="rde0" name="rd" />
                            <label for='rde0'>
                                禁用</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                        <td align="left">
                            <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                                保 存</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                                    关 闭</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
