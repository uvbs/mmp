//注册页面
var codeInterval = null;
var codeIntervalNum = 60;
$(function () {
    //加载用户协议
    GetDetail({ article_id: data.register.userArtcle }, false, function (resp) {
        data.register.userContent = resp.articel_context;
    })
    //加载公司协议
    GetDetail({ article_id: data.register.companyArtcle }, false, function (resp) {
        data.register.companyContent = resp.articel_context;
    });

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
    $('#getcode').click(function () {
        if (!tt.validateForm('userRegister')) {   //userRegister是form的name  
            return false;
        }
        var phone = $('#code_phone').val();
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: '/serv/api/common/smsvercode.ashx',
            data: { phone: phone, smscontent: '您的注册验证码为{{SMSVERCODE}}', is_register: 1,content:'1' },
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
                            $('#getcode').text('等待（' + codeIntervalNum + '）');
                            $('#getcode').attr('disabled', 'disabled');
                        }
                    }, 1000);
                } else {
                    alert(data.errmsg);
                }
            }
        });
    });

    //个人注册
    tt.vf.req.add('user_nick', 'user_pwd', 'user_configpwd', 'user_phone');
    var pwd = new tt.Field('密码', null, 'code_pwd');
    var configpwd = new tt.Field("确认密码", null, 'code_configpwd');
    new tt.CV().add(configpwd).set('v', "==", pwd, false);
    new tt.LV().set(6, '++').addId("code_pwd");
    new tt.RV().set(new RegExp("^1[0-9]{10}$"), "手机号码有误！").addId("code_phone");
    $('#RegisterUser').click(function () {
        tt.vf.req.add('user_code');
        if (!tt.validateForm('userRegister')) {   //userRegister是form的name  
            return false;
        }
        var ck = $('#ck').get(0).checked;
        if (!ck) {
            alert('请阅读金融玩家协议');
            return;
        }
        var userModel = {
            nickname: $.trim($('#code_nick').val()),
            username: $.trim($('#code_phone').val()),
            password: $.trim($('#code_pwd').val()),
            passwordconfirm: $.trim($('#code_configpwd').val()),
            smsvercode: $.trim($('[name=user_code]').val()),
            is_repeat_nickname: '1',
            view_type: '1'
        }
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: '/serv/api/user/RegisterByPhone.ashx',
            data: userModel,
            dataType: 'json',
            success: function (data) {
                layer.close(layerIndex);
                if (data.isSuccess) {
                    alert('注册成功');
                    setInterval(function () {
                        ToCenter();
                    }, 2000);
                } else {
                    alert(data.errmsg);
                }
            }
        });
    })
    $('#user .article').click(function () {
        showDialogArticle('金融玩家协议——用户', data.register.userContent);
    })

    //公司注册
    tt.vf.req.addId('company_name', 'company_pwd', 'company_confirmpwd', 'img-lince');
    new tt.LV().set(6, '++').addId("company_pwd");
    var pwd = new tt.Field('密码', null, 'company_pwd');
    var confirmpwd = new tt.Field("确认密码", null, 'company_confirmpwd');
    new tt.CV().add(confirmpwd).set('v', "==", pwd, false);

    var url = '/serv/api/common/file.ashx?action=Add';
    $(document).on('change', '#licensePath', function () {
        var layerIndex = progress();
        $.ajaxFileUpload({
            url: url,
            secureuri: false,
            fileElementId: 'licensePath',
            dataType: 'json',
            success: function (result) {
                layer.close(layerIndex);
                if (result.errcode == 0) {
                    $('#img-lince').attr('src', result.file_url_list[0]);
                }
                else {
                    alert(result.errmsg);
                }
            }
        });
    });
    $('#RegisterComapny').click(function () {

        if (!tt.validateForm('companyRegister')) {   //companyRegister是form的name  
            return false;
        }
        var linceUrl = $('#img-lince').attr('src');
        if (linceUrl == 'http://static-files.socialcrmyun.com/customize/StockPlayer/Img/zhizhao.png') {
            alert('请上传营业执照');
            return;
        }
        var cb = $('#cb').get(0).checked;
        if (!cb) {
            alert('请选择金融玩家协议');
            return;
        }
        var companyModel = {
            company: $.trim($('#company_name').val()),
            pwd: $.trim($('#company_pwd').val()),
            confirm_pwd: $.trim($('#company_confirmpwd').val()),
            licence: linceUrl
        };
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: '/serv/api/user/RegisterByCompany.ashx',
            data: companyModel,
            dataType: 'json',
            success: function (data) {
                layer.close(layerIndex);
                if (data.status) {
                    alert(data.msg);
                    setInterval(function () {
                        ToCenter();
                    }, 1000)
                } else {
                    alert(data.msg);
                }
            }
        });
    })
    $('#company .article').click(function () {
        showDialogArticle('金融玩家协议——公司', data.register.companyContent);
    });
});
function showDialogArticle(title, content) {
    qrLayerIndex = layer.open({
        type: 1,
        title: title,
        skin: 'layui-layer-regarticle', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        area: ['970px', '600px'],
        content: content,
        btn: ['关闭'], //按钮
        yes: function (index, layero) {
            layer.close(index);
        }
    });
}