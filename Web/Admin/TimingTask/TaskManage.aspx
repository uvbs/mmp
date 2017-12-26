<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="TaskManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.TimingTask.TaskManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>定时任务管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">取消任务</a>
            状态:&nbsp;<select id="sStatus">
                <option value="">全部</option>
                <option value="-1">已取消</option>
                <option value="1">待处理</option>
                <option value="2">进行中</option>
                <option value="3">已结束</option>
            </select>
            &nbsp;
            类型:&nbsp;<select id="sType">
                <option value="">全部</option>
                 <option value="1">微信定时客服接口群发图文</option>
                 <option value="2">微信模板消息通知积分账户变化</option>
                 <option value="3">同步微信粉丝信息</option>
                 <option value="4">群发模板消息</option>
                 <option value="5">同步分销下级人数</option>
                 <option value="6">同步微信素材</option>
                 <option value="7">同步分销销售额</option>
                 <option value="8">会员清洗</option>
            </select>
                &nbsp;
            <input type="text" size="30" id="txtTaskInfo" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/serv/api/admin/TimingTask/list.ashx";
        var handlerUrl1 = "/serv/api/admin/TimingTask/CancelTask.ashx";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       height: document.documentElement.clientHeight - 98,
                       pagination: true,
                       striped: true,
                       pageSize: 20,
                       rownumbers: true,
                      // singleSelect: true,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'task_type', title: '任务类型', width: 50, align: 'left',formatter:FormatterTitle },
                                   {field: 'task_info', title: '任务信息', width: 70, align: 'left', formatter: FormatterTitle},
                                   { field: 'create_time', title: '创建日期', width: 40, align: 'left', formatter: FormatDate },
                                   { field: 'schedule_time', title: '任务日期', width: 40, align: 'left', formatter: FormatDate },
                                   { field: 'finish_time', title: '完成日期', width: 40, align: 'left', formatter: FormatDate },
                                   {
                                       field: 'status', title: '状态', width: 50, align: 'left', formatter: FormatterTitle
                                   }
                       ]]
                   }
               );
        })


        function Search() {
            $('#grvData').datagrid(
               {
                   method: "Post",
                   url: handlerUrl,
                   queryParams: { status: $("#sStatus").val(), keyword: $("#txtTaskInfo").val(), type: $("#sType").val() }
               });
        }

        //取消任务
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定要取消任务?", function (o) {
                if (o) {
                   
                    $.ajax({
                        type: "Post",
                        url: handlerUrl1,
                        data: {  ids: GetRowsIds(rows).join(',')},
                        dataType: "json",
                        success: function (resp) {
                            if (resp.status) {
                                alert(resp.msg);
                                $('#grvData').datagrid('reload');
                            } else {
                                alert('操作出错.');
                            }
                        }
                    });
                }
            })
        }

        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id
                    );
            }
            return ids;
        }
    </script>
</asp:Content>
