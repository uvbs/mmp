<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="WeixinMsgDetailsManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeixinMsgDetailsManage" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">



   <script type="text/javascript">
       var grid;
       //处理文件路径
       var url = "/Handler/WeiXin/WeixinMsgDetailsManage.ashx";
       //加载文档
       jQuery().ready(function () {

           //-----------------加载gridview
           grid = jQuery("#list_data").datagrid({
               method: "Post",
               url: url,
               height: 500,
               pageSize: 20,
               pagination: true,
               rownumbers: true,
               singleSelect: true,
               queryParams: { Action: "Query", SearchTitle: "" }
           });
           //------------加载gridview

           //取消---------------------
           $("#win").find("#btnExit").bind("click", function () {
               $("#win").window("close");
           });
           //取消---------------------


           //搜索开始------------------------
           $("#btnSearch").click(function () {
               var SearchTitle = $("#txtName").val();
               grid.datagrid({ url: url, queryParams: { Action: "Query", SearchTitle: SearchTitle} });
           });
           //搜索结束---------------------
       });






       //展示编辑框---------------------
       function Edit() {
           var rows = grid.datagrid('getSelections');
           var num = rows.length;
           if (num == 0) {
               messager('系统提示', "请选择一条记录进行操作！");
               return;
           }
           if (num > 1) {
               $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行查看 。", "warning");
               return;
           }
           $("#win").window({
               title: "详细",
               closed: false,
               collapsible: false,
               minimizable: false,
               maximizable: false,
               iconCls: "icon-edit",
               resizable: false,
               width: 370,
               height: 300
           });
           // $("#btnSave").attr("tag", "Edit");
           //加载信息

           $("#lblUserID").html(rows[0].UserID);
           $("#lblUserWeixinPubOrgID").html(rows[0].UserWeixinPubOrgID);
           $("#lblReceiveOpenID").html(rows[0].ReceiveOpenID);
           $("#lblReceiveType").html(rows[0].ReceiveType);
           $("#lblReceiveLocationX").html(rows[0].ReceiveLocationX);
           $("#lblReceiveLocationY").html(rows[0].ReceiveLocationY);
           $("#lblReceiveLocationScale").html(rows[0].ReceiveLocationScale);
           $("#lblReceiveLocationLabel").html(rows[0].ReceiveLocationLabel);
           $("#lblDescription").html(rows[0].Description);


       }
       //展示编辑框--------------





 
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div>
        <div class="center" style="margin: 5px;">
                  <% if (IsManager)
             { %>

        <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
    <div style="margin-bottom: 5px">
        <div>
            <span style="font-size:12px;font-weight:normal">用户名：</span>
             <input type="text"  style="width: 200px" id="txtName" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
        </div>
        
       
        <div>
           
            <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="Edit()">详细</a>
           <%-- <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="Delete()">删除</a> --%>
        </div>
        

    </div>
</div>
<% }%>
    


      <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">  
    <thead>  
        <tr>
            <th field="ck" width="10" checkbox="true"></th>  
            <th field="UserID" width="10" >用户名</th>  
            <th field="UserWeixinPubOrgID" width="10">系统用户微信号</th> 
            <th field="ReceiveOpenID"  width="10">发送方帐号</th> 
            <th field="ReceiveType" width="10">发送类型</th>
            <th field="ReceiveLocationX" width="10">纬度</th>
            <th field="ReceiveLocationY" width="10">经度</th>
            <th field="ReceiveLocationScale" width="10">地图缩放大小</th>
            <th field="ReceiveLocationLabel" width="15">地理位置信息</th>
            <th field="Description" width="15">说明</th>
            
        </tr>  
    </thead>  
</table> 

     <div id="win" class="easyui-window" modal="true" closed="true">
               
                    <div style="margin-left:20px">
                   <table >
                        <tr><td>用户名:</td><td>  <label id="lblUserID"></label>
                       
</td></tr>
<tr><td></td><td></td></tr>
                          
                           <tr>
                            
                           <td>系统用户微信号:</td><td>
                           <label id="lblUserWeixinPubOrgID"></label>

                          
                          
</td></tr>
<tr><td>发送方帐号:</td><td>
<label id="lblReceiveOpenID"></label> 

</td></tr>

<tr><td>发送类型:</td><td>
<label id="lblReceiveType"></label> 

</td></tr>

<tr><td>纬度:</td><td>
<label id="lblReceiveLocationX"></label> 

</td></tr>
<tr><td>经度:</td><td>
<label id="lblReceiveLocationY"></label> 

</td></tr>
<tr><td>地图缩放大小:</td><td>
<label id="lblReceiveLocationScale"></label> 

</td></tr>
<tr><td>地理位置信息:</td><td>
<label id="lblReceiveLocationLabel"></label> 

</td></tr>
<tr><td>说明:</td><td>
<label id="lblDescription"></label> 

</td></tr>


<tr><td></td><td>
               <br />
 <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                                    关 闭</a>

             </td></tr>
                    </table>
                        

                    </div>

    

            </div>

        </div>
    </div>

</asp:Content>

