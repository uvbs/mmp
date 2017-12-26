<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/WebMainContent.Master" CodeBehind="ExamMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Exam.ExamMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置:<strong>试卷管理</strong>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="ExamAdd.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true"  id="btnAdd">添加新试卷</a>
          <%--  <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();">编辑</a>--%>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>

            <br />
            <span style="font-size: 12px; font-weight: normal">试卷名称：</span>
            <input type="text" style="width: 200px" id="txtName" />
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="请用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var domain = "<%=Request.Url.Host%>";
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl,
                       queryParams: { Action: "QueryQuestionnaire", QuestionnaireType: "2" },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       rownumbers: true,
                       singleSelect: true,
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },

                                   //{
                                   //    field: 'QuestionnaireImage', title: '图片', width: 20, align: 'center', formatter: function (value) {
                                   //        if (value == '' || value == null)
                                   //            return "";
                                   //        var str = new StringBuilder();
                                   //        str.AppendFormat('<img  height="50" width="50" src="{0}" />', value);
                                   //        return str.ToString();
                                   //    }
                                   //},

                                   { field: 'QuestionnaireName', title: '试卷名称', width: 50, align: 'left' },


                                     //{
                                     //    field: 'ippv', title: 'PV/IP', width: 10, align: 'left', formatter: function (value, rowData) {
                                     //        var str = new StringBuilder();
                                     //        if (rowData.PV == 0) {
                                     //            str.AppendFormat('{0}', 0);
                                     //        } else {
                                     //            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&share=1" title="点击查看统计详情">{1}/{2}</a>', rowData.QuestionnaireID, rowData.PV, rowData.IP);
                                     //        }
                                     //        return str.ToString();
                                     //    }
                                     //},
                                      //{
                                      //    field: 'UV', title: '微信阅读人数', width: 10, align: 'left', formatter: function (value, rowData) {
                                      //        var str = new StringBuilder();
                                      //        if (value == 0) {
                                      //            str.AppendFormat('{0}', 0);
                                      //        } else {
                                      //            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1&share=1" title="点击查看统计详情">{1}</a>', rowData.QuestionnaireID, value);
                                      //        }
                                      //        return str.ToString();
                                      //    }
                                      //},
                                      {
                                          field: 'SubmitCount', title: '试卷记录', width: 10, align: 'left', formatter: function (value, rowData) {
                                              var str = new StringBuilder();
                        
                                           str.AppendFormat('<a class="listClickNum" href="ExamRecord.aspx?id={0}" title="点击查看试卷记录">{1}</a>', rowData.QuestionnaireID,value);
                                              
                                              return str.ToString();
                                          }
                                      },




                                   { field: 'InsertDate', title: '创建时间', width: 15, align: 'left', formatter: FormatDate }

                                   ,
                                   {
                                       field: 'op', title: '操作', width: 15, align: 'left', formatter: function (value, rowData) {
                                           var str = new StringBuilder();
                                           str.AppendFormat('&nbsp;<a title="点击查看二维码" href="javascript:void(0)" onclick="ShowQRcode(\'{0}\')">点击查看考试链接</a>&nbsp;', rowData.QuestionnaireID);
                                    return str.ToString();
                                }
                                }

	                ]]
	            }
            );






         $("#btnSearch").click(function () {
             var QuestionnaireName = $("#txtName").val();
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryQuestionnaire", QuestionnaireName: QuestionnaireName, QuestionnaireType: "2" } });
         });



     });


                    function Delete() {
                        try {

                            var rows = $('#grvData').datagrid('getSelections');

                            if (!EGCheckIsSelect(rows))
                                return;

                            $.messager.confirm("系统提示", "确认删除选中试卷?<br/>选中试卷相关数据将会全部删除!", function (r) {
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
                        $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Exam/Exam.aspx?id=' + id);
                        var linkurl = "http://" + domain + "/App/Exam/Exam.aspx?id=" + id;
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

                        window.location.href = "ExamEdit.aspx?id=" + rows[0].QuestionnaireID;

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
