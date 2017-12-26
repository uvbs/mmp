<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="OrderMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Vote.OrderMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;投票;&nbsp;&gt;&nbsp;&nbsp;<span>购票记录 </span>
     <a href="VoteInfoMgr.aspx" style="float: right; margin-right: 20px;" title="返回"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
           
              子账户:
              <select id="ddlSubAccount">
                <option value="">全部</option>
                <%=sbSubAccountList%>
              </select>

               投票:
                <select id="ddlVote">
                <option value="">全部</option>
                <%=sbVoteList%>
                </select>
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            <br />


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

      
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryVoteOrderInfo", Status: "1" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { field: 'Ex2', title: '投票', width: 5, align: 'left', formatter: function (value, rowData) {
                                    if (value != "") {
                                        return $("#ddlVote").find("option[value=" + value + "]").text();
                                    }
                                    else {
                                        return "";
                                    }

                                }
                                },
                                { field: 'OrderId', title: '订单号', width: 10, align: 'left' },
                                { field: 'Trade_No', title: '交易流水号', width: 10, align: 'left' },
                                { field: 'UserId', title: '用户', width: 5, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="VoteLogInfoMgr.aspx?vid={0}&uid={1}" >{2}</a>', rowData.Ex2, rowData.UserId, rowData.UserId);
                                    return str.ToString();

                                } 
                                },
                                { field: 'Ex1', title: '票数', width: 5, align: 'left' },
                                { field: 'Total_Fee', title: '金额', width: 5, align: 'left' },
                                { field: 'Status', title: '状态', width: 5, align: 'left', formatter: function (value, rowData) {
                                    if (value == "1") {
                                        return "<font color='green'>已付款<font/>";
                                    }
                                    else {
                                        return "<font color='red'>未付款<font/>";
                                    }

                                }
                                },
                             { field: 'InsertDate', title: '购票时间', width: 10, align: 'left', formatter: FormatDate }

                             ]]
	            }
            );

         $("#btnSearch").click(function () {
             var SubAccount = $("#ddlSubAccount").val();
             var VoteID = $("#ddlVote").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryVoteOrderInfo", Status: "1", VoteID: VoteID, SubAccount: SubAccount} });
         });

     })





 </script>
</asp:Content>