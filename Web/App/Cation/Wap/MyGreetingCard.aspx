<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapUser.Master" AutoEventWireup="true" CodeBehind="MyGreetingCard.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.MyGreetingCard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">
        var GreetingCardPageIndex = 1; //文章第几页
        var GreetingCardPageSize = 5; //文章每页显示条数
        $(function () {
            $('body').bind('scrollstop', function (event) {
                //相关的滚动结束代码
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                var cha = ($(document).height() - totalheight);
                if (((cha <= 60) && ($("#btnNextGreetingCard").attr("isshow") == "yes"))) {
                    $("#btnNextGreetingCard").click();
                }

            });
            LoadDataGreetingCard();
            $("#btnNextGreetingCard").live("click", function () {
                GreetingCardPageIndex++;
                LoadDataGreetingCard();

            });

        });
        //加载贺卡列表
        function LoadDataGreetingCard() {
            try {
                $.mobile.loading('show', { textVisible: true, text: '正在加载...' });
                jQuery.ajax({
                    type: "Post",
                    url: "/Handler/JuActivity/JuActivityHandler.ashx",
                    data: {
                        Action: "GetJuActivityInfoList", ArticleType: "greetingcard", page:
    GreetingCardPageIndex,rows: GreetingCardPageSize
                    },
                    dataType: "html",
                    timeout: 60000,
                    success: function (result) {
                        $.mobile.loading('hide');
                        if (GreetingCardPageIndex == 1) {
                            $("#ulGreetingCard").html(result);
                        }
                        else {
                            if (result != "") {
                                $("#progressBarGreetingCard").before(result);
                            }
                            else {
                                $("#btnNextGreetingCard").attr("isshow", "no");
                                $("#btnNextGreetingCard").hide();
                                $("#progressBarGreetingCard").remove();
                            }
                        }
                    },
                    error: function () {
                        $.mobile.loading('hide');
                        if (GreetingCardPageIndex > 1) {
                            GreetingCardPageIndex--;
                        }
                        //alert("网络超时，请重试");
                    }
                })

            } catch (e) {
                alert(e);
            }

        }
    </script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div data-role="page" id="page-title" data-theme="b">
        <div data-role="header" data-theme="f" data-position="fixed" style="" id="divTop">
           
            <a href="UserHub.aspx" data-icon="home" data-ajax="false">主页</a>
            <h1>
                新年贺卡
            </h1>
            <a href="/App/Cation/Wap/NewGreetingCard.aspx" data-ajax="false" data-role="button" data-inline="true" data-mini="true" data-icon="Plus">新建贺卡</a>
        </div>
       <div id="divGreetingCard" >
            <ul data-role="listview" data-split-icon="gear" data-split-theme="d" data-inset="true"

                id="ulGreetingCard">
            </ul>
        </div>

    </div>
</asp:Content>
