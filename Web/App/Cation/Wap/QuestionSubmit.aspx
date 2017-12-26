<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMain.Master" AutoEventWireup="true" CodeBehind="QuestionSubmit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.QuestionSubmit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div data-role="header" data-theme="b" data-position="fixed" style="" id="divTop">
        <a href="#" data-role="button" data-rel="back" data-icon="arrow-l">返回</a>
        <h1>
            提交问题</h1>
    </div>

    <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetCurrUserModel(); %>
        <table align="center" width="320px">
            <tr>
                <td style="width: 52px;">
                    <%
                        if (currUser != null)
                        {
                            if (string.IsNullOrWhiteSpace(currUser.WXHeadimgurl))
                                Response.Write(@"<img alt=""图片未显示"" src=""/img/offline_user.png"" width=""52"" height=""52"" />");
                            else
                                Response.Write(string.Format(@"<img alt=""图片未显示"" src=""{0}"" width=""52"" height=""52"" />", currUser.WXHeadimgurlLocal));
                        }
                    %>
                </td>
                <td style="width: *; text-decoration: none; margin-top: 10px; font-size: 16px;" align="left">
                    <strong>
                        <label id="lbName">
                            <% 
                                string str = "{还没有填写姓名}";
                                //显示排序：真实姓名、微信昵称、登录名、手机
                                if (currUser != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(currUser.WXNickname))
                                        str = currUser.WXNickname;
                                    else if (!string.IsNullOrWhiteSpace(currUser.TrueName))
                                        str = currUser.TrueName;
                                    else if (!string.IsNullOrWhiteSpace(currUser.LoginName))
                                        str = currUser.LoginName;
                                    else if (!string.IsNullOrWhiteSpace(currUser.Phone))
                                        str = currUser.Phone;
                                }

                                Response.Write(str);
                            %>
                        </label>
                    </strong>
                </td>
            </tr>
        </table>
        <table style="width: 100%; background-color: #fca572; color: #564e4c; text-align: center">
            <tbody>
                <tr>
                    <td>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    <label for="selectMaster">
        指定专家:</label>
    <select id="selectMaster">
        <option value="0">不指定</option>
        <%
            ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL();

            StringBuilder strHtml = new StringBuilder();

            foreach (var item in bll.GetList<ZentCloud.BLLJIMP.Model.JuMasterInfo>(string.Format(" WebsiteOwner = '{0}' ", ZentCloud.Common.ConfigHelper.GetConfigString("WebsiteOwner"))))
            {
                strHtml.AppendFormat("<option value=\"{0}\">{1}:{2}</option>", item.MasterID, item.MasterName, item.Title);
            }

            Response.Write(strHtml.ToString());
            
        %>
    </select>
    <label for="textarea">
        问题内容:</label>
    <textarea name="textarea" placeholder="点击输入问题内容" id="txtFeedBackContent" style="height: 120px;"
        required="required"></textarea>
    <a href="javascript:;" onclick="Save();" data-role="button" inline="true" data-theme="f"
        id="btnSubmitFeedBack">提交留言信息</a>
    <div data-role="popup" id="dlgMsg" style="padding: 20px; text-align: center; font-weight: bold;">
        <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
            data-iconpos="notext" class="ui-btn-right"></a>
        <label id="lbDlgMsg">
        </label>
    </div>
    <script type="text/javascript">
        var handlerUrl = '/Handler/App/CationHandler.ashx';
        $(function () {

        });
        function Save() {
            var model =
            {
                MasterID: $('#selectMaster').val(),
                FeedBackContent: $.trim($('#txtFeedBackContent').val()),
                Action: 'AddJuMasterFeedBack'
            }

            //alert(model.FeedBackContent);
            //            return;
            if (model.FeedBackContent == '') {
                $('#lbDlgMsg').html("内容不能为空!");
                $('#dlgMsg').popup("open");
                return;
            }

            //alert(model.FeedBackContent);

            $.mobile.loading('show', { textVisible: true, text: '正在处理。。。' });
            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: model,
                success: function (result) {
                    $.mobile.loading('hide');
                    var resp = $.parseJSON(result);
                    if (resp.Status == 1) {
                        $('#lbDlgMsg').html('提交成功！<br />正在返回问题列表。。。');
                        $('#dlgMsg').popup("open");
                        setTimeout(function () {
                            window.location.href = '/App/Cation/Wap/QuestionList.aspx';
                        }, 1000);
                    }
                    else {
                        $('#lbDlgMsg').html(resp.Msg);
                        $('#dlgMsg').popup("open");
                    }

                }
            });

        }
    </script>
</asp:Content>
