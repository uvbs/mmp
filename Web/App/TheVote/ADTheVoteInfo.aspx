<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ADTheVoteInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.TheVote.ADTheVoteInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 30px;
        }
        input[type='text']
        {
         height:30px;   
            
         }
         .datebox-calendar-inner
         {
             
             height:195px;
          }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;投票 >
    <%=Tag%>投票&nbsp;
    
   <a href="/App/TheVote/TheVoteInfoMgr.aspx" style="float: right; margin-right: 20px; color: Black;" title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
       
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        投票标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="TxtVoteName" class="" style="width: 100%;" placeholder="必填" />
                    </td>
                </tr>
                                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSummary" style="width: 100%;" placeholder="将显示在微信分享描述中" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 50px;" align="right" class="tdTitle">缩略图：
                    </td>
                    <td align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath"
                            class="rounded" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        是否公开：
                    </td>
                    <td width="*" align="left">
                        <input type="checkbox" name="IsVoteOpen" id="ckIsVoteOpen" v="1" /><label for="ckIsVoteOpen">是否公开</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        投票类型：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdVotePosition" id="VoteSelectTrue" checked="checked" v="1" /><label
                            for="VoteSelectTrue">单选</label>
                        <input type="radio" name="rdVotePosition" id="VoteSelectFlase" v="2" /><label for="VoteSelectFlase">多选</label>
                    </td>
                </tr>
                  <tr id="trMaxSelectItemCount" style="display:none;">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        最多可以同时选择几个选项：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtMaxSelectItemCount"  onkeyup='this.value=this.value.replace(/[^1-9]\D*$/,"")'  placeholder="最多可以同时选择几个选项"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        投票结束时间：
                    </td>
                    <td width="*" align="left">
                        <input id="txtOverDate"placeholder="投票结束时间" cssclass="ipt1"  type="text" class="easyui-datetimebox"  />
                     <%--   <input type="text" id="txtOverDate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'});"
                             />--%>
                    </td>
                </tr>

                <tr>
                    <td>
                    </td>
                    <td width="*" align="left" colspan="2" id="tdAnswer">
                        <div data-role="fieldcontain" style="width: 100%;">
                            <input name="answer" placeholder="选项1" class="anser_input_class" style="width: 70%;
                                " type="text" value="">
                        </div>
                        <div data-role="fieldcontain" style="width: 100%;">
                            <input name="answer" placeholder="选项2" class="anser_input_class" type="text" style="width: 70%;
                                " value="">
                        </div>
                        <div data-role="fieldcontain" style="width: 100%;">
                            <input name="answer" placeholder="选项3" class="anser_input_class" style="width: 70%;
                                " type="text" value=""><span name="ResetCurr" >&nbsp;删除选项</span>
                        </div>
                    </td>
                    <td>
                        <input type="hidden" id="Aid" value="0" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <a  class="button button-rounded button-primary" href="javascript:void(0)" onclick="Addanswer()">添加投票选项</a>
                    </td>
                </tr>
                <%--<tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;" class="button button-rounded button-primary">
                            保存</a>
                    </td>
                </tr>--%>
            </table>
             <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 55px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top:10px;">
                             <a href="javascript:;" id="btnSave" style="font-weight: bold;width:200px;" class="button button-rounded button-primary">
                            保存</a>
                            </div>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXTheVoteInfoHandler.ashx";
     var currAction = '<%=currAction %>';
     var editor;
     var currID = '<%=AutoId %>';
     $(function () {

         $("span[name='ResetCurr']").live("click", function () {
             $(this).parent().fadeOut();
         });

         $("#txtThumbnailsPath").hide(); //缩略图
         if (currAction == 'add') {
             //获取随机海报
             GetRandomHb();
             
         }
         else {
             ShowEdit(currID);
         }

         $('#btnSave').click(function () {
             var answers = "";
             $("input[name='answer']").each(function () {
                 answers += $(this).val() + ",";
             })
             try {
                 var model =
                    {
                        AutoId: currID,
                        Action: "InsertTheVoteInfo",
                        VoteName: $.trim($('#TxtVoteName').val()),
                        VoteOverDate: $('#txtOverDate').datetimebox('getValue'),
                        VotePosition: $("input[name=rdVotePosition]:checked").attr("v"),
                        Answer: answers,
                        aid: $("#Aid").val(),
                        MaxSelectItemCount: $(txtMaxSelectItemCount).val(),
                        ThumbnailsPath: $('#imgThumbnailsPath').attr('src'),
                        Summary: $("#txtSummary").val()
                    };

                 if (model.VoteName == '') {
                     $('#VoteName').focus();
                     Alert('请输入投票标题');
                     return;
                 }
                 if (model.VoteOverDate=="") {
                     
                     Alert('请输入结束时间');
                     return;
                 }
                 $.messager.progress({ text: '正在处理...' });
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: "json",
                     success: function (resp) {
                         $.messager.progress('close');
                         if (resp.Status == 1) {
                             //if (currAction == 'add') {
                             //    ResetCurr();
                             //}
                             Alert(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }
                 });

             } catch (e) {
                 Alert(e);
             }


         });

         $('#btnReset').click(function () {
             ResetCurr();
         });

         $("#txtOverDate").datebox({
             editable:false  
         });


         $("[name='rdVotePosition']").live("click", function () {
             if ($(this).attr("v") == "2") {
                 $(trMaxSelectItemCount).show();
             }
             else {
                 $(trMaxSelectItemCount).hide();
             }

         });

         //活动缩略图
         $("#txtThumbnailsPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                  {
                      url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                      secureuri: false,
                      fileElementId: 'txtThumbnailsPath',
                      dataType: 'json',
                      success: function (resp) {
                          $.messager.progress('close');
                          if (resp.Status == 1) {
                              imgThumbnailsPath.src = resp.ExStr;

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

     });


     //格式化当前特殊情况时间
     function FormateCurrPageDate(d, h, m) {
         var result = new StringBuilder();
         result.AppendFormat('{0} {1}:{2}:00', d, h, m);
         return result.ToString();
     }
     function GetRandomHb() {
         var randInt = GetRandomNum(1, 7);
         imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
     }
     function ShowEdit(activityID) {
         $.ajax({
             type: 'post',
             url: handlerUrl,
             data: { Action: 'GetTheVoteInfo', Autoid: currID },
             dataType: "json",
             success: function (resp) {
                 try {
                     if (resp.Status == 0) {
                         var model = resp.ExObj;
                         $('#TxtVoteName').val(model.VoteName);
                         $("#txtOverDate").datetimebox("setValue", model.TheVoteOverDateStr);
                         $("#txtSummary").val(model.Summary);
                         $('#imgThumbnailsPath').attr('src', model.ThumbnailsPath);
                         if (model.IsVoteOpen == 1)
                             ckIsVoteOpen.checked = true;
                         else
                             ckIsVoteOpen.checked = false;

                         if (model.VoteSelect == 2) {
                             $(trMaxSelectItemCount).show();
                             $("#VoteSelectFlase").attr("checked", true);
                             $("#txtMaxSelectItemCount").val(model.MaxSelectItemCount);
                         }


                         if (model.diInfos.length > 0) {
                             $("#tdAnswer").html("");
                             var html = "";
                             var id = "";
                             for (var i = 0; i < model.diInfos.length; i++) {
                                 id += model.diInfos[i].AutoID + ",";
                                 html += '<div data-role="fieldcontain" style="width: 100%;">';
                                 html += '<input name="answer" placeholder="投票选项" class="anser_input_class" style="width: 70%; height: 20px;" type="text" value="' + model.diInfos[i].ValueStr + '"></div>';
                             }
                             $("#Aid").val(id);
                             $("#tdAnswer").html(html);
                         }
                     }
                     else {
                         Alert(resp.Msg);
                     }
                 } catch (e) {
                     Alert(e);
                 }
             }

         });


     }

    





//     function RcurrLine() {
//         alert($(this).html());
     //     }
     function ResetCurr() {
         ClearAll();
         editor.html('');
     }

     function Addanswer() {
         var html = '<div data-role="fieldcontain" style="width: 100%;">';
         html += '<input name="answer" placeholder="投票选项" class="anser_input_class" style="width: 70%;" type="text" value=""><span name="ResetCurr" >&nbsp;删除选项</span></div>';
         $("#tdAnswer").append(html);
     }

 </script>
</asp:Content>

