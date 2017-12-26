<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="CardCode.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.CardCode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<section class="box">
    <div class="m_codebox">
        <div class="code">
            <img id="imgcarcode" src="" class="codepic">
            <h2 class="text">扫描二维码，尊享会员特权</h2>
            <a href="Card.aspx"><span class="btn orange">返回</span></a>
        </div>
    </div>

    <div class="backbar">
        <a href="Card.aspx" class="back"><span class="icon"></span></a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">

    var handlerUrl = "/Handler/QCode.ashx";
    var code = "<%=QCode.ToString()%>";
    $(function () {

        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { code: code },
            success: function (result) {
                $("#imgcarcode").attr("src", result);
            }
        });


    });

</script>
</asp:Content>
