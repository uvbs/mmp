<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="GameCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.game.GameCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = '<%=webAction %>';
        var currId = '<%=model.AutoID %>';
        var editor;
        $(function () {

            KindEditor.ready(function (K) {
                editor = K.create('#txtGameCode', {
                    items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                    filterMode: false
                });
            });



            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }
            if (currAction == 'edit') {

                $('#txtGameName').val("<%=model.GameName %>");
                $('#imgThumbnailsPath').attr("src", "<%=model.GameImage %>");
                $('#txtGameDesc').val("<%=model.GameDesc %>");
                $('#txtGameSort').val("<%=model.GameSort %>");
                

            }

            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        Action: currAction == 'add' ? 'AddGameInfo' : 'EditGameInfo',
                        AutoID: currId,
                        GameName: $.trim($('#txtGameName').val()),
                        GameImage: $('#imgThumbnailsPath').attr('src'),
                        GameDesc: $.trim($('#txtGameDesc').val()),
                        GameCode: editor.html(),
                        GameSort:$.trim($('#txtGameSort').val()),
                        GameViewPort:$('#txtGameViewPort').val()
                    }

                    if (model.GameName == '') {
                        $('#txtGameName').focus();

                        return;
                    }

                    if (model.GameImage == '') {
                        Alert("请上传游戏图片");
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
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=game',
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

            $(":input[type=text]").val("");

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
       input[type=text],select,textarea
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
             
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="javascript:;" onclick="window.location.href='/Game/GameMgr.aspx'">游戏管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%if (model != null && webAction == "edit") { Response.Write("：" + model.GameName); } %></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <a href="/Game/GameMgr.aspx" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
            
        <br />
        <hr style="border: 1px dotted #036" />
        <div style="font-size: 12px; width: 100%">
            <table width="100%">

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        游戏名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtGameName" class="" style="width: 100%;" />
                    </td>
                </tr>

                  
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        游戏图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" width="100px" height="130px" id="imgThumbnailsPath" /><br />
                         <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传图片</a><br />
                        
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>

                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        游戏描述：
                    </td>
                    <td width="*" align="left">
                       
                        <input id="txtGameDesc" type="text" style="width:100%;"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        ViewPort：
                    </td>
                    <td width="*" align="left">
                       <textarea  id="txtGameViewPort" style="width:100%;"><%=model.GameViewPort %></textarea>
                       
                    </td>
                </tr>

                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        游戏排序：
                    </td>
                    <td width="*" align="left">
                       
                        <input id="txtGameSort" type="text" style="width:200px;" value="1" />从小到大排序
                    </td>
                </tr>

                    <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        游戏代码：
                    </td>
                    <td width="*" align="left">
                       <textarea id="txtGameCode" style="width:100%;height:400px;"><%=model.GameCode%></textarea>
                       
                       
                       
                        
                    </td>
                </tr>
               
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;" class="button button-rounded button-primary">
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
