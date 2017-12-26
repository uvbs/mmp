<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.Expand.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;
    <%if (ZentCloud.BLLJIMP.BLLUserExpand.dicTypes.ContainsKey(Request["type"]))
      {%>
    <% = ZentCloud.BLLJIMP.BLLUserExpand.dicTypes[Request["type"]] %>
    <%} %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <% 
        
        string curUserID = new ZentCloud.BLLJIMP.BLLUser().GetCurrUserID();
        if (ZentCloud.BLLJIMP.BLLUserExpand.dicTypes.ContainsKey(Request["type"]))
      {%>
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/user/expand/List.ashx',pagination:true,striped:true,loadFilter: pagerFilter,rownumbers:true,showFooter:true,
        queryParams:{type:'<% = Request["type"] %>',member:'<% = Request["member"] %>'}">
        <thead>
            <tr>
                <%
                  foreach (var item in ZentCloud.BLLJIMP.BLLUserExpand.dicDefColumns[Request["type"]])
                  {
                    %>
                    <th field="<%=item.field %>" width="<%=item.width>0?item.width:50 %>" formatter="<%=item.type == "img"?"FormatterImage50":"FormatterTitle" %>"><%=item.name %></th>
                <%
                  }
                %>
                <%
                  foreach (var item in ZentCloud.BLLJIMP.BLLUserExpand.dicColumns[Request["type"]])
                  {
                    %>
                    <th field="<%=item.field %>" width="<%=item.width>0?item.width:50 %>" formatter="<%=item.type == "img"?"FormatterImage50":"FormatterTitle" %>"><%=item.name %></th>
                <%
                  }
                %>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            会员：<input id="txtMember" class="easyui-textbox" style="width: 90px;" placeholder="手机/姓名" value="<% = Request["member"] %>" />
            <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
            <%if (new ZentCloud.BLLPermission.BLLMenuPermission("").CheckUserAndPmsKey(curUserID, ZentCloud.BLLPermission.Enums.PermissionSysKey.MemberExport))
              { %>
            <a id="btnExport" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-excel" onclick="SearchExport()">导出</a>
            <%} %>
        </div>
    </div>
    <%} %>
    <div class="exportDiv" style="width:0px; height:0px; position:absolute; top:-10px;">
        <iframe id="exportIframe" style="width:0px; height:0px; position:absolute; top:-10px;"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(function () {
        });
        function Search() {
            var searchModel = { type: '<% = Request["type"] %>' };
            searchModel.member = $.trim($('#txtMember').val());
            $('#grvData').datagrid('load', searchModel);
        }
        function SearchExport() {
            var searchModel = { type: '<% = Request["type"] %>' };
            searchModel.member = $.trim($('#txtMember').val());
            $.messager.progress();
            $.ajax({
                type: 'post',
                url: '/serv/api/admin/user/expand/ListExport.ashx',
                data: searchModel,
                dataType: 'json',
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        $('#exportIframe').attr('src', '/Serv/API/Common/ExportFromRedis.ashx?cache=' + resp.result.cache);
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
