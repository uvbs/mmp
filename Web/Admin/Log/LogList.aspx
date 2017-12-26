<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="LogList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Log.LogList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /*.panel.window{
                top: 1050px!important;
        }*/
        .window{
            top:118px !important;
        }
        .window-shadow{
            top:118px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;操作日志&nbsp;&gt;&nbsp;<span>日志管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
     <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <div>
                <span style="font-size: 12px; font-weight: normal">关键字：</span>
                <input type="text" style="width: 200px" id="txtRemark" />
                <span style="font-size: 12px; font-weight: normal">用户名：</span>
                <input type="text" style="width: 200px" id="txtUserID" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>
        </div>
    </div>

    <table id="grvData" fitcolumns="true">
    </table>

    <div id="kefuInfo" class="easyui-dialog" closed="true" modal="true" title="(双击选择)" style="width: 450px; ">
       <div>
           关键字<input type="text" id="txtTrueNameValue" style="width:300px;height:18px;">
           <a  class="easyui-linkbutton" iconcls="icon-search" id="search">搜索</a>
       </div>
        <table id="grvUserInfo" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

    <script type="text/javascript">

        var handlerUrl = "/Serv/API/Admin/Log/List.ashx";

     $(function () {

         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'id', title: '日志编号', width: 30, align: 'left', formatter: FormatterTitle },
                                {field: 'user_id', title: '用户名', width: 40, align: 'left', formatter: FormatterTitle},
                                { field: 'ip', title: 'Ip地址', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'browser_id', title: '浏览器', width: 40, align: 'left', formatter: FormatterTitle },
                                { field: 'time', title: '操作时间', width: 80, align: 'left', formatter: FormatDate },
                                { field: 'module', title: '模块', width: 60, align: 'left', formatter: FormatterTitle },
                                { field: 'action', title: '操作', width: 60, align: 'left', formatter: FormatterTitle },
                                { field: 'remark', title: '备注', width: 150, align: 'left', formatter: FormatterTitle }
	                ]]
	            }
            );


         $("#btnSearch").click(function () {
             var Remark = $("#txtRemark").val();
             var UserID = $("#txtUserID").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { keyword: Remark, user_id: UserID } });
         });


         //单击文本框
         $("#txtUserID").click(function () {
             $('#grvUserInfo').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser', HaveTrueName: 1 } });
             $('#kefuInfo').dialog('open');


         });

         $('#grvUserInfo').datagrid(
                {
                    onDblClickRow: function (rowIndex, rowData) {
                        $("#txtUserID").val(rowData["UserID"]);
                        $('#kefuInfo').dialog('close');
                    },
                    loadMsg: "正在加载数据",
                    method: "Post",
                    height: 280,
                    pagination: true,
                    striped: true,
                    singleSelect: true,
                    pageSize: 10,
                    rownumbers: true,
                    columns: [[

                                { field: 'TrueName', title: '真实姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'UserID', title: '用户名', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'WXNickname', title: '昵称', width: 100, align: 'left', formatter: FormatterTitle }
                    ]]

                }
            );

         $("#search").click(function () {
             var txtTrueName = $("#txtTrueNameValue").val();
             $('#grvUserInfo').datagrid({ url: "/Handler/App/CationHandler.ashx", queryParams: { Action: 'QueryWebsiteUser', HaveTrueName: 1, KeyWord: txtTrueName } });
         });
     });



 </script>
</asp:Content>
