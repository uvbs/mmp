<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Performance.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<%= string.IsNullOrWhiteSpace(Request["module_name"])?"查看业绩" : Request["module_name"] %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" class="easyui-datagrid"
        data-options="fitColumns:true,toolbar:'#divToolbar',border:false,method:'Post',height:document.documentElement.clientHeight-70,
        url:'/serv/api/admin/performance/list.ashx',pagination:true,striped:true,loadFilter: thisPagerFilter,rownumbers:true,showFooter:true,
        queryParams:{yearmonth:'<% = DateTime.Now.ToString("yyyyMM") %>',sum:1 }">
        <thead>
            <tr>
                <th field="phone" width="50" formatter="FormatterTitle">会员手机</th>
                <th field="name" width="50" formatter="FormatterTitle">会员姓名</th>
                <th field="business" width="50" formatter="FormatterTitle">公司</th>
                <th field="yearmonth" width="50" formatter="FormatterTitle">月份</th>
                <th field="performance" width="50" formatter="FormatterTitle">业绩</th>
                <th field="reward" width="50" formatter="FormatterTitle">管理奖</th>
                <th field="status" width="50" formatter="FormatterStatus">状态</th>
                <th field="action" width="50" formatter="FormatterAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            会员：<input id="txtMember" class="easyui-textbox" style="width: 90px;" placeholder="手机/姓名" value="<% = Request["member"] %>" />
            推荐人：<input id="txtUpMember" class="easyui-textbox" style="width: 90px;" placeholder="手机/姓名" value="<% = Request["up_member"] %>" />
            年份：
            <select id="sltYear" class="easyui-combobox" style="width:80px;" editable="false">

            </select>
            月份：
            <select id="sltMonth" class="easyui-combobox" style="width:70px;" editable="false">
            </select>
            <a id="btnSearch" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a>
            <%if (canPerformanceExport)
              { %>
            <a id="btnComputeReward" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-list" onclick="ComputeReward();">计算管理奖</a>
            <a id="btnPublish" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-list" onclick="Publish();">发布到前台</a>
            <%} %>
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
        var handlerUrl = '/serv/api/admin/performance/list.ashx';
        var curYear = new Date().getFullYear();
        var curMonth = padLeft((new Date().getMonth() + 1),10);
        var yearList = [];
        var monthList = [];
        $(function () {
            initSelectYear();
            $('#sltMonth').combobox('setValue', curMonth);
        });

        function thisPagerFilter(result) {
            var data = result.result;
            if (data == null) {
                return {
                    total: 0,
                    rows: [],
                    footer: []
                }
            }
            var foot = {
                performance: '总业绩：' + data.sum,
                isFoot:true
            }
            return {
                total: data.totalcount,
                rows: data.list,
                footer: [foot]
            };
        }
        function initSelectYear() {
            for (var i = 2016; i <= curYear; i++) {
                yearList.push({ id: i, text: i + '年' });
            }
            yearList.push({ id: 0, text: '全部' });
            for (var i = 1; i <= 12; i++) {
                var pm = padLeft(i, 10);
                monthList.push({ id: pm, text: i + '月' });
            }
            $('#sltYear').combobox({
                valueField: 'id',
                textField: 'text',
                data: yearList,
                onSelect: function (record) {
                    if (record.id == 0) {
                        $('#sltMonth').combobox('loadData', []);
                        $('#sltMonth').combobox('setValue', '');
                    } else {
                        $('#sltMonth').combobox('loadData', monthList);
                        $('#sltMonth').combobox('setValue', '01');
                    }
                    Search();
                }
            })
            $('#sltMonth').combobox({
                valueField: 'id',
                textField: 'text',
                data: monthList,
                onSelect: function (record) {
                    Search();
                }
            })
            $('#sltYear').combobox('setValue', curYear);
            $('#sltMonth').combobox('setValue', curMonth);
        }
        function FormatterStatus(value, rowData) {
            if (value == 0) return '待发布';
            if (value == 1) return '已发布';
            if (value == 2) return '票据审核';
            if (value == 9) return '审核完成';
            return '';
        }
        function FormatterAction(value, rowData) {
            if (rowData.isFoot) return "";
            var str = new StringBuilder();
            str.AppendFormat('<a href="/Admin/Performance/Details.aspx?id={0}" target="_blank"><img alt="财务明细" class="imgAlign" style="margin-right: 2px; position: relative;top: 4px;" src="/MainStyle/Res/easyui/themes/icons/list.png" title="财务明细" />明细</a>', rowData.id);
            <%if (canPerformanceConfrimExport)
              { %>
            str.AppendFormat('<a href="javascript:void(0)" style="margin-left:10px;" onclick="ExportConfrimForm({0})"><img alt="业绩确认表" class="imgAlign" style="margin-right: 2px; position: relative;top: 4px;" src="/MainStyle/Res/easyui/themes/icons/excel.png" title="业绩确认表" />确认表</a>', rowData.id);
            <%}%>
            return str.ToString();
        }
        function Search() {
            var searchModel = {sum:1};
            searchModel.member = $.trim($('#txtMember').val());
            searchModel.up_member = $.trim($('#txtUpMember').val());
            var selectYear = parseInt($('#sltYear').combobox('getValue'));
            if (selectYear > 0) {
                searchModel.yearmonth = selectYear + $('#sltMonth').combobox('getValue');
            } else {
                searchModel.yearmonth = 0;
            }
            $('#grvData').datagrid('load', searchModel);
        }
        function ComputeReward() {
            var searchModel = {};
            var selectYear = parseInt($('#sltYear').combobox('getValue'));
            if (selectYear > 0) {
                searchModel.yearmonth = selectYear + $('#sltMonth').combobox('getValue');
            } else {
                alert('请选择月份');
                return;
            }
            $.messager.progress();
            $.ajax({
                type: 'post',
                url: '/serv/api/admin/Performance/ComputeReward.ashx',
                data: searchModel,
                dataType: 'json',
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        $('#grvData').datagrid('reload');
                    } else {
                        alert(resp.msg);
                    }
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }
        function Publish() {
            var searchModel = {};
            var selectYear = $('#sltYear').combobox('getValue');
            var selectMonth = $('#sltMonth').combobox('getValue');
            if (selectYear > 0) {
                searchModel.yearmonth = selectYear + selectMonth;
            } else {
                alert('请选择月份');
                return;
            }
            $.messager.confirm('系统提示', '确认发布当前管理奖到前台?', function (r) {
                if (r) {
                    $.ajax({
                        type: 'post',
                        url: '/serv/api/admin/Performance/PublishReward.ashx',
                        data: searchModel,
                        dataType: 'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.status) {
                                $('#grvData').datagrid('reload');
                            } else {
                                alert(resp.msg);
                            }
                        },
                        error: function () {
                            $.messager.progress('close');
                        }
                    });
                }
            });
        }
        function ExportConfrimForm(_id) {
            $.messager.progress();
            $.ajax({
                type: 'post',
                url: '/serv/api/admin/Performance/ConfrimFormExport.ashx',
                data: { id: _id },
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
        function SearchExport() {
            var searchModel = {};
            searchModel.member = $.trim($('#txtMember').val());
            searchModel.up_member = $.trim($('#txtUpMember').val());
            var selectYear = parseInt($('#sltYear').combobox('getValue'));
            if (selectYear > 0) {
                searchModel.yearmonth = selectYear + $('#sltMonth').combobox('getValue');
            } else {
                searchModel.yearmonth = 0;
            }
            $.messager.progress();
            $.ajax({
                type: 'post',
                url: '/serv/api/admin/Performance/ListExport.ashx',
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
    </script>
</asp:Content>
