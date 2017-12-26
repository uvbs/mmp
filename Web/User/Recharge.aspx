<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Recharge.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.User.Recharge" %>
<%@ Register src="../Control/wucCheckRight.ascx" tagname="wucCheckRight" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc1:wucCheckRight ID="wucCheckRight1" runat="server" Pms = "1" />
  <div id="box">
        <h3>
            用户充值
        </h3>
    </div>
</asp:Content>
