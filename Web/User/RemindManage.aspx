<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="RemindManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.RemindManage" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
   <script type="text/javascript">
       var grid;
       //处理文件路径
       var url = "/Handler/User/UserRemindManage.ashx";
       //加载文档
       jQuery().ready(function () {

           $(window).resize(function () {
               $('#list_data').datagrid('resize', {
                   width: function () { return document.body.clientWidth; },
                   height: function () { return document.documentElement.clientHeight - 80; }
               });
           });


           //-----------------加载gridview
           grid = jQuery("#list_data").datagrid({
               method: "Post",
               url: url,
               height:document.documentElement.clientHeight - 80,
               pageSize: 10,
               pagination: true,
               rownumbers: true,
               singleSelect: false,
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

           //保存---------------------
           $("#btnSave").bind("click", function () {
               Save();
           }); 
           //保存---------------------
       });

       //弹出添加或编辑框开始
       function ShowAddOrEdit(addoredit) {
           var title = ""; //弹出框标题
           var titleicon = "icon-" + addoredit; //弹出框标题图标
           //弹出添加框开始
           if (addoredit == "add") {
               //清除数据
               ClearAll();

               //设置默认值
               $("#rd_phone").attr("checked", true);
               $("#txtTheme").val("打电话");
               //设置弹出框标题
               title = "添加";

           }
           //弹出添加框结束

           //弹出编辑框开始
           else if (addoredit == "edit") {
               // 只能选择一条记录操作
               var rows = grid.datagrid('getSelections');
               var num = rows.length;
               if (num == 0) {
                   messager('系统提示', "请选择一条记录进行操作！");
                   return;
               }
               if (num > 1) {
                   $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
                   return;
               }
               // 只能选择一条记录操作

               //加载信息开始
               $("#txtTheme").val(rows[0].Title);
               $("#txtContent").val(rows[0].Content);

               $("#txtRemindTime").datetimebox("setValue", rows[0].RemindTime);
               var isenable = rows[0].IsEnable;
               if (isenable == "1") {
                   $("#rd1").attr("checked",true);
               }
               else if (isenable=="0") {
                   $("#rd0").attr("checked", true);
               }
               //加载信息结束
               //设置弹出框标题
               title = "编辑";


           }
           //弹出编辑框结束


           //弹出对话框
           $("#win").window({
               title: title,
               closed: false,
               collapsible: false,
               minimizable: false,
               maximizable: false,
               iconCls: titleicon,
               resizable: false,
               width: 400,
               height: 300,
               top: ($(window).height() - 300) * 0.5,
               left: ($(window).width() - 400) * 0.5

           });
           //弹出对话框

           //设置保存按钮属性 add为添加，edit为编辑
           $("#btnSave").attr("tag", addoredit);


       }
       //展示添加或编辑框结束


       //添加或编辑操作开始---------
       function Save() {
           var model = GetModelInfo();
           if (model.Title == "") {
               $("#txtTheme").focus();
               return false;
           }

           if (model.RemindTime == "") {
               messager("系统提示", "请选择提醒时间");
               return false;
           }


           //-------检查输入
           var action = $("#btnSave").attr("tag"); //获取添加或编辑属性
           //----------执行添加操作开始
           if (action == "add") {
               //------------添加
               jQuery.ajax({
                   type: "Post",
                   url: url,
                   data: { Action: "Add", JsonData: JSON.stringify(model).toString() },
                   success: function (result) {
                       if (result == "true") {
                           messager("系统提示", "添加成功");
                           grid.datagrid('reload');
                           $("#win").window("close");
                       } else {
                           messager("系统提示", result);
                       }
                   }
               });

               //添加---------------
           }
           //-----------执行添加操作结束
           //-----------执行编辑操作开始
           else if (action == "edit") {
               //-----------修改
               var rows = grid.datagrid('getSelections');
               var RemindID = rows[0].RemindID;
               jQuery.ajax({
                   type: "Post",
                   url: url,
                   data: { Action: "Edit", RemindID: RemindID, JsonData: JSON.stringify(model).toString()},
                   success: function (result) {
                       if (result == "true") {
                           messager("系统提示", "修改成功");
                           $("#win").window("close");
                           grid.datagrid('reload');
                       }
                       else {
                           messager("系统提示", result);
                       }
                   }
               })               //修改
           }
           //--------------执行编辑操作结束

       }
       //添加或编辑操作结束---------



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
               ids.push(rows[i].RemindID);
           }

           $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
               if (r) {
                   jQuery.ajax({
                       type: "Post",
                       url: url,
                       data: { Action: "Delete", id: ids.join(',') },
                       success: function (result) {
                           if (result) {
                               messager('系统提示', "删除成功！");
                               grid.datagrid('reload');

                               return;
                           }
                           $.messager.alert("删除失败。");
                       }
                   });
               }
           });
       };
       // 删除---------------------




       function operate(value) {

           if (value=="1") {
               return "<font color='green'>启用</font>";
           }
           else if (value == "0") {
               return "<font color='red'>停用</font>";

           }




       }
       function SetThemeText(obj) {

           var text = $(obj).text();
           $("#txtTheme").val(text)


       }

       //获取提醒数据
       function GetModelInfo() {


           var model =
            {
           
                "Title": $("#txtTheme").val(),
                "Content": $("#txtContent").val(),
                "RemindTime": $("#txtRemindTime").datetimebox("getValue"),
                "IsEnable": rd1.checked ? 1 : 0
            }

           return model;
       }

       //批量启用禁用
       function BatChangState(value) {
           var rows = grid.datagrid('getSelections');
           var num = rows.length;
           if (num == 0) {
               messager("系统提示", "请选择您要修改的记录");
               return;
           }

           var ids = [];

           for (var i = 0; i < rows.length; i++) {
               ids.push(rows[i].RemindID);
           }
           var msg = "启用";
           if (value == 0) {
               msg = "停用";
           }
           $.messager.confirm("系统提示", "是否确定" + msg + "选中信息?", function (r) {
               if (r) {
                   jQuery.ajax({
                       type: "Post",
                       url: url,
                       data: { Action: "BatChangState", Ids: ids.join(','), State: value },
                       success: function (result) {
                           if (result) {
                               messager('系统提示', "修改成功！");
                               grid.datagrid('reload');

                               return;
                           } else {
                               messager('系统提示', result);
                           }

                       }
                   });
               }
           });
       };
       //批量启用禁用

 
   </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div>
        <div class="center" style="margin: 5px;">
                    <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
    <div style="margin-bottom: 5px">
      <div>
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="ShowAddOrEdit('add')">添加</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">编辑</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="Delete()">删除</a> 
             <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="BatChangState(1)">批量启用</a>
              <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="BatChangState(0)">批量停用</a>
        </div>
        <div>
            <span style="font-size:12px;font-weight:normal">主题：</span>
             <input type="text"  style="width: 200px" id="txtName" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
        </div>
        
        
      
        

    </div>
</div>



      <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">  
    <thead>  
        <tr>
             <th field="ck" width="10" checkbox="true"></th>  
            <th field="Title" width="20" >主题</th>  
            <th field="Content" width="30">备注</th> 
            <th field="RemindTime"  width="10">提醒时间</th> 
            <th field="IsEnable"  formatter="operate" width="10">是否启用</th> 
            
        </tr>  
    </thead>  
</table> 

    <div id="win" class="easyui-window" modal="true" closed="true">
               
                    <div style="margin-left:20px">
                                 <table  >

                          
                      
<tr><td style="text-align:right">主题:</td><td>
<input type="text" id="txtTheme" style="width:250px;"class="easyui-validatebox" required="true" missingmessage="请输入主题" />


</td></tr>
<tr><td></td><td>
<input type="radio" name="rdgroup" id="rd_phone" checked="checked"  /><label for="rd_phone" onclick="SetThemeText(this)">打电话</label>
<input type="radio" name="rdgroup" id="rd_sms"  /><label for="rd_sms" onclick="SetThemeText(this)">发短信</label>
<input type="radio" name="rdgroup" id="rd_email"  /><label for="rd_email" onclick="SetThemeText(this)">发邮件</label>
<input type="radio" name="rdgroup" id="rd_other"  /><label for="rd_other" onclick="SetThemeText(this)">其它</label>

</td></tr>
<tr><td style="text-align:right">备注：</td><td>

<textarea id="txtContent" style="width:250px;height:100px;"></textarea>


</td></tr>


<tr><td style="text-align:right">提醒时间:</td><td><input id="txtRemindTime" style="width:200px" readonly="readonly" class="easyui-datetimebox" required="true"
                            validtype="datetime" missingmessage="请输入正确的时间格式" invalidmessage="请输入正确的时间格式"/></td></tr>
                            <tr><td>是否启用</td><td><input type="radio" name="rdenable" id="rd1" checked="checked" /><label for="rd1">启用</label><input type="radio" name="rdenable" id="rd0" /><label for="rd0">停用</label></td></tr>


<tr><td ></td><td>
               <br />
<a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                                确定</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                                    关 闭</a>

             </td></tr>
                    </table>
                        

                    </div>


            </div>

        </div>
    </div>

</asp:Content>

