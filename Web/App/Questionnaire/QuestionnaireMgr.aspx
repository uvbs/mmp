<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/WebMainContent.Master" CodeBehind="QuestionnaireMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Questionnaire.QuestionnaireMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置:<strong><%=typeName %>管理</strong>
    <%if(type=="0") {%>
     <a title="返回答题" style="float: right; margin-right: 20px;" href="/App/Questionnaire/QuestionnaireSetMgr.aspx" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回答题</a>
    <%}%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="ShowAdd();" id="btnAdd">发布新<%=typeName %></a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除<%=typeName %></a>
            <%
            if(typeName=="问卷"){
                %>
                    <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowForward();">加入到微转发</a>
                <%
            }     
            %>
             <br />
                <span style="font-size: 12px; font-weight: normal"><%=typeName %>名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Authority%>';
     var type = "<%=type %>";
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryQuestionnaire", QuestionnaireType: type },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                rownumbers: true,
	                singleSelect: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                <% if(type =="1"){%>
                                { field: 'QuestionnaireImage', title: '图片', width: 20, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img  height="50" width="50" src="{0}" />', value);
                                    return str.ToString();
                                }
                                },
                                <% }%>
                                { field: 'QuestionnaireName', title: '<%=typeName %>名称', width: 30, align: 'left' },

                                 <% if(type =="1"){%>
                                  {
                                      field: 'ippv', title: 'PV/IP', width: 10, align: 'left', formatter: function (value, rowData) {
                                          var str = new StringBuilder();
                                          if (rowData.PV == 0) {
                                              str.AppendFormat('{0}', 0);
                                          } else {
                                              str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&share=1" title="点击查看统计详情">{1}/{2}</a>', rowData.QuestionnaireID, rowData.PV, rowData.IP);
                                          }
                                          return str.ToString();
                                      }
                                  },
                                   {
                                       field: 'UV', title: '微信阅读人数', width: 10, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           if (value == 0) {
                                               str.AppendFormat('{0}', 0);
                                           } else {
                                               str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1&share=1" title="点击查看统计详情">{1}</a>', rowData.QuestionnaireID, value);
                                           }
                                           return str.ToString();
                                       }
                                   },
                                   {
                                       field: 'SubmitCount', title: '提交份数', width: 10, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           if (value == 0) {
                                               str.AppendFormat('{0}', 0);
                                           } else {
                                               str.AppendFormat('<a class="listClickNum" href="/App/Questionnaire/QuestionnaireRecord.aspx?id={0}&type={1}" title="点击查看统计详情">{2}</a>', rowData.QuestionnaireID, type,value);
                                           }
                                           return str.ToString();
                                       }
                                   },
                                 <%}%>



                                { field: 'InsertDate', title: '创建时间', width: 15, align: 'left', formatter: FormatDate }
                                <% if(type =="1"){%>
                                ,
                                { field: 'op', title: '操作', width: 20, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('&nbsp;<a title="点击查看二维码" href="javascript:void(0)" onclick="ShowQRcode(\'{0}\')">二维码</a>&nbsp;<a title="点击查看统计" href="QuestionnaireStatistics.aspx?id={0}&type=' + type + '"><%=typeName %>统计</a>&nbsp;<a title="导出记录" target="_blank"  href="/Serv/API/Admin/Question/ExportQuestionRecords.ashx?questionnaire_id={0}">导出记录</a>', rowData.QuestionnaireID);
                                    return str.ToString();
                                } 
                                }
                                <% }%>
                             ]]
	            }
            );






         $("#btnSearch").click(function () {
             var QuestionnaireName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryQuestionnaire", QuestionnaireName: QuestionnaireName, QuestionnaireType: type } });
         });



     });

     function ShowAdd() {

         window.location.href = "QuestionnaireCompile.aspx?Action=add&type=" + type;


     }
     function Delete() {
         try {

             var rows = $('#grvData').datagrid('getSelections');

             if (!EGCheckIsSelect(rows))
                 return;

             $.messager.confirm("系统提示", "确认删除选中<%=typeName %>?<br/>选中<%=typeName %>相关数据将会全部删除!", function (r) {
                 if (r) {
                     var ids = [];

                     for (var i = 0; i < rows.length; i++) {
                         ids.push(rows[i].QuestionnaireID);
                     }

                     var dataModel = {
                         Action: 'DeleteQuestionnaire',
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

     function ShowQRcode(id) {
         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Questionnaire/Wap/Questionnaire.aspx?id=' + id);
         var linkurl = "http://" + domain + "/App/Questionnaire/Wap/Questionnaire.aspx?id=" + id;
         $("#alinkurl").html(linkurl);
         $("#alinkurl").attr("href", linkurl);
         $('#dlgSHowQRCode').dialog('open');
     }

     function ShowEdit() {
         var rows = $('#grvData').datagrid('getSelections');

         if (!EGCheckIsSelect(rows))
             return;

         if (!EGCheckNoSelectMultiRow(rows))
             return;

         window.location.href = "QuestionnaireEdit.aspx?id=" + rows[0].QuestionnaireID + "&type="+type;

     }

     //加入到微转发
     function ShowForward() {
         var rows = $('#grvData').datagrid('getSelections');
         if (!EGCheckIsSelect(rows)) return;
         $.messager.confirm('友情提示', '确定加入到微转发?', function (o) {
             if (o) {
                 $.ajax({
                     type: "post",
                     url: '/serv/api/admin/forward/add.ashx',
                     data: { ids: GetRowsIds(rows).join(','), forward_type: "questionnaire" },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.status) {
                             Alert(resp.msg)
                         } else {
                             Alert(resp.msg)
                         }
                     }
                 });
             }
         });
     }

     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].QuestionnaireID
                 );
         }
         return ids;
     }

    </script>
</asp:Content>
