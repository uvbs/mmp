<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMainV1.Master" AutoEventWireup="true" CodeBehind="ActivityList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.ActivityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <section class="box">
    <ul class="mainlist" id="ullist"></ul>

</section>
<section class="navbar">
    <a href="UserHub.aspx" class="backbtn">
        <span class="icon"></span>
    </a>
    <a href="NewActivity.aspx" class="publish">
        <span class="icon"></span>组织我的活动
    </a>
</section>

 <script type="text/javascript">
     var PageIndex = 1; //第几页
     var PageSize = 5; //每页显示条数
     $(function () {
         document.title = "活动列表";

         LoadData();
         $("#btnNext").live("click", function () {
             PageIndex++;
             LoadData();

         });



     });

     function LoadData() {

         try {
             jQuery.ajax({
                 type: "Post",
                 url: "/Handler/App/CationHandler.ashx",
                 data: { Action: "QueryJuActivityForWap", page: PageIndex, rows: PageSize, ArticleType: "Activity"
                 },
                 dataType: "html",
                 success: function (result) {

                     if (PageIndex == 1) {
                         $("#ullist").html(result);
                     }
                     else {
                         if (result != "") {
                             $("#progressBar").before(result);

                         }
                         else {

                             $("#btnNext").html("没有更多了");
                             $("#progressBar").remove();
                         }


                     }


                 }
             })

         } catch (e) {
             MessageBox.show(e, 2, 3000);
         }

     }

    </script>
</asp:Content>