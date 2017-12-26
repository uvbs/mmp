<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="AlipayConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.AlipayConfig" %>
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
    当前位置：&nbsp;支付宝配置
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <br />
        <hr style="border: 1px dotted #036;" />
        <br />
        <table width="100%" id="tbMain">

            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    商户号：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtPartner" class="" style="width: 350px;" value="<%=model==null?"":model.Partner %>" />
                   
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    密钥：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtPartnerKey" class="" style="width: 350px;" value="<%=model==null?"":model.PartnerKey %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    支付宝账号：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtSellerAccountName" class="" style="width: 350px;" value="<%=model==null?"":model.Seller_Account_Name %>" />
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
        var dataModel = {
            Action: 'SetAlipayConfig',
            Partner: $.trim($(txtPartner).val()),
            PartnerKey: $.trim($(txtPartnerKey).val()),
            SellerAccountName: $.trim($(txtSellerAccountName).val())

        }

        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: dataModel,
            dataType: "json",
            success: function (resp) {
                if (resp.Status == 1) {
                    Alert("保存成功!");
                }
                else {
                    Alert("保存失败!");
                }

            }
        });

    }

    </script>
</asp:Content>
