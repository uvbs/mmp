<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="MemberList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Member.MemberList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="box">
                <h3>
                    &nbsp;&nbsp;&nbsp;&nbsp;</h3>
                <h3>
                    <table>
                        <tr align="center">
                            <%-- <td>
                                <asp:LinkButton ID="lbtnAdd" runat="server" Font-Underline="True">新增</asp:LinkButton>
                            </td>--%>
                            <td>
                                <asp:LinkButton ID="lbtnBatchImport" runat="server" Font-Underline="True" OnClick="lbtnBatchImport_Click">批量导入</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnAdd" runat="server" Font-Underline="True" OnClick="lbtnAdd_Click">增加</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnDelete" runat="server" Font-Underline="True" OnClick="lbtnDelete_Click">删除</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnSearch" runat="server" Font-Underline="True" OnClick="lbtnSearch_Click">按分组筛选</asp:LinkButton>
                            </td>
                            <%--  <td>
                                <asp:LinkButton ID="lbtnGroup" runat="server" Font-Underline="True">设置分组</asp:LinkButton>
                            </td>--%>
                            <%--<td>
                                <asp:LinkButton ID="lbtnSendSMS" runat="server" Font-Underline="True" OnClick="lbtnSendSMS_Click">发送短信</asp:LinkButton>
                            </td>--%>
                        </tr>
                    </table>
                </h3>
                <table width="100%">
                    <tr>
                        <td valign="top">
                            <fieldset>
                                <legend>数据操作</legend>
                                <table>
                                    <tr>
                                        <td width="100px" align="right">
                                            发送短信
                                        </td>
                                        <td width="*" align="left">
                                            <asp:DropDownList ID="ddlSetSendSMS" runat="server" Font-Size="12px">
                                                <asp:ListItem Text="将勾选结果发送短信" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="将筛选结果发送短信" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnSendSMS" runat="server" Text="确认发送" Font-Size="12px" OnClick="lbtnSendSMS_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="25" width="100px" align="right">
                                            设置分组
                                        </td>
                                        <td height="25" width="*" align="left">
                                            <asp:DropDownList ID="ddlSetGroup" runat="server" Font-Size="12px">
                                                <asp:ListItem Text="将勾选结果设置为" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="将筛选结果设置为" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlGroup" runat="server" Font-Size="12px">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnSaveSetGroup" runat="server" Text="保存设置" Font-Size="12px" OnClick="btnSaveSetGroup_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <%--    <tr>
                <td height="25" width="30%" align="right">
                    
                </td>
                <td height="25" width="*" align="left">
                    
                </td>
            </tr>--%>
                                <tr>
                                    <td height="25" colspan="2" width="*" align="left">
                                        <asp:GridView ID="grvData" Width="100%" runat="server" CellPadding="4" ForeColor="#333333"
                                            DataKeyNames="MemberID" GridLines="None" AutoGenerateColumns="False" OnSelectedIndexChanged="grvData_SelectedIndexChanged">
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
                                                <%--<asp:TemplateField HeaderText="选择">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbMeeting" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                            </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="序号" InsertVisible="False">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Name" HeaderText="姓名">
                                                    <HeaderStyle Width="80px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Mobile" HeaderText="手机">
                                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Email" HeaderText="电子邮件">
                                                    <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="QQ" HeaderText="QQ" />
                                                <asp:BoundField DataField="Company" HeaderText="公司">
                                                    <HeaderStyle Width="120px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Title" HeaderText="职位">
                                                    <HeaderStyle Width="80px" />
                                                </asp:BoundField>
                                                <%--<asp:BoundField DataField="Group" HeaderText="所属分组" />--%>
                                                <asp:BoundField DataField="GroupName" HeaderText="分组" />
                                            
                                        <asp:ImageField AlternateText="无图发送" DataImageUrlField="CardImageUrl" HeaderText="图片">
                                        <ControlStyle Height="100px" Width="100px" />
                                        <ItemStyle Height="100px" Width="100px" />
                                    </asp:ImageField>

                                                  <asp:BoundField DataField="Remark" HeaderText="备注">
                                                    <HeaderStyle Width="80px" />
                                                </asp:BoundField>


                                                <asp:HyperLinkField HeaderText="修改" DataNavigateUrlFields="MemberID" DataNavigateUrlFormatString="Modify.aspx?MemberID={0}"
                                                    Text="修改" />
                                            </Columns>
                                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                            <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                            <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                            <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                            <SortedDescendingHeaderStyle BackColor="#820000" />
                                        </asp:GridView>
                                        <br />
                                        <webdiyer:AspNetPager ID="AspNetPager1" CssClass="paginator" CurrentPageButtonClass="cpb"
                                            PageSize="30" runat="server" AlwaysShow="false" FirstPageText="首页" LastPageText="尾页"
                                            NextPageText="下一页" PrevPageText="上一页" ShowCustomInfoSection="Left" ShowInputBox="Never"
                                            CustomInfoTextAlign="Left" LayoutType="Table" OnPageChanged="AspNetPager1_PageChanged"
                                            NumericButtonCount="3">
                                        </webdiyer:AspNetPager>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 200px" align="left" valign="top">
                            <div style="overflow: auto; display: block; width: 200px; height: 400px;">
                                <fieldset>
                                    <legend>客户分组</legend>
                                    <hzh:MemberGroupSelect runat="server" ID="wucMemberGroup" GroupType="1" />
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
