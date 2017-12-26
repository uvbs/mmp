<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Master/WapMain.Master"  CodeBehind="NewArticle.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.NewArticle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetCurrUserModel(); %>
    <link href="/UMEditor/themes/default/_css/umeditor.css" rel="stylesheet" type="text/css" />
    <script src="/UMEditor/umeditor.config.js" type="text/javascript"></script>
    <script src="/UMEditor/editor_api.js" type="text/javascript"></script>
    <script src="/UMEditor/lang/zh-cn/zh-cn.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/Handler/JuActivity/JuActivityHandler.ashx";
        var openId = '<%=currUser.WXOpenId %>';
        var currAcvityID = 0;
        var currAction = 'add';
        var ue;
        $(function () {

            $('#btnSave').live('click', function () {
                try {
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
                        ArticleTemplate: 1,
                        IsSpread: 1,
                        IsHideRecommend: 0
                    };
                    if (model.ActivityName == '') {
                        $('#lbDlgMsg').html('请输入文章标题');
                        $('#dlgMsg').popup();
                        $('#dlgMsg').popup('open');
                        return;
                    }
                    if (model.ActivityDescription == '') {
                        $('#lbDlgMsg').html('请输入文章正文');
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

                                    ClearLocalData();

                                    $('#lbDlgMsg').html("已成功创建文章!<br />正在转到分享页面。。。");
                                    $('#dlgMsg').popup();
                                    $('#dlgMsg').popup('open');
                                    window.location.href = '/' + resp.ExObj.JuActivityIDHex + '/share.chtml';


                                }
                                else {
                                    $('#lbDlgMsg').html(resp.Msg);
                                    $('#dlgMsg').popup();
                                    $('#dlgMsg').popup('open');
                                }
                            } catch (e) {
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
                //                $(document).keypress(function () {
                //                    SetLocalData();
                //                }
                //                );

                //                $(document).on("scrollstart", function () {
                //                    SetLocalData();
                //                });

                //                $(document).on("tap", function () {
                //                    SetLocalData();
                //                });

            } catch (e) {
                alert(e);
            }

            //            $('#btnTest').click(function () {
            //                alert(ue.getContent());
            //                alert(getCookie('articleCnt'));
            //            });
        });

        function SetLocalData() {

            SetCookie('articleCnt', ue.getContent());
            SetCookie('articleTitle', $('#txtTitle').val());
        }

        function GetLocalData() {

            var articleCnt = getCookie('articleCnt');
            var articleTitle = getCookie('articleTitle');

            //            alert(articleTitle);
            //            alert(articleCnt);
            //            
            if (articleTitle != null)
                $('#txtTitle').val(articleTitle);
            if (articleCnt != null)
                ue.setContent(articleCnt);


        }

        function ClearLocalData() {
            //情空cookie数据
            SetCookie('articleCnt', '');
            SetCookie('articleTitle', '');

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
           <a href="/FShare/Wap/UserHub.aspx" data-icon="home" data-ajax="false">主页</a>
            <h1>
                发表文章
            </h1>
        </div>
        <label for="txtTitle">
            文章标题:</label>
        <input type="text" id="txtTitle" value="" placeholder="请输入文章标题" />
        <label for="txtContent">
            文章正文:</label>
        <%--<textarea cols="40" rows="8" style="height: 160px" id="txtContent" placeholder="请输入文章正文"></textarea>--%>
        <table width="100%">
            <tr>
                <td style=" width:1px"></td>
                <td style=" width:*">
                    <script type="text/plain" id="myEditor" style=" width:100%; height:200px; word-wrap: break-word;">
                <p>请输入文章内容</p>
            </script>        
                </td>
                <td style=" width:1px;"></td>
            </tr>
        </table>
            
        
        <a href="#" data-role="button" data-inline="false" data-theme="f" id="btnSave" data-ajax="false"
            data-mini="false">立即发布</a>
             <%--<a href="#" data-role="button" data-inline="false" data-theme="f" id="btnTest" data-ajax="false"
            data-mini="false">测试1</a>--%>
            <div data-role="popup" id="dlgMsg" style="padding: 20px; text-align: center; font-weight: bold;">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
            <label id="lbDlgMsg">
            </label>
        </div>
    </div>
</asp:Content>

