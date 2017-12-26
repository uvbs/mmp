<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="MonitorLink.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Monitor.MonitorLink" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">



        var handlerUrl = '/Handler/Monitor/MonitorHandler.ashx';
        var grid;
        var currSelectID = 0;
        var planid = "<%=PlanId %>";

        $(function () {




            //-----------------加载gridview
            grid = jQuery("#list_data").datagrid({
                method: "Post",
                url: handlerUrl,
                height: 570,
                toolbar: '#divToolbar',
                fitCloumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                queryParams: { Action: "QueryLink", PlanId: planid }
            });
            //------------加载gridview

            //窗体关闭按钮---------------------
            $("#dlgLinkInfo").find("#btnExit").bind("click", function () {
                $("#dlgLinkInfo").window("close");
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
                var linkName = $.trim($("#txtLinkNames").val());
                grid.datagrid({ url: handlerUrl, queryParams: { Action: "QueryLink", PlanId: planid, LinkName: linkName} });

            });




        });



        function ShowAdd() {
            ClearWinDataByTag('input', dlgLinkInfo);
            $("#btnSave").attr('tag', 'add');
            $('#dlgLinkInfo').window(
            {
                title: '添加'
            }
            );

            $('#dlgLinkInfo').dialog('open');

            $('#txtLink').focus();
            $('#txtOpenCode').val("创建后自动生成");
            $('#txtConvertUrl').val("创建后自动生成");

        }

        function ShowEdit() {
            var rows = grid.datagrid('getSelections');
            if (!EGCheckNoSelectMultiRow(rows)) {
                return;
            }
            ClearWinDataByTag('input', dlgLinkInfo);

            $('#dlgLinkInfo').window(
            {
                title: '编辑'
            }
            );

            $('#dlgLinkInfo').dialog('open');
            $("#btnSave").attr('tag', 'edit');

            //加载编辑数据
            currSelectID = rows[0].LinkID;
            $('#txtLink').val(rows[0].RealLink);
            $('#txtLinkName').val(rows[0].LinkName);
            $('#txtOpenCode').val(rows[0].LinkOpenCode);
            $('#txtConvertUrl').val(rows[0].ConvertUrl);

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
                    data: { Action: "AddLink", JsonData: JSON.stringify(model).toString() },
                    success: function (result) {
                        if (result == "true") {
                            $.messager.show({
                                title: '系统提示',
                                msg: '添加成功.'
                            });
                            grid.datagrid('reload');
                            $("#dlgLinkInfo").window("close");
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
                    data: { Action: "EditLink", JsonData: JSON.stringify(model).toString() },
                    success: function (result) {
                        if (result == "true") {
                            $.messager.show({
                                title: '系统提示',
                                msg: '编辑成功.'
                            });
                            grid.datagrid('reload');
                            $("#dlgLinkInfo").window("close");
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
                        ids.push(rows[i].LinkID);
                    }
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteLink", ids: ids.join(',') },
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
            var model =
            {
                "LinkID": currSelectID,
                "RealLink": $.trim($("#txtLink").val()),
                "MonitorPlanID": planid,
                "LinkName": $("#txtLinkName").val()

            }
            return model;
        }

        //检查输入框输入
        function CheckDlgInput(model) {
            if (model['RealLink'] == '') {
                $("#txtLink").val("");
                $("#txtLink").focus();
                return false;
            }
            return true;
        }


        //格式化状态
        function FormatStatus(value) {

            if (value == "0") {
                return "<font color='red'>已停止</font>";
            }
            if (value == "1") {
                return "<font color='green'>正在运行</font>";
            }

        }


        //管理
        function Opreate(value, row) {
            var result = new StringBuilder();
            result.AppendFormat('<a title="管理" href="javascript:;" onclick="GotoLinkPage({0})">管理</a>', row.MonitorPlanID);
            return result.ToString();
        }

        //管理跳转
        function GotoLinkPage(id) {
            var url = "/Monitor/MonitorLink.aspx?id=" + id;
            parent.addTab('管理链接-' + id, url, 'tu0818', true);
        }
        //管理跳转

        function GetOpenCode() {

            var rows = grid.datagrid('getSelections');
            if (!EGCheckIsSelect(rows)) {
                return;
            }

            $("#txtCode").html(rows[0].ConvertUrl);
            $('#dlgReadCode').dialog('open');

        }

        function FormatOpenCount(value, row) {

            return "<a title=\"点击查看详细列表\" href=\"/Monitor/MonitorEventDetails.aspx?planId=" + row.MonitorPlanID + " &linkid=" + row.LinkID + "\">" + value + "</a>";


        }


    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：监测平台&nbsp;<span>任务管理-链接管理</span>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="list_data" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true">
                    <th field="LinkID" width="10">
                        链接ID
                    </th>
                    <th field="LinkName" width="30">
                        链接名称
                    </th>
                    <th field="RealLink" width="40">
                        链接地址
                    </th>
                    <th field="OpenCount1" formatter="FormatOpenCount" align="center" width="10">
                        浏览量（PV）
                    </th>
                    <th field="DistinctOpenCount1" formatter="FormatOpenCount" align="center" width="10">
                        IP数
                    </th>
                    <th field="InsertDate" formatter="FormatDate" width="15">
                        建立日期
                    </th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                title="添加" onclick="ShowAdd()" id="btnAdd" runat="server">添加链接</a> <a href="javascript:;"
                    class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="编辑链接" onclick="ShowEdit()"
                    id="btnEdit" runat="server">编辑链接 </a><a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-delete" plain="true" title="删除链接" onclick="Delete()" id="btnDelete"
                        runat="server">删除链接 </a>
            <%--<a style="float: right;"  href="/Monitor/MonitorPlanManage.aspx"
                                    class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>--%>
            <a style="float: right;" href="javascript:history.go(-1);" class="easyui-linkbutton"
                iconcls="icon-back" plain="true">返回上一页</a>
            <br />
            <div>
                <span style="font-size: 12px; font-weight: normal">链接名称:</span>
                <input id="txtLinkNames" style="width: 200px" />
                <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>
        </div>
    </div>
    <div id="dlgLinkInfo" class="easyui-dialog" closed="true" modal="true" title="链接"
        style="width: 500px; height: 300px; padding: 10px">
        <div>
            <table>
                <tr>
                    <td valign="top">
                        链接地址：
                    </td>
                    <td height="25" width="*" align="left">
                        <input id="txtLink" style="width: 380px;" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        链接名称：
                    </td>
                    <td height="25" width="*" align="left">
                        <input id="txtLinkName" style="width: 200px;" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        检测代码：
                    </td>
                    <td height="25" width="*" align="left">
                        <textarea id="txtOpenCode" style="width: 380px; height: 60px;" readonly="readonly"></textarea>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        转换链接：
                    </td>
                    <td height="25" width="*" align="left">
                        <textarea id="txtConvertUrl" style="width: 380px; height: 50px;" readonly="readonly"></textarea>
                    </td>
                </tr>
            </table>
            <div style="float: right; margin-top: 5px; margin-right: 10px;">
                <a href="#" class="easyui-linkbutton" id="btnSave">确定</a> <a href="#" class="easyui-linkbutton"
                    id="btnExit">取消</a>
            </div>
        </div>
    </div>
    <div id="dlgReadCode" class="easyui-dialog" closed="true" modal="true" title="转换后的链接"
        style="width: 400px; height: 120px; padding: 0px">
        <textarea id="txtCode" style="width: 366px; height: 70px;"></textarea>
    </div>
</asp:Content>
