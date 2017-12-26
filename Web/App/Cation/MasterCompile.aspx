<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="MasterCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.MasterCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/ajaxfileupload2.1.js" type="text/javascript"></script>
    <link href="/kindeditor-4.1.10/themes/default/default.css" rel="stylesheet" type="text/css" />
   <script src="/kindeditor-4.1.10/kindeditor.js" type="text/javascript"></script>
   <script src="/kindeditor-4.1.10/lang/zh_CN.js" type="text/javascript"></script>
   <%--
   <script type="text/javascript">
       $(function () {
           var myMenu;
           myMenu = new SDMenu("my_menu");
           myMenu.init();
           var firstSubmenu = myMenu.submenus[2];
           myMenu.expandMenu(firstSubmenu);
       });



    </script>--%>
<script type="text/javascript">
        var handlerUrl ="/Handler/App/CationHandler.ashx";
        var editor;
        $(function () {

            var gender = '<%=model==null?"":model.Gender%>';
            
            if (gender == "男") {
                rdman.checked = true;
            }
            if (gender == "女") {
                rdwoman.checked = true;
            }

            $("#btnSave").click(function () {


                try {

                    var model=GetModel();
                    if (model.MasterName == '') {
                        $("#txtMasterName").focus();
                        return;
                    }

                    if (model.Company == '') {
                        $("#txtCompany").focus();
                        return;
                    }
                    if (model.Title == '') {
                        $("#txtTitle").focus();
                        return;
                    }
                    if (model.Summary == '') {
                        $("#txtSummary").focus();
                        return;
                    }
                   if (model.HeadImg == '') {
                        Alert("请先上传头像图片");
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
                            Alert(resp.Msg);
                            
                        }
                    });

                } catch (e) {
                    Alert(e);
                }


            });

            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片。。。' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuMasterHeadImg',
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

                                 $("#imgHead").attr('src', resp.ExStr);
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




        KindEditor.ready(function (K) {
            editor = K.create('#txtIntroductionContent', {
               uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                width: "100%",
                height: "300px",
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template'],
                filterMode: false
            });
        });

        function GetModel(){
        try {
    

                    var action="<%=webAction %>";
                    action=action=="edit" ? "EditJuMasterInfo" : "AddJuMasterInfo";
                    var model =
                    {

                        MasterID:<%=mid%>,
                        MasterName: $("#txtMasterName").val(),
                        Gender: rdman.checked ? "男" : "女",
                        Title: $("#txtTitle").val(),
                        Summary: $("#txtSummary").val(),
                        IntroductionContent: editor.html(),
                        HeadImg: $("#imgHead").attr('src'),
                        Action: action
                    }
                    return model;
                    } catch (e) {
                    alert(e);
    
}
        
        }

       

</script>
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
            text-align:left;
            width:8%;
        }
        .tdright
        {
            text-align:left;
             width:92%;
         }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
当前位置：&nbsp;<a href="javascript:;" onclick="window.location.href='MasterManage.aspx'">专家团管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %>专家<%if (model != null && webAction == "edit") { Response.Write("：" + model.MasterName); } %></span>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div id="toolbar" class="pageTopBtnBg" >
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-save" plain="true"
                id="btnSave">保存资料</a>
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-redo"
            plain="true" onclick="window.location.href='MasterManage.aspx'" style="float:right;">返回</a>
        </div>
    </div>

    <div style=" top: 50px; font-size: 12px; width: 100%">
        <fieldset>
            
           <table width="100%">
                <tr>
                    <td class="tdTitle">
                        姓名：
                    </td>
                    <td class="tdright" align="left">
                        <input type="text" id="txtMasterName" value="<%=model==null?"":model.MasterName %>" class="easyui-validatebox" required="true" missingmessage="请输入您的姓名" style="width: 100%;" />
                           
                            
                    </td>
                </tr>
                           <tr>
                    <td   class="tdTitle">
                        性别：
                    </td>
                    <td class="tdright" align="left">
                        <input type="radio" id="rdman" checked="checked" name="rdogender"/>
                                <label for='rdman'>
                                    男</label>
                                <input type="radio" id="rdwoman" name="rdogender" />
                                <label for='rdwoman'>
                                    女</label>
                    </td>
                </tr>
                  
                   <tr>
                    <td   class="tdTitle">
                        职位：
                    </td>
                    <td class="tdright" align="left">
                        <input type="text" id="txtTitle" value="<%=model==null?"":model.Title %>" style="width: 100%;" class="easyui-validatebox" required="true" missingmessage="请输入职位"/>
                    </td>
                </tr>
                <tr>
                    <td  class="tdTitle">
                        头像：
                    </td>
                    <td class="tdright" align="left">
                        <img alt="头像" id="imgHead" src="<%=model==null?"":model.HeadImg %>" width="80px" height="100px"  /><br />
                         <a href="javascript:;" class="easyui-linkbutton"
                                iconcls="icon-add" plain="true" onclick="txtThumbnailsPath.click()">上传头像</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*100。
                        <input type="file" id="txtThumbnailsPath" name="file1" style="display: none" />
                    </td>
                </tr>
                    <tr>
                    <td   class="tdTitle">
                        简介：
                    </td>
                    <td class="tdright" align="left">
                    <textarea id="txtSummary"  style="width: 100%;height:80px;" class="easyui-validatebox" required="true" missingmessage="请输入您的简介 80个字以内 "><%=model==null?"":model.Summary%></textarea>
                      
                    </td>
                </tr>
                              <tr>
                    <td   class="tdTitle">
                        详细:
                    </td>
                    <td class="tdright" align="left">
                    <textarea id="txtIntroductionContent"  style="width: 100%; height: 300px;">
                    <%=model==null?"":model.IntroductionContent %>
                    </textarea>
                        
                    </td>
                </tr>

       
               
            </table>
        </fieldset>
         
    </div>
</asp:Content>
