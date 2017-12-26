<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="VoteLogInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Vote.VoteLogInfoMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;投票;&nbsp;&gt;&nbsp;&nbsp;<span>投票记录 </span>
     <a href="javascript:history.go(-1)" style="float: right; margin-right: 20px;" title="返回"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">

    <table id="grvData" fitcolumns="true">
    </table>

      
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var VoteId="<%=Request["vid"] %>";
     var UserId="<%=Request["uid"] %>";
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryVoteLogInfo",VoteID: VoteId,UserId:UserId},
	                height: document.documentElement.clientHeight - 120,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { field: 'VoteName', title: '投票', width: 20, align: 'left' },
                                { field: 'VoteObjectName', title: '投票对象', width: 20, align: 'left' },
                                { field: 'UserID', title: '投票用户', width: 20, align: 'left' },
                                { field: 'VoteCount', title: '票数', width: 20, align: 'left'},
                                { field: 'InsertDate', title: '投票时间', width: 20, align: 'left',formatter: FormatDate }

                             ]]
	            }
            );

         $("#btnSearch").click(function () {
             var VoteID = $("#ddlVote").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryVoteLogInfo",VoteID: VoteID} });
         });

     })





 </script>
</asp:Content>
