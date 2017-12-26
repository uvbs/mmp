<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ScoreRechargeConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.ScoreDefine.ScoreRechargeConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;充值设置
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable">
                <tr>
                    <td style="width: 160px;" align="right" class="tdTitle">100<%=moduleName %>=</td>
                    <td align="left" style="width: 180px;">
                        <input id="txtRecharge" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" value="<%=Recharge %>" />元
                    </td>
                    <td valign="top" rowspan="5" style="height: 400px; <%=isHideVIPInterestID == 1? "display:none":"" %>">
                        VIP权益：<%=VIPInterestID%><br />
                        <div id="txtEditor" style="width: 99%; height: 400px;">
                            <%=VIPInterestDescription%>
                        </div>
                        <%--<div id="toolbar" class="pageTopBtnBg" style="height: auto;">
                            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" id="btnAddItem" onclick="ShowAdd()">新增充值金额</a>
                            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" id="btnEditItem" onclick="ShowEdit()">修改充值金额</a>
                            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" id="btnDelItem" onclick="Delete()">删除充值金额</a>
                        </div>
                        <table id="grvData" fitcolumns="true">
                        </table>--%>
                    </td>
                </tr>
                <tr <%=isShowSendNotice == 1? "" :"style=\"display:none\""%>>
                    <td align="right" class="tdTitle">短信通知消费：</td>
                    <td align="left">
                        <input id="txtSendNoticePrice" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" value="<%=SendNoticePrice %>" /><%=moduleName %>
                    </td>
                </tr>
                <tr <%=isShowMinScore == 1? "" :"style=\"display:none\""%>>
                    <td align="right" class="tdTitle">最低账户余额：</td>
                    <td align="left">
                        <input id="txtMinScore" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" value="<%=MinScore %>" /><%=moduleName %>
                    </td>
                </tr>
                <tr <%=isShowMinWithdrawCashScore == 1? "" :"style=\"display:none\""%>>
                    <td align="right" class="tdTitle">最低提现数额：</td>
                    <td align="left">
                        <input id="txtMinWithdrawCashScore" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" value="<%=MinWithdrawCashScore %>" /><%=moduleName %>
                    </td>
                </tr>
                <tr <%=isHideVIPPrice == 1? "style=\"display:none\"":"" %>>
                    <td align="right" class="tdTitle">充值VIP价格（带发票）：</td>
                    <td align="left">
                        <input id="txtVIPPrice" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" value="<%=VIPPrice %>" />元
                    </td>
                </tr>
                <tr <%=isHideVIPPrice0 == 1? "style=\"display:none\"":"" %>>
                    <td align="right" class="tdTitle">充值VIP价格（无发票）：</td>
                    <td align="left">
                        <input id="txtVIPPrice0" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" value="<%=VIPPrice0 %>" />元
                    </td>
                </tr>
                <tr <%=isHideVIPDatelong == 1? "style=\"display:none\"":"" %>>
                    <td align="right" class="tdTitle">充值VIP时长：</td>
                    <td align="left">
                        <input id="txtVIPDatelong" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" value="<%=VIPDatelong %>" />个月
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
    </div>
    <%--<div id="dlgInput" title="新增充值金额" closed="true" modal="true" style="width: 420px; padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td>
                    充值金额:
                </td>
                <td>
                    <input id="txtDataValue" type="text" style="width: 250px;"  class="easyui-numberbox" data-options="min:1,precision:0" />
                </td>
            </tr>
            <tr>
                <td>
                    排序:
                </td>
                <td>
                    <input id="txtOrderNum" type="text" style="width: 250px;"  class="easyui-numberbox" data-options="min:0,precision:0"/>
                </td>
            </tr>
        </table>
    </div>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "Handler/ScoreDefineListHandler.ashx",
        currUserId = '<%=UserId %>',
        currAcvityID = '<%=VIPInterestID %>',
        editor,
        currSelectID = '0';
        currAction = "";

        $(function () {
            if (currUserId == '') {
                window.parent.location.href = "/login";
                return;
            }
            bindEvent();

            KindEditor.ready(function (K) {
                editor = K.create('#txtEditor', {
                    uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                    items: [
                        'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                        'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                        'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'
                    ],
                    filterMode: false
                });
            });

            //$('#grvData').datagrid({
            //    method: "Post",
            //    url: handlerUrl,
            //    queryParams: { Action: "getRechargePriceList" },
            //    striped: true,
            //    pageSize: 20,
            //    height:300,
            //    rownumbers: true,
            //    rowStyler: function () { return 'height:25px'; },
            //    columns: [[
            //        { title: 'ck', width: 5, checkbox: true },
            //        { field: 'DataValue', title: '充值金额', width: 50, align: 'left', formatter: FormatterTitle },
            //        { field: 'OrderBy', title: '排序', width: 30, align: 'left', formatter: FormatterTitle }
            //    ]]
            //});
            //$('#dlgInput').dialog({
            //    buttons: [{
            //        text: '保存',
            //        handler: function () {
            //            var dataModel = {
            //                Action: currAction,
            //                AutoId: currSelectID,
            //                DataValue: $('#txtDataValue').numberbox("getValue"),
            //                OrderNum: $('#txtOrderNum').numberbox("getValue")
            //            }

            //            if (dataModel.DataValue == '') {
            //                Alert("请输入金额!");
            //                return;
            //            }

            //            if (dataModel.OrderNum == '') {
            //                Alert("请输入排序!");
            //                return;
            //            }

            //            $.ajax({
            //                type: 'post',
            //                url: handlerUrl,
            //                data: dataModel,
            //                dataType: "json",
            //                success: function (resp) {
            //                    if (resp.Status == 1) {
            //                        $('#dlgInput').dialog('close');
            //                        $('#grvData').datagrid('reload');
            //                    }
            //                    else {
            //                        Alert(resp.Msg);
            //                    }
            //                }
            //            });

            //        }
            //    }, {
            //        text: '取消',
            //        handler: function () {
            //            $('#dlgInfo').dialog('close');
            //        }
            //    }]
            //});
        });

        function bindEvent() {
            $("#btnSave").on('click', function () {
                saveData();
            });
        }

        function saveData() {
            var $btnSave = $('#btnSave'), $btnReset = $('#btnReset');
            if ($btnSave.hasClass('disabled ')) {
                return;
            }

            $btnSave.addClass('disabled').text('正在处理...');
            $btnReset.addClass('disabled');

            var model = {
                Recharge: $.trim($('#txtRecharge').numberbox("getValue")),
                
                <% if (isShowSendNotice == 1){%>
                    SendNoticePrice: $.trim($('#txtSendNoticePrice').numberbox("getValue")),
                <% } %>
                <% if (isShowMinScore == 1)
                   {%>
                MinScore: $.trim($('#txtMinScore').numberbox("getValue")),
                <% } %>
                <% if (isShowMinWithdrawCashScore == 1)
                   {%>
                MinWithdrawCashScore: $.trim($('#txtMinWithdrawCashScore').numberbox("getValue")),
                <% } %>
                <% if(isHideVIPPrice == 0){%>
                VIPPrice: $.trim($('#txtVIPPrice').numberbox("getValue")),
                <% } %>
                <% if(isHideVIPPrice0 == 0){%>
                VIPPrice0: $.trim($('#txtVIPPrice0').numberbox("getValue")),
                <% } %>
                <% if(isHideVIPDatelong == 0){%>
                VIPDatelong: $.trim($('#txtVIPDatelong').numberbox("getValue")),
                <% } %>
                <% if(isHideVIPInterestID == 0){%>
                VIPInterestID: currAcvityID,
                VIPInterestDescription: editor.html(),
                <% } %>
                Action: 'EditRechargeConfig'
            };
            setTimeout(function () {
                if (model.Recharge == '') {
                    $('#txtRecharge').focus();
                    alert('<%=moduleName%>不能为空', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                <% if (isShowSendNotice == 1){%>
                if (model.SendNoticePrice == '') {
                    $('#txtSendNoticePrice').focus();
                    alert('请填写短信通知消费多少<%=moduleName%>', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                <% } %>
                <% if (isShowMinScore == 1)
                   {%>
                if (model.MinScore == '') {
                    $('#txtMinScore').focus();
                    alert('最低剩余多少<%=moduleName%>', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                <% } %>
                <% if (isShowMinWithdrawCashScore == 1)
                   {%>
                if (model.MinWithdrawCashScore == '') {
                    $('#txtMinWithdrawCashScore').focus();
                    alert('最低提现多少<%=moduleName%>', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                <% } %>
                <% if(isHideVIPPrice == 0){%>
                if (model.VIPPrice == '') {
                    $('#txtVIPPrice').focus();
                    alert('请填写VIP价格', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                <% } %>
                <% if(isHideVIPDatelong == 0){%>
                if (model.VIPDatelong == '') {
                    $('#txtVIPDatelong').focus();
                    alert('请填写VIP时长', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                <% } %>
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        $btnReset.removeClass('disabled');
                        $btnSave.removeClass('disabled').text('保存');
                        if (resp.Status == 1) {
                            alert(resp.Msg);
                        } else {
                            alert(resp.Msg);
                        }
                    }
                });

            }, 400);
        }

        function ShowAdd() {
            currAction = 'AddRechargePrice';
            currSelectID = 0;
            var List = $('#grvData').datagrid('getRows');
            if (List.length >= 10) {
                $.messager.alert("提示","最多只能添加10个选项");
                return
            }
            $('#txtDataValue').numberbox("setValue", "");
            $('#txtOrderNum').numberbox("setValue","");

            $('#dlgInput').dialog({ title: '新增充值金额' });
            $('#dlgInput').dialog('open');
        }

        function Delete() {
            try {
                var rows = $('#grvData').datagrid('getSelections');
                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                    if (r) {
                        var ids = [];
                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoId);
                        }
                        var dataModel = {
                            Action: 'DeleteRechargePrice',
                            ids: ids.join(',')
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    Show("删除完成");
                                    $('#grvData').datagrid('reload');
                                } else {
                                    Show("删除失败");
                                }
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
            if (!EGCheckNoSelectMultiRow(rows))
                return;

            currAction = 'UpdateRechargePrice';
            currSelectID = rows[0].AutoId;

            $('#txtDataValue').numberbox("setValue", rows[0].DataValue);
            $('#txtOrderNum').numberbox("setValue", rows[0].OrderBy);

            $('#dlgInput').dialog({ title: '修改充值金额' });
            $('#dlgInput').dialog('open');
        }

    </script>
</asp:Content>
