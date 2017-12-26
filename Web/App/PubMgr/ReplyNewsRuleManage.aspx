<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ReplyNewsRuleManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.ReplyNewsRuleManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #tb_source tbody tr{
            cursor:move;
            display: block;
        }
       #tb_source tbody tr:hover{
           
           border: 2px dashed #bdacac;
            
       }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微信公众号&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>自动回复</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="center">
        <div id="toolbar" class="datagrid-toolbar" style="padding: 5px; height: auto">
            <div>
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAddOrEdit('add')">
                    添加</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowAddOrEdit('edit')">
                        编辑</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">
                            删除</a>
            </div>
            <div>
                <span style="font-size: 12px; font-weight: normal">关键字：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <select id="type">
                    <option value="">全部</option>
                    <option value="text">文本</option>
                    <option value="news">图文</option>
                </select>
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>
        </div>
        <table id="grvData" cellspacing="0" cellpadding="0" fitcolumns="true">
            <thead>
                <tr>
                    <th field="ck" width="10" checkbox="true">
                    </th>
                    <th field="MsgKeyword" width="20">
                        关键字
                    </th>
                    <th field="ReplyType" width="10" formatter="gettype">
                        类型
                    </th>
                    <th field="MatchType" width="10">
                        匹配类型
                    </th>
                    <th field="RelpayImageList" formatter="operate" width="50">
                        回复内容
                    </th>
                </tr>
            </thead>
        </table>

        <div id="win" class="easyui-dialog" closed="true" modal="true" title="操作" style="padding: 10px;
            text-align:right;width:500px;height:400px;line-height:30px;">
           
            <div>
                <table>
                    <tr id="trType">
                        <td style="width:80px;">
                            类型:
                        </td>
                        <td align="left">
                             <input type="radio" name="rdotype" value="news"  class="positionTop2" id="news" /><label for="news">图文</label>
                            <input type="radio" name="rdotype" value="text" class="positionTop2" id="text"/><label for="text">文本</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:80px;">
                            关键字:
                        </td>
                        <td>
                            <input id="txtKeyWord" type="text" class="easyui-validatebox form-control" required="true" missingmessage="请输入关键字" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr >
                        <td style="width:80px;">
                            匹配类型:
                        </td>
                        <td>
                            <select id="ddlMatchType" class=" form-control" style="height:auto;" >
                                <option value="全文匹配">全文匹配 </option>
                                <option value="开始匹配">开始匹配 </option>
                                <option value="结束匹配">结束匹配 </option>
                                <option value="包含匹配">包含匹配 </option>
                            </select>
                        </td>
                    </tr>
                    <tr id="trNews">
                        <td style="width:80px;">
                           
                        </td>
                        <td>
                            <%--选中的素材在这里显示
                            --%>
                            <div id="div_selectsourcelist">

                            </div>
                            <%--选中的素材在这里显示
                            --%>
                            <input type="button" id="btnSelectImage" onclick="ShowAddSource()" value="选择素材" />
                        </td>
                    </tr>
                      <tr style="display:none;" id="trText">
                        <td>
                            回复内容:
                        </td>
                        <td>
                            <textarea id="txtReplyContent" style="width: 200px; height: 100px" class="easyui-validatebox"
                                required="true" missingmessage="请输入回复内容"></textarea>
                        </td>
                    </tr>
                    
                </table>
            </div>
        </div>

        <div id="win_source" class="easyui-dialog" closed="true" modal="true" title="选择素材" style="padding: 10px;
            text-align: center;width:480px;">
            <div style="text-align: left">
                <span style="font-size: 12px; font-weight: normal;">素材标题:</span>
                <input id="txtSourceName" style="width: 200px" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch_Source">查询</a>
                
            </div>
            <br />
          
            <table id="list_source" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                       <th field="ck" checkbox="true" width="10">
                            标题
                        </th>
                    <th field="PicUrl" formatter="getimage" width="20">
                            图片
                        </th>
                        <th field="Title" width="70">
                            标题
                        </th>
                        <%--<th field="Url"  width="30">图片链接</th> --%>
                        <%--   <th field="Description"  width="30">描述</th>  --%>
                    </tr>
                </thead>
            </table>
            
        </div>


        <div id="win_viewimagelist" class="easyui-window" modal="true" closed="true" style="padding: 10px;
            text-align: center;">
            <div style="text-align: left" id="div_imagelist">
            </div>
            <div>
            </div>
        </div>
      
        <div id="divfancybox" style="display:none;">
         <div style="text-align: left" id="div_imagelist_fancybox">

         </div>
        </div>
    </div>
   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    
<script type="text/javascript" src="http://static-files.socialcrmyun.com/lib/jquery/sortable/jquery-ui-sortable.min.js"></script>
 <script type="text/javascript">
     var grid; //回复规则列表
     var gridsource; //素材列表

     //处理文件路径
     var url = "/Handler/App/CationHandler.ashx";
     var currentAction = "";
     var sourcetype = "source";
     var currentType = 'news';
     //加载文档
     jQuery().ready(function () {
         //-----------------加载gridview
         grid = jQuery("#grvData").datagrid({
             method: "Post",
             url: url,
             height: document.documentElement.clientHeight - 112,
             
             pagination: true,
             rownumbers: true,
             singleSelect: false,
             queryParams: { Action: "QueryNewsReply", SearchTitle: "" },
             onClickRow: function (value, row) {
                 $("#div_imagelist").html(row.RelpayImageList);
                 $("#div_imagelist_fancybox").html(row.RelpayImageList);

             }
         });
         //------------加载gridview

         //-加载未添加到此用户的素材列表------------------------
         gridsource = jQuery("#list_source").datagrid({
             idField: "SourceID",
             view: fileview,
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



         //搜索开始------------------------
         $("#btnSearch").click(function () {
             var SearchTitle = $("#txtName").val();
             grid.datagrid({ url: url, queryParams: { Action: "QueryNewsReply", SearchTitle: SearchTitle, type: $(type).val() } });
         });
         //搜索结束---------------------


         $('#win_source').dialog({
             buttons: [{
                 text: "确定",
                 handler: function () {
                     SelectSource();

                 }
             }, {
                 text: "取消",
                 handler: function () {
                     $('#win_source').dialog('close');
                 }
             }]
         });


         $('#win').dialog({
             buttons: [{
                 text: "确定",
                 handler: function () {

                     Save();
                 }
             }, {
                 text: "取消",
                 handler: function () {
                     $('#win').dialog('close');
                 }
             }]
         });


         $("input[name=rdotype]").click(function () {
             if ($(this).val() == 'text') {
                 $(trNews).hide();
                 $(trText).show();
                 currentType = 'text';
                
             } else {
                 $(trNews).show();
                 $(trText).hide();
                 currentType = 'news';
             }
         })

    



     });  //文档加载结束



     //弹出添加或编辑框开始
     function ShowAddOrEdit(addoredit) {
         var title = ""; //弹出框标题
         //弹出添加框开始
         if (addoredit == "add") {
             //$(trType).show();
             //清除数据
             $("input[value=news]").attr("checked",true);
             Clear("txtKeyWord|ddlMatchType|txtSelectSourceName|txtReplyContent");
             //设置默认值
             $("#div_selectsourcelist").html("");
             $("#btnSelectImage").val("选择素材");

             //设置弹出框标题
             title = "添加";
             $("#txtKeyWord").removeAttr("readonly");
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
             //$(trType).hide();

             $("#txtKeyWord").val(rows[0].MsgKeyword);
             if (rows[0].MsgKeyword == "关注自动回复" || rows[0].MsgKeyword == "消息自动回复") {
                 $("#txtKeyWord").attr("readonly", "readonly");

             }
             else {
                 $("#txtKeyWord").removeAttr("readonly");
             }
             $("#ddlMatchType").val(rows[0].MatchType);
             if (rows[0].ReplyType=='news') {
                 //加载图文
                 $("#btnSelectImage").val("重新选择素材");
                 $("input[value=news]").attr("checked",true);
                 getimagelist(rows[0].UID);
                 $(trNews).show();
                 $(trText).hide();
                
             } else {
                 $("input[value=text]").attr("checked", true);
                 $("#txtReplyContent").val(rows[0].ReplyContent);
                 getimagelist(rows[0].UID);
                 $(trNews).hide();
                 $(trText).show();
                 $("#div_selectsourcelist").html("");
                 
             }
             //加载信息结束
             //设置弹出框标题
             title = "编辑";


         }
         //弹出编辑框结束
         currentAction = addoredit;

         $('#win').dialog({ title: title });
         $("#win").dialog("open");
     }
     //展示添加或编辑框结束


     //添加或编辑操作开始---------
     function Save() {
         var keyword = $("#txtKeyWord").val();
         var matchtype = $("#ddlMatchType").val();
         var context = $("#txtReplyContent").val();
         var sourceids = GetSelectImage();
         var indexs = GetSelectImage1();
         //--检查输入---------------
         if (keyword == "") {
             $("#txtKeyWord").focus();
             return false;
         }

         //----------执行添加操作开始
         if (currentAction == "add") {
             //if (keyword == "关注自动回复") {
             //    Alert("关注自动回复为系统关键字，因此不能添加");
             //    return false;
             //}
             //if (keyword == "消息自动回复") {
             //    Alert("消息自动回复为系统关键字，因此不能添加");
             //    return false;
             //}
             //------------添加
             var model = {};
             if (currentType == 'news') {
                 if (sourceids == "") {
                     Alert("请先选择素材");
                     return false;
                 }
                 model = {
                     Action: "AddNewsReply",
                     MsgKeyword: keyword,
                     MatchType: matchtype,
                     SourceIds: sourceids
                 };

             } else {
                 if (context == "") {
                     $("#txtReplyContent").focus();
                     return false;
                 }
                 model = {
                     Action: "AddTextReply",
                     MsgKeyword: keyword,
                     MatchType: matchtype,
                     ReplyContent: $.trim($("#txtReplyContent").val())
                 };
             }
             jQuery.ajax({
                 type: "Post",
                 url: url,
                 data: model,
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
         else if (currentAction == "edit") {
             //-----------修改
             var rows = grid.datagrid('getSelections');
             var uid = rows[0].UID;
             var model = {
                 Action: "EditNewsReply",
                 UID: uid,
                 MsgKeyword: keyword,
                 MatchType: matchtype,
                 SourceIds:sourceids,
                 SourceType: sourcetype,
                 ReplyContent: $.trim($("#txtReplyContent").val()),
                 type: $("input[name=rdotype]:checked").val(),
                 index:indexs
             };
            
             
             jQuery.ajax({
                 type: "Post",
                 url: url,
                 data: model,
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

     //选择素材弹出框---------------------
     function ShowAddSource() {

         $("#win_source").dialog("open");

     }
     //添加素材弹出框---------------------

     //--------------获取选中素材
     function GetSelectImage() {
         var ids = [];
         $(".imgclass").each(function () {
             var id = $(this).attr("id");
             ids.push(id);
         });
         return ids.join(',');
     }
     function GetSelectImage1() {
         var ids = [];
         $(".imgclass").each(function (k, v) {
             var index = k + 1;
             ids.push(index);
         });
         return ids.join(',');
     }


     //------------获取选中素材

     //删除选中素材
     function delselectimage(id) {

         $("#" + id).parent().parent().remove();

     }
     //删除选中素材

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
             if (rows[i].MsgKeyword=="关注自动回复"||rows[i].MsgKeyword=="消息自动回复") {
                 messager("系统提示", "关注自动回复 和 消息自动回复 为系统关键字,因此不能删除");
                 return;

             }

         }

         $.messager.confirm("系统提示", "是否确定删除选中规则?", function (r) {
             if (r) {
                 jQuery.ajax({
                     type: "Post",
                     url: url,
                     data: { Action: "DeleteNewsReply", id: ids.join(',') },
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

     //图片--------------
     function getimage(value, row, index) {

         var str = "";
         if (value != "") {

             str = "<img src=" + value + " width=50 height=50 >";
         }
         return str;

     }
     //       //图片--------------


     function operate(value, row, index) {
         if (value != '') {
             return "<a href='#divfancybox' class='fancybox' title='点击查看图文' onclick='edit_image(this)'>点击查看图文</a>";
         } else {
             return row.ReplyContent;
         }

     }
     function gettype(value, row, index) {
         if (value == 'text') {
          return '<span style="color:#3f9f00;">文本</span>';

         } else {
             return '<span style="color:#3f9f00;">图文</span>';
         }
     }
     //查看图文信息
     function edit_image() {
         $("#win_viewimagelist").window({
             title: "图文列表",
             closed: false,
             collapsible: false,
             minimizable: false,
             maximizable: false,
             iconCls: "icon-edit",
             resizable: false,
             width: 300,
             height: 400,
             top: ($(window).height() - 400) * 0.5,
             left: ($(window).width() - 400) * 0.5


         });


     }
     //查看图文信息

     // 选择素材---------------------
     function SelectSource() {
         var rows = gridsource.datagrid('getSelections');
         var num = rows.length;
         if (num == 0) {
             Alert("请选择素材");
             return;
         }
         if (num > 10) {
             Alert("系统提示", "您选中了" + num + "个素材，最多只能同时选择10个素材");
             return;
         }
         var ids = [];
         var strselectsourcename = "";
         var selectsourcelist = "<table id='tb_source'>"; //选中的素材
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].SourceID);
             selectsourcelist += "<tr><td>" + "<img class=imgclass id=" + rows[i].SourceID + " alt=" + rows[i].Title + " src=" + rows[i].PicUrl + " width=50 height=50 >" + "</td><td><span style='font-weight:bold;'>" + rows[i].Title + "</span></br><a href='javascript:void(0)' title='删除' onclick='delselectimage(" + rows[i].SourceID + ")'>删除</a>" + "</td></tr>";
         }
         selectsourcelist += "</table>";
         sourcetype = "source";
         $("#div_selectsourcelist").html(selectsourcelist);
         sortImgList();
         $("#btnSelectImage").val("重新选择素材");
         $("#win_source").dialog("close"); //关闭选择素材窗口

     };
     // 选择素材---------------------


     //获取规则图文列表
     function getimagelist(uid) {
         $.post(url, { Action: "GetSourceImageList", UID: uid }, function (data) {
             var strhtml = "<table id=tb_source>";
             var objData = eval(data);
            
             $.each(objData, function (index, item) {
                 strhtml += "<tr><td style='width:50px;'>" + "<img class=imgclass id=" + item.UID + " alt=" + item.Title + " src=" + item.PicUrl + " width=50 height=50 >" + "</td><td style='width:200px;text-align:left;'>" + item.Title + "<br/>" + "</td>&nbsp;<td><a href='javascript:void(0)' title='删除' onclick='delselectimage(" + item.UID + ")'>删除</a></td></tr>";
             });
             strhtml += "</table>";
             $("#div_selectsourcelist").html(strhtml);
             sortImgList();
             sourcetype = "imagelist";
             if (data == "") {

             }
         });

     }
     //获取规则图文列表

     function sortImgList() {

         $("#div_selectsourcelist table tbody").sortable({
             axis: 'y',
             delay: 300,
             forcePlaceholderSize: true,
             opacity: 0.6,
             scroll: false,//关闭滚动事件
             placeholder: 'ui-state-highlight',
             update: function (event, ui) {

             }
         });
     }
    </script>
</asp:Content>
