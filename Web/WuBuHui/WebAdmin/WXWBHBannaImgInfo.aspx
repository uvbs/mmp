<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXWBHBannaImgInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.WXWBHBannaImgInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXWuBuHuiPartnerHandler.ashx";
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

                    var PartnerStr = "";
                    $("input[name='Partner']:checked").each(function () {
                        PartnerStr += $(this).val() + ",";
                    });
                    var model =
                    {
                        Autoid: currID,
                        Action: "AUBannerInfo",
                        BannaUrl: $.trim($('#txtBannaUrl').val()),
                        BannaImg: imgThumbnailsPath.src
                    };


                    $.messager.progress({ text: '正在处理。。。' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        success: function (result) {
                            $.messager.progress('close');
                            var resp = $.parseJSON(result);
                            if (resp.Status == 0) {
                                window.location.href = "WXWBHBannaImg.aspx";
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


        function ShowEdit(activityID) {
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: 'GetBannerInfo', Autoid: currID },
                success: function (result) {
                    try {
                        var resp = $.parseJSON(result);
                        if (resp.Status == 0) {
                            var model = resp.ExObj;
                            imgThumbnailsPath.src = model.BannaImg;
                            $('#txtBannaUrl').val(model.BannaUrl);
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
            $('#imgThumbnailsPath').attr('path', "/img/hb/hb" + randInt + ".jpg");
        }


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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理 > 五伴会
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <a href="javascript:history.go(-1)" class="easyui-linkbutton" iconcls="icon-redo" plain="true">
            返回</a>
        <br />
        <hr style="border: 1px dotted #036" />
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        banner图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a><br />
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为640*220。
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        URL：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtBannaUrl" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <input type="hidden" id="Aid" value="0" />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;" class="button button-rounded button-primary">
                            保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold;" class="button button-rounded button-flat">
                                重置</a>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
