<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ApplyPostionRecord.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.ApplyPostionRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script type="text/javascript">

       var handlerUrl = "/Handler/App/WXWuBuHuiPosintionHandler.ashx";
       var currSelectID = 0;
       var currAction = '';
       var grid;
       $(function () {

           //-----------------加载gridview
           grid = jQuery("#grvData").datagrid({
               method: "Post",
               url: handlerUrl,
               height: document.documentElement.clientHeight - 160,
               toolbar: '#divToolbar',
               //idField: "AutoID",
               //view: fileview,
               pageSize: 50,
               fitCloumns: true,
               pagination: true,
               rownumbers: true,
               singleSelect: true,
               queryParams: { Action: "QueryApplyPostition", PostionId: "<%=Request["id"]%>" }
           });






       });



    





       //批量删除
       function Delete() {
           var rows = grid.datagrid('getSelections');
           if (!EGCheckIsSelect(rows)) {
               return;
           }
           $.messager.confirm('系统提示', '确定删除选中记录？', function (o) {
               if (o) {
                   var ids = new Array();
                   for (var i = 0; i < rows.length; i++) {
                       ids.push(rows[i].AutoId);
                   }
                   $.ajax({
                       type: "Post",
                       url: handlerUrl,
                       data: { Action: "DelApplyPositionInfo", ids: ids.join(',') },
                       dataType: "json",
                       success: function (resp) {
                           if (resp.Status == 1) {
                               Alert("删除成功!");
                           }
                           else {
                               Alert("删除失败!");
                           }
                           grid.datagrid('reload');

                       }
                   });
               }
           });
       }

   </script>
    <style type="text/css">
        .style1
        {
            width: 29%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>申请记录 </span>
     <a href="PositionInfoMag.aspx" style="float: right; margin-right: 20px;" title="返回"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
                 <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="UserName" width="100">
                            姓名
                        </th>
                        <th field="Phone" width="100">
                            手机
                        </th>
                        <th field="Trade" width="100">
                            行业
                        </th>
                       <th field="Professional" width="100">
                            专业
                        </th>
                       
                    </tr>
                </thead>
</table>

</asp:Content>
