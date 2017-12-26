<%@ Page Title="" Language="C#" MasterPageFile="~/customize/forbes/question/Master.Master" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.question.Result" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
 <link href="css/result.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
<div class="wrapForbesResult">
    <div class="medal">
        <img class="medalImg" src="images/medal.png" />
    </div>
    <div class="scoreWord">
        本次答题得分是
    </div>
    <div class="score">
        <%=Score %>分
    </div>
    <div class="satisfy">
        满意本次分数
    </div>
    <div class="nextBtn">
        <button id="nextStep" class="button button-block button-positive" onclick="window.location.href='ChooseResult.aspx'">
            下一步
        </button>
    </div>

    <%if (isShowSencondQuestion)
      {%>
          <div class="satisfy">
        不满意本次分数
    </div>
              <div class="moreBtn">
        <button id="moreTest" class="button button-block button-positive" onclick="window.location.href='Question.aspx?count=2'">
            再做一套题
        </button>
    </div>


      <%} %>


</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
</asp:Content>
