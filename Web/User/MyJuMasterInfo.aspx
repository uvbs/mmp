<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true" CodeBehind="MyJuMasterInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.MyJuMasterInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/JuActivity/JuActivityHandler.ashx";
        var editor;
        $(function () {

            var gender = "<%=model==null?"":model.Gender%>";
            
            if (gender == "男") {
                rdman.checked = true;
            }
            if (gender == "女") {
                rdwoman.checked = true;
            }

            $("#btnSave").click(function () {


                try {
                    var model =
                    {
                        MasterName: $("#txtMasterName").val(),
                        Gender: rdman.checked ? "男" : "女",
                        Company: $("#txtCompany").val(),
                        Title: $("#txtTitle").val(),
                        Summary: $("#txtSummary").val(),
                        IntroductionContent: editor.html(),
                        HeadImg: $("#imgHead").attr('src'),
                        Action: "EditMyJuMasterInfo"

                    };



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
                            if (resp.Status == 1) {


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
                uploadJson: '/Comm/Upload.ashx',
                width: "80%",
                height: "400px",
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template'],
                filterMode: false
            });
        });

    </script>
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="z-index: 10000; padding: 5px; height: auto;
        width: 100%; position: fixed; top: 0;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-save" plain="true"
                id="btnSave">保存资料</a>
        </div>
    </div>
    <div style="position: absolute; top: 50px; font-size: 12px; width: 100%">
        <fieldset>
            <legend>
                我的专家资料</legend>
           <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        姓名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtMasterName" value="<%=model==null?"":model.MasterName %>" class="easyui-validatebox" required="true" missingmessage="请输入您的姓名" style="width: 30%;" />
                           
                            
                    </td>
                </tr>
                           <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        性别：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" id="rdman" checked="checked" name="rdogender"/>
                                <label for='rdman'>
                                    男</label>
                                <input type="radio" id="rdwoman" name="rdogender" />
                                <label for='rdwoman'>
                                    女</label>
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        公司：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCompany"  style="width: 80%;" value="<%=model==null?"":model.Company %>" class="easyui-validatebox" required="true" missingmessage="请输入您的公司名称" />
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        职位：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTitle" value="<%=model==null?"":model.Title %>" style="width: 30%;" class="easyui-validatebox" required="true" missingmessage="请输入您的职位"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        头像：
                    </td>
                    <td width="*" align="left">
                        <img alt="头像" id="imgHead" src="<%=model==null?"":model.HeadImg %>" width="80px" height="100px"  /><br />
                         <a href="javascript:;" class="easyui-linkbutton"
                                iconcls="icon-add" plain="true" onclick="txtThumbnailsPath.click()">上传头像</a><br />
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*100。
                        <input type="file" id="txtThumbnailsPath" name="file1" style="display: none" />
                    </td>
                </tr>
                    <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        简介：
                    </td>
                    <td width="*" align="left">
                    <textarea id="txtSummary"  style="width: 60%;height:80px;" class="easyui-validatebox" required="true" missingmessage="请输入您的简介 80个字以内 "><%=model==null?"":model.Summary%></textarea>
                      
                    </td>
                </tr>
                              <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        详细:
                    </td>
                    <td width="*" align="left">
                    <textarea id="txtIntroductionContent"  style="width: 80%; height: 400px;">
                    <%=model==null?"":model.IntroductionContent %>
                    </textarea>
                        
                    </td>
                </tr>

       
               
            </table>
        </fieldset>
         
    </div>
</asp:Content>
