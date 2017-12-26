<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Flow.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .search-status {
            color: #0face0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%=module_name %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/flow/list.ashx',pagination:true,striped:true,loadFilter: pagerFilter,rownumbers:true,queryParams:{flow_key:'<% = flow_key %>',status:'<% = def_status %>',inactme:'<% = def_status=="0,11"?"1":"" %>'}">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="flowname" width="40" formatter="FormatterTitle">流程</th>
                <%if (flow_key == "PerformanceReward")
                  {%>
                <th field="ex1" width="40" formatter="FormatterTitle">月份</th>
                <%}%>
                <th field="amount" width="60" formatter="FormatterTitle">金额</th>
                <th field="member_id" width="30" formatter="FormatterTitle">会员编号</th>
                <th field="member_name" width="50" formatter="FormatterTitle">会员姓名</th>
                <th field="member_phone" width="40" formatter="FormatterTitle">会员手机</th>
                <th field="lvname" width="50" formatter="FormatterTitle">会员级别</th>
                <%if (flow_key == "RegisterOffLine" || flow_key == "RegisterEmptyBill" || flow_key == "CancelRegister")
                  {%>
                <th field="creater_name" width="50" formatter="FormatterTitle">提交人</th>
                <%}%>
                <th field="start" width="50" formatter="FormatterStartTime">提交时间</th>
                <th field="status" width="30" formatter="FormatterStatus">状态</th>
                <th field="end" width="50" formatter="FormatterTime">结束时间</th>
                <th field="action" width="30" formatter="FormatterAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <% if (hide_status != "all")
               { %>
            状态：
            <%if (def_status == "")
              { %>
            <a href="javascript:void(0);" class="search-status" style="color: red;" onclick="SearchStatus(this,'');">全部</a>
            <%}%>
            <%if (flow_key == "Withdraw")
              { %>
            <a href="javascript:void(0);" class="search-status" style="color: red;" onclick="SearchStatus(this,'0,11');">待处理</a>
            <%}
              else if (hide_status_str.IndexOf(",0,") < 0)
              {%>
            <a href="javascript:void(0);" class="search-status" style="color: red;" onclick="SearchStatus(this,0);">待处理</a>
            <%}%>
            <%if (flow_key == "Withdraw")
              { %>
            <a href="javascript:void(0);" class="search-status" onclick="SearchStatus(this,'9');">已通过</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchStatus(this,'12');">拒绝取消</a>
            <%}
              else
              {%>
            <a href="javascript:void(0);" class="search-status" onclick="SearchStatus(this,'9');">已通过</a>
            <%}%>
            <%if (flow_key == "Withdraw")
              { %>
            <a href="javascript:void(0);" class="search-status" onclick="SearchStatus(this,11);">申请取消</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchStatus(this,10);">已取消</a>
            <%}%>
            <a href="javascript:void(0);" class="search-status" onclick="SearchStatus(this,8);">未通过</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchIsActionMe(this,1);">我处理过</a>
            <%if (def_status != "")
              { %>
            <a href="javascript:void(0);" class="search-status" onclick="SearchStatus(this,'');">全部</a>
            <%}%>
            <%}%>
            会员：<input id="txtMember" class="easyui-textbox" placeholder="会员编号/手机/姓名" />
            <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/flow/';
        var flow_key = "<%=flow_key%>";
        var module_name = "<%=module_name %>";
        var hide_status = "<%=hide_status %>";

        var searchModel = {
            flow_key: flow_key,
            status: '<% = def_status %>',
            inactme: '<% = (def_status=="0" || def_status=="0,11") ?"1":"" %>',
            isactme: '',
            member: ''
        };
        $(function () {

        });
        function SetSearchColor(ob) {
            $('.search-status').css('color', '#0face0');
            $(ob).css('color', 'red');
        }
        //搜索会员
        function Search() {
            searchModel.member = $.trim($('#txtMember').val());
            $('#grvData').datagrid('load', searchModel);
        }
        function SearchStatus(ob, num) {
            SetSearchColor(ob);
            searchModel.status = num;
            if (num === 0) {
                searchModel.inactme = 1;
            } else {
                searchModel.inactme = '';
            }
            searchModel.isactme = '';
            searchModel.member = $.trim($('#txtMember').val());
            $('#grvData').datagrid('load', searchModel);
        }
        function SearchIsActionMe(ob, num) {
            SetSearchColor(ob);
            searchModel.status = '';
            searchModel.isactme = 1;
            searchModel.inactme = '';
            searchModel.member = $.trim($('#txtMember').val());
            $('#grvData').datagrid('load', searchModel);
        }
        //格式化时间
        function FormatterStartTime(value) {
            if (value == '' || value == null) return "";
            if (value.indexOf('0001') == 0) return "";
            if (new Date(value).format('yyyy/MM/dd') == new Date().format('yyyy/MM/dd')) {
                return '<span style="color:green;">' + new Date(value).format('yyyy/MM/dd hh:mm') + '</span>';
            }
            return new Date(value).format('yyyy/MM/dd hh:mm');
        }
        function FormatterTime(value) {
            if (value == '' || value == null) return "";
            if (value.indexOf('0001') == 0) return "";
            return new Date(value).format('yyyy/MM/dd hh:mm');
        }
        //格式化状态
        function FormatterStatus(value) {
            if (value === 0) return '处理中';
            if (value == '' || value == null) return "";
            if (value == 8)
                return '<span style="color:red;">未通过</span>';
            else if (value == 9)
                return '已通过';
            else if (value == 10)
                return '<span style="color:red;">已取消</span>';
            else if (value == 11)
                return '<span style="color:red;">申请取消</span>';
            else if (value == 12)
                return '<span style="color:red;">拒绝取消</span>';
            return "";
        }
        //格式化操作
        function FormatterAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="/Admin/Flow/Action.aspx?flow_key={0}&module_name={1}&id={2}&hide_status={3}"><img alt="审核" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="审核" /></a>'
                , flow_key, module_name, rowData.id, hide_status);
            return str.ToString();
        }
        //显示新增框
        function ShowAddItem() {
            InitCard();
            $('#dlgAddCard').dialog('open');
        }
        //初始新增框
        function InitCard() {
            $('#txtName').val('');
            $('#txtAmount').numberbox('setValue', '');
            $('#txtMaxCount').numberbox('setValue', '');
            $('#txtValidTo').datetimebox('setValue', '');
        }
    </script>
</asp:Content>
