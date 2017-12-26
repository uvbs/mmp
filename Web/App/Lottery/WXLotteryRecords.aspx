<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXLotteryRecords.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.WXLotteryRecords" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var lotteryid = '<%=Request["id"]%>';
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXLotteryRecord", LotteryId:lotteryid},
	                height: document.documentElement.clientHeight - 170,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                singleSelect: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Token', title: '领取码', width: 20, align: 'left' },
                                { field: 'WXAwardName', title: '奖品', width: 20, align: 'left' },
                                { field: 'UserId', title: '用户', width: 20, align: 'left' },
                                { field: 'InsertDate', title: '中奖时间', width: 20, align: 'left', formatter: FormatDate }

                             ]]
	            }
            );


        });










        function Search() {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXLotteryRecord", LotteryId:lotteryid,Token: $("#txtToken").val()}
	            });
        }





    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>中奖结果</span> 
    <a href="/App/Lottery/WXLotteryMgr.aspx" class="easyui-linkbutton" iconcls="icon-back" plain="true" style="float:right;margin-right:50px;">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
         领取码:<input id="txtToken" style="width:200px;" />
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    
</asp:Content>