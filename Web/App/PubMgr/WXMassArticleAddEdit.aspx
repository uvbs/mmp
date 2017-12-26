<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMassArticleAddEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.WXMassArticleAddEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 40px;
        }
        

        .title
        {
            
         font-size:12px;   
         }
         .return
         {
             
             float:right;
             margin-right:5px;
         }
        input[type=text],select
         {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
            width:80%;
             
        }

         
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">当前位置：&nbsp;公众号设置&nbsp;&nbsp;&gt;&nbsp;&nbsp;<a href="WXMassArticleMgr.aspx" title="返回素材管理" >素材管理</a>&gt;&nbsp;<span><%=HeadTitle%></span>
    <a href="WXMassArticleMgr.aspx" style="float: right; margin-right: 20px;" title="返回素材管理列表"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="<%=model.ThumbImage%>" width="200px" height="100px" id="imgThumbnailsPath"/>
                        <br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a>
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                         onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片
                        
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTitle" value=" <%=model.Title%>" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        描述：
                    </td>
                 <td width="*" align="left">
                 <input type="text"  id="txtDigest" value="<%=model.Digest%>" >
                
                     </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        作者：
                    </td>
                <td width="*" align="left">
                <input type="text" id="txtAuthor" value="<%=model.Author%>" style="width:200px;">
                
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
                             <%=model.Content%>
                            </div>
                        </div>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        原文链接：
                    </td>
                    <td width="*" align="left">
                    <input type="text"  id="txtContent_Source_Url" value="<%=model.Content_Source_Url%>" >
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                    <input id="hdSort" value="<%=model.Sort%>" type="hidden" />
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;"  id="btnSave" style="font-weight: bold;width:200px;text-decoration:underline;" class="button button-rounded button-primary">
                            保存</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = '<%=webAction %>';
        var editor;
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

            }

            $('#btnSave').click(function () {
                try {
                    var model =
                    {

                        AutoID: "<%=model.AutoID%>",
                        ThumbImage: $('#imgThumbnailsPath').attr('src'),
                        Author: $.trim($('#txtAuthor').val()),
                        Title: $.trim($("#txtTitle").val()),
                        Content_Source_Url: $.trim($("#txtContent_Source_Url").val()),
                        Content: editor.html(),
                        Digest: $.trim($('#txtDigest').val()),
                        Sort: $("#hdSort").val(),
                        Action: currAction == 'add' ? 'AddWXMassArticle' : 'EditWXMassArticle'

                    };
                    if (model.Title == '') {
                        $('#txtTitle').focus();
                        Alert('请输入标题！');
                        return;
                    }
                    if (model.Content == '') {

                        Alert('请输入内容');
                        return;
                    }
                    if (model.Content_Source_Url != '') {
                        if (!IsURL(model.Content_Source_Url)) {
                            Alert('链接不正确');
                            return;
                        }
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
                                Alert(resp.Msg);
                                window.location.href = "WXMassArticleMgr.aspx";
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
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFileLocal&fd=JuActivityImg&filegroup=file1',
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





        function ResetCurr() {
            ClearAll();
            editor.html('');
        }

        //获取随机图片
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
            $('#imgThumbnailsPath').attr('path', "/img/hb/hb" + randInt + ".jpg");
        }


        KindEditor.ready(function (K) {
            editor = K.create('#txtEditor', {
                uploadJson: '/kindeditor-4.1.10/asp.net/UploadImgWeixin.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
        });

    </script>
</asp:Content>