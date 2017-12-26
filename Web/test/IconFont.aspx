<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IconFont.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.test.IconFont" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
    .icon {
       width: 1em; height: 1em;
       vertical-align: -0.15em;
       fill: currentColor;
       overflow: hidden;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="FileUpload1" runat="server"  />
        <asp:Button ID="Button1" runat="server" Text="生成最新图标文件" OnClick="Button1_Click" />
    </div>
    <div style="width:100px;height:100px;">
        <svg class="icon" aria-hidden="true">
            <use xlink:href="#icon-510"></use>
        </svg>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="//at.alicdn.com/t/font_7ocsa7yc5cihehfr.js"></script>