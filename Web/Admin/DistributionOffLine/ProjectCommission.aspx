<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ProjectCommission.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.ProjectCommission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;分销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>佣金记录</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <div>
                <span style="font-size: 12px; font-weight: normal">订单号：</span>
                <input type="text" style="width: 200px" id="txtKeyWord" />
                <%--<span style="font-size: 12px; font-weight: normal">账号：</span>--%>
                <input type="text" style="width: 200px;display:none;" id="txtUserId" />

                  <span style="font-size: 12px; font-weight: normal">时间段：</span>
                <input class="easyui-datebox"  id="txtFrom" />&nbsp;至
                <input class="easyui-datebox" id="txtTo" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">
                    查询</a>

                 <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-excel" plain="true" onclick="exportData();" >导出数据</a>
            </div>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

       <%if (true)
         {
             
             
         }%>
        //$("#txtFrom").datebox('setValue','<%=Request["from_date"]%>');
        //$("#txtTo").datebox('setValue', '<%=Request["to_date"]%>');

        var handlerUrl = "Handler/ProjectCommission/List.ashx";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                height: document.documentElement.clientHeight - 100,
	                queryParams: { type: "<%=Request["type"]%>", fromDate: "<%=Request["from_date"]%>", to_date: "<%=Request["to_date"]%>", userId: "<%=Request["user_id"]%>" },
	                pagination: true,
	                striped: true,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'ProjectName', title: '名称', width: 15, align: 'left' },
                                { field: 'ProjectAmount', title: '订单金额(元)', width: 10, align: 'left' },
                                {
                                    field: 'Rate', title: '佣金比例', width: 10, align: 'left', formatter: function (value, row) {
                                        return value + "%";

                                    }
                                },
                                { field: 'Amount', title: '佣金金额(元)', width: 10, align: 'left' },
                                //{ field: 'UserId', title: '账号', width: 10, align: 'left' },
                               {
                                   field: 'UserInfo', title: '受益者', width: 20, align: 'left', formatter: function (value, row) {
                                       if (row.UserInfo == null) {
                                           return "";
                                       }
                                       if ("<%=Request["type"]%>" == "DistributionOnLineChannel") {

                                            return row.UserInfo.ChannelName + "(" + row.UserInfo.AutoID + ")";
                                       }
                                       if ("<%=Request["type"]%>" == "DistributionOnLineSupplierChannel") {

                                           return row.UserInfo.Company + "(" + row.UserInfo.AutoID + ")";
                                       }
                                        return row.UserInfo.TrueName + "(" + row.UserInfo.AutoID + ")";

                                    }
                               },
                                //{ field: 'CommissionUserId', title: '贡献者账号', width: 20, align: 'left' },
                                {
                                    field: 'CommissionUserInfo', title: '贡献者', width: 20, align: 'left', formatter: function (value, row) {

                                        if (row.CommissionUserInfo == null) {
                                            return "";
                                        }
                                        return row.CommissionUserInfo.TrueName + "(" + row.CommissionUserInfo.AutoID + ")";

                                    }
                                },
                                { field: 'InsertDate', title: '分佣日期', width: 20, align: 'left', formatter: FormatDate },
                                 { field: 'Remark', title: '备注', width: 20, align: 'left' }
	                ]]
	            }
            );

            $("#btnSearch").click(function () {
                var keyWord = $("#txtKeyWord").val();
                var userId = $("#txtUserId").val();
                var fromDate = $("#txtFrom").datebox('getValue');
                var toDate = $("#txtTo").datebox('getValue');
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { keyWord: keyWord, type: "<%=Request["type"]%>", fromDate: fromDate, toDate: toDate,  userId: "<%=Request["user_id"]%>" } });
            });

        });

        //导出
        function exportData() {

            $.messager.confirm('系统提示', '确认按当前筛选导出excel?', function (o) {
                if (o) {
                    var fromDate = $("#txtFrom").datebox('getValue');
                    var toDate = $("#txtTo").datebox('getValue');
                    var zurl = "handler/projectcommission/export.ashx?type=<%=Request["type"]%>&from_date=" + fromDate + "&&to_date=" + toDate + "&keyword=" + $("#txtKeyWord").val() + "&user_id=" + "<%=Request["user_id"]%>";
                    window.open(zurl);
                }
            });
        }


    </script>
</asp:Content>
