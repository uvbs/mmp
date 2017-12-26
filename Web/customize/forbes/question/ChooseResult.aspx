<%@ Page Title="" Language="C#" MasterPageFile="~/customize/forbes/question/Master.Master" AutoEventWireup="true" CodeBehind="ChooseResult.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.question.ChooseResult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="css/chooseResult.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<div class="wrapForbesChooseResult">
    <div class="medal">
        <img class="medalImg" src="images/medal.png" />
    </div>
<%--    <div class="scoreWord">
        本次答题得分是
    </div>
    <div class="score">
        89分
    </div>--%>
    <div class="satisfy">
        选择两次答题中你最满意的分数结果
    </div>
    <div class="chooseRegion">
        <div class="row">
            <div class="col selected" data-select="1">第一次<%=scoreFirst %>分</div>
            <%if (isShowSecond)
              {%>
                  
                  <div class="col" data-select="2">第二次<%=scoreSecond %>分</div>

              <%} %>
            
        </div>
    </div>
    <div class="chooseAnsBtn">
        <button id="btnShare" class="button button-block button-positive" >
            下一步
        </button>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script>

    var count = 1;
    $(function () {

        $("[data-select]").click(function () {

            $("[data-select]").removeClass("selected");
            $(this).addClass("selected");
            count = $(this).data("select");
        })

        $("#btnShare").click(function () {

            window.location.href = "Share.aspx?count="+count;

        })


    })
</script>

</asp:Content>
