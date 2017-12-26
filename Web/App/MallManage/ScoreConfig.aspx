<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ScoreConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.ScoreConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 40px;
        } 
        
        .sort
        {
            width: 780px;
        }
        .title
        {
            
         font-size:12px;   
         }
         input{
         
         height:30px;
         }


        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;会员&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>积分配置</span></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        订单获得积分：
                    </td>
                    <td>
                        下单金额:<input id="txtOrderAmount" value="<%=model.OrderAmount %>" /> 元,交易成功获得
                        <input id="txtOrderScore" value="<%=model.OrderScore %>" />积分
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        积分兑换比例：
                    </td>
                    <td>
                        使用
                        <input id="txtExchangeScore" value="<%=model.ExchangeScore %>" />积分可以抵扣
                        <input id="txtExchangeAmount" value="<%=model.ExchangeAmount %>" />元
                    </td>
                </tr>
                <tr>
                    <td align="center" class="tdTitle" valign="top" colspan="2">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; text-decoration: underline;"
                            class="button button-rounded button-primary">保存</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
    var handlerUrl = "/Handler/App/CationHandler.ashx";
    $(function () {

        $('#btnSave').click(function () {
            try {
                var model =
                    {
                        OrderAmount: $("#txtOrderAmount").val(),
                        OrderScore: $("#txtOrderScore").val(),
                        ExchangeAmount: $("#txtExchangeAmount").val(),
                        ExchangeScore: $("#txtExchangeScore").val(),
                        Action: "UpdateScoreConfig"
                    };
                $.messager.progress({ text: '正在保存...' });
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        $.messager.progress('close');
                        Alert(resp.Msg);

                    }
                });

            } catch (e) {
                Alert(e);
            }


        });
    })

    </script>
</asp:Content>
