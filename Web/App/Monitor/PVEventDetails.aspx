<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PVEventDetails.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Monitor.PVEventDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>访问量监测明细</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
 
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
            <input type="radio"  name="time" value="day" id="day"/><label for="day">昨天</label>
            <input type="radio"  name="time" value="week" id="week"/><label for="week">近7天</label>
             <input type="radio"  name="time" value="month" id="month"/><label for="month">近30天</label>
            
            <%
                if (!string.IsNullOrEmpty(userId))
                {
                    %>
                    <div style="text-align:right;float:right">
                        <a href="javascript:history.go(-1);"   class="easyui-linkbutton"
                            iconcls="icon-redo" plain="true" >返回</a>
                        </div>
                    <%
                }
           %>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

    <script type="text/javascript">
        var handlerUrl = "/Handler/Monitor/MonitorHandler.ashx";
        var time = '<%=date%>';
        var userId = '<%=userId%>';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryPlanEventDetailByPV", time: time, user_auto_id: "<%=Request["autoId"]%>", userid: userId },
	                height: document.documentElement.clientHeight - 85,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                singleSelect: true,
	                columns: [[
                                {
                                    field: 'EventUserWXImg', title: '微信头像', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="微信头像" height="50" width="50" /></a>', value);
                                        return str.ToString();
                                    }
                                },
                                { field: 'EventUserWXNikeName', title: '微信昵称', width: 100, align: 'left' },
                                { field: 'EventUserTrueName', title: '姓名', width: 100, align: 'left' },
                                { field: 'EventUserPhone', title: '手机', width: 100, align: 'left' },
                                {
                                    field: 'ModuleTyppeString', title: '页面', width: 100, align: 'left', formatter: function (value, rowData) {
                                            //debugger;
                                        var str = new StringBuilder();
                                        if (rowData.ModuleTyppeString == '商品') {
                                            str.AppendFormat('<a href="/customize/shop/?v=1.0&ngroute=/productDetail/{0}#/productDetail/{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', GetQueryString(rowData.SourceUrl, 'product_id'), rowData.ModuleTyppeString);
                                        } else if (rowData.ModuleTyppeString == '摇一摇') {
                                            str.AppendFormat('<a href="/customize/shake/?ngroute=/shake/{0}#/shake/{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>',  GetQueryString(rowData.SourceUrl, 'id'), rowData.ModuleTyppeString);
                                        } else if (rowData.ModuleTyppeString=='答题') {
                                            str.AppendFormat('<a href="/customize/dati/index.aspx?id={0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', GetQueryString(rowData.SourceUrl, 'id'), rowData.ModuleTyppeString);
                                        } else if (rowData.ModuleTyppeString == '文章') {
                                            str.AppendFormat('<a href="{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', rowData.SourceUrl, rowData.ModuleTyppeString);
                                        } else if (rowData.ModuleTyppeString == '活动') {
                                            str.AppendFormat('<a href="{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', rowData.SourceUrl, rowData.ModuleTyppeString);
                                        } else if (rowData.ModuleTyppeString == '选题投票') {
                                            str.AppendFormat('<a href="{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', rowData.SourceUrl, rowData.ModuleTyppeString);
                                        } else if (rowData.ModuleTyppeString == '微秀') {
                                            str.AppendFormat('<a href="{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', rowData.SourceUrl, rowData.ModuleTyppeString);
                                        } else if (rowData.ModuleTyppeString == '刮刮奖') {
                                            str.AppendFormat('<a href="{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', rowData.SourceUrl, rowData.ModuleTyppeString);
                                        } else if (rowData.ModuleTyppeString == '问卷') {
                                            str.AppendFormat('<a href="{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', rowData.SourceUrl, rowData.ModuleTyppeString);
                                        } else if(rowData.ModuleTypeString=='贺卡'){
                                            str.AppendFormat('<a href="{0}" target="_blank" class="listClickNum" title="点击查看详情">{1}</a>', rowData.SourceUrl, rowData.ModuleTyppeString);
                                        } else {
                                            str.AppendFormat('<a href="javascript:;" target="_blank" class="listClickNum" title="点击查看详情">{0}</a>',  rowData.ModuleTyppeString);
                                        }
                                        return str.ToString();
                                    }
                                },
                                { field: 'SourceIP', title: 'IP地址', width: 100, align: 'left' },
                                { field: 'IPLocation', title: 'IP所在地', width: 100, align: 'left' },
                                { field: 'EventBrowserID', title: '浏览器', width: 100, align: 'left' },
                                { field: 'EventDate', title: '访问时间', width: 100, align: 'left', formatter: FormatDate }
	                ]]
	            });

            if (time == 'day') {
                day.checked = true;
            } else if (time == 'week') {
                week.checked = true;
            } else if (time == 'month') {
                month.checked = true;
            }


            $("input[name=time]").click(function () {
                var model = {
                    Action: "QueryPlanEventDetailByPV",
                    time: $(this).val(),
                    user_auto_id: "<%=Request["autoId"]%>"
                };
                $('#grvData').datagrid(
                    {
                        method: "Post",
                        url: handlerUrl,
                        queryParams: model
                    });
            })



        });

        ///采用正则表达式获取地址栏参数
        function GetQueryString(url,name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = url.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }



    </script>

</asp:Content>
