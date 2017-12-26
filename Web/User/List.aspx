<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="box">
                <fieldset>
                    <legend>用户列表</legend>
                    <table width="95%">
                        <tr>
                            <td>
                                用户ID:<asp:TextBox ID="txtSearchUserID" runat="server"></asp:TextBox>
                                <asp:Button ID="btnSearch" runat="server" Text="查找" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grvData" Width="100%" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None" AutoGenerateColumns="False" DataKeyNames="UserID" OnSelectedIndexChanging="grvData_SelectedIndexChanging">
                                    <AlternatingRowStyle BackColor="White" />
                                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                    <SortedDescendingHeaderStyle BackColor="#820000" />
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" />
                                        <asp:BoundField DataField="UserID" HeaderText="用户ID" SortExpression="UserID" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Password" HeaderText="登陆密码" SortExpression="Password"
                                            ItemStyle-HorizontalAlign="Center" Visible="False">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserType" HeaderText="用户类型" SortExpression="UserType"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TrueName" HeaderText="姓名" SortExpression="TrueName" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Company" HeaderText="公司名" SortExpression="Company" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Phone" HeaderText="手机号码" SortExpression="Phone" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Email" HeaderText="邮箱地址" SortExpression="Email" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Regtime" HeaderText="注册时间" SortExpression="Regtime" ItemStyle-HorizontalAlign="Center"
                                            Visible="False">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Points" HeaderText="短信点数" SortExpression="Points" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmailPoints" HeaderText="邮件点数" SortExpression="EmailPoints"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WeiboID" HeaderText="WeiboID" SortExpression="WeiboID"
                                            ItemStyle-HorizontalAlign="Center" Visible="False">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WeiboScreenName" HeaderText="微博昵称" SortExpression="WeiboScreenName"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WXNickname" HeaderText="微信昵称" SortExpression="WeiboScreenName"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WeiboAccessToken" HeaderText="微博授权" SortExpression="WeiboAccessToken"
                                            ItemStyle-HorizontalAlign="Center" Visible="False">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WeiboAccessStatus" HeaderText="授权状态" SortExpression="WeiboAccessStatus"
                                            ItemStyle-HorizontalAlign="Center" Visible="False">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PermissionGroupID" HeaderText="所属权限组ID" SortExpression="PermissionGroupID"
                                            ItemStyle-HorizontalAlign="Center" Visible="False">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" AlwaysShow="false" CssClass="paginator"
                                    CurrentPageButtonClass="cpb" CustomInfoTextAlign="Left" FirstPageText="首页" LastPageText="尾页"
                                    LayoutType="Table" NextPageText="下一页" NumericButtonCount="3" OnPageChanged="AspNetPager1_PageChanged"
                                    PageSize="15" PrevPageText="上一页" ShowCustomInfoSection="Left" ShowInputBox="Never">
                                </webdiyer:AspNetPager>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:Panel runat="server" ID="panelSet" Visible="false">
                    <fieldset>
                        <legend>用户组设置</legend>
                        <hzh:PmsGroupSelect runat="server" ID="wucGroup" />
                        <asp:LinkButton ID="lbtnSave" runat="server" CssClass="easyui-linkbutton" data-options="iconCls:'icon-edit'"
                            OnClick="lbtnSave_Click" Visible="false">更改权限组</asp:LinkButton>
                        <asp:Button ID="btnSavePmsGroup" runat="server" OnClick="lbtnSave_Click" Text="更改权限组" />
                    </fieldset>
                    <fieldset>
                        <legend>充值</legend>
                        <table class="tableNoBorder">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rdoReCharge" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rdoReCharge_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="短信" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="邮件"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_label" style="width: 120px;" align="left">
                                    当前余额:
                                </td>
                                <td class="td_value" align="left">
                                    <asp:Label ID="lbBalance" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_label" style="width: 120px;" align="left">
                                    充值点数:
                                </td>
                                <td class="td_value" align="left">
                                    <asp:TextBox ID="txtReCharge" runat="server" ValidationGroup="recharge"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                        ForeColor="Red" ControlToValidate="txtReCharge" ValidationGroup="recharge"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_label" style="width: 120px;" align="left">
                                </td>
                                <td class="td_value" align="left">
                                    <asp:Button ID="btnReCharge" runat="server" Text="充值" OnClick="btnReCharge_Click"
                                        ValidationGroup="recharge" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>短信发送通道</legend>
                        <br />
                        付费通道：<asp:DropDownList ID="ddlChargePipe" runat="server">
                        </asp:DropDownList>
                        发送通道：<asp:DropDownList ID="ddlSendPipe" runat="server">
                        </asp:DropDownList>
                        <asp:Button ID="btnAddUserPipeSet" runat="server" Text="添加到通道设置" OnClick="btnAddUserPipeSet_Click" />
                        <br />
                        <asp:GridView ID="grvUserPipeSet" runat="server" BackColor="White" DataKeyNames="UserID,UserPipe,SendPipe"
                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Width="100%"
                            AutoGenerateColumns="False" OnRowCreated="grvUserPipeSet_RowCreated" OnRowDeleting="grvUserPipeSet_RowDeleting">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            Text="删除"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="UserID" HeaderText="用户名">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserPipe" HeaderText="付费通道">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SendPipe" HeaderText="发送通道">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#9CC525" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </fieldset>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
