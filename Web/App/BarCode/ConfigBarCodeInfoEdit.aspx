<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ConfigBarCodeInfoEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.BarCode.ConfigBarCodeInfoEdit" %>

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
    当前位置：&nbsp;管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;:<%=Tag%>产品真伪
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a style="float: right;" href="javascript:history.go(-1);" class="easyui-linkbutton"
                iconcls="icon-redo" plain="true">返回</a>
        </div>
    </div>
    <div class="ActivityBox">
        <br />
        <hr style="border: 1px dotted #036;" />
        <br />
        <table width="100%" id="tbMain">
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    名称：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtCodeName" class="" style="width: 350px;" value="<%=model==null?"":model.CodeName %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    条形码：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtBarCode" class="" style="width: 350px;" value="<%=model==null?"":model.BarCode %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    型号：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtModelCode" class="" style="width: 350px;" value="<%=model==null?"":model.ModelCode %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    经销商：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtAgency" class="" style="width: 350px;" value="<%=model==null?"":model.Agency %>" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" valign="middle">
                    <br />
                    <a href="javascript:;" style="width: 200px; margin-left: 200px;" id="btnSave" onclick="Save();"
                        class="button button-rounded button-primary">保存</a>
                </td>
            </tr>
        </table>
    </div>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
        var handlerUrl = "/Handler/App/WXBarCodeHandler.ashx";
        function Save() {
            var dataModel = {
                Action: 'AUBarCodeInfoData',
                AutoId: <%=model==null?"0":model.AutoId.ToString() %>,
                CodeName: $.trim($(txtCodeName).val()),
                BarCode: $.trim($(txtBarCode).val()),
                ModelCode: $.trim($(txtModelCode).val()),
                Agency: $.trim($(txtAgency).val())
            }
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: dataModel,
                dataType:"json",
                success: function (resp) {
                    if (resp.Status == 0) {
                        Alert(resp.Msg);
                    }
                    else {
                        Alert(resp.Msg);
                    }

                }
            });

        }
    </script>
</asp:Content>
