<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CompanyCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.CompanyCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style type="text/css">
         body{font-family:微软雅黑;}
        .tdTitle
        {
            font-weight: bold;
            font-size:14px;
        }

        
        input[type=text]
        {
        height:30px;    
        }
    </style>
    
   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="CompanyMgr.aspx">企业管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%if (webAction == "edit") { Response.Write("：" + model.CompanyName); } %></span>
    <a href="CompanyMgr.aspx" style="float:right;margin-right:20px;color:Black;" title="返回企业列表" class="easyui-linkbutton" iconcls="icon-back" plain="true" >
            返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        登录用户名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtUserId"  style="width: 100%;"  placeholder="用户名(必填)" value="<%=model.UserId%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        登录密码：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPassword" class="" style="width: 100%;"  placeholder="密码(必填)" value="<%=model.Password%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        启用/禁用：
                    </td>
                    <td width="*" align="left">
                       <input type="radio" name="rdostatus" value="0" id="rdostauts0" checked="checked"/><label for="rdostauts0">启用</label>
                       <input type="radio" name="rdostatus" value="1" id="rdostauts1" /><label for="rdostauts1">禁用</label>
                       提示:禁用后用户将不能登录
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
                        区/县：
                    </td>
                    <td width="*" align="left">
               <select id="ddlArea">
              
               <option value="黄浦区">黄浦区</option>
               <option value="长宁区">长宁区</option>
               <option value="徐汇区">徐汇区</option>
               <option value="静安区">静安区</option>
               <option value="杨浦区">杨浦区</option>
               <option value="虹口区">虹口区</option>
               <option value="闸北区">闸北区</option>
               <option value="普陀区">普陀区</option>
               <option value="浦东新区">浦东新区</option>
               <option value="宝山区">宝山区</option>
               <option value="闵行区">闵行区</option>
               <option value="金山区">金山区</option>
               <option value="嘉定区">嘉定区</option>
               <option value="青浦区">青浦区</option>
               <option value="松江区">松江区</option>
               <option value="奉贤区">奉贤区</option>
               <option value="崇明县">崇明县</option>
               </select>
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
                        企业地址：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAddress" class="" style="width: 100%;"  placeholder="企业地址" value="<%=model.Address%>"  />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        电话：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTel" class="" style="width: 100%;"  placeholder="电话" value="<%=model.Tel%>"  />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        手机：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPhone" class="" style="width: 100%;"  placeholder="手机" value="<%=model.Phone%>" />
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        QQ：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtQQ" class="" style="width: 100%;"  placeholder="QQ" value="<%=model.QQ%>" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        负责人：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtContacts" class="" style="width: 100%;"  placeholder="负责人" value="<%=model.Contacts%>"  />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        营业执照号码：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtBusinessLicenseNumber" class="" style="width: 100%;"  placeholder="营业执照号码" value="<%=model.BusinessLicenseNumber%>"  />
                    </td>
                </tr>
                 


                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            企业介绍：</label>
                    </td>
                    <td width="*" align="left">
                       
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            <%=model.Introduction%>
                            </div>
                       
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
         if (currAction == 'edit') {
             $("#ddlArea").val("<%=model.Area%>");
             var IsDisable = "<%=model.IsDisable%>";
             if (IsDisable == "0") {
                 rdostauts0.checked = true;
             }
             else {
                 rdostauts1.checked = true;
             }

         }
         $('#btnSave').click(function () {
             try {
                 var model =
                    {
                        AutoID: '<%=model==null?0:model.AutoID%>',
                        Action: currAction == 'add' ? 'AddCompanyInfo' : 'EditCompanyInfo',
                        CompanyName: $.trim($("#txtCompanyName").val()),
                        Thumbnails: $('#imgThumbnailsPath').attr('src'),
                        Address: $.trim($("#txtAddress").val()),
                        Area: $.trim($('#ddlArea').val()),
                        Tel: $.trim($('#txtTel').val()),
                        Phone: $.trim($('#txtPhone').val()),
                        QQ: $.trim($('#txtQQ').val()),
                        Contacts: $.trim($('#txtContacts').val()),
                        BusinessLicenseNumber: $.trim($('#txtBusinessLicenseNumber').val()),
                        UserId: $.trim($('#txtUserId').val()),
                        Password: $.trim($('#txtPassword').val()),
                        IsDisable: rdostauts1.checked ? 1 : 0,
                        Introduction: editor.html()
                    };
                 if (model.UserId == '') {
                     $('#txtUserId').focus();
                     return;
                 }
                 if (model.Password == '') {
                     $('#txtPassword').focus();
                     return;
                 }
                 if (model.CompanyName == '') {
                     $('#txtCompanyName').focus();
                     return;
                 }

                 if (model.Contacts == '') {
                     $('#txtContacts').focus();
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
         editor.html('');
     }

     //获取随机图片
     function GetRandomHb() {

         imgThumbnailsPath.src = "/img/hb/hb" + GetRandomNum(1, 7) + ".jpg";
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

    </script>
</asp:Content>
