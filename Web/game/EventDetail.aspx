<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="EventDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.game.EventDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>监测明细</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            
            <a href="javascript:history.go(-1);" class="easyui-linkbutton"
                iconcls="icon-redo" plain="true" >返回</a>
            
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>

<asp:Content ID="bottom" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var handlerUrl = "/Handler/App/CationHandler.ashx";
    var planId = '<%=Request["pid"]%>';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryGameEventDetail", planId: planId },
	                height: document.documentElement.clientHeight - 145,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                singleSelect: true,
	                columns: [[

                                { field: 'SourceIP', title: 'IP地址', width: 100, align: 'left' },
                                { field: 'IPLocation', title: 'IP所在地', width: 100, align: 'left' },
                                { field: 'EventBrowser', title: '浏览器', width: 100, align: 'left' },
                                { field: 'EventDate', title: '访问时间', width: 100, align: 'left', formatter: FormatDate }]]
	            });
        });
    </script>
    </asp:Content>