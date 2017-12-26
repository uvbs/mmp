<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PartnerEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Partner.PartnerEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivityName" class="commonTxt" placeholder="标题(必填)"  value="<%=model.ActivityName %>"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSummary" class="commonTxt"  value="<%=model.Summary %>"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="<%=model.ThumbnailsPath %>" width="80px" height="80px" id="imgThumbnailsPath" class="rounded" /><br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为750*750。
                        <input type="file" id="txtThumbnailsPath" name="file1" class="hidden"/>
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
                                <%=model.ActivityDescription%>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="IsHide" id="rdoIsNotHide" checked="checked" value="0" /><label for="rdoIsNotHide">显示</label>
                        <input type="radio" name="IsHide" id="rdoIsHide" value="1" /><label for="rdoIsHide">隐藏</label>
                    </td>
                </tr>
                 <tr>
                    <td  style="width: 100px;" align="right" class="tdTitle">
                        阅读数:
                    </td>
                    <td>
                    <input type="text" id="txtPv" value="<%=model.PV %>" class="commonTxt" placeholder="阅读数" style="width:100px;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle"></td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold; width: 200px;" class="button button-rounded button-flat">重置</a>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/static-modules/lib/tagsinput/jquery.tagsinput.js" type="text/javascript"></script>
    <script src="/static-modules/lib/chosen/chosen.jquery.min.js" type="text/javascript"></script>
    <script src="/static-modules/lib/layer/layer.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "Handler/PartnerHandler.ashx",
            currAcvityID = '<%=model.JuActivityID %>',
            editor,
            type='<%= Request["type"]%>';
            $document = $(document);

        init();

        function init() {
            if(currAcvityID != 0){
                $('#btnReset').hide();
            }
            ShowEdit();
            bindEvent();
            
        }
        function bindEvent() {
            $document.on('click', '#btnSave', function () {
                saveData();
            });
            $document.on('click', '#btnReset', function () { ResetCurr(); });
            $document.on('change', '#txtThumbnailsPath', function () {
                try {
                    $.messager.progress({
                        text: '正在上传图片...'
                    });

                    $.ajaxFileUpload({
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

                            } else {
                                alert(resp.Msg);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }
            });

        }

        function saveData() {
            var $btnSave = $('#btnSave'), $btnReset = $('#btnReset');
            if ($btnSave.hasClass('disabled ')) {
                return;
            }

            $btnSave.addClass('disabled').text('正在处理...');
            $btnReset.addClass('disabled');

            var model = {
                Action:'EditPartner',
                ActivityName: $.trim($('#txtActivityName').val()),
                JuActivityID: <% =model.JuActivityID %>,
                ArticleType:type,
                IsHide: rdoIsHide.checked ? 1 : 0,
                ActivityDescription: editor.html(),
                Summary: $("#txtSummary").val(),
                PV: $("#txtPv").val(),
                ThumbnailsPath: $("#imgThumbnailsPath").attr("src")
            };
            setTimeout(function () {
                if (model.ActivityName == '') {
                    $('#txtActivityName').focus();
                    alert('标题不能为空', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                if(model.ThumbnailsPath == ''){
                    alert('请上传缩略图', 3);
                    $btnSave.removeClass('disabled').text('保存');
                    $btnReset.removeClass('disabled');
                    return;
                }
                //$.messager.progress({
                //    text: '正在处理...'
                //});
                //layer.load('正在处理...');
                //var loadi = layer.load(5, 0);
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        $btnReset.removeClass('disabled');
                        $btnSave.removeClass('disabled').text('保存');
                        //layer.closeLoad();
                        //$.messager.progress('close');
                        if (resp.Status == 1) {
                            if (model.JuActivityID == '0')
                            { 
                                ResetCurr();
                            }
                            alert(resp.Msg);
                        } else {
                            alert(resp.Msg);
                        }
                    }
                });

            }, 400);

        }
        
        function ResetCurr() {
            ClearAll();
            $("#txtPv").val(0);
            editor.html('');
        }

        function ShowEdit() {
            if ('<%= model.IsHide %>' == '1') {
                rdoIsHide.checked = true;
            } else {
                rdoIsNotHide.checked = true;
            }
        }

        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'
                ],
                filterMode: false
            });
        });

    </script>
</asp:Content>
