<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SetPwd.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.SetPwd" %>
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
    当前位置：&nbsp;修改密码
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <br />
        <hr style="border: 1px dotted #036;" />
        <br />
        <table width="100%" id="tbMain">
            
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    旧密码：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="password" id="txtPwd" class="" style="width: 350px;" value="" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    新密码：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="password" id="txtPwdNew" class="" style="width: 350px;" value="" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    新密码确认：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="password" id="txtPwdNewSure" class="" style="width: 350px;" value="" />
                </td>
            </tr>
            
            <tr>
                <td  align="left" colspan="2" valign="middle">
                <br />
                <a href="javascript:;" style="width:200px;margin-left:200px;" id="btnSave" onclick="Save();" class="button button-rounded button-primary">确定</a>
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
                Action: 'SetPwd',
                PwdOld: $.trim($(txtPwd).val()),
                PwdNew: $.trim($(txtPwdNew).val()),
                PwdNewSure: $.trim($(txtPwdNewSure).val())
               
            }

            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: dataModel,
                dataType:"json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        alert("密码修改成功！请重新登录");
                        window.location.href = "<%=ZentCloud.Common.ConfigHelper.GetConfigString("logoutUrl") + "?op=logout"%>";
                    }
                    else {
                        Alert(resp.Msg);
                    }

                }
            });

        }

    </script>
</asp:Content>

