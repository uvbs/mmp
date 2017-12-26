<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="UserLevelConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.UserScoreConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hide {
            display: none;
        }

        .leveNumber {
            width: 150px;
            color: red;
            font-size: 14px;
            font-weight: bold;
        }

        tr td:first-child {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;用户等级配置
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd();" id="btnAdd">添加新等级</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 420px; padding: 35px; line-height: 30px;">
        <table width="100%">
            <%--               <tr style="display:none;">
                <td>
                    类型:
                </td>
                <td>
                   <select id="ddlLevelType" >
                   <option value="DistributionOffLine">线下分销等级</option>
                   <option value="">用户等级</option>
                   </select>
                </td>
            </tr>--%>
            <tr>
                <td>等级数字:
                </td>
                <td>
                    <input id="txtLevelNumber" type="text" style="width: 150px;" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="9" placeholder="请输入数字" />
                </td>
            </tr>
            <tr>
                <td>等级名称:
                </td>
                <td>
                    <input id="txtLevelString" type="text" style="width: 150px;" placeholder="请输入等级名称" />
                </td>
            </tr>
            <tr class="<%=GetIsHideString("FromHistoryScore",true) %>">
                <td><%=GetFieldName("FromHistoryScore","累计佣金最小值")%>
                </td>
                <td>
                    <%--<input id="txtFromHistoryScore" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="9" placeholder="请输入数字" />元--%>
                    <input id="txtFromHistoryScore" type="text" class="leveNumber" onkeyup="this.value=this.value.replace(/[^0-9.]/g,'')"   placeholder="请输入数字" />元
                </td>
            </tr>
            <tr class="<%=GetIsHideString("ToHistoryScore",true) %>">
                <td><%=GetFieldName("ToHistoryScore","累计佣金最大值")%>
                </td>
                <td>
                    <input id="txtToHistoryScore" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="9" placeholder="请输入数字" />元
                </td>
            </tr>
            <tr class="<%=GetIsHideString("Discount",false) %>">
                <td><%=GetFieldName("Discount","商城下单折扣")%>
                </td>
                <td>
                    <input id="txtDiscount" type="text" style="width: 150px;" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="9" />
                </td>
            </tr>
            <tr class="<%=GetIsHideString("DistributionRateLevel0First",true) %>">
                <td><%=GetFieldName("DistributionRateLevel0First","会员首次购买返利给自己比例")%>
                </td>
                <td>
                    <input id="txtDistributionRateLevel0First" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=GetIsHideString("DistributionRateLevel1First",true) %>">
                <td><%=GetFieldName("DistributionRateLevel1First","会员首次购买返利给上级比例")%>
                </td>
                <td>
                    <input id="txtDistributionRateLevel1First" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=GetIsHideString("DistributionRateLevel0",true) %>">
                <td><%=GetFieldName("DistributionRateLevel0","会员重复购买返利给自己比例")%>
                </td>
                <td>
                    <input id="txtDistributionRateLevel0" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=GetIsHideString("RebateMemberRate",true) %>">
                <td><%=GetFieldName("RebateMemberRate","会费奖励比例")%>
                </td>
                <td>
                    <input id="txtRebateMemberRate" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=GetIsHideString("DistributionRateLevel1",true) %>">
                <td><%=GetFieldName("DistributionRateLevel1","会员重复购买返利给上级比例")%>
                </td>
                <td>
                    <input id="txtDistributionRateLevel1" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=GetIsHideString("DistributionRateLevel1",true) %>">
                <td><%=GetFieldName("RebateScoreRate","会员购买可获得的返积分比例")%>
                </td>
                <td>
                    <input id="txtRebateScoreRate" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="4" placeholder="请输入0-9999内的数字" />%
                </td>
            </tr>
            <tr class="<%=(model.DistributionLimitLevel >=2 && GetIsShow("DistributionRateLevel2",false))?"":"hide" %>">
                <td><%=GetFieldName("DistributionRateLevel2","二级返利比例")%>
                </td>
                <td>
                    <input id="txtDistributionRateLevel2" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=(model.DistributionLimitLevel >=3 && GetIsShow("DistributionRateLevel3",false))?"":"hide" %>">
                <td><%=GetFieldName("DistributionRateLevel3","三级返利比例")%>
                </td>
                <td>
                    <input id="txtDistributionRateLevel3" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=(GetIsShow("DistributionRateLevel1Ex1",false))?"":"hide" %>">
                <td><%=GetFieldName("DistributionRateLevel1Ex1","返利购房补助比例")%>
                </td>
                <td>
                    <input id="txtDistributionRateLevel1Ex1" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=(GetIsShow("AccumulationFundRateLevel1",false))?"":"hide" %>">
                <td><%=GetFieldName("AccumulationFundRateLevel1","返利时到公积金比例")%>
                </td>
                <td>
                    <input id="txtAccumulationFundRateLevel1" type="text" class="leveNumber" onkeyup="value=value.replace(/[^\d]/g,'')" maxlength="2" placeholder="请输入0-99内的数字" />%
                </td>
            </tr>
            <tr class="<%=(GetIsShow("AwardAmount",true))?"":"hide" %>">
                <td><%=GetFieldName("AwardAmount","奖励金额")%>
                </td>
                <td>
                    <input id="txtAwardAmount" type="text" class="leveNumber" onkeyup="this.value=this.value.replace(/[^0-9.]/g,'')"  placeholder="请输入奖励金额" />
                </td>
            </tr>
            <tr class="<%=(GetIsShow("IsDisable",false))?"":"hide" %>">
                <td><%=GetFieldName("IsDisable","是否禁用")%>
                </td>
                <td>
                    <input id="chkIsDisable" type="checkbox" class="positionTop2" /><label for="chkIsDisable">禁用</label>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
<asp:Content ID="bottom" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currSelectID = 0;
        var currAction = '';
        var levelType = "<%=Request["type"]%>";
        var distributionLimitLevel = Number('<%=model.DistributionLimitLevel%>');
        var distributionGetWay = Number('<%=model.DistributionGetWay%>');
        $(function () {
            $('#grvData').datagrid(
                {
                    method: "Post",
                    url: handlerUrl,
                    queryParams: { Action: "QueryUserLevelConfig", type: levelType },
                    height: document.documentElement.clientHeight - 100,
                    pagination: true,
                    striped: true,
                    pageSize: 10,
                    rownumbers: true,
                    rowStyler: function () { return 'height:25px'; },
                    columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'LevelNumber', title: '等级', width: 5, align: 'left' },
                                { field: 'LevelString', title: '等级名称', width: 30, align: 'left' }
                                <%if (GetIsShow("DistributionRateLevel0First", true))
                                  {%>
                                ,{ field: 'DistributionRateLevel0First', title: '<%=GetFieldName("DistributionRateLevel0First","会员首次购买<br/>返利给自己比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                 <%}%>

                                <%if (GetIsShow("DistributionRateLevel1First", true))
                                  {%>
                                , { field: 'DistributionRateLevel1First', title: '<%=GetFieldName("DistributionRateLevel1First","会员首次购买<br/>返利给上级比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                 <%}%>

                                <%if (GetIsShow("DistributionRateLevel0", true))
                                  {%>
                                , { field: 'DistributionRateLevel0', title: '<%=GetFieldName("DistributionRateLevel0","会员重复购买<br/>返利给自己比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                 <%}%>
                                <%if (GetIsShow("RebateMemberRate", true))
                                  {%>
                                , { field: 'RebateMemberRate', title: '<%=GetFieldName("RebateMemberRate","会费奖励比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                 <%}%>

                                <%if (GetIsShow("DistributionRateLevel1", true))
                                  {%>
                                , { field: 'DistributionRateLevel1', title: '<%=GetFieldName("DistributionRateLevel1","会员重复购买<br/>返利给上级比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                 <%}%>
                                <%if (GetIsShow("RebateScoreRate", true))
                                  {%>
                                , { field: 'RebateScoreRate', title: '<%=GetFieldName("RebateScoreRate","会员购买<br/>返积分给自己比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                 <%}%>

                                <%if (model.DistributionLimitLevel >= 2 && GetIsShow("DistributionRateLevel2", false))
                                  {%>
                                , { field: 'DistributionRateLevel2', title: '<%=GetFieldName("DistributionRateLevel2","二级返利比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                <%}%>

                                <%if (model.DistributionLimitLevel >=3 && GetIsShow("DistributionRateLevel3", false))
                                  {%>
                                , { field: 'DistributionRateLevel3', title: '<%=GetFieldName("DistributionRateLevel3","三级返利比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                <%}%>

                                <%if (GetIsShow("FromHistoryScore", true))
                                  {%>
                                , { field: 'FromHistoryScore', title: '<%=GetFieldName("FromHistoryScore","累计佣金最小值")%>', width: 20, align: 'left' }
                                <%}%>

                                <%if (GetIsShow("ToHistoryScore",true))
                                  {%>
                                , { field: 'ToHistoryScore', title: '<%=GetFieldName("ToHistoryScore","累计佣金最大值")%>', width: 20, align: 'left' }
                                <%}%>

                                <%if (GetIsShow("DistributionRateLevel1Ex1", false))
                                  {%>
                                , { field: 'DistributionRateLevel1Ex1', title: '<%=GetFieldName("DistributionRateLevel1Ex1","返利购房补助比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                <%}%>

                                <%if (GetIsShow("AccumulationFundRateLevel1", false))
                                  {%>
                                , { field: 'AccumulationFundRateLevel1', title: '<%=GetFieldName("AccumulationFundRateLevel1","返利时到公积金比例")%>', width: 20, align: 'left', formatter: function (value) { return value + "%" } }
                                <%}%>

                                  <%if (GetIsShow("AwardAmount", false))
                                  {%>
                                , { field: 'AwardAmount', title: '<%=GetFieldName("AwardAmount","奖励金额")%>', width: 20, align: 'left', formatter: function (value) { return value} }
                                <%}%>

                                <%if (GetIsShow("IsDisable", false))
                                  {%>
                                , { field: 'IsDisable', title: '<%=GetFieldName("IsDisable","状态")%>', width: 20, align: 'left', formatter: function (value) { if (value == 1) return '<span style="color:red;">禁用</span>'; return ''; } }
                                <%}%>
                    ]]
                }
            );
            $('#dlgInput').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: currAction,
                                AutoID: currSelectID,
                                LevelType: levelType,
                                LevelNumber: $.trim($('#txtLevelNumber').val()),
                                LevelString: $.trim($('#txtLevelString').val()),
                                FromHistoryScore: $.trim($('#txtFromHistoryScore').val()),
                                ToHistoryScore: $.trim($('#txtToHistoryScore').val()),
                                Discount: $("#txtDiscount").val(),
                                DistributionRateLevel0First: $("#txtDistributionRateLevel0First").val(),
                                DistributionRateLevel0: $("#txtDistributionRateLevel0").val(),
                                DistributionRateLevel1First: $("#txtDistributionRateLevel1First").val(),
                                RebateMemberRate: $("#txtRebateMemberRate").val(),
                                DistributionRateLevel1: $("#txtDistributionRateLevel1").val(),
                                DistributionRateLevel2: $("#txtDistributionRateLevel2").val(),
                                DistributionRateLevel3: $("#txtDistributionRateLevel3").val(),
                                RebateScoreRate: $("#txtRebateScoreRate").val(),
                                DistributionRateLevel1Ex1: $("#txtDistributionRateLevel1Ex1").val(),
                                AwardAmount:$('#txtAwardAmount').val(),
                                AccumulationFundRateLevel1: $("#txtAccumulationFundRateLevel1").val(),
                                IsDisable: $("#chkIsDisable").get(0).checked ? 1 : 0
                            }
                            if (dataModel.Discount == "") {
                                dataModel.Discount = 0;
                            }
                            if (dataModel.Discount > 10) {
                                Alert('折扣需小于10');
                                return false;
                            }

                            if (dataModel.LevelNumber == '') {
                                $('#txtLevelNumber').focus();
                                return false;
                            }
                            if (dataModel.LevelString == '') {
                                $('#txtLevelString').focus();
                                return false;
                            }
                            if (dataModel.FromHistoryScore == '') {
                                dataModel.FromHistoryScore = 0;
                            }
                            if (dataModel.ToHistoryScore == '') {
                                dataModel.ToHistoryScore = 0;
                            }
                            if (!dataModel.AwardAmount) dataModel.AwardAmount=0;
                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                success: function (result) {
                                    var resp = $.parseJSON(result);
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
        });
        function ShowAdd() {
            currAction = 'AddUserLevelConfig';
            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");
            $("#dlgInput #chkIsDisable").get(0).checked=false;
        }
        function Delete() {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中等级?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoId);
                        }

                        var dataModel = {
                            Action: 'DeleteUserLevelConfig',
                            ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            success: function (result) {
                                Alert(result);
                                $('#grvData').datagrid('reload');
                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

        function ShowEdit() {
            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            currAction = 'EditUserLevelConfig';
            currSelectID = rows[0].AutoId;
            $('#txtLevelNumber').val(rows[0].LevelNumber);
            $('#txtLevelString').val(rows[0].LevelString);
            $('#txtFromHistoryScore').val(rows[0].FromHistoryScore);
            $('#txtToHistoryScore').val(rows[0].ToHistoryScore);
            $('#txtDiscount').val(rows[0].Discount);
            $("#txtDistributionRateLevel0First").val(rows[0].DistributionRateLevel0First);
            $("#txtDistributionRateLevel0").val(rows[0].DistributionRateLevel0);
            $("#txtDistributionRateLevel1First").val(rows[0].DistributionRateLevel1First);
            $("#txtRebateMemberRate").val(rows[0].RebateMemberRate);
            $("#txtDistributionRateLevel1").val(rows[0].DistributionRateLevel1);
            $("#txtDistributionRateLevel2").val(rows[0].DistributionRateLevel2);
            $("#txtDistributionRateLevel3").val(rows[0].DistributionRateLevel3);
            $("#txtRebateScoreRate").val(rows[0].RebateScoreRate);
            $("#txtDistributionRateLevel1Ex1").val(rows[0].DistributionRateLevel1Ex1);
            $("#txtAccumulationFundRateLevel1").val(rows[0].AccumulationFundRateLevel1);
            $('#txtAwardAmount').val(rows[0].AwardAmount);
            $("#chkIsDisable").get(0).checked = rows[0].IsDisable == 1 ? true : false;
            //$("#ddlLevelType").val(rows[0].LevelType);
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }


    </script>
</asp:Content>
