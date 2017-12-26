<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ExamEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Exam.ExamEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body {
            font-family: 微软雅黑;
        }

        .tdTitle {
            font-weight: bold;
        }

        .title {
            font-size: 12px;
        }

        input[type=text], select {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }

        .question {
            border: 1px solid;
            border-radius: 5px;
            border-color: #CCCCCC;
            margin-top: 10px;
            width: 600px;
            position: relative;
        }

        .fieldsort {
            float: left;
            margin-left: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .deletequestion {
            right: 5px;
            margin-top: -5px;
            cursor: pointer;
            float: right;
        }

        .question input[type=text] {
            width: 90%;
        }

        .deleteanswer {
            margin-left: 8px;
            margin-top: 5px;
            position: absolute;
            cursor: pointer;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;<%=typeName %>&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>编辑<%=typeName %></span> <a title="返回<%=typeName %>管理" style="float: right; margin-right: 20px;" href="/App/Questionnaire/QuestionnaireMgr.aspx?type=<%=type %>" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%;">

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        <%=typeName %>名称：
                    </td>

                    <td width="*" align="left">
                        <input type="text" id="txtQuestionnaireName" value="<%=model.QuestionnaireName %>" style="width: 100%;" placeholder="<%=typeName %>名称(必填)" />
                    </td>
                </tr>
                <%if (type == "1")
                  { %>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" width="80px" height="80px" id="imgThumbnailsPath" src="<%=model.QuestionnaireImage %>" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG,PNG 格式图片，图片最佳显示效果大小为80*80。
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtQuestionnaireSummary" value="<%=model.QuestionnaireSummary %>" style="width: 100%;" placeholder="微信分享描述(选填)" />

                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        <%=typeName %>介绍及说明：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtContent" style="width: 100%; height: 200px;"><%=model.QuestionnaireContent%></textarea>
                    </td>
                </tr>
                <%if (type == "1")
                  { %>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        <%=typeName %>结束时间：
                    </td>
                    <td align="left" class="time">
                        <input class="easyui-datetimebox" value="<%=model.QuestionnaireStopDate==null?"": ((DateTime)model.QuestionnaireStopDate).ToString("yyyy-MM-dd HH:mm:ss")%>" id="txtStopDate" />
                    </td>
                </tr>
              
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">赠送积分：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAddScore" value="<%=model.AddScore%>"   style="width:100%;"/>
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">答卷后按钮文字：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtButtonText" value="<%=model.ButtonText %>" style="width:100%;" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">答卷后按钮链接：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtButtonLink" value="<%=model.ButtonLink %>" style="width:100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">每页题目数(0不分页)：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtEachPageNum" value="<%=model.EachPageNum%>" style="width:100%;" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">是否需要微信高级授权：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" id="noneed" value="0" checked="checked" name="weixin" /><label for="noneed">否</label>
                        <input type="radio" id="need" value="1" name="weixin" /><label for="need">是</label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td>显示/隐藏</td>
                    <td>
                        <input type="radio" name="rdovisible" id="rdovisible1" value="1" /><label for="rdovisible1">显示</label><input type="radio" name="rdovisible" id="rdovisible0" value="0" /><label for="rdovisible0">隐藏</label></td>
                </tr>
            </table>
            <div>
                <%if (haseRecord)
                  { %>
                <strong style="font-size: 20px; color: red;">题库已有答题记录，禁止修改题目！</strong>
                <% }
                  else
                  {%>
                <strong style="font-size: 20px;">问题列表:</strong>
                <% for (int i = 0; i < model.Questions.Count; i++)
                   {%>
                <div class="question" data-question-index="<%=i %>" data-question-id="<%= model.Questions[i].QuestionID %>">
                    <img src="/img/icons/up.png" class="upfield fieldsort" />
                    <img src="/img/icons/down.png" class="downfield fieldsort" />
                    <img src="/img/delete.png" class="deletequestion" />
                    <table style="width: 100%; margin-left: 10px;">
                        <tr class="trquestionname" <% =model.Questions[i].QuestionType !=6?"":"style=\"display:none;\""  %>>
                            <td style="width: 100px;">问题:</td>
                            <td>
                                <input type="text" name="questionname" placeholder="问题(必填)" value="<%= model.Questions[i].QuestionType !=6?model.Questions[i].QuestionName:"" %>" /></td>
                        </tr>
                        <tr>
                            <td style="width: 100px;">问题类型:</td>
                            <td>
                                <input type="radio" class="positionTop2" name="rdtype<%=i %>" value="0" <% =model.Questions[i].QuestionType ==0?"checked=\"checked\"":""  %> id="rd<%=i %>0" /><label for="rd<%=i %>0">单选</label>
                                <%if (type == "1")
                                  { %>
                                <input name="rdtype<%=i %>" class="positionTop2" type="radio" value="1" <% =model.Questions[i].QuestionType ==1?"checked=\"checked\"":""  %> id="rd<%=i %>1" /><label for="rd<%=i %>1">多选</label>
                                <input type="radio" class="positionTop2" name="rdtype<%=i %>" value="2" <% =model.Questions[i].QuestionType ==2?"checked=\"checked\"":""  %> id="rd<%=i %>2" /><label for="rd<%=i %>2">填空</label>
                                <input type="radio" class="positionTop2" name="rdtype<%=i %>" value="3" <% =model.Questions[i].QuestionType ==3?"checked=\"checked\"":""  %> id="rd<%=i %>3" /><label for="rd<%=i %>3">多组</label>
                                <input type="radio" class="positionTop2" name="rdtype<%=i %>" value="4" <% =model.Questions[i].QuestionType ==4?"checked=\"checked\"":""  %> id="rd<%=i %>4" /><label for="rd<%=i %>4">省市区</label>
                                <input type="radio" class="positionTop2" name="rdtype<%=i %>" value="5" <% =model.Questions[i].QuestionType ==5?"checked=\"checked\"":""  %> id="rd<%=i %>5" /><label for="rd<%=i %>5">年月日</label>
                                <input type="radio" class="positionTop2" name="rdtype<%=i %>" value="6" <% =model.Questions[i].QuestionType ==6?"checked=\"checked\"":""  %> id="rd<%=i %>6" /><label for="rd<%=i %>6">文本描述</label>
                                <%} %>
                            </td>
                        </tr>
                        <%if (type == "1")
                          { %>
                        <tr class="trgroupname" <% =model.Questions[i].QuestionType ==3?"":"style=\"display:none;\""  %>>
                            <td>组名:</td>
                            <td>
                                <input type="text" name="answergroupname" placeholder="组名(必填)(,号分隔)" value="<%=model.Questions[i].AnswerGroupName %>" />
                            </td>
                        </tr>
                        <tr class="tryearmonth" <% =model.Questions[i].QuestionType ==5?"":"style=\"display:none;\""  %>>
                            <td>选择类型:</td>
                            <td>
                                <select class="ddlyearmonth">
                                    <option value="month" <% =model.Questions[i].AnswerGroupName =="month"?"selected=\"selected\"":""  %>>年月</option>
                                    <option value="date" <% =model.Questions[i].AnswerGroupName =="date"?"selected=\"selected\"":""  %>>年月日</option>
                                    <option value="time" <% =model.Questions[i].AnswerGroupName =="time"?"selected=\"selected\"":""  %>>时间</option>
                                    <option value="datetime" <% =model.Questions[i].AnswerGroupName =="datetime"?"selected=\"selected\"":""  %>>日期时间</option>
                                </select>
                            </td>
                        </tr>
                        <tr class="trKindeditor" <% =model.Questions[i].QuestionType ==6?"":"style=\"display:none;\""  %>>
                            <td>描述:</td>
                            <td>
                                <div id="txtEditor<%=i %>" style="height: 300px;">
                                    <%=model.Questions[i].QuestionName %>
                                </div>
                            </td>
                        </tr>
                        <tr class="trrequired" <% =model.Questions[i].QuestionType !=6?"":"style=\"display:none;\""  %>>
                            <td>选填/必填:</td>
                            <td>
                                <select class="ddlrequired">
                                    <option value="1" <% =model.Questions[i].IsRequired ==1?"selected=\"selected\"":""  %>>必填</option>
                                    <option value="0" <% =model.Questions[i].IsRequired ==0?"selected=\"selected\"":""  %>>选填</option>
                                </select>
                            </td>
                        </tr>
                        <% }%>
                        <%for (int j = 0; j < model.Questions[i].Answers.Count; j++)
                          {%>

                        <tr data-answer-index="<%=j %>" data-answer-id="<%= model.Questions[i].Answers[j].AnswerID %>">
                            <td>选项:
                            <%if (type == "0")
                              { %>
                                <input id="rdCorrect<%=i %><%=j %>" class="positionTop2" <% =model.Questions[i].Answers[j].IsCorrect ==1?"checked=\"checked\"":""  %> style="margin-left: 10px; <% =model.Questions[i].QuestionType !=0?"display: none;": ""  %>" type="radio" name="rdCorrect<%=i %>" /><label id="lblrd<%=i %><%=j %>" style="<% =model.Questions[i].QuestionType !=0?"display: none;": ""  %>" for="rdCorrect<%=i %><%=j %>">正确</label>
                                <input id="ckCorrect<%=i %><%=j %>" class="positionTop2" <% =model.Questions[i].Answers[j].IsCorrect ==1?"checked=\"checked\"":""  %> style="margin-left: 10px; <% =model.Questions[i].QuestionType !=1?"display: none;": ""  %>" type="checkbox" name="ckCorrect<%=i %>" /><label id="lblck<%=i %><%=j %>" style="<% =model.Questions[i].QuestionType !=1?"display: none;": ""  %>" for="ckCorrect<%=i %><%=j %>">正确</label>
                                <%} %>
                            </td>
                            <td>
                                <input type="text" name="answer" placeholder="选项名称" value="<%=model.Questions[i].Answers[j].AnswerName %>" /><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer" /></td>
                        </tr>
                        <% }%>
                        <tr>
                            <td></td>
                            <td><a class="button button-rounded button-primary" data-action="addanswer">添加选项</a></td>
                        </tr>
                    </table>
                </div>
                <% } %>
                <a class="button button-rounded button-primary" style="width: 90%; margin-top: 10px; margin-bottom: 10px;" id="btnAddQuestion">添加问题</a>
            </div>
            <% } %>
            <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top: 10px;">
                <a href="javascript:;" id="btnSave" class="button button-rounded button-primary" style="width: 200px;">保存</a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/json2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var editor;
        var type = "<%=type %>";
        var questioncount = <%= model.Questions.Count %>; //问题数量
        var editors = {};
        $(function () {
            
            $('.question input[type="radio"][value="6"]:checked').each(function(){
                var indexNum = $(this).closest("div").attr('data-question-index');
                if (!editors['editor' + indexNum]) {
                    KindEditor.ready(function (K) {
                        editors['editor' + indexNum] = K.create('#txtEditor' + indexNum, {
                            uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                            items: [
                                'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                                'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                                'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table'],
                            filterMode: false,
                            width:'93%'
                        });
                    });
                }
            });
            //上移
            $(".upfield").live("click", function () {
                if ($(this).closest("div").prev(".question").length > 0) {
                    var QuestionType = parseInt($(this).closest("div").find("input[type='radio']:checked").val());
                    if(QuestionType!=6){
                        $(this).closest("div").prev(".question").before($(this).closest("div").clone());
                        $(this).closest("div").remove();
                    }
                    else{
                        var indexNum = $(this).closest("div").attr('data-question-index');
                        var nhtml = editors['editor' + indexNum].html();
                        KindEditor.remove('#txtEditor' + indexNum);
                        editors['editor' + indexNum] = null;
                        $(this).closest("div").prev(".question").before($(this).closest("div").clone());
                        $(this).closest("div").remove();
                        KindEditor.ready(function (K) {
                            $('#txtEditor' + indexNum).html(nhtml);
                            editors['editor' + indexNum] = K.create('#txtEditor' + indexNum, {
                                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                                items: [
                                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table'],
                                filterMode: false,
                                width:'93%'
                            });
                        });
                    }
                }
            });
            //下移
            $(".downfield").live("click", function () {
                if ($(this).closest("div").next(".question").length > 0) {
                    var QuestionType = parseInt($(this).closest("div").find("input[type='radio']:checked").val());
                    if(QuestionType!=6){
                        $(this).closest("div").next(".question").after($(this).closest("div").clone());
                        $(this).closest("div").remove();
                    }
                    else{
                        var indexNum = $(this).closest("div").attr('data-question-index');
                        var nhtml = editors['editor' + indexNum].html();
                        KindEditor.remove('#txtEditor' + indexNum);
                        editors['editor' + indexNum] = null;
                        $(this).closest("div").next(".question").after($(this).closest("div").clone());
                        $(this).closest("div").remove();
                        KindEditor.ready(function (K) {
                            $('#txtEditor' + indexNum).html(nhtml);
                            editors['editor' + indexNum] = K.create('#txtEditor' + indexNum, {
                                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                                items: [
                                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table'],
                                filterMode: false,
                                width:'93%'
                            });
                        });
                    }
                }
            });
            //删除答案选项
            $('.deleteanswer').live("click", function () {
                var questiontype = $(this).closest(".question").find("input[type='radio']:checked").val();
                if (questiontype == "0" || questiontype == "1") {
                    if ($(this).closest(".question").find("input[name='answer']").length <= 1) {
                        Alert("至少要添加一个选项");
                        return false;
                    }
                }
                $(this).closest("tr").remove();
            });
            //删除答案选项

            //添加答案选项
            $("[data-action='addanswer']").live("click", function () {
                var nquestioncount = $(this).closest(".question").attr("data-question-index");
                var oanswercount = $($(this).closest("tr").prev().find("td")[0]).attr("data-answer-index");
                var nanswercount = Number(oanswercount) + 1;
                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat('<tr data-answer-index="{0}" data-answer-id="0"><td>选项:', nanswercount);
                <%if (type == "0")
                  { %>
                appendhtml.AppendFormat('<input id="rdCorrect{0}{1}" class="positionTop2" style="margin-left:10px;" type="radio" name="rdCorrect{0}" /><label id="lblrd{0}{1}" for="rdCorrect{0}{1}">正确</label><input id="ckCorrect{0}{1}" class="positionTop2" style="margin-left:10px; display:none;" type="checkbox" name="ckCorrect{0}" /><label id="lblck{0}{1}" style="display:none;" for="ckCorrect{0}{1}">正确</label>', nquestioncount, nanswercount);
                <%} %>
                appendhtml.AppendFormat('</td><td><input type="text" name="answer" placeholder="选项名称" /><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer"/></td></tr>');
                $(this).closest("tr").before(appendhtml.ToString());
            });
            //添加答案选项


            //删除问题
            $('.deletequestion').live("click", function () {
                if ($('.deletequestion').length <= 1) {
                    Alert("至少添加一个问题");
                    return false;
                }
                var indexNum = $(this).closest("div").attr('data-question-index');
                if (editors['editor' + indexNum]) {
                    KindEditor.remove('#txtEditor' + indexNum);
                    editors['editor' + indexNum] = null;
                }
                $(this).parent().remove();
            });
            //删除问题

            //添加问题
            $("#btnAddQuestion").click(function () {

                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat("<div class=\"question\" data-question-index=\"{0}\"  data-question-id=\"0\">", questioncount);
                appendhtml.AppendFormat("<img src=\"/img/icons/up.png\" class=\"upfield fieldsort\"/>");
                appendhtml.AppendFormat("<img src=\"/img/icons/down.png\" class=\"downfield fieldsort\"/>");
                appendhtml.AppendFormat("<img src=\"/img/delete.png\" class=\"deletequestion\"/>");
                appendhtml.AppendFormat("<table style=\"width:100%;margin-left:10px;\">");
                appendhtml.AppendFormat("<tr class=\"trquestionname\"><td style=\"width:100px;\">问题:</td><td><input type=\"text\" name=\"questionname\" placeholder=\"问题(必填)\" /></td></tr>");
                appendhtml.AppendFormat("<tr><td style=\"width:100px;\">问题类型:</td><td><input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"0\" checked=\"checked\" id=\"rd{0}0\"/><label for=\"rd{0}0\">单选</label>", questioncount);
                <%if (type == "1")
                  { %>
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"1\" id=\"rd{0}1\"/><label for=\"rd{0}1\">多选</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"2\" id=\"rd{0}2\"/><label for=\"rd{0}2\">填空</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"3\" id=\"rd{0}3\"/><label for=\"rd{0}3\">多组</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"4\" id=\"rd{0}4\"/><label for=\"rd{0}4\">省市区</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"5\" id=\"rd{0}5\"/><label for=\"rd{0}5\">年月日</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"6\" id=\"rd{0}6\"/><label for=\"rd{0}6\">文本描述</label>", questioncount);
                <%} %>
                appendhtml.AppendFormat("</td></tr>", questioncount);
                <%if (type == "1")
                  { %>
                appendhtml.AppendFormat("<tr class=\"trgroupname\" style=\"display:none;\"><td>组名:</td><td><input type=\"text\" name=\"answergroupname\" placeholder=\"组名(必填)(,号分隔)\" /></td></tr>");
                appendhtml.AppendFormat("<tr class=\"tryearmonth\" style=\"display:none;\"><td>选择类型:</td><td><select class=\"ddlyearmonth\"><option value=\"month\">年月</option><option value=\"date\">年月日</option><option value=\"time\">时间</option><option value=\"datetime\">日期时间</option></select></td></tr>");
                appendhtml.AppendFormat("<tr class=\"trKindeditor\" style=\"display:none;\"><td>描述:</td><td><div id=\"txtEditor{0}\" style=\"height: 300px;\"></div></td></tr>", questioncount);
                appendhtml.AppendFormat("<tr class=\"trrequired\"><td>选填/必填:</td><td><select class=\"ddlrequired\"><option value=\"1\" selected=\"selected\">必填</option><option value=\"0\">选填</option></select></td></tr>");
                <%} %>
                appendhtml.AppendFormat('<tr data-answer-index="0" data-answer-id="0"><td>选项:');
                <%if (type == "0")
                  { %>
                appendhtml.AppendFormat('<input id="rdCorrect{0}0" class="positionTop2" style="margin-left:10px;" type="radio" name="rdCorrect{0}" /><label id="lblrd{0}0" for="rdCorrect{0}0">正确</label><input id="ckCorrect{0}0" class="positionTop2" style="margin-left:10px; display:none;" type="checkbox" name="ckCorrect{0}" /><label id="lblck{0}0" style="display:none;" for="ckCorrect{0}0">正确</label>', questioncount);
                <%} %>
                appendhtml.AppendFormat('</td><td><input type="text" name="answer" placeholder="选项名称" /><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer"/></td></tr>');
                appendhtml.AppendFormat('<tr data-answer-index="1" data-answer-id="0"><td>选项:');
                <%if (type == "0")
                  { %>
                appendhtml.AppendFormat('<input id="rdCorrect{0}1" class="positionTop2" style="margin-left:10px;" type="radio" name="rdCorrect{0}" /><label id="lblrd{0}1" for="rdCorrect{0}1">正确</label><input id="ckCorrect{0}1" class="positionTop2" style="margin-left:10px; display:none;" type="checkbox" name="ckCorrect{0}" /><label id="lblck{0}1" style="display:none;" for="ckCorrect{0}1">正确</label>', questioncount);
                <%} %>
                appendhtml.AppendFormat('</td><td><input type="text" name="answer" placeholder="选项名称" /><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer"/></td></tr>');
                appendhtml.AppendFormat('<tr data-answer-index="2" data-answer-id="0"><td>选项:');
                <%if (type == "0")
                  { %>
                appendhtml.AppendFormat('<input id="rdCorrect{0}2" class="positionTop2" style="margin-left:10px;" type="radio" name="rdCorrect{0}" /><label id="lblrd{0}2" for="rdCorrect{0}2">正确</label><input id="ckCorrect{0}2" class="positionTop2" style="margin-left:10px; display:none;" type="checkbox" name="ckCorrect{0}" /><label id="lblck{0}2" style="display:none;" for="ckCorrect{0}2">正确</label>', questioncount);
                <%} %>
                appendhtml.AppendFormat('</td><td><input type="text" name="answer" placeholder="选项名称"/><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer"/></td></tr>');
                appendhtml.AppendFormat("<tr><td></td><td><a class=\"button button-rounded button-primary\" data-action=\"addanswer\">添加选项</a></td></tr>");
                appendhtml.AppendFormat("</table>");
                appendhtml.AppendFormat("</div>");
                $(this).before(appendhtml.ToString());
                questioncount++;
            });
            //添加问题

            $("input[type='radio'][name^='rdtype']").live("click", function () {
                $(this).closest(".question").find(".trquestionname").show();
                $(this).closest(".question").find(".trrequired").show();
                $(this).closest(".question").find(".trgroupname").hide();
                $(this).closest(".question").find(".tryearmonth").hide();
                $(this).closest(".question").find(".trKindeditor").hide();

                if ($.inArray($(this).val(), ['2', '4', '5', '6']) >= 0) {
                    $(this).closest(".question").find("input[name='answer']").closest("tr").hide();
                    $(this).closest(".question").find("a[data-action='addanswer']").hide();
                    $(this).closest(".question").find("input[name='rdCorrect0']").hide();

                    if ($(this).val() == "5") {
                        $(this).closest(".question").find(".tryearmonth").show();
                    }
                    else if ($(this).val() == "6") {
                        $(this).closest(".question").find(".trquestionname").hide();
                        $(this).closest(".question").find(".trrequired").hide();
                        var indexNum = $(this).closest("div").attr('data-question-index');
                        if (!editors['editor' + indexNum]) {
                            KindEditor.ready(function (K) {
                                editors['editor' + indexNum] = K.create('#txtEditor' + indexNum, {
                                    uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                                    items: [
                                        'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
                                        'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                                        'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'table'],
                                    filterMode: false,
                                    width:'93%'
                                });
                            });
                        }
                        $(this).closest(".question").find(".trKindeditor").show();

                    }
                }
                else {
                    $(this).closest(".question").find("input[name='answer']").closest("tr").show();
                    $(this).closest(".question").find("a[data-action='addanswer']").show();

                    if ($(this).val() == "1") {
                        $(this).closest(".question").find("input[name^='rdCorrect']").hide();
                        $(this).closest(".question").find("label[id^='lblrd']").hide();
                        $(this).closest(".question").find("input[name^='ckCorrect']").show();
                        $(this).closest(".question").find("label[id^='lblck']").show();
                    }
                    else {
                        $(this).closest(".question").find("input[name^='ckCorrect']").hide();
                        $(this).closest(".question").find("label[id^='lblck']").hide();
                        $(this).closest(".question").find("input[name^='rdCorrect']").show();
                        $(this).closest(".question").find("label[id^='lblrd']").show();

                        if($(this).val() == "3"){
                            $(this).closest(".question").find(".trgroupname").show();
                        }
                    }
                }
            })


            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
                $("#txtThumbnailsPath").show();
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }



            var visible = "<%=model.QuestionnaireVisible%>";
            if (visible == "1") {
                $("#rdovisible1").attr("checked", "checked");
            }
            else {
                $("#rdovisible0").attr("checked", "checked");
            }
            var wxLicensing="<%=model.IsWeiXinLicensing%>";
            if(wxLicensing=="1"){
                $("#need").attr("checked", "checked");
            }else{
                $("#noneed").attr("checked", "checked");
            }

            $('#btnSave').click(function () {
                try {

                   
                    if ($.trim($("#txtQuestionnaireName").val()) == "") {
                        $("#txtQuestionnaireName").focus();
                        return false;
                    }
                    
                    <%if (type == "1"){ %>
                    if ($("#txtStopDate").datetimebox('getValue').length > 0) {
                        if ($.trim($("#txtStopDate").datetimebox('getValue')) == "") {
                            $("#txtStopDate").focus();
                            return false;
                        }
                    }
                    <%}%>
                    
                    if ($("#txtAddScore").length > 0) {
                        if ($.trim($("#txtAddScore").val()) == "") {
                            $("#txtAddScore").focus();
                           
                            return false;
                        }
                    }
                    
                    //if($("#txtButtonLink").val()!=''){
                    //    if(!IsURL($.trim($("#txtButtonLink").val()))){
                    //        $("#txtButtonLink").focus();
                    //        return false;
                    //    }
                    //}
                    
                    

                    var checkresult = true;
                    //检查
                    $(".question").each(function () {
                        if (checkresult == false) return;
                        var QuestionType = parseInt($(this).find("input[type='radio']:checked").val());
                        if (QuestionType != 6) {
                            var QuestionName = $(this).find("input[name='questionname']").first().val();
                            if ($.trim(QuestionName) == "") {
                                $(this).find("input[name='questionname']").first().focus();
                                checkresult = false;
                                return;
                            }
                        }
                        else {
                            var indexNum = $(this).closest("div").attr('data-question-index');
                            var QuestionName = editors['editor' + indexNum].html();
                            if ($.trim(QuestionName) == "") {
                                editors['editor' + indexNum].edit.iframe[0].contentDocument.body.focus();
                                checkresult = false;
                                return;
                            }
                        }
                        if (QuestionType == 0 || QuestionType == 1) {
                            $(this).find("input[name='answer']").each(function () {
                                if (checkresult == false) return;
                                if ($.trim($(this).val()) == "") {
                                    $(this).focus();
                                    checkresult = false;
                                    return;
                                }
                            })
                        }
                    });
                    if (checkresult == false) return false;

                    var model = GetData();
                    if (model === true) return false;
                    
                    var JsonData = JSON.stringify(model);
                    $.messager.progress({ text: '正在保存...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: { Action: "EditQuestionnaire", JsonData: JsonData },
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status == 1) {
                                //alert(resp.Msg);
                                alert("保存成功");
                                window.location.href = "QuestionnaireMgr.aspx?type=" + type;
                            }
                            else {
                                Alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }


            });


            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Questionnaire',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {

                                 $('#imgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    alert(e);
                }
            });




        });



        //获取随机图片
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";

        }


        KindEditor.ready(function (K) {
            editor = K.create('#txtContent', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
        });
        
        function GetData() {
            //问卷模型
            var Questionnaire = {
                Action: "EditQuestionnaire",
                QuestionnaireID: '<%=model.QuestionnaireID%>',
                QuestionnaireName: $("#txtQuestionnaireName").val(),
                QuestionnaireContent: editor.html(),
                QuestionnaireVisible: $("input[name='rdovisible']:checked").val(),
                QuestionnaireSummary: $("#txtQuestionnaireSummary").val(),
                QuestionList: []
            }
            Questionnaire.QuestionnaireType = type;
            Questionnaire.EachPageNum = $("#txtEachPageNum").val();

            if (Questionnaire.EachPageNum=="") {
                Questionnaire.EachPageNum =0;
            }

            Questionnaire.ButtonText = $.trim($("#txtButtonText").val());
            Questionnaire.ButtonLink = $.trim($("#txtButtonLink").val());
            if (Questionnaire.QuestionnaireType == 0) {
                Questionnaire.IsWeiXinLicensing = 0;
                Questionnaire.QuestionnaireStopDate = "<%=DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss")%>";
            }
            else {
                Questionnaire.QuestionnaireStopDate = $("#txtStopDate").datetimebox('getValue');
                Questionnaire.IsWeiXinLicensing = $("input[name=weixin]:checked").val();
            }

            if ($("#txtAddScore").length > 0) {
                Questionnaire.AddScore = $.trim($("#txtAddScore").val());
            }
            else {
                Questionnaire.AddScore = 0;
            }
            if ($("#imgThumbnailsPath").length > 0) {
                Questionnaire.QuestionnaireImage = $("#imgThumbnailsPath").attr("src");
            }
            else {
                var randInt = GetRandomNum(1, 7);
                Questionnaire.QuestionnaireImage = "/img/hb/hb" + randInt + ".jpg";
            }
            var noHaveCorrect = false;
            $(".question").each(function () {
                if (noHaveCorrect) return;
                var Question = {
                    QuestionID : $(this).attr("data-question-id"),
                    QuestionName: '',
                    QuestionType: 0,
                    IsRequired: 0,
                    Answer: []
                }; //问题模型
                Question.QuestionType = parseInt($(this).find("input[type='radio']:checked").val());
                if (Question.QuestionType != 6) {
                    Question.QuestionName = $(this).find("input[name='questionname']").first().val();
                }
                if ($(this).find("select").length > 0) {
                    Question.IsRequired = parseInt($(this).find(".ddlrequired").val());
                }
                else {
                    Question.IsRequired = 1;
                }
                if (Question.QuestionType == 3) {
                    Question.AnswerGroupName = $(this).find("input[name='answergroupname']").first().val()
                }
                else if (Question.QuestionType == 5) {
                    Question.AnswerGroupName = $(this).find(".ddlyearmonth").first().val()
                }
                else if (Question.QuestionType == 6) {
                    var indexNum = $(this).closest("div").attr('data-question-index');
                    Question.QuestionName = editors['editor' + indexNum].html();
                }
                var CorrectCount = 0;
                $(this).find("input[name='answer']").each(function () {
                    var nAnswerID = $(this).closest("tr").attr("data-answer-id");
                    if (($(this).val() != "") && (Question.QuestionType == 0 || Question.QuestionType == 3)) {
                        var IsCorrect = $(this).closest("td").prev().find("input[id^='rdCorrect']").attr("checked") ? "1" : "0";
                        if (IsCorrect == "1") CorrectCount++;
                        Question.Answer.push({ AnswerID:nAnswerID, AnswerName: $(this).val(), IsCorrect: IsCorrect });
                    }
                    else if (($(this).val() != "") && (Question.QuestionType == 1)) {
                        var IsCorrect = $(this).closest("td").prev().find("input[id^='ckCorrect']").attr("checked") ? "1" : "0";
                        if (IsCorrect == "1") CorrectCount++;
                        Question.Answer.push({ AnswerID:nAnswerID,  AnswerName: $(this).val(), IsCorrect: IsCorrect });
                    }
                })

                <%if (type == "0")
                  { %>
                if (Question.QuestionType == 0 && CorrectCount != 1) {
                    $.messager.alert("系统提示", "单选题[" + Question.QuestionName + "]必须有且仅有1个正确答案");
                    noHaveCorrect = true;
                }
                else if (Question.QuestionType == 1 && CorrectCount == 0) {
                    $.messager.alert("系统提示", "多选题[" + Question.QuestionName + "]必须要设置正确答案");
                    noHaveCorrect = true;
                }
                <%} %>
                Questionnaire.QuestionList.push(Question);
            });
            if (noHaveCorrect === true) return noHaveCorrect;
            return Questionnaire;
        }


    </script>
</asp:Content>
