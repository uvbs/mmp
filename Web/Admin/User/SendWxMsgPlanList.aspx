<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SendWxMsgPlanList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.SendWxMsgPlanList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .sort{
            height:0!important;
        }
        .centent_r_btm{
            border:0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <%--<div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">

             批次号:<input id="txtSerialNum" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>--%>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script>
        var handlerUrl = "/Handler/App/CationHandler.ashx";


        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QuerySendWxMsgPlan" },
                height: document.documentElement.clientHeight-60,
                pagination: true,
                striped: true,
                pageSize: 20,
                rownumbers: true,
                singleSelect: false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                    {
                        field: 'PlanID', title: '批次号', width: 10, align: 'center', formatter: function (value) {
                            return value;
                        }
                    }, {
                        field: 'Title', title: '标题', width: 20, align: 'left', formatter: function (value) {
                            return FormatterTitle(value);
                        }
                    },{
                        field: 'SendContent', title: '内容', width: 20, align: 'left', formatter: function (value) {
                            return FormatterTitle(value);
                        }
                    },{
                        field: 'Url', title: '链接', width: 20, align: 'left', formatter: function (value) {
                            return FormatterLinkBlank(value, value);
                        }
                    }, {
                        field: 'ProcStatus', title: '状态', width: 20, align: 'left', formatter: function (value) {
                            var str = new StringBuilder();

                            if (value == 1) {
                                str.AppendFormat("待处理");
                            } else if (value == 2) {
                                str.AppendFormat("进行中");
                            } else if (value == 3) {
                                str.AppendFormat("已结束");
                            }
                            return str.ToString();
                        }
                    },
                        {
                        field: 'SubmitCount', title: '发送总数', width: 10, align: 'center', formatter: function (value, row) {
                            return value;
                        }
                    }, {
                        field: 'SuccessCount', title: '成功数', width: 10, align: 'center', formatter: function (value,row) {
                            return value;
                        }
                    }, {
                        field: 'FailCount', title: '失败数', width: 10, align: 'center', formatter: function (value, row) {
                            return value;
                        }
                    }, {
                        field: 'SubmitDateStr', title: '发送时间', width: 20, align: 'center', formatter: function (value) {
                            return value;
                        }
                    }, {
                        field: 'btn', title: '', width: 10, align: 'center', formatter: function (value, row) {
                            //return '<a href="/Admin/User/SendWxMsgList.aspx?planId=' + row.PlanID + '" style="color: #0E74BF; text-decoration: underline;">发送详情</a>';
                            return FormatterLink('/Admin/User/SendWxMsgList.aspx?planId=' + row.PlanID, '发送详情');

                        }
                    }
                    
                ]]
            }
           );
        });

        function Search() {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "QuerySendWxMsgPlan", serialNum: $("#txtSerialNum").val() }
            });
        }
    </script>
</asp:Content>
