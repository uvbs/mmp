<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="QuestionnaireSetEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Questionnaire.QuestionnaireSetEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .txtPath{width:70%;}
        .imgPath{cursor:pointer;}
        .txtWidth200{width:200px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
 <div class="title">当前位置：&nbsp;<a href="QuestionnaireSetMgr.aspx">答题管理</a>&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%= Request["id"]=="0"?"新建":"编辑" %>答题</span> <a title="返回答题管理" style="float:right;margin-top:10px;margin-right:20px;" href="QuestionnaireSetMgr.aspx" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
 </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%;">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTitle" style="width: 70%;" placeholder="标题(必填)"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图"  width="80px" height="80px" src="/img/hb/hb1.jpg" class="imgPath" id="imgImg"/><br />
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG,PNG 格式图片，图片最佳显示效果大小为80*80。
                        <br />
                        <input id="fileImg" type="file" name="file1" class="filePath" style="display:none;"/>
                        <input id="txtImg" type="text" class="txtPath" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        描述：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtDescription" rows="5" style="width: 70%;" placeholder="描述(必填)"></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        背景图：
                    </td>
                    <td>
                        <table style="width:100%;">
                            <tr>
                                <td>首页背景</td>
                                <td>答题页Banner</td>
                                <%--<td>结果页背景</td>--%>
                            </tr>
                            <tr>
                                <td>
                                    <img width="80px" height="80px" src="/img/hb/hb1.jpg" class="imgPath" id="imgBgImgIndex" /><br />
                                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />点击图片上传，图片最佳显示效果大小为640*1008。<br />
                                    <input id="fileBgImgIndex" type="file" name="file1" class="filePath" style="display:none;" />
                                    <input id="txtBgImgIndex" type="text" class="txtPath" />
                                </td>
                                <td>
                                    <img width="80px" height="80px" src="/img/hb/hb1.jpg" class="imgPath" id="imgBgImgAnswer" /><br />
                                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />点击图片上传，图片最佳显示效果大小为640*320。<br />
                                    <input id="fileBgImgAnswer" type="file" name="file1" class="filePath" style="display:none;"/>
                                    <input id="txtBgImgAnswer" type="text" class="txtPath" />
                                </td>
                                <%--<td>
                                    <img width="80px" height="80px" src="/img/hb/hb1.jpg" class="imgPath" id="imgBgImgEnd" /><br />
                                    <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />图片最佳显示效果大小为640*1008。<br />
                                    <input id="fileBgImgEnd" type="file" name="file1" class="filePath" style="display:none;" />
                                    <input id="txtBgImgEnd" type="text" class="txtPath" />
                                </td>--%>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        题库绑定：
                    </td>
                    <td width="*" align="left">
                        <select id="sltQuestionnaire">

                        </select>
                        题目数:<input type="text" id="txtQuestionCount" style="width:60px;" />
                        总题数:<span id="spanQuestionTotal" style="color:red;">0</span>;
                        每题得分:<input type="text" id="txtQuestionScore" class="easyui-numberbox" data-options="min:0,precision:0" style="width:60px;" />
                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        随机：
                    </td>
                    <td align="left">
                        <input id="chkIsQuestionRandom" type="checkbox" class="positionTop2" /><label for="chkIsQuestionRandom">问题随机</label>
                        <input id="chkIsOptionRandom" type="checkbox" class="positionTop2" /><label for="chkIsOptionRandom">选项随机</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        多次答题：
                    </td>
                    <td align="left">
                        <input id="chkIsMoreAnswer" type="checkbox" class="positionTop2" checked="checked" /><label for="chkIsMoreAnswer">能多次答题</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        胜利正确数：
                    </td>
                    <td align="left">
                        <input type="text" id="txtWinCount" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        胜利描述：
                    </td>
                    <td align="left">
                        <input type="text" id="txtWinDescription" class="txtWidth200"  />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        胜利按钮：
                    </td>
                    <td align="left">
                        文字:<input type="text" id="txtWinBtnText" class="txtWidth200"  />
                        链接:<input type="text" id="txtWinBtnUrl" class="txtWidth200" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        胜利可得积分数：
                    </td>
                    <td align="left">
                        <input type="text" id="txtScore" class="easyui-numberbox" data-options="min:0,precision:0"  />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        可得积分次数：
                    </td>
                    <td align="left">
                        <input type="text" id="txtScoreNum" class="easyui-numberbox" data-options="min:0,precision:0" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        答题开始时间：
                    </td>
                    <td align="left">
                        <input type="text" id="txtStartDate" class="easyui-datetimebox" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        答题结束时间：
                    </td>
                    <td align="left">
                        <input type="text" id="txtEndDate" class="easyui-datetimebox" />
                    </td>
                </tr>
            </table>
            <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top:10px;">
            <a href="javascript:;"  id="btnSave" class="button button-rounded button-primary" style="width:200px;">保存</a> 
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Serv/API/Admin/QuestionnaireSet/";
        var curId = <% =Request["id"] %>;
        var oldData = null;
        var selectData = null;
        var sDate = '<%= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>';
        var eDate = '<%= DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss") %>';
        $(function () {
            //加载选择问卷
            loadSelectQuestionnaire();
            if(curId >0){
                loadSetData();
            }

            $(".imgPath").live("click", function () {
                $(this).closest("td").find(".filePath").click();
            });
            $(".txtPath").live('change', function () {
                var nimg = $(this).closest("td").find(".imgPath");
                $(nimg).attr('src', $(this).val());
            })
            $("#sltQuestionnaire").live('change', function () {
                var dcount = $(this).find("option:selected").attr("data-count");
                $("#spanQuestionTotal").text(dcount);
                var ntotal = $("#txtQuestionCount").val();
                if(parseInt(dcount)<parseInt(ntotal)){
                    $("#txtQuestionCount").val(dcount);
                }
                ntotal = $("#txtQuestionCount").val();
                var wcount = $("#txtWinCount").val();
                if(wcount == "") wcount = "0";
                if(parseInt(ntotal)<parseInt(wcount)){
                    $("#txtWinCount").val(ntotal);
                }
            })
            $("#txtQuestionCount").live('change', function () {
                var ncount = $(this).val();
                var dcount = $("#sltQuestionnaire").find("option:selected").attr("data-count");
                if(parseInt(ncount)>parseInt(dcount)){
                    $("#txtQuestionCount").val(dcount);
                    Alert("不能大于总题数");
                }
            })
            $("#txtWinCount").live('change', function () {
                var ncount = $(this).val();
                var ntotal = $("#txtQuestionCount").val();
                if(parseInt(ncount)>parseInt(ntotal)){
                    $("#txtWinCount").val(ntotal);
                    Alert("不能大于答题数");
                }
            })
            
            $(".filePath").live('change', function () {
                var nimg = $(this).closest("td").find(".imgPath");
                var ntxt = $(this).closest("td").find(".txtPath");
                var nid = $(this).attr("id");
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Questionnaire',
                         secureuri: false,
                         fileElementId: nid,
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $(nimg).attr('src', resp.ExStr);
                                 $(ntxt).val(resp.ExStr);
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

            $("#btnSave").live('click',function(){
                var model = GetModel();
                if(model.Title == ""){
                    Alert("请输入标题");
                    $("#txtTitle").focus();
                    return;
                }
                if(model.QuestionnaireId == "0"){
                    Alert("请选择题库");
                    return;
                }
                if(model.QuestionCount == "0"){
                    Alert("请输入题目数");
                    $("#txtQuestionCount").focus();
                    return;
                }
                if(isNaN(model.QuestionCount)){
                    Alert("题目数请输入数字");
                    $("#txtQuestionCount").focus();
                    return;
                }
                if(model.QuestionScore == ""){
                    Alert("请输入每题得分");
                    $("#txtQuestionScore").focus();
                    return;
                }
                if(isNaN(model.QuestionScore)){
                    Alert("每题得分请输入数字");
                    $("#txtQuestionScore").focus();
                    return;
                }
                if(model.StartDate == ""){
                    Alert("请选择开始时间");
                    $("#txtStartDate").focus();
                    return;
                }
                if(model.EndDate == ""){
                    Alert("请选择结束时间");
                    $("#txtEndDate").focus();
                    return;
                }
                if(isNaN(model.Score)){
                    Alert("胜利可得积分数请输入数字");
                    $("#txtScore").focus();
                    return;
                }
                if(isNaN(model.ScoreNum)){
                    Alert("可得积分次数请输入数字");
                    $("#txtScoreNum").focus();
                    return;
                }
                var action = curId >0?"Update.ashx":"Add.ashx";
                $.ajax({
                    type: 'post',
                    url: handlerUrl+action,
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        if (resp.status) {
                            document.location.href="QuestionnaireSetMgr.aspx";
                        }
                        else{
                            Alert(resp.msg);
                        }
                    }
                });
            });
        });
        function loadSelectQuestionnaire() {
            $.ajax({
                type: 'post',
                url: "/Serv/API/Admin/Question/SelectList.ashx?type=0",
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        $("#sltQuestionnaire").html("");
                        var str = new StringBuilder();
                        str.AppendFormat('<option value="0" data-count="0">请选择</option>')
                        for (var i = 0; i < resp.result.length; i++) {
                            str.AppendFormat('<option value="{0}" data-count="{2}">{1}</option>',
                                resp.result[i].value, resp.result[i].name, resp.result[i].count);
                        }
                        $("#sltQuestionnaire").append(str.ToString());
                        selectData = resp.result;
                        if(oldData!=null)SetOldData();
                    }
                }
            });
        }
        function loadSetData(){
            $.ajax({
                type: 'post',
                url: handlerUrl+"Get.ashx",
                data: {AutoID:curId},
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        oldData = resp.result;
                        if(selectData!=null)SetOldData();
                    }
                    else{
                        Alert(resp.msg);
                        $("#btnSave").hide();
                    }
                }
            });
        }
        function ClearData(){
            $("#txtTitle").val("");
            $("#txtImg").val("");
            $("#imgImg").attr('src',"/img/hb/hb1.jpg");
            $("#txtDescription").val("");
            $("#txtBgImgIndex").val("");
            $("#imgBgImgIndex").attr('src',"/img/hb/hb1.jpg");
            $("#txtBgImgAnswer").val("");
            $("#imgBgImgAnswer").attr('src',"/img/hb/hb1.jpg");
            $("#sltQuestionnaire").val("0")
            $("#txtQuestionCount").val("");
            $("#txtQuestionScore").numberbox("setValue",20);
            $("#chkIsQuestionRandom").attr("checked",false);
            $("#chkIsOptionRandom").attr("checked",false);
            $("#chkIsMoreAnswer").attr("checked",true);
            $("#txtWinCount").val("");
            $("#txtWinDescription").val("");
            $("#txtWinBtnText").val("");
            $("#txtWinBtnUrl").val("");
            $("#txtScore").numberbox("setValue",0);
            $("#txtScoreNum").numberbox("setValue",0);
            $("#txtStartDate").datetimebox("setValue",sDate);
            $("#txtEndDate").datetimebox("setValue",eDate);
        }
        function SetOldData(){
            $("#txtTitle").val(oldData.Title);
            $("#txtImg").val(oldData.Img);
            $("#imgImg").attr('src',oldData.Img);
            $("#txtDescription").val(oldData.Description);
            $("#txtBgImgIndex").val(oldData.BgImgIndex);
            $("#imgBgImgIndex").attr('src',oldData.BgImgIndex);
            $("#txtBgImgAnswer").val(oldData.BgImgAnswer);
            $("#imgBgImgAnswer").attr('src',oldData.BgImgAnswer);
            $("#sltQuestionnaire").val(oldData.QuestionnaireId);
            var dcount = $("#sltQuestionnaire").find("option:selected").attr("data-count");
            $("#spanQuestionTotal").text(dcount);
            $("#txtQuestionCount").val(oldData.QuestionCount);
            $("#txtQuestionScore").numberbox("setValue",oldData.QuestionScore);
            if(oldData.IsQuestionRandom==1){
                $("#chkIsQuestionRandom").attr("checked",true);
            }
            if(oldData.IsOptionRandom==1){
                $("#chkIsOptionRandom").attr("checked",true);
            }
            if(oldData.IsMoreAnswer==1){
                $("#chkIsMoreAnswer").attr("checked",true);
            }
            $("#txtWinCount").val(oldData.WinCount);
            $("#txtWinDescription").val(oldData.WinDescription);
            $("#txtWinBtnText").val(oldData.WinBtnText);
            $("#txtWinBtnUrl").val(oldData.WinBtnUrl);
            $("#txtScore").numberbox("setValue",oldData.Score);
            $("#txtScoreNum").numberbox("setValue",oldData.ScoreNum);
            $("#txtStartDate").datetimebox("setValue",oldData.StartDate);
            $("#txtEndDate").datetimebox("setValue",oldData.EndDate);
        }
        function GetModel(){
            //问卷模型
            var QuestionnaireId = $.trim($("#sltQuestionnaire").val());
            if(QuestionnaireId == "") QuestionnaireId = "0";
            var ModelSet = {
                AutoID: curId,
                Title: $.trim($("#txtTitle").val()),
                Img: $.trim($("#txtImg").val()),
                Description: $.trim($("#txtDescription").val()),
                BgImgIndex: $.trim($("#txtBgImgIndex").val()),
                BgImgAnswer: $.trim($("#txtBgImgAnswer").val()),
                QuestionnaireId: QuestionnaireId,
                QuestionCount: $.trim($("#txtQuestionCount").val()),
                QuestionScore: $.trim($("#txtQuestionScore").numberbox("getValue")),
                IsQuestionRandom: $("#chkIsQuestionRandom").attr("checked") == "checked"?1:0,
                IsOptionRandom: $("#chkIsOptionRandom").attr("checked") == "checked"?1:0,
                IsMoreAnswer: $("#chkIsMoreAnswer").attr("checked") == "checked"?1:0,
                WinCount: $.trim($("#txtWinCount").val()),
                WinDescription: $.trim($("#txtWinDescription").val()),
                WinBtnText: $.trim($("#txtWinBtnText").val()),
                WinBtnUrl: $.trim($("#txtWinBtnUrl").val()),
                Score: $.trim($("#txtScore").numberbox("getValue")),
                ScoreNum: $.trim($("#txtScoreNum").numberbox("getValue")),
                StartDate: $.trim($("#txtStartDate").datetimebox("getValue")),
                EndDate: $.trim($("#txtEndDate").datetimebox("getValue"))
            }
            if(ModelSet.Score =="") ModelSet.Score=0;
            if(ModelSet.ScoreNum =="") ModelSet.ScoreNum=0;
            return ModelSet;
        }
    </script>
</asp:Content>
