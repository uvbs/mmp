<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ADTheVoteInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.TheVote.ADTheVoteInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/kindeditor-4.1.10/themes/default/default.css" rel="stylesheet" type="text/css" />
    <script src="/kindeditor-4.1.10/kindeditor.js" type="text/javascript"></script>
    <script src="/kindeditor-4.1.10/lang/zh_CN.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXTheVoteInfoHandler.ashx";
        var currAction = '<%=currAction %>';
        var editor;
        var currID = '<%=AutoId %>';
        $(function () {
            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }

            if (currAction == 'add') {
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
                        VoteImg: $('#imgThumbnailsPath').attr('path'),
                        VoteContent: editor.html(),
                        IsVoteOpen: ckIsVoteOpen.checked ? 1 : 0,
                        VotePosition: $("input[name=rdVotePosition]:checked").attr("v"),
                        VoteSelect: $("input[name=rdVoteSelect]:checked").attr("v"),
                        Answer: answers,
                        aid: $("#Aid").val()
                    };

                    if (model.VoteName == '') {
                        $('#VoteName').focus();
                        Alert('请输入名称！');
                        return;
                    }


                    $.messager.progress({ text: '正在处理。。。' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        success: function (result) {
                            $.messager.progress('close');
                            var resp = $.parseJSON(result);
                            if (resp.Status == 1) {
                                if (currAction == 'add')
                                    ResetCurr();
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

            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片。。。' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'text',
                         success: function (result) {
                             $.messager.progress('close');
                             try {
                                 result = result.substring(result.indexOf("{"), result.indexOf("</"));
                             } catch (e) {
                                 alert(e);
                             }
                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {
                                 imgThumbnailsPath.src = resp.ExStr;
                                 $('#imgThumbnailsPath').attr('path', resp.ExStr);
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


        //格式化当前特殊情况时间
        function FormateCurrPageDate(d, h, m) {
            var result = new StringBuilder();
            result.AppendFormat('{0} {1}:{2}:00', d, h, m);
            return result.ToString();
        }

        function ShowEdit(activityID) {
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: 'GetTheVoteInfo', Autoid: currID },
                success: function (result) {
                    try {
                        var resp = $.parseJSON(result);
                        if (resp.Status == 0) {
                            var model = resp.ExObj;
                            $('#TxtVoteName').val(model.VoteName);
                            editor.html(model.VoteContent);
                            imgThumbnailsPath.src = model.VoteImg;
                            if (model.IsVoteOpen == 1)
                                ckIsVoteOpen.checked = true;
                            else
                                rdoIsNotHide.checked = false;

                            if (model.VotePosition == 1)
                                VotePosition1.checked = true;
                            else if (model.VotePosition == 2)
                                VotePosition2.checked = true;
                            else
                                VotePosition3.checked = true;

                            if (model.VoteSelect == 1)
                                VoteSelectTrue.checked = true;
                            else if (model.VoteSelect == 2)
                                VoteSelectTrue.checked = true;

                            if (model.diInfos.length > 0) {
                                $("#tdAnswer").html("");
                                var html = "";
                                var id = "";
                                for (var i = 0; i < model.diInfos.length; i++) {
                                    id += model.diInfos[i].AutoID + ",";
                                    html += '<div data-role="fieldcontain" style="width: 100%;">';
                                    html += '<input name="answer" placeholder="追加" class="anser_input_class" style="width: 70%; height: 20px;" type="text" value="' + model.diInfos[i].ValueStr + '"></div>';
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


        function ResetCurr() {
            ClearAll();
            editor.html('');
        }

        //获取随机海报
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
            $('#imgThumbnailsPath').attr('path', "/img/hb/hb" + randInt + ".jpg");
        }




        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
        });



        function Addanswer() {
            var html = '<div data-role="fieldcontain" style="width: 100%;">';
            html += '<input name="answer" placeholder="追加" class="anser_input_class" style="width: 70%; height: 20px;" type="text" value=""></div>';
            $("#tdAnswer").append(html);
        }

    </script>
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
             
        }
        table td
        {
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;升级投票 >
    <%=Tag%>投票
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <a href="TheVoteInfoMgr.aspx" class="easyui-linkbutton" iconcls="icon-redo" plain="true">
            返回</a>
        <br />
        <hr style="border: 1px dotted #036" />
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="TxtVoteName" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a><br />
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            内容：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        是否公开：
                    </td>
                    <td width="*" align="left">
                        <input type="checkbox" name="IsVoteOpen" id="ckIsVoteOpen" v="1" /><label for="ckIsVoteOpen">是否公开</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" >
                        投票设置：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdVoteSelect" id="VoteSelectTrue" checked="checked" v="1" /><label
                            for="VoteSelectTrue">单选</label>
                        <input type="radio" name="rdVoteSelect" id="VoteSelectFlase" v="2" /><label for="VoteSelectFlase">多选</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        投票位置：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdVotePosition" id="VotePosition1"  v="1" /><label
                            for="VotePosition1">页首
                        </label>
                        <input type="radio" name="rdVotePosition" id="VotePosition2" v="2" /><label for="VotePosition2">中间</label>
                        <input type="radio" name="rdVotePosition" id="VotePosition3" v="3" checked="checked"/><label for="VotePosition3">页尾</label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td width="*" align="left" colspan="2" id="tdAnswer">
                        <div data-role="fieldcontain" style="width: 100%;">
                            <input name="answer" placeholder="选项1" class="anser_input_class" style="width: 70%;
                                height: 20px;" type="text" value="">
                        </div>
                        <div data-role="fieldcontain" style="width: 100%;">
                            <input name="answer" placeholder="选项2" class="anser_input_class" type="text" style="width: 70%;
                                height: 20px;" value="">
                        </div>
                        <div data-role="fieldcontain" style="width: 100%;">
                            <input name="answer" placeholder="选项3" class="anser_input_class" style="width: 70%;
                                height: 20px;" type="text" value="">
                        </div>
                    </td>
                    <td>
                        <input type="hidden" id="Aid" value="0" />
                        <a href="javascript:void(0)" onclick="Addanswer()">添加投票选项</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;" class="button button-rounded button-flat-primary">
                            保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold;" class="button button-rounded button-flat">
                                重置</a>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
