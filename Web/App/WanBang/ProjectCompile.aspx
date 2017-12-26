<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ProjectCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.ProjectCompile" %>
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
    当前位置：&nbsp;<a href="ProjectMgr.aspx">项目管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%if (webAction == "edit") { Response.Write("：" + model.ProjectName); } %></span>
    <a href="ProjectMgr.aspx" style="float:right;margin-right:20px;color:Black;" title="返回项目列表" class="easyui-linkbutton" iconcls="icon-back" plain="true" >
            返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        项目状态：
                    </td>
                    <td width="*" align="left">
                       <select id="ddlStatus">
                        <option value="0">审核中</option>
                        <option value="1">征集中</option>
                        <option value="2">已结束</option>
                        </select>
                        提示:只有状态为 征集中 的项目才会在手机端显示
                    </td>
                </tr>
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
                        项目所属用户名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtUserId"  style="width: 100%;"  placeholder="用户名(必填)" value="<%=model.UserId%>" />
                    </td>
                </tr>

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        分类：
                    </td>
                    <td width="*" align="left">
                    <select id="ddlCategory">
                    <option value="粘贴折叠">粘贴折叠</option>
                    <option value="成品包装">成品包装</option>
                    <option value="组件装配">组件装配</option>
                    <option value="加工制作">加工制作</option>
                    <option value="纺织串接">纺织串接</option>
                    <option value="缝纫整熨">缝纫整熨</option>
                    <option value="其它项目">其它项目</option>
                    <option value="阳光办公室">阳光办公室</option>
                    </select>
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
                        项目物流：
                    </td>
              <td width="*" align="left">
              <select id="ddlLogistics">
               <option value="0">基地负责配送</option>
               <option value="1">企业负责配送</option>
               </select>
               </td>
                </tr>
              <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        项目周期：
                    </td>
              <td width="*" align="left">
              <select id="ddlProjectCycle">
               <option value="0">临时(1个月以内)</option>
               <option value="1">短期(1-3个月)</option>
               <option value="2">中期(3-6个月)</option>
               <option value="3">长期(6-12个月)</option>
               
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
                        周期要求：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTimeRequirement" class="" style="width: 100%;"  placeholder="周期要求" value="<%=model.TimeRequirement%>"  />
                    </td>
                </tr>

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            项目介绍：</label>
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
                $("#ddlCategory").val("<%=model.Category%>");
                $("#ddlLogistics").val("<%=model.Logistics%>");
                $("#ddlProjectCycle").val("<%=model.ProjectCycle%>");
                $("#ddlStatus").val("<%=model.Status%>");

            }
            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        AutoID: '<%=model==null?0:model.AutoID%>',
                        Action: currAction == 'add' ? 'AddProjectInfo' : 'EditProjectInfo',
                        UserId: $.trim($('#txtUserId').val()),
                        ProjectName: $.trim($("#txtProjectName").val()),
                        Thumbnails: $('#imgThumbnailsPath').attr('src'),
                        Area: $.trim($("#ddlArea").val()),
                        Category: $.trim($('#ddlCategory').val()),
                        Logistics: $.trim($('#ddlLogistics').val()),
                        ProjectCycle: $.trim($('#ddlProjectCycle').val()),
                        Status: $.trim($('#ddlStatus').val()),
                        TimeRequirement: $.trim($('#txtTimeRequirement').val()),
                        Introduction: editor.html()
                    };
                    if (model.ProjectName == '') {
                        $('#txtProjectName').focus();
                        return;
                    }
                    if (model.UserId == '') {
                        $('#txtUserId').focus();
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

