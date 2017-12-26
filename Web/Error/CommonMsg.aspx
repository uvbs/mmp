<%@ Page Title="" Language="C#" MasterPageFile="~/Error/ErrorPage.Master" AutoEventWireup="true" CodeBehind="CommonMsg.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Error.CommonMsg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapCheckIn">
        <div class="signed">
            <div class="row icon-row">
                <div class="col icon-col">
                    <i class="icon iconfont kulian <%=icon%>"></i>
                </div>
            </div>
            <div class="row tips-row">
                <div class="col tips-col">
                    <br />
                        <%=msg %>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
