$(function () {
    try {
        var selectTcType = getCookie('selectTcType');
        var selectIds = getCookie('selectIds');
        var tcName = '';
        switch (selectTcType) {
            case '1':
                tcName = '温馨套餐';
                break;
            case '2':
                tcName = '小康套餐';
                break;
            case '3':
                tcName = '美满套餐';
                break;
        }

        $('#txtTcName').val(tcName);

        $('#btnSubmit').click(function () {

            var model = {
                LoginName: 'dWxpZmU=',
                LoginPwd: 'B9#A#!E##BA59!5#01B4D#A45B7C1#E7',
                ActivityID: 125265,
                Name: $.trim($('#txtUserName').val()),
                Phone: $.trim($('#txtPhone').val()),
                K1: $.trim($('#txtAddress').val()),
                K2: tcName,
                K3: $('#selectQX').val(),
                K4: selectIds,
                DistinctKeys: 'none'
            }

            if (model.Name == '') {
                $('#lbDlgMsg').html('姓名不能为空!');
                $('#dlgMsg').popup('open');
                return;
            }
            if (model.Phone == '') {
                $('#lbDlgMsg').html('手机不能为空!');
                $('#dlgMsg').popup('open');
                return;
            }
            if (model.K1 == '') {
                $('#lbDlgMsg').html('送菜地址不能为空!');
                $('#dlgMsg').popup('open');
                return;
            }
            $.mobile.loading('show', { textVisible: true, text: '正在提交订单。。。' });
            $.ajax({
                type: 'post',
                url: '/Serv/ActivityApiJson.ashx',
                data: model,
                success: function (result) {
                    $.mobile.loading('hide');
                    var resp = $.parseJSON(result);
                    if (resp.Status == 0) {
                        $('#lbDlgMsg').html('提交成功!');
                        $('#dlgMsg').popup('open');
                        ClearAll();
                    }
                    else if (resp.Status == 5)
                    {
                        $('#lbDlgMsg').html('手机号码格式错误!');
                        $('#dlgMsg').popup('open');
                    }
                    else {
                        $('#lbDlgMsg').html('提交失败，请联系管理员!');
                        $('#dlgMsg').popup('open');
                    }
                }
            });


        });

    } catch (e) {
        alert(e);
    }
});