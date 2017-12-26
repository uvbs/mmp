<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="Config.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.DistributionMall.Config" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .sort {
            height: 0 !important;
        }

        .centent_r_btm {
            border: 0;
        }

        .hide {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%; padding-bottom: 72px;">
            <table width="100%" style="border-spacing: 10px;">
                <tr class="hide">
                    <td align="right" class="tdTitle">一级分销提成：
                    </td>
                    <td>
                        <input class="form-control" style="width: 200px; display: inline-block;" type="text"
                            id="txtDistributionRateLevel1" value="<%=website.DistributionRateLevel1 %>" onkeyup="value=value.replace(/[^\d]/g,'')" />
                        <span style="font-size: 16px; pandding-left: 10px;">%</span>
                    </td>
                </tr>
                <tr class="hide">
                    <td align="right" class="tdTitle">二级分销提成：
                    </td>
                    <td>
                        <input class="form-control" style="width: 200px; display: inline-block;" type="text"
                            id="txtDistributionRateLevel2" value="<%=website.DistributionRateLevel2%>" onkeyup="value=value.replace(/[^\d]/g,'')" />
                        <span style="font-size: 16px; pandding-left: 10px;">%</span>
                    </td>
                </tr>
                <tr class="hide">
                    <td align="right" class="tdTitle">三级分销提成：
                    </td>
                    <td>
                        <input class="form-control" style="width: 200px; display: inline-block;" type="text"
                            id="txtDistributionRateLevel3" value="<%=website.DistributionRateLevel3%>" onkeyup="value=value.replace(/[^\d]/g,'')" />
                        <span style="font-size: 16px; pandding-left: 10px;">%</span>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="tdTitle">显示头像[二维码页面]：
                    </td>
                    <td>
                        <input id="rdoShowAvatar" class="positionTop2" type="radio" name="avatar" <%= (config==null||config.IsHideHeadImg==0)?"checked='checked'":"" %> value="0" /><label for="rdoShowAvatar">是</label>
                        <input id="rdoHideAvatar" class="positionTop2" type="radio" name="avatar" <%= (config!=null&&config.IsHideHeadImg==1)?"checked='checked'":"" %> value="1" /><label for="rdoHideAvatar">否</label>
                    </td>
                </tr>
                
                <tr>
                    <td align="right" class="tdTitle">是否显示微信昵称：
                    </td>
                    <td>
                        <input id="rdoShowWXNickName" class="positionTop2" type="radio" name="nick" <%= (config!=null&&config.IsShowWXNickName==0)?"checked='checked'":"" %> value="0" /><label for="rdoShowWXNickName">是</label>
                        <input id="rdoHideWXNickName" class="positionTop2" type="radio" name="nick" <%= (config==null||config.IsShowWXNickName==1)?"checked='checked'":"" %> value="1" /><label for="rdoHideWXNickName">否</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">微信昵称显示位置：</td>
                    <td>
                        <input type="radio" class="positionTop2" name="QRCodeText" id="rdoShowHeadButton" <%= (config==null||config.WXNickShowPosition==0)?"checked='checked'":"" %> value="0" /><label for="rdoShowHeadButton">显示在头像下方</label>
                        <input type="radio" class="positionTop2" name="QRCodeText" id="rdoShowQRCodeButton" <%= (config!=null&&config.WXNickShowPosition==1)?"checked='checked'":"" %> value="1" /><label for="rdoShowQRCodeButton">显示在二维码下方</label>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">微信昵称字体颜色：
                    </td>
                    <td>
                        <input  class="positionTop2 form-control" id="txtWXNickNameFontColor" style="width:200px;" type="color"   value="<%=config.WXNickNameFontColor %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">我的二维码背景图：
                    </td>
                    <td>
                        <img alt="缩略图" src="<%=website.DistributionShareQrcodeBgImg%>" width="120px" height="200px"
                            id="imgThumbnailsPath" /><br />
                        <a id="auploadThumbnails" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2"
                            plain="true" onclick="txtThumbnailsPath.click()">上传图片</a>
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG格式图片，宽高比例为640*1008
                        <input type="file" id="txtThumbnailsPath" name="file1" style="visibility: hidden;" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">系统推广二维码：
                    </td>
                    <td>
                        <img src="<%=qrcondeUrl %>" width="300" height="300">
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;" align="right" valign="middle">二维码使用说明：
                    </td>
                    <td style="width: *;" align="left" valign="middle">
                        <div id="divQRCodeUseGuide">
                            <div id="txtQRCodeUseGuide" style="width: 375px; height: 360px;">
                               <%=config.QRCodeUseGuide %>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr><td style="width: 200px;" align="right" valign="middle">未成为代言人提示消息</td>
                    <td>
                       
                        <textarea style="width:90%;height:100px;" id="txtNotDistributionMsg" ><%=website.NotDistributionMsg%></textarea>

                    </td>


                </tr>
            </table>
        </div>
        <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0; left: 0; height: 60px; line-height: 60px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top: 14px;">
            <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px; text-decoration: underline;"
                class="button button-rounded button-primary" onclick="Save();">保存</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var editor1;
        KindEditor.ready(function (K) {
         
            editor1 = K.create('#txtQRCodeUseGuide', {
                uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
                filterMode: false
            });
        });

        $(function () {
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
                                 $('#imgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 layer.msg(resp.Msg);
                             }
                         }
                     }
                    );

                } catch (e) {
                    alert(e);
                }
            });
        });

        function Save() {
            var distributionShareQrcodeBgImg = $('#imgThumbnailsPath').attr('src');

            var reqData = {
                action: 'UpdateDistributionMallConfig',
                distributionShareQrcodeBgImg: distributionShareQrcodeBgImg,
                distributionRateLevel1: $("#txtDistributionRateLevel1").val(),
                distributionRateLevel2: $("#txtDistributionRateLevel2").val(),
                distributionRateLevel3: $("#txtDistributionRateLevel3").val(),


                IsHideHeadImg: $('[name=avatar]:checked').val(),
                WXNickShowPosition: $('[name=QRCodeText]:checked').val(),
                QRCodeUseGuide: editor1.html(),
                IsShowWXNickName: $('[name=nick]:checked').val(),
                WXNickNameFontColor:$('#txtWXNickNameFontColor').val(),
                NotDistributionMsg: $("#txtNotDistributionMsg").val()
            };

           

            $.ajax({
                type: 'post',
                url: '/Handler/App/CationHandler.ashx',
                data: reqData,
                success: function (result) {
                    result = JSON.parse(result);
                    if (result.IsSuccess) {
                        layer.msg('保存成功');
                    } else {
                        layer.msg('保存失败');
                    }
                }
            });

        }
    </script>
</asp:Content>
