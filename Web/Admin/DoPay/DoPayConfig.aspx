<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="DoPayConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.DoPay.DoPayConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="OpenList.aspx">其他管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>支付设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">微信AppId</td>
                    <td align="left">
                        <input id="txtWXAppId" type="text" style="width: 520px;" value="<%=payConfig.WXAppId %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">微信商户Id</td>
                    <td align="left">
                        <input id="txtWXMCH_ID" type="text" style="width: 520px;" value="<%=payConfig.WXMCH_ID %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">微信支付密钥</td>
                    <td align="left">
                        <input id="txtWXPartnerKey" type="text" style="width: 520px;" value="<%=payConfig.WXPartnerKey %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle"></td>
                    <td width="*" align="center">
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a> 
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "Handler/DoPayHandler.ashx";
        init();

        function init() {
            bindEvent();
        }

        
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
                Action: 'PayConfig',
                WXAppId: $.trim($('#txtWXAppId').val()),
                WXMCH_ID: $.trim($('#txtWXMCH_ID').val()),
                WXPartnerKey: $.trim($('#txtWXPartnerKey').val())
            };
            setTimeout(function () {
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

    </script>
</asp:Content>
