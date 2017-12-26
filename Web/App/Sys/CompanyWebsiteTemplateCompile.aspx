<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CompanyWebsiteTemplateCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Sys.CompanyWebsiteTemplateCompile" %>
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
    当前位置：&nbsp;<a href="javascript:;" onclick="window.location.href='/App/Cation/ArticleManage.aspx'">模板管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %>幻灯片<%if (model != null && webAction == "edit") { Response.Write("：" + model.TemplateName); } %></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <a href="CompanyWebsiteTemplateManage.aspx" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
            
        <br />
        <hr style="border: 1px dotted #036" />
        <div style="font-size: 12px; width: 100%">
            <table width="100%">

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        模板名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTemplateName" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        模板目录名称：
                    </td>
                    <td width="*" align="left">
                       
                        <input id="txtTemplatePath" type="text" style="width:100%;"/>
                    </td>
                </tr>
                  
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        缩略图：
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
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currAction = '<%=webAction %>';
     var currId = '<%=model.AutoID %>';
     $(function () {


         if ($.browser.msie) { //ie 下
             //缩略图
             $("#auploadThumbnails").hide();
             $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
         }
         else {
             $("#txtThumbnailsPath").hide(); //缩略图
         }
         if (currAction == 'edit') {

             $('#txtTemplateName').val("<%=model.TemplateName %>");
             $('#txtTemplatePath').val("<%=model.TemplatePath %>");
             $('#imgThumbnailsPath').attr("src", "<%=model.TemplateThumbnail %>");


         }

         $('#btnSave').click(function () {
             try {
                 var model =
                    {
                        Action: currAction == 'add' ? 'AddCompanyWebsiteTemplate' : 'EditCompanyWebsiteTemplate',
                        AutoID: currId,
                        TemplateName: $.trim($('#txtTemplateName').val()),
                        TemplatePath: $.trim($('#txtTemplatePath').val()),
                        TemplateThumbnail: $('#imgThumbnailsPath').attr('src')


                    }

                 if (model.TemplateName == '') {
                     $('#txtTemplateName').focus();

                     return;
                 }
                 if (model.TemplatePath == '') {
                     $('#txtTemplatePath').focus();

                     return;
                 }
                 if (model.TemplateThumbnail == '') {
                     Alert("请上传缩略图");
                     return;
                 }

                 $.messager.progress({ text: '正在处理。。。' });
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: "json",
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
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=CompanyWebsite',
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
</asp:Content>
