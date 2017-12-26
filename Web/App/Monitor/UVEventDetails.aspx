<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="UVEventDetails.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Monitor.UVEventDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>微信访问监测明细</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
            <input type="radio"  name="time" value="day" id="day"/><label for="day">昨天</label>
            <input type="radio"  name="time" value="week" id="week"/><label for="week">近7天</label>
             <input type="radio"  name="time" value="month" id="month"/><label for="month">近30天</label>
            
    </div>

     <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/Monitor/MonitorHandler.ashx";
        var date = '<%=date%>';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryPlanEventDetailByUV", date: date,user_auto_id:"<%=Request["autoId"]%>" },
	                height: document.documentElement.clientHeight - 55,
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
                                { field: 'SourceIP', title: 'IP地址', width: 100, align: 'left' },
                                { field: 'IPLocation', title: 'IP所在地', width: 100, align: 'left' },
                                { field: 'EventBrowserID', title: '浏览器', width: 100, align: 'left' },
                                { field: 'EventDate', title: '访问时间', width: 100, align: 'left', formatter: FormatDate }
	                ]]
	            });
          
            if (date == 'day') {
                day.checked = true;
            } else if (date == 'week') {
                week.checked = true;
            } else if (date == 'month') {
                month.checked = true;
            }


            $("input[name=time]").click(function () {
                var model = {
                    Action: "QueryPlanEventDetailByUV",
                    date: $(this).val(),
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
    </script>
</asp:Content>
