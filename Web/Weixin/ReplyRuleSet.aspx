<%@ Page Title="回复规则设置" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ReplyRuleSet.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.ReplyRuleSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updatePanelMain" runat="server">
        <ContentTemplate>
            <div id="box">
                <%-- <fieldset>
                    <legend>默认自动回复设置</legend>

                </fieldset>--%>
                <!--操作按钮开始-->
                <table id="tbOperater" class="tableNoBorder" width="100%">
                    <tr>
                        <td align="center">
                            <h3>
                                <asp:Button ID="btnCreateNew" runat="server" Text="保存规则" Font-Bold="True" Font-Size="14px"
                                    ForeColor="#727171" Visible="true" OnClick="btnCreateNew_Click" />
                                &nbsp;
                                <asp:Button ID="btnDelete" runat="server" Text="删除规则" Font-Bold="True" Font-Size="14px"
                                    ForeColor="#727171" Visible="true" onclick="btnDelete_Click" />
                            </h3>
                        </td>
                    </tr>
                </table>
                <!--操作按钮结束-->
                <asp:Panel ID="panelAdd" runat="server" Visible="true">
                    <fieldset>
                        <legend>关键词自动回复设置</legend>
                        <table style="width: 95%;">
                            <tr>
                                <td style="width: 80px;" class="td_label">
                                    关键字
                                </td>
                                <td style="width: 300px;">
                                    <asp:TextBox ID="txtKeyWord" runat="server" Width="90%"></asp:TextBox>
                                </td>
                                <td style="width: 80px;" class="td_label">
                                    匹配类型
                                </td>
                                <td style="width: 300px;">
                                    <asp:DropDownList ID="ddlMatchType" runat="server">
                                        <asp:ListItem Text="全文匹配"></asp:ListItem>
                                        <asp:ListItem Text="开始匹配"></asp:ListItem>
                                        <asp:ListItem Text="结尾匹配"></asp:ListItem>
                                        <asp:ListItem Text="包含匹配"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80px;" class="td_label">
                                    回复内容
                                </td>
                                <td style="width: *;" colspan="3">
                                    <asp:RadioButtonList ID="rdoListReplyType" runat="server" RepeatDirection="Horizontal"
                                        Height="26px" Width="294px" AutoPostBack="True" OnSelectedIndexChanged="rdoListReplyType_SelectedIndexChanged">
                                        <asp:ListItem Text="文本" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="图文"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:Panel ID="panelText" runat="server">
                                        <asp:TextBox ID="txtTextContent" runat="server" Rows="5" Width="500px" TextMode="MultiLine"></asp:TextBox>
                                    </asp:Panel>
                                    <asp:Panel ID="panelNews" runat="server" Visible="false">
                                        <table class="tableNoBorder" width="100%">
                                            <tr>
                                                <td style="width: 80px">
                                                    <span style="color:Red; font-weight:bolder;">*&nbsp; </span>选择图片
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="fileImg" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 80px">
                                                    
                                                </td>
                                                <td>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 80px">
                                                    <span style="color:Red; font-weight:bolder;">*&nbsp; </span>图片标题
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtImgTitle" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 80px">
                                                    <span style="color:Red; font-weight:bolder;">*&nbsp; </span>图片描述
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtImgDescription" runat="server" TextMode="MultiLine" Rows="5" Width="300px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 80px">
                                                    <span style="color:Red; font-weight:bolder;">*&nbsp; </span>跳转链接
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtImgNavUrl" runat="server" Width="300px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 80px; background-color:FBDA95">
                                                    
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAddImg" runat="server" Text="上传图片" 
                                                        onclick="btnAddImg_Click" />
                                                    <br />
                                                    <span style=" color:Red;">
                                                    注：<br />
                                                    1.图文消息个数，限制为10条以内；<br />
                                                    2.多条图文消息信息，默认第一个图片为大图；<br />
                                                    3.图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
                                                    </span>

                                                    <asp:GridView ID="grvTmpImgData" runat="server" Width="90%" 
                                                        AutoGenerateColumns="False" CellPadding="4" GridLines="None" 
                                                        ForeColor="#333333" onrowdeleting="grvTmpImgData_RowDeleting">
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="图片">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("PicUrl") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="Image1" runat="server" Height="80px" Width="80px" ImageUrl='<%# Eval("PicUrl") %>' />
                                                                </ItemTemplate>
                                                                
                                                                <ItemStyle Width="90px" />
                                                                
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Title" HeaderText="标题" >
                                                            <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" HeaderText="描述" />
                                                            <asp:BoundField DataField="Url" HeaderText="跳转链接" >
                                                            <ItemStyle Width="200px" />
                                                            </asp:BoundField>
                                                            <asp:CommandField ShowDeleteButton="True">
                                                            <ItemStyle Width="30px" />
                                                            </asp:CommandField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#990000" ForeColor="White" Font-Bold="True" />
                                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" BorderWidth="1px" />
                                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                                        <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                        <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                        <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                        <SortedDescendingHeaderStyle BackColor="#820000" />
                                                    </asp:GridView>

                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80px;" class="td_label">
                                </td>
                                <td style="width: 300px;">
                                    <asp:Button ID="btnSaveRule" runat="server" Text="保存" OnClick="btnSaveRule_Click" Visible="false" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnCancelRule" runat="server" Text="取消" OnClick="btnCancelRule_Click" Visible="false" />
                                </td>
                                <td style="width: 80px;" class="td_label">
                                </td>
                                <td style="width: 300px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80px;" class="td_label">
                                </td>
                                <td style="width: 300px;">
                                </td>
                                <td style="width: 80px;" class="td_label">
                                </td>
                                <td style="width: 300px;">
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:Panel>
                <asp:GridView ID="grvData" runat="server" Width="100%" AutoGenerateColumns="False" DataKeyNames="UID"
                    CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="20px" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="cbAll" runat="server" OnCheckedChanged="cbAll_CheckedChanged" AutoPostBack="true" />
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="cbRow" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MsgKeyword" HeaderText="关键字">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MatchType" HeaderText="匹配类型">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReplyContent" HeaderText="回复内容" />
                        <asp:BoundField DataField="ReplyType" HeaderText="回复类型">
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddImg" />
            <asp:PostBackTrigger ControlID="rdoListReplyType" />
            
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
