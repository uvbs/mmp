<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXTutorProblem.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.WuBuHui.Tutor.WXTutorProblem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <title></title>
</head>
<body>
    <div>
        <input type="radio" value="0" checked="checked" name="RdPower" id="IsGk" /><label
            for="IsGk">公开</label>
        <input type="radio" value="1" name="RdPower" id="IsFgk" /><label for="IsFgk">不公开</label>
        标题<input type="text" name="name" id="txtTitle" value="" />
        描述<textarea id="txtReviewContent"></textarea>
        <input type="button" id="SubSave" onclick="SavaReviewInfo()" value="提交" id="btnSubmit" />
    </div>
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var userId = '<%=uinfo.UserID%>';
        var IsSumbit = false;
        $(function () {


        });
        function SavaReviewInfo() {
            var Power = $("input[name='RdPower']:checked").val();
            if (IsSumbit) {
                return false;
            }
            IsSumbit = true;
            $("#btnSubmit").val("正在提交...");
            $.ajax({
                type: 'post',
                url: "/Handler/App/WXWuBuHuiTutorHandler.ashx",
                data: { Action: "SaveReviewInfo", UserId: userId, Power: Power, Title: $.trim($("#txtTitle").val()), ReviewContent: $.trim($("#txtReviewContent").val()) },
                dataType:"json",
                success: function (resp) {
                    if (resp.Status == 1) {
                        alert(resp.Msg);
                    }
                    else {
                        alert(resp.Msg);
                    }
                },
                complete: function () {
                    IsSumbit = false;
                    $("#btnSubmit").val("提交");

                }
            });

        }
    </script>
</body>
</html>
