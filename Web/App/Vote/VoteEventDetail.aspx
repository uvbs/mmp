﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="VoteEventDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Vote.VoteEventDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>监测明细</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            
            <a href="javascript:history.go(-1);" class="easyui-linkbutton"
                iconcls="icon-redo" plain="true" >返回</a>
            
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var voteObjectID = "Request["id"]";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryVoteEventDetail", VoteObjectID: voteObjectID },
	                height: document.documentElement.clientHeight - 145,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                singleSelect: true,
	                columns: [[
                                { field: 'EventUserWXImg', title: '微信头像', width: 50, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="微信头像" height="50" width="50" /></a>', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'EventUserWXNikeName', title: '微信昵称', width: 100, align: 'left' },
                                { field: 'SourceIP', title: 'IP地址', width: 100, align: 'left' },
                                { field: 'IPLocation', title: 'IP所在地', width: 100, align: 'left' },
                                { field: 'EventBrowserID', title: '浏览器', width: 100, align: 'left' },
	                //                                 { field: 'SpreadUserShowName', title: '推广人', width: 100, align: 'left' },
                                {field: 'EventDate', title: '访问时间', width: 100, align: 'left', formatter: FormatDate}]]
	            });
        });
    </script>
</asp:Content>
