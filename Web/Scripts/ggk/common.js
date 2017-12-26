$(document).ready(function () {
    $("#btnSignIn").live("click", function () {
        var Name = $("#txtName").val();
        var Content = $("#K4").val();
        if (Content == "") {
            $("#K4").focus();
            return false;
        }
        if (Name == "") {

            $("#txtName").focus();
            return false;
        }

        try {
            $("#btnSignIn").val("正在提交...");
            $('#btnSignIn').attr('disabled', "true");
            var option = {
                url: "/serv/ActivityApiJson.ashx",
                type: "post",
                dataType: "text",
                timeout: 30000,
                success: function (result) {
                    $('#btnSignIn').removeAttr("disabled");
                    var resp = $.parseJSON(result);
                    if (resp.Status == 0) {//清空

                        //$('input:text').val("");
                        //$('#K1').val("");
                        $("#btnSignIn").val("已回复");

                    }
                    else if (resp.Status == 1) {
                        $("#btnSignIn").val("已回复");
                    }
                    else {

                        $("#btnSignIn").val("回复失败");
                    }



                },
                error: function () {
                    //alert("网络超时，请重试");
                    alert(resp.Msg);
                    $('#btnSignIn').removeAttr("disabled");
                    $("#btnSignIn").val("回复");
                }


            };
            $("#formsignin").ajaxSubmit(option);
            return false;

        }
        catch (e) {
            alert(e);
        }



    });



});