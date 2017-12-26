<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="WeixinReplyNewsRuleInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WeixinReplyNewsRuleInfo" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">


   <script type="text/javascript">
       var grid; //回复规则列表
       var gridsource; //素材列表

       //处理文件路径
       var url = "/Handler/WeiXin/WeixinReplyRuleInfoNewsManage.ashx";

 

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
               queryParams: { Action: "Query", SearchTitle: "" },
               onClickRow: function (value, row) {
                $("#div_imagelist").html(row.RelpayImageList);
                
                
                }
           });
           //------------加载gridview



          //-加载未添加到此用户的素材列表------------------------
           gridsource = jQuery("#list_source").datagrid({
               idField: "SourceID", 
               view: fileview ,
               method: "Post",
               url: url,
               height: 350,
               pageSize: 10,
               pagination: true,
               rownumbers: true,
               singleSelect: false,
               queryParams: { Action: "QuerySource", SearchTitle: "" }
           });       
          //-加载未添加到此用户的素材列表------------------------




           //搜索未添加到此用户的素材列表------------------------
           $("#btnSearch_Source").click(function () {
               var SearchTitle = $("#txtSourceName").val();
               gridsource.datagrid({ url: url, queryParams: { Action: "QuerySource", SearchTitle: SearchTitle} });
           });
           //搜索未添加到此用户的素材列表------------------------



           //取消---------------------
           $("#btnExit").bind("click", function () {
               $("#win").window("close");
           });
           //取消---------------------

           //取消---------------------
           $("#btnExit_Source").bind("click", function () {
               $("#win_source").window("close");
           });
           //取消---------------------

//                      //取消---------------------
//           $("#btnExit_Image").bind("click", function () {
//               $("#win_viewimagelist").window("close");
//           });
//           //取消---------------------

           


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
           //保存---------------------

       }); //文档加载结束



       //弹出添加或编辑框开始
       function ShowAddOrEdit(addoredit) {
           var title = ""; //弹出框标题
           var titleicon = "icon-" + addoredit; //弹出框标题图标
           //弹出添加框开始
           if (addoredit == "add") {
               //清除数据
               Clear("txtKeyWord|ddlMatchType|txtSelectSourceName");
               //设置默认值
               $("#div_selectsourcelist").html("");
               $("#btnSelectImage").val("选择素材");

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
               //加载信息
               $("#txtKeyWord").val(rows[0].MsgKeyword);
               $("#ddlMatchType").val(rows[0].MatchType);
               $("#btnSelectImage").val("重新选择素材");
               //加载图文
               getimagelist(rows[0].UID);
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
               height: 400,
               top: ($(window).height() - 400) * 0.5,
               left: ($(window).width() - 400) * 0.5

           });
           //弹出对话框

           //设置保存按钮属性 add为添加，edit为编辑
           $("#btnSave").attr("tag", addoredit);


       }
       //展示添加或编辑框结束


       //添加或编辑操作开始---------
       function Save() {
           var keyword = $("#txtKeyWord").val();
           var matchtype = $("#ddlMatchType").val();
           var sourceids = GetSelectImage();

           //--检查输入---------------
           if (keyword == "") {
               $("#txtKeyWord").focus();
               return false;
           }
           if (sourceids == "") {
               messager("系统提示", "请先选择素材");
               return false;
           }
           var action = $("#btnSave").attr("tag"); //获取添加或编辑属性
           //----------执行添加操作开始
           if (action == "add") {
               //------------添加
               jQuery.ajax({
                   type: "Post",
                   url: url,
                   data: { Action: "Add", MsgKeyword: keyword, MatchType: matchtype, SourceIds: sourceids },
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
               var uid = rows[0].UID;
               var sourcetype = $("#tb_source").attr("sourcetype");
               jQuery.ajax({
                   type: "Post",
                   url: url,
                   data: { Action: "Edit", UID: uid, MsgKeyword: keyword, MatchType: matchtype, SourceIds: sourceids, SourceType: sourcetype },
                   success: function (result) {
                       if (result == "true") {
                           messager("系统提示", "修改成功");
                           $("#win").window("close");
                           grid.datagrid('reload');
                       } else {
                           messager("系统提示", result);
                       }
                   }
               });
               //修改
           }
           //--------------执行编辑操作结束

       }
       //添加或编辑操作结束---------


       var fileview = $.extend({}, $.fn.datagrid.defaults.view, { onAfterRender: function (target) { ischeckItem(); } });

       var checkedItems = [];
       function ischeckItem() {
           for (var i = 0; i < checkedItems.length; i++) {
               gridsource.datagrid('selectRecord', checkedItems[i]); //根据id选中行 
           }
       }

       function findCheckedItem(ID) {
           for (var i = 0; i < checkedItems.length; i++) {
               if (checkedItems[i] == ID) return i;
           }
           return -1;
       }

       function addcheckItem() {
           var row = gridsource.datagrid('getChecked');
           for (var i = 0; i < row.length; i++) {
               if (findCheckedItem(row[i].SourceID) == -1) {
                   checkedItems.push(row[i].SourceID);
               }
           }
       }
       function removeAllItem(rows) {

           for (var i = 0; i < rows.length; i++) {
               var k = findCheckedItem(rows[i].SourceID);
               if (k != -1) {
                   checkedItems.splice(i, 1);
               }
           }
       }
       function removeSingleItem(rowIndex, rowData) {
           var k = findCheckedItem(rowData.SourceID);
           if (k != -1) {
               checkedItems.splice(k, 1);
           }
       }





       //添加素材弹出框---------------------
       function ShowAddSource() {
           $("#win_source").window({
               title: "选择素材",
               closed: false,
               collapsible: false,
               minimizable: false,
               maximizable: false,
               iconCls: "icon-add",
               resizable: false,
               width: 500,
               height: 470,
              top:($(window).height() - 500) * 0.5,   
              left:($(window).width() - 470) * 0.5
           });


       }
       //添加素材弹出框---------------------


//       //添加信息输入框---------------------
//       function ShowAdd() {
//           $("#win").window({
//               title: "添加",
//               closed: false,
//               collapsible: false,
//               minimizable: false,
//               maximizable: false,
//               iconCls: "icon-add",
//               resizable: true,
//               width: 400,
//               height: 400
//           });
//           //设置保存按钮目标为添加
//           $("#btnSave").attr("tag", "add");
//           //清除数据
//           Clear();


//       }
//       //添加信息输入框---------------------


//       //保存添加的信息---------------------
//       function Add() {
//           var keyword = $("#txtKeyWord").val();
//           var matchtype = $("#ddlMatchType").val();
//           var sourceids = GetSelectImage();

//           //--检查输入---------------
//           if (keyword == "") {
//               $("#txtKeyWord").focus();
//               return false;
//           }
//           if (sourceids == "") {
//               messager("系统提示", "请先选择素材");
//               return false;
//           }
//           //-------检查输入
//           jQuery.ajax({
//               type: "Post",
//               url: url,
//               data: { Action: "Add", MsgKeyword: keyword, MatchType: matchtype, SourceIds: sourceids },
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


        //--------------获取选中素材
       function GetSelectImage() {
           var ids = [];
           $(".imgclass").each(function () {
               var id = $(this).attr("id");
               ids.push(id);
              
           });
           return  ids.join(',');

       }
       //------------获取选中素材

       //删除选中素材
       function delselectimage(id){

       $("#"+id).parent().parent().remove();
       
       }
       //删除选中素材

//       //保存添加的信息---------------------
//       function Save() {

//           var rows = grid.datagrid('getSelections');    
//           var keyword = $("#txtKeyWord").val();
//           var matchtype = $("#ddlMatchType").val();
//           var sourceids=GetSelectImage();
//           var uid=rows[0].UID;
//           var sourcetype=$("#tb_source").attr("sourcetype");
//         
//           //--检查输入---------------
//           if(keyword == "") {
//               $("#txtKeyWord").focus();
//               return false;
//           }
//           if (sourceids == "") {
//               messager("系统提示", "请先选择素材");
//               return false;
//           }
//           //-------检查输入
//           jQuery.ajax({
//               type: "Post",
//               url: url,
//               data: { Action: "Edit",UID:uid, MsgKeyword: keyword, MatchType: matchtype, SourceIds: sourceids ,SourceType:sourcetype},
//               success: function (result) {
//                   if (result == "true") {
//                       messager("系统提示", "修改成功");
//                       $("#win").window("close");
//                       grid.datagrid('reload');
//                   } else {
//                       messager("系统提示", result);
//                   }
//               }
//           });
//       };
//       //保存添加的信息---------------------




       // 删除---------------------
       function Delete() {
           var rows = grid.datagrid('getSelections');
           var num = rows.length;
           if (num == 0) {
               messager("系统提示", "请选择您要删除的规则");
               return;
           }
           var ids = [];

           for (var i = 0; i < rows.length; i++) {
               ids.push(rows[i].UID);
           }

           $.messager.confirm("系统提示", "是否确定删除选中规则?", function (r) {
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




//       //清除数据-----------
//       function Clear() {
//           $("#txtKeyWord").val("");
//           $("#ddlMatchType").val("");
//           $("#txtSelectSourceName").val("");
//           $("#div_selectsourcelist").html("");
//           $("#btnSelectImage").val("选择素材");

//       }

//       //清除数据-----------

       //图片--------------
       function getimage(value, row, index) {
         
           var str = "";
           if (value!="") {
         
              str= "<img src=" + value + " width=100 height=100 >";          
         }
           return str;

       }
//       //图片--------------


       function operate(value, row, index) {


           return "<a href='#' title='查看图文' onclick='edit_image()'>查看图文</a>";



       }
       //查看图文信息
         function edit_image() {
           
            $("#win_viewimagelist").window({
                title: "查看图文",
                closed: false,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                iconCls: "icon-edit",
                resizable: true,
                width: 400,
                height: 400,
                top:($(window).height() - 400) * 0.5,   
                left:($(window).width() - 400) * 0.5

            });
        }
        //查看图文信息

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
//               resizable: true,
//               width: 400,
//               height: 400
//           });

//           $("#btnSave").attr("tag", "Edit");
//           //加载信息
//           $("#txtKeyWord").val(rows[0].MsgKeyword);
//           $("#ddlMatchType").val(rows[0].MatchType);
//           //$("#div_selectsourcelist").html(rows[0].RelpayImageList);
//           $("#btnSelectImage").val("重新选择素材");
//           //加载图文
//           getimagelist(rows[0].UID);

//       }
//       //展示编辑框--------------

        // 选择素材---------------------
       function SelectSource() {
           var rows = gridsource.datagrid('getSelections');
           var num = rows.length;
           if (num == 0) {
               messager("系统提示", "请选择素材");
               return;
           }
           if (num>10) {
               messager("系统提示", "您选中了"+num+"个素材，最多只能同时选择10个素材");
               return;
           }
           var ids = [];
           var strselectsourcename = "";
           var selectsourcelist = "<table id=tb_source sourcetype=source>";//选中的素材
           for (var i = 0; i < rows.length; i++) {
               ids.push(rows[i].SourceID);
               selectsourcelist += "<tr><td>" + "<img class=imgclass id="+rows[i].SourceID+" alt=" + rows[i].Title + " src=" + rows[i].PicUrl + " width=100 height=100 >" + "</td><td>标题：" + rows[i].Title + "</br>描述：" + rows[i].Description + "</br>链接:" + rows[i].Url + "<br/>"+"<br/><a href='#' title='删除' onclick='delselectimage("+rows[i].SourceID+")'>删除</a>" +"</td></tr>";
           }
           selectsourcelist += "</table>";
               //$("#hdsourceids").val(ids); //保存选中的素材
               $("#div_selectsourcelist").html(selectsourcelist);
                $("#btnSelectImage").val("重新选择素材");
               $("#win_source").window("close");//关闭选择素材窗口

       };
       // 选择素材---------------------



       //获取规则图文列表
       function getimagelist(uid){
       $.post(url,{Action:"GetImageList",UID:uid},function (data) {
                var strhtml="<table id=tb_source sourcetype=imagelist>";
                var objData = eval(data);
                $.each(objData,function(index,item){
                 strhtml += "<tr><td>" + "<img class=imgclass id="+item.UID+" alt=" + item.Title + " src=" + item.PicUrl + " width=100 height=100 >" + "</td><td>标题：" + item.Title + "</br>描述：" + item.Description + "</br>链接:" + item.Url + "<br>"+"<br/><a href='#' title='删除' onclick='delselectimage("+item.UID+")'>删除</a>" +"</td></tr>";
              });
                strhtml+="</table>";
                $("#div_selectsourcelist").html(strhtml);
                if (data=="") {
    
                        }
            });
       
       }
       //获取规则图文列表


   </script>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div>
        <div class="center" style="margin: 5px;">
                    <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
    
    <div>
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="ShowAddOrEdit('add')">添加</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">编辑</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="Delete()">删除</a> 
        </div>
        <div>
            <span style="font-size:12px;font-weight:normal">关键字：</span>
             <input type="text"  style="width: 200px" id="txtName" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
        </div>
        
      
        
        

   
</div>


      <table id="list_data" cellspacing="0" cellpadding="0" fitcolumns="true">  
        <thead>  
        <tr>
             <th field="ck" width="10" checkbox="true"></th>  
            <th field="MsgKeyword" width="20" >关键字</th>  
            <th field="MatchType" width="10">匹配类型</th> 
          <%--  <th field="ReplyContent"  width="100">回复内容</th> --%>
           <%-- <th field="ReplyType" formatter="operate" width="10">回复类型</th>  --%>
            <th field="RelpayImageList" formatter="operate"   width="10">图文</th>  
            
        </tr>  
    </thead>  
</table> 



     <div id="win" class="easyui-window" modal="true" closed="true">
               
                    <div style="margin-left:20px">
                                 <table >
                        <tr><td>关键字名称:</td><td>  <input  id="txtKeyWord" type="text" class="easyui-validatebox" required="true" missingmessage="请输入关键字名称" />
                       
</td></tr>
<tr><td></td><td></td></tr>
                          
  <tr>
                            
                           <td>匹配类型:</td><td>
                          
                            <select id="ddlMatchType" style="width:150px">
                           <option value="全文匹配">
                           全文匹配
                           </option>
                           <option value="开始匹配">
                           开始匹配
                           </option>
                         <option value="结束匹配">
                           结束匹配
                           </option>
                          <option value="包含匹配">
                           包含匹配
                           </option>

                            </select>
</td></tr>
<tr><td>素材:</td><td>


<%--选中的素材在这里显示
--%>
<div id="div_selectsourcelist">


</div>
<%--选中的素材在这里显示
--%>

<input type="button"  id="btnSelectImage" onclick="ShowAddSource()" value="选择素材" />


</td></tr>

<tr><td></td><td>
               <br />
<a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton" iconcls="icon-ok">
                                保 存</a> <a href="javascript:void(0)" id="btnExit" class="easyui-linkbutton" iconcls="icon-no">
                                    关 闭</a>

             </td></tr>
                    </table>
                        

                    </div>

    

            </div>




    <div id="win_source" class="easyui-window" modal="true" closed="true" style="padding: 10px;
                text-align: center;">
                <div style="text-align: left">
                    <span style="font-size: 12px; font-weight: normal;">素材名称:</span>
                    <input id="txtSourceName" style="width: 200px" />
                    <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch_Source">
                        查询</a>
                </div>
              
                     <table id="list_source" cellspacing="0" cellpadding="0" fitcolumns="true">
                     <thead>  
                    <tr>

                    <th field="ck" width="10" checkbox="true"></th>  
                
                     <th field="Title" width="60" >标题</th>  
                     <th field="PicUrl" formatter="getimage"  width="30">图片</th> 
                     <%--<th field="Url"  width="30">图片链接</th> --%>
                  <%--   <th field="Description"  width="30">描述</th>  --%>
            
            
                 </tr>  
                </thead>  
                 </table>

                <div>
                    <br />
                    <a href="javascript:void(0)" id="btnSelectSource" onclick="SelectSource()" class="easyui-linkbutton" iconcls="icon-ok">
                        确定</a> <a href="javascript:void(0)" id="btnExit_Source" class="easyui-linkbutton" iconcls="icon-no">
                            关 闭</a>
                </div>
            </div>


     <div id="win_viewimagelist" class="easyui-window" modal="true" closed="true" style="padding: 10px;
                text-align: center;">

                <div style="text-align: left" id="div_imagelist">


                </div>

                <div>
                   
                    <%--<a href="javascript:void(0)" id="btnExit_Image" class="easyui-linkbutton" iconcls="icon-no">
                            关 闭</a>--%>
                </div>
            </div>


        </div>
    </div>

</asp:Content>
