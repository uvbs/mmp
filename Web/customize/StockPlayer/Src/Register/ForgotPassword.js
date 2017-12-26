//注册页面
var codeInterval = null;
var codeIntervalNum = 60;
$(function () {
    //顶部切换
    $('[value=company]').click(function () {
        $(this).removeClass('btn btn-default btn-company');
        $(this).addClass('btn btn-default btn-user');
        $('[value=user]').removeClass('btn btn-default btn-user');
        $('[value=user]').addClass('btn btn-default btn-company');
        $('#company').show();
        $('#user').hide();
    })
    $('[value=user]').click(function () {
        $(this).removeClass('btn btn-default btn-company');
        $(this).addClass('btn btn-default btn-user');
        $('[value=company]').removeClass('btn btn-default btn-user');
        $('[value=company]').addClass('btn btn-default btn-company');
        $('#company').hide();
        $('#user').show();
    })

    //获取验证码
    tt.vf.req.addId('code_pwd', 'code_configpwd', 'code_phone');
    var pwd = new tt.Field('新密码', null, 'code_pwd');
    var configpwd = new tt.Field("确认密码", null, 'code_configpwd');
    new tt.CV().add(configpwd).set('v', "==", pwd, false);
    new tt.LV().set(6, '++').addId("code_pwd");
    new tt.RV().set(new RegExp("^1[0-9]{10}$"), "手机号码有误！").addId("code_phone");

    var wait = 60;
    $('#getcode').click(function () {
        if (!tt.validateId('code_pwd', 'code_configpwd', 'code_phone')) {  
            return false;
        }
        var phone = $('#code_phone').val();
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: '/serv/api/common/smsvercode.ashx',
            data: { phone: phone, smscontent: '{{SMSVERCODE}}',content:'1' },
            dataType: 'json',
            success: function (data) {
                layer.close(layerIndex);
                if (data.errcode == 0) {
                    alert('验证码已发送');
                    codeIntervalNum = 60;
                    $('#getcode').text('等待（' + codeIntervalNum + '）');
                    codeInterval = setInterval(function () {
                        codeIntervalNum--;
                        if (codeIntervalNum < 0) {
                            $('#getcode').text('获取验证码');
                            $('#getcode').removeAttr('disabled');
                            clearInterval(codeInterval);
                        } else {
                            $('#getcode').attr('disabled', 'disabled');
                            $('#getcode').text('等待（' + codeIntervalNum + '）');
                        }
                    }, 1000);
                } else {
                    alert(data.errmsg);
                }
            }
        });
    })

    //个人注册
    $('#RegisterUser').click(function () {
        tt.vf.req.addId('user_code');
        if (!tt.validateId('code_pwd', 'code_configpwd', 'code_phone', 'user_code')) {
            return false;
        }
        var userModel = {
            phone: $.trim($('#code_phone').val()),
            code: $.trim($('#user_code').val()),
            new_pwd: $.trim($('#code_pwd').val()),
            confirm_pwd: $.trim($('#code_configpwd').val()),
            pwd_length:'6'
        }
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: '/serv/api/user/SMSBackPwd.ashx',
            data: userModel,
            dataType: 'json',
            success: function (data) {
                layer.close(layerIndex);
                if (data.isSuccess) {
                    alert('密码找回成功');
                    setInterval(function () {
                        window.location.href = homePage;
                    },2000);

                } else {
                    alert(data.errmsg);
                }
            }
        });
    })
   

})