<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="RechargeConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CreditAcount.RechargeConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="OpenList.aspx">其他管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>信用金充值设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable">
                <tr>
                    <td style="width: 160px;" align="right" class="tdTitle">100信用金=</td>
                    <td align="left" style="width: 180px;">
                        <input id="txtRecharge" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0" value="<%=Recharge %>" />元
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Serv/Api/Admin/CreditAcount/";
        $(function () {
            bindEvent();
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

            var model = {
                recharge: $.trim($('#txtRecharge').numberbox("getValue"))
            };
            setTimeout(function () {
                if (model.Recharge == '') {
                    $('#txtRecharge').focus();
                    alert('金额不能为空', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    return;
                }
                $.ajax({
                    type: 'post',
                    url: handlerUrl + "SetRecharge.ashx",
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        $btnSave.removeClass('disabled').text('保存');
                        if (resp.status) {
                            alert(resp.msg);
                        } else {
                            alert(resp.msg);
                        }
                    }
                });

            }, 400);
        }
    </script>
</asp:Content>
