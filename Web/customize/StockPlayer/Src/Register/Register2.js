//注册页面
var codeInterval = null;
var codeIntervalNum = 60;
$(function () {
    //加载用户协议
    GetDetail({ article_id: data.register.userArtcle }, false, function (resp) {
        data.register.userContent = resp.articel_context;
    });
   
   

    //个人注册
    tt.vf.req.add('user_nick', 'user_pwd', 'user_configpwd', 'user_phone');
    var pwd = new tt.Field('密码', null, 'code_pwd');
    var configpwd = new tt.Field("确认密码", null, 'code_configpwd');
    new tt.CV().add(configpwd).set('v', "==", pwd, false);
    new tt.LV().set(6, '++').addId("code_pwd");
    $('#RegisterUser').click(function () {
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
            is_repeat_nickname: '1',
            view_type: '1'
        }
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: '/serv/api/user/Register2.ashx',
            data: userModel,
            dataType: 'json',
            success: function (data) {
                layer.close(layerIndex);
                if (data.isSuccess) {
                    alert('注册成功');
                    $('#code_nick').val('');
                    $('#code_phone').val('');
                    $('#code_pwd').val('');
                    $('#code_configpwd').val('');
                    $('#ck').attr('checked',false);
                } else {
                    alert(data.errmsg);
                }
            }
        });
    })
    $('#user .article').click(function () {
        showDialogArticle('金融玩家协议——用户', data.register.userContent);
    })


    //登录
    $('#LoginUser').click(function () {
        if (!tt.validateForm('userLogin')) {   //userRegister是form的name  
            return false;
        }
        var username = $.trim($('#login_account1').val());
        var userpassword = $.trim($('#login_pwd1').val());
        if (username == '') return;
        if (userpassword == '') return;
        loginApi2(username, userpassword, function (resp) {
            alert('登录成功');
            $('#login_account1').val('');
            $('#login_pwd1').val('');
            setInterval(function () {
                ToCenter();
            }, 1000);
        });
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