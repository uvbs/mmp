<%@ Page Title="问题列表" Language="C#" MasterPageFile="~/Master/WapMain.Master" AutoEventWireup="true" CodeBehind="QuestionList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.QuestionList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%string justMe = Request["justMe"]; %>
    <div data-role="page">
        <div data-role="header" data-theme="b" data-position="fixed" style="">
            <%if (!string.IsNullOrWhiteSpace(justMe))
              { %>
            <a href="#" data-role="button" data-rel="back" data-icon="arrow-l">返回</a>
            <%} %>
            <h1>
                问题列表</h1>
            <div style="font-size: 12px" align="center">
                注：您可以点击问题进入回复页面</div>
        </div>
        <div id="divfeedbacklist" style="margin-left: 5px; margin: 5px;">
        </div>
        <div data-role="footer" data-theme="c" data-position="fixed">
            <div data-role="navbar" data-theme="f">
                <ul>
                    <li><a href="#" id="btnAllFeedBack">全部问题</a></li>
                    <li><a href="#" id="btnHaveReply">已回复</a></li>
                    <li><a href="#" id="btnUnReply">未回复</a></li>
                </ul>
            </div>
            <a href="javascript:window.location.href='QuestionSubmit.aspx';" data-role="button"
                inline="false" data-mini="false" data-theme="f" id="btnLogin" data-transition="flip"
                style="width: 100%;">我要提交问题</a>
        </div>
        <div data-role="popup" id="dlgMsg" style="padding: 20px; text-align: center; font-weight: bold;">
            <a href="#" data-rel="back" data-role="button" data-theme="a" data-icon="delete"
                data-iconpos="notext" class="ui-btn-right"></a>
        </div>
    </div>
    <script type="text/javascript">
        //var MasterID = "1";
        var FeedBackStatus = ""
        var PageIndex = 1; //第几页
        var PageSize = 10; //每页显示条数
        $(function () {
            Search();
            $('body').bind('scrollstop', function (event) {
                //相关的滚动结束代码
                totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
                var cha = ($(document).height() - totalheight);
                if (((cha <= 60) && ($("#btnNext").attr("isshow") == "yes"))) {
                    $("#btnNext").click();

                }
            });
            $("#btnNext").live("click", function () {
                PageIndex++;
                Search();

            });
            $("#btnAllFeedBack").live("click", function () {//全部回复

                PageIndex = 1;
                FeedBackStatus = "";
                Search();

            });
            $("#btnHaveReply").live("click", function () {//已经回复
                PageIndex = 1;
                FeedBackStatus = "1";
                Search();

            });
            $("#btnUnReply").live("click", function () {//未回复
                PageIndex = 1;
                FeedBackStatus = "0";
                Search();

            });

        });
        function Search() {
            try {
                //$("#progressBar").show();
                $.mobile.loading('show');
                jQuery.ajax({
                    type: "Post",
                    url: "/Handler/App/CationHandler.ashx",
                    data: { Action: "QueryJuMasterFeedBack", FeedBackStatus: FeedBackStatus, page:
PageIndex, rows: PageSize, justMe: '<%=justMe %>'
                    },
                    dataType: "html",
                    success: function (result) {
                        //$("#progressBar").hide();
                        $.mobile.loading('hide');
                        if (PageIndex == 1) {
                            $("#divfeedbacklist").html(result);
                        }
                        else {
                            if (result != "") {
                                $("#progressBar").before(result);

                            }
                            else {

                                $("#btnNext").attr("isshow", "no");
                                $("#btnNext").hide();
                                $("#progressBar").remove();
                            }


                        }


                    }
                })

            } catch (e) {
                alert(e);
            }

        }

    </script>
</asp:Content>
