<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SendInsideMsg.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.SendInsideMsg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;用户管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;用户列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            选择Excel：<input id="txtfile" name="file1" type="file" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-send" id="btnSendMsg" onclick="SendMsg()">发站内信</a>
            <a href="站内信模板.xls" class="easyui-linkbutton" iconcls="icon-list" target="_blank">模板下载</a>
            <span style="color:red;">注意：只能对有账号的邮箱发送，仅支持后缀.xls的excel 2003文件</span>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/UserHandler.ashx";
        var currDlgAction = '';
        var domain = '<%=Request.Url.Host%>';

        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                height: document.documentElement.clientHeight - 170,
                striped: true,
                rownumbers: true,
                singleSelect: false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                    { title: 'ck', width: 5, checkbox: true },
                    { field: 'email', title: '邮箱', width: 80, align: 'left', formatter: FormatterTitle },
                    { field: 'msg', title: '消息', width: 80, align: 'left', formatter: FormatterTitle },
                    { field: 'userName', title: '用户', width: 50, align: 'left', formatter: FormatterTitle },
                    {
                        field: 'errmsg', title: '状态', width: 50, align: 'left', formatter: FormatterTitle, styler: function (value, row, index) {
                            if (row["status"] == 1) {
                                return 'color:green;';
                            }
                            else if (row["status"] == 2) {
                                return 'color:blue;';
                            }
                            else{
                                return 'color:red;';
                            }
                        }
                    }
                ]]
            });
        });
        function SendMsg() {
            var fileName = $("#txtfile").val()
            if (fileName == "") {
                alert("请选择excel");
                return;
            }
            var extension = fileName.substring(fileName.lastIndexOf(".")).toLowerCase();
            if (extension !=".xls" && extension !=".xlsx") {
                alert("仅支持后缀.xls文件");
                return;
            }
            $.messager.progress({
                text: '正在提交...'
            });
            $.ajaxFileUpload({
                url: 'Handler/UserHandler.ashx?Action=SendInsideMsg',
                secureuri: false,
                fileElementId: 'txtfile',
                dataType: 'text',
                success: function (result) {
                    $.messager.progress('close');
                    try {
                        result = result.substring(result.indexOf("{"), result.indexOf("</"));
                    } catch (e) {
                        alert(e);
                    }
                    var resp = JSON.parse(result);
                    if (resp.Status == 1) {
                        console.log(resp.ExObj);
                        $('#grvData').datagrid("loadData", resp.ExObj);
                    } else {
                        alert(resp.Msg);
                    }
                }
            });
            //$('#grvData').datagrid({
            //    method: "Post",
            //    url: handlerUrl,
            //    queryParams: { Action: "getUserList", keyword: $("#txtKeyword").val() }
            //});
        }
    </script>
</asp:Content>
