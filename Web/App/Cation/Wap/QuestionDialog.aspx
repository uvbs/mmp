<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMain.Master" AutoEventWireup="true" CodeBehind="QuestionDialog.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.QuestionDialog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div data-role="page" id="page-title" data-theme="b">
        <div data-role="header" data-theme="b" data-position="fixed" style="" id="divTop">
            <a href="#" data-role="button" data-rel="back" data-icon="arrow-l">返回</a>
            <h1>
                问题回复</h1>
        </div>
        <div id="divfeedbacklDialog" style="margin-left: 5px; margin: 5px;">
        </div>
        <div data-role="footer" data-theme="c" data-position="fixed" data-ajax="false">
            <table style="width: 100%">
                <tr>
                    <td style="width: 85%">
                        <input type="text" id="txtDiaContent" style="width: 100%;" placeholder="点击输入内容" />
                    </td>
                    <td style="width: 15%">
                        <a href="#" data-role="button" inline="true" data-theme="f" id="btnAddDia">发送</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {

            LoaduMasterFeedBackDialogueData();
            $("#btnAddDia").click(function () {
                var DialogueContent = $("#txtDiaContent").val();
                if (DialogueContent == "") {
                    return false;
                }
                // alert(DiaContent);

                $.mobile.loading('show');
                jQuery.ajax({
                    type: "Post",
                    url: "/Handler/App/CationHandler.ashx",
                    data: { Action: "AddJuMasterFeedBackDialog", FeedBackID: "<%=FeedBackID %>", DialogueContent: DialogueContent },
                    dataType: "html",
                    success: function (result) {
                        $.mobile.loading('hide');
                        var resp = $.parseJSON(result);
                        if (resp.Status == 1) {//成功
                            LoaduMasterFeedBackDialogueData();
                            $("#txtDiaContent").val("");

                        }
                        else {//失败

                            alert(resp.Message);
                        }




                    }
                })




            });

        });
        //加载对话框信息
        function LoaduMasterFeedBackDialogueData() {
            try {

                $.mobile.loading('show');
                jQuery.ajax({
                    type: "Post",
                    url: "/Handler/App/CationHandler.ashx",
                    data: { Action: "QueryJuMasterFeedBackDialogue", FeedBackID: "<%=FeedBackID %>" },
                    dataType: "html",
                    success: function (result) {

                        $.mobile.loading('hide');

                        $("#divfeedbacklDialog").html(result);



                    }
                })

            } catch (e) {
                alert(e);
            }

        }
    </script>
</asp:Content>
