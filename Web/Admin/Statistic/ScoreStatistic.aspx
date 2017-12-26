<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ScoreStatistic.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Statistic.ScoreStatistic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%=moduleName %>统计
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#grvData').datagrid({
                striped: true,
                singleSelect: true,
                columns: [[
                    { field: 'title', title: '统计项', width: 20, align: 'left', formatter: FormatterTitle },
                    { field: 'num', title: '<%=moduleName %>总额', width: 20, align: 'left', formatter: FormatterTitle }
                ]]
            });
            $('#grvData').datagrid('loadData',<%= data %>);
        });
    </script>
</asp:Content>
