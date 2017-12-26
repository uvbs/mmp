<%@ Page Title="任务管理" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ActivityManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Data.ActivityManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>数据管理</span>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div class="center">
        <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
            <div style="margin-bottom: 5px">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd()">
                                添加</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit()">
                                    编辑</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">
                                        删除</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="BatChangState(1)">
                                            批量开启</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                                                onclick="BatChangState(0)">批量关闭</a> <a href="javascript:;" class="easyui-linkbutton"
                                                    data-options="plain:true,iconCls:'icon-excel'" id="btnExportToFile">导出报名数据</a>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                </table>
                <div>
                    <span style="font-size: 12px; font-weight: normal">任务名称:</span>
                    <input id="txtActivityNames" style="width: 200px" />
                    <span style="font-size: 12px; font-weight: normal">任务ID:</span>
                    <input id="txtActivityId" style="width: 100px" />
                    <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
                </div>
            </div>
        </div>
        <table id="grvData" cellspacing="0" cellpadding="0" fitcolumns="true">
            <thead>
                <tr>
                    <th field="ck" width="5" checkbox="true">
                    </th>
                    <th field="ActivityID" align="center" width="10">
                        任务ID
                    </th>
                    <th field="ActivityName" align="left" width="20">
                        任务名称
                    </th>
                    <th field="ActivityStatus" align="center" formatter="changestate" width="10">
                        任务状态
                    </th>
                    <th field="ActivityDescription" align="center" width="20">
                        任务说明
                    </th>
                     <th field="SignInCount" width="20" align="center" formatter="formartsignincount">
                        报名
                    </th>
                    <th field="op" width="20" align="center" formatter="operate">
                        操作
                    </th>
                </tr>
            </thead>
        </table>
        <div id="dlgInput" easyui-dialog modal="true" closed="true" title="" style="width: 380px;
        padding: 15px;line-height:15px;">
            <table style="margin: auto;">
                <tr>
                    <td align="right">
                        任务名称:
                    </td>
                    <td style="text-align: left">
                        <input type="text" id="txtActivityName" style="width: 250px;" class="easyui-validatebox"
                            required="true" missingmessage="请输入任务名称" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        任务状态:
                    </td>
                    <td style="text-align: left">
                        <input type="radio" id="rdoActivityStatus1" name="rd" checked="checked" />
                        <label for='rdoActivityStatus1'>
                            开启</label>
                        <input type="radio" id="rdoActivityStatus0" name="rd" />
                        <label for='rdoActivityStatus0'>
                            关闭</label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        时间:
                    </td>
                    <td style="text-align: left">
                        <input type="text" id="txtActivityDate" class="easyui-datetimebox" required="true"
                            validtype="datetime" missingmessage="请输入正确的时间格式" invalidmessage="请输入正确的时间格式"
                            style="width: 250px;" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        地点:
                    </td>
                    <td style="text-align: left">
                        <input type="text" id="txtActivityAddress" style="width: 250px;" />
                    </td>
                </tr>
                    <tr>
                    <td align="right">
                        报名通知到以下客服:
                    </td>
                    <td style="text-align: left">
                        <select id="ddlActivityNoticeKeFu" style="width:200px;">
                        <option value="">无</option>
                        <%=sbActivityNoticeKeFuList.ToString()%>
                        </select>
                        
                    </td>
                   
                </tr>
                <tr>
                <td></td>
                <td>
                <span>还没有客服?<a  href="/App/PubMgr/WXKeFuManage.aspx" style="color:Blue">点击添加</a></span>
                </td>
                </tr>
                <tr>
                    <td align="right">
                        网址:
                    </td>
                    <td style="text-align: left">
                        <input type="text" id="txtActivityWebsite" style="width: 250px;" />
                    </td>
                </tr>
                <tr style="display:none;">
                    <td align="right" >
                        短信通知内容:
                    </td>
                    <td style="text-align: left">
                        <textarea id="txtConfirmSMSContent" style="width: 250px; height: 50px"></textarea>
                    </td>
                   
                </tr>
                <tr>
                    <td align="right">
                        说明:
                    </td>
                    <td style="text-align: left">
                        <textarea id="txtActivityDescription" style="width: 250px; height: 50px"></textarea>
                    </td>
                </tr>
                <tr style="display:none;">
                    <td align="right" >
                        IP访问限制次数:
                    </td>
                    <td style="text-align: left">
                        <input type="text" id="txtLimitCount" style="width: 100px;" />
                    </td>
                </tr>
                <tr style="display:none;">
                    <td align="right" >
                        管理员通知手机号:
                    </td>
                    <td style="text-align: left">
                        <input type="text" id="txtAdminPhone" style="width: 250px;" />
                    </td>
                    
                </tr>
                <tr style="display:none;">
                    <td align="right" >
                        管理员通知内容:
                    </td>
                    <td style="text-align: left">
                        <textarea id="txtAdminSMS" style="width: 250px; height: 50px"></textarea>
                    </td>
                   
                </tr>
                
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
   <% ZentCloud.BLLJIMP.Model.UserInfo currUser =ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel(); %>
    <script type="text/javascript">
        var dataGrid;
        //处理文件路径
        var handlerUrl = "/Handler/Activity/ActivityManage.ashx";
        var currAction = '';
        var currSelectId = 0;

        //加载
        $(function () {

            //grid加载数据
            dataGrid = $("#grvData").datagrid({
                method: "Post",
                url: handlerUrl,
                pageSize: 10,
                height: document.documentElement.clientHeight - 112,
                fitCloumns: true,
                nowrap: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                queryParams: { Action: "QueryActivity" }
            });

            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: currAction,
                                ActivityID: currSelectId,
                                ActivityName: $.trim($(txtActivityName).val()),
                                ActivityDate: $(txtActivityDate).datetimebox("getValue"),
                                ActivityAddress: $(txtActivityAddress).val(),
                                ActivityWebsite: $(txtActivityWebsite).val(),
                                ActivityDescription: $(txtActivityDescription).val(),
                                ConfirmSMSContent: $(txtConfirmSMSContent).val(),
                                ActivityStatus: rdoActivityStatus0.checked ? 0 : 1,
                                LimitCount: $(txtLimitCount).val(),
                                AdminPhone: $(txtAdminPhone).val(),
                                AdminSMSContent: $(txtAdminSMS).val(),
                                ActivityNoticeKeFuId: $(ddlActivityNoticeKeFu).val()
                            }

                            if (dataModel.ActivityName == '') {
                                $(txtActivityName).focus();
                                return;
                            }

                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status == 1) {
                                        Show(resp.Msg);
                                        $('#dlgInput').dialog('close');
                                        $('#grvData').datagrid('reload');
                                    }
                                    else {
                                        Alert(resp.Msg);
                                    }


                                }
                            });

                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgInput').dialog('close');
                    }
                }]
            });

            //查询按钮点击绑定
            $("#btnSearch").click(function () {
                var ActivityName = $("#txtActivityNames").val();
                var ActivityId = $("#txtActivityId").val();
                dataGrid.datagrid({
                    url: handlerUrl,
                    queryParams: { Action: "QueryActivity", ActivityName: ActivityName, ActivityId: ActivityId }
                });
            });




            //导出文件
            $(btnExportToFile).click(function () {

                var rows = dataGrid.datagrid('getSelections');
                if (!CheckIsSelect(rows)) {
                    return;
                }
                // DownLoadData(rows[0].ActivityID);
                $.messager.confirm('系统提示', '确认导出数据？', function (o) {
                    if (o) {
                        for (var i = 0; i < rows.length; i++) {
                            DownLoadData(rows[i].ActivityID);
                        }
                    }
                });

            });


        });

        //判断是否选中了多行
        function CheckNoSelectMultiRow(r) {
            var num = r.length;
            if (num == 0) {
                $.messager.alert('系统提示', "请选择一条记录进行操作！", "warning");
                return false;
            }
            if (num > 1) {
                $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
                return false;
            }
            return true;
        }

        //判断是否选中行
        function CheckIsSelect(r) {
            var num = r.length;
            if (num == 0) {
                $.messager.alert('系统提示', "请至少选择一条记录进行操作！", "warning");
                return false;
            }
            return true;
        }


        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].ActivityID);
            }
            return ids;
        }

        //窗体清除数据
        function ClearWinData() {
            ClearWinDataByTag("input|textarea");
            $(rdoActivityStatus1).attr("checked", true);
        }

        function ClearWinDataByTag(o) {
            var arr = o.split('|');
            for (var i = 0; i < arr.length; i++) {
                $(dlgInput).find(arr[i]).val("");
            }
        }

        //验证长时间
        function strDateTime(str) {
            var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
            var r = str.match(reg);
            if (r == null) return false;
            var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
            return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
        }


        //显示添加信息输入框
        function ShowAdd() {
            ClearWinData();
            $(txtLimitCount).val("100");
            currAction = 'AddActivity';
            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');



        }

        //显示编辑信息输入框
        function ShowEdit() {
            ClearWinData();
            var rows = dataGrid.datagrid('getSelections');
            if (!CheckNoSelectMultiRow(rows)) {
                return;
            }

            currAction = 'EditActivity';
            currSelectId = rows[0].ActivityID;

            //加载编辑数据
            //$("txtActivityName")
            $(txtActivityName).val(rows[0].ActivityName);
            if (rows[0].ActivityStatus == 1) {
                $(rdoActivityStatus1).attr("checked", true);
            }
            else {
                $(rdoActivityStatus0).attr("checked", true);
            }

            $(txtActivityDate).datetimebox("setValue", rows[0].ActivityDate);
            $(txtActivityAddress).val(rows[0].ActivityAddress);
            $(txtActivityWebsite).val(rows[0].ActivityWebsite);
            $(txtConfirmSMSContent).val(rows[0].ConfirmSMSContent);
            $(txtActivityDescription).val(rows[0].ActivityDescription);
            $(txtLimitCount).val(rows[0].LimitCount);
            $(txtAdminPhone).val(rows[0].AdminPhone);
            $(txtAdminSMS).val(rows[0].AdminSMSContent);
            $(ddlActivityNoticeKeFu).val(rows[0].ActivityNoticeKeFuId);
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');

        }




        //删除数据
        function Delete() {
            var rows = dataGrid.datagrid('getSelections'); //获取选中的行

            if (!CheckIsSelect(rows)) {
                return;
            }



            $.messager.confirm("系统提示", "确定删除选中任务?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteActivity", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            if (result == "true") {
                                messager('系统提示', "删除成功！");
                                dataGrid.datagrid('reload');
                                return;
                            }
                            else {
                                $.messager.alert(result);
                            }

                        }
                    });


                }
            });


        }

        //批量更改启用禁用状态
        function BatChangState(v) {
            var rows = dataGrid.datagrid('getSelections'); //获取选中的行

            if (!CheckIsSelect(rows)) {
                return;
            }

            var ids = GetRowsIds(rows).join(','); //id集合字符串

            var msg = v == 1 ? "确定开启选中任务?" : "确定关闭选中任务?";

            $.messager.confirm("系统提示", msg, function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "BatChangState", ids: ids, ActivityStatus: v },
                        success: function (result) {
                            if (result == "true") {
                                messager('系统提示', "更新成功！");
                                dataGrid.datagrid('reload');
                                return;
                            }
                            $.messager.alert("更新失败", result);
                        }

                    });
                }
            });


        }



        function changestate(value) {

            if (value == "0") {
                return "<font color='red'>关闭</font>"
            }
            else if (value == "1") {
                return "<font color='green'>开启</font>"
            }

        }
        function operate(value, rowData) {

            var str = new StringBuilder();
            str.AppendFormat('<a href="/App/Cation/ActivitySignUpTableManage.aspx?ActivityID={0}" title="查看字段">查看字段</a>', rowData.ActivityID);
            str.AppendFormat('&nbsp;');
            str.AppendFormat('<a href="/App/Cation/ActivitySignUpDataManage.aspx?ActivityID={0}" title="查看报名数据">查看报名数据</a>', rowData.ActivityID);
            return str.ToString();

        }
        function formartsignincount(value, rowData) {

            var str = new StringBuilder();
            str.AppendFormat('<a href="/App/Cation/ActivitySignUpDataManage.aspx?ActivityID={0}" title="查看数据">{1}</a>', rowData.ActivityID, value);

            return str.ToString();

        }



        function DownLoadData(ActivityID) {
            window.open('/Handler/Activity/ActivityData.ashx?Action=DownLoadActivityData&ActivityID=' + ActivityID);

        }
        
    </script>
</asp:Content>
