data.checkInterval = -1;
data.orderId = -1;
$(function () {
    $(document).on('click', '.warp-tj span', function () {
        var num = $(this).text();
        var r_num = parseFloat(num) * rc_num / 100;
        $('.warp-money').text(r_num);
        $('.recharge-num').val(num);
        $('.warp-tj .selected').removeClass('selected');
        $(this).addClass('selected');
    });
    $(document).on('change', '.recharge-num', function () {
        var num = $(this).val();
        var r_num = parseFloat(num) * rc_num / 100;
        $('.warp-money').text(r_num);
        $('.warp-tj .selected').removeClass('selected');
    });
    $(document).on('click', '.recharge .btn-pay', function () {
        var num = $.trim($('.recharge-num').val());
        if (num == "") {
            alert('请输入充值数额');
            return;
        }
        qrCodeRecharge(num);
    });
});

//二维码支付充值
function qrCodeRecharge(score) {
    var html = new StringBuilder();
    html.AppendFormat('<div class="dlg-qrcode">');
    html.AppendFormat('<div class="qrcode-tip">正在生成<br />二维码</div>');
    html.AppendFormat('<div class="qrcode-img">');
    html.AppendFormat('<img />');
    html.AppendFormat('</div>');
    html.AppendFormat('</div>');
    qrLayerIndex = layer.open({
        type: 1,
        title: '二维码充值',
        skin: 'layui-layer-qrcode', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        area: ['260px', '360px'],
        content: html.ToString(),
        btn: ['关闭'], //按钮
        yes: function (index, layero) {
            layer.close(index);
            clearInterval(data.checkInterval);
        }
    });
    $.ajax({
        type: 'post',
        url: '/Serv/API/Score/Recharge.ashx',
        data: { score: score },
        dataType: 'JSON',
        success: function (resp) {
            if (resp.status) {
                data.orderId = resp.result.orderId;
                $('.layui-layer-qrcode .qrcode-img img').attr('src', '/Handler/ImgHandler.ashx?v=' + encodeURIComponent(resp.result.code_url));
                $('.layui-layer-qrcode .qrcode-tip').hide();
                $('.layui-layer-qrcode .qrcode-img').show();
                data.checkInterval = setInterval(function () {
                    checkRecharge();
                }, 3000);
            } else {
                //alert(resp.msg);
                $('.layui-layer-qrcode .qrcode-tip').text('生成二维码出错：' + resp.msg);
            }
        }
    });
}
function checkRecharge() {
    $.ajax({
        type: 'post',
        url: '/Serv/API/Score/CheckIsRecharge.ashx',
        data: { id: data.orderId },
        dataType: 'JSON',
        success: function (resp) {
            if (resp.status) {
                $('.layui-layer-qrcode .qrcode-img').hide();
                $('.layui-layer-qrcode .qrcode-tip').show();
                $('.layui-layer-qrcode .qrcode-tip').text('支付成功');
                clearInterval(data.checkInterval);
            }
        }
    });
}