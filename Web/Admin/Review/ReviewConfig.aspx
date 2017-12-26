<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ReviewConfig.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Review.ReviewConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 40px;
        } 
        
        .sort
        {
            width: 780px;
        }
        .title
        {
            
         font-size:12px;   
         }
         


        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;话题&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>话题配置</span></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox00">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                             <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        话题配置：
                    </td>
                    <td>
                        <input type="radio" name="rdoIsEnableUserReleaseReview" id="rdoEnableUserReleaseReview" value="1" checked="checked" /><label
                            for="rdoEnableUserReleaseReview">&nbsp;用户可以发话题</label>
                        &nbsp;&nbsp;
                        <input type="radio" name="rdoIsEnableUserReleaseReview" id="rdoDisEnableUserReleaseReview" value="0" /><label
                            for="rdoDisEnableUserReleaseReview">&nbsp;用户不可以发话题</label>
                    </td>
                </tr>


            </table>
            <br />
              <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px; text-decoration: underline;"
                            class="button button-rounded button-primary">保存</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = "Handler/ReviewHandler.ashx";
        $(function () {
            var isEnableUserReleaseReview = "<%=currentWebsiteInfo.IsEnableUserReleaseReview %>";
            switch (isEnableUserReleaseReview) {
                case "0":
                    $("#rdoDisEnableUserReleaseReview").attr("checked", "checked");
                    break;
                case "1":
                    $("#rdoEnableUserReleaseReview").attr("checked", "checked");
                    break;
            }

            $('#btnSave').click(function () {
                try {
                    var model =
                    {
                        IsEnableUserReleaseReview: $(":radio[name=rdoIsEnableUserReleaseReview]:checked").val(),
                        Action: "UpdateReviewConfig"
                    };
                    $.messager.progress({ text: '正在保存...' });
                    $.ajax({
                        type: 'post',
                        url: handlerUrl,
                        data: model,
                        dataType: "json",
                        success: function (resp) {
                            $.messager.progress('close');
                            Alert(resp.Msg);

                        }
                    });

                } catch (e) {
                    Alert(e);
                }


            });
        })

    </script>
</asp:Content>
