<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="JointProjectCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.JointProjectCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style type="text/css">
         body{font-family:微软雅黑;}
        .tdTitle
        {
            font-weight: bold;
            font-size:14px;
        }

        
        input[type=text],select
        {
        height:30px;    
        }
    </style>
    

   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="ProjectMgr.aspx">对接项目管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%if (webAction == "edit") { Response.Write("：" + model.ProjectName); } %></span>
    <a href="JointProjectMgr.aspx" style="float:right;margin-right:20px;color:Black;" title="返回对接项目列表" class="easyui-linkbutton" iconcls="icon-back" plain="true" >
            返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">

                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        项目名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtProjectName"  style="width: 100%;"  placeholder="项目名称(必填)" value="<%=model.ProjectName%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        企业名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCompanyName"  style="width: 100%;"  placeholder="企业名称(必填)" value="<%=model.CompanyName%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        基地名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtBaseName"  style="width: 100%;"  placeholder="基地名称(必填)" value="<%=model.BaseName%>" />
                    </td>
                </tr>
             
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图"  src="<%=string.IsNullOrEmpty(model.Thumbnails)?"/img/hb/hb1.jpg":model.Thumbnails%>" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                      
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
            
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/WanBang/PC.ashx";
     var currAction = '<%=webAction%>';
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

         $('#btnSave').click(function () {
             try {
                 var model =
                    {
                        AutoID: '<%=model==null?0:model.AutoID%>',
                        Action: currAction == 'add' ? 'AddJointProject' : 'EditJointProject',
                        ProjectName: $.trim($("#txtProjectName").val()),
                        BaseName: $.trim($("#txtBaseName").val()),
                        CompanyName: $.trim($("#txtCompanyName").val()),
                        Thumbnails: $('#imgThumbnailsPath').attr('src')

                    };
                 if (model.ProjectName == '') {
                     $('#txtProjectName').focus();
                     return;
                 }
                 if (model.BaseName == '') {
                     $('#txtBaseName').focus();
                     return;
                 }
                 if (model.CompanyName == '') {
                     $('#txtCompanyName').focus();
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
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

     });







     function ResetCurr() {
         $("input[type='text']").val("");

     }

     //获取随机图片
     function GetRandomHb() {
         imgThumbnailsPath.src = "/img/hb/hb" + GetRandomNum(1, 7) + ".jpg";
     }


    </script>
</asp:Content>
