<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ReplyTextRuleManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.ReplyTextRuleManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;微客服&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>文本自动回复 </span>
</asp:Content> 
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加</a>
                <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
           <br />
                关键字：
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" onclick="Search()">查询</a>
           


        </div>
    </div>
    <table id="grvData" fitcolumns="true">

    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加新分类" style="width: 320px;
         padding: 15px;line-height:30px;">
        <table>
                    <tr>
                        <td>
                            关键字:
                        </td>
                        <td>
                            <input id="txtKeyWord" type="text" style="width:200px;" class="easyui-validatebox" required="true" missingmessage="请输入关键字" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            匹配类型:
                        </td>
                        <td>
                            <select id="ddlMatchType" style="width: 200px">
                                <option value="全文匹配">全文匹配 </option>
                                <option value="开始匹配">开始匹配 </option>
                                <option value="结束匹配">结束匹配 </option>
                                <option value="包含匹配">包含匹配 </option>
                            </select>
                        </td>
                    </tr>
                    <tr>
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
 
   
   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currSelectID = 0;
     var currAction = '';

     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryTextReply" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                pageSize: 50,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'MsgKeyword', title: '关键字', width: 20, align: 'left' },
                                { field: 'MatchType', title: '匹配类型', width: 10, align: 'left' },
                                { field: 'ReplyContent', title: '回复内容', width: 65, align: 'left' }


                             ]]
	            }
            );



         $('#dlgInput').dialog({
             buttons: [{
                 text: '保存',
                 handler: function () {
                     try {
                         var dataModel = {
                             Action: currAction,
                             UID: currSelectID,
                             MsgKeyword: $.trim($("#txtKeyWord").val()),
                             MatchType: $("#ddlMatchType").val(),
                             ReplyContent: $.trim($("#txtReplyContent").val())
                         }

                         if (dataModel.MsgKeyword == '') {
                             $("#txtKeyWord").focus();
                             return;
                         }
                         if (dataModel.ReplyContent == '') {
                             $("#txtReplyContent").focus();
                             return;
                         }

                         $.ajax({
                             type: 'post',
                             url: handlerUrl,
                             data: dataModel,
                             success: function (result) {
                                 if (result == "true") {
                                     messager("系统提示", "保存成功");
                                     $("#dlgInput").dialog("close");
                                     $("#grvData").datagrid('reload');
                                 }
                                 else {
                                     Alert(result);
                                 }


                             }
                         });

                     } catch (e) {
                         Alert(e);
                     }
                 }
             }, {
                 text: '取消',
                 handler: function () {

                     $('#dlgInput').dialog('close');
                 }
             }]
         });






     });

     function Search() {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryTextReply", SearchTitle: $("#txtName").val() }
	            });


     }

     function ShowAdd() {
         currAction = 'AddTextReply';
         $("#dlgInput input").val("");
         $("#ddlMatchType").val("全文匹配");
         $("#txtReplyContent").val("");
         $('#dlgInput').dialog({ title: '添加' });
         $('#dlgInput').dialog('open');




     }


     function ShowEdit() {
         var rows = $('#grvData').datagrid('getSelections');

         if (!EGCheckIsSelect(rows))
             return;

         if (!EGCheckNoSelectMultiRow(rows))
             return;


         currAction = 'EditTextReply';
         currSelectID = rows[0].UID;
         $("#txtKeyWord").val(rows[0].MsgKeyword);
         $("#ddlMatchType").val(rows[0].MatchType);
         var content = replacebrtag(rows[0].ReplyContent);
         $("#txtReplyContent").val(content);
         $('#dlgInput').dialog({ title: '编辑' });
         $('#dlgInput').dialog('open');

     }

     function Delete() {
         var rows = $("#grvData").datagrid('getSelections');
         if (!EGCheckIsSelect(rows))
             return;

         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].UID);
         }

         $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
             if (r) {
                 jQuery.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DeleteTextReply", id: ids.join(',') },
                     success: function (result) {
                         if (result) {
                             messager('系统提示', "删除成功！");
                             $("#grvData").datagrid('reload');

                             return;
                         }
                         $.messager.alert("删除失败");
                     }
                 });
             }
         });
     };


    </script>
</asp:Content>


