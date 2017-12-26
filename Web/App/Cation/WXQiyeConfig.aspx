<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXQiyeConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.WXQiyeConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
input 
{
    height: 35px;
    border: 1px solid #d5d5d5;
    border-radius: 5px;
    background-color: #fefefe;
   
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;企业微信号配置
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">

        <table width="100%" id="tbMain">

            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    CorpID：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtCorpID" placeholder="必填"  style="width: 350px;" value="<%=model.CorpID %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    Secret：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtSecret" placeholder="必填"  style="width: 350px;" value="<%=model.Secret %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    应用ID：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtAppId" placeholder="必填"  style="width: 350px;" value="<%=model.AppId%>" />
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
            Action: 'SetWXQiyeConfig',
            CorpID: $.trim($("#txtCorpID").val()),
            Secret: $.trim($("#txtSecret").val()),
            AppId: $.trim($("#txtAppId").val())
        }
        if (model.CorpID == "") {
            $("#txtCorpID").focus();
            return false;
        }
        if (model.Secret == "") {
            $("#txtSecret").focus();
            return false;
        }
        if (model.AppId == "") {
            $("#txtAppId").focus();
            return false;
        }

        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: model,
            dataType: 'json',
            success: function (resp) {
                if (resp.Status == 1) {
                    Alert("保存成功");
                }
                else {
                    Alert(resp.Msg);
                }


            }
        });

    }

    </script>
</asp:Content>

