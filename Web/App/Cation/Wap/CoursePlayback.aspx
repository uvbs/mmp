<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMainV1.Master" AutoEventWireup="true" CodeBehind="CoursePlayback.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.CoursePlayback" %>

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
     var PageIndex = 1; //第几页
     var PageSize = 5; //每页显示条数
     $(function () {
         document.title = "精彩课程回放";

         //            $('body').bind('scrollstop', function (event) {
         //                //相关的滚动结束代码
         //                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
         //                var cha = ($(document).height() - totalheight);
         //                if (((cha <= 60) && ($("#btnNext").attr("isshow") == "yes"))) {
         //                    $("#btnNext").click();
         //                }
         //            });

         LoadData();
         $("#btnNext").live("click", function () {
             PageIndex++;
             LoadData();

         });



     });


     function LoadData() {

         try {
             //$.mobile.loading('show');
             jQuery.ajax({
                 type: "Post",
                 url: "/Handler/App/CationHandler.ashx",
                 data: { Action: "QueryArticleListForWap", page:
PageIndex, rows: PageSize, ArticleTypeEx1: 'hf_course', RecommendCate: '精彩课程回放'
                 },
                 dataType: "html",
                 success: function (result) {

                     // $.mobile.loading('hide');


                     if (PageIndex == 1) {
                         $("#ullist").html(result);
                     }
                     else {
                         if (result != "") {
                             $("#progressBar").before(result);

                         }
                         else {

                             //$("#btnNext").attr("isshow", "no");
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
