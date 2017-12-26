<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXTempmsgList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.WXTempmsg.WXTempmsgList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置:<strong>微信模板管理</strong>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="ShowAdd();" id="btnAdd">添加模板</a>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Admin/KeyVauleData/Handler/KeyVauleDataHandler.ashx";
        var domain = "<%=Request.Url.Host %>" + "<%= Request.Url.Port == 80 ? "" : ":" + Request.Url.Port.ToString() %>";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "Query", type: "WXTmplmsg" },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       
                       rownumbers: true,
                       singleSelect: true,
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'AutoId', title: '编号', width: 10, align: 'center' },
                                   { field: 'DataValue', title: '名称', width: 30, align: 'left'},
                                   { field: 'DataKey', title: '微信模板Id', width: 65, align: 'left' },
                                   {
                                       field: 'op', title: '操作', width: 15, align: 'center', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('<a title="编辑" style="color:blue;" href="WXTempmsgEdit.aspx?id={0}">编辑</a>', rowData.AutoId);
                                           return str.ToString();
                                       }
                                   }
                       ]]
                   }
               );
        });
        function ShowAdd() {
            window.location.href = "WXTempmsgEdit.aspx?id=0";
        }
    </script>
</asp:Content>
