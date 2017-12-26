<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Trave/Master.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Trave.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
首页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <!--Page 1 content-->
    <div class="pages_container">
        <div class="image_single">
            <img src="images/index_01.png" alt="" title="" border="0" />
        </div>
        <div class="list3">
            <div class="page_padding10">
                <div class="image_single">
                    <img src="images/code.png" alt="" title="" border="0" />
                    <img src="images/code_msg.png" alt="" title="" border="0" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    //分享
    var shareTitle = "中青旅遨游网，寻找小小旅行家，境外亲子游大奖等你来赢!";
    var shareDesc = "晒宝贝旅行靓照，分享旅途趣闻，赢取境外亲子游等丰厚大奖！";
    var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Trave/images/logo.png";
    var shareLink = window.location.href;
    //分享
</script>
</asp:Content>
