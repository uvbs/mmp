<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="SignHistoryList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.SignHistoryList" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">


   <script type="text/javascript">
       var grid;
       //处理文件路径
       var url = "/Handler/SignHistoryManage.ashx";
       //加载文档
       jQuery().ready(function () {



           //-----------------加载gridview
           grid = jQuery("#list_data").datagrid({
               method: "Post",
               url: url,
               height: 390,
               pageSize: 20,
               pagination: true,
               rownumbers: true,
               singleSelect: false,
               queryParams: { Action: "Query", SearchTitle: "" }
           });
           //------------加载gridview



           //搜索开始------------------------
           $("#btnSearch").click(function () {
               grid.datagrid('reload');
           });
           //搜索结束---------------------

       });



       // 删除---------------------
       function Delete() {
           var rows = grid.datagrid('getSelections');
           var num = rows.length;
           if (num == 0) {
               messager("系统提示", "请选择您要删除的记录");
               return;
           }
           var ids = [];

           for (var i = 0; i < rows.length; i++) {
               ids.push(rows[i].SignID);
           }

           $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
               if (r) {
                   jQuery.ajax({
                       type: "Post",
                       url: url,
                       data: { Action: "Delete", id: ids.join(',') },
                       success: function (result) {
                           if (result=="true") {
                               messager('系统提示', "删除成功！");
                               grid.datagrid('reload');

                               return;
                           }
                           else{ messager('系统提示', r);}
                          
                       }
                   });
               }
           });
       };
       // 删除---------------------





 
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div>
        <div class="center" style="margin: 5px;">
                    <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
    <div style="margin-bottom: 5px">
        <div>
            <a href="#" class="easyui-linkbutton" iconcls="icon-reload" id="btnSearch">刷新</a>
        </div>
        <br />
          <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="Delete()">删除</a> 
        
        
        

    </div>
</div>



      <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">  
    <thead>  
        <tr>
             <th field="ck" width="10" checkbox="true"></th> 
              <th field="AddDate" width="20" >时间</th>  
               <th field="Address"  width="40">地点</th> 
            <th field="Latitude" width="20" >经度</th>  
            <th field="Longitude" width="20">纬度</th> 
           
          
            
        </tr>  
    </thead>  
</table> 

   

        </div>
    </div>

</asp:Content>