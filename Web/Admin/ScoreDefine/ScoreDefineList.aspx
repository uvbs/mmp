<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ScoreDefineList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.ScoreDefine.ScoreDefineList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        #txtBaseRateValue, #txtBaseRateScore {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%=moduleName %>规则
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            
            <%if (bllKeyValueData.WebsiteOwner == "songhe")
                { %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" id="btnAddItem" onclick="AddItem()">新增规则</a>
            <%} %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" id="btnEditItem" onclick="EditItem()">修改规则</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 500px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>类型:
                </td>
                <td>
                    <%=selectOptionHtml %>
                </td>
            </tr>
            <tr>
                <td>增减数额:
                </td>
                <td>
                    <input id="txtScore" type="text" class="easyui-numberbox" style="width: 80px;" data-options="min:-100,max:500,precision:0" />最小-100，最大500
                </td>
            </tr>
            <tr>
                <td>增加每日上限<%=moduleName %>:
                </td>
                <td>
                    <input id="txtDayLimit" type="text" class="easyui-numberbox" style="width: 80px;" data-options="min:-1,max:500,precision:0" />小于0时不限制，最小-1，最大500
                </td>
            </tr>
            <tr>
                <td>增加总上限<%=moduleName %>:
                </td>
                <td>
                    <input id="txtTotalLimit" type="text" class="easyui-numberbox" style="width: 80px;" data-options="min:-1,max:50000,precision:0" />小于0时不限制，最小-1，最大50000
                </td>
            </tr>
            <tr>
                <td>启用/禁用:
                </td>
                <td>
                    <select id="selectHide" style="width: 250px;">
                        <option value="0" selected="selected">启用</option>
                        <option value="1">禁用</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>描述:
                </td>
                <td>
                    <input id="txtDescription" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>排序:
                </td>
                <td>
                    <input id="txtOrderNum" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" />
                </td>
            </tr>
            <tr <%=isHideEx1 == 1? "style=\"display:none\"":"" %>>
                <td>关联ID串:
                </td>
                <td>
                    <input id="txtEx1" type="text" style="width: 250px;" />
                    ,号分隔，0为所有
                   
                </td>
            </tr>
            <tr <%=isHideEvent == 1? "style=\"display:none\"":"" %>>
                <td><%= moduleName %>事件名称:
                </td>
                <td>
                    <input id="txtScoreEvent" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr <%=isHideEvent == 1? "style=\"display:none\"":"" %>>
                <td>基础比例:
                </td>
                <td>
                    <input id="txtBaseRateValue" type="text" class="easyui-numberbox" />获得<input id="txtBaseRateScore" type="text" class="easyui-numberbox" /><%= moduleName %>
                </td>
            </tr>
        </table>
        <div id="dlgEx" class="easyui-window" closed="true" title="" style="width: 550px; padding: 15px;">

            <table id="grvDataEx" fitcolumns="true">
            </table>

        </div>
    </div>

    <div id="dlgInfoEx" class="easyui-dialog" closed="true" title="" style="width: 400px; padding: 15px;">
        <table width="100%">

            <tr>
                <td>开始时间:</td>
                <td>
                    <input class="easyui-datetimebox" style="width: 150px;" id="txtBeginTime" /></td>
            </tr>

            <tr>
                <td>结束时间:</td>
                <td>
                    <input class="easyui-datetimebox" style="width: 150px;" id="txtEndTime" /></td>
            </tr>

            <tr>
                <td>比例:</td>
                <td>
                    <input id="txtExRateValue" type="text" style="width: 50px;" class="easyui-numberbox" />事件值 获得<input id="txtExRateScore" type="text" style="width: 50px;" class="easyui-numberbox" />积分</td>
            </tr>

        </table>


    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/ScoreDefineListHandler.ashx";
        var ActionType = "";
        var ScoreId = 0;//积分规则Id
        var ScoreExId = 0;//积分扩展规则Id
        var SelectIndex = 0;
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "getDefineList" },
                height: document.documentElement.clientHeight - 112,
                pagination: true,
                striped: true,
                rownumbers: true,
                singleSelect: true,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                    { title: 'ck', width: 5, checkbox: true },
//                    { field: 'ScoreType', title: '类型编码', width: 50, align: 'left', formatter: FormatterTitle },
                    { field: 'Name', title: '名称', width: 50, align: 'left', formatter: FormatterTitle },
                    { field: 'Score', title: '增减数额', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'DayLimit', title: '每日上限', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'TotalLimit', title: '总上限', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'Description', title: '描述', width: 100, align: 'left', formatter: FormatterTitle },
                    <% if (isHideEx1 == 0){%>
                    { field: 'Ex1', title: '关联ID', width: 100, align: 'left', formatter: FormatterTitle },
                    <%}%>
                    {
                        field: 'IsHide', title: '隐藏', width: 30, align: 'left', formatter: function (value, rowData) {
                            var str = value == 1 ? "是" : "否";
                            return str;
                        }
                    },
                    { field: 'OrderNum', title: '排序', width: 30, align: 'left', formatter: FormatterTitle },
                            <% if(isHideEvent == 0){%>
                    { field: 'ScoreEvent', title: '积分事件', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'BaseRateValue', title: '事件值', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'BaseRateScore', title: '事件值对应积分', width: 30, align: 'left', formatter: FormatterTitle },
                    <%}%>
                    {
                        field: 'action', title: '操作', width: 50, align: 'left', formatter: function (value, rowData) {
                            var str = new StringBuilder();
                            <% if(isHideEvent == 0){%>
                            str.AppendFormat('<a class="l-btn" href="javascript:void(0);" onclick="ExMgr(\'{0}\')"><span class="l-btn-left"><span class="l-btn-text">扩展</span></span><br/></a> &nbsp;', rowData['ScoreId']);
                            <%}%>
                            str.AppendFormat('<a href="javascript:void(0);" onclick="DelItem(\'{0}\',\'{1}\')"><img alt="删除" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a> ', rowData['ScoreId'], rowData['Name']);
                            return str.ToString();
                        }
                    }
                ]]
            });
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        var dataModel = {
                            Action: "putDefine",
                            score_id: ScoreId,
                            type: $('#ddlType').val(),
                            score: $.trim($('#txtScore').val()),
                            limit: $.trim($('#txtDayLimit').val()),
                            total_limit: $.trim($('#txtTotalLimit').val()),
                            hide: $.trim($('#selectHide').val()),
                            summary: $.trim($('#txtDescription').val()),
                            order: $.trim($('#txtOrderNum').val()),
                            ex1: $.trim($('#txtEx1').val()),
                            score_event: $.trim($('#txtScoreEvent').val()),
                            base_rate_value: $.trim($('#txtBaseRateValue').val()),
                            base_rate_score: $.trim($('#txtBaseRateScore').val())
                        }

                        if (dataModel.type == '' || dataModel.type == 0) {
                            Alert("类型不能为空!");
                            return;
                        }

                        if (dataModel.score == '') {
                            Alert("积分不能为空!");
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

            $('#dlgEx').dialog({
                //
                buttons: [{
                    text: '关闭',
                    handler: function () {
                        $('#dlgEx').dialog('close');
                    }
                }]
                //

            });
            $('#dlgInfoEx').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        var dataModel = {
                            Action: "AddEditDefineEx",
                            id: ScoreExId,
                            score_id: ScoreId,
                            begin_time: $('#txtBeginTime').datetimebox("getValue"),
                            end_time: $('#txtEndTime').datetimebox("getValue"),
                            rate_value: $.trim($('#txtExRateValue').val()),
                            rate_score: $.trim($('#txtExRateScore').val())
                        }
                        if (dataModel.begin_time == "") {
                            Alert("开始时间必填");
                            return false;
                        }
                        if (dataModel.end_time == "") {
                            Alert("结束时间必填");
                            return false;
                        }
                        var rowData = $("#grvDataEx").datagrid("getRows");
                        if (!checkTimeInData(rowData, dataModel.begin_time, dataModel.end_time,SelectIndex)) {
                            return false;
                        }
                        if (dataModel.rate_value == "") {
                            Alert("值必填");
                            return false;
                        }
                        if (dataModel.rate_score == "") {
                            Alert("积分必填");
                            return false;
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    $('#dlgInfoEx').dialog('close');
                                    $('#grvDataEx').datagrid('reload');
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
                        $('#dlgInfoEx').dialog('close');
                    }
                }]
            });


        });







        function EditItem() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            ActionType = "Edit";
            $('#txtScore').numberbox("setValue", rows[0].Score);
            $('#txtDayLimit').numberbox("setValue", rows[0].DayLimit);
            $('#txtTotalLimit').numberbox("setValue", rows[0].TotalLimit);
            $('#ddlType').val(rows[0].ScoreType);
            $('#selectHide').val(rows[0].IsHide);
            $('#txtDescription').val(rows[0].Description);
            $('#txtOrderNum').numberbox("setValue", rows[0].OrderNum);
            $('#txtEx1').val(rows[0].Ex1);
            $('#txtScoreEvent').val(rows[0].ScoreEvent);
            $('#txtBaseRateValue').numberbox("setValue", rows[0].BaseRateValue);
            $('#txtBaseRateScore').numberbox("setValue", rows[0].BaseRateScore);
            ScoreId = rows[0].ScoreId;
            ddlType.readOnly = true;

            $('#dlgInfo').dialog({ title: '规则修改' });
            $('#dlgInfo').dialog('open');
        }

        function AddItem() {
            ActionType = "Add";
            ScoreId = 0;
            ScoreExId = 0;
            $('#txtScore').numberbox("setValue", 0);
            $('#txtDayLimit').numberbox("setValue", -1);
            $('#txtTotalLimit').numberbox("setValue", -1);
            $('#ddlType').val("");
            $('#txtName').val("");
            $('#selectHide').val(0);
            $('#txtDescription').val("");
            $('#txtOrderNum').numberbox("setValue", 0);
            $('#txtEx1').val(0);
            $('#txtScoreEvent').val("");
            $('#txtBaseRateValue').val("");
            $('#txtBaseRateScore').val("");
            ddlType.readOnly = false;
            $('#dlgInfo').dialog({ title: '规则新增' });
            $('#dlgInfo').dialog('open');

        }

        function DelItem(scoreId, modelName) {
            $.messager.confirm('系统提示', '确定删除[' + modelName + ']？', function (o) {
                if (o) {
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: { Action: "delDefine", scoreId: scoreId },
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
            });
        }

        function DelItemEx(autoId) {
            $.messager.confirm('系统提示', '确定删除？', function (o) {
                if (o) {
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: { Action: "DelDefineEx", id: autoId },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {

                                $('#grvDataEx').datagrid('reload');
                            }
                            else {
                                Alert(resp.Msg);
                            }
                        }
                    });
                }
            });
        }

        function ExMgr(scoreId) {

            ScoreId = scoreId;


            $('#dlgEx').dialog('open');
            $("#dlgEx").window("move", { top: $(document).scrollTop() + ($(window).height() - 400) * 0.5 });
            //
            $('#grvDataEx').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "GetDefineListEx", ScoreId: ScoreId },
                height: 300,
                pagination: true,
                striped: true,
                pageSize: 1000,
                rownumbers: true,
                singleSelect: true,
                rowStyler: function () { return 'height:25px'; },
                toolbar: ["添加", {
                    id: '',
                    text: '添加扩展',
                    iconCls: 'icon-add2',
                    handler: function () {
                        SelectIndex = -1;
                        $(txtExRateScore).val("");
                        $(txtExRateValue).val("");

                        $('#dlgInfoEx').dialog({ title: '扩展规则' });
                        $('#dlgInfoEx').dialog('open');


                    }
                },
                "编辑", {
                    id: '',
                    text: '编辑',
                    iconCls: 'icon-edit',
                    handler: function () {
                        //
                        var rows = $('#grvDataEx').datagrid('getSelections'); //获取选中的行
                        if (!EGCheckIsSelect(rows)) {
                            return;
                        }
                       SelectIndex= $('#grvDataEx').datagrid('getRowIndex', $("#grvDataEx").datagrid('getSelected'));

                        $('#txtExRateScore').numberbox("setValue", rows[0].RateScore);
                        $('#txtExRateValue').numberbox("setValue", rows[0].RateValue);
                        $('#txtBeginTime').datetimebox("setValue", rows[0].BeginTimeStr);
                        $('#txtEndTime').datetimebox("setValue", rows[0].EndTimeStr);
                        ScoreExId = rows[0].AutoId;
                        //

                        $('#dlgInfoEx').dialog({ title: '扩展规则' });
                        $('#dlgInfoEx').dialog('open');

                    }
                }],
                columns: [[
                    { field: 'BeginTime', title: '起始时间', width: 50, align: 'left', formatter: FormatDate },
                    { field: 'EndTime', title: '结束时间', width: 50, align: 'left', formatter: FormatDate },
                    { field: 'RateValue', title: '事件值', width: 20, align: 'left', formatter: FormatterTitle },
                    { field: 'RateScore', title: '对应积分', width: 20, align: 'left', formatter: FormatterTitle },
                    {
                        field: 'action', title: '操作', width: 15, align: 'left', formatter: function (value, rowData) {
                            var str = new StringBuilder();
                            str.AppendFormat('<a href="javascript:void(0);" onclick="DelItemEx(\'{0}\')"><img alt="删除" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_delete.gif" title="删除" /></a>', rowData['AutoId']);
                            return str.ToString();
                        }
                    }
                ]]
            });
            //



        }


        function checkTimeInData(data, startDateStr, endDateStr,index) {
            var hasRp = false;
            var startDate = new Date(startDateStr);
            var endDate = new Date(endDateStr);
            for (var i = 0; i < data.length; i++) {
                if (index==i) {
                    continue;
                }
                var dstart = new Date(data[i].BeginTimeStr);
                var dend = new Date(data[i].EndTimeStr);
                if (!((startDate >= dend && endDate > dend) || (startDate < dstart && endDate <= dstart))) {
                    hasRp = true;
                    break;
                }


            }
            if (hasRp) {
                $.messager.alert("提示", "所选时间不能与已有时间交集");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
