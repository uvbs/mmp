<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="MasterAddEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.MasterAddEdit" %>
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

        input 
        {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
   
        }


    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;理财师详情
     <a href="MasterMgr.aspx" style="float:right;margin-right:20px;" class="easyui-linkbutton" iconcls="icon-back" plain="true">
            返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">


                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        理财师姓名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTutorName" class="" style="width: 100%;" value="<%=tInfo.TutorName%>" />
                    </td>
                </tr>
               <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        头像：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="<%=tInfo.TutorImg%>" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机头像</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传头像</a>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为300*300。
                        
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        用户名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtUserId" class="" style="width: 100%;" value="<%=tInfo.UserId%>" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        公司：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCompany" style="width:100%;"  value="<%=tInfo.Company%>"  />
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        职位：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPosition" class="" style="width: 200px;" value="<%=tInfo.Position%>" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        城市：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCity" class="" style="width: 200px;" value="<%=tInfo.City%>" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        第几届理财师：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtNumber" class="" style="width: 200px;" value="<%=tInfo.Number%>" />
                        提示: 0或空表示理财师不属于往届理财师. 比如填写2015表示是第2015届理财师
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        理财师第几名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtRank" class="" style="width: 200px;" value="<%=tInfo.Rank%>" />
                    </td>
                </tr>
               <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        性别：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlGender">
                        <option value="男">男</option>
                        <option value="女">女</option>
                        </select>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        年龄：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAge"  style="width:200px;" value="<%=tInfo.Age%>" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        银行：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtBank"  style="width:100%;" value="<%=tInfo.Bank%>" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        公司类型：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlCompanyType">
                        <option value="银行">银行</option>
                        <option value="第三方理财公司">第三方理财公司</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        工作年限：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtWorkYear"  style="width:200px;" value="<%=tInfo.WorkYear%>" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        理财师工作年限：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtMasterWorkYear"  style="width:200px;" value="<%=tInfo.MasterWorkYear%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        最高学历：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtEducation" style="width:200px;" value="<%=tInfo.Education%>" />
                    </td>
                </tr>
                    <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        Email：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtEmail" class="" style="width: 100%;" value="<%=tInfo.Email%>" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        区域：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlArea" style="display:none;">
                        <option value="华东">华东</option>
                        <option value="华北">华北</option>
                        <option value="华南">华南</option>
                        <option value="中西部">中西部</option>
                        </select>
                        <input type="text" id="txtArea" class="" style="width: 100%;" value="<%=tInfo.Area%>" />
                    </td>
                </tr>

                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        标签：
                    </td>
                    <td width="*" align="left">
                        <%=Tags%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        简要介绍：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtDigest" rows="3" style="width:100%;"><%=tInfo.Digest %></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        详细介绍：
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            <%=tInfo.TutorExplain %>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;width:400px;" class="button button-rounded button-primary">
                            保存</a> 
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
     var handlerUrl = "/Handler/App/WXWuBuHuiTutorHandler.ashx";
     var editor;
     var currID = '<%=AutoId %>';
     $(function () {

         if (currID != "") {
             //ShowEdit(currID);
         }

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
                 var ProfessionalStr = "";
                 $("input[name='Professional']:checked").each(function () {
                     ProfessionalStr += $(this).val() + ",";
                 });
                 var model =
                    {
                        Autoid: currID,
                        Action: "AddEditMasterInfo",
                        TutorName: $.trim($('#txtTutorName').val()),
                        TutorExplain: editor.html(),
                        TutorImg: $("#imgThumbnailsPath").attr("src"),
                        ProfessionalStr: ProfessionalStr,
                        UserId: $("#txtUserId").val(),
                        Company: $("#txtCompany").val(),
                        Position: $("#txtPosition").val(),
                        Digest: $("#txtDigest").val(),
                        City: $("#txtCity").val(),
                        Number: $("#txtNumber").val(),
                        Gender: $("#ddlGender").val(),
                        Age: $("#txtAge").val(),
                        Bank: $("#txtBank").val(),
                        CompanyType: $("#ddlCompanyType").val(),
                        WorkYear: $("#txtWorkYear").val(),
                        MasterWorkYear: $("#txtMasterWorkYear").val(),
                        Education: $("#ddlEducation").val(),
                        Email: $("#txtEmail").val(),
                        Area: $("#txtArea").val(),
                        Rank: $("#txtRank").val()

                    };
                 if (model.TutorName == "") {
                     Alert("请输入理财师姓名");
                     return false;
                 }
                 if (model.Number == "") {
                     model.Number = 0;
                 }
                 $.messager.progress({ text: '正在处理...' });
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: 'json',
                     success: function (resp) {
                         $.messager.progress('close');
                         if (resp.Status == 1) {
                             alert(resp.Msg);
                             window.location.href = "MasterMgr.aspx";
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

         $("#ddlGender").val("<%=tInfo.Gender%>");
         $("#ddlCompanyType").val("<%=tInfo.CompanyType%>");
         $("#ddlArea").val("<%=tInfo.Area%>");


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


     //     function ShowEdit(activityID) {
     //         $.ajax({
     //             type: 'post',
     //             url: handlerUrl,
     //             data: { Action: 'GetTutorInfo', Autoid: currID },
     //             dataType:"json",
     //             success: function (resp) {
     //                 try {
     //                     if (resp.Status == 0) {
     //                         var model = resp.ExObj;
     //                         $("#txtUserId").val(model.UserId);
     //                         $('#txtTutorName').val(model.TutorName);
     //                         editor.html(model.TutorExplain);
     //                         imgThumbnailsPath.src = model.TutorImg;
     //                         $("#txtCompany").val(model.Company);
     //                         $("#txtCity").val(model.City);
     //                         $("#txtPosition").val(model.Position);
     //                         $("#txtDigest").val(model.Digest);

     //                     }
     //                     else {
     //                         alert(resp.Msg);
     //                     }
     //                 } catch (e) {
     //                     alert(e);
     //                 }
     //             }

     //         });


     //     }

     //获取随机海报
     function GetRandomHb() {
         var randInt = GetRandomNum(1, 7);
         imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
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
