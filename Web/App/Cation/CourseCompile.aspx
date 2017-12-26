<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CourseCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.CourseCompile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="/Kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />--%>
    <%--    <script src="/Kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="/Kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    --%>
    <link href="/kindeditor-4.1.10/themes/default/default.css" rel="stylesheet" type="text/css" />
    <script src="/kindeditor-4.1.10/kindeditor.js" type="text/javascript"></script>
    <script src="/kindeditor-4.1.10/lang/zh_CN.js" type="text/javascript"></script>
    <link href="/Ju-Modules/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/Ju-Modules/bootstrap/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css"
        rel="stylesheet" type="text/css" />
    <script src="/Ju-Modules/bootstrap/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js"
        type="text/javascript"></script>
    <script src="/Ju-Modules/bootstrap/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js"
        type="text/javascript"></script>
    <%--<script type="text/javascript">

        $(function () {
            var myMenu;
            myMenu = new SDMenu("my_menu");
            myMenu.init();
            var firstSubmenu = myMenu.submenus[0];
            myMenu.expandMenu(firstSubmenu);
        });

    </script>--%>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currAction = '<%=webAction %>';
        var currAcvityID = '<%=CurrActivityModel.JuActivityID %>';

        var editor;
        var grid;
        $(function () {

            $('#divActivity').hide();
            $('#divActivityWebsite').hide();


            if ($.browser.msie) { //ie 下
                //缩略图
                $("#auploadThumbnails").hide();
                $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
            }
            else {
                $("#txtThumbnailsPath").hide(); //缩略图
            }
            if (currAction == 'add') {
                GetRandomHb();
            }
            else {
                ShowEdit(currAcvityID);
            }

            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        IsSignUpJubit: 0,
                        SignUpActivityID: 0,
                        ActivityName: $.trim($('#txtActivityName').val()),
                        ActivityWebsite: $.trim($('#txtActivityWebsite').val()),
                        ActivityDescription: editor.html(), //$.trim($(txtActivityDescription).val()),
                        ThumbnailsPath: $('#imgThumbnailsPath').attr('path'),
                        Action: currAction == 'add' ? 'AddJuActivity' : 'EditJuActivity',
                        JuActivityID: currAcvityID,
                        IsHide: rdoIsHide.checked ? 1 : 0,
                        alluser: 1,
                        IsByWebsiteContent: 0, //rdoWriteIsOnline.checked ? 0 : 1,
                        ArticleType: 'article',
                        ArticleTypeEx1: 'hf_course',
                        ArticleTemplate: 1,
                        IsSpread: 1,
                        ActivityLecturer: $.trim($('#txtActivityLecturer').val()),
                        ActivityStartDate: $('#txtActivityStartDate').val(),
                        RecommendCate: GetCheckGroupVal('RecommendCate', 'v')
                    };

                    if (model.ActivityName == '') {
                        $('#txtActivityName').focus();
                        Alert('请输入课程标题！');
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


            $("input[name='IsSignUpJubit']").change(function () {
                try {
                    var val = $(this).attr('v');
                    if (val == '2') {
                        $("#btnSelectActivity").show();
                        $('#lblSignUpAcvityID').show();
                    }
                    else {
                        $('#txtSignUpActivityID').hide();
                        $("#btnSelectActivity").hide();
                        $('#lblSignUpAcvityID').hide();

                    }
                } catch (e) {
                    alert(e);
                }
            });

            $("input[name='WriteIsOnline']").change(function () {
                try {
                    var val = $(this).attr('v');

                    if (val == '1') {

                        $('#divActivityWebsite').hide();

                        $('#lbEditor').show();
                        $('#divEditor').show();
                    }
                    else {

                        $('#divActivityWebsite').show();

                        $('#lbEditor').hide();
                        $('#divEditor').hide();
                    }
                } catch (e) {
                    alert(e);
                }
            });

            $("#txtThumbnailsPath").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片。。。' });

                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
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


            $('.form_datetime').datetimepicker({
                language: 'zh-CN',
                weekStart: 1,
                todayBtn: 1,
                autoclose: 1,
                todayHighlight: 1,
                startView: 2,
                forceParse: 0,
                showMeridian: 1,
                format: 'yyyy-mm-dd hh:ii'
            });

            //$(txtActivityStartDate).val('2014-1-1 12:00');
            $('#btnTest').click(function () {
                //alert($(txtActivityStartDate).val());
                $('#myModal').modal('show')
            });
        }

        );


        //格式化当前特殊情况时间
        function FormateCurrPageDate(d, h, m) {
            var result = new StringBuilder();
            result.AppendFormat('{0} {1}:{2}:00', d, h, m);
            return result.ToString();
        }

        function ShowEdit(activityID) {
            //ClearAll();
            //获取数据
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: { Action: 'GetSingelJuActivity', JuActivityID: activityID },
                success: function (result) {
                    try {
                        var resp = $.parseJSON(result);
                        if (resp.Status == 1) {
                            var model = resp.ExObj;
                            $('#txtSignUpActivityID').val(model.SignUpActivityID);
                            $('#txtActivityName').val(model.ActivityName);
                            $('#txtActivityAddress').val(model.ActivityAddress);
                            $('#txtActivityWebsite').val(model.ActivityWebsite);

                            editor.html(model.ActivityDescription);

                            imgThumbnailsPath.src = model.ThumbnailsPath;
                            $('#imgThumbnailsPath').attr('path', model.ThumbnailsPath);

                            $('#txtActivityLecturer').val(model.ActivityLecturer);

                            try {
                                $('#txtActivityStartDate').val(FormatDate(model.ActivityStartDate));
                            } catch (e) {
                            }

                            $('#txtSignUpActivityID').hide();

                            if (model.IsHide == 1)
                                rdoIsHide.checked = true;
                            else
                                rdoIsNotHide.checked = true;


                        }
                        else {
                            Alert(resp.Msg);
                        }
                    } catch (e) {
                        Alert(e);
                    }
                }

            });


        }


        function ResetCurr() {
            ClearAll();
            editor.html('');
        }

        //获取随机海报
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 7);
            imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
            $('#imgThumbnailsPath').attr('path', "/img/hb/hb" + randInt + ".jpg");
        }

        //多选框分组赋值
        function CurrSetCheckGroupVal(values) {
            try {
                var value = values.split(',');
                for (var i = 0; i < value.length; i++) {
                    $('input[name="chkRecommendCate"]').each(function () {
                        if (value[i] == $(this).val()) {
                            this.checked = true;
                        }
                    });
                }
            } catch (e) {
                alert(e);
            }
        }
        //多选框分组取值
        function CurrGetCheckGroupVal() {
            try {
                var values = [];
                $('input[name="chkRecommendCate"]:checked').each(function () {
                    var id = $(this).val();
                    values.push(id);
                });
                if (values.length > 0)
                    return values.join(',');
                else
                    return '';
            } catch (e) {
                alert(e);
            }
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
            height: 40px;
        }
        
        .sort
        {
            width: 780px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="javascript:;" onclick="window.location.href='ArticleManage.aspx'">课程管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %>课程<%if (CurrActivityModel != null && webAction == "edit") { Response.Write("：" + CurrActivityModel.ActivityName); } %></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-redo" plain="true"
            onclick="window.location.href='CourseManage.aspx'">返回</a>
        <br />
        <hr style="border: 1px dotted #036" />
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        课程分类：
                    </td>
                    <td width="*" align="left">
                        <%
                            if (CurrActivityModel.RecommendCate == null)
                                CurrActivityModel.RecommendCate = "";

                            //读取hf分类
                            List<ZentCloud.BLLJIMP.Model.UserPersonalizeDataInfo> cateList = new ZentCloud.BLLJIMP.BLLUserPersonalize("").QueryUserPList("hf").Where(p => p.PersonalizeType == 2).ToList(); ;

                            StringBuilder strCateHtml = new StringBuilder();
                            foreach (var item in cateList)
                            {
                                strCateHtml.AppendFormat("<input type=\"checkbox\" name=\"RecommendCate\" v=\"{1}\" id=\"chkArticleCate{0}\" {2} /><label for=\"chkArticleCate{0}\">{1}</label> &nbsp;",
                                        item.PersonalizeID,
                                        ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetJuActiviCateRName(item.PersonalizeID),
                                        CurrActivityModel.RecommendCate.ToLower().Contains(item.Val1.ToLower()) ? "checked" : ""
                                    );
                            }
                            Response.Write(strCateHtml.ToString());
                            
                            
                            //ZentCloud.BLLJIMP.Model.WebsiteInfo websiteInfo = ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetWebsiteInfoModel();
                            //StringBuilder strCateHtml = new StringBuilder();
                            //strCateHtml.AppendFormat("<input type=\"checkbox\" name=\"RecommendCate\" v=\"{0}\" id=\"chkArticleCate1\" /><label for=\"chkArticleCate1\">{0}</label> &nbsp;", websiteInfo.CourseCate1);

                            //strCateHtml.AppendFormat("<input type=\"checkbox\" name=\"RecommendCate\" v=\"{0}\" id=\"chkArticleCate2\" /><label for=\"chkArticleCate2\">{0}</label> &nbsp;", websiteInfo.CourseCate2);
                            //Response.Write(strCateHtml.ToString());       
                            
                        %>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        课程标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtActivityName" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        讲师：
                    </td>
                    <td width="*" align="left">
                        <input type="text" value="" id="txtActivityLecturer">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        开课时间：
                    </td>
                    <td width="*" align="left">
                        <input type="text" value="" readonly class="form_datetime" id="txtActivityStartDate">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        课程缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" /><br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a><br />
                        <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        课程状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="IsHide" id="rdoIsNotHide" checked="checked" v="0" /><label
                            for="rdoIsNotHide">显示</label>
                        <input type="radio" name="IsHide" id="rdoIsHide" v="1" /><label for="rdoIsHide">隐藏</label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                        <label id="lbEditor">
                            课程内容：</label>
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtEditor" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle" valign="top">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <br />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;" class="button button-rounded button-flat-primary">
                            保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold;" class="button button-rounded button-flat">
                                重置</a> <a href="javascript:;" id="btnTest" style="font-weight: bold; display: none;"
                                    class="button button-rounded button-flat-primary">测试</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

