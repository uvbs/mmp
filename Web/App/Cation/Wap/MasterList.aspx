<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMainV1.Master" AutoEventWireup="true" CodeBehind="MasterList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.MasterList" %>
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

</section>
 <script type="text/javascript">
     var PageIndex = 1;
     var PageSize = 5;

     $(function () {

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
                 data: { Action: "QueryJuMaster", page: PageIndex, rows: PageSize },
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
