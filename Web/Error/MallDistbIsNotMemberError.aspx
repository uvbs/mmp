<%@ Page Title="" Language="C#" MasterPageFile="~/Error/ErrorPage.Master" AutoEventWireup="true" CodeBehind="MallDistbIsNotMemberError.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Error.MallDistbIsNotMemberError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapCheckIn">
        <div class="signed">
            <div class="row icon-row">
                <div class="col icon-col">
                    <i class="icon iconfont icon-kulian kulian"></i>
                </div>
            </div>
            <div class="row tips-row">
                <div class="col tips-col">
                <br />
                     您还不是代言人 (<a class="colorRed " href="/customize/comeoncloud/Index.aspx?key=MallHome">点击链接成为代言人</a>)
                </div>
            </div>
        </div>

    </div>
</asp:Content>
