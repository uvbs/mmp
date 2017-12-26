var sStr = "0";
var Select = "";
var AutoId = "";
//$("#btnSave").click(function () {
//    Select = $("#SelectStr").val();
//    AutoId = $("#AutoId").val();
//    if (Select == "radio") {
//        sStr = $('input:radio:checked').attr("v")
//    } else if (Select == "checkbox") {
//        sStr = "0";
//        $('input:checkbox:checked').each(function () {
//            sStr += "," + $(this).attr("v")
//        });
//    }
//    SaveInfo();
//});
////投票
function SaveInfo() {
    Select = $("#SelectStr").val();
    AutoId = $("#AutoId").val();
    if (Select == "radio") {
        sStr = $('input:radio:checked').attr("v")
    } else if (Select == "checkbox") {
        sStr = "0";
        $('input:checkbox:checked').each(function () {
            sStr += "," + $(this).attr("v")
        });
    }
    if (sStr == "0") {
        layermsg("请至少选择一个选项");
        return;
    }
    $.ajax({
        type: 'post',
        url: "/Handler/App/WXTheVoteInfoHandler.ashx",
        data: { Action: 'SaveDataInfo', AutoId: AutoId, Ids: sStr },
        timeout: 60000,
        success: function (result) {
            var resp = $.parseJSON(result);
            if (resp.Status == 0) {
                layermsg("投票成功");
                //location.reload();
            }
            else {
                layermsg(resp.Msg);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (textStatus == "timeout") {
                layermsg("加载超时，请刷新页面");
            }
        }
    });
}
