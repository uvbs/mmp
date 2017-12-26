function updatePassword() {

    var password = $("#txtPwd").val();
    var passwordNew = $("#txtPwdNew").val();
    var passwordNewConfirm = $("#txtPwdNewConfirm").val();
    if ($.trim(password) == "") {
        zcAlert("请输入旧密码");
        return false;
    }
    if ($.trim(passwordNew) == "") {
        zcAlert("请输入新密码");
        return false;
    }
    if ($.trim(passwordNewConfirm) == "") {
        zcAlert("请确认新密码");
        return false;
    }
    if (passwordNew != passwordNewConfirm) {
        zcAlert("新密码不一致");
        return false;
    }
    zcConfirm('确认修改密码？', '确定', '关闭', function () {
        $.ajax({
            type: 'post',
            url: '/Serv/API/User/UpdateLoginPwd.ashx',
            data: {
                user_pwd: password,
                new_pwd: passwordNew,
                config_pwd: passwordNewConfirm
            },
            dataType: 'json',
            success: function (resp) {
                if (resp.errcode == 0) {
                    zcAlert('密码已修改', '', 3, function () {
                        login();
                    });
                }
                else {
                    zcAlert(resp.errmsg);
                }
            }
        });
    });



}

function login() {
    window.location.href = "LoginBinding.aspx";
}