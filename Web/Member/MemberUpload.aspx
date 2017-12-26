<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="MemberUpload.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Member.MemberUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="PanelImport" runat="server">
        <table>
            <tr>
                <td>
                    从文件导入数据<asp:FileUpload ID="FileUpload1" runat="server" on_paste="return false" onkeydown="event.returnValue=false;"
                        Style="font-size: 13px" Width="330px" />
                    <asp:Button ID="btnUpload" runat="server" Text="上传" OnClick="btnUpload_Click" />
                    <asp:Label ID="lbUploadResult" runat="server" Text="Label" Visible="False"></asp:Label>
    </asp:Panel>
    </td> </tr> </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="box">
                <table>
                    <tr>
                        <td>
                            <font color="red">说明：<br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1. 支持从Excel文件(*.xls)导入数据<br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2. 支持Excel文件有多个Sheet<br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3. 支持智能字段匹配（自动识别Excel文件中列名“姓名”，“手机”，“电子邮件”，“电话”，
                                “QQ”，“Weibo”等，无须顺序排列）<br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 4. 最大支持4M文件大小。</font>
                        </td>
                    </tr>
                </table>
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
                                GridLines="None" AutoGenerateColumns="False">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="checkAll(this)" />
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckbqx" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="选择">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbMeeting" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />
                            </asp:TemplateField>--%>
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
                                    <asp:BoundField DataField="Company" HeaderText="公司">
                                        <HeaderStyle Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Title" HeaderText="职位">
                                        <HeaderStyle Width="80px" />
                                    </asp:BoundField>
                                    <%--<asp:BoundField DataField="Group" HeaderText="所属分组" />--%>
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
                            <hzh:BackBtn ID="BackBtn1" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
