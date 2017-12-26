<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="PositionInfoAddU.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.PositionInfoAddU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <link href="/DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <link href="/DatePicker/skin/whyGreen/datepicker.css" rel="stylesheet" type="text/css" />
    <link href="/DatePicker/skin/default/datepicker.css" rel="stylesheet" type="text/css" />
    <script src="/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/DatePicker/config.js" type="text/javascript"></script>
    <script src="/DatePicker/calendar.js" type="text/javascript"></script>
--%>   
 <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXWuBuHuiPosintionHandler.ashx";
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
                    var Trade = [];
                    var Professional = [];
                    $("input[name='trade']:checked").each(function () {
                        Trade.push($(this).val());
                    });
                    $("input[name='professional']:checked").each(function () {
                        Professional.push($(this).val());
                    });
                    var model =
                    {
                        Autoid: currID,
                        Action: "AUPositionInfo",
                        Title: $.trim($('#txtTitle').val()),
                        IocnImg: imgThumbnailsPath.src,
                        Personal: $.trim($('#txtPersonal').val()),
                        SalaryRange: $.trim($('#txtSalaryRange').val()),
                        EnterpriseScale: $.trim($('#txtEnterpriseScale').val()),
                        Address: $.trim($('#txtAddress').val()),
                        WorkYear: $.trim($('#txtWorkYear').val()),
                        Education: $.trim($('#txtEducation').val()),
                        TutorExplain: editor.html(),
                        TradeIds: Trade.join(','),
                        ProfessionalIds: Professional.join(','),
                        City:$.trim($('#ddlcity').val()),
                        Company: $.trim($('#txtCompany').val())
                    };
                    $.messager.progress({ text: '正在处理...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType:'json',
                        success: function (resp) {
                            $.messager.progress('close');
                            if (resp.Status ==1) {
                                alert(resp.Msg);
                                window.location.href = "PositionInfoMag.aspx";
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
                data: { Action: 'GetPositionInfo', Autoid: currID },
                success: function (result) {
                    try {
                        var resp = $.parseJSON(result);
                        if (resp.Status == 0) {
                            var model = resp.ExObj;

                            $('#txtTitle').val(model.Title);
                            imgThumbnailsPath.src = model.IocnImg;
                            $('#txtPersonal').val(model.Personal);
                            $('#txtSalaryRange').val(model.SalaryRange);
                            $('#txtEnterpriseScale').val(model.EnterpriseScale);
                            $('#txtAddress').val(model.Address);
                            $('#txtWorkYear').val(model.WorkYear);
                            $('#txtEducation').val(model.Education);
                            $("#ddlcity").val(model.City);
                            $("#txtCompany").val(model.Company);
                            editor.html(model.Context);
                            if (model.TradeIds != null && model.TradeIds != "") {
                                var TradeIds = model.TradeIds.split(',');
                                for (var i = 0; i < TradeIds.length; i++) {

                                    $("input[name='trade']").each(function () {

                                        if ($(this).val() == TradeIds[i]) {
                                            $(this).attr("checked", "checked");

                                        }

                                    });


                                }
                            }
                            if (model.ProfessionalIds != null && model.ProfessionalIds != "") {
                                var ProfessionalIds = model.ProfessionalIds.split(',');
                                for (var i = 0; i < ProfessionalIds.length; i++) {
                                    $("input[name='professional']").each(function () {
                                        if ($(this).val() == ProfessionalIds[i]) {
                                            $(this).attr("checked", "checked");
                                        }
                                    });
                                }
                            }
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理 >职位
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <a href="PositionInfoMag.aspx" class="easyui-linkbutton" iconcls="icon-redo" plain="true">
            返回</a>
        <br />
        <hr style="border: 1px dotted #036" />
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTitle" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPersonal" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        工作年限：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtWorkYear" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        学历：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtEducation" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        logo：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a><br />
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为240*120。
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        薪资范围：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSalaryRange" class="" style="width: 100%;" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        公司名称：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtCompany" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        公司规模：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtEnterpriseScale" class="" style="width: 100%;" />
                    </td>
                </tr>

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        地址：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlcity">
                        <option value="上海">上海</option>
                         <option value="香港">香港</option>
                          <option value="浙江">浙江</option>
                           <option value="河北">河北</option>
                            <option value="北京">北京</option>
                        </select>
                        <input type="text" id="txtAddress" class="" style="width:50%;" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        行业：
                    </td>
                    <td width="*" align="left">
                       <%
                         System.Text.StringBuilder sbTrade = new StringBuilder();
                          for (int i = 0; i < TradeList.Count; i++)
                         {
                             sbTrade.AppendFormat("<input type=\"checkbox\" id=\"{0}\"  name=\"trade\" value=\"{1}\" />", "trade" + TradeList[i].AutoID, TradeList[i].AutoID);
                             sbTrade.AppendFormat("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "trade" + TradeList[i].AutoID, TradeList[i].CategoryName);
                             
                         } 
                           Response.Write(sbTrade.ToString());
                         %>


                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        专业：
                    </td>
                    <td width="*" align="left">
                       <%
                          System.Text.StringBuilder sbProfessional = new StringBuilder();
                         for (int i = 0; i < ProfessionalList.Count; i++)
                         {
                             sbProfessional.AppendFormat("<input type=\"checkbox\" id=\"{0}\"  name=\"professional\" value=\"{1}\" />", "professional" + ProfessionalList[i].AutoID, ProfessionalList[i].AutoID);
                             sbProfessional.AppendFormat("<label for=\"{0}\">{1}</label>&nbsp;&nbsp;&nbsp;", "professional" + ProfessionalList[i].AutoID, ProfessionalList[i].CategoryName);
                             
                         }
                         Response.Write(sbProfessional.ToString());
                         %>


                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        职位描述：
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
