<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Analysis.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.test.data.Analysis" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--<link href="/lib/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" />--%>
</head>
<body>
    <div class="container" style="padding: 50px;">
        <form id="form1" runat="server">
           <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                
                <ContentTemplate>--%>


                    <div>
                        项目编号：
            <asp:TextBox ID="txtTpObjId" runat="server"></asp:TextBox>
                        选手编号：
            <asp:TextBox ID="txtId" runat="server"></asp:TextBox>
                        <asp:Button ID="btnStart" runat="server" Text="start" OnClick="btnStart_Click" />

                        <asp:Button ID="btnCreateSubDomain" runat="server" Text="创建子域名" OnClick="btnCreateSubDomain_Click" />

                        <asp:Button ID="Button1" runat="server" Text="加密" OnClick="Button1_Click" />
                        <asp:Button ID="Button2" runat="server" Text="创建订单测试" OnClick="Button2_Click" />
                        <asp:Button ID="Button3" runat="server" Text="创建订单测试2" OnClick="Button3_Click" />
                    </div>
                    <fieldset>
                        <legend><%=number > 0? number+"号选手":"" %>票数统计</legend>
                        <asp:GridView ID="grvMonth" runat="server" OnRowCommand="grvMonth_RowCommand" OnSelectedIndexChanged="grvMonth_SelectedIndexChanged" OnSelectedIndexChanging="grvMonth_SelectedIndexChanging">

                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                            </Columns>

                        </asp:GridView>
                    </fieldset>

                    <fieldset>
                        <legend><%=currMonth > 0? currMonth+"月":"" %>票数统计</legend>
                        <asp:GridView ID="grvDay" runat="server" OnSelectedIndexChanging="grvDay_SelectedIndexChanging">
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                            </Columns>
                        </asp:GridView>
                    </fieldset>

                    <fieldset>
                        <legend><%=currMonth > 0? currMonth+"月":"" %><%=currDay > 0? currDay+"日":"" %>票数统计</legend>
                        <div class="row">
                            <div class="col-xs-4" style="float:left">
                                <asp:GridView ID="grvHour" runat="server" OnSelectedIndexChanging="grvHour_SelectedIndexChanging" Width="200px">
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="col-xs-8" style="float:left; margin-left:30px">
                                <asp:GridView ID="grvDayDetail" runat="server" Width="500px"></asp:GridView>
                            </div>
                        </div>
                    </fieldset>
                <%--</ContentTemplate>
                
            </asp:UpdatePanel>--%>
        </form>
    </div>
</body>
</html>
