<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SlideAddEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.SlideAddEdit" %>
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
        #txtSort{width:50px;}

         
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">当前位置：&nbsp;&nbsp;&gt;&nbsp;&nbsp;<a href="SlideMgr.aspx" title="返回幻灯片管理" >幻灯片管理</a>&gt;&nbsp;<span><%=HeadTitle%></span>
    <a href="SlideMgr.aspx" style="float: right; margin-right: 20px;" title="返回幻灯片管理"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="<%=model.ImageUrl%>"  id="imgThumbnailsPath"/>
                        <br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机图片</a>
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                         onclick="txtThumbnailsPath.click()">上传图片</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        跳转链接：
                    </td>
                    <td width="*" align="left">
                    <input type="text"  id="txtLink" value="<%=model.Link%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        排序：
                    </td>
                    <td width="*" align="left">
                    <input type="text"  id="txtSort" value="<%=model.Sort%>"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
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
                        ImageUrl: $('#imgThumbnailsPath').attr('src'),
                        Link: $.trim($('#txtLink').val()),
                        Sort: $("#txtSort").val(),
                        Action: currAction == 'add' ? 'AddSlide' : 'EditSlide'

                    };
                    if (model.Sort == "") {
                        model.Sort = 0;
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
                                window.location.href = "SlideMgr.aspx";
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




    </script>
</asp:Content>