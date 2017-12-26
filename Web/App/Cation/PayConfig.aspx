<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PayConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.PayConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
input 
{
    height: 35px;
    border: 1px solid #d5d5d5;
    border-radius: 5px;
    background-color: #fefefe;
   
}
textarea
{
    
    height: 35px;
    border: 1px solid #d5d5d5;
    border-radius: 5px;
    background-color: #fefefe;

}

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;支付配置
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <br />

        <table width="100%" id="tbMain">

            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                   微信 AppId：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtWXAppId" class="" style="width: 350px;" value="<%=model.WXAppId %>" />
                   
                </td>
            </tr>
             <tr>
                <td style="width: 200px;" align="right" valign="middle">
                   微信商户号：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtWXMCHID" class="" style="width: 350px;" value="<%=model.WXMCH_ID%>"  onkeyup='this.value=this.value.replace(/\D/gi,"")'/>
                   
                </td>
            </tr>
             <tr>
                <td style="width: 200px;" align="right" valign="middle">
                   微信 Api密钥：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtWXPartnerKey" class="" style="width: 350px;" value="<%=model.WXPartnerKey%>" maxlength="32"/>
                   
                </td>
            </tr>

            <tr>
                <td  align="left" colspan="2" valign="middle">
                <br />
                <a href="javascript:;" style="width:200px;margin-left:200px;" id="btnSave" onclick="Save();" class="button button-rounded button-primary">保存</a>
                </td>
               
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var handlerUrl = "/Handler/App/CationHandler.ashx";
    function Save() {
        var model = {
            Action: 'SavePayConfig',
            WXAppId: $.trim($(txtWXAppId).val()),
            WXMCH_ID: $.trim($(txtWXMCHID).val()),
            WXPartnerKey: $.trim($(txtWXPartnerKey).val())

        }
        if (model.WXAppId == "") {
            $(txtWXAppId).focus();
            return false;
        }
        if (model.WXMCH_ID == "") {
            $(txtWXMCHID).focus();
            return false;
        }
        if (model.WXPartnerKey == "") {
            $(txtWXPartnerKey).focus();
            return false;
        }
        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: model,
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 1) {
                    Alert("支付配置保存成功!");
                }
                else {
                    Alert("支付配置保存失败!");
                }

            }
        });

    }

</script>
</asp:Content>
