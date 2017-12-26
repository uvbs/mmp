<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="AttentionManger.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.AttentionManger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>关注管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
      
                <span style="font-size: 12px; font-weight: normal">关注用户名：</span>
                <input type="text" style="width: 150px" id="txtUser" />
                 <span style="font-size: 12px; font-weight: normal">关注姓名：</span>
                <input type="text" style="width: 150px" id="txtName" />
                <span style="font-size: 12px; font-weight: normal">被关注用户名：</span>
                <input type="text" style="width: 150px" id="txtToUser" />
                <span style="font-size: 12px; font-weight: normal">被关注姓名：</span>
                <input type="text" style="width: 150px" id="txtToName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        
    
    <table id="grvData" fitcolumns="true">
    </table>
   
  
   
    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currSelectID = 0;
        var currAction = '';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryAttention" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[

                                { field: 'FromUserID', title: '关注人用户名', width: 20, align: 'left' },
                                { field: 'FromTrueName', title: '关注人姓名', width: 20, align: 'left' },
                                { field: 'ToUserID', title: '被关注人用户名', width: 20, align: 'left' },
                                { field: 'ToTrueName', title: '被关注人姓名', width: 20, align: 'left' }

                             ]]
	            }
            );






            $("#btnSearch").click(function () {
                var FromUserID = $("#txtUser").val();
                var FromTrueName = $("#txtName").val();
                var ToUserID = $("#txtToUser").val();
                var ToTrueName = $("#txtToName").val();
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryAttention", FromUserID: FromUserID, FromTrueName: FromTrueName, ToUserID: ToUserID, ToTrueName: ToTrueName} });
            });



        });    

    </script>
</asp:Content>

