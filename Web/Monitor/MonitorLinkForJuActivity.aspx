<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true"
    CodeBehind="MonitorLinkForJuActivity.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Monitor.MonitorLinkForJuActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/Handler/Monitor/MonitorHandler.ashx';
        var monitorPlanID = '<%=Request["MonitorPlanID"] %>';
        $(function () {
            $(window).resize(function () {
                $("#grvData").datagrid('resize',
	            {
	                width: document.body.clientWidth,
	                height: document.documentElement.clientHeight
	            });
            });
            //-----------------加载gridview
            $("#grvData").datagrid({
                method: "Post",
                url: handlerUrl,
                height: document.documentElement.clientHeight,
                toolbar: '#divToolbar',
                fitCloumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                queryParams: { Action: "QueryLink2", PlanId: monitorPlanID }
            });
            //------------加载gridview
        });

        function FormatOpenCount(value, row) {
            return "<a title=\"点击查看详细列表\" href=\"/Monitor/MonitorEventDetails.aspx?planId=" + monitorPlanID + " &linkid=" + row.LinkID + "\">" + value + "</a>";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <label style="font-size: 14px; font-weight: bold;">
                <%--总计： 0 IP 0 PV--%>&nbsp;</label>
            <%string refUrl = Request.UrlReferrer.ToString(); %>
            <a style="float: right;" href="javascript:window.location='<%=refUrl %>'" class="easyui-linkbutton"
                iconcls="icon-back" plain="true">返回上一页</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true">
                    <th field="LinkName" width="30">
                        来源名称
                    </th>
                    <th field="RealLink" width="40">
                        链接地址
                    </th>
                    <th field="PV" formatter="FormatOpenCount" align="center" width="10">
                        浏览量（PV）
                    </th>
                    <th field="IP" formatter="FormatOpenCount" align="center" width="10">
                        IP数
                    </th>
            </tr>
        </thead>
    </table>
</asp:Content>
