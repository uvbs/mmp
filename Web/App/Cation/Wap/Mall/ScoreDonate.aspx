<%@ Page Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true"  CodeBehind="ScoreDonate.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.ScoreDonate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">积分赠送</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .sendjifen{font-size:16px;padding:0 10px;margin-top:5px;}
    .sendjifen:first-child{margin-top:10px;}
    .jifenconcent{width:100px;}
    #txtUser{-moz-user-select: initial;
-webkit-user-select: initial;
-ms-user-select: initial;
-khtml-user-select: initial;
user-select: initial;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <section class="box" style="-moz-user-select: initial;
-webkit-user-select: initial;
-ms-user-select: initial;
-khtml-user-select: initial;
user-select: initial;">
    <div class="sendjifen">我的账号:<span class="jifenconcent" id="txtUser"><%=currentUserInfo.UserID %><span></div>
    <div class="sendjifen">我的积分:<span class="jifenconcent"></span><%=currentUserInfo.TotalScore%></span></div>
    <div class="donatebox">
      
        <label for="it1" class="donateidl">对方账号:</label>
        <input class="donateid"  type="text" id="txtTargetAccount">
        <label for="it2" class="donatenuml">赠送积分:</label>
        <input class="donatenum" pattern="\d*"  type="number" id="txtScore">
        <button class="btn orange" id="btnOk" type="button">确定赠送</button>
    </div>
    <div class="backbar">
        <a href="ScoreManage.aspx" class="back"><span class="icon"></span></a>
    </div>
</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var mallHandlerUrl = '/Handler/App/WXMallHandler.ashx';
    $(function () {

        $("#btnOk").click(function () {
            var tagetAccount = $("#txtTargetAccount").val();
            var score = $("#txtScore").val();
            if ($.trim(tagetAccount) == "") {
                $("#txtTargetAccount").focus();
                return false;
            }
            if ($.trim(score) == "") {
                $("#txtScore").focus();
                return false;
            }
            if (confirm("确定赠送积分?")) {
                $("#btnOk").html("正在赠送...");
                $.ajax({
                    type: 'post',
                    url: mallHandlerUrl,
                    data: { Action: 'GiveScoreToOtherAccount', TargetAccount: tagetAccount, Score: score },
                    timeout: 60000,
                    dataType:"json",
                    success: function (resp) {
                        $("#btnOk").html("确定赠送");
                        if (resp.Status == 1) {
                            //alert("操作成功!");
                            msgText.init("操作成功", 3000);
                            window.location = 'ScoreRecord.aspx';
                        }
                        else {
                            //alert(resp.Msg);
                            msgText.init(resp.Msg, 3000);

                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $("#btnOk").html("确定赠送");
                        if (textStatus == "timeout") {
                            //alert("操作超时，请重试");
                            msgText.init("操作超时，请重试", 3000);
                        }
                        else {
                            //alert(textStatus + " 请重试");
                            msgText.init(textStatus + " 请重试", 3000);

                        }
                    }
                });

            }

        });




    })

</script>
</asp:Content>

