<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXTutorInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.WXTutorInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXWuBuHuiTutorHandler.ashx";
        var editor;
        var currID = '<%=AutoId %>';
        $(function () {

            if (currID != "") {
                ShowEdit(currID);
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
                    var TradeStr = "";
                    var ProfessionalStr = "";
                    $("input[name='trade']:checked").each(function () {
                        TradeStr += $(this).val() + ",";
                    });
                    $("input[name='Professional']:checked").each(function () {
                        ProfessionalStr += $(this).val() + ",";
                    });

                    var model =
                    {
                        Autoid: currID,
                        Action: "AddTutorInfo",
                        TutorName: $.trim($('#txtTutorName').val()),
                        TutorExplain: editor.html(),
                        TutorImg: $("#imgThumbnailsPath").attr("src"),
                        TradeStr: TradeStr,
                        ProfessionalStr: ProfessionalStr,
                        UserId: $("#txtUserId").val(),
                        Company: $("#txtCompany").val(),
                        Position: $("#txtPosition").val(),
                        Signature: $("#txtSignature").val(),
                        WxQiyeUserId: $("#txtWxQiyeUserId").val(),
                        City: $("#txtCity").val()
                    };

                    $.messager.progress({ text: '正在处理。。。' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType:'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status ==1) {
                                alert(resp.Msg);
                                window.location.href = "TutorManger.aspx";
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

        });


        //格式化当前特殊情况时间
        function FormateCurrPageDate(d, h, m) {
            var result = new StringBuilder();
            result.AppendFormat('{0} {1}:{2}:00', d, h, m);
            return result.ToString();
        }

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


        function ShowEdit(activityID) {
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: 'GetTutorInfo', Autoid: currID },
                success: function (result) {
                    try {
                        var resp = $.parseJSON(result);
                        if (resp.Status == 0) {
                            var model = resp.ExObj;
                            $("#txtUserId").val(model.UserId);
                            $('#txtTutorName').val(model.TutorName);
                            editor.html(model.TutorExplain);
                            imgThumbnailsPath.src = model.TutorImg;
                            $("#txtCompany").val(model.Company);
                            $("#txtPosition").val(model.Position);
                            $("#txtSignature").val(model.Signature);
                            $("#txtWxQiyeUserId").val(model.WxQiyeUserId);
                            $("#txtCity").val(model.City);

                            
                        }
                        else {
                            alert(resp.Msg);
                        }
                    } catch (e) {
                        alert(e);
                    }
                }

            });


        }

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
    当前位置：&nbsp;五步会 > 导师
     <a href="TutorManger.aspx" style="float:right;margin-right:20px;" class="easyui-linkbutton" iconcls="icon-back" plain="true">
            返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
       
        
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        导师名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTutorName" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        用户名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtUserId" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        描述：
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        行业：
                    </td>
                    <td width="*" align="left">
                        <%=tradeStr%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        专业：
                    </td>
                    <td width="*" align="left">
                        <%=ProfessionalStr%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        头像：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为300*300。
                        
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                     <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        公司：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCompany" class="" style="width: 100%;" value="<%=tInfo.Company%>"  />
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        职位：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPosition" class="" style="width: 100%;" value="<%=tInfo.Position%>" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        签名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSignature" class="" style="width: 100%;" value="<%=tInfo.Signature%>" />
                    </td>
                </tr>
                <tr>
                <td style="width: 100px;" align="right" class="tdTitle">
                  微信企业号成员唯一标识：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtWxQiyeUserId" placeholder="成员唯一标识，不支持中文"  style="width: 100%;" value="<%=tInfo.WxQiyeUserId%>" />
                    </td>
                </tr>
                
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        城市：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCity" class="" />
                    </td>
                </tr>


                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <input type="hidden" id="Aid" value="0" />
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
