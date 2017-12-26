<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CrowdFundRecordMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CrowdFund.Admin.CrowdFundRecordMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;众筹&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>付款记录</span>
    <a href="javascript:history.go(-1);"style="float: right; margin-right: 20px; color: Black;"
        title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">

        var handlerUrl = "AdminHandler.ashx";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryCrowdFundRecord" ,CrowdFundID:"<%=Request["id"] %>"},
	                height: document.documentElement.clientHeight - 105,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'UserID', title: '用户', width: 10, align: 'left' },
                                { field: 'Amount', title: '金额', width: 10, align: 'left' },
                                { field: 'InsertDate', title: '时间', width: 15, align: 'left',formatter: FormatDate },
                                { field: 'Name', title: '姓名', width: 10, align: 'left' },
                                { field: 'Phone', title: '手机', width: 10, align: 'left' },
                                { field: 'Company', title: '公司', width: 10, align: 'left' },
                                { field: 'Position', title: '职位', width: 10, align: 'left' },
                                { field: 'PayMentStatus', title: '状态', width: 10, align: 'left',formatter:function(value,row){
                                 return "<font color='green'>已付款</font>";
                                } }

                             ]]
	            }
            );

        });

    </script>
</asp:Content>
