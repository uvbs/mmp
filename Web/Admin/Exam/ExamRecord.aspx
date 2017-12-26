<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ExamRecord.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Exam.ExamRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
<%ZentCloud.BLLJIMP.BLL bllBase = new ZentCloud.BLLJIMP.BLL();
  ZentCloud.BLLJIMP.Model.Questionnaire model = bllBase.Get<ZentCloud.BLLJIMP.Model.Questionnaire>(string.Format("QuestionnaireID={0}",Request["id"]));
    %>
     当前位置：&nbsp;&nbsp;<%=model.QuestionnaireName%>&nbsp;&gt;&nbsp;&nbsp;<span>试卷提交记录 </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除记录</a>
        <a title="返回" style="float:right;margin-right:10px;" href="ExamMgr.aspx" class="easyui-linkbutton" iconcls="icon-back" plain="true" >返回</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">

    var handlerUrl = "/Handler/App/CationHandler.ashx";
    
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryQuestionnaireRecord",QuestionnaireID:<%=Request["id"]%> },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: false,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'WXHeadimgurl', title: '微信头像', width: 15, align: 'left', formatter:function(value){
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                
                                } },
                                { field: 'WXNickname', title: '微信昵称', width: 15, align: 'left', formatter: FormatterTitle },
                                { field: 'InsertDate', title: '提交时间', width: 15, align: 'left', formatter: FormatDate },
                                //{ field: 'IP', title: 'IP', width: 20, align: 'left' },
                                { field: 'op', title: '操作', width: 15, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="查看详细" target="_blank" href="ExamRecordDetail.aspx?id={0}&uid={1}">查看试卷</a>', rowData.QuestionnaireID,rowData.UserId);
                                    return str.ToString();
                                } 
                                }


                             ]]
	            }
            );










     });


     function Delete() {
         try {

             var rows = $('#grvData').datagrid('getSelections');

             if (!EGCheckIsSelect(rows))
                 return;

             $.messager.confirm("系统提示", "确认删除选中提交记录?", function (r) {
                 if (r) {
                     var ids = [];

                     for (var i = 0; i < rows.length; i++) {
                         ids.push(rows[i].AutoId);
                     }

                     var dataModel = {
                         Action: 'DeleteQuestionnaireRecord',
                         ids: ids.join(',')
                     }

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         success: function (result) {
                             Alert(result);
                             $('#grvData').datagrid('reload');
                         }
                     });
                 }
             });

         } catch (e) {
             Alert(e);
         }
     }


    </script>
</asp:Content>
