<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Score.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .search-status{
            color:#0face0;
        }
        .datagrid-footer .datagrid-cell-c1-time_str{
            text-align:right !important;
            color:green;
        }
        .datagrid-footer .datagrid-cell-c1-score{
            text-align:right !important;
            color:red;
        }
        .datagrid-footer .datagrid-cell-c1-score_event{
            text-align:right !important;
            color:green;
        }
        .datagrid-footer .datagrid-cell-c1-addnote{
            text-align:right !important;
            color:blue;
        }
        .datagrid-body .datagrid-cell-c1-addnote{
            white-space:normal;
        }
        .divPrint table{
            border-collapse: collapse;
            width:100% !important;
        }
        .divPrint table td{
            border:1px solid #ccc;
            padding:0px 4px;
        }
        .divPrint table td[field="uid"]{
            width:15%;
        }
        .divPrint table td[field="time_str"]{
            width:15%;
        }
        .divPrint table td[field="score"]{
            width:15%;
        }
        .divPrint table td[field="score_event"]{
            width:15%;
        }
        .divPrint table td[field="addnote"]{
            width:25%;
        }
        .divPrint table td[field="deduct_score"]{
            width:15%;
        }
        .divPrint table td .datagrid-cell{
            width:auto !important;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%= string.IsNullOrWhiteSpace(Request["module_name"])?"积分明细" : Request["module_name"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/score/list.ashx',pagination:true,striped:true,loadFilter: thisPagerFilter,rownumbers:true,showFooter:true,
        onLoadSuccess:thisOnLoadSuccess,
        queryParams:{sum_score:1,win_score:1,lose_score:1,accumulationfund_score: 1,taxation_score: 1,score_type:'<% = Request["score_type"] %>',member:'<% = Request["member"] %>'}">
        <thead>
            <tr>
                <th field="uid" width="50" formatter="FormatterMember">会员</th>
                <th field="time_str" width="30" formatter="FormatterTitle">时间</th>
                <th field="score" width="30" align="right" formatter="FormatterTitle">变动</th>
                <th field="score_event" width="30" formatter="FormatterTitle">事件</th>
                <th field="addnote" width="60" formatter="FormatterTitle">说明</th>
                <th field="deduct_score" width="50" align="right" formatter="FormatterOther">其他</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            事件：
            <a href="javascript:void(0);" class="search-status" style="color:red;" onclick="SearchEvents(this,'');">全部</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'线上注册充值,线下注册充值,注册充值,注册会员,替他人注册,他人注册转入,他人代替注册');">注册</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'升级会员');">升级</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'返利,返购房补助,撤单扣返利,撤单扣购房补助,变更扣返利,变更扣购房补助');">分佣</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'申请提现,提现退款');">提现</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'线上充值,线下充值,升级充值');">充值</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'转账,获得转账');">转账</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'补账,撤单,下级撤单,冲正,变更推荐人,管理奖');">其他</a>
            <br />
            会员：<input id="txtMember" class="easyui-textbox" placeholder="手机/姓名" value="<% = Request["member"] %>" />
            时间：<input id="txtStartTime" class="easyui-datebox" />至<input id="txtEndTime" class="easyui-datebox" />
            <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
            <%if (canTotalAmountExport)
              { %>
            <a id="btnExport" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-excel" onclick="SearchExport()">导出</a>
            <%} %>
            <%if (canTotalAmountPrint)
              { %>
            <a id="btnPrint" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-print" onclick="PrintPage()">打印</a>
            <%} %>
        </div>
    </div>
    <div class="exportDiv" style="width:0px; height:0px; position:absolute; top:-10px;">
        <iframe id="exportIframe" style="width:0px; height:0px; position:absolute; top:-10px;"></iframe>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/score/';
        var module_name = "<%=Request["module_name"]%>";
        var score_type = "<%=Request["score_type"] %>";
        var member = "<%=Request["member"] %>";
        var searchModel = {
            sum_score: 1,
            win_score: 1,
            lose_score: 1,
            accumulationfund_score: 1,
            taxation_score: 1,
            score_type: score_type,
            member: member
        };

        $(function () {

        });
        function FormatterNum(value, rowData, index) {
            return index + 1;
        }
        function FormatterMember(value,rowData) {
            if (!value) return "";
            return rowData.nickname + '[' + rowData.phone + ']';
        }

        function FormatterOther(value, rowData) {
            if (rowData.score_event == "返利" || rowData.score_event == "返购房补助" || rowData.score_event == "撤单扣返利" ||
                rowData.score_event == "撤单扣购房补助" || rowData.score_event == "管理奖") {
                return "公积金：" + value;
            }
            else if (rowData.score_event == "申请提现" || rowData.score_event == "提现退款") {
                return "税费：" + value;
            }
            return value;
        }
        function thisOnLoadSuccess(data) {
            //console.log(data);
        }
        function thisPagerFilter(result) {
            //console.log(result);
            var data = result.result;
            if (data == null) {
                return {
                    total: 0,
                    rows: [],
                    footer: []
                }
            }
            var foot = {
                score: '统计：' + data.sum,
                score_event: '总收入：' + data.win,
                addnote: '总支出：' + data.lose,
                deduct_score:""
            }
            if (data.accumulationfund) {
                foot.deduct_score += '公积金：' + data.accumulationfund;
            }
            if (data.taxation) {
                if (foot.deduct_score != "") foot.deduct_score += '，';
                foot.deduct_score += '税费：' + data.taxation;
            }

            return {
                total: data.totalcount,
                rows: data.list,
                footer: [foot]
            };
        }
        function SetSearchColor(ob) {
            $('.search-status').css('color', '#0face0');
            $(ob).css('color', 'red');
        }
        //搜索会员
        function Search() {
            searchModel.member = $.trim($('#txtMember').val());
            searchModel.start = $.trim($('#txtStartTime').datebox('getValue'));
            searchModel.end = $.trim($('#txtEndTime').datebox('getValue'));
            $('#grvData').datagrid('load', searchModel);
        }
        function SearchExport() {
            searchModel.member = $.trim($('#txtMember').val());
            searchModel.start = $.trim($('#txtStartTime').datebox('getValue'));
            searchModel.end = $.trim($('#txtEndTime').datebox('getValue'));

            $.messager.progress();
            $.ajax({
                type: 'post',
                url: '/serv/api/admin/score/listexport.ashx',
                data: searchModel,
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
        function SearchEvents(ob, events) {
            SetSearchColor(ob);
            searchModel.score_events = events;
            Search();
        }
        function PrintPage() {
            $('.page_def').hide();
            var tbl = $('#grvData').closest('.datagrid-view').find('.datagrid-view2 .datagrid-btable').clone();
            var tbh = $('#grvData').closest('.datagrid-view').find('.datagrid-view2 .datagrid-header .datagrid-htable tr').clone();
            var tbf = $('#grvData').closest('.datagrid-view').find('.datagrid-view2 .datagrid-footer .datagrid-ftable tr').clone();
            $(tbl).find('tbody').prepend(tbh);
            $(tbl).find('tbody').append(tbf);
            var ob = $('<div class="divPrint"></div>');
            $(ob).append(tbl);
            $('body').append(ob);
            $('body').css('background-color', '#ffffff');
            window.print();
            $('body').css('background-color', '#f4f4f4');
            $('.page_def').show();
            $(ob).remove();
        }

    </script>
</asp:Content>
