<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetPaymentList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.test.efast.GetPaymentList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnGetSku" runat="server" Text="获取sku" OnClick="btnGetSku_Click" />
        <asp:Button ID="btnGetColorList" runat="server" Text="同步颜色档案" OnClick="btnGetColorList_Click" />
        <asp:Button ID="btnGetSizeList" runat="server" Text="同步尺码档案" OnClick="btnGetSizeList_Click" />
        <asp:Button ID="btnGetGoodsList" runat="server" Text="同步商品档案" OnClick="btnGetGoodsList_Click" />
        <asp:Button ID="btnGetSkuAndStock" runat="server" Text="同步SKU及库存" OnClick="btnGetSkuAndStock_Click" />
        <asp:Button ID="btnCreateOrder" runat="server" Text="创建订单" OnClick="btnCreateOrder_Click"/>
        <asp:Button ID="btnCancelOrder" runat="server" Text="取消订单" OnClick="btnCancelOrder_Click"/>
        <asp:Button ID="btnGetShipping" runat="server" Text="查看收货状态和快递信息" OnClick="btnGetShipping_Click"/>
        <asp:Button ID="btnChangeBrouns" runat="server" Text="积分变更" OnClick="btnChangeBrouns_Click"/>
        <br />
        <br />
        <asp:TextBox ID="txtInput" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtInput2" runat="server"></asp:TextBox>
        <br />
        <br />
    <div>
        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
    </div>
    </form>
</body>
</html>
