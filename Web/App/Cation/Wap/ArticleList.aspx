<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMainV1.Master" AutoEventWireup="true" CodeBehind="ArticleList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.ArticleList" %>

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
    <a href="NewShare.aspx?cateId=<%=cateId %>" class="publish">
        <span class="icon"></span>发表我的分享
    </a>
</section>

 <script type="text/javascript">
     var RecommendCate = '<%=cateModel.Val1 %>';
     var ArticleTypeEx1 = 'hf_article';
     var PageIndex = 1; //第几页
     var PageSize = 5; //每页显示条数
     $(function () {
         document.title = "<%=cateModel.Val1%>";

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
                 data: { Action: "QueryArticleListForWap", page: PageIndex, rows: PageSize, RecommendCate: RecommendCate, ArticleTypeEx1: ArticleTypeEx1
                 },
                 dataType: "html",
                 success: function (result) {

                     //$.mobile.loading('hide');


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
