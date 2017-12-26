<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="WeinxinMsgSourceInfoManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeinxinMsgSourceInfoManage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">

   

   <script type="text/javascript">


       KindEditor.ready(function (K) {
           var editor = K.editor({
               allowFileManager: true,
               uploadJson: '/Kindeditor/asp.net/upload_json_ChangeName.ashx'

           });
           $('#btnupload').click(function () {
               editor.loadPlugin('image', function () {
                   editor.plugin.imageDialog({
                       showRemote: false,
                       imageUrl: $('#txtImgSrc').val(),
                       clickFn: function (url, title, width, height, border, align) {
                         var newurl="http://"+ window.location.host+url;
                           $('#txtImgSrc').val(newurl);
                           editor.hideDialog();
                       }
                   });
               });
           });
       });
   </script>

   <script type="text/javascript">
       var grid;
       //处理文件路径
       var url = "/Handler/WeiXin/WeiXinMsgSourceInfoMng.ashx";
       //加载文档
       jQuery().ready(function () {


           $(window).resize(function () {
               $(list_data).datagrid('resize',
	            {
	                width: document.body.clientWidth-10,
	                height: document.documentElement.clientHeight - 75
	            });
           });
           //-----------------加载gridview
           grid = jQuery("#list_data").datagrid({
               method: "Post",
               url: url,
               height: document.documentElement.clientHeight - 75,
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

//               var tag = jQuery.trim(jQuery(this).attr("tag"));

//               if (tag == "add") {
//                   //添加
//                   Add();
//                   return;
//               }
//               //修改
               Save();
           });
       });
       //保存---------------------


       //弹出添加或编辑框开始
       function ShowAddOrEdit(addoredit) {
           var title = ""; //弹出框标题
           var titleicon = "icon-" + addoredit; //弹出框标题图标
           //弹出添加框开始
           if (addoredit == "add") {
               //清除数据
               Clear("txtSourceName|txtUrl|txtImgSrc|txtDescription");
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
               $("#txtSourceName").val(rows[0].Title);
               $("#txtImgSrc").val(rows[0].PicUrl);
               $("#txtUrl").val(rows[0].Url);
               var description = replacebrtag(rows[0].Description);
               $("#txtDescription").val(description);
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
               width: 370,
               height: 250,
               top: ($(window).height() - 250) * 0.5,
               left: ($(window).width() - 370) * 0.5

           });
           //弹出对话框

           //设置保存按钮属性 add为添加，edit为编辑
           $("#btnSave").attr("tag", addoredit);


       }
       //展示添加或编辑框结束


       //添加或编辑操作开始---------
       function Save() {
           var SourceName = $("#txtSourceName").val();
           var PicUrl = $("#txtImgSrc").val();
           var LinkUrl = $("#txtUrl").val();
           var Description = $("#txtDescription").val();

           //--检查输入---------------
           if (SourceName == "") {
               $("#txtSourceName").focus();
               return false;
           }
           if (PicUrl == "") {
               $("#txtImgSrc").focus();
               return false;
           }
           var action = $("#btnSave").attr("tag"); //获取添加或编辑属性
           //----------执行添加操作开始
           if (action == "add") {
               //------------添加
               jQuery.ajax({
                   type: "Post",
                   url: url,
                   data: { Action: "Add", SourceName: SourceName, PicUrl: PicUrl, LinkUrl: LinkUrl, Description: Description },
                   success: function (result) {
                       if (result == "true") {
                           messager("系统提示", "添加成功");
                           $("#win").window("close");
                           grid.datagrid('reload');
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
               var SourceID = rows[0].SourceID;
                jQuery.ajax({
                   type: "Post",
                   url: url,
                   data: { Action: "Edit", SourceID: SourceID, SourceName: SourceName, PicUrl: PicUrl, LinkUrl: LinkUrl, Description: Description },
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
               });
               //修改
           }
           //--------------执行编辑操作结束

       }
       //添加或编辑操作结束---------


//       //添加信息输入框---------------------
//       function ShowAdd() {
//           $("#win").window({
//               title: "添加",
//               closed: false,
//               collapsible: false,
//               minimizable: false,
//               maximizable: false,
//               iconCls: "icon-add",
//               resizable: false,
//               width: 370,
//               height: 250
//           });
//           //设置保存按钮目标为添加
//           $("#btnSave").attr("tag", "add");
//           //清除数据
//           Clear();


//       }
//       //添加信息输入框---------------------


//       //保存添加的信息---------------------
//       function Add() {
//           var SourceName = $("#txtSourceName").val();
//           var PicUrl = $("#txtImgSrc").val();
//           var LinkUrl = $("#txtUrl").val();
//           var Description = $("#txtDescription").val();

//           //--检查输入---------------
//           if (SourceName == "") {
//               $("#txtSourceName").focus();
//               return false;
//           }
//           if (PicUrl == "") {
//               $("#txtImgSrc").focus();
//               return false;
//           }
//           //-------检查输入
//           jQuery.ajax({
//               type: "Post",
//               url: url,
//               data: { Action: "Add", SourceName: SourceName, PicUrl: PicUrl, LinkUrl: LinkUrl, Description: Description },
//               success: function (result) {
//                   if (result == "true") {
//                       messager("系统提示", "添加成功");
//                       $("#win").window("close");
//                       grid.datagrid('reload');
//                   } else {
//                       messager("系统提示", result);
//                   }
//               }
//           });
//       };
//       //保存添加的信息---------------------


//       // 修改信息---------------------
//       function Save() {
//           var rows = grid.datagrid('getSelections');
//           var SourceID = rows[0].SourceID;
//           var SourceName = $("#txtSourceName").val();
//           var PicUrl = $("#txtImgSrc").val();
//           var LinkUrl = $("#txtUrl").val();
//           var Description = $("#txtDescription").val();

//           //--检查输入---------------
//           if (SourceName == "") {
//               $("#txtSourceName").focus();
//               return false;
//           }
//           if (PicUrl == "") {
//               $("#txtImgSrc").focus();
//               return false;
//           }
//           //-------检查输入


//           //           if (websort == "") {
//           //               $("#txtSort").focus();
//           //               return false;
//           //           }
//           //-------检查输入

//           jQuery.ajax({
//               type: "Post",
//               url: url,
//               data: { Action: "Edit", SourceID: SourceID, SourceName: SourceName, PicUrl: PicUrl, LinkUrl: LinkUrl, Description: Description },
//               success: function (result) {
//                   if (result == "true") {
//                       messager("系统提示", "修改成功");
//                       $("#win").window("close");
//                       grid.datagrid('reload');
//                   }
//                   else {
//                       messager("系统提示", result);
//                   }
//               }
//           });
//       }
//       // 修改信息---------------------


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
               ids.push(rows[i].SourceID);
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


//       //展示编辑框---------------------
//       function Edit() {
//           var rows = grid.datagrid('getSelections');
//           var num = rows.length;
//           if (num == 0) {
//               messager('系统提示', "请选择一条记录进行操作！");
//               return;
//           }
//           if (num > 1) {
//               $.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
//               return;
//           }
//           $("#win").window({
//               title: "修改",
//               closed: false,
//               collapsible: false,
//               minimizable: false,
//               maximizable: false,
//               iconCls: "icon-edit",
//               resizable: false,
//               width: 370,
//               height: 250
//           });
//           $("#btnSave").attr("tag", "Edit");
//           //加载信息
//           $("#txtSourceName").val(rows[0].Title);
//           $("#txtImgSrc").val(rows[0].PicUrl);
//           $("#txtUrl").val(rows[0].Url);
//           var description = replacebrtag(rows[0].Description);
//           $("#txtDescription").val(description);

//       }
//       //展示编辑框--------------


//       //清除数据-----------
//       function Clear() {
//           $("#txtSourceName").val("");
//           $("#txtUrl").val("");
//           $("#txtImgSrc").val("");
//           $("#txtDescription").val("");
//       }

//       //清除数据-----------

       //网站图片--------------
       function operate(value, row, index) {


           var d = "<img src=" + value + " width=100 height=100 >";

           return d;

       }
       //网站图片--------------

       //网站链接--------------
       function gotowebsite(value, row, index) {


           var d = "<a href=" + value + " title=" + row.WebsiteName + " taget=_blank>" + value + "</a>";

           return d;

       }
       //网站链接--------------
 
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div>
        <div class="center" style="margin: 5px;">
                    <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
   
     
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="ShowAddOrEdit('add')">添加</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">编辑</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="Delete()">删除</a> 
             <br />
            <span style="font-size:12px;font-weight:normal">标题名称：</span>
             <input type="text"  style="width: 200px" id="txtName" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
        
        
        
       
        

   
</div>


      <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">  
    <thead>  
        <tr>

            <th field="ck" width="10" checkbox="true"></th> 
             <th field="PicUrl" formatter="operate" width="20">图片</th>  
            <th field="Title" width="20" >图片标题</th>  
            <th field="Url"  width="30">链接</th> 
            <th field="Description"  width="30">描述</th>  
            
            
        </tr>  
    </thead>  
</table> 

     <div id="win" class="easyui-window" modal="true" closed="true">
                <form id="fm" method="post" novalidate>
                    <div style="margin-left:20px">
                     <table>
                        <tr><td>标题名称:</td>
                        <td> <input name="Title" id="txtSourceName" type="text" class="easyui-validatebox" required="true" missingmessage="请输入标题名称" />
                         </td></tr>
                         
                            <tr><td>图片:</td>
                            
                            <td> <input type="text" id="txtImgSrc" value="" class="easyui-validatebox" required="true" missingmessage="请输入图片地址或上传图片" /> <input type="button" id="btnupload" value="上传图片" /></td></tr>

                             <tr><td>链接:</td>
                             <td> <input name="Title" id="txtUrl" type="text" class="easyui-validatebox" required="true" missingmessage="图片链接地址  格式如 http://www.baidu.com" />
                            </td></tr>
                    <tr><td>描述:</td><td>
                    <textarea id="txtDescription" style="width:150px;height:50px"></textarea>
                    </td></tr>
                     <tr><td></td><td>
               <br />
<a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                                保 存</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                                    关 闭</a>

             </td></tr>
                    </table>
                        

                    </div>

    </form>

            </div>

        </div>
    </div>

</asp:Content>


