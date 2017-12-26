<%@ Page Title="抽奖活动详情" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.LuckDraw.wap.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .btnAdd{
                margin: 30px 30px 0px 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

   <article class="weui-article">
    <h1><%=lottery.LotteryName %></h1>
    <section>
      
        <section>
            <p>
                <%=lottery.LotteryContent %>.
            </p>
        </section>
    </section>
</article>
    <div class="button-sp-area btnAdd">
         <a href="javascript:;" class="weui-btn weui-btn_plain-primary">加入抽奖</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">


    <script type="text/javascript">

        var lotteryId = '<%=lottery.LotteryID%>';

        $(function () {

            $('.btnAdd').click(function () {
                window.location.href = '/app/luckdraw/wap/join.aspx?lotteryId=' + lotteryId;
            });
        });

    </script>
</asp:Content>
