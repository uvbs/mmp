<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ClassDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.totema.ClassDetail" %>

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
    当前位置：&nbsp;<a href="ClassMgr.aspx">班级管理</a> <a href="ClassMgr.aspx" style="float: right;
        margin-right: 20px; color: Black;" title="返回列表" class="easyui-linkbutton" iconcls="icon-back"
        plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        班级图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="<%=model.VoteObjectHeadImage%>" width="100px" height="130px" id="imgThumbnailsPath" /><br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtThumbnailsPath.click()">上传班级照片</a><br />
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        班级口号：
                    </td>
                    <td width="*" align="left">
                        <input id="txtIntroduction" type="text" style="width: 100%;" value="<%=model.Introduction%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        班级名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtVoteObjectName" class="" style="width: 100%;" value="<%=model.VoteObjectName%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        学校全称：
                    </td>
                    <td width="*" align="left">
                        <input id="txtSchoolName" type="text" style="width: 100%;" value="<%=model.SchoolName%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        所在区域：
                    </td>
                    <td width="*" align="left">
                        <input id="txtArea" type="text" style="width: 100%;" value="<%=model.Area%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        学校地址：
                    </td>
                    <td width="*" align="left">
                        <input id="txtAddress" type="text" style="width: 100%;" value="<%=model.Address%>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        联系人：
                    </td>
                    <td width="*" align="left">
                        <input id="txtContact" type="text" style="width: 200px;" value="<%=model.Contact %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        联系电话：
                    </td>
                    <td width="*" align="left">
                        <input id="txtPhone" type="text" style="width: 200px;" value="<%=model.Phone %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        审核状态：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlStatus">
                            <option value="0">等待审核</option>
                            <option value="1">审核通过</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-flat-primary">
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
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currId = '<%=model.AutoID %>';
     var voteId = '14';
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
                        Action: 'EditVoteObjectInfo',
                        AutoID: currId,
                        VoteID: voteId,
                        VoteObjectName: $.trim($('#txtVoteObjectName').val()),
                        VoteObjectHeadImage: $('#imgThumbnailsPath').attr('src'),
                        Introduction: $.trim($('#txtIntroduction').val()),
                        SchoolName: $("#txtSchoolName").val(),
                        Area: $.trim($('#txtArea').val()),
                        Address: $.trim($('#txtAddress').val()),
                        Contact: $.trim($('#txtContact').val()),
                        Phone: $.trim($('#txtPhone').val()),
                        Status: $.trim($('#ddlStatus').val())


                    }

                 if (model.VoteObjectName == '') {
                     $('#txtVoteObjectName').focus();
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
                             Alert("编辑成功");

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
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
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
         $("#ddlStatus").val("<%=model.Status%>");

     });

    </script>
</asp:Content>
