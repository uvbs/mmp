<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMain.Master" AutoEventWireup="true"
    CodeBehind="SignUp.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div data-role="page" id="page-title" data-theme="b">
        <div data-role="header" data-theme="b" data-position="fixed" style="" id="divTop">
            <h1>
                在线报名</h1>
        </div>
        <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel(); %>
        <table align="center" width="320px">
            <tr>
                <td style="width: 52px;">
                    <%
                        if (currUser != null)
                        {
                            if (string.IsNullOrWhiteSpace(currUser.WXHeadimgurl))
                                Response.Write(@"<img alt=""图片未显示"" src=""/img/offline_user.png"" width=""52"" height=""52""  style=""border-radius:50px;"" />");
                            else
                                Response.Write(string.Format(@"<img alt=""图片未显示"" src=""{0}"" width=""52"" height=""52""  style=""border-radius:50px;"" />", currUser.WXHeadimgurlLocal));
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
        <div id="divSignUp">
            <div data-role="fieldcontain">
                <label for="txtName">
                    姓名:</label>
                <input type="text" name="Name" id="txtName" placeholder="请输入您的真实姓名" value="">
            </div>
            <div data-role="fieldcontain">
                <label for="txtPhone">
                    手机号码:</label>
                <input type="text" name="Phone" id="txtPhone" placeholder="请输入您的手机号码" value="">
            </div>
            <div data-role="fieldcontain">
                <label for="txtEmail">
                    邮件地址:</label>
                <input type="text" name="Email" id="txtEmail" placeholder="请输入您的邮件地址" value="">
            </div>
            <div data-role="fieldcontain">
                <label for="txtCompany">
                    公司名称:</label>
                <input type="text" name="Company" id="txtCompany" placeholder="请输入您的公司名称" value="">
            </div>
            <div data-role="fieldcontain">
                <label for="txtPostion">
                    职位:</label>
                <input type="text" name="Postion" id="txtPostion" placeholder="请输入您的职位" value="">
            </div>
            <div data-role="fieldcontain">
                <label for="txtPostion">
                    备注:</label>
                <textarea style="width: 100%; height: 100px;" id="txtMemo" placeholder="请输入备注信息"></textarea>
            </div>
            <a href="#" data-role="button" data-inline="false" data-theme="f" id="btnSave" data-ajax="false"
                data-mini="false">提交报名信息</a>
        </div>
        <div id="divOtherMsg" style="padding-top: 30px; font-size: large; font-weight: bold;
            width: 100%; text-align: center;">
        </div>
        <div data-role="popup" id="dlgMsg" style="padding: 20px; text-align: center; font-weight: bold;">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
            <label id="lbDlgMsg">
            </label>
        </div>
    </div>
    <%  %>
    <script type="text/javascript">
        var PageActionType = '<%=PageActionType %>';
        $(function () {
            //判断当前用户资料是否正在审核中、或者已经是正式用户

            document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
                WeixinJSBridge.call('hideOptionMenu');
            });

            if (PageActionType == '1') {
                $('#divSignUp').hide();
                $('#divOtherMsg').html("报名资料已提交，正在审核中！");
                return;
            }

            if (PageActionType == '2') {
                $('#divSignUp').hide();
                $('#divOtherMsg').html("您已经是正式学员了，无需再报名！");
                return;
            }

            if (PageActionType == '3') {
                $('#divSignUp').hide();
                $('#divOtherMsg').html("您是教师，无需报名！");
                return;
            }

            $('#btnSave').click(function () {
                var model = {
                    ActivityID: '<%=currWebSiteModel.SignUpActivityID %>',
                    loginName: '<%=signUpLoginName %>',
                    loginPwd: '<%=signUploginPwd %>',
                    WXCurrOpenerOpenID: '<%=currUser.WXOpenId %>',
                    Name: $.trim($('#txtName').val()),
                    Phone: $.trim($('#txtPhone').val()),
                    K1: $.trim($('#txtEmail').val()), //邮箱
                    K2: $.trim($('#txtCompany').val()), //公司
                    K3: $.trim($('#txtPostion').val()), //职位
                    K4: $.trim($('#txtMemo').val())//备注
                }

                if (model.Name == '') {
                    MShowMsg('请输入姓名！');
                    return;
                }
                if (model.Phone == '') {
                    MShowMsg('请输入手机！');
                    return;
                }
                if (model.K1 == '') {
                    MShowMsg('请输入邮箱！');
                    return;
                }
                if (model.K2 == '') {
                    MShowMsg('请输入公司！');
                    return;
                }
                if (model.K3 == '') {
                    MShowMsg('请输入职位！');
                    return;
                }
                $.mobile.loading('show', { textVisible: true, text: '正在提交...' });
                $.ajax({
                    url: '/serv/ActivityApiJson.ashx',
                    type: 'post',
                    data: model,
                    dataType: "json",
                    success: function (resp) {
                        if (resp.Status == 0) {
                            $.mobile.loading('hide');
                            //MShowMsg(resp.Msg);
                            //window.location.href = '/App/Cation/wap/signup.aspx';
                            $('#divSignUp').hide();
                            $('#divOtherMsg').html("报名资料已提交，正在审核中！");
                        }
                        else {
                            $.mobile.loading('hide');
                            MShowMsg(resp.Msg);
                        }

                    }
                });

                //alert(model.WXCurrOpenerOpenID);

            });
        });

        function MShowMsg(msg) {
            $('#lbDlgMsg').html(msg);
            $('#dlgMsg').popup();
            $('#dlgMsg').popup('open');
        }
    </script>
</asp:Content>
