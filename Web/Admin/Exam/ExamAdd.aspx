﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ExamAdd.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Exam.ExamAdd" %>

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
            width: 98%;
            position: relative;
        }

        .fieldsort {
            float: left;
            margin-left: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .deletequestion {
            float: right;
            right: 5px;
            margin-top: -5px;
            cursor: pointer;
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

        .trrequired {
            display: none;
        }

     
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>添加新试卷</span> <a title="返回管理" style="float: right; margin-right: 20px;" href="ExamMgr.aspx" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>


    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%;">

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">试卷名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtQuestionnaireName" maxlength="100" value="" style="width: 100%;" placeholder="试卷名称(必填)" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">考试时长：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtExamMin" maxlength="3" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" value="" style="width: 200px;" placeholder="考试时长(必填)" />&nbsp;分钟
                    </td>
                </tr>
                <tr class="hidden">
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="" width="80px" height="80px" id="imgThumbnailsPath" /><br />
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

                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtQuestionnaireSummary" value="" style="width: 100%;" placeholder="描述(选填)" />

                    </td>
                </tr>
                <tr class="hidden">
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">考试说明：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtContent" style="width: 100%; height: 200px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>

                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">结束时间：
                    </td>
                    <td width="*" align="left">

                        <input class="easyui-datetimebox" id="txtStopDate" />
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">赠送积分：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAddScore" value="0" style="width: 100%;" />
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">答卷后按钮文字：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtButtonText" value="" style="width: 100%;" />
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">答卷后按钮链接：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtButtonLink" value="" style="width: 100%;" />
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">每页题目数：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtEachPageNum" value="10" style="width: 100%;" /><span style="color: red;">题目数为0时不进行分页</span>
                    </td>
                </tr>
                <tr class="hidden">
                    <td style="width: 100px;" align="right" class="tdTitle">是否需要微信高级授权：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" id="noneed" value="0" checked="checked" name="weixin" /><label for="noneed">否</label>
                        <input type="radio" id="need" value="1" name="weixin" /><label for="need">是</label>
                    </td>
                </tr>
                <tr class="hidden">
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>

            </table>
            <strong style="font-size: 20px;">题目列表:</strong>
            <div class="question" data-question-index="0">
                <img src="/img/icons/up.png" class="upfield fieldsort" />
                <img src="/img/icons/down.png" class="downfield fieldsort" />
                <img src="/img/delete.png" class="deletequestion" />
                <table style="width: 100%; margin-left: 10px;">
                    <tr class="trquestionname">
                        <td style="width: 100px;">题目:</td>
                        <td>
                            <input type="text" maxlength="100" name="questionname" placeholder="题目(必填)" /></td>
                    </tr>
                    <tr>
                        <td style="width: 100px;">题目类型:</td>
                        <td>
                            <input type="radio" class="positionTop2" name="rdtype0" value="0" checked="checked" id="rd00" /><label for="rd00">单选</label>

                            <input name="rdtype0" class="positionTop2" type="radio" value="1" id="rd01" /><label for="rd01">多选</label>
                            <input name="rdtype0" class="positionTop2" type="radio" value="2" id="rd02" /><label for="rd02">填空</label>
                            <input name="rdtype0" class="positionTop2" type="radio" value="3" id="rd03" /><label for="rd03">多组</label>
                            <input name="rdtype0" class="positionTop2" type="radio" value="4" id="rd04" /><label for="rd04">省市区</label>
                            <input name="rdtype0" class="positionTop2" type="radio" value="5" id="rd05" /><label for="rd05">年月日</label>
                            <input name="rdtype0" class="positionTop2" type="radio" value="6" id="rd06" /><label for="rd06">文本描述</label>

                        </td>
                    </tr>

                    <tr class="trgroupname" style="display: none;">
                        <td>组名:</td>
                        <td>
                            <input type="text" name="answergroupname" placeholder="组名(必填)(,号分隔)" />
                        </td>
                    </tr>
                    <tr class="tryearmonth" style="display: none;">
                        <td>选择类型:</td>
                        <td>
                            <select class="ddlyearmonth">
                                <option value="month">年月</option>
                                <option value="date">年月日</option>
                                <option value="time">时间</option>
                                <option value="datetime">日期时间</option>
                            </select>
                        </td>
                    </tr>
                    <tr class="trKindeditor" style="display: none;">
                        <td>描述:</td>
                        <td>
                            <div id="txtEditor0" style="height: 300px;">
                            </div>
                        </td>
                    </tr>
                    <tr class="trrequired">
                        <td>选填/必填:</td>
                        <td>
                            <select class="ddlrequired">
                                <option value="0">选填</option>
                                <option value="1">必填</option>

                            </select>
                        </td>
                    </tr>


                    <tr>
                        <td></td>
                        <td><a class="button button-rounded button-primary" data-action="addanswer">添加答案选项</a></td>
                    </tr>
                </table>
            </div>
            <a class="button button-rounded button-primary" style="width: 90%; margin-top: 10px; margin-bottom: 10px;" id="btnAddQuestion">添加题目</a>

            <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top: 10px;">
                <a href="javascript:;" id="btnSave" class="button button-rounded button-primary" style="width: 200px;">确定添加试卷</a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/json2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var editor;
        var questioncount = 1; //题目数量
        var type = "2";
        var editors = {};
        $(function () {
            //上移
            $(".upfield").live("click", function () {
                if ($(this).closest("div").prev(".question").length > 0) {
                    var QuestionType = parseInt($(this).closest("div").find("input[type='radio']:checked").val());
                    if (QuestionType != 6) {
                        $(this).closest("div").prev(".question").before($(this).closest("div").clone());
                        $(this).closest("div").remove();
                    }
                    else {
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
                                width: '93%'
                            });
                        });
                    }
                }
            });
            //下移
            $(".downfield").live("click", function () {
                if ($(this).closest("div").next(".question").length > 0) {
                    var QuestionType = parseInt($(this).closest("div").find("input[type='radio']:checked").val());
                    if (QuestionType != 6) {
                        $(this).closest("div").next(".question").after($(this).closest("div").clone());
                        $(this).closest("div").remove();
                    }
                    else {
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
                                width: '93%'
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
                if (confirm("确定要删除此答案选项")) {
                    $(this).closest("tr").remove();
                }


            });
            //删除答案选项

            //添加答案选项
            $("[data-action='addanswer']").live("click", function () {
                var nquestioncount = $(this).closest(".question").attr("data-question-index");
                var oanswercount = $($(this).closest("tr").prev().find("td")[0]).attr("data-answer-index");
                var nanswercount = Number(oanswercount) + 1;
                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat('<tr><td data-answer-index="{0}">选项:', nanswercount);

                appendhtml.AppendFormat('</td><td><input maxlength="150" type="text" name="answer" placeholder="选项名称" /><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer"/></td></tr>');
                $(this).closest("tr").before(appendhtml.ToString());
            });
            //添加答案选项


            //删除题目
            $('.deletequestion').live("click", function () {
                if ($('.deletequestion').length <= 1) {
                    Alert("至少添加一个题目");
                    return false;
                }
                if (confirm("确定要删除此题目?")) {
                    var indexNum = $(this).closest("div").attr('data-question-index');
                    if (editors['editor' + indexNum]) {
                        KindEditor.remove('#txtEditor' + indexNum);
                        editors['editor' + indexNum] = null;
                    }
                    $(this).parent().remove();
                }



            });
            //删除题目

            //添加题目
            $("#btnAddQuestion").click(function () {
                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat("<div class=\"question\" data-question-index=\"{0}\">", questioncount);
                appendhtml.AppendFormat("<img src=\"/img/icons/up.png\" class=\"upfield fieldsort\"/>");
                appendhtml.AppendFormat("<img src=\"/img/icons/down.png\" class=\"downfield fieldsort\"/>");
                appendhtml.AppendFormat("<img src=\"/img/delete.png\" class=\"deletequestion\"/>");
                appendhtml.AppendFormat("<table style=\"width:100%;margin-left:10px;\">");
                appendhtml.AppendFormat("<tr class=\"trquestionname\"><td style=\"width:100px;\">题目:</td><td><input type=\"text\" name=\"questionname\" placeholder=\"题目(必填)\" /></td></tr>");
                appendhtml.AppendFormat("<tr><td style=\"width:100px;\">题目类型:</td><td><input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"0\" checked=\"checked\" id=\"rd{0}0\"/><label for=\"rd{0}0\">单选</label>", questioncount);

                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"1\" id=\"rd{0}1\"/><label for=\"rd{0}1\">多选</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"2\" id=\"rd{0}2\"/><label for=\"rd{0}2\">填空</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"3\" id=\"rd{0}3\"/><label for=\"rd{0}3\">多组</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"4\" id=\"rd{0}4\"/><label for=\"rd{0}4\">省市区</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"5\" id=\"rd{0}5\"/><label for=\"rd{0}5\">年月日</label>", questioncount);
                appendhtml.AppendFormat("<input  type=\"radio\" class=\"positionTop2\" name=\"rdtype{0}\" value=\"6\" id=\"rd{0}6\"/><label for=\"rd{0}6\">文本描述</label>", questioncount);

                appendhtml.AppendFormat("</td></tr>", questioncount);

                appendhtml.AppendFormat("<tr class=\"trgroupname\" style=\"display:none;\"><td>组名:</td><td><input type=\"text\" name=\"answergroupname\" placeholder=\"组名(必填)(,号分隔)\" /></td></tr>");
                appendhtml.AppendFormat("<tr class=\"tryearmonth\" style=\"display:none;\"><td>选择类型:</td><td><select class=\"ddlyearmonth\"><option value=\"month\">年月</option><option value=\"date\">年月日</option><option value=\"time\">时间</option><option value=\"datetime\">日期时间</option></select></td></tr>");
                appendhtml.AppendFormat("<tr class=\"trKindeditor\" style=\"display:none;\"><td>描述:</td><td><div id=\"txtEditor{0}\" style=\"height: 300px;\"></div></td></tr>", questioncount);
                appendhtml.AppendFormat("<tr class=\"trrequired\" class=\"hidden\"><td>选填/必填:</td><td><select class=\"ddlrequired\"><option value=\"0\">选填</option><option value=\"1\" selected=\"selected\">必填</option></select></td></tr>");

                appendhtml.AppendFormat('<tr><td data-answer-index="0">选项:');

                appendhtml.AppendFormat('</td><td><input type="text" name="answer" placeholder="选项名称" /><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer"/></td></tr>');
                appendhtml.AppendFormat('<tr><td data-answer-index="1">选项:');

                appendhtml.AppendFormat('</td><td><input type="text" name="answer" placeholder="选项名称" /><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer"/></td></tr>');
                appendhtml.AppendFormat('<tr><td data-answer-index="2">选项:');

                appendhtml.AppendFormat('</td><td><input type="text" name="answer" placeholder="选项名称"/><img src="/img/delete.png" width="20" height="20" alt="删除选项" class="deleteanswer"/></td></tr>');
                appendhtml.AppendFormat("<tr><td></td><td><a class=\"button button-rounded button-primary\" data-action=\"addanswer\">添加答案选项</a></td></tr>");
                appendhtml.AppendFormat("</table>");
                appendhtml.AppendFormat("</div>");
                $(this).before(appendhtml.ToString());
                questioncount++;
            });
            //添加题目


            //if ($.browser.msie) { //ie 下
            //    //缩略图
            //    $("#auploadThumbnails").hide();
            //    $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            //    $("#txtThumbnailsPath").show();
            //}
            //else {
            //    $("#txtThumbnailsPath").hide(); //缩略图
            //}

            //GetRandomHb();


            $('#btnSave').click(function () {

                if (confirm("确定添加试卷?添加后将不能修改,请仔细核对?")) {





                    try {


                        var checkresult = true;

                        if ($.trim($("#txtQuestionnaireName").val()) == "") {
                            $("#txtQuestionnaireName").focus();
                            return false;
                        }
                        if ($("#txtExamMin").val() == "") {
                            $("#txtExamMin").focus();
                            return false;

                        }



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
                        var model = GetData(checkresult);
                        if (model === true) return false;
                        //
                        var JsonData = JSON.stringify(model);
                        $.messager.progress({ text: '正在添加...' });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: { Action: "AddQuestionnaire", JsonData: JsonData },
                            dataType: "json",
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.Status == 1) {
                                    alert("添加成功");
                                    window.location.href = "ExamMgr.aspx";
                                }
                                else {
                                    Alert(resp.Msg);
                                }
                            }
                        });

                    } catch (e) {
                        Alert(e);
                    }

                }



            });


            //$("#txtThumbnailsPath").live('change', function () {
            //    try {
            //        $.messager.progress({ text: '正在上传图片。。。' });

            //        $.ajaxFileUpload(
            //         {
            //             url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Questionnaire',
            //             secureuri: false,
            //             fileElementId: 'txtThumbnailsPath',
            //             dataType: 'json',
            //             success: function (resp) {
            //                 $.messager.progress('close');
            //                 if (resp.Status == 1) {

            //                     $('#imgThumbnailsPath').attr('src', resp.ExStr);
            //                 }
            //                 else {
            //                     Alert(resp.Msg);
            //                 }
            //             }
            //         }
            //        );

            //    } catch (e) {
            //        Alert(e);
            //    }
            //});

            $("input[type='radio'][name^='rdtype']").live("click", function () {
                $(this).closest(".question").find(".trquestionname").show();
                $(this).closest(".question").find(".trrequired").hide();
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
                                    width: '93%'
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


                        if ($(this).val() == "3") {
                            $(this).closest(".question").find(".trgroupname").show();
                        }
                    }
                }
            });
        });



        ////获取随机图片
        //function GetRandomHb() {
        //    var randInt = GetRandomNum(1, 7);
        //    $("#imgThumbnailsPath").attr("src", "/img/hb/hb" + randInt + ".jpg");

        //}


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

        function GetData(checkresult) {
            //模型
            var Questionnaire = {
                QuestionnaireName: $("#txtQuestionnaireName").val(),
                ExamMinute: $("#txtExamMin").val(),
                QuestionnaireContent: editor.html(),
                QuestionnaireVisible: 1,
                QuestionnaireSummary: $("#txtQuestionnaireSummary").val(),
                QuestionList: []
            }
            Questionnaire.QuestionnaireType = type;
            Questionnaire.QuestionnaireStopDate = "<%=DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss")%>";
            Questionnaire.EachPageNum = $("#txtEachPageNum").val();
            Questionnaire.ButtonText = $.trim($("#txtButtonText").val());
            Questionnaire.ButtonLink = $.trim($("#txtButtonLink").val());
            Questionnaire.AddScore = 0;
            //if ($("#imgThumbnailsPath").length > 0) {
            Questionnaire.QuestionnaireImage = $("#imgThumbnailsPath").attr("src");

            //}
            //else {
            //    var randInt = GetRandomNum(1, 7);
            //    Questionnaire.QuestionnaireImage = "/img/hb/hb" + randInt + ".jpg";
            //}
            var noHaveCorrect = false;
            $(".question").each(function () {
                if (noHaveCorrect) return;
                var Question = {
                    QuestionName: '',
                    QuestionType: 0,
                    IsRequired: 0,
                    Answer: []
                }; //题目模型
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
                    if (($(this).val() != "") && (Question.QuestionType == 0 || Question.QuestionType == 3)) {
                        var IsCorrect = $(this).closest("td").prev().find("input[id^='rdCorrect']").attr("checked") ? "1" : "0";
                        if (IsCorrect == "1") CorrectCount++;
                        Question.Answer.push({ AnswerName: $(this).val(), IsCorrect: IsCorrect });
                    }
                    else if (($(this).val() != "") && (Question.QuestionType == 1)) {
                        var IsCorrect = $(this).closest("td").prev().find("input[id^='ckCorrect']").attr("checked") ? "1" : "0";
                        if (IsCorrect == "1") CorrectCount++;
                        Question.Answer.push({ AnswerName: $(this).val(), IsCorrect: IsCorrect });
                    }
                })


                Questionnaire.QuestionList.push(Question);
            });
            if (noHaveCorrect === true) return noHaveCorrect;
            return Questionnaire;
        }
    </script>
</asp:Content>

