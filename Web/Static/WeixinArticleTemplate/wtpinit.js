//var activity
//var activityData;
//var currOpenID = '';
//var activityFieldMapping;

var isLogin = 0;
var wxAuthPageMustLogin = 0;
$(document).ready(function () {
    window.alert = window.Alert = function (msg) {
        layer.open({
            content: msg,
            time: 2
        });
    };
    $.ajax({
        type: 'post',
        url: "/Serv/API/User/IsLogin.ashx",
        dataType: "json",
        success: function (resp) {
            if (resp.is_login) {
                isLogin = 1;
            }
            else {
                $('#divMasterContact').trigger('collapse');
                $('#divMasterContact').unbind().bind('expand', function () {
                    GotoLoginPage();
                });
            }
        }
    });
    $.ajax({
        type: 'post',
        url: "/Serv/API/Common/CheckWebsiteCommRelation.ashx",
        dataType: "json",
        data: { key: 'WXAuthPageMustLogin' },
        success: function (resp) {
            if (resp.status) {
                wxAuthPageMustLogin = 1;
            }
        }
    });
    
    $("#btnSignIn").live("click", function () {
        if (isLogin == 0 && wxAuthPageMustLogin == 1) {
            GotoLoginPage();
            return;
        }
        var Name = $("#txtName").val();
        var Phone = $("#txtPhone").val();
        if (Name == "" || (Phone == "")) {
            alert("请输入姓名、手机号码");
            return false;
        }
        try {
           // $.mobile.loading('show', { textVisible: true, text: '正在提交...' });
            var option = {
                url: "/serv/ActivityApiJson.ashx",
                type: "post",
                dataType: "json",
                success: function (resp) {
                    //App/Cation/Wap/ActivityLlists.aspx$.mobile.loading('hide');
                    if (resp.Status == 0) {
                        alert("提交成功");
                        return;
                        //清空
//                        $('input:text').val("");
//                        $('textarea').val("");
//                        //取得返回的Activity和ActivityData
//                        activity = resp.ExObj["Activity"];
//                        activityData = resp.ExObj["ActivityData"];
//                        activityFieldMapping = resp.ExObj["ActivityFieldMapping"];
//                        currOpenID = getCurrOpenID();
                        //                        alert("currOpenID:" + currOpenID);
                        //                        alert(currOpenID != '');
                        //                        alert(currOpenID != 'undefined');
                        //                        alert(currOpenID != '' && typeof(currOpenID) != 'undefined');



                        //如果有传入openID，判断是否会员，不是会员则提示注册，并将信息转提交到注册接口
//                        if (currOpenID != '') {
//                            $.ajax({
//                                type: 'post',
//                                url: '/Handler/WeiXin/WXRegistration.ashx',
//                                data: { Action: 'CheckIsWXMember', UserID: activity.UserID, wxOpenId: currOpenID },
//                                success: function (result) {
//                                    if (result == 'false') {

//                                        /*
//                                        var strMsg = new StringBuilder();
//                                        strMsg.AppendFormat('提交成功!<br /><span style="font-size:12px;">您可以立即注册成我们的会员，以后无需再填写个人信息</span><br /><a href="javascript:;" onclick="direSignInToRegWXMember(activity.UserID, currOpenID, activityData, activityFieldMapping)">点击立即注册！</a>');
//                                        $('#lbDlgMsg').html(strMsg.ToString());
//                                        $('#dlgMsg').popup();
//                                        $('#dlgMsg').popup('open');
//                                        */
//                                        //修改逻辑，不是会员直接注册

//                                        direSignInToRegWXMember(activity.UserID, currOpenID, activityData, activityFieldMapping);

//                                        return;
//                                    }
//                                    else {
//                                        $('#lbDlgMsg').html("提交成功!");
//                                        $('#dlgMsg').popup();
//                                        $('#dlgMsg').popup('open');
//                                        return;
//                                    }
//                                }
//                            });
//                        }
//                        else {
//                           
//                        }
                    }
                    else if (resp.Status == 1) {
                        //该用户已提交过数据
                        alert("该用户已提交过数据");
                    }
                    else {
                        alert(resp.Msg);
                    }

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

function GotoLoginPage() {
    alert("您还没有登录，请先登录");
    setTimeout(function () {
        document.location.href = appLoginUrl + '?redirect=' + encodeURI(document.location.href);
    }, 2000);
}

////获取当前的openID
//function getCurrOpenID() {
//    //根据浏览器链接参数获取 例如 /wxad/1f958/1f95d/o7eeXjkwa5gCCjLCGBx8NkI5C2IY/detail.chtml
//    var result = "";
//    var currPath = window.location.pathname;
//    var tmpArr = currPath.split('/');
//    if (tmpArr.length > 5) {
//        result = tmpArr[4];
//    }

//    if (result == '' && $('#WXCurrOpenerOpenID').length > 0) {
//        try {
//            result = $('#WXCurrOpenerOpenID').val();
//        } catch (e) {

//        }
//    }

//    return result;
//}


////转投提交信息到注册接口
//function direSignInToRegWXMember(userId, openId, data, mapping) {

//    //构造注册信息
//    var model = {
//        Name: data.Name,
//        Company: "",
//        Postion: "",
//        Phone: data.Phone,
//        Email: "",
//        UserID: userId,
//        Action: "WXReg",
//        WeixinOpenID: openId
//    }

//    for (var i = 0; i < mapping.length; i++) {
//        //alert(mapping[i]["ExFieldIndex"] + mapping[i]['MappingName']);

//        var mappingName = "_" + mapping[i]['MappingName'];

//        //判断邮箱、邮件
//        if (mappingName.indexOf('邮箱') > 0 || mappingName.indexOf('邮件') > 0 || mappingName.toLowerCase().indexOf('email') > 0) {
//            model.Email = data["K" + mapping[i]["ExFieldIndex"]];
//            //alert(model.Email);
//            continue;
//        }

//        //判断职位
//        if (mappingName.indexOf('职位') > 0) {
//            model.Postion = data["K" + mapping[i]["ExFieldIndex"]];
//            //alert(model.Postion);
//            continue;
//        }

//        //判断公司
//        if (mappingName.indexOf('公司') > 0) {
//            model.Company = data["K" + mapping[i]["ExFieldIndex"]];
//            //alert(model.Company);
//            continue;
//        }

//    }

//    $.ajax({
//        type: 'post',
//        url: '/Handler/WeiXin/WXRegistration.ashx',
//        data: model,
//        success: function (result) {
//            var resp = $.parseJSON(result);

//            if (resp.Status == 1) {
//                //$('#lbDlgMsg').html('提交成功!<br /><span style="font-size:12px;">您已经注册成我们的会员，以后无需再填写个人信息</span>');
//                $('#lbDlgMsg').html('提交成功!');
//                $('#dlgMsg').popup();
//                $('#dlgMsg').popup('open');
//            }
//            else {
//                $('#lbDlgMsg').html(resp.Msg);
//                $('#dlgMsg').popup();
//                $('#dlgMsg').popup('open');
//            }

//            //            $('#lbDlgMsg').html("恭喜您已成功注册!");
//            //            $('#dlgMsg').popup();
//            //            $('#dlgMsg').popup('open');

//        }
//    });

//}



