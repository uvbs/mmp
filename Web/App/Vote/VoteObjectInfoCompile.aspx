<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="VoteObjectInfoCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Vote.VoteObjectInfoCompile" %>

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
        input[type=text], select, textarea
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
        img {max-width:400px;}
        #txtVoteObjectName,#txtPhone{font-size:18px;font-weight:bold;}
        input[type=file] {
        display:none;
        
        
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="/App/Vote/VoteObjectInfoMgr.aspx?vid=<%=Request["vid"]%>">参与者管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %><%if (model != null && webAction == "edit") { Response.Write("：" + model.VoteObjectName); } %></span>
    
    <a href="VoteObjectInfoMgr.aspx?vid=<%=Request["vid"]%>" style="float: right; margin-right: 20px;
        color: Black;" title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">
        返回</a>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <%--                    <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        编号：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtNumber" class="" style="width: 100%;" />
                    </td>
                </tr>--%>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        姓名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtVoteObjectName" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        地区：
                    </td>
                    <td width="*" align="left">
                        <input id="txtArea" type="text" style="width: 100%;" />
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        学生名：
                    </td>
                    <td width="*" align="left">
                        <input id="txtContact" type="text" style="width: 100%;" value="<%=model.Contact %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        指导老师：
                    </td>
                    <td width="*" align="left">
                        <input id="txtEx2" type="text" style="width: 100%;" value="<%=model.Ex2 %>" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        Email：
                    </td>
                    <td width="*" align="left">
                        <input id="txtEx3" type="text" style="width: 100%;" value="<%=model.Ex3 %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        手机号码：
                    </td>
                    <td width="*" align="left">
                        <input id="txtPhone" type="text" style="width: 100%;" value="<%=model.Phone %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        身高：
                    </td>
                    <td width="*" align="left">
                        <input id="txtHeight" type="text" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        年龄：
                    </td>
                    <td width="*" align="left">
                        <input id="txtAge" type="text" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        学校：
                    </td>
                    <td width="*" align="left">
                        <input id="txtSchoolName" type="text" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        星座：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlConstellation">
                            <option value=""></option>
                            <option value="水瓶座">水瓶座</option>
                            <option value="白羊座">白羊座</option>
                            <option value="金牛座">金牛座</option>
                            <option value="双子座">双子座</option>
                            <option value="巨蟹座">巨蟹座</option>
                            <option value="狮子座">狮子座</option>
                            <option value="处女座">处女座</option>
                            <option value="天秤座">天秤座</option>
                            <option value="天蝎座">天蝎座</option>
                            <option value="射手座">射手座</option>
                            <option value="魔羯座">魔羯座</option>
                            <option value="双鱼座">双鱼座</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        头像：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" id="imgThumbnailsPath" /><br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtThumbnailsPath.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                        <a class="easyui-linkbutton" onclick="TransformImage('imgThumbnailsPath')">旋转图片</a>
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        平面结构图：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" id="imgEx1" /><br />
                        <a  href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtEx1.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtEx1" name="fileex1" />
                        <a class="easyui-linkbutton" onclick="TransformImage('txtEx1')">旋转图片</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        爱好：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtHobbies" style="width: 100%; height: 100px;"><%=model.Hobbies%></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        参赛宣言：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtIntroduction" style="width: 100%; height: 100px;"><%=model.Introduction%></textarea>
                    </td>
                </tr>
                <tr id="trIntroductionDetail">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        详情(视频代码)：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtIntroductionDetail" style="width: 100%; height: 100px;"><%=model.IntroductionDetail%></textarea>
                    </td>
                </tr>
                <tr id="trimgShow1">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片1：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg"  id="imgShow1" /><br />
                        <a id="a1" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="txtThumbnailsPath1.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtThumbnailsPath1" name="file2" style="display: none;" />
                        <a class="easyui-linkbutton" onclick="TransformImage('imgShow1')">旋转图片</a>
                    </td>
                </tr>
                <tr id="trimgShow2">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片2：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg"  id="imgShow2" /><br />
                        <a id="a2" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="txtThumbnailsPath2.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtThumbnailsPath2" name="file3" style="display: none;" />
                        <a class="easyui-linkbutton" onclick="TransformImage('imgShow2')">旋转图片</a>
                    </td>
                </tr>
                <tr id="trimgShow3">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片3：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg"  id="imgShow3" /><br />
                        <a id="a3" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="txtThumbnailsPath3.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtThumbnailsPath3" name="file4" style="display: none;" />
                        <a class="easyui-linkbutton" onclick="TransformImage('imgShow3')">旋转图片</a>
                    </td>
                </tr>
                <tr id="trimgShow4">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片4：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg"  id="imgShow4" /><br />
                        <a id="a4" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="txtThumbnailsPath4.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtThumbnailsPath4" name="file5" style="display: none;" />
                        <a class="easyui-linkbutton" onclick="TransformImage('imgShow4')">旋转图片</a>
                    </td>
                </tr>
                <tr id="trimgShow5">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示图片5：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" id="imgShow5" /><br />
                        <a id="a5" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            onclick="txtThumbnailsPath5.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtThumbnailsPath5" name="file6" style="display: none;" />
                        <a class="easyui-linkbutton" onclick="TransformImage('imgShow5')">旋转图片</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        底部内容：
                    </td>
                    <td width="*" align="left">
                        <textarea id="txtBottomContent" style="width: 100%; height: 100px;"><%=model.BottomContent%></textarea>
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        课题1
                       
                    </td>
                    <td width="*" align="left">
                       <%=ObjUserInfo.Ex5 %>
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        
                        课题1 视频地址：
                    </td>
                    <td width="*" align="left">
                         <input id="txtEx4" type="text"  style="width: 100%;" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        课题2:
                       
                    </td>
                    <td width="*" align="left">
                       <%=ObjUserInfo.Ex6 %>
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        
                        课题2 视频地址：
                    </td>
                    <td width="*" align="left">
                         <input id="txtEx6"  type="text" style="width: 100%;" />
                    </td>
                </tr>
                 <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">其他资料链接：</td>
                     <td width="*" align="left">
                         <input type="text" style="width:100%" value="<%=model.OtherInfoLink %>" id="txtOtherInfoLink"/>
                     </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        审核状态:
                    </td>
                    <td width="*" align="left">
                        <select id="ddlStatus">
                            <option value="0">等待审核</option>
                            <option value="1">审核通过</option>
                            <option value="2">审核不通过</option>
                        </select>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        审核结果:
                    </td>
                    <td width="*" align="left">
                    <select id="ddlRemark" style="display:none;" >
                     <option value="">请选择</option>
                    <option value="上传照片一定要是微信朋友圈点赞内容的截图，一定要是朋友圈截图">上传照片一定要是微信朋友圈点赞内容的截图，一定要是朋友圈截图</option>
                    <option value="请换一张朋友圈内容截图">请换一张朋友圈内容截图</option>
                     <option value="截图内容不能重复">截图内容不能重复</option>
                    </select>
                        <input id="txtRemark" type="text" style="width: 100%;" value="<%=model.Remark %>"/>
                    </td>
                </tr>
               <%-- <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">
                            保存</a> <a href="javascript:history.go(-1);" id="btnReturn" style="font-weight: bold; width: 200px;"
                                class="button button-rounded button-flat">返回</a>
                    </td>
                </tr>--%>
            </table>
             
            <br />
            <br />
        </div>
        <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 52px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top:10px;">
                <a href="javascript:;" id="btnSave" class="button button-rounded button-primary" style="width: 200px;">保存</a>
             <a href="javascript:history.go(-1);" id="btnReturn" style="font-weight: bold; width: 200px;"
                                class="button button-rounded button-flat">返回</a>
            </div>

       
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = '<%=webAction %>';
        var currId = '<%=model.AutoID %>';
        var voteId = '<%=Request["vid"]%>';
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

                //$('#txtNumber').val("<%=model.Number %>");
                $('#txtVoteObjectName').val("<%=model.VoteObjectName %>");
                $('#imgThumbnailsPath').attr("src", "<%=model.VoteObjectHeadImage %>");
                $('#txtArea').val("<%=model.Area %>");
                $('#txtHeight').val("<%=model.Height %>");
                $('#ddlConstellation').val("<%=model.Constellation %>");
                $('#imgShow1').attr("src", "<%=model.ShowImage1 %>");
                $('#imgShow2').attr("src", "<%=model.ShowImage2 %>");
                $('#imgShow3').attr("src", "<%=model.ShowImage3 %>");
                $('#imgShow4').attr("src", "<%=model.ShowImage4 %>");
                $('#imgShow5').attr("src", "<%=model.ShowImage5 %>");
                $("#txtAge").val("<%=model.Age %>");
                $("#txtSchoolName").val("<%=model.SchoolName %>");
                $("#ddlStatus").val("<%=model.Status%>");
                $("#txtEx4").val("<%=model.Ex4 %>");
                $("#txtEx6").val("<%=model.Ex6 %>");
                $('#imgEx1').attr("src", "<%=model.Ex1 %>");

            }

            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        Action: currAction == 'add' ? 'AddVoteObjectInfo' : 'EditVoteObjectInfo',
                        AutoID: currId,
                        VoteID: voteId,
                        VoteObjectName: $.trim($('#txtVoteObjectName').val()),
                        VoteObjectHeadImage: $('#imgThumbnailsPath').attr('src'),
                        Area: $.trim($('#txtArea').val()),
                        Height: $.trim($('#txtHeight').val()),
                        Constellation: $.trim($('#ddlConstellation').val()),
                        Hobbies: $.trim($('#txtHobbies').val()),
                        Introduction: $.trim($('#txtIntroduction').val()),
                        ShowImage1: $('#imgShow1').attr('src'),
                        ShowImage2: $('#imgShow2').attr('src'),
                        ShowImage3: $('#imgShow3').attr('src'),
                        ShowImage4: $('#imgShow4').attr('src'),
                        ShowImage5: $('#imgShow5').attr('src'),
                        Age: $("#txtAge").val(),
                        SchoolName: $("#txtSchoolName").val(),
                        IntroductionDetail: $("#txtIntroductionDetail").val(),
                        BottomContent: editor.html(),
                        Status: $("#ddlStatus").val(),
                        Remark: $("#txtRemark").val(),
                        Ex4: $('#txtEx4').val(),
                        Ex6: $('#txtEx6').val(),
                        Phone: $('#txtPhone').val(),
                        OtherInfoLink: $.trim($("#txtOtherInfoLink").val()),

                        Contact: $('#txtContact').val(),
                        Ex1: $('#imgEx1').attr("src"),
                        Ex2: $('#txtEx2').val(),
                        Ex3: $('#txtEx3').val(),

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
            $("#txtEx1").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtEx1',
                         dataType: 'text',
                         success: function (result) {
                             $.messager.progress('close');

                             //try {
                             //    result = result.substring(result.indexOf("{"), result.indexOf("</"));
                             //} catch (e) {
                             //    alert(e);
                             //}
                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {

                                 $('#imgEx1').attr('src', resp.ExStr);
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


            //
            $("#txtThumbnailsPath1").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Vote&filegroup=file2',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath1',
                         dataType: 'text',
                         success: function (result) {

                             $.messager.progress('close');

                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {

                                 $('#imgShow1').attr('src', resp.ExStr);
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

            $("#txtThumbnailsPath2").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Vote&filegroup=file3',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath2',
                         dataType: 'text',
                         success: function (result) {

                             $.messager.progress('close');

                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {

                                 $('#imgShow2').attr('src', resp.ExStr);
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
            //
            $("#txtThumbnailsPath3").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Vote&filegroup=file4',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath3',
                         dataType: 'text',
                         success: function (result) {
                             $.messager.progress('close');


                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {

                                 $('#imgShow3').attr('src', resp.ExStr);
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
            //
            $("#txtThumbnailsPath4").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Vote&filegroup=file5',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath4',
                         dataType: 'text',
                         success: function (result) {
                             $.messager.progress('close');


                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {

                                 $('#imgShow4').attr('src', resp.ExStr);
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
            //
            $("#txtThumbnailsPath5").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Vote&filegroup=file6',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath5',
                         dataType: 'text',
                         success: function (result) {
                             $.messager.progress('close');


                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {

                                 $('#imgShow5').attr('src', resp.ExStr);
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
            ShowVoteType("<%=votemodel.VoteType%>");

            $("#ddlRemark").change(function () {

                $(txtRemark).val($(this).val());

            })




        });





        function ResetCurr() {

            $(":input[type=text]").val("");
            $("#txtHobbies").val("");
            $("#txtIntroduction").val("");
            $("#txtIntroductionDetail").val("")
            editor.html("");
        }

        // 根据投票类型显示对应的内容
        function ShowVoteType(type) {

            switch (type) {

                case "0": //图片投票
                    $("#trimgShow1").show();
                    $("#trimgShow2").show();
                    $("#trimgShow3").show();
                    $("#trimgShow4").show();
                    $("#trimgShow5").show();
                    $("#txtIntroductionDetail").hide();
                    break;
                case "1": //视频投票
                    $("#trimgShow1").hide();
                    $("#trimgShow2").hide();
                    $("#trimgShow3").hide();
                    $("#trimgShow4").hide();
                    $("#trimgShow5").hide();
                    $("#txtIntroductionDetail").show();
                    break;
                default:
                    $("#trimgShow1").show();
                    $("#trimgShow2").show();
                    $("#trimgShow3").show();
                    $("#trimgShow4").show();
                    $("#trimgShow5").show();
                    $("#txtIntroductionDetail").hide();
                    break;


            }


        }


        KindEditor.ready(function (K) {
            editor = K.create('#txtBottomContent', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
        });

        function TransformImage(id) {

            var obj = $("#" + id);
            var imagePath = obj.attr("src");
            if (imagePath.indexOf("?") > 0) {
                imagePath = imagePath.substring(0, imagePath.indexOf("?"));

            }
            else {

            }

            $.messager.progress({ text: '正在旋转图片...' });
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: "TransformImage", ImagePath: imagePath },
                dataType: "json",
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.Status == 1) {
                        obj.attr("src", resp.ExStr + "?v=" + Math.random());
                        alert("图片旋转成功");
                        //window.location.reload();


                    }
                    else {
                        Alert("操作失败");
                    }
                }
            });


        }

    </script>
</asp:Content>
