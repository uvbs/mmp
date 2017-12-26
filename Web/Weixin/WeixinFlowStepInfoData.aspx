<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true"
    CodeBehind="WeixinFlowStepInfoData.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeixinFlowStepInfoData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


         var grid;
         
         //处理文件路径
         var url = "/Handler/WeiXin/WeiXinFlowStepInfoData.ashx";
         
         //传入的流程ID
         var FlowID = <%=FlowID %>;


         //加载文档
         jQuery().ready(function () {
               $(window).resize(function () {
                $(list_data).datagrid('resize',
	            {
	                width: document.body.clientWidth - 10,
	                height: document.documentElement.clientHeight - 55
	            });
            });
             grid = jQuery("#list_data").datagrid({
                 method: "Post",
                 url: url,
                 height: document.documentElement.clientHeight - 55,
                 pageSize: 100,
                 pagination: true,
                 rownumbers: true,
                 singleSelect: false,
                 queryParams: { Action: "Query",FlowID:FlowID ,SearchTitle: "" }
          
             });
             });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="center" style="margin: 5px;">
            <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
                <table style="width: 100%">
                    <tr>
                        <td>
                            流程名称：<%=FlowName%>
                        </td>
                        <td>
                            <a style="float: right;" href="/Weixin/WeiXinFlowInfoManage.aspx" class="easyui-linkbutton"
                                iconcls="icon-back" plain="true">返回流程管理</a>
                        </td>
                    </tr>
                </table>
            </div>
            <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                        <%=Columns %>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
</asp:Content>
