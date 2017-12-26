<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="WeixinMemberInfoManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeixinMemberInfoManage" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
 


   <script type="text/javascript">
       var grid;
       //处理文件路径
       var url = "/Handler/WeiXin/WeixinMemberInfoManage.ashx";
       //加载文档
       jQuery().ready(function () {

           //-----------------加载gridview
           grid = jQuery("#list_data").datagrid({
               method: "Post",
               url: url,
               height: 390,
               pageSize: 10,
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
           $("#lblWeixinOpenID").html(rows[0].WeixinOpenID);
           $("#lblUserWeixinPubOrgID").html(rows[0].UserWeixinPubOrgID);
           $("#lblName").html(rows[0].Name);
           $("#lblGender").html(rows[0].Gender);
           $("#lblMobile").html(rows[0].Mobile);
           $("#lblEmail").html(rows[0].Email);
           $("#lblFirstVisitDate").html(rows[0].FirstVisitDate);
           $("#lblLastVisitDate").html(rows[0].LastVisitDate);
           $("#lblRegDate").html(rows[0].RegDate);
           $("#lblCurrAction").html(rows[0].CurrAction);


       }
       //展示编辑框--------------


       function operate(value) {
           var str = "";
           if (value=="1") {
               str = "<font color='green'>已注册</font>";
           }
           else if (value=="0") {
               str = "<font color='red'>未注册</font>";
       }
       return str;
       
       }


 
   </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div>
        <div class="center" style="margin: 5px;">
<%--                    <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
    <div style="margin-bottom: 5px">
        <div>
            <span style="font-size:12px;font-weight:normal">用户名：</span>
             <input type="text"  style="width: 200px" id="txtName" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
        </div>
        
       
        <div>
           
            <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="Edit()">详细</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="Delete()">删除</a> 
        </div>
        

    </div>
</div>
--%>


      <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">  
    <thead>  
        <tr>
            <th field="ck" width="10" checkbox="true"></th>  
           
            <th field="WeixinOpenID" width="10">微信OpenID</th> 
            <th field="UserWeixinPubOrgID"  width="10">公众号ID</th> 
            <th field="Name" width="10">姓名</th>
            <th field="Gender" width="10">性别</th>
            <th field="Mobile" width="10">手机号</th>
            <th field="Email" width="10">邮箱</th>
            <th field="FirstVisitDate" width="15">初次访问时间</th>
            <th field="LastVisitDate" width="15">最后访问时间</th>
            <th field="RegDate" width="15">注册时间</th>
            <th field="CurrAction" width="10">用户当前操作</th>
            <th field="IsRegMember" formatter="operate" width="10">注册状态</th>
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
                            
                           <td>微信OpenID:</td><td>
                           <label id="lblWeixinOpenID"></label>

                          
                          
</td></tr>
<tr><td>公众号ID:</td><td>
<label id="lblUserWeixinPubOrgID"></label> 

</td></tr>

<tr><td>姓名:</td><td>
<label id="lblName"></label> 

</td></tr>
<tr><td>性别:</td><td>
<label id="lblGender"></label> 

</td></tr>
<tr><td>手机号:</td><td>
<label id="lblMobile"></label> 

</td></tr>
<tr><td>邮箱:</td><td>
<label id="lblEmail"></label> 

</td></tr>
<tr><td>初次访问时间:</td><td>
<label id="lblFirstVisitDate"></label> 

</td></tr>
<tr><td>最后访问时间:</td><td>
<label id="lblLastVisitDate"></label> 

</td></tr>
<tr><td>注册时间:</td><td>
<label id="lblRegDate"></label> 

</td></tr>
<tr><td>用户当前操作:</td><td>
<label id="lblCurrAction"></label> 

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



