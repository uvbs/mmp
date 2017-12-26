<%@ Page Title="配置公众号" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="SetPubID.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.SetPubID" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script src="/Scripts/ajaxfileupload2.1.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        if ($.browser.msie) { //ie 下
            $("#btnThumbnails").hide();
            $("#txtThumbnailsPath").show();
        }
        else {
            $("#btnThumbnails").show();
            $("#txtThumbnailsPath").hide();

        }


        $("#txtThumbnailsPath").live('change', function () {
            try {
                $.messager.progress({ text: '正在上传图片。。。' });

                $.ajaxFileUpload(
                     {

                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=WXLogoImage',
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

                                 //wxlogoimg.src = resp.ExStr;
                                 $("#wxlogoimg").attr("src", resp.ExStr);
                                 // $('#imgThumbnailsPath').attr('path', resp.ExStr);
                                 //保存图片
                                 $.ajax({
                                     type: 'post',
                                     url: "/Handler/WeiXin/WeixinHandler.ashx",
                                     data: { Action: "SetWXLogoImage", FilePath: resp.ExStr },
                                     success: function (result) {
                                         var resp = $.parseJSON(result);
                                         if (resp.Status == 1) {
                                             alert(resp.Msg);
                                         }
                                         else {
                                             alert(resp.Msg);
                                         }


                                     }
                                 });
                             }
                             else {
                                 alert(resp.Msg);
                             }
                         }
                     }
                    );

            } catch (e) {
                alert(e);
            }
        });
    })
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updatePanelMain" runat="server">
        <ContentTemplate>
            <div id="box">
                <!--操作按钮开始-->
                <table id="tbOperater" class="tableNoBorder" width="100%">
                    <tr>
                        <td align="center">
                            <h3>
                                <asp:Button ID="btnSave" runat="server" Text="保存设置" Font-Bold="True" Font-Size="14px"
                                    ForeColor="#727171" Visible="true" onclick="btnSave_Click" />
                            </h3>
                        </td>
                    </tr>
                </table>
                <!--操作按钮结束-->
                <fieldset>
                    <legend>接口配置信息</legend>
                    <table style="width: 95%;">
                        <tr>
                            <td style="width: 120px; height: 30px;">
                                公众号名称
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeinxinPublicName" runat="server" Width="200px"></asp:TextBox>
                                <span style="color: Red;">公众号名称跟短信签名等功能绑定</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 30px;">
                                URL
                            </td>
                            <td>
                                <asp:Label ID="lbURL" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 30px;">
                                Token
                            </td>
                            <td>
                                <asp:TextBox ID="txtToken" runat="server" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 30px;">
                            </td>
                            <td>
                                <%--<asp:Button ID="btnApiSet" runat="server" Text="生成URL" OnClick="btnApiSet_Click" />--%>
                                <span style="color: Red;">说明:填写Token保存后，将上面的URL和Token到公众平台开发模式配置即可完成配置接入；</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 30px;">
                                微信认证
                            </td>
                            <td>
                               
                                <asp:RadioButtonList ID="rblIsWeixinVerify" runat="server" 
                                    RepeatDirection="Horizontal" Width="150px">
                                    <asp:ListItem Value="1">已认证</asp:ListItem>
                                    <asp:ListItem Value="0">未认证</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 30px;">
                                启用自定义菜单
                            </td>
                            <td>
                               
                                <asp:RadioButtonList ID="rblEnableMenu" runat="server" 
                                    RepeatDirection="Horizontal" Width="110px">
                                    <asp:ListItem Value="1">启用</asp:ListItem>
                                    <asp:ListItem Value="0">不启用</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                             <tr>
                            <td style="width: 120px; height: 30px;">
                                AppId
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppId" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 30px;">
                                AppSecret
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppSecret" runat="server" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                                                <tr>
                            <td style="width: 120px; height: 30px;">
                                模块图片:
                            </td>
                            <td>
                               <img id="wxlogoimg" src="<%=user.WXLogoImg%>" width="300" height="150"/>
                               <br />
                               
                                <input id="btnThumbnails" type="button" value="上传图片" onclick="txtThumbnailsPath.click()"/>
                                <br />
                                <img src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为300*150。
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                            </td>
                        </tr>

                    </table>
                </fieldset>
                <% ZentCloud.BLLJIMP.Model.UserInfo userModel = ZentCloud.JubitIMP.Web.Comm.DataLoadTool.GetCurrUserModel(); %>
                <%if (userModel.WeixinIsOpenReg != null && userModel.WeixinIsOpenReg == 1)
                  {%>
                <fieldset>
                    <legend>注册回复设置</legend>
                    <table style="width: 95%;">
                        <tr>
                            <td style="width: 120px; height: 30px;">
                                注册关键字
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegKeyWord" runat="server" Width="200px" Text="zc"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <%} %>
                <fieldset>
                    <legend>菜单设置</legend>
                    <table style="width: 95%;">
                        <tr>
                            <td style="width: 120px; height: 30px;">
                                菜单关键字
                            </td>
                            <td>
                                <asp:TextBox ID="txtMenuKeyWord" runat="server" Width="200px" Text="m"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 30px;" valign="top">
                                菜单内容
                            </td>
                            <td>
                                <asp:TextBox ID="txtMenuContent" runat="server" Width="400px" Text="欢迎进入微信公众平台！
1.输入【注册】进行注册;
2.输入【m】可返回菜单;" TextMode="MultiLine" Rows="20"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <%--  <fieldset>
                    <legend>公众号详细信息</legend>
                    <table style="width: 95%;">
                        <tr>
                            <td style="width: 120px;">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </fieldset>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
