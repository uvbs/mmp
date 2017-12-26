<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ReviewInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Review.ReviewInfoMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理 &nbsp;&gt;&nbsp;&nbsp;<span>评论信息</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a> <a href="javascript:;" class="easyui-linkbutton" iconcls=" icon-add"
                    plain="true" onclick="ConfigReviewInfo()">配置评论是否显示</a><br />
            评论类型:
            <select id="ddlVote">
                <option value="">全部</option>
                <option value="投票">投票</option>
                <option value="活动">活动</option>
                <option value="文章">文章</option>
            </select>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">
                查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgPmsInfo" class="easyui-dialog" title="Basic Dialog" closed="true" style="width: 320px;
        height: 135px; padding: 10px">
        <table>
            <tr>
                <td width="*" align="left">
                    <input type="checkbox" name="IsVoteOpen" id="ckIsVote" /><label for="ckIsVote">投票评论</label>
                </td>
                <td width="*" align="left">
                    <input type="checkbox" name="IsArticle" id="ckIsArticle" /><label for="ckIsArticle">文章评论</label>
                </td>
                <td width="*" align="left">
                    <input type="checkbox" name="IsActivity" id="ckIsActivity" /><label for="ckIsActivity">活动评论</label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td align="right">
                    <a href="javascript:void(0)" id="btnSave" class="easyui-linkbutton">保 存</a> <a href="javascript:void(0)"
                        id="btnExit" class="easyui-linkbutton">关 闭</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXReviewInfoHandler.ashx";
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetReviewInfos" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                //	                treeField: "UserName",
	                
	                pageSize: 50,
	                rownumbers: true,
	                animate: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'UserName', title: '评论人', width: 10, align: 'left' },
                                { field: 'ReviewContent', title: '评论内容', width: 10, align: 'left' },
                                { field: 'ForeignkeyName', title: '名称', width: 5, align: 'left' },
                                { field: 'ReviewType', title: '类型', width: 5, align: 'left' },
                                { field: 'NumCount', title: '回复数量', width: 5, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="点击查看回复人" href="ReplyReviewInfoMgr.aspx?id={0}" >{1}</a>', rowData.AutoId, value);
                                    return str.ToString();
                                }
                                },
                                { field: 'PraiseNum', title: '顶', width: 5, align: 'left' },
                                { field: 'StepNum', title: '踩', width: 5, align: 'left' },
                                { field: 'InsertDate', title: '评论时间', width: 10, align: 'left', formatter: FormatDate }
                             ]]
	            }
            );

         $("#btnSearch").click(function () {
             var VoteID = $("#ddlVote").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "GetReviewInfos", ReviewType: VoteID} });
         });

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
                     data: { Action: "DeleteReviewInfos", ids: GetRowsIds(rows).join(',') },
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
