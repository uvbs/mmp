<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="UseRecord.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.SVCard.Record.UseRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;储值卡消费记录

    <a href="List.aspx?card_id=<%= this.Request["card_id"] %>" style="float: right; margin-right: 20px; color: Black;" title="返回" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <table id="grvData" fitcolumns="true">
    </table>
   
  
   
   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Serv/Api/Admin/SvCard/UseRecord/List.ashx";
     var currSelectID = 0;
     var currAction = '';
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { id: "<%=Request["id"]%>" },
	                height: document.documentElement.clientHeight - 112,
	                //pagination: true,
	                striped: true,
	                //pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                
                                
                                { field: 'use_date', title: '使用日期', width: 20, align: 'left' },
                                { field: 'remark', title: '备注', width: 80, align: 'left' }


	                ]]
	            }
            );




     });

      
     
    </script>
</asp:Content>