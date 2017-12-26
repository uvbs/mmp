<%@ Page Title="发表新分享" Language="C#" MasterPageFile="~/Master/WapMain.Master" AutoEventWireup="true" CodeBehind="NewShare.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.NewShare" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetCurrUserModel(); %>
    <link href="/UMEditor/themes/default/_css/umeditor.css" rel="stylesheet" type="text/css" />
    <script src="/UMEditor/umeditor.config.js" type="text/javascript"></script>
    <script src="/UMEditor/editor_api.js" type="text/javascript"></script>
    <script src="/UMEditor/lang/zh-cn/zh-cn.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var openId = '<%=currUser.WXOpenId %>';
        var currAcvityID = 0;
        var currAction = 'add';
        var ue;
        var isNotSubmit = true;
        $(function () {

            var btnSaveLock = false;

            $('#btnSave').live('click', function () {
                try {

                    if (btnSaveLock)
                        return;
                    btnSaveLock = true;

                    var model = {
                        IsSignUpJubit: 0,
                        ActivityName: $.trim($('#txtTitle').val()),
                        ActivityWebsite: "",
                        ActivityDescription: ue.getContent(), //$('#txtContent').val(),
                        Action: currAction == 'add' ? 'AddJuActivity' : 'EditJuActivity',
                        JuActivityID: currAcvityID,
                        ThumbnailsPath: GetRandomHb(),
                        IsHide: 0,
                        IsByWebsiteContent: 0,
                        ArticleType: 'article',
                        ArticleTypeEx1: 'hf_article',
                        ArticleTemplate: 1,
                        IsSpread: 1,
                        RecommendCate: $('#selectCate').val()
                    };
                    if (model.ActivityName == '') {
                        btnSaveLock = false;
                        $('#lbDlgMsg').html('请输入分享标题');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');

                        return;
                    }
                    if (model.ActivityDescription == '') {
                        btnSaveLock = false;
                        $('#lbDlgMsg').html('请输入分享正文');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }
                    $.mobile.loading('show', { textVisible: true, text: '正在提交...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        success: function (result) {
                            try {
                                $.mobile.loading('hide');
                                var resp = $.parseJSON(result);
                                if (resp.Status == 1) {
                                    isNotSubmit = false;
                                    ClearLocalData();

                                    $('#lbDlgMsg').html("已成功创建分享!<br />正在转到分享页面。。。");
                                    $('#dlgMsg').popup();
                                    $('#dlgMsg').popup('open');


                                    setInterval(function () {
                                        window.location.href = '/' + resp.ExObj.JuActivityIDHex + '/share.chtml';
                                        btnSaveLock = false;
                                    }, 1500);

                                }
                                else {
                                    btnSaveLock = false;
                                    $('#lbDlgMsg').html(resp.Msg);
                                    $('#dlgMsg').popup();
                                    $('#dlgMsg').popup('open');
                                }
                            } catch (e) {
                                btnSaveLock = false;
                                alert(e);
                            }
                        }
                    });

                } catch (e) {
                    alert(e);
                }

            });

            ue = UM.getEditor('myEditor', {
                toolbar: ['undo redo | bold italic underline forecolor backcolor insertorderedlist insertunorderedlist justifyleft justifycenter justifyright justifyjustify'],
                //focus时自动清空初始化时的内容
                autoClearinitialContent: true,
                //关闭字数统计
                wordCount: false,
                //关闭elementPath
                elementPathEnabled: false,
                //默认的编辑区域高度
                initialFrameHeight: 300,
                //是否保持toolbar的位置不动
                autoFloatEnabled: false
            });


            GetLocalData();

            try {

                setInterval(function () { SetLocalData() }, 500);

            } catch (e) {
                alert(e);
            }

        });

        function SetLocalData() {
            if (isNotSubmit) {
                SetCookie('article_share_Cnt', ue.getContent());
                SetCookie('article_share_Title', $('#txtTitle').val());
            }
        }

        function GetLocalData() {

            var articleCnt = getCookie('article_share_Cnt');
            var articleTitle = getCookie('article_share_Title');
            //            alert(articleTitle);
            //            alert(articleCnt);
            if (articleTitle != null)
                $('#txtTitle').val(articleTitle);
            if (articleCnt != null)
                ue.setContent(articleCnt);

        }

        function ClearLocalData() {
            try {
                //情空cookie数据
                SetCookie('article_share_Cnt', '');
                SetCookie('article_share_Title', '');
            } catch (e) {
                alert(e);
            }
        }

        //获取随机海报
        function GetRandomHb() {
            var randInt = GetRandomNum(1, 21);
            return "/img/hb/hb" + randInt + ".jpg";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div data-role="page" id="page-title" data-theme="b">
        <div data-role="header" data-theme="f" data-tap-toggle="false" style="" id="divTop">
            <a href="#" data-role="button" data-rel="back" data-icon="arrow-l">返回</a>
            <h1>
                发表分享
            </h1>
        </div>
        <label for="txtTitle">
            所属类别:</label>
        <select id="selectCate">
            <%
                //读取hf分类
                List<ZentCloud.BLLJIMP.Model.UserPersonalizeDataInfo> cateList = new ZentCloud.BLLJIMP.BLLUserPersonalize("").QueryUserPList("hf").Where(p => p.PersonalizeType == 1).ToList();

                StringBuilder strCateHtml = new StringBuilder();
                foreach (var item in cateList)
                {
                    strCateHtml.AppendFormat("<option value=\"{0}\">{0}</option>",
                            item.Val1
                        );
                }

                Response.Write(strCateHtml.ToString());
                 
                 
            %>
        </select>
        <%
            int cateId = Convert.ToInt32(Request["cateId"]);
            ZentCloud.BLLJIMP.Model.UserPersonalizeDataInfo cateModel = new ZentCloud.BLLJIMP.BLLUserPersonalize("").Get(cateId);
            string currSelectCate = cateModel == null ? "" : cateModel.Val1;
        %>
        <script type="text/javascript">
            var selectCate = '<%=currSelectCate %>';
            $('#selectCate').val(selectCate);
        </script>
        <label for="txtTitle">
            分享标题:</label>
        <input type="text" id="txtTitle" value="" placeholder="请输入分享标题" />
        <label for="txtContent">
            分享正文:</label>
        <table width="100%">
            <tr>
                <td style="width: 1px">
                </td>
                <td style="width: *">
                    <script type="text/plain" id="myEditor" style="width: 100%; height: 200px; word-wrap: break-word;">
                <p>请输入分享内容</p>
                    </script>
                </td>
                <td style="width: 1px;">
                </td>
            </tr>
        </table>
        <a href="#" data-role="button" data-inline="false" data-theme="f" id="btnSave" data-ajax="false"
            data-mini="false">立即发布</a>
        <div data-role="popup" id="dlgMsg" style="padding: 20px; text-align: center; font-weight: bold;">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
            <label id="lbDlgMsg">
            </label>
        </div>
    </div>
</asp:Content>

