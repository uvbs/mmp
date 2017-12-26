<%@ Page Title="业绩明细" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Performance.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%= string.IsNullOrWhiteSpace(Request["module_name"])?"业绩明细" : Request["module_name"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/performance/details.ashx',pagination:true,striped:true,loadFilter: pagerFilter,rownumbers:true,showFooter:true,
        queryParams:{id:'<% = Request["id"] %>'}">
        <thead>
            <tr>
                <th field="phone" width="50" formatter="FormatterMember">会员</th>
                <th field="pphone" width="50" formatter="FormatterUpMember">推荐人</th>
                <th field="performance" width="50" formatter="FormatterTitle">金额</th>
                <th field="addtime" width="50" formatter="FormatterTitle">发生时间</th>
                <th field="addnote" width="100" formatter="FormatterTitle">说明</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <%if (canPerformanceExport)
              { %>
            <a id="btnExport" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-excel" onclick="SearchExport()">导出</a>
            <%} %>
        </div>
    </div>
    <div class="exportDiv" style="width:0px; height:0px; position:absolute; top:-10px;">
        <iframe id="exportIframe" style="width:0px; height:0px; position:absolute; top:-10px;"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/performance/details.ashx';
        var id = '<% = Request["id"] %>';
        $(function () {
        });
        function FormatterMember(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('{0} [{1}]', rowData.name, rowData.phone);
            return str.ToString();
        }
        function FormatterUpMember(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('{0} [{1}]', rowData.pname, rowData.pphone);
            return str.ToString();
        }
        function SearchExport() {
            var searchModel = {};
            $.messager.progress();
            $.ajax({
                type: 'post',
                url: '/serv/api/admin/Performance/DetailsExport.ashx',
                data: { id: id },
                dataType: 'json',
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        $('#exportIframe').attr('src', '/Serv/API/Common/ExportFromCache.ashx?cache=' + resp.result.cache);
                    } else {
                        alert('导出出错');
                    }
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }
    </script>
</asp:Content>
