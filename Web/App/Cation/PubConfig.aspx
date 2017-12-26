<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="PubConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.PubConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        input
        {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
        textarea
        {
            height: 35px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
        
        h1
        {
            margin-left: 50px;
        }
        .button-rounded
        {
            margin-left: 50px;
        }
        .centent_r_btm
        {
            min-height: 600px;
        }
        .button-group
        {
            margin-left: 200px;
        }
        #tbAutoAuth
        {
            margin-top: 20px;
        }
        .hide {
            display:none;
        }
        .title {
            font-weight:bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微信公众号&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>公众号接入</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div class="button-group">
            <button type="button" class="button button-primary" id="btnManual">
                自动设置</button>
            <button type="button" class="button button-primary" id="btnAutoAuth">
                手动填写</button>
        </div>
        <table width="100%" id="tbAutoAuth">
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    <h2>
                        </h2>
                </td>
                <td style="width: *;height:100%;" align="left" valign="middle" >
                <iframe width="1000" height="1000"  frameborder="no" border="0" src="http://<%=OauthDomain%>/weixinopen/oauthbutton.aspx?websiteowner=<%=currentWebsiteInfo.WebsiteOwner %>" >
                
                </iframe>
                    
                </td>
            </tr>
        </table>
        <table width="100%" id="tbManual"  style="display: none;">
            <tr>
                <td style="width: 200px;" align="right" valign="middle">
                    <h2>
                        手动填写:</h2>
                </td>
                <td style="width: *;" align="left" valign="middle">
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle" class="title">
                    URL(服务器地址)：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <%=string.Format("http://{0}/Weixin/OAuthPage.aspx?u={1}",
                                 !string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host,
                                ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(user.UserID)
                            )
                    %>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle" class="title">
                    Token(令牌)：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtToken" class="" style="width: 350px;" value="<%=user.WeixinToken %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle" class="title">
                    AppID(应用ID)：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtAppId" class="" style="width: 350px;" value="<%=user.WeixinAppId %>" />
                </td>
            </tr>
            <tr>
                <td style="width: 200px;" align="right" valign="middle" class="title">
                    AppSecret(应用密钥)：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtAppSecret" class="" style="width: 350px;" value="<%=user.WeixinAppSecret %>" />
                </td>
            </tr>
            <tr class="hide">
                <td style="width: 200px;" align="right" valign="middle">
                    被添加字段回复关键字：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <input type="text" id="txtSubscribeKeyWord" class="" style="width: 350px;" value="<%=user.SubscribeKeyWord %>" />
                </td>
            </tr>
            <tr class="hide">
                <td style="width: 200px;" align="right" valign="top">
                    消息自动回复内容：
                </td>
                <td style="width: *;" align="left" valign="middle">
                    <textarea style="width: 350px; height: 100px" id="txtWellcomeReplyContent"><%=menuModel ==null ? "": menuModel.ReplyContent%></textarea>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" valign="middle">
                    <br />
                    <a href="javascript:;" style="width: 200px; margin-left: 200px;" id="btnSave" onclick="Save();"
                        class="button button-rounded button-primary">保存</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        function Save() {
            var dataModel = {
                Action: 'SetPubConfig',
                WeixinToken: $.trim($(txtToken).val()),
                WeixinAppId: $.trim($(txtAppId).val()),
                WeixinAppSecret: $.trim($(txtAppSecret).val())
                //WellcomeReplyContent: $.trim($(txtWellcomeReplyContent).val()),
                //SubscribeKeyWord: $("#txtSubscribeKeyWord").val()
            }

            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: dataModel,
                dataType: "json",
                success: function (resp) {
                    Alert(resp.Msg);
                }
            });

        }


        $(function () {

            $("#btnAutoAuth").click(function () {

                
                 $(tbManual).show();
                $(tbAutoAuth).hide();

            });

            $("#btnManual").click(function () {


                $(tbAutoAuth).show();
                $(tbManual).hide();



            });

        })
    </script>
</asp:Content>
