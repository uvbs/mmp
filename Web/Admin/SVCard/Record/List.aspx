<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.SVCard.Record.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;发放详情
    <a href="/Admin/SVCard/List.aspx" style="float: right; margin-right: 20px; color: Black;" title="返回" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        pagination:true,striped:true,loader:pagerLoader,loadFilter: pagerFilter,rownumbers:true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <%--<th field="card_id" width="30" formatter="FormatterTitle">储值卡ID</th>--%>
                <th field="card_number" width="70" formatter="FormatterTitle">储值卡编号</th>
                <th field="amount" width="70" >储值卡总额</th>
                <th field="canuse_amount" width="70" >储值卡余额</th>
                <th field="create_date" width="50" formatter="FormatterDate">发放日期</th>
                <th field="valid_to" width="50" formatter="FormatterValidDate">有效日期</th>
                <th field="use_date" width="50" formatter="FormatterUseDate">使用日期</th>
                <th field="user_id" width="50" formatter="FormatUser">发放用户</th>
                <th field="touser_id" width="50" formatter="FormatToUser">转赠用户</th>
                <th field="op" width="50" formatter="FormatUseRecord">消费记录</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div>
            状态：<select id="sltStatus" class="easyui-combobox" data-options="editable:false,onSelect:Search,width:200">
                <option value="">全部</option>
                <option value="0">未使用(含转赠未过期)</option>
                <option value="1">已使用(含转赠)</option>
                <option value="2">已过期(含转赠)</option>
                <option value="3">已发放(无转赠)</option>
                <option value="4">已转赠(仅转赠)</option>
            </select>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        //用户列表查询
        function pagerLoader(param, success, error) {
            var qParam = $.extend({}, param, {
                card_id: '<%= this.Request["card_id"] %>'
            })
            $.ajax({
                type: "Post",
                url: '/serv/api/admin/svcard/record/list.ashx',
                data: qParam,
                dataType: "json",
                success: success,
                error: error
            });
        }
        function FormatterDate(value) {
            if (value == '' || value == null) return "";
            return new Date(value).format('yyyy-MM-dd hh:mm');
        }
        function FormatterValidDate(value, rowData) {
            if (value == '' || value == null) return "";
            var strDate = new Date(value).format('yyyy-MM-dd hh:mm');
            if (rowData.use_status == 2) {
                return '<span style="color:red;">'+strDate+'</span>';
            }
            return strDate;
        }
        function FormatterUseDate(value, rowData) {
            if (value == '' || value == null) return "";
            var strDate = new Date(value).format('yyyy-MM-dd hh:mm');
            if (rowData.use_status == 1) {
                return '<span style="color:red;">' + strDate + '</span>';
            }
            return strDate;
        }
        function FormatUser(value, rowData) {
            if (value == 0) return "";
            var str = new StringBuilder();
            if (!!rowData.user_nickname) str.AppendFormat('{0}', rowData.user_nickname);
            if (!!rowData.user_phone) str.AppendFormat('({0})', value);
            str.AppendFormat('<br />');
            if (!!rowData.user_phone) str.AppendFormat('{0}', rowData.user_phone);
            return str.ToString();
        }
        function FormatToUser(value, rowData) {
            if (value == 0) return "";
            var str = new StringBuilder();
            if (!!rowData.touser_nickname) str.AppendFormat('{0}', rowData.touser_nickname);
            if (!!rowData.user_phone) str.AppendFormat('({0})', value);
            str.AppendFormat('<br />');
            if (!!rowData.touser_phone) str.AppendFormat('{0}', rowData.touser_phone);
            return str.ToString();
        }
        function FormatUseRecord(value, rowData) {
        
            var str = new StringBuilder();
            str.AppendFormat('<a target="_blank" href="UseRecord.aspx?id={0}&card_id={1}">查看消费记录<a>',rowData.id,rowData.card_id);
            return str.ToString();

        
        }


        function Search() {
            var model = {
                status: $.trim($("#sltStatus").combobox('getValue'))
            }
            $('#grvData').datagrid('load', model);
        }
    </script>
</asp:Content>
