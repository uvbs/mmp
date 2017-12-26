<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ReplyReviewInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Review.ReplyReviewInfoMgr" %>
     
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理 &nbsp;&gt;&nbsp;&nbsp;<span>评论信息-</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
               <a href="ReviewInfoMgr.aspx" class="easyui-linkbutton" iconcls="icon-redo" plain="true">
                返回</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXReviewInfoHandler.ashx";
     var ReviewID = '<%=ReviewId %>'
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetReplyReviewInfos", ReviewID: ReviewID },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                //	                treeField: "UserName",  
	                pageSize: 10,
	                rownumbers: true,
	                animate: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'UserName', title: '回复人', width: 10, align: 'left' },
                                { field: 'ReplyContent', title: '回复内容', width: 10, align: 'left' },
                                { field: 'InsertDate', title: '评论时间', width: 10, align: 'left', formatter: FormatDate }
                             ]]
	            }
            );

         //            $("#btnSearch").click(function () {
         //                var VoteID = $("#ddlVote").val();
         //                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "GetReviewInfos", ReviewType: VoteID} });
         //            });

     })
     //删除
     function Delete() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确定删除选中?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DeleteReplyReviewInfos", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 0) {
                             $('#grvData').datagrid('reload');
                             Show(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });
     }
     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].AutoId);
         }
         return ids;
     }

     //配置是否显示评论
     function ConfigReviewInfo() {
         $('#dlgPmsInfo').window(
            {
                title: '配置是否显示评论'
            }
            );

         $.post(handlerUrl, { Action: "GetReviewConFig" }, function (data) {
             var resp2 = $.parseJSON(data);
             if (resp2.Status == 0) {
                 $('#dlgPmsInfo').dialog('open');

                 if (resp2.ExObj.VoteId == 1)
                     ckIsVote.checked = true;
                 else
                     ckIsVote.checked = false;

                 if (resp2.ExObj.Article == 1)
                     ckIsArticle.checked = true;
                 else
                     ckIsArticle.checked = false;

                 if (resp2.ExObj.Activity == 1)
                     ckIsActivity.checked = true;
                 else
                     ckIsActivity.checked = false;
             }
         });
     }
     //窗体关闭按钮---------------------
     $("#btnExit").live("click", function () {
         $("#dlgPmsInfo").window("close");
     });


     $("#btnSave").live("click", function () {
         var Vote = ckIsVote.checked ? 1 : 0;
         var Article = ckIsArticle.checked ? 1 : 0;
         var Activity = ckIsActivity.checked ? 1 : 0;
         $.post(handlerUrl, { Action: "SaveReviewConFig", Vote: Vote, Article: Article, Activity: Activity }, function (data) {
             var resp3 = $.parseJSON(data);
             if (resp3.Status = 0) {
                 Show(resp3.Msg);
             } else {
                 Alert(resp3.Msg);
             }
         });
     });
    </script>
</asp:Content>
