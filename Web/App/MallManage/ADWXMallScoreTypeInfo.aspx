<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ADWXMallScoreTypeInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.ADWXMallScoreTypeInfo" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置:&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>添加积分分类</span>
    <a href="WXMallScoreTypeInfo.aspx" style="float:right;margin-right:20px;" class="easyui-linkbutton" iconcls="icon-back"
            plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        
        
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        分类名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="TxtTypeName" class="" style="width: 100%;" />
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
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;width:200px;" class="button button-rounded button-primary">
                            保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold;width:200px;" class="button button-rounded button-flat">
                                重置</a>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
      var handlerUrl = "/Handler/App/CationHandler.ashx";
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
                        Action: "ADScoreTypeInfo",
                        TName: $.trim($('#TxtTypeName').val()),
                        TypeImg: $('#imgThumbnailsPath').attr('path'),
                    };

                    if (model.TName == '') {
                        $('#VoteName').focus();
                        Alert('请输入名称！');
                        return;
                    }


                    $.messager.progress({ text: '正在处理。。。' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType:"json",
                        success: function (resp) {
                            $.messager.progress('close');
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
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
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
                data: { Action: 'GetScoreTypeInfo', Autoid: currID },
                dataType:"json",
                success: function (resp) {
                        if (resp.Status == 0) {
                            var model = resp.ExObj;
                            $('#TxtTypeName').val(model.TypeName);
                            imgThumbnailsPath.src = model.TypeImg;
                        }
                        else {
                            Alert(resp.Msg);
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
</asp:Content>
