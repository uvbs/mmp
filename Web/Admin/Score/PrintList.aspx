<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PrintList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Score.PrintList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .search-status{
            color:#0face0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%= string.IsNullOrWhiteSpace(Request["module_name"])?"充值/提现" : Request["module_name"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData">
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            事件：
            <%--<a href="javascript:void(0);" class="search-status" style="color:red;" onclick="SearchEvents(this,'');">全部</a>--%>
            <a href="javascript:void(0);" class="search-status" style="color:red;" onclick="SearchEvents(this,'注册充值,线上注册充值,线上充值');">线上充值</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'线下注册充值,线下充值');">线下充值</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'升级充值');">升级充值</a>
            <a href="javascript:void(0);" class="search-status" onclick="SearchEvents(this,'申请提现');">提现</a>
            <br />
            会员：<input id="txtMember" class="easyui-textbox" placeholder="手机/姓名" value="<% = Request["member"] %>" />
            时间：<input id="txtStartTime" class="easyui-datebox" />至<input id="txtEndTime" class="easyui-datebox" />
            <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
            <%if (canWithdrawExport)
              { %>
            <a id="btnExport" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-excel" onclick="SearchExport()">导出</a>
            <%} %>
            <%if (canWithdrawPrint)
              { %>
            <a id="btnPrint" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-print" onclick="PrintPage()">打印</a>
            <%} %>
        </div>
    </div>
    <div id="dlgDetail" class="easyui-dialog" closed="true"
        data-options="iconCls:'icon-tip',title:'记录详情',
            width:800,height:document.documentElement.clientHeight-50,
            modal:true,buttons:'#dlgDetailButtons',border:false">
        <iframe id="ifmDetail" frameborder="0" width="100%" height="100%" src="/Admin/Score/PrintDetail.aspx"></iframe>
    </div>
    <div id="dlgDetailButtons">
            <%if (canWithdrawPrint)
              { %>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="PrintDetail()">打印</a>
            <%} %>
        <a href="javascript:void(0);" class="easyui-linkbutton" onclick="$('#dlgDetail').dialog('close');">关闭</a>
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
         var grvOpts = {
             fitColumns: true,
             toolbar: '#divToolbar',
             border: false,
             method: 'Post',
             height: document.documentElement.clientHeight - 70,
             url: '/serv/api/admin/score/list.ashx',
             pagination: true,
             striped: true,
             loadFilter: pagerFilter,
             rownumbers: true,
             showFooter: true,
             columns:[[
		        { field: 'ck', width: 5,checkbox: true },
		        { field: 'uid', title: '会员', width: 40, formatter: FormatterMember },
		        { field: 'time_str', title: '时间', width: 40, formatter: FormatterTitle },
		        { field: 'score', title: '金额', width: 30, formatter: FormatterEventAmount },
		        { field: 'score_event', title: '事件', width: 35, formatter: FormatterTitle },
		        { field: 'ex1', title: '支付方式', width: 40, formatter: FormatterEx1 },
		        { field: 'ex2', title: '商户单号', width: 40, formatter: FormatterEx2 },
		        { field: 'ex3', title: '支付单号', width: 60, formatter: FormatterEx3 },
		        { field: 'deduct_score', title: '税费', width: 30, hidden: true, formatter: FormatterTitle },
		        { field: 'act', title: '操作', width: 20, formatter: FormatterAction }
             ]],
             queryParams: {
                 score_type: '<% = Request["score_type"] %>',
                 member: '<% = Request["member"] %>',
                 is_print: 1,
                 score_events: '注册充值,线上注册充值,线上充值'
             }
         };
        var searchModel = {
            score_type: score_type,
            member: member,
            is_print: 1,
            score_events: '注册充值,线上注册充值,线上充值'
        };

        $(function () {
            $('#grvData').datagrid(grvOpts);
        });
        function FormatterEventAmount(value, rowData) {
            if (!value) return "";
            if (rowData.score_event == '申请提现') {
                return 0 - value;
            } else {
                return value;
            }
        }
        function FormatterMember(value,rowData) {
            if (!value) return "";
            return rowData.nickname + '[' + rowData.phone + ']';
        }
        function FormatterEx1(value, rowData) {
            if (rowData.score_event.indexOf('提现') < 0) {
                if (rowData.ex5 == 'alipay') { return '支付宝'; }
                else if (rowData.ex5 == 'weixin') { return '微信'; }
            } 
            return $.trim(rowData.ex1);
        }
        function FormatterEx2(value, rowData) {
            if (rowData.score_event.indexOf('提现') >= 0) {
                return $.trim(rowData.ex2);
            } else if (rowData.ex5 == 'offline') {
                return '';
            } else {
                return $.trim(rowData.rel_id);
            }
        }
        function FormatterEx3(value, rowData) {
            if (rowData.score_event.indexOf('提现') >= 0) {
                return $.trim(rowData.ex3);
            } else if (rowData.ex5 == 'offline') {
                return '';
            } else {
                return $.trim(rowData.serial_number);
            }
        }
        function FormatterOther(value, rowData) {
            if (rowData.score_event.indexOf('提现') >= 0) {
                return '税费：' + $.trim(rowData.deduct_score) + '，开户银行：' + $.trim(rowData.ex1) + '<br />开户名：' + $.trim(rowData.ex2) + '，银行卡号：' + $.trim(rowData.ex3);
            } else if (rowData.ex5 == 'offline') {
                if (rowData.ex1) return '充值渠道：' + $.trim(rowData.ex1);
                return '';
            } else {
                var pex1 = '';
                if (rowData.ex5 == 'alipay') { pex1 = '支付方式：支付宝'; }
                else if (rowData.ex5 == 'weixin') { pex1 = '支付方式：微信'; }
                var pex2 = '';
                if (rowData.rel_id) pex2 = '<br />商户单号：' + $.trim(rowData.rel_id);
                var pex3 = '';
                if (rowData.serial_number) pex3 = '<br />支付单号：' + $.trim(rowData.serial_number);
                return pex1 + pex2 + pex3;
            }
        }
        function FormatterAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="javascript:void(0);" onclick="ShowDetail({0})"><img alt="详情" class="imgAlign" style="margin-right: 2px; position: relative;top: 4px;" src="/MainStyle/Res/easyui/themes/icons/list.png" title="详情" />详情</a>', rowData.id);
            return str.ToString();
        }
        function ShowDetail(did) {
            //$('#ifmDetail').attr('src', '/Admin/Score/PrintDetail.aspx?id=' + did);
            document.getElementById("ifmDetail").contentWindow.SetEmpty();
            $('#dlgDetail').dialog('open');
            document.getElementById("ifmDetail").contentWindow.GetPrintDetail(did);
        }
        function PrintDetail() {
            document.getElementById("ifmDetail").contentWindow.PrintDetail();
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
            //5,6,7,8
            if (searchModel.score_events == '申请提现') {
                grvOpts.columns[0][5].title = '开户银行';
                grvOpts.columns[0][6].title = '开户名';
                grvOpts.columns[0][7].title = '银行卡号';
                grvOpts.columns[0][8].title = '税费';
                grvOpts.columns[0][5].hidden = false;
                grvOpts.columns[0][6].hidden = false;
                grvOpts.columns[0][7].hidden = false;
                grvOpts.columns[0][8].hidden = false;
                grvOpts.columns[0][5].width = 130;
                grvOpts.columns[0][6].width = 120;
                grvOpts.columns[0][7].width = 190;
                grvOpts.columns[0][8].width = 90;
            } else if (searchModel.score_events == '线下注册充值,线下充值') {
                grvOpts.columns[0][5].title = '充值渠道';
                grvOpts.columns[0][5].hidden = false;
                grvOpts.columns[0][6].hidden = true;
                grvOpts.columns[0][7].hidden = true;
                grvOpts.columns[0][8].hidden = true;
                grvOpts.columns[0][5].width = 130;
            } 
            else {
                grvOpts.columns[0][5].title = '支付方式';
                grvOpts.columns[0][6].title = '商户单号';
                grvOpts.columns[0][7].title = '支付单号';
                grvOpts.columns[0][5].hidden = false;
                grvOpts.columns[0][6].hidden = false;
                grvOpts.columns[0][7].hidden = false;
                grvOpts.columns[0][8].hidden = true;
                grvOpts.columns[0][5].width = 136;
                grvOpts.columns[0][6].width = 136;
                grvOpts.columns[0][7].width = 238;
            }
            grvOpts.queryParams = searchModel;
            $('#grvData').datagrid(grvOpts);
            //Search();
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
